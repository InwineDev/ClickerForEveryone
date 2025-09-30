using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class CustomHUD : MonoBehaviour
{
    public Button buttonHost;
    public Button buttonClient;
    public InputField inputFieldAddress;
    public InputField inputFieldPort;

    public void Start()
    {
        buttonHost.onClick.AddListener(ButtonHost);
        buttonClient.onClick.AddListener(ButtonClient);
    }

    public void ButtonHost()
    {
        NetworkManager.singleton.StartHost();
    }

    public void ButtonClient()
    {
        // Установка IP-адреса и порта из полей ввода
        NetworkManager.singleton.networkAddress = inputFieldAddress.text;

        // Установка порта, если введено корректное значение
        ushort port = 7777; // Стандартный порт по умолчанию
        if (ushort.TryParse(inputFieldPort.text, out ushort parsedPort))
        {
            port = parsedPort;
        }
        
        TelepathyTransport transport = NetworkManager.singleton.GetComponent<TelepathyTransport>();
        if (transport != null)
        {
            transport.port = port;
        }
        else
        {
            Debug.LogError("Transport component not found! Make sure TelepathyTransport is attached to the NetworkManager.");
        }

        NetworkManager.singleton.StartClient();
    }
}
