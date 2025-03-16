using System;
using UnityEditor;

public class StringArrayPopupProperty
{
    public string[] Values { get; set; }
    public int SelectedIndex { get; set; }

    public StringArrayPopupProperty(string[] values, string selectedValue)
    {
        Set(values, selectedValue);
    }
    public void Set(string[] values, string selectedValue)
    {
        if (values == null)
            values = new string[0];

        Values = values;
        SelectedIndex = Array.IndexOf(Values, selectedValue);
    }
    public string DrawLayout(string label)
    {
        SelectedIndex = EditorGUILayout.Popup(label, SelectedIndex, Values);

        if (Values.Length > 0)
            return Values[SelectedIndex >= 0 ? SelectedIndex : 0];
        else
            return "";
    }
}
