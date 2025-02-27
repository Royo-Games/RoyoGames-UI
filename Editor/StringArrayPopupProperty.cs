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
        Values = values;
        SelectedIndex = Array.IndexOf(Values, selectedValue);
    }
    public string DrawLayout(string label)
    {
        SelectedIndex = EditorGUILayout.Popup(label, SelectedIndex, Values);
        return Values[SelectedIndex >= 0 ? SelectedIndex : 0];
    }
}
