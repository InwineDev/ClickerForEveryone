using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    // Поля для выбора кнопок и канваса в инспекторе
    public GameObject canvasObject;  // Ссылка на сам Canvas
    public Button openButton;        // Кнопка активации (включения) Canvas
    public Button closeButton;       // Кнопка деактивации (выключения) Canvas

    void Start()
    {
        // Привязываем функции к кнопкам
        openButton.onClick.AddListener(ActivateCanvas);  // Привязываем кнопку активации
        closeButton.onClick.AddListener(DeactivateCanvas); // Привязываем кнопку деактивации

        // Убедимся, что Canvas изначально выключен
        canvasObject.SetActive(false);
    }

    // Функция для активации (включения) Canvas
    public void ActivateCanvas()
    {
        canvasObject.SetActive(true);
    }

    // Функция для деактивации (выключения) Canvas
    public void DeactivateCanvas()
    {
        canvasObject.SetActive(false);
    }
}
