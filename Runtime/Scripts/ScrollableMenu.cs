using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollableMenu : MonoBehaviour
{
    public ScrollRect ScrollRect;
    public ScrollRectSnap ScrollRectSnap;
    public Scrollbar ButtonsScrollBar;
    public HorizontalLayoutGroup ButtonsLayoutGroup;

    private ScrollableMenuButton currentButton;
    private ScrollableMenuPage currentPage;

    [SerializeField] private float flexibleWidth = 2;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        ScrollRect.onValueChanged.AddListener(ScrollRectOnValueChanged);
        ScrollRectSnap.OnCenterElement.AddListener(OnChangedCenterElement);
    }
    private void ScrollRectOnValueChanged(Vector2 pos)
    {
        ButtonsScrollBar.value = pos.x;
    }
    private void OnChangedCenterElement(int elementIndex)
    {
        UpdateButtonScrollbarSize();
        var button = ButtonsLayoutGroup.transform.GetChild(elementIndex).GetComponent<ScrollableMenuButton>();

        if (button != currentButton)
        {
            if (currentButton != null)
                currentButton.Deselect();

            button.Select();

            currentButton = button;
        }

        var page = ScrollRect.content.GetChild(elementIndex).GetComponent<ScrollableMenuPage>();

        if (page != currentPage)
        {
            if (currentPage != null)
                currentPage.OnClose.Invoke();

            currentPage = page;
            page.OnOpen.Invoke();
        }
    }
    private void UpdateButtonScrollbarSize()
    {
        float count = ButtonsLayoutGroup.transform.childCount - 1 + flexibleWidth;
        float size = 1.0f / count * flexibleWidth;
        ButtonsScrollBar.size = size;
    }
}
