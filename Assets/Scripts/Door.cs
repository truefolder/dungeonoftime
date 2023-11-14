using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Door : MonoBehaviour
{
	[System.Serializable]
	public class LeverSequence
	{
		public Lever lever;
		public bool neededCondition;
	}
	public Sprite openedSprite;
	public Sprite closedSprite;

	public LeverSequence[] leverSequence;
	public bool opened;

	private SpriteRenderer spriteRenderer;
	private BoxCollider2D _collider;

	private void Awake()
	{
		spriteRenderer = transform.GetComponent<SpriteRenderer>();
		_collider = transform.GetComponent<BoxCollider2D>();
	}

	private void Update()
	{
		bool flagOpened = true;

		foreach(var lever in leverSequence)
		{
			if (lever.lever.activated != lever.neededCondition)
				flagOpened = false;
		}

		opened = flagOpened;

		SetSprite();
		SetCollider();
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
