using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IRewindable
{
    private Rigidbody2D body;
    private float horizontal;
    private float vertical;
    private float moveLimiter = 0.7f;

    private Animator animator;

    public float runSpeed = 20.0f;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (TimeController.instance.isRewinding)
            return;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        SetAnimation();

        if (horizontal != 0 && vertical != 0)
        {
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    private void SetAnimation() // это можно сделать лучше
    {
        if (horizontal == 1)
            animator.Play("Right");
        else if (horizontal == -1)
            animator.Play("Left");
        else if (vertical == 1)
            animator.Play("Forward");
        else if (vertical == -1)
            animator.Play("Back");
    }

    private LinkedList<Vector3> playerPositions = new();
    private LinkedList<Quaternion> playerRotations = new();
    private LinkedList<AnimatorClipInfo> playerAnimState = new();
	public void Record()
	{
        playerPositions.AddFirst(gameObject.transform.position);
        playerRotations.AddFirst(gameObject.transform.rotation);
        playerAnimState.AddFirst(animator.GetCurrentAnimatorClipInfo(0)[0]);
	}

	public void Rewind()
	{
        gameObject.transform.position = playerPositions.First.Value;
        gameObject.transform.rotation = playerRotations.First.Value;
        animator.Play(playerAnimState.First.Value.clip.name);
        playerPositions.RemoveFirst();
        playerRotations.RemoveFirst();
        playerAnimState.RemoveFirst();
	}

    public void RemoveLast()
	{
        playerPositions.RemoveLast();
        playerRotations.RemoveLast();
        playerAnimState.RemoveLast();
	}
}
