using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayersCounter : NetworkBehaviour
{
    public Text onlinePlayersText; // Ссылка на текст для отображения количества игроков

    [SyncVar(hook = nameof(OnPlayerCountChanged))]
    public int onlinePlayers; // Количество игроков онлайн, синхронизируемое между клиентами

    // Метод для обновления текста количества игроков онлайн
    private void UpdateOnlinePlayersText()
    {
        onlinePlayersText.text = "Online Players: " + onlinePlayers.ToString();
    }

    // Хук, вызываемый при изменении SyncVar для количества игроков
    private void OnPlayerCountChanged(int oldCount, int newCount)
    {
        UpdateOnlinePlayersText(); // Обновляем текст
    }

    // Увеличение количества игроков при подключении
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isServer)
        {
            onlinePlayers++; // Увеличиваем количество игроков онлайн на сервере
        }
    }

    // Уменьшение количества игроков при отключении
    public override void OnStopClient()
    {
        base.OnStopClient();
        if (isServer)
        {
            onlinePlayers--; // Уменьшаем количество игроков онлайн на сервере
        }
    }

    // Метод, вызываемый при старте сервера
    public override void OnStartServer()
    {
        onlinePlayers = 0; // Обнуляем количество игроков при старте сервера
    }
}
