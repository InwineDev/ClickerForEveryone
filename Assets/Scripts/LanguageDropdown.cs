using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageDropdown : MonoBehaviour
{
    [SerializeField] private Dropdown languageDropdown;
    private string[] languages = { "English", "Русский" };

    void Start()
    {
        SetupDropdown();
        LoadSavedLanguage();
    }

    private void SetupDropdown()
    {
        languageDropdown.ClearOptions();
        foreach (string language in languages)
        {
            languageDropdown.options.Add(new Dropdown.OptionData(language));
        }
        languageDropdown.onValueChanged.AddListener(OnLanguageSelected);
    }

    private void LoadSavedLanguage()
    {
        int savedLanguageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);
        if (savedLanguageIndex >= 0 && savedLanguageIndex < languages.Length)
        {
            languageDropdown.value = savedLanguageIndex;
            SetLanguage(savedLanguageIndex);
        }
    }

    private void OnLanguageSelected(int index)
    {
        PlayerPrefs.SetInt("SelectedLanguage", index);
        PlayerPrefs.Save();
        SetLanguage(index);
    }

    private void SetLanguage(int index)
    {
        Locale selectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        LocalizationSettings.SelectedLocale = selectedLocale;
    }
}
