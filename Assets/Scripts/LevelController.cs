using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using InstantGamesBridge.Common;
using InstantGamesBridge;

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
    public Animator greetingsAnimator;
    public TextMeshProUGUI deathText;
    public AudioSource mainAudioSource;
    public AudioClip boxDrop;

    public AudioClip[] hurtClips;

    public bool isLevelFailed = false;
    public bool levelStarted = false;
    private GameObject pickedUpItem;
    private bool isItemPickedUp;
    private bool firstDamage;
    private bool firstPickup;
    private bool firstLevelStart;

    private void Awake()
    {
        firstLevelStart = true;
        firstDamage = true;
        firstPickup = true;

        instance = this;
        hearts = SceneVariables.livesCount;
        UpdateHeartUI();
    }

    private void Start()
    {
        mainAudioSource.volume = 0.3f * SceneVariables.volumeMultiplier;
        FadeTransition.FadeScreen(Color.black, 1, 0, 1);
        TimeController.instance.rewindables.Add(new TNRD.SerializableInterface<IRewindable>(this));
    }

    private void FixedUpdate()
    {
        if (isLevelFailed || !levelStarted)
            return;

        levelTimeInSeconds -= Time.fixedDeltaTime;
        if (levelTimeInSeconds <= 0)
        {
            FailLevel("Время кончилось.");
            return;
        }
        timerText.text = FormatSeconds(levelTimeInSeconds);
    }

    private void Update()
    {
        if (!levelStarted)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                levelStarted = true;
                if (firstLevelStart)
                {
                    InfoPanel.instance.ShowInfo("Поставить игру на паузу и снова посмотреть управление можно нажав на кнопку \"Escape\"");
                    firstLevelStart = false;
                }
                greetingsAnimator.Play("Disappearing");
            }
            return;
        }

        if (!isLevelFailed && levelStarted)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                greetingsAnimator.gameObject.SetActive(true);
                levelStarted = false;
            }
        }

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
        mainAudioSource.PlayOneShot(hurtClips[Random.Range(0, hurtClips.Length)]);
        if (firstDamage)
        {
            InfoPanel.instance.ShowInfo("Откати время зажав \"Space\", чтобы восстановить потраченную жизнь");
            firstDamage = false;
        }
        hearts--;
        UpdateHeartUI();
        if (hearts == 0)
            FailLevel("Вы умерли.");
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
        if (firstPickup)
        {
            InfoPanel.instance.ShowInfo("Коробку нужно поставить на напольную кнопку. Снова нажми \"F\", чтобы бросить коробку.");
            firstPickup = false;
        }
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
        mainAudioSource.PlayOneShot(boxDrop);
        isItemPickedUp = false;
    }

    public void NextLevel(string sceneName)
    {
        Bridge.advertisement.ShowInterstitial();
        SceneVariables.livesCount = hearts;
        FadeTransition.FadeScreen(Color.black, 0, 1, 0.5f, () => SceneManager.LoadScene(sceneName));
    }

    public void ReloadLevel()
    {
        Bridge.advertisement.ShowInterstitial();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateKeyUI()
    {
        foreach(var key in keys)
            key.textCount.text = $"x{key.count}";
    }

    public void FailLevel(string reasonText)
    {
        isLevelFailed = true;
        deathText.text = reasonText;
        levelFailedUI.SetActive(true);
    }


    private LinkedList<float> levelTime = new();
    private LinkedList<int[]> keysCount = new();
    private LinkedList<int> heartsState = new();
    private LinkedList<bool> itemPickedUp = new();
    public void Record()
    {
        levelTime.AddFirst(levelTimeInSeconds);
        keysCount.AddFirst(keys.Select(a => a.count).ToArray());
        heartsState.AddFirst(hearts);
        itemPickedUp.AddFirst(isItemPickedUp);
    }

    public void Rewind()
    {
        levelTimeInSeconds = levelTime.First.Value;
        for (int i = 0; i < keysCount.First.Value.Length; ++i)
            keys[i].count = keysCount.First.Value[i];
        UpdateKeyUI();
        hearts = heartsState.First.Value;
        UpdateHeartUI();
        isItemPickedUp = itemPickedUp.First.Value;
        itemPickedUp.RemoveFirst();
        heartsState.RemoveFirst();
        keysCount.RemoveFirst();
        levelTime.RemoveFirst();
    }

    public void RemoveLast()
    {
        itemPickedUp.RemoveLast();
        keysCount.RemoveLast();
        levelTime.RemoveLast();
        heartsState.RemoveLast();
    }
}
