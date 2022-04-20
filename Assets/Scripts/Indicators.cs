using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RootMotion.Dynamics;

[Serializable]
public enum TypeMan 
    {
        man,
        arrowman
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

    private void Start()
    {
        GetComponent<DamageBase>().enabled = false;
        GetComponent<Controller>().enabled = false;
        transform.root.GetComponentInChildren<PuppetMaster>().mode = PuppetMaster.Mode.Kinematic;
    }

    [ContextMenu("EnableToFight")]
    public void EnabledToFight()
    {
        GetComponent<DamageBase>().enabled = true;
        GetComponent<Controller>().enabled = true;
        transform.root.GetComponentInChildren<PuppetMaster>().mode = PuppetMaster.Mode.Active;
        enabled = false;
    }
}
