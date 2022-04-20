using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Button startFight;
    public Button addArrowman;
    public Button addMan;
    public Animator animator;
    private int anima = -1;

    private void Start()
    {
        if (MySceneManager.GetLevel() == 1)
        {
            startFight.interactable = false;
            addArrowman.interactable = false;
            addMan.onClick.AddListener(NextStep);
            startFight.onClick.AddListener(NextStep);
        }
        else
            Destroy(gameObject);
            
    }

    public void NextStep ()
    {
        anima++;
        animator.SetInteger("anima", anima);
    }

    public void Update ()
    {
        if (anima >= 2)
        {
            startFight.interactable = true;
        }
    }
}
