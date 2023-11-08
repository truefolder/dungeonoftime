using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Tick
{
    public MovingObject player;
    public float levelTime;
    public bool[] leversState;
    public Tick(MovingObject player, float levelTime, bool[] leversState)
    {
        this.player = player;
        this.levelTime = levelTime;
        this.leversState = leversState;
    }
}
public class MovingObject
{
    public Vector3 position;
    public Quaternion rotation;
    public MovingObject(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}

public class TimeController : MonoBehaviour
{
    public GameObject player;
    public Lever[] levers;

    private List<Tick> ticks;
    private bool isRewinding = false;
    private float recordTime = 5f;

    public int rewindsPerFrame = 1;

    private void Start()
    {
        ticks = new();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartRewind();
        if (Input.GetKeyUp(KeyCode.Space))
            StopRewind();
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
        if (ticks.Count == 0)
        {
            StopRewind();
            return;
        }
        var tick = ticks[0];
        player.transform.position = tick.player.position;
        player.transform.rotation = tick.player.rotation;
        LevelController.instance.levelTimeInSeconds = tick.levelTime;
        for (int i = 0; i < levers.Length; ++i)
        {
            levers[i].activated = tick.leversState[i];
            levers[i].SetSprite();
        }
        ticks.RemoveAt(0);
    }

    private void Record()
    {
        if (ticks.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
            ticks.RemoveAt(ticks.Count - 1);

        var movingPlayer = new MovingObject(player.transform.position, player.transform.rotation);
        ticks.Insert(0, new Tick(movingPlayer, 
            LevelController.instance.levelTimeInSeconds, 
            levers.Select(d => d.activated).ToArray()));
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