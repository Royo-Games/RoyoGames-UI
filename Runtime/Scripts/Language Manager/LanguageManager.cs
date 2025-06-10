using RoyoGames.Document;
using System;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1000)]
public class LanguageManager : MonoBehaviour
{
    public static bool IsDestroy;
    private static LanguageManager _instance;

    [SerializeField] private TextAsset textAsset;
    [SerializeField] private SystemLanguage defaultLanguage;
    [SerializeField] private bool autoLoad = true;

    [HideInInspector] public static UnityAction OnChangedLanguageEvent;

    private static EntityTable table;

    private static string currentLanguageName;
    private static string defaultLanguageName;

    public static SystemLanguage DefaultLanguage
    {
        get
        {
            return _instance.defaultLanguage;
        }

        set
        {
            _instance.defaultLanguage = value;
            defaultLanguageName = value.ToString();
        }
    }

    private static SystemLanguage currentLanguage;
    public static SystemLanguage CurrentLanguage
    {
        get
        {
            return currentLanguage;
        }

        set
        {
            currentLanguage = value;
            currentLanguageName = value.ToString();

            if (table != null)
                return;

            foreach (var textTranslate in FindObjectsByType<TextTranslator>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                textTranslate.Translate();
            }

            OnChangedLanguageEvent?.Invoke();
        }
    }

    public static TextAsset TextAsset
    {
        get
        {
            return _instance.textAsset;
        }
        set
        {
            _instance.textAsset = value;
        }
    }

    public void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        if (autoLoad)
            Load();
    }

    private void OnDestroy()
    {
        IsDestroy = true;
    }

    public static void Load()
    {
        Load(Application.systemLanguage);
    }

    public static void Load(SystemLanguage language)
    {
        table = new EntityTable();
        table.LoadFromString(_instance.textAsset.text);

        DefaultLanguage = _instance.defaultLanguage;
        CurrentLanguage = language;
    }

    public static SystemLanguage[] GetLanguages()
    {
        if (table == null)
            return new SystemLanguage[0];

        string[] columnNames = table.GetColumns();
        SystemLanguage[] languages = new SystemLanguage[columnNames.Length - 1];

        for (int i = 1; i < columnNames.Length; i++)
            languages[i - 1] = Enum.Parse<SystemLanguage>(columnNames[i]);

        return languages;
    }

    public static string Translate(string key)
    {
        if (table == null)
            return "";

        EntityTable.Row row = table.GetRow("Key", key);
        string value = row.GetValue(currentLanguageName);

        if (string.IsNullOrEmpty(value))
            value = row.GetValue(defaultLanguageName);

        return value;
    }
}
