using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LampController : MonoBehaviour, IRewindable
{
    public float startFuelInSeconds = 90;
    public float fuelLeftInSeconds;

    public static LampController instance;
    public Image fuelProgressBar;
    private void Awake()
    {
        instance = this;
        fuelLeftInSeconds = startFuelInSeconds;
    }

    private void Update()
    {
        if (fuelLeftInSeconds > 0)
        {
            fuelLeftInSeconds -= Time.deltaTime;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        fuelProgressBar.fillAmount = fuelLeftInSeconds / startFuelInSeconds;
    }

    private LinkedList<float> fuelLeft = new();
    public void Record()
    {
        fuelLeft.AddFirst(fuelLeftInSeconds);
    }

    public void Rewind()
    {
        fuelLeftInSeconds = fuelLeft.First.Value;
        UpdateUI();
        fuelLeft.RemoveFirst();
    }

    public void RemoveLast()
    {
        fuelLeft.RemoveLast();
    }
}
