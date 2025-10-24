using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

/// <summary>
/// Отвечает за рекламу с вознаграждением (Rewarded).
/// </summary>
public class YandexRewardedAd : MonoBehaviour
{
    [SerializeField] private string adUnitId = "demo-rewarded-yandex";


    /// <summary>
    /// Загрузчик рекламы.
    /// </summary>
    private RewardedAdLoader rewardedLoader;
    
    /// <summary>
    /// Сама реклама.
    /// </summary>
    private RewardedAd rewardedAd;

    private void Awake()
    {
        rewardedLoader = new RewardedAdLoader();
        LoadRewarded(); // Загружаем первую рекламу при старте
    }

    /// <summary>
    /// Загружает рекламу с вознаграждением.
    /// Целесообразно вызывать при старте или сразу после показа предыдущей рекламы.
    /// </summary>
    private void LoadRewarded()
    {
        var request = new AdRequestConfiguration.Builder(adUnitId).Build();

        rewardedLoader.OnAdLoaded += (s, e) =>
        {
            rewardedAd = e.RewardedAd;
            Debug.Log("Yandex Rewarded: загружена");
        };

        rewardedLoader.LoadAd(request);
    }

    /// <summary>
    /// Показывает рекламу с вознаграждением.
    /// Целесообразно вызывать, когда пользователь хочет получить бонус.
    /// После показа автоматически загружается новая.
    /// </summary>
    /// <param name="onReward">Действие, выполняемое при выдаче награды. Другими словами - функция с типом void.</param>
    public void ShowRewarded(System.Action onReward)
    {
        // Проверяем, загружена ли реклама
        if (rewardedAd == null)
        {
            Debug.Log("Yandex Rewarded: не загружена, пробуем снова...");
            LoadRewarded();
            return;
        }

        // Подписываемся на событие, когда пользователь досмотрел рекламу до конца
        rewardedAd.OnRewarded += (s, e) =>
        {
            Debug.Log("Yandex Rewarded: пользователь получил награду");
            onReward?.Invoke(); // Выполняем награду (например, даём премиум валюту)
        };

        // Подписываемся на событие закрытия рекламы
        rewardedAd.OnAdDismissed += (s, e) =>
        {
            // Когда пользователь закрыл видео — удаляем старое объявление,
            // чтобы не использовать его повторно.
            rewardedAd.Destroy();
            rewardedAd = null;

            // Загружаем новое объявление, чтобы быть готовыми к следующему показу
            LoadRewarded();
        };

        // Показываем рекламу пользователю
        rewardedAd.Show();
        Debug.Log("Yandex Rewarded: показано");
    }
}
