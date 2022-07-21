using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAppMetrica : MonoBehaviour
{
    //Порядковый номер игры для пользователя (параметр, который обновляется каждый старт уровня, то есть показывает какое количество игр пользователь сыграл за все время жизни) - Начинается с 1
    static int level_count = 1;
    static int level;

    private void Awake()
    {
        FightController.levelStartEvent.AddListener(StartLevel);
        FightController.levelWinEvent.AddListener(WinLevel);
        ADSController.onStartAds.AddListener(VideoAdsStarted);
        ADSController.onWatchAds.AddListener(VideoAdsWatch);
        ADSController.onAvailableAds.AddListener(VideoAdsAvailable);
    }


    public static void StartLevel()
    {
        level = MySceneManager.GetLevel();
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("level", level);
        parameters.Add("level_count", level_count);
        AppMetrica.Instance.ReportEvent("level_start", parameters);
        AppMetrica.Instance.SendEventsBuffer();
        level_count++;
        print("App_Metrica старт уровня " + level + "попытка " + level_count);
    }

    public static void WinLevel()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("level", level);
        AppMetrica.Instance.ReportEvent("level_finish", parameters);
        AppMetrica.Instance.SendEventsBuffer();
        print("App_Metrica победа на уровне " + level);
    }


    public static void VideoAdsAvailable(string ad_type, string placement, string result, int connection)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("ad_type", ad_type);
        parameters.Add("placement", placement);
        parameters.Add("result", result);
        parameters.Add("connection", connection);
        AppMetrica.Instance.ReportEvent("video_ads_available", parameters);
        print($"App_Metrica реклама загрузка {ad_type}, {placement}, {result}, {connection}");
    }

    public static void VideoAdsStarted(string ad_type, string placement, string result, int connection)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("ad_type", ad_type);
        parameters.Add("placement", placement);
        parameters.Add("result", result);
        parameters.Add("connection", connection);
        AppMetrica.Instance.ReportEvent("video_ads_started", parameters);
        print($"App_Metrica реклама старт {ad_type}, {placement}, {result}, {connection}" );
    }

    

    public static void VideoAdsWatch(string ad_type, string placement, string result, int connection)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("ad_type", ad_type);
        parameters.Add("placement", placement);
        parameters.Add("result", result);
        parameters.Add("connection", connection);
        AppMetrica.Instance.ReportEvent("video_ads_watch", parameters);
        print($"App_Metrica реклама финиш {ad_type}, {placement}, {result}, {connection}");
    }
}
