using UnityEngine;

[ExecuteInEditMode()]
public class UISafeArea : MonoBehaviour
{
    private Rect lastSafeArea;
    private ScreenOrientation tempOrientation;

    [SerializeField] private SafeAreaSide safeAreaSide;
    [SerializeField] private BannerSide bannerSide;

    public enum SafeAreaSide
    {
        None,
        Top,
        Bot,
        Both
    }
    public enum BannerSide
    {
        None,
        Top,
        Bot
    }
    private void Update()
    {
        if (!Application.isPlaying || lastSafeArea.size != Screen.safeArea.size || tempOrientation != Screen.orientation)
            UpdateSafeArea();
    }
    public void UpdateSafeArea()
    {
        if (Screen.width == 0 || Screen.height == 0)
            return;

        var rect = transform as RectTransform;
        var safeArea = Screen.safeArea;

        var anchorMin = safeArea.position;
        var anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMax.x /= Screen.width;

        switch (safeAreaSide)
        {
            case SafeAreaSide.Top:
                anchorMax.y /= Screen.height;
                anchorMin.y = 0;
                break;
            case SafeAreaSide.Bot:
                anchorMin.y /= Screen.height;
                anchorMax.y = 1;
                break;
            case SafeAreaSide.Both:
                anchorMin.y /= Screen.height;
                anchorMax.y /= Screen.height;
                break;
            case SafeAreaSide.None:
                anchorMin = new Vector2(0, 0);
                anchorMax = new Vector2(1, 1);
                break;
        }

        if(bannerSide != BannerSide.None)
        {
            float rate = GetBannerHeight() / Screen.height;

            switch (bannerSide)
            {
                case BannerSide.Top:
                    anchorMax.y -= rate;
                    break;

                case BannerSide.Bot:
                    anchorMin.y += rate;
                    break;
            }
        }

        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;  

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;

        lastSafeArea = Screen.safeArea;
        tempOrientation = Screen.orientation;
    }
    private float GetBannerHeight()
    {
        if (Application.isEditor)
            return 100;

        return 100;

        //return MaxSdkUtils.GetScreenDensity() * MaxSdkUtils.GetAdaptiveBannerHeight();
    }
}
