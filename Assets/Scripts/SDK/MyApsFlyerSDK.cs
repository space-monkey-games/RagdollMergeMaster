using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK; 

public class MyApsFlyerSDK : MonoBehaviour
{
    static int level;

    private void Start()
    {
        
        FightController.levelStartEvent.AddListener(StartLevel);
        FightController.levelWinEvent.AddListener(WinLevel);
        FightController.levelFailEvent.AddListener(FailLevel);
    }
    
    public void StartLevel () 
    {
        level = MySceneManager.GetLevelCount();
        Dictionary<string, string> eventParameters0 = new Dictionary<string, string>();
        eventParameters0.Add(AFInAppEvents.LEVEL, level.ToString());
        
        AppsFlyer.sendEvent(AFInAppEvents.LEVEL_ACHIEVED, eventParameters0);
        print("ApsFlyer Start Level " + level);
    }

    public void WinLevel()
    {
        Dictionary<string, string> eventParameters0 = new Dictionary<string, string>();
        eventParameters0.Add(AFInAppEvents.LEVEL, level.ToString());

        AppsFlyer.sendEvent("LEVEL_WIN", eventParameters0);
        print("ApsFlyer Win Level " + level);
    }

    public void FailLevel()
    {
        Dictionary<string, string> eventParameters0 = new Dictionary<string, string>();
        eventParameters0.Add(AFInAppEvents.LEVEL, level.ToString());

        AppsFlyer.sendEvent("LEVEL_FAIL", eventParameters0);
        print("ApsFlyer Fail Level " + level);
    }

}
