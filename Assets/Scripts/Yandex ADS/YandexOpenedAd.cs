using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

/// <summary>
/// ��������� ��������� � ������� ������� ��� �������� ���������� (App Open Ad) ����� Yandex Mobile Ads.
/// ��������� ������� ��� �������, ����� ����������, � ����� �������� ����������� ������� � ��������� �����.
/// </summary>
public class YandexOpenedAd : MonoBehaviour
{
    [SerializeField] private string adUnitId = "demo-appopenad-yandex";

    private AppOpenAdLoader appOpenAdLoader;
    private AppOpenAd appOpenAd;

    private void Awake()
    {
        appOpenAdLoader = new AppOpenAdLoader();

        // �������� �� ������� �������� �������� �������
        appOpenAdLoader.OnAdLoaded += (sender, args) =>
        {
            appOpenAd = args.AppOpenAd;
            ShowAd();   // <------------ ��� ����� ���. ���� ���� ������� ��������� �� ����� � ���������� ������������.
        };

        // �������� �� ������� ��������� ��������
        appOpenAdLoader.OnAdFailedToLoad += (sender, args) =>
        {
            appOpenAd = null;
            Debug.LogWarning("AppOpenAd failed to load: " + args.Message);
        };

        LoadAd();
    }

    // ��������� ��������� ������
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

    // ���������� ������� � ������������� �� ������� ��������
    private void ShowAd()
    {
        if (appOpenAd != null)
        {
            appOpenAd.OnAdDismissed += (s, e) =>
            {
                appOpenAd.Destroy();
                appOpenAd = null;
                // LoadAd(); // ��������� ����� ������� ��� ���������� ������
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
