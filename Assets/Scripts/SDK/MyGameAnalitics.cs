
using UnityEngine;
using GameAnalyticsSDK;

public class MyGameAnalitics : MonoBehaviour
{
    public static bool start = false;

    private void Awake()
    {
        if (!start)
        {
            GameAnalytics.Initialize();
            start = true;
        }
    }

    void Start()
    {
        
        FightController.levelStartEvent.AddListener(StartLevel);
        FightController.levelWinEvent.AddListener(EndLevel);
        FightController.levelFailEvent.AddListener(FailLevel);
    }

    public static void StartLevel(int levelNum, string sceneName)
    {
       
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level " + levelNum);
        Debug.Log("Стартовал уровнеь " + PlayerPrefs.GetInt("level"));
    }

    public static void EndLevel(int levelNum, string sceneName)
    {
        // отправка события прогресса - старт уровня
        // в GAProgressionStatus есть варианты статуса
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level " + levelNum);
        Debug.Log("Пройден уровнеь " + PlayerPrefs.GetInt("level"));
    }

    public static void FailLevel(int levelNum, string sceneName)
    {
        // отправка события прогресса - старт уровня
        // в GAProgressionStatus есть варианты статуса
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "level " + levelNum);
        Debug.Log("Пройден уровнеь " + PlayerPrefs.GetInt("level"));
    }
    
}
