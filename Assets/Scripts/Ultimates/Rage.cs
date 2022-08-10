using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : Ultimate
{
    public float activeTime = 5f;
    public GameObject rageParticle;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI ()
    {
        currentLevel = MySceneManager.GetRageLevel();
        currentCount = currentLevel * countStep;
        countText.text = currentCount.ToString();
        levelText.text = "LVL " + currentLevel.ToString();
        if (currentCount <= MySceneManager.GetSecondMoney())
        {
            buttonFreeUse.SetActive(false);
        }
        else
        {
            buttonFreeUse.SetActive(true);
        }
        UpdateSecondMoney();
    }

    public void BuyUltimate()
    {
        if (currentCount > MySceneManager.GetSecondMoney())
            return;
        MySceneManager.AddSecondMoney(-currentCount);
        MySceneManager.AddNextLevelRage();
        UpdateUI();
    }

    public void UseRage ()
    {
        GameObject [] allTarget = GameObject.FindGameObjectsWithTag(targetTag);
        List<DamageBase> allDB = new List<DamageBase>();
        foreach (GameObject go in allTarget)
        {
            DamageBase db = go.GetComponent<DamageBase>();
            allDB.Add(db);
            float b = db.damage * (startBuffValue + buffValue * currentLevel)/ 100;
            db.damage +=(int) b;
            db.transform.root.localScale = db.transform.root.localScale * 1.5f;
            Instantiate(rageParticle, go.transform.position, Quaternion.identity);
        }
        StartCoroutine(EndUse(allDB));
    }
    
    IEnumerator EndUse (List<DamageBase> allTarget)
    {
        yield return new WaitForSeconds(activeTime);
        foreach (DamageBase go in allTarget)
        {
            float b = go.damage * (startBuffValue + buffValue * currentLevel) / 100;
            go.damage -= (int)b;
            go.transform.root.localScale = go.transform.root.localScale / 1.5f;
        }
    }
}
