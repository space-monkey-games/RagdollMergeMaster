using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RootMotion.Dynamics;

[Serializable]
public enum TypeMan 
    {
        man,
        arrowman,
        boss
    }

public class Indicators : MonoBehaviour
{

    public float pin;
    [ExecuteInEditMode]
    [ContextMenu("Spring")]
    public void Spring ()
    {
        transform.root.GetComponentInChildren<PuppetMaster>().pinDistanceFalloff = pin;
    }



    public TypeMan typeMan;
    public int level;
    public static int lastOpenMan = 0;
    public static int lastOpenArrowman = 0;

    private void Start()
    {
        if (lastOpenMan == 0)
            lastOpenMan = PlayerPrefs.GetInt("lastopenman", 0);
        if (lastOpenArrowman == 0)
            lastOpenArrowman = PlayerPrefs.GetInt("lastopenarrowman", 0);

        if (GetComponent<DamageBase>().useActiveRagdol)
            transform.root.GetComponentInChildren<PuppetMaster>().mode = PuppetMaster.Mode.Kinematic;

        if (gameObject.CompareTag("Player"))
        {
            if (typeMan == TypeMan.man && level > lastOpenMan)
            {
                FindObjectOfType<Bestiary>().OpenNewMan(level);
                return;
            }
            if (typeMan == TypeMan.arrowman && level > lastOpenArrowman)
                FindObjectOfType<Bestiary>().OpenNewArrowman(level);
        }
    }

    [ContextMenu("EnableToFight")]
    public void EnabledToFight()
    {
        GetComponent<DamageBase>().enabled = true;
        GetComponent<Controller>().enabled = true;
        if (GetComponent<DamageBase>().useActiveRagdol)
            transform.root.GetComponentInChildren<PuppetMaster>().mode = PuppetMaster.Mode.Active;
        enabled = false;
    }
}
