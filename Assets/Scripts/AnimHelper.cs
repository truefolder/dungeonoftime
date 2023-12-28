using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHelper : MonoBehaviour
{
    public void OnAnimationEnded()
    {
        gameObject.SetActive(false);
    }
}
