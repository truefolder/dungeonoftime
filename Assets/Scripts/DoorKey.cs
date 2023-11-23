using System.Collections;
using System.Collections.Generic;
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
        keyHoleSpriteRenderer.sprite = keyHoleSprite;
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
        if (!onTrigger || opened)
            return;
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!LevelController.instance.RemoveKey(neededKeyName, neededKeyCount))
                return;
            opened = true;
            SetSprite();
            keyHoleSpriteRenderer.enabled = false;
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
            keyHoleSpriteRenderer.enabled = true;
        SetSprite();
        SetCollider();
        doorOpened.RemoveFirst();
    }

    public void RemoveLast()
    {
        doorOpened.RemoveLast();
    }
}
