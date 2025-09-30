using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class EdgegapManager : MonoBehaviour
{
    private const string API_URL = "https://api.edgegap.com/v1/sessions";
    private const string API_KEY = "your_api_key";

    public string roomCode;
    public string sessionUrl;

    public void CreateSession()
    {
        StartCoroutine(CreateEdgegapSession());
    }

    private IEnumerator CreateEdgegapSession()
    {
        roomCode = GenerateRoomCode();
        string requestData = JsonUtility.ToJson(new
        {
            name = "Room_" + roomCode,
            region = "auto",
            template_id = "your_template_id"
        });

        using UnityWebRequest request = new UnityWebRequest(API_URL, "POST");
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(requestData));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {API_KEY}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<EdgegapResponse>(request.downloadHandler.text);
            sessionUrl = response.session_url;
            Debug.Log($"Session created: {sessionUrl}");
        }
        else
        {
            Debug.LogError($"Failed to create session: {request.error}");
        }
    }

    private string GenerateRoomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] buffer = new char[6];
        for (int i = 0; i < buffer.Length; i++)
            buffer[i] = chars[Random.Range(0, chars.Length)];
        return new string(buffer);
    }

    [System.Serializable]
    private class EdgegapResponse
    {
        public string session_url;
    }
}
