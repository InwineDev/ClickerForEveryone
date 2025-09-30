using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public GameObject canvasObject;
    public Button openButton;
    public Button closeButton;

    void Start()
    {
        openButton.onClick.AddListener(ActivateCanvas);
        closeButton.onClick.AddListener(DeactivateCanvas);
        canvasObject.SetActive(false);
    }

    public void ActivateCanvas()
    {
        canvasObject.SetActive(true);
    }

    public void DeactivateCanvas()
    {
        canvasObject.SetActive(false);
    }
}
