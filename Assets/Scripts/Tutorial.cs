using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Button startFight;
    public Button addArrowman;
    public Animator animator;
    private int anima = -1;

    private void Start()
    {
        if (MySceneManager.GetLevel() == 1)
        {
            startFight.interactable = false;
            addArrowman.interactable = false;
        }
        else
            Destroy(gameObject);
            
    }

    public void Update ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            anima++;
            animator.SetInteger("anima", anima);
        }
        if (anima >= 2)
        {
            startFight.interactable = true;
        }
    }
}
