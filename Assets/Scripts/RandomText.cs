using UnityEngine;
using UnityEngine.UI; // Для использования компонента Text
using TMPro; // Для использования TextMeshPro (если нужно)

public class RandomText : MonoBehaviour
{
    [Header("Настройки текста")]
    public string[] textArray; // Массив с текстовыми элементами
    public Text uiText; // Компонент Text для отображения текста (UI)
    public TextMeshProUGUI textMeshPro; // Компонент TextMeshPro для отображения текста (UI)
    public TextMesh textMeshWorld; // Компонент TextMesh для отображения текста в мире (3D)

    void Start()
    {
        // Проверяем, есть ли элементы в массиве
        if (textArray == null || textArray.Length == 0)
        {
            Debug.LogError("Массив textArray пуст или не назначен!");
            return;
        }

        // Выбираем случайный текст из массива
        string randomText = GetRandomText();

        // Отображаем текст в выбранном компоненте
        if (uiText != null)
        {
            uiText.text = randomText; // Для стандартного UI Text
        }
        else if (textMeshPro != null)
        {
            textMeshPro.text = randomText; // Для TextMeshPro (UI)
        }
        else if (textMeshWorld != null)
        {
            textMeshWorld.text = randomText; // Для TextMesh (3D)
        }
        else
        {
            Debug.LogError("Не назначен компонент для отображения текста!");
        }
    }

    // Метод для выбора случайного текста из массива
    private string GetRandomText()
    {
        int randomIndex = Random.Range(0, textArray.Length); // Случайный индекс
        return textArray[randomIndex]; // Возвращаем случайный текст
    }
}