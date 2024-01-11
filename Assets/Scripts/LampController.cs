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

    private void Start()
    {
        TimeController.instance.rewindables.Add(new TNRD.SerializableInterface<IRewindable>(this));
    }

    private void Update()
    {
        if (!LevelController.instance.levelStarted)
            return;

        if (fuelLeftInSeconds > 0)
        {
            fuelLeftInSeconds -= Time.deltaTime;
            UpdateUI();
        }
        else
            LevelController.instance.FailLevel("Подземелье поглотило вас во тьму.");
    }

    public void ResetFuel()
    {
        fuelLeftInSeconds = startFuelInSeconds;
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
