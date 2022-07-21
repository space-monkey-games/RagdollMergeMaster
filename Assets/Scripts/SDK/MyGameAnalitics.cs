
using UnityEngine;
using GameAnalyticsSDK;

public class MyGameAnalitics : MonoBehaviour
{
    public static bool start = false;
    static int level;

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

    public static void StartLevel()
    {
        level = MySceneManager.GetLevel();  
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level " + level);
        Debug.Log("Game_Analitics cтартовал уровнеь " + level);
    }

    public static void EndLevel()
    {
        // отправка события прогресса - старт уровня
        // в GAProgressionStatus есть варианты статуса
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level " + level);
        Debug.Log("Game_Analitics Пройден уровнеь " + level);
    }

    public static void FailLevel()
    {
        // отправка события прогресса - старт уровня
        // в GAProgressionStatus есть варианты статуса
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "level " + level);
        Debug.Log("Game_Analitics проигран уровнеь " + level);
    }
    
}
