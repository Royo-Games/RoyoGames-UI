using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<UIPanel> ActivePanels = new List<UIPanel>();

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
                instance = new GameObject("UIPanel Manager").AddComponent<UIManager>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }
    private void OnDestroy()
    {
        isDestroy = true;
    }
}
