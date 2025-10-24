using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

/*
 
Как работает Load() и Show()
Load() — загружает рекламу асинхронно. Это нужно делать заранее, потому что загрузка занимает время (сетевой запрос, кэширование).

Show() — показывает уже загруженную рекламу. Если реклама не загружена — ничего не произойдёт.

Одной загрузки недостаточно на всю игру: после показа рекламы она уничтожается (Yandex так работает). Чтобы показать рекламу снова — нужно загрузить её заново.

Поэтому:
Load() → (реклама загружена) → Show() → (реклама показана и уничтожена) → нужно снова Load() → Show().
 
 */

/// <summary>
/// Отвечает за межстраничную рекламу (Interstitial).
/// Используется для показа полноэкранных объявлений между сценами или после уровней.
/// </summary>
public class YandexInterstitialAd : MonoBehaviour
{
    [SerializeField] private string adUnitId = "demo-interstitial-yandex";


    /// <summary>
    /// Загрузчик рекламы.
    /// </summary>
    private InterstitialAdLoader interstitialAdLoader;
    
    /// <summary>
    /// Скаченная реклама, которая показывается пользователю.
    /// </summary>
    private Interstitial interstitial;

    private void Awake()
    {
        interstitialAdLoader = new InterstitialAdLoader();

        interstitialAdLoader.OnAdLoaded += (sender, args) =>
        {
            interstitial = args.Interstitial;
            Debug.Log("Реклама загружена");

            // Подписываемся на событие закрытия межстраничной рекламы один раз здесь
            interstitial.OnAdDismissed += OnInterstitialDismissed;
        };

        interstitialAdLoader.OnAdFailedToLoad += (sender, args) =>
        {
            Debug.Log($"Не удалось загрузить рекламу: {args.Message}");
            interstitial = null;
        };

        LoadAd();
    }


    /// <summary>
    /// Отображает межстраничную рекламу, при завершении скачивает (но пока что не отображает) новую.
    /// </summary>
    public void ShowInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Show();
        }
        else
        {
            Debug.Log("Реклама не готова, подождите.");
        }
    }

    /// <summary>
    /// Освобождаем ресурсы и загружаем следующую рекламу
    /// </summary>
    private void OnInterstitialDismissed(object sender, System.EventArgs args)
    {
        interstitial.Destroy();
        interstitial = null;
        LoadAd();
    }

    /// <summary>
    /// Подгружает следующую рекламу (пока что не отображает).
    /// </summary>
    private void LoadAd()
    {
        var request = new AdRequestConfiguration.Builder(adUnitId).Build();
        interstitialAdLoader.LoadAd(request);
    }

}
