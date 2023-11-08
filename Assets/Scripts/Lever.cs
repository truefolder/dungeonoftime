using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    public Sprite leverOffSprite;
    public Sprite leverOnSprite;

    private SpriteRenderer spriteRenderer;
    public bool activated = false;

    private bool onTrigger = false;
    private void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
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
}