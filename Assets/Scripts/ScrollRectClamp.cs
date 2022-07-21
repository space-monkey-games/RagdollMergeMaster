using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectClamp : MonoBehaviour
{
    public Vector2 maxClamp = new Vector2(0, 650);
    public bool horizontall;
    public bool vertical;
    RectTransform currentRect;

    private void Start()
    {
        currentRect = transform as RectTransform;
    }

    public void ClampScroll ()
    {
        
        if (vertical)
        {
            Vector2 clampPos = new Vector2(maxClamp.x, maxClamp.y);
            currentRect.localPosition = new Vector2(currentRect.localPosition.x, Mathf.Clamp(currentRect.localPosition.y, clampPos.x, clampPos.y));
        }
        if (horizontall)
        {
            Vector2 clampPos = new Vector2(0, currentRect.rect.width/2);
            currentRect.localPosition = new Vector2(Mathf.Clamp(currentRect.localPosition.x, clampPos.x, clampPos.y), currentRect.localPosition.y);
        }
    }

}
