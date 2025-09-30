using UnityEngine;
using UnityEngine.UI;

public class PlayerNickname : MonoBehaviour
{
    public InputField nicknameInput; // Поле для ввода ника
    public Button applyButton;       // Кнопка "Применить"

    void Start()
    {
        // Загрузка ника из PlayerPrefs (если он был сохранен)
        if (PlayerPrefs.HasKey("PlayerNickname"))
        {
            nicknameInput.text = PlayerPrefs.GetString("PlayerNickname");
        }

        applyButton.onClick.AddListener(SaveNickname);
    }

    void SaveNickname()
    {
        string nickname = nicknameInput.text;

        if (!string.IsNullOrEmpty(nickname))
        {
            // Сохраняем ник в PlayerPrefs
            PlayerPrefs.SetString("PlayerNickname", nickname);
            PlayerPrefs.Save();
            Debug.Log("Ник сохранен: " + nickname);
        }
        else
        {
            Debug.LogWarning("Пустой ник! Введите ник.");
        }
    }
}
