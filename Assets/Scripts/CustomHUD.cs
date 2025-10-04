using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomHUD : MonoBehaviour
{
    public Button buttonClient;
    public TMP_InputField inputFieldAddress;
    public TMP_InputField inputFieldPort;

    private void Start()
    {
        buttonClient.onClick.AddListener(Client);
    }

    public void Client()
    {
        NetworkManager.singleton.networkAddress = string.IsNullOrEmpty(inputFieldAddress.text) 
            ? "localhost" 
            : inputFieldAddress.text;
        
        Port();
        NetworkManager.singleton.StartClient();
    }

    private void Port()
    {
        if (ushort.TryParse(inputFieldPort.text, out ushort port))
        {
            TelepathyTransport transport = NetworkManager.singleton.GetComponent<TelepathyTransport>();
        }
    }

    private void OnDestroy()
    {
        buttonClient.onClick.RemoveListener(Client);
    }
}