using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZXing;

public class QRScanner : MonoBehaviour
{
    [Header("Camera")]
    public RawImage cameraView;

    [Header("UI")]
    public GameObject darkBackground;
    public GameObject infoPanel;
    public Image itemImage;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    [Header("Sprites")]
    public Sprite bookSprite;
    public Sprite idCardSprite;
    public Sprite penSprite;

    private WebCamTexture webcamTexture;
    private BarcodeReader barcodeReader;
    private string lastResult = "";

    void Start()
    {
        webcamTexture = new WebCamTexture();

        cameraView.texture = webcamTexture;
        cameraView.material.mainTexture = webcamTexture;

        webcamTexture.Play();

        barcodeReader = new BarcodeReader();

        if (darkBackground != null)
            darkBackground.SetActive(false);

        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    void Update()
    {
        cameraView.rectTransform.localEulerAngles =
    new Vector3(0, 0, -webcamTexture.videoRotationAngle);
        if (webcamTexture == null || !webcamTexture.isPlaying)
            return;

        if (webcamTexture.width < 100)
            return;

        try
        {
            var result = barcodeReader.Decode(
                webcamTexture.GetPixels32(),
                webcamTexture.width,
                webcamTexture.height
            );

            if (result != null && result.Text != lastResult)
            {
                lastResult = result.Text.Trim().ToUpper();

                ShowItem(lastResult);
            }
        }
        catch
        {
            // Ignore decode errors
        }
    }

    void ShowItem(string qr)
    {
        webcamTexture.Stop();
        cameraView.gameObject.SetActive(false);

        darkBackground.SetActive(true);
        infoPanel.SetActive(true);

        switch (qr)
        {
            case "BOOK":
                titleText.text = "Book";
                descriptionText.text = "A book is used for reading, learning and gaining knowledge.";
                itemImage.sprite = bookSprite;
                break;

            case "ID_CARD":
                titleText.text = "ID Card";
                descriptionText.text = "An ID card is used to identify a person and provide official information.";
                itemImage.sprite = idCardSprite;
                break;

            case "PEN":
                titleText.text = "Pen";
                descriptionText.text = "A pen is used for writing, drawing and taking notes.";
                itemImage.sprite = penSprite;
                break;

            default:
                titleText.text = "Unknown QR";
                descriptionText.text = qr;
                itemImage.sprite = null;
                break;
        }
    }

    public void ClosePanel()
    {
        infoPanel.SetActive(false);
        darkBackground.SetActive(false);

        cameraView.gameObject.SetActive(true);

        webcamTexture.Play();

        lastResult = "";
    }
}