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
        var key = keys.Where(x => x.name == keyName).First();
        if (key.count == 0)
            return false;

        key.count -= 1;
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
    private LinkedList<int[]> keysCount = new();
	public void Record()
	{
        levelTime.AddFirst(levelTimeInSeconds);
        keysCount.AddFirst(keys.Select(a => a.count).ToArray());
	}

	public void Rewind()
	{
        levelTimeInSeconds = levelTime.First.Value;
        for (int i = 0; i < keysCount.First.Value.Length; ++i)
            keys[i].count = keysCount.First.Value[i];
        UpdateKeyUI();
        keysCount.RemoveFirst();
        levelTime.RemoveFirst();
    }

    public void RemoveLast()
	{
        keysCount.RemoveLast();
        levelTime.RemoveLast();
	}
}
