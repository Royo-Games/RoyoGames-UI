using System.IO;
using UnityEditor;
using UnityEngine;

public class LanguageManagerMenuItems : MonoBehaviour
{
    [MenuItem("GameObject/Royo Games/Language/Language Manager", false, 0)]
    private static void AddLanguageManager()
    {
        GameObject uiPanel = Instantiate(Resources.Load<GameObject>("Language Manger/Language Manaager"));
        uiPanel.name = "Language Manager";
        Selection.activeObject = uiPanel;
    }

    [MenuItem("Assets/Create/Royo Games/Language/Text Template", priority = 0)]
    private static void CreateLanguageTextTemplate()
    {
        string directory = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (Path.HasExtension(directory))
            directory = Path.GetDirectoryName(directory);

        TextAsset textAsset = Resources.Load<TextAsset>("Language Manger/LanguageTextTemplate");
        
        string sourcePath = AssetDatabase.GetAssetPath(textAsset);

        string fileName = Path.GetFileName(sourcePath);
        string destPath = Path.Combine(directory, fileName).Replace("\\", "/");

        if (AssetDatabase.CopyAsset(sourcePath, destPath))
        {
            AssetDatabase.Refresh();

            Object newAsset = AssetDatabase.LoadAssetAtPath<Object>(destPath);
            Selection.activeObject = newAsset;
            EditorUtility.FocusProjectWindow();
        }
    }
}
