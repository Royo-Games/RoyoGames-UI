using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatedInfo : MonoBehaviour
{
    public static List<AnimatedInfo> ActiveList = new();
    public static int MaxCount = 1;

    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private bool autoShow = true;

    public UIPanel Panel { get; private set; }

    public bool AutoDestroy
    {
        get
        {
            return autoDestroy;
        }
        set
        {
            autoDestroy = value;
        }
    }
    public bool AutoShow
    {
        get
        {
            return autoShow;
        }
        set
        {
            autoShow = value;
        }
    }
    private void Awake()
    {
        Panel = GetComponent<UIPanel>();
        Panel.OnBeginShow.AddListener(OnBeginShow);
        Panel.OnEndShow.AddListener(OnEndShow);
        Panel.OnEndHide.AddListener(OnEndHide);
    }
    private void Start()
    {
        if(autoShow)
            Panel.Show();
    }
    private void OnDestroy()
    {
        ActiveList.Remove(this);
    }
    private void OnBeginShow()
    {
        if(ActiveList.Count > 0 && ActiveList.Count >= MaxCount)
        {
            ActiveList[0].Panel.HideImmediate();
            ActiveList.RemoveAt(0);
        }

        ActiveList.Add(this);
    }
    private void OnEndShow()
    {
        ActiveList.Remove(this);
        Panel.Hide();
    }
    private void OnEndHide()
    {
        if(autoDestroy)
        Destroy(gameObject);
    }
}