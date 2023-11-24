using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorKey : Door, IRewindable
{
    public Sprite keyHoleSprite;
    public SpriteRenderer keyHoleSpriteRenderer;

    public int neededKeyCount = 1;
    public string neededKeyName;
    private bool onTrigger = false;

    private void Start()
    {
        TimeController.instance.rewindables.Add(new TNRD.SerializableInterface<IRewindable>(this));
        keyHoleSpriteRenderer.sprite = keyHoleSprite;
        transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = $"x{neededKeyCount}";
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onTrigger = false;
    }

    private void Update()
    {
            transform.GetChild(1).gameObject.SetActive(onTrigger && !opened);
        if (!onTrigger || opened)
            return;
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!LevelController.instance.RemoveKey(neededKeyName, neededKeyCount))
                return;
            opened = true;
            SetSprite();
            transform.GetChild(0).gameObject.SetActive(false);
            SetCollider();
        }
    }

    private LinkedList<bool> doorOpened = new();
    public void Record()
    {
        doorOpened.AddFirst(opened);
    }

    public void Rewind()
    {
        opened = doorOpened.First.Value;
        if (!opened)
            transform.GetChild(0).gameObject.SetActive(true);
        SetSprite();
        SetCollider();
        doorOpened.RemoveFirst();
    }

    public void RemoveLast()
    {
        doorOpened.RemoveLast();
    }
}
