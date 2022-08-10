using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ultimate : MonoBehaviour
{
    public Text secondMoneyText;
    public GameObject buttonFreeUse;
    public Text levelText;
    public Text countText;
    public int countStep = 1;
    public int currentCount;
    public int currentLevel;
    public string targetTag;
    public float startBuffValue;
    public float buffValue;

    private void Start()
    {
        UpdateSecondMoney();
    }

    public void UpdateSecondMoney ()
    {
        secondMoneyText.text = MySceneManager.GetSecondMoney().ToString();
    }
}
