using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : Ultimate
{
    public GameObject healingParticle;
    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        currentLevel = MySceneManager.GetHealingLevel();
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
        MySceneManager.AddNextLevelHealing();
        UpdateUI();

    }

    public void UseHealing()
    {
        GameObject[] allTarget = GameObject.FindGameObjectsWithTag(targetTag);
        foreach (GameObject go in allTarget)
        {
            DamageBase damageBase = go.GetComponent<DamageBase>();
            if (damageBase.hp <= 0)
                continue;
            int b = (int)(startBuffValue + buffValue * currentLevel);
            damageBase.Healing(b);
            Instantiate(healingParticle, go.transform.position, Quaternion.identity);
        }
    }
}
