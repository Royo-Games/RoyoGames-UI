using UnityEngine;

public enum UIPopupDirection
{
    Top,
    Bot,
    Left,
    Right
}

[RequireComponent(typeof(UIPanel))]
public class UIPopup : MonoBehaviour
{
    private RectTransform body;

    [SerializeField] private UIPopupDirection direction;
    [SerializeField] private float offset;
    [SerializeField] private Vector2 clampPadding = new Vector2(20, 20);
    [Space]
    [SerializeField] private bool autoClose = true;

    private UIPanel panel;
    private RectTransform rectTransform;

    public UIPanel Panel
    {
        get
        {
            if(panel == null)
                panel = GetComponent<UIPanel>();

            return panel;
        }
    }

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

    public UIPopupDirection Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
            isChanged = true;
        }
    }

    public float Offset
    {
        get
        {
            return offset;
        }
        set
        {
            offset = value;
            isChanged = true;
        }
    }
    public Vector2 ClampPadding
    {
        get
        {
            return clampPadding;
        }
        set
        {
            clampPadding = value;
            isChanged = true;
        }
    }
    private Vector2 position;
    public Vector2 Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
            isChanged = true;
        }
    }

    private RectTransform horizontalArrowArea;
    private RectTransform verticalArrowArea;

    private RectTransform[] arrows = new RectTransform[4];

    private RectTransform currentArrow;
    private UIPopupDirection currentDirection;
    private bool isChanged;

    private void Awake()
    {
        panel = GetComponent<UIPanel>();
        panel.OnBeginShow.AddListener(OnPanelOpen);
        panel.OnBeginHide.AddListener(OnPanelClose);

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
    private void OnValidate()
    {
        if (Application.isPlaying)
            isChanged = true;
    }
    public void Show(Vector2 position)
    {
        Show(position, offset);
    }
    public void Show(Vector2 position, float offset)
    {
        this.offset = offset;
        this.position = position;
        UpdatePopupPosition();
        panel.Show();
    }
    public void Hide(float delay = 0)
    {
        panel.Hide(delay);
    }
    private void Update()
    {
        if (panel.IsShow)
        {
            if (autoClose && Input.GetMouseButton(0) && !RectTransformUtility.RectangleContainsScreenPoint(body, Input.mousePosition))
            {
                Hide();
            }
        }

        if (isChanged)
        {
            isChanged = false;
            UpdatePopupPosition();
        }
    }
    public void UpdatePopupPosition()
    {
        Vector3 pointPosition = rectTransform.InverseTransformPoint(Position);

        currentDirection = DetectedDirection(pointPosition, direction);

        switch (currentDirection)
        {
            case UIPopupDirection.Top:
                pointPosition.y += offset;
                body.pivot = new Vector2(0.5f, 0);
                break;
            case UIPopupDirection.Left:
                pointPosition.x -= offset;
                body.pivot = new Vector2(1, 0.5f);
                break;
            case UIPopupDirection.Right:
                pointPosition.x += offset;
                body.pivot = new Vector2(0, 0.5f);
                break;
            case UIPopupDirection.Bot:
                pointPosition.y -= offset;
                body.pivot = new Vector2(0.5f, 1);
                break;
        }

        body.localPosition = ClampPosition(pointPosition, body, rectTransform);
        UpdateArrowPosition(currentDirection);

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
                pointPos = horizontalArrowArea.InverseTransformPoint(Position);
                clampSize = (horizontalArrowArea.rect.width - arrow.rect.width) / 2;
                pos.x = Mathf.Clamp(pointPos.x, -clampSize, clampSize);

                break;
            case UIPopupDirection.Left:
            case UIPopupDirection.Right:
                pointPos = verticalArrowArea.InverseTransformPoint(Position);
                clampSize = (verticalArrowArea.rect.height - arrow.rect.height) / 2;
                pos.y = Mathf.Clamp(pointPos.y, -clampSize, clampSize);
                break;
        }

        arrow.localPosition = pos;
    }
    private UIPopupDirection DetectedDirection(Vector3 pointPosition, UIPopupDirection direction)
    {
        switch (direction)
        {
            case UIPopupDirection.Top:
                if (pointPosition.y + body.rect.height + offset > rectTransform.rect.height / 2)
                    direction = UIPopupDirection.Bot;
                break;
            case UIPopupDirection.Right:
                if (pointPosition.x + body.rect.width + offset > rectTransform.rect.width / 2)
                    direction = UIPopupDirection.Left;
                break;
            case UIPopupDirection.Left:
                if (pointPosition.x - body.rect.width - offset < -rectTransform.rect.width / 2)
                    direction = UIPopupDirection.Right;
                break;
            case UIPopupDirection.Bot:
                if (pointPosition.y - body.rect.height - offset < -rectTransform.rect.height / 2)
                    direction = UIPopupDirection.Top;
                break;
        }

        return direction;
    }
    private Vector3 ClampPosition(Vector3 pos, RectTransform panel, RectTransform parent)
    {
        Vector2 minPosition = parent.rect.min - panel.rect.min + clampPadding;
        Vector2 maxPosition = parent.rect.max - panel.rect.max - clampPadding;

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
