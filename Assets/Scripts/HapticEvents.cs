using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class HapticEvents : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        DamageBase.deadEvent.AddListener(HapticPlay);
        Lineup.mergeEvent.AddListener(HapticPlay);
    }

    public void HapticPlay ()
    {
        MMVibrationManager.Haptic(HapticTypes.Success);

    }
}
