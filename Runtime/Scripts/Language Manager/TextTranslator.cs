using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextTranslator : MonoBehaviour
{
    public string Key
    {
        get
        {
            return key;
        }
        set
        {
            key = value;
            Translate(true);
        }
    }
    public TextMeshProUGUI Text
    {
        get
        {
            if (text == null)
                text = GetComponent<TextMeshProUGUI>();

            return text;
        }
        set
        {
            text = value;
        }
    }
   
    [SerializeField] private string key;

    private TextMeshProUGUI text;
    private string value = null;
    private object[] formatParameters;
    private SystemLanguage language = SystemLanguage.Unknown;

    private void Awake()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        Translate();
    }
    internal void Translate(bool forceTranslate = false)
    {
        if (LanguageManager.CurrentLanguage == language && !forceTranslate)
            return;

        language = LanguageManager.CurrentLanguage;

        if (!string.IsNullOrEmpty(key))
        {
            value = LanguageManager.Translate(key);
            Text.text = value;
        }

        Format();
    }
    public void Format(params object[] parameters)
    {
        formatParameters = parameters;
        Format();
    }
    private void Format()
    {
        if (formatParameters == null || formatParameters.Length == 0 || string.IsNullOrEmpty(value))
            return;

        Text.text = string.Format(value, formatParameters);
    }
    public void ClearFormat()
    {
        formatParameters = null;
    }
}