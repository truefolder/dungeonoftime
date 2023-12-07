using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEntity : MonoBehaviour, IRewindable
{
    public string name;

    private bool onTrigger;
    private bool pickedUp = false;

    private void Start()
    {
        TimeController.instance.rewindables.Add(new TNRD.SerializableInterface<IRewindable>(this));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            onTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            onTrigger = false;
    }

    private void Update()
    {
        if (onTrigger && !TimeController.instance.isRewinding)
        {
            LevelController.instance.AddKey(name);
            pickedUp = true;
            UpdateEntity();
        }
    }

    private void UpdateEntity()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = !pickedUp;
        gameObject.GetComponent<Collider2D>().enabled = !pickedUp;
    }

    private LinkedList<bool> keyState = new(); 
    public void Record()
    {
        keyState.AddFirst(pickedUp);
    }

    public void Rewind()
    {
        pickedUp = keyState.First.Value;
        UpdateEntity();
        keyState.RemoveFirst();
    }

    public void RemoveLast()
    {
        keyState.RemoveLast();
    }
}
