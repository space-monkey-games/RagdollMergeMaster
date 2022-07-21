using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test : MonoBehaviour
{
    public UnityEvent attackEvent = new UnityEvent();
    public DamageBase dmg;


    public void Attack()
    {
        if (attackEvent != null)
            attackEvent.Invoke();
    }

    public void AllDeath ()
    {
        DamageBase[] dmg = FindObjectsOfType<DamageBase>();
        foreach (DamageBase d in dmg)
            if (d.gameObject.CompareTag("Player"))
                d.Dead();
    }

}
