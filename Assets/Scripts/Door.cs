using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Door : MonoBehaviour
{
    
    public Sprite openedSprite;
    public Sprite closedSprite;

    public bool opened;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D _collider;

    private void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        _collider = transform.GetComponent<BoxCollider2D>();
    }

    public void SetSprite()
    {
        if (opened)
            spriteRenderer.sprite = openedSprite;
        else
            spriteRenderer.sprite = closedSprite;
    }

    public void SetCollider()
    {
        _collider.isTrigger = opened;
    }
}
