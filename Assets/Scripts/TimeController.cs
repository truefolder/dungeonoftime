using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TNRD;

public class TimeController : Cooldown
{
    [SerializeField]
    public List<SerializableInterface<IRewindable>> rewindables = new();
    public GameObject player;
    public static TimeController instance;

    private int ticks = 0;
    public bool isRewinding = false;

    public float recordTime = 5f;
    public int rewindsPerFrame = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        CooldownUpdate();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canUse)
                StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (canUse)
            {
                StartCooldown();
                StopRewind();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isRewinding)
            for (int i = 0; i < rewindsPerFrame; i++)
                Rewind();
        else
            Record();
    }

    private void Rewind()
    {
        if (ticks == 0)
        {
            StopRewind();
            return;
        }
        
        foreach(var rewindable in rewindables)
            rewindable.Value.Rewind();

        ticks -= 1;
    }

    private void Record()
    {
        if (ticks > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            foreach (var rewindable in rewindables)
                rewindable.Value.RemoveLast();
            ticks -= 1;
        }

        ticks += 1;

        foreach (var rewindable in rewindables)
            rewindable.Value.Record();
    }

    public void StartRewind()
    {
        isRewinding = true;
        player.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
        player.GetComponent<Rigidbody2D>().isKinematic = false;
    }
}