using System.Reflection;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

/// <summary>
/// Отвечает за баннерную рекламу Yandex.
/// Показывает баннер выбранного размера и позиции.
/// </summary>
public class YandexBannerAd : MonoBehaviour
{
    [SerializeField] private string adUnitId = "demo-banner-yandex";

    private Banner banner;
    private bool isLoaded = false;

    private void Awake()
    {
        ShowBanner();
    }

    /// <summary>
    /// Подготавливает баннер к показу.
    /// </summary>
    private void LoadBanner()
    {
        banner = new Banner(adUnitId, GetAdSize(), GetAdPosition());
        banner.LoadAd(new AdRequest.Builder().Build());
    }

    /// <summary>
    /// Отображает баннер. Если баннер не загружен — пытается загрузить заново.
    /// </summary>
    public void ShowBanner()
    {
        if (banner != null)
        {
            // Уничтожаем старый баннер перед созданием нового
            banner.Destroy();
            banner = null;
            isLoaded = false;
        }

        LoadBanner();

        // Показать баннер лучше после того, как он загрузится
        banner.OnAdLoaded += (sender, args) =>
        {
            banner.Show();
            isLoaded = true;
        };

        banner.OnAdFailedToLoad += (sender, args) =>
        {
            Debug.LogWarning("Banner failed to load: " + args.Message);
            isLoaded = false;
        };
    }


    /// <summary>
    /// Скрывает баннер.
    /// </summary>
    public void HideBanner()
    {
        banner?.Hide();
    }



    /// <summary>
    /// Получаем размер баннера из перечисления
    /// </summary>
    private BannerAdSize GetAdSize()
    {
        int screenWidthDp = ScreenUtils.ConvertPixelsToDp((int)Screen.safeArea.width);
        //return BannerAdSize.InlineSize(screenWidthDp, 100); // 100 — пример высоты, можно параметризовать
        return BannerAdSize.StickySize(screenWidthDp);
        //return BannerAdSize.FixedSize(300, 250);
    }

    /// <summary>
    /// Получаем позицию баннера из перечисления
    /// </summary>
    private AdPosition GetAdPosition()
    {
        return AdPosition.TopCenter;
        // return AdPosition.BottomCenter,
    }
}
