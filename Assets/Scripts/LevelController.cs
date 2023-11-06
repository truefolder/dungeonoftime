using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public float levelTimeInSeconds = 300f;

    public static LevelController instance;

    public Text timerText;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        levelTimeInSeconds -= Time.fixedDeltaTime;

        timerText.text = FormatSeconds(levelTimeInSeconds);
    }

    private string FormatSeconds(float seconds)
    {
        var minutes = seconds / 60;
        seconds %= 60;
        return string.Format("{0:d2}:{1:d2}", Mathf.RoundToInt(Mathf.Floor(minutes)), Mathf.RoundToInt(Mathf.Floor(seconds)));
    }
}
