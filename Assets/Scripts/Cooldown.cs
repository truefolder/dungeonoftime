using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Cooldown : MonoBehaviour
{
    public float cooldownTimeInSeconds;
    public Image cooldownProgressBar;

    public bool canUse = true;
    public float currentTime;

    public void StartCooldown()
    {
        canUse = false;
        currentTime = 0;
    }

    public void CooldownUpdate()
    {
        if (canUse)
            return;
        currentTime += Time.deltaTime;
        UpdateCooldownUI();
        if (currentTime >= cooldownTimeInSeconds)
            canUse = true;
    }

    private void UpdateCooldownUI()
    {
        cooldownProgressBar.fillAmount = 1 - (currentTime / cooldownTimeInSeconds);
    }

}
