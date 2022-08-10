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
    public struct userAttributes { }
    public struct appAttributes { }

    public static bool entryAdsButton;
    public float timerToAds = 30;
    private static float _timerToAds;
    private static float _timeToLastAds;
    public GameObject noAdsButton;
    public Lineup _lineup;
    private static bool _showAds;
    public static AdsType _adsType;

    public void EntryAdsTrue()
    {
        entryAdsButton = true;
    }

    private void Awake()
    {
        //PullRemoteConfiguration();
        /*
        if (Time.unscaledTime < 10)
        {
            if (MySceneManager.GetLevel() > 3)
                _showAds = true;
            else
                _showAds = false;
        }*/

        _timerToAds = timerToAds;
        
    }
    /*
    private void Update()
    {
        if (_showAds)
            ShowInterstitial();
    }
    */
    /*
    void PullRemoteConfiguration ()
    {
        ConfigManager.FetchCompleted += ApplyConfig;
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }

    void ApplyConfig(ConfigResponse configResponse)
    {
        switch(configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                timerToAds = 30;
                break;
            case ConfigOrigin.Cached:
                break;
            case ConfigOrigin.Remote:
                SetSettings();
                break;
        }
    }

    void SetSettings ()
    {
        _timerToAds = ConfigManager.appConfig.GetFloat("timeReloadAds");
    }
    */
    private void Start()
    {
        //_applovinInterstitial = FindObjectOfType<MyApplovinInterstitial>();
        //_applovinRewarded = FindObjectOfType<MyApplovinRewarded>();
        
        if (PlayerPrefs.GetInt("noads", 0) == 1)
        {
            noAdsButton.SetActive(false);
            return;
        }
        //if (MySceneManager.GetLevel() > 3)
        //    StartCoroutine(Timer());
    }

    public void OnRewardedShow (string type)
    {
        switch (type)
        {
            case "man":
                //_adsType = AdsType.add_man;
                MyApplovinRewarded.watchAds.AddListener(AddMan);
                break;
            case "arrowman":
                //_adsType = AdsType.add_arrowman;
                MyApplovinRewarded.watchAds.AddListener(AddArrowman);
                break;
            case "money":
                //_adsType = AdsType.add_money;
                MyApplovinRewarded.watchAds.AddListener(AddMoney);
                break;
            case "increase":
                //_adsType = AdsType.increase;
                MyApplovinRewarded.watchAds.AddListener(IncreaseMoney);
                break;
            case "rage":
                MyApplovinRewarded.watchAds.AddListener(AddRage);
                break;
            case "freezing":
                MyApplovinRewarded.watchAds.AddListener(AddFreezing);
                break;
            case "healing":
                MyApplovinRewarded.watchAds.AddListener(AddHealing);
                break;
        }
        MyApplovinRewarded.Show();
    }
    /*
    private void OnRewardedShow()
    {
        MyApplovinRewarded.watchAds.RemoveAllListeners();
        switch (_adsType)
        {
            case AdsType.add_man:
                MyApplovinRewarded.watchAds.AddListener(AddMan);
                break;
            case AdsType.add_arrowman:
                MyApplovinRewarded.watchAds.AddListener(AddArrowman);
                break;
            case AdsType.increase:
                MyApplovinRewarded.watchAds.AddListener(IncreaseMoney);
                break;
            case AdsType.add_money:
                MyApplovinRewarded.watchAds.AddListener(AddMoney);
                break;
        }

        MyApplovinRewarded.Show();      
        
    }*/

    void AddMan ()
    {
        if (_lineup == null)
            _lineup = FindObjectOfType<Lineup>();
        _lineup.AddNewManFree(true, 0);
        _lineup.AddNewManFree(true, 0);
    }

    void AddArrowman ()
    {
        if (_lineup == null)
            _lineup = FindObjectOfType<Lineup>();
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
        money *= 4;//Roulette.rouletteCoeffisient;
        MySceneManager.AddMoney(money);
        FindObjectOfType<Lineup>().UpdateUI();
    }

    void AddRage()
    {
        MySceneManager.AddNextLevelRage();
        if (TryGetComponent(out Rage hl))
            hl.UpdateUI();
    }

    void AddHealing ()
    {
        MySceneManager.AddNextLevelHealing();
        if (TryGetComponent(out Healing hl))
            hl.UpdateUI();
    }

    void AddFreezing()
    {
        MySceneManager.AddNextLevelFreezing();
        if (TryGetComponent(out Freezing hl))
            hl.UpdateUI();
    }

    public void StopAds ()
    {
        PlayerPrefs.SetInt("noads", 1);
        //StopCoroutine(Timer());
        noAdsButton.SetActive(false);
    }

    public static void ShowInterstitial ()
    {
        print(Time.unscaledTime - _timeToLastAds);
        //if (_showAds == false)
        //   return;
        if ((Time.unscaledTime - _timeToLastAds) < _timerToAds)        
            return;
        if (MySceneManager.GetLevel() >= 3)
        {
            MyApplovinInterstitial.Show();            
        }

    }

    public static void RestartInterstitial ()
    {
        _timeToLastAds = Time.unscaledTime;
    }

}
