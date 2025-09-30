using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class LeaveButton : MonoBehaviour
{
    public void Leave()
    {
        if (NetworkServer.active)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
    }
}
