using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class UIPanelEditor
{
    [InitializeOnLoadMethod]
    private static void InitializeOnLoad()
    {
        Selection.selectionChanged += OnSelectionChanged;
    }
    private static void OnSelectionChanged()
    {
        if (EditorApplication.isPlaying)
            return;

        DisablePanels();

        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            EnableSelectedPanels(Selection.gameObjects[i].transform);
        }
    }
    private static void EnableSelectedPanels(Transform selectedTransform)
    {
        bool isDirty = false;

        foreach (var panel in selectedTransform.GetComponentsInParent<UIPanel>())
        {
            panel.GetComponent<CanvasGroup>().alpha = 1;
            isDirty = true;
        }

        if (isDirty)
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
    private static void DisablePanels()
    {
        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
        {
            GameObject[] gameObjects = EditorSceneManager.GetSceneAt(i).GetRootGameObjects();

            foreach (var item in gameObjects)
            {
                UIPanel[] panels = item.GetComponentsInChildren<UIPanel>(true);

                foreach (var panel in panels)
                {
                    if (panel.IsEditMode)
                        continue;

                    panel.GetComponent<CanvasGroup>().alpha = 0;
                }
            }
        }
    }
}