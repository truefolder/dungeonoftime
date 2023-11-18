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

    private GameObject pickedUpItem;
    private bool isItemPickedUp;

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

    private void Update()
    {
        if (isItemPickedUp && Input.GetKeyDown(KeyCode.F))
        {
            DropItem();
        }
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

    public void PickupItem(GameObject itemPrefab)
    {
        pickedUpItem = itemPrefab;
        isItemPickedUp = true;
        PlayerMovement.instance.ActivateSecondBody(pickedUpItem);
    }

    public void DropItem()
    {
        var obj = Instantiate(pickedUpItem, 
            PlayerMovement.instance.secondBody.transform.position, 
            PlayerMovement.instance.secondBody.transform.rotation);
        obj.GetComponent<CarriableItem>().isItemPickedUp = false;
        obj.SetActive(true);
        pickedUpItem = null;
        PlayerMovement.instance.RemoveSecondBody();
        isItemPickedUp = false;
    }

    public void UpdateKeyUI()
    {
        foreach(var key in keys)
            key.textCount.text = $"x{key.count}";
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
