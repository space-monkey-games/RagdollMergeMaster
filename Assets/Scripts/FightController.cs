using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable]
public class Bot
{
    public DamageBase damageBase; 
    public Image hpImage;
    public Transform hpTransform;
    public float offcet;
    public float hpMax;


    public Bot (DamageBase _damageBase, Transform _hpTransform)
    {
        damageBase = _damageBase;
        hpTransform = _hpTransform;
        hpImage = _hpTransform.GetChild(0).GetComponent<Image>();
        offcet = damageBase.offcetForUI;
        hpMax = damageBase.hp;
    }
}

public class FightController : MonoBehaviour
{
    [Tooltip("Боевая камера")]
    public CinemachineVirtualCamera fightCamera;
    [Tooltip("панель кнопок управления армией")]
    public GameObject buttonsPanel;
    [Tooltip("Объект сетки")]
    public GameObject grid;
    [Tooltip("Список всех ботов")]
    public List<Bot> allBots = new List<Bot>();
    public AudioClip finishAudioClip;
    //заработанные за уровень деньги
    public int moneyPerLevel;
    public GameObject menuWin;
    public GameObject menuLoss;
    public Text youEarned;
    private Camera _myCamera;

    public static UnityEvent levelStartEvent = new UnityEvent();
    public static UnityEvent levelWinEvent = new UnityEvent();
    public static UnityEvent levelFailEvent = new UnityEvent();

    private void Start()
    {
        _myCamera = Camera.main;
        if (levelStartEvent != null)
            levelStartEvent.Invoke();
    }



    public void StartFight ()
    {
        GetComponent<AudioSource>().Play();
        Indicators[] allController = FindObjectsOfType<Indicators>();
        Transform uiRoot = FindObjectOfType<Canvas>().transform;
        foreach (Indicators c in allController)
        {

            Transform hpBar = c.GetComponent<DamageBase>().hpBar.transform;
            hpBar.SetParent(uiRoot);

            Bot b = new Bot(c.GetComponent<DamageBase>(), hpBar);
            allBots.Add(b);            
            c.EnabledToFight();            
        }
        foreach (Bot bot in allBots)
        {
            bot.damageBase.target = FindTarget(bot.damageBase);
        }
        fightCamera.Priority = 99;
        buttonsPanel.SetActive(false);
        GetComponent<Lineup>().enabled = false;
        grid.SetActive(false);

        SaveAndLoad gameSave = GetComponent<SaveAndLoad>();        
        gameSave.SaveMySave();
        GameObject.FindGameObjectWithTag("Box").SetActive(false);
    }

    public void BotDead (DamageBase bot)
    {
        CheckAllHp();
        ReplaceAllTargets();
        if (!bot.gameObject.CompareTag("Player"))
        {
            //int n = bot.GetComponent<Indicators>().level;
            //moneyPerLevel += 50 * (int)Mathf.Pow(2, (n - 1));
        }
        foreach (Bot bt in allBots)
            if (bt.damageBase == bot)
            {
                bt.hpTransform.gameObject.SetActive(false);
                break;
            }
    }

    public DamageBase FindTarget (DamageBase current)
    {
        DamageBase target = null;
        float distToTarget = 1000000;

        if (current.CompareTag("Player"))
        {
            foreach (Bot tr in allBots)
            {
                if (tr.damageBase.tag != "Player" && tr.damageBase.hp > 0)
                {
                    float dist = Distance(current.transform, tr.damageBase.transform);
                    if (dist < distToTarget)
                    {
                        target = tr.damageBase;
                        distToTarget = dist;
                    }
                }
            }
        }
        else
        {
            foreach (Bot tr in allBots)
            {
                if (tr.damageBase.CompareTag("Player") && tr.damageBase.hp > 0)
                {
                    float dist = Distance(current.transform, tr.damageBase.transform);
                    if (dist < distToTarget)
                    {
                        target = tr.damageBase;
                        distToTarget = dist;
                    }
                }
            }
        }
        return target;
        
    }

    public void CheckAllHp()
    {
        int blueHp = 0, redHp = 0;
        foreach (Bot dmg in allBots)
        {
            if (dmg.damageBase.CompareTag("Player"))
                blueHp += dmg.damageBase.hp;
            else
                redHp += dmg.damageBase.hp;
        }

        if (blueHp <= 0 && redHp > 0)
        {
            Failed();
            return;
        }
        else
        {
            if (redHp <= 0 && blueHp > 0)
            {
                Winner();
                return;
            }
        }
        

    }

    public void ReplaceAllTargets ()
    {
        foreach (Bot d in allBots)
            d.damageBase.target = FindTarget(d.damageBase);
    }

    void Winner ()
    {
        

        MySceneManager.AddNextLevel();
        moneyPerLevel = (int)(Math.Max(MySceneManager.GetArrowmanCount(), MySceneManager.GetManCount()) * 2f);
        MySceneManager.AddMoney(moneyPerLevel);
        StartCoroutine(YouEarned(true));
        GetComponent<AudioSource>().PlayOneShot(finishAudioClip);
        if (levelWinEvent != null)
            levelWinEvent.Invoke();
        
    }

    void Failed ()
    {
        moneyPerLevel = (int)(Math.Min(MySceneManager.GetArrowmanCount(), MySceneManager.GetManCount())*1.1f);
        //moneyPerLevel = Math.Max(moneyPerLevel, minCount);
        MySceneManager.AddMoney(moneyPerLevel);
        StartCoroutine(YouEarned(false));

        if (levelFailEvent != null)
            levelFailEvent.Invoke();
    }

    IEnumerator YouEarned (bool win)
    {
        yield return new WaitForSeconds(1f);
        foreach (Bot bot in allBots)
            if (bot.hpTransform.gameObject.activeSelf)
            {
                bot.hpTransform.gameObject.SetActive(false);
            }

        youEarned.text = Lineup.Convert(moneyPerLevel);
        if (win)
            menuWin.SetActive(true);
        else
            menuLoss.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        youEarned.transform.parent.parent.gameObject.SetActive(true);
        while(MyApplovinRewarded.rewardedAsLoaded)
        {
            youEarned.text = Lineup.Convert(moneyPerLevel*Roulette.rouletteCoeffisient);
            yield return null;
        }
    }

    public static float Distance (Transform start, Transform target)
    {
        if (start == null | target == null)
            return 0;
        float dist = (start.position - target.position).magnitude;
        return dist;
    }

    private void LateUpdate()
    {
        if (allBots.Count <= 0)
            return;
        foreach (Bot bot in allBots)
        {
            if (bot.hpTransform.gameObject.activeSelf)
            {
                bot.hpTransform.position = _myCamera.WorldToScreenPoint(bot.damageBase.transform.position + new Vector3(0, bot.offcet, 0));
                bot.hpImage.fillAmount = (float)bot.damageBase.hp / (float)bot.hpMax;
            }

        }
    }


}
