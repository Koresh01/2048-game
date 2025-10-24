using TMPro;
using UnityEngine;

/// <summary>
/// Пример как может выглядеть скрипт вызывающий РЕКЛАМУ С ВОЗНАГРАЖДЕНИЕМ.
/// </summary>
public class RewardSetExample : MonoBehaviour
{
    public YandexRewardedAd rewardedAdScript;
    public GameManager gameManager;

    public void OnRewardedAdButtonClicked()
    {
        rewardedAdScript.ShowRewarded(() =>
        {
            // Код выдачи награды игроку, например:
            Debug.Log("Игрок получил 100 монет!");
            // Добавьте монеты игроку

            gameManager.IncreaseScore(100);
        });
    }
}
