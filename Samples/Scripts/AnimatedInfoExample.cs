using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedInfoExample : MonoBehaviour
{
    [SerializeField] AnimatedInfo animatedInfoPrefab;
    [SerializeField] int maxCount = 2;

    private AnimatedInfoPanel infoPanel;

    private void Awake()
    {   
        infoPanel = GetComponent<AnimatedInfoPanel>();
    }
    public void Create()
    {
        infoPanel.InstantiateInfo(animatedInfoPrefab, maxCount);
    }
}
