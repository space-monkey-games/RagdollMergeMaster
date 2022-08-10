using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roulette : MonoBehaviour
{
    public Text xText;
    public Button adsButton;
    public GameObject notLoad;
    Transform tr;
    public static int rouletteCoeffisient;


    private void Start()
    {
        tr = transform;
    }

    private void OnEnable()
    {
        //if (MyApplovinRewarded.rewardedAsLoaded == false)
        //    transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        int i = (int)Mathf.Abs(tr.localRotation.z*10);
        switch(i)
        {
            case 10:
            case 2:
            case 1:
            case 0:
            case 9:
                rouletteCoeffisient = 2;
                break;
            case 4:
            case 3:
                rouletteCoeffisient = 3;
                break;
            case 7:
            case 6:
                rouletteCoeffisient = 5;
                break;            
            case 8:
                rouletteCoeffisient = 4;
                break;
        }
        xText.text = "X" + rouletteCoeffisient; 
    }

    public void Reward ()
    {
        if (MyApplovinRewarded.rewardedAsLoaded == false)
            notLoad.SetActive(true);
        else
            adsButton.interactable = false;
    }
}
