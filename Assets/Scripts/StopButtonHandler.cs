using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class StopButtonHandler : MonoBehaviour
{
    public Button stopButton; // ������, ������� ����� �������� ���������

    void Start()
    {
        // ����������� ����� � ������
        stopButton.onClick.AddListener(OnStopButtonClicked);
    }

    // �����, ���������� ��� ������� ������
    private void OnStopButtonClicked()
    {
        // ���������, �������� �� ������� ������ ������
        if (NetworkServer.active)
        {
            // ���� ����, ������������� ����
            NetworkManager.singleton.StopHost();
            Debug.Log("Host stopped.");
        }
        else if (NetworkClient.isConnected)
        {
            // ���� ������, ����������� �� �������
            NetworkManager.singleton.StopClient();
            Debug.Log("Client disconnected.");
        }
    }
}
