using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class StopButtonHandler : MonoBehaviour
{
    public Button stopButton; // Кнопка, которая будет вызывать остановку

    void Start()
    {
        // Привязываем метод к кнопке
        stopButton.onClick.AddListener(OnStopButtonClicked);
    }

    // Метод, вызываемый при нажатии кнопки
    private void OnStopButtonClicked()
    {
        // Проверяем, является ли текущий клиент хостом
        if (NetworkServer.active)
        {
            // Если хост, останавливаем хост
            NetworkManager.singleton.StopHost();
            Debug.Log("Host stopped.");
        }
        else if (NetworkClient.isConnected)
        {
            // Если клиент, отключаемся от сервера
            NetworkManager.singleton.StopClient();
            Debug.Log("Client disconnected.");
        }
    }
}
