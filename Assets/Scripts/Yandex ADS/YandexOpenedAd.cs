using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

/// <summary>
/// Управляет загрузкой и показом рекламы при открытии приложения (App Open Ad) через Yandex Mobile Ads.
/// Загружает рекламу при запуске, сразу показывает, а после закрытия освобождает ресурсы и загружает новую.
/// </summary>
public class YandexOpenedAd : MonoBehaviour
{
    [SerializeField] private string adUnitId = "demo-appopenad-yandex";

    private AppOpenAdLoader appOpenAdLoader;
    private AppOpenAd appOpenAd;

    private void Awake()
    {
        appOpenAdLoader = new AppOpenAdLoader();

        // Подписка на событие успешной загрузки рекламы
        appOpenAdLoader.OnAdLoaded += (sender, args) =>
        {
            appOpenAd = args.AppOpenAd;
            ShowAd();   // <------------ Вся фишка тут. Типо если реклама скачалась то сразу её показываем пользователю.
        };

        // Подписка на событие неудачной загрузки
        appOpenAdLoader.OnAdFailedToLoad += (sender, args) =>
        {
            appOpenAd = null;
            Debug.LogWarning("AppOpenAd failed to load: " + args.Message);
        };

        LoadAd();
    }

    // Загружает рекламный баннер
    private void LoadAd()
    {
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        var request = new AdRequestConfiguration.Builder(adUnitId).Build();
        appOpenAdLoader.LoadAd(request);
    }

    // Показывает рекламу и подписывается на событие закрытия
    private void ShowAd()
    {
        if (appOpenAd != null)
        {
            appOpenAd.OnAdDismissed += (s, e) =>
            {
                appOpenAd.Destroy();
                appOpenAd = null;
                // LoadAd(); // Загружаем новую рекламу для следующего показа
            };

            appOpenAd.Show();
        }
    }

    private void OnDestroy()
    {
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }
    }
}
