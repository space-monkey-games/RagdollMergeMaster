using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bestiary : MonoBehaviour
{
    public GameObject allPopapMan;
    public GameObject allPopapArrowman;
    [Space]
    public GameObject allManInBestiary;
    public GameObject allArrowmanInBestiary;



    public void OpenNewMan (int level)
    {
        ClearMan();
        Indicators.lastOpenMan = level;
        PlayerPrefs.SetInt("lastopenman", level);
        allPopapMan.SetActive(true);
        allPopapMan.transform.GetChild(level).gameObject.SetActive(true);
    }

    public void OpenNewArrowman (int level)
    {
        ClearArrowman();
        Indicators.lastOpenArrowman = level;
        PlayerPrefs.SetInt("lastopenarrowman", level);
        allPopapArrowman.SetActive(true);
        allPopapArrowman.transform.GetChild(level).gameObject.SetActive(true);
    }

    public void CheckBestiary ()
    {
        int m = PlayerPrefs.GetInt("lastopenman");
        int a = PlayerPrefs.GetInt("lastopenarrowman");
        for (int i = 0; i <= m; i++)
        {
            allManInBestiary.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
        }
        for (int i = 0; i <= a; i++)
        {
            allArrowmanInBestiary.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
        }
    }
    
    void ClearMan ()
    {
        
        foreach (Transform tr in allPopapMan.transform)
        {
            tr.gameObject.SetActive(false);
        }
    }

    void ClearArrowman ()
    {
        foreach (Transform tr in allPopapArrowman.transform)
        {
            tr.gameObject.SetActive(false);
        }
    }
}
