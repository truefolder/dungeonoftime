using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class LevelController : MonoBehaviour, IRewindable
{
    public float levelTimeInSeconds = 300f;

    public static LevelController instance;

    public Key[] keys;

    public Text timerText;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        levelTimeInSeconds -= Time.fixedDeltaTime;
        if (levelTimeInSeconds <= 0)
        {
            FailLevel();
            return;
        }
        timerText.text = FormatSeconds(levelTimeInSeconds);
    }

    private string FormatSeconds(float seconds)
    {
        var minutes = seconds / 60;
        seconds %= 60;
        return string.Format("{0:d2}:{1:d2}", Mathf.RoundToInt(Mathf.Floor(minutes)), Mathf.RoundToInt(Mathf.Floor(seconds)));
    }

    public void AddKey(string keyName)
    {
        keys.Where(x => x.name == keyName).First().count += 1;
        UpdateKeyUI();
    }

    public bool RemoveKey(string keyName)
    {
        var keyCount = keys.Where(x => x.name == keyName).First().count;
        if (keyCount == 0)
            return false;

        keyCount -= 1;
        UpdateKeyUI();

        return true;
    }

    public void UpdateKeyUI()
    {
        foreach(var key in keys)
            key.textCount.text = $"x{key.count.ToString()}";
    }

    private void FailLevel()
    {

    }


    private LinkedList<float> levelTime = new();

	public void Record()
	{
        levelTime.AddFirst(levelTimeInSeconds);
	}

	public void Rewind()
	{
        levelTimeInSeconds = levelTime.First.Value;
        levelTime.RemoveFirst();
    }

    public void RemoveLast()
	{
        levelTime.RemoveLast();
	}
}
