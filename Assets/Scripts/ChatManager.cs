using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ChatManager : NetworkBehaviour
{
    [SerializeField] private InputField inputField;  // Поле для ввода сообщений
    [SerializeField] private Text chatText;          // Текстовый объект для вывода чата
    [SerializeField] private ScrollRect scrollRect;  // ScrollRect для прокрутки чата

    void Start()
    {
        // Присоединение функции отправки к кнопке
        Button sendButton = inputField.transform.parent.GetComponentInChildren<Button>();
        if (sendButton != null)
        {
            sendButton.onClick.AddListener(OnSendMessage);
        }
    }

    // Локальная функция для отправки сообщения на сервер
    public void OnSendMessage()
    {
        if (!string.IsNullOrWhiteSpace(inputField.text))
        {
            // Вызываем команду на сервере
            CmdSendMessage(inputField.text);
            inputField.text = "";  // Очищаем поле ввода
        }
    }

    // Команда отправки сообщения на сервер
    [Command(requiresAuthority = false)]
    private void CmdSendMessage(string message)
    {
        RpcReceiveMessage(message);  // Рассылаем сообщение всем клиентам
    }

    // RPC для получения сообщения на всех клиентах
    [ClientRpc]
    private void RpcReceiveMessage(string message)
    {
        chatText.text += $"{message}\n";


    }
}
