using UnityEngine;

public enum UIPopupDirection
{
    Top,
    Bot,
    Left,
    Right
}

public class UIPopup : MonoBehaviour
{
    private RectTransform body;

    public UIPanel Panel => panel;
    public bool AutoClose
    {
        get
        {
            return autoClose;
        }
        set
        {
            autoClose = value;
        }
    }

    [SerializeField] private RectTransform pointRect;
    [SerializeField] private UIPopupDirection direction;

    [Space]
    [SerializeField] private bool autoClose = true;

    private UIPanel panel;
    private RectTransform rectTransform;

    private float heightOfset;
    private float widthOfset;

    private Vector3 pointPosition;

    private RectTransform horizontalArrowArea;
    private RectTransform verticalArrowArea;

    private RectTransform[] arrows = new RectTransform[4];

    private RectTransform currentArrow;

    private void Awake()
    {
        panel = GetComponent<UIPanel>();
        panel.OnOpening.AddListener(OnPanelOpen);
        panel.OnClosing.AddListener(OnPanelClose);

        body = transform.Find("Body") as RectTransform;
        horizontalArrowArea = body.Find("Arrows/Horizontal Area") as RectTransform;
        verticalArrowArea = body.Find("Arrows/Vertical Area") as RectTransform;

        arrows[0] = horizontalArrowArea.Find("Arrow Bot") as RectTransform;
        arrows[1] = horizontalArrowArea.Find("Arrow Top") as RectTransform;
        arrows[2] = verticalArrowArea.Find("Arrow Right") as RectTransform;
        arrows[3] = verticalArrowArea.Find("Arrow Left") as RectTransform;

        rectTransform = transform as RectTransform;
        enabled = false;
    }
    public void Open()
    {
        Open(pointRect, direction);
    }
    public void Open(RectTransform pointRect)
    {
        Open(pointRect, direction);
    }
    public void Open(UIPopupDirection direction)
    {
        Open(pointRect, direction);
    }
    public void Open(RectTransform pointRect, UIPopupDirection direction)
    {
        this.direction = direction;
        this.pointRect = pointRect;
        UpdatePopupPosition();
        panel.Show();
    }
    public void Close(float delay = 0)
    {
        panel.Hide(delay);
    }
    private void Update()
    {
        if (panel.IsShow)
        {
            if (autoClose && Input.GetMouseButton(0) && !RectTransformUtility.RectangleContainsScreenPoint(body, Input.mousePosition))
            {
                Close();
            }
        }

        UpdatePopupPosition();
    }

    private void UpdatePopupPosition()
    {
        if (pointRect != null)
        {
            pointPosition = rectTransform.InverseTransformPoint(pointRect.position);
            var dir = DetectedDirection(direction);
            UpdateArrowPosition(dir);

            heightOfset = (pointRect.rect.height + currentArrow.rect.height) / 2;
            widthOfset = (pointRect.rect.width + currentArrow.rect.width) / 2;

            switch (dir)
            {
                case UIPopupDirection.Top:
                    pointPosition.y += heightOfset;
                    body.pivot = new Vector2(0.5f, 0);
                    break;
                case UIPopupDirection.Left:
                    pointPosition.x -= widthOfset;
                    body.pivot = new Vector2(1, 0.5f);
                    break;
                case UIPopupDirection.Right:
                    pointPosition.x += widthOfset;
                    body.pivot = new Vector2(0, 0.5f);
                    break;
                case UIPopupDirection.Bot:
                    pointPosition.y -= heightOfset;
                    body.pivot = new Vector2(0.5f, 1);
                    break;
            }

            body.localPosition = ClampPosition(pointPosition, body, rectTransform);
        }
    }
    private void UpdateArrowPosition(UIPopupDirection direction)
    {
        var arrow = arrows[(int)direction];

        if (currentArrow != arrow)
        {
            foreach (var item in arrows)
            {
                if (arrow != item)
                    item.gameObject.SetActive(false);
            }

            arrow.gameObject.SetActive(true);
            currentArrow = arrow;
        }

        var pointPos = Vector3.zero;
        var pos = arrow.localPosition;
        var clampSize = 0f;

        switch (direction)
        {
            case UIPopupDirection.Top:
            case UIPopupDirection.Bot:
                pointPos = horizontalArrowArea.InverseTransformPoint(pointRect.position);
                clampSize = (horizontalArrowArea.rect.width - arrow.rect.width) / 2;
                pos.x = Mathf.Clamp(pointPos.x, -clampSize, clampSize);

                break;
            case UIPopupDirection.Left:
            case UIPopupDirection.Right:
                pointPos = verticalArrowArea.InverseTransformPoint(pointRect.position);
                clampSize = (verticalArrowArea.rect.height - arrow.rect.height) / 2;
                pos.y = Mathf.Clamp(pointPos.y, -clampSize, clampSize);
                break;
        }

        arrow.localPosition = pos;
    }
    private UIPopupDirection DetectedDirection(UIPopupDirection direction)
    {
        switch (direction)
        {
            case UIPopupDirection.Top:
                if (pointPosition.y + body.rect.height + heightOfset > rectTransform.rect.height / 2)
                    direction = UIPopupDirection.Bot;
                break;
            case UIPopupDirection.Right:
                if (pointPosition.x + body.rect.width + widthOfset > rectTransform.rect.width / 2)
                    direction = UIPopupDirection.Left;
                break;
            case UIPopupDirection.Left:
                if (pointPosition.x - body.rect.width - widthOfset < -rectTransform.rect.width / 2)
                    direction = UIPopupDirection.Right;
                break;
            case UIPopupDirection.Bot:
                if (pointPosition.y - body.rect.height - heightOfset < -rectTransform.rect.height / 2)
                    direction = UIPopupDirection.Top;
                break;
        }

        return direction;
    }
    private Vector3 ClampPosition(Vector3 pos, RectTransform panel, RectTransform parent)
    {
        Vector3 minPosition = parent.rect.min - panel.rect.min;
        Vector3 maxPosition = parent.rect.max - panel.rect.max;

        pos.x = Mathf.Clamp(pos.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(pos.y, minPosition.y, maxPosition.y);

        return pos;
    }
    private void OnPanelOpen()
    {
        enabled = true;
    }
    private void OnPanelClose()
    {
        enabled = false;
    }
}
