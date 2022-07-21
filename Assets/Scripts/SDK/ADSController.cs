using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public enum AdsType
{
    add_man,
    add_arrowman,
    increase,
    add_money
}


[Serializable]
public class MyUnityEventClass : UnityEvent<string, string, string, int> { }

public class ADSController : MonoBehaviour
{
    public float timerToAds = 45;
    public GameObject noAdsButton;
    private static float _timeToAds;
    private Lineup _lineup;

    public static MyUnityEventClass onAvailableAds = new MyUnityEventClass();
    public static MyUnityEventClass onStartAds = new MyUnityEventClass();
    public static MyUnityEventClass onWatchAds = new MyUnityEventClass();


    static MyApplovinInterstitial _applovinInterstitial;
    static MyApplovinRewarded _applovinRewarded;

    private AdsType _adsType;

    private void Start()
    {
        _applovinInterstitial = FindObjectOfType<MyApplovinInterstitial>();
        _applovinRewarded = FindObjectOfType<MyApplovinRewarded>();
        _lineup = FindObjectOfType<Lineup>();
        if (PlayerPrefs.GetInt("noads", 0) == 1)
        {
            noAdsButton.SetActive(false);
            return;
        }
        if (MySceneManager.GetLevel() > 3)
            StartCoroutine(Timer());
    }

    public void OnRewardedShow (string type)
    {
        switch (type)
        {
            case "man":
                _adsType = AdsType.add_man;
                break;
            case "arrowman":
                _adsType = AdsType.add_arrowman;
                break;
            case "money":
                _adsType = AdsType.add_money;
                break;
            case "increase":
                _adsType = AdsType.increase;
                break;
        }
        OnStartAds();
    }

    void OnStartAds ()
    {
        string b = "error";
        if (_applovinRewarded.IsAdsReady())
        {            
            b = "success";
        }
        else
            b = "not_available";
        OnRewardedShow();
        if (onAvailableAds != null)
            onAvailableAds.Invoke("rewarded", _adsType.ToString(), b, Application.internetReachability.GetHashCode());

    }

    private void OnRewardedShow ()
    {
        _applovinRewarded.watchAds.RemoveAllListeners();
        switch (_adsType)
        {
            case AdsType.add_man:
                _applovinRewarded.watchAds.AddListener(AddMan);
                break;
            case AdsType.add_arrowman:
                _applovinRewarded.watchAds.AddListener(AddArrowman);
                break;
            case AdsType.increase:
                _applovinRewarded.watchAds.AddListener(IncreaseMoney);
                break;
            case AdsType.add_money:
                _applovinRewarded.watchAds.AddListener(AddMoney);
                break;
        }
        
        _applovinRewarded.watchAds.AddListener(WatchRewarded);
        _applovinRewarded.Show();
        _timeToAds = 45;
        if (onStartAds != null)
            onStartAds.Invoke("rewarded", _adsType.ToString(), "start", Application.internetReachability.GetHashCode());
    }

    public static void OnInterstitialShow ()
    {
        _applovinInterstitial.Show();
        _timeToAds = 45;
        if (onStartAds != null)
            onStartAds.Invoke("interstitial", "ad_on_replay", "start", Application.internetReachability.GetHashCode());
    }

    public void WatchRewarded ()
    {
        if (onWatchAds != null)
            onWatchAds.Invoke("rewarded", _adsType.ToString(), "watched", Application.internetReachability.GetHashCode());
    }

    void AddMan ()
    {
        _lineup.AddNewManFree(true, 0);
        _lineup.AddNewManFree(true, 0);
    }

    void AddArrowman ()
    {
        _lineup.AddNewManFree(false, 0);
        _lineup.AddNewManFree(false, 0);
    }

    void AddMoney ()
    {
        //print("add money");
        MySceneManager.AddMoney(MySceneManager.GetMidleCount());
        FindObjectOfType<Lineup>().UpdateUI();
    }

    void IncreaseMoney ()
    {
        //print("increase_money");
        int money = 0;
        try
        {
            money = GetComponent<FightController>().moneyPerLevel;
        }
        catch
        {
            money = FindObjectOfType<FightController>().moneyPerLevel;
        }
        money *= Roulette.rouletteCoeffisient;
        MySceneManager.AddMoney(money);
        FindObjectOfType<Lineup>().UpdateUI();
    }

    public void StopAds ()
    {
        PlayerPrefs.SetInt("noads", 1);
        StopCoroutine(Timer());
        noAdsButton.SetActive(false);
    }

    IEnumerator Timer ()
    {
        while (true)
        {
            if (_timeToAds > 0)
                _timeToAds -= 1;
            else
            {
                OnInterstitialShow();
                _timeToAds = timerToAds;
            }
            yield return new WaitForSeconds(1.0f);
            
        }
    }

}
