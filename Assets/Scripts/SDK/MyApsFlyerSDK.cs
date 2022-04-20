using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK; 

public class MyApsFlyerSDK : MonoBehaviour
{
    private void Start()
    {
        FightController.levelStartEvent.AddListener(StartLevel);
        FightController.levelWinEvent.AddListener(WinLevel);
        FightController.levelFailEvent.AddListener(FailLevel);
    }
    
    public void StartLevel (int levelNum, string sceneName) 
    {
        Dictionary<string, string> eventParameters0 = new Dictionary<string, string>();
        eventParameters0.Add(AFInAppEvents.LEVEL, levelNum.ToString());
        
        AppsFlyer.sendEvent(AFInAppEvents.LEVEL_ACHIEVED, eventParameters0);
        print("ApsFlyer Start Level");
    }

    public void WinLevel(int levelNum, string sceneName)
    {
        Dictionary<string, string> eventParameters0 = new Dictionary<string, string>();
        eventParameters0.Add(AFInAppEvents.LEVEL, levelNum.ToString());

        AppsFlyer.sendEvent("LEVEL_WIN", eventParameters0);
        print("ApsFlyer Win Level");
    }

    public void FailLevel(int levelNum, string sceneName)
    {
        Dictionary<string, string> eventParameters0 = new Dictionary<string, string>();
        eventParameters0.Add(AFInAppEvents.LEVEL, levelNum.ToString());

        AppsFlyer.sendEvent("LEVEL_FAIL", eventParameters0);
        print("ApsFlyer Fail Level");
    }

}
