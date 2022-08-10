using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyApplovinRewarded : MonoBehaviour
{
    static string adUnitId = "86a78a0486613e48";
    int retryAttempt;
    public static UnityEvent watchAds = new UnityEvent();

    public static MyUnityEventClass video_ads_watch = new MyUnityEventClass();
    public static MyUnityEventClass video_ads_started = new MyUnityEventClass();
    public static MyUnityEventClass video_ads_available = new MyUnityEventClass();
    public static MyUnityEventClass errorLoadAds = new MyUnityEventClass();

    public static bool rewardedAsLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }

    public static void LoadRewardedAd()
    {
        rewardedAsLoaded = false;
        MaxSdk.LoadRewardedAd(adUnitId);
    }

    public static void Show ()
    {
        string sucsess = "not_available";
        if (MaxSdk.IsRewardedAdReady(adUnitId))
        {
            MaxSdk.ShowRewardedAd(adUnitId);
            sucsess = "success";
            if (video_ads_started != null)
                video_ads_started.Invoke("rewarded", ADSController._adsType.ToString(), sucsess, Application.internetReachability.GetHashCode());
        }
        else
            LoadRewardedAd();
        if (video_ads_available != null)
            video_ads_available.Invoke("rewarded", ADSController._adsType.ToString(), sucsess, Application.internetReachability.GetHashCode());
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.
        //MaxSdk.ShowRewardedAd(adUnitId);
        // Reset retry attempt
        retryAttempt = 0;
        rewardedAsLoaded = true;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        double retryDelay = Mathf.Pow(2, Mathf.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float)retryDelay);
        ADSController.RestartInterstitial();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadRewardedAd();
        ADSController.RestartInterstitial();
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
        ADSController.RestartInterstitial();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (video_ads_watch != null)
            video_ads_watch.Invoke("rewarded", ADSController._adsType.ToString(), "clicked", Application.internetReachability.GetHashCode());
    }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
        ADSController.RestartInterstitial();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        if (watchAds != null)
        {
            watchAds.Invoke();
        }
        if (video_ads_watch != null)
            video_ads_watch.Invoke("rewarded", ADSController._adsType.ToString(), "watched", Application.internetReachability.GetHashCode());

        ADSController.RestartInterstitial();
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {

        ADSController.RestartInterstitial();
        // Ad revenue paid. Use this callback to track user revenue.

        // Update is called once per frame

    }
}
