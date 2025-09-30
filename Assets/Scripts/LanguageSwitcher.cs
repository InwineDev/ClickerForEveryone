using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LanguageSwitcher : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string> { "English", "Русский" });
        dropdown.value = (int)LocalizationSystem.language;
        dropdown.RefreshShownValue();
        dropdown.onValueChanged.AddListener(OnLanguageSelected);
    }

    private void OnLanguageSelected(int index)
    {
        LocalizationSystem.SetLanguage((LocalizationSystem.Language)index);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}