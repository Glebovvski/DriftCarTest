using System;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static string uniqueUserId = "demoUserUnity";
    public event Action<bool> OnRewardedAdAvailable;

    private Action rewardAction;

    [SerializeField] private bool test = true;

    void Start()
    {
#if UNITY_ANDROID
        string appKey = "85460dcd";
#elif UNITY_IPHONE
        string appKey = "8545d445";
#else
        string appKey = "unexpected_platform";
#endif

        //Dynamic config example
        IronSourceConfig.Instance.setClientSideCallbacks(true);

        string id = IronSource.Agent.getAdvertiserId();

        IronSource.Agent.validateIntegration();

        IronSourceRewardedVideoEvents.onAdAvailableEvent += OnRewardedAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += OnRewardedUnavailable;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += OnAdRewarded;

        IronSource.Agent.init(appKey);
    }

    private void OnAdRewarded(IronSourcePlacement arg1, IronSourceAdInfo arg2)
    {
        rewardAction?.Invoke();
        rewardAction = null;
    }

    private void OnRewardedUnavailable()
    {
        OnRewardedAdAvailable?.Invoke(false);
    }

    private void OnRewardedAvailable(IronSourceAdInfo adInfo)
    {
        OnRewardedAdAvailable?.Invoke(true);
    }


    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    public void ShowRewarded(Action _rewardAction)
    {
        rewardAction = _rewardAction;
#if UNITY_ANDROID || UNITY_IPHONE
        if (test)
        {
            rewardAction?.Invoke();
        }
        else
        {
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                IronSource.Agent.showRewardedVideo();
            }
            else
            {
                Debug.LogError("REWARDED AD UNAVAILABLE");
            }
        }
#elif UNITY_EDITOR
        rewardAction?.Invoke();
#endif
    }
}