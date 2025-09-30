using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNicknameDisplay : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNicknameChanged))]
    public string playerNickname = "Player"; // Синхронизируемая переменная для ника игрока

    public Text nicknameText;     // UI элемент для отображения ника над игроком
    public Transform playerTransform; // Трансформ игрока для позиционирования ника
    public Vector3 offset = new Vector3(0, 2, 0); // Смещение текста относительно игрока

    // Метод вызывается только у локального игрока
    public override void OnStartLocalPlayer()
    {
        // Загружаем ник из PlayerPrefs
        string savedNickname = PlayerPrefs.GetString("PlayerNickname", "Player");

        Debug.Log("Загруженный ник для локального игрока: " + savedNickname);

        // Отправляем ник на сервер, чтобы синхронизировать его
        CmdSetNickname(savedNickname);
    }

    // Команда, отправляющая ник на сервер
    [Command]
    void CmdSetNickname(string nickname)
    {
        Debug.Log("Команда установки ника: " + nickname);

        // Синхронизируем ник на всех клиентах
        playerNickname = nickname;
    }

    // Этот метод вызывается, когда ник изменяется (синхронизированный ник)
    void OnNicknameChanged(string oldNickname, string newNickname)
    {
        Debug.Log("Изменение ника с " + oldNickname + " на " + newNickname);

        nicknameText.text = newNickname; // Обновляем UI с ником
    }

    void Update()
    {
        // Позиционируем текст над игроком
        if (nicknameText != null)
        {
            nicknameText.transform.position = Camera.main.WorldToScreenPoint(playerTransform.position + offset);
        }
    }
}
