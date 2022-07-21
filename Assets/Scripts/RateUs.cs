using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateUs : MonoBehaviour
{
    static float lastTime;
    public float nextTime = 60;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("rateus", 0) == 1)
            Destroy(this);
    }

    private void Start()
    {
        lastTime = Time.time;
        print(lastTime);
        if (lastTime >= nextTime)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            nextTime = 120000;
        }
    }

    public void Rate (Image img)
    {
        if (img.fillAmount == 1)
        {
            Application.OpenURL("market://details?id=ragdoll.merge.masters");
        }
        PlayerPrefs.SetInt("rateus", 1);
    }
}
