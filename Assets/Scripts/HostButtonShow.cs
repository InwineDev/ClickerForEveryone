using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HostButtonController : NetworkBehaviour
{
    public Button openButton; // Ссылка на вашу кнопку

    void Start()
    {
        // Проверяем, являемся ли мы сервером
        if (isServer)
        {
            openButton.gameObject.SetActive(true); // Показываем кнопку для хоста
            openButton.onClick.AddListener(OnOpenButtonClicked); // Добавляем обработчик клика
        }
        else
        {
            openButton.gameObject.SetActive(false); // Скрываем кнопку для всех остальных
        }
    }

    void OnOpenButtonClicked()
    {
        // Ваш код для обработки нажатия кнопки
        Debug.Log("Кнопка открытия нажата!");
    }
}
