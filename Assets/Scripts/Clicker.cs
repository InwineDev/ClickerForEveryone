using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Clicker : NetworkBehaviour
{
    public Text scoreText;          // Текст для отображения счёта
    public InputField inputField;   // Поле для ввода нового значения
    public Button applyButton;      // Кнопка для применения нового значения

    [SyncVar(hook = nameof(OnScoreChanged))]
    public int score;               // Переменная для синхронизации счёта

    void Start()
    {
        UpdateScoreText();

        // Проверка, чтобы ссылки на UI-элементы были установлены
        if (applyButton != null && inputField != null)
        {
            // Привязка функции к кнопке "Применить"
            applyButton.onClick.AddListener(OnApplyButtonClicked);
        }
        else
        {
            Debug.LogError("Не установлены ссылки на InputField или ApplyButton в инспекторе.");
        }
    }

    // Функция для обработки нажатия на кнопку "Применить"
    private void OnApplyButtonClicked()
    {
        if (int.TryParse(inputField.text, out int newScore))  // Проверка, что введённое значение - число
        {
            CmdSetClicks(newScore);  // Отправляем команду на сервер для установки нового значения
        }
        else
        {
            Debug.LogWarning("Введите корректное числовое значение.");
        }
    }

    public void OnClick()
    {
        CmdIncreaseScore();  // Увеличение счёта при нажатии кнопки клика
    }

    [Command(requiresAuthority = false)]
    private void CmdIncreaseScore()
    {
        score++;  // Увеличиваем значение счёта
    }

    [Command(requiresAuthority = false)]  // Команда для обновления значения score
    public void CmdSetClicks(int newScore)
    {
        score = newScore;  // Устанавливаем новое значение счёта
    }

    // Обновляем текст счёта
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    // Функция, вызываемая при изменении счёта
    private void OnScoreChanged(int oldScore, int newScore)
    {
        UpdateScoreText();  // Обновляем текст при изменении счёта
    }

    public override void OnStartServer()
    {
        score = 0;  // Инициализация счёта на сервере
    }
}
