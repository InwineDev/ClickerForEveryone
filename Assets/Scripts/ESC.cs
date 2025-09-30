using UnityEngine;
using UnityEngine.UI; // Для работы с UI, включая Button

public class ESC : MonoBehaviour
{
    public Canvas canvas; // Ссылка на Canvas, который нужно открыть/закрыть
    public Button openCanvasButton; // Ссылка на кнопку, по нажатию которой откроется Canvas
    private bool isCanvasActive; // Переменная состояния активности Canvas

    void Start()
    {
        // Проверка текущего состояния Canvas
        if (canvas != null)
        {
            isCanvasActive = canvas.gameObject.activeSelf;
        }

        // Привязка функции ToggleCanvas к кнопке
        if (openCanvasButton != null)
        {
            openCanvasButton.onClick.AddListener(ToggleCanvas);
        }
    }

    void Update()
    {
        // Проверка нажатия клавиши "Esc"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCanvas();
        }
    }

    // Функция для переключения состояния Canvas
    void ToggleCanvas()
    {
        if (canvas != null)
        {
            // Переключение состояния Canvas
            isCanvasActive = !isCanvasActive;
            canvas.gameObject.SetActive(isCanvasActive);
        }
    }
}
