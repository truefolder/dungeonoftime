using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelController : MonoBehaviour, IRewindable
{
    public float levelTimeInSeconds = 300f;

    public static LevelController instance;

    public Key[] keys;

    public Text timerText;

    public int hearts = 3;
    public Sprite heartFilled;
    public Sprite heartUnfilled;
    public Image[] heartsImage;
    public GameObject levelFailedUI;

    public bool isLevelFailed = false;
    private GameObject pickedUpItem;
    private bool isItemPickedUp;

    private void Awake()
    {
        instance = this;
        hearts = SceneVariables.livesCount;
        UpdateHeartUI();
    }

    private void Start()
    {
        FadeTransition.FadeScreen(Color.black, 1, 0, 0.5f);
        TimeController.instance.rewindables.Add(new TNRD.SerializableInterface<IRewindable>(this));
    }

    private void FixedUpdate()
    {
        if (isLevelFailed)
            return;

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
        if (isLevelFailed && Input.GetKeyDown(KeyCode.R))
            ReloadLevel();
        if (isLevelFailed)
            return;
        if (Input.GetKeyDown(KeyCode.F) && isItemPickedUp)
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

    public void RemoveHeart()
    {
        hearts--;
        UpdateHeartUI();
        if (hearts == 0)
            FailLevel();
    }

    public void AddHeart()
    {
        hearts++;
        UpdateHeartUI();
    }

    public void UpdateHeartUI()
    {
        foreach (var hp in heartsImage)
            hp.sprite = heartFilled;
        for (int i = heartsImage.Length; i > hearts; --i)
            heartsImage[i - 1].sprite = heartUnfilled;
    }

    public void AddKey(string keyName)
    {
        keys.Where(x => x.name == keyName).First().count += 1;
        UpdateKeyUI();
    }

    public bool RemoveKey(string keyName, int count)
    {
        var key = keys.Where(x => x.name == keyName).First();
        if (key.count < count)
            return false;

        key.count -= count;
        UpdateKeyUI();

        return true;
    }

    public void PickupItem(GameObject itemPrefab)
    {
        isItemPickedUp = true;
        pickedUpItem = itemPrefab;
        PlayerMovement.instance.ActivateSecondBody(pickedUpItem);
    }

    public void DropItem()
    {
        if (pickedUpItem.GetComponent<CarriableItem>().firstPickup)
        {
            pickedUpItem.GetComponent<CarriableItem>().firstPickup = false;
            return;
        }
        pickedUpItem.GetComponent<CarriableItem>().isItemPickedUp = false;
        pickedUpItem.SetActive(true);
        pickedUpItem.transform.position = PlayerMovement.instance.secondBody.transform.position;
        PlayerMovement.instance.DisableSecondBody();
        isItemPickedUp = false;
    }

    public void NextLevel(string sceneName)
    {
        SceneVariables.livesCount = hearts;
        FadeTransition.FadeScreen(Color.black, 0, 1, 0.5f, () => SceneManager.LoadScene(sceneName));
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateKeyUI()
    {
        foreach(var key in keys)
            key.textCount.text = $"x{key.count}";
    }

    public void FailLevel()
    {
        isLevelFailed = true;
        levelFailedUI.SetActive(true);
    }


    private LinkedList<float> levelTime = new();
    private LinkedList<int[]> keysCount = new();
    private LinkedList<int> heartsState = new();
    public void Record()
    {
        levelTime.AddFirst(levelTimeInSeconds);
        keysCount.AddFirst(keys.Select(a => a.count).ToArray());
        heartsState.AddFirst(hearts);
    }

    public void Rewind()
    {
        levelTimeInSeconds = levelTime.First.Value;
        for (int i = 0; i < keysCount.First.Value.Length; ++i)
            keys[i].count = keysCount.First.Value[i];
        UpdateKeyUI();
        hearts = heartsState.First.Value;
        UpdateHeartUI();
        heartsState.RemoveFirst();
        keysCount.RemoveFirst();
        levelTime.RemoveFirst();
    }

    public void RemoveLast()
    {
        keysCount.RemoveLast();
        levelTime.RemoveLast();
        heartsState.RemoveLast();
    }
}
