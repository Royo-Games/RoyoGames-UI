using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LayoutElement))]
public class ScrollableMenuButton : MonoBehaviour, IPointerClickHandler
{
    public Image IconImage;
    public TextMeshProUGUI TitleText;

    private Animator animator;
    private ScrollableMenu mainMenu;

    private void Awake()
    {
        mainMenu = GetComponentInParent<ScrollableMenu>();
        animator = GetComponent<Animator>();
    }
    public void Select()
    {
        TitleText.gameObject.SetActive(true);
        animator.SetTrigger("Selected");
    }
    public void Deselect()
    {
        TitleText.gameObject.SetActive(false);
        animator.SetTrigger("Deselected");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        mainMenu.ScrollRectSnap.CenterTo(transform.GetSiblingIndex());
    }
}
