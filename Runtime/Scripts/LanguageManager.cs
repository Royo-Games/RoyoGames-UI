using RoyoGames.Document;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1000)]
public class LanguageManager : Singleton<LanguageManager>
{
    public static bool IsDestroy;

    public static SystemLanguage CurrentLanguage { get; private set; }

    [SerializeField] private TextAsset textAsset;
    [SerializeField] private SystemLanguage defaultLanguage;
    [SerializeField] private bool autoLoad = true;

    [HideInInspector] public UnityAction OnChangedLanguageEvent;

    private EntityTable table;

    private static string currentLanguageName;
    private static string defaultLanguageName;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        Load();
    }

    private void OnDestroy()
    {
        IsDestroy = true;
    }

    private void Load()
    {
        table = new EntityTable();
        table.LoadFromString(textAsset.text);

        SetDefaultLanguage(defaultLanguage);
        SetLanguage(Application.systemLanguage);
    }

    public void SetDefaultLanguage(SystemLanguage language)
    {
        defaultLanguage = language;
        defaultLanguageName = defaultLanguage.ToString();
    }

    public void SetLanguage(SystemLanguage language)
    {
        CurrentLanguage = language;
        currentLanguageName = language.ToString();

        foreach (var textTranslate in FindObjectsByType<TextTranslator>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            textTranslate.Translate();
        }

        OnChangedLanguageEvent?.Invoke();
    }

    public static string Translate(string key)
    {
        EntityTable.Row row = Instance.table.GetRow("Key", key);
        string value = row.GetValue(currentLanguageName);

        if(string.IsNullOrEmpty(value))
            value = row.GetValue(defaultLanguageName);

        return value;
    }
}
