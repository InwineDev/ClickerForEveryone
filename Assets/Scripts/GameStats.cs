using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameStats : NetworkBehaviour
{
    [Header("UI Elements")]
    public Text pingText;           // Для отображения пинга
    public Text fpsText;            // Для отображения FPS
    public Text playerCountText;    // Для отображения количества игроков
    public Text sessionTimeText;    // Для отображения времени сессии

    private int frameCount = 0;
    private float deltaTime = 0.0f;
    private float sessionStartTime;

    // Для синхронизации количества игроков
    [SyncVar] private int syncedPlayerCount = 0;

    void Start()
    {
        sessionStartTime = Time.time;

        if (pingText == null || fpsText == null || playerCountText == null || sessionTimeText == null)
        {
            Debug.LogError("UI Text elements not assigned in the Inspector!");
        }
    }

    void Update()
    {
        UpdatePing();
        UpdateFPS();
        UpdatePlayerCount();
        UpdateSessionTime();
    }

    void UpdatePing()
    {
        if (NetworkClient.isConnected)
        {
            var latency = NetworkTime.rtt * 1000f; // В миллисекундах
            pingText.text = $"Ping: {latency:F1} ms";
        }
        else
        {
            pingText.text = "Ping: N/A";
        }
    }

    void UpdateFPS()
    {
        frameCount++;
        deltaTime += Time.unscaledDeltaTime;

        if (deltaTime >= 1.0f)
        {
            int fps = Mathf.RoundToInt(frameCount / deltaTime);
            fpsText.text = $"FPS: {fps}";
            frameCount = 0;
            deltaTime = 0.0f;
        }
    }

    void UpdatePlayerCount()
    {
        if (isServer)
        {
            // Сервер обновляет количество игроков
            syncedPlayerCount = NetworkServer.connections.Count;
        }

        // Клиенты получают количество игроков через синхронизацию
        playerCountText.text = $"Players: {syncedPlayerCount}";
    }

    void UpdateSessionTime()
    {
        float elapsedTime = Time.time - sessionStartTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        sessionTimeText.text = $"Session Time: {minutes:D2}:{seconds:D2}";
    }
}
