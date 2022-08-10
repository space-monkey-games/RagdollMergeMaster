using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezing : Ultimate
{
    public Material freezeMaterial;
    public GameObject freezeParticle;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        currentLevel = MySceneManager.GetFreezingLevel();
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
        MySceneManager.AddNextLevelFreezing();
        UpdateUI();

    }

    public void UseFreeze()
    {
        GameObject[] allTarget = GameObject.FindGameObjectsWithTag(targetTag);
        foreach (GameObject go in allTarget)
        {
            float b = startBuffValue + buffValue * currentLevel;
            go.GetComponent<Controller>().Freezing(freezeMaterial ,b);
            Instantiate(freezeParticle, go.transform.position, Quaternion.identity);
        }
    }

}
