using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    private static bool isDestroy;
    public static UIManager Instance
    {
        get
        {
            if (isDestroy)
                return null;

            if(instance == null)
            {
                instance = new GameObject("Royo Games UI Manager").AddComponent<UIManager>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    private List<GameObject> highlightObjectList = new List<GameObject>();

    private void OnDestroy()
    {
        isDestroy = true;
    }
    public static void AddHighlightUIObject(GameObject highlightObject, int sortingOrder)
    {
        if(Instance.highlightObjectList.Contains(highlightObject))
            return;

        var canvas = highlightObject.AddComponent<Canvas>();
        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 |
                                               AdditionalCanvasShaderChannels.TexCoord2 |
                                               AdditionalCanvasShaderChannels.TexCoord3 |
                                               AdditionalCanvasShaderChannels.Normal |
                                               AdditionalCanvasShaderChannels.Tangent;
        highlightObject.AddComponent<GraphicRaycaster>();

        canvas.overrideSorting = true;
        canvas.sortingOrder = sortingOrder;

       Instance.highlightObjectList.Add(highlightObject);
    }
    public static void RemoveHighlightUIObjects(GameObject highlightObject)
    {
        if (highlightObject == null)
            return;

        if (!Instance.highlightObjectList.Contains(highlightObject))
            return;

        Instance.highlightObjectList.Remove(highlightObject);

        DestroyHighlightUIObjectCanvasComponents(highlightObject);
    }
    public static void ClearHighlightUIObjects()
    {
        foreach (var highlightObject in Instance.highlightObjectList)
        {
            if (highlightObject == null)
                continue;

            DestroyHighlightUIObjectCanvasComponents(highlightObject);
        }

        Instance.highlightObjectList.Clear();
    }
    private static void DestroyHighlightUIObjectCanvasComponents(GameObject highlightObject)
    {
        Destroy(highlightObject.GetComponent<GraphicRaycaster>());
        Destroy(highlightObject.GetComponent<Canvas>());
    }
}
