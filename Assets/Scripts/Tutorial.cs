using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Button startFight;
    public Button addArrowman;
    public Button addMan;
    public Button freeMan;
    public Button freeArrowman;
    public Animator animator;
    private int anima = -1;

    private void Start()
    {
        if (MySceneManager.GetLevel() == 0)
        {
            startFight.interactable = false;
            addArrowman.interactable = false;
            addMan.onClick.AddListener(NextStep);
            startFight.onClick.AddListener(NextStep);
            freeArrowman.interactable = false;
            freeArrowman.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            freeMan.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            freeMan.interactable = false;
        }
        else
            Destroy(gameObject);
            
    }

    public void NextStep ()
    {
        anima++;
        animator.SetInteger("anima", anima);
        if (anima >= 1)
            addMan.interactable = false;
    }

    public void Update ()
    {
        if (anima >= 2)
        {
            startFight.interactable = true;
            
        }
    }
}
