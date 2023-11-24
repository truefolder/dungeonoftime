using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lever : MonoBehaviour, IRewindable
{
    public Sprite leverOffSprite;
    public Sprite leverOnSprite;

    private SpriteRenderer spriteRenderer;
    public bool activated = false;

    private bool onTrigger = false;
    private void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        TimeController.instance.rewindables.Add(new TNRD.SerializableInterface<IRewindable>(this));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onTrigger = false;
    }

    public void SetSprite()
    {
        if (activated)
            spriteRenderer.sprite = leverOnSprite;
        else
            spriteRenderer.sprite = leverOffSprite;
    }

    private void Update()
    {
        if (onTrigger && Input.GetKeyDown(KeyCode.E))
        {
            activated = !activated;
            SetSprite();
        }
    }

    private LinkedList<bool> leverState = new();
    public void Record()
    {
        leverState.AddFirst(activated);
    }

    public void Rewind()
    {
        activated = leverState.First.Value;
        SetSprite();
        leverState.RemoveFirst();
    }

    public void RemoveLast()
    {
        leverState.RemoveLast();
    }
}
