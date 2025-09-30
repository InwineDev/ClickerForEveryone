using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationSystem : MonoBehaviour
{
    public enum Language
    {
        English,
        Russian
    }

    public static Language language = Language.Russian;
    private static Dictionary<string, string> localisedEN;
    private static Dictionary<string, string> localisedRU;
    public static bool isInit;

    private const string LanguagePrefKey = "SelectedLanguage";

    public static void Init()
    {
        language = (Language)PlayerPrefs.GetInt(LanguagePrefKey, (int)Language.Russian);
        
        CSVLoader csvLoader = new CSVLoader();
        csvLoader.LoadCSV();
        localisedEN = csvLoader.GetDictionaryValues("en");
        localisedRU = csvLoader.GetDictionaryValues("ru");
        isInit = true;
    }

    public static void SetLanguage(Language newLanguage)
    {
        language = newLanguage;
        PlayerPrefs.SetInt(LanguagePrefKey, (int)newLanguage);
        PlayerPrefs.Save();
    }

    public static string GetLocalisedValue(string key)
    {
        if (!isInit) { Init(); }
        string value = key;
        switch (language)
        {
            case Language.English:
                localisedEN.TryGetValue(key, out value);
                break;
            case Language.Russian:
                localisedRU.TryGetValue(key, out value);
                break;
        }
        return value;
    }
}