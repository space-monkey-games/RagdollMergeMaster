using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;



public class TimeSystem : MonoBehaviour
{
    public GameObject root;
    public GameObject[] allBonus = new GameObject[0];

    private void Start()
    {
        CheckTime();
    }

    public static TimeSpan GetTime()
    {
        TimeSpan ts;
        if (PlayerPrefs.HasKey("lastsession"))
        {
            string s = PlayerPrefs.GetString("lastsession");
            ts = DateTime.Now - DateTime.Parse(s);
        }
        else
            ts = new TimeSpan(0, 0, 0, 0);
        PlayerPrefs.SetString("lastsession", DateTime.Now.ToString());
        return ts;
    }

    void CheckTime ()
    {
        TimeSpan ts = GetTime();
        
        if (ts.Days == 1)
        {
            ActivateDailyBonus();
        }
        else
        {
            if (ts.Days > 1)
            {
                PlayerPrefs.DeleteKey("daylibonus");
                ActivateDailyBonus();
            }
        }
    }

    [ContextMenu("ActivateDailyBonus")]
    public void ActivateDailyBonus ()
    {
        int i = PlayerPrefs.GetInt("daylibonus", 0);
        if (i > 6)
        {
            PlayerPrefs.DeleteKey("daylibonus");
            i = 0;
        }
        allBonus[i].SetActive(true);
        root.SetActive(true);
        switch (i)
        {
            case 2:
                FindObjectOfType<Lineup>().AddNewManFree(true, 4);
                break;
            case 6:
                FindObjectOfType<Lineup>().AddNewManFree(false, 7);
                break;
            default:
                MySceneManager.AddMoney(MySceneManager.GetMidleCount());
                string st = Lineup.Convert(MySceneManager.GetMidleCount());
                allBonus[i].GetComponentInChildren<Text>().text = st;
                break;

        }
        i++;
        PlayerPrefs.SetInt("daylibonus", i);
    }

}
