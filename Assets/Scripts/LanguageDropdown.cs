using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageDropdown : MonoBehaviour
{
    [SerializeField] private Dropdown languageDropdown; // Ссылка на Dropdown
    private string[] languages = { "English", "Русский" }; // Доступные языки

    void Start()
    {
        SetupDropdown();
        LoadSavedLanguage();
    }

    private void SetupDropdown()
    {
        // Очищаем текущие опции
        languageDropdown.ClearOptions();

        // Добавляем доступные языки в Dropdown
        foreach (string language in languages)
        {
            languageDropdown.options.Add(new Dropdown.OptionData(language));
        }

        // Подписываемся на изменение выбора
        languageDropdown.onValueChanged.AddListener(OnLanguageSelected);
    }

    private void LoadSavedLanguage()
    {
        // Загружаем индекс выбранного языка из PlayerPrefs (по умолчанию - 0)
        int savedLanguageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);

        // Устанавливаем сохранённый язык, если он доступен
        if (savedLanguageIndex >= 0 && savedLanguageIndex < languages.Length)
        {
            languageDropdown.value = savedLanguageIndex;
            SetLanguage(savedLanguageIndex);
        }
    }

    private void OnLanguageSelected(int index)
    {
        // Сохраняем выбранный язык
        PlayerPrefs.SetInt("SelectedLanguage", index);
        PlayerPrefs.Save();

        // Устанавливаем язык
        SetLanguage(index);
    }

    private void SetLanguage(int index)
    {
        // Получаем локаль на основе выбранного языка
        Locale selectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        LocalizationSettings.SelectedLocale = selectedLocale;

        Debug.Log($"Язык установлен на: {languages[index]}");
    }
}
