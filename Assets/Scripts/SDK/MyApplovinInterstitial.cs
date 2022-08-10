using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyApplovinInterstitial : MonoBehaviour
{
    private static string adUnitId = "d9d406b1bf77bb32";
    int retryAttempt;

    public static MyUnityEventClass video_ads_watch = new MyUnityEventClass();
    public static MyUnityEventClass video_ads_started = new MyUnityEventClass();
    public static MyUnityEventClass video_ads_available = new MyUnityEventClass();
    public static MyUnityEventClass errorLoadAds = new MyUnityEventClass();

    private static bool isLoad;

    void Start()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialAdRevenuePaidEvent;


        // Load the first interstitial
        LoadInterstitial();
    }

    public static void LoadInterstitial()
    {
        isLoad = false;
        MaxSdk.LoadInterstitial(adUnitId);
    }

    public static void Show()
    {
        print("ShowInter");
        string sucsess = "not_available";
        if (MaxSdk.IsInterstitialReady(adUnitId))
        {
            MaxSdk.ShowInterstitial(adUnitId);
            sucsess = "success";
            if (video_ads_started != null)
                video_ads_started.Invoke("interstitial", "ad_on_replay", "start", Application.internetReachability.GetHashCode());
        }
        else
            LoadInterstitial();
        if (video_ads_available != null)
            video_ads_available.Invoke("interstitial", "ad_on_replay", sucsess, Application.internetReachability.GetHashCode());
    }

    
    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'
        isLoad = true;
        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        double retryDelay = Mathf.Pow(2, Mathf.Min(6, retryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);
        if (errorLoadAds != null)
            errorLoadAds.Invoke("interstitial", "ad_on_replay", errorInfo.Message, Application.internetReachability.GetHashCode());
    }

    private void OnInterstitialAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (video_ads_watch != null)
            video_ads_watch.Invoke("interstitial", "ad_on_replay", "watched", Application.internetReachability.GetHashCode());
        
        LoadInterstitial();
        ADSController.RestartInterstitial();
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadInterstitial();
        ADSController.RestartInterstitial();
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        if (video_ads_watch != null)
            video_ads_watch.Invoke("interstitial", "ad_on_replay", "fail_to_display", Application.internetReachability.GetHashCode());

        LoadInterstitial();
        ADSController.RestartInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (video_ads_watch != null)
            video_ads_watch.Invoke("interstitial", "ad_on_replay", "clicked", Application.internetReachability.GetHashCode());

        ADSController.RestartInterstitial();
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
        ADSController.RestartInterstitial();        

    }

}
