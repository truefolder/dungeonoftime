using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IRewindable
{
    public enum Direction
    {
        Forward,
        Backward,
        Left,
        Right
    }
    private Rigidbody2D body;
    private float horizontal;
    public static PlayerMovement instance;
    private float vertical;
    private float moveLimiter = 0.7f;
    public GameObject secondBody;
    public Direction currentDirection = Direction.Forward;

    private Animator animator;

    public float runSpeed = 20.0f;
    void Start()
    {
        instance = this;
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
        UpdateSecondBodyPosition();
    }

    private void SetAnimation() // это можно сделать лучше
    {
        if (horizontal == 1)
        {
            currentDirection = Direction.Right;
            animator.Play("Right");
        }
        else if (horizontal == -1)
        {
            currentDirection = Direction.Left;
            animator.Play("Left");
        }  
        else if (vertical == 1)
        {
            currentDirection = Direction.Forward;
            animator.Play("Forward");
        }   
        else if (vertical == -1)
        {
            currentDirection = Direction.Backward;
            animator.Play("Back");
        }  
    }

    public void ActivateSecondBody(GameObject body)
    {
        secondBody.SetActive(true);
        UpdateSecondBodyPosition();
    }

    public void DisableSecondBody()
    {
        secondBody.SetActive(false);
    }

    public void UpdateSecondBodyPosition()
    {
        if (secondBody == null)
            return;
        switch (currentDirection)
        {
            case Direction.Left:
                secondBody.transform.localPosition = new Vector3(-0.5f, 0, 0);
                break;
            case Direction.Right:
                secondBody.transform.localPosition = new Vector3(0.5f, 0, 0);
                break;
            case Direction.Forward:
                secondBody.transform.localPosition = new Vector3(0, 0.5f, 0);
                break;
            case Direction.Backward:
                secondBody.transform.localPosition = new Vector3(0, -0.5f, 0);
                break;
        }
    }

    private LinkedList<Vector3> playerPositions = new();
    private LinkedList<Quaternion> playerRotations = new();
    private LinkedList<Vector3> secondBodyLocalPositions = new();
    private LinkedList<bool> secondBodyObjActive = new();
    private LinkedList<AnimatorClipInfo> playerAnimState = new();
    public void Record()
    {
        playerPositions.AddFirst(gameObject.transform.position);
        playerRotations.AddFirst(gameObject.transform.rotation);
        secondBodyObjActive.AddFirst(secondBody.activeSelf);
        secondBodyLocalPositions.AddFirst(secondBody.transform.localPosition);
        playerAnimState.AddFirst(animator.GetCurrentAnimatorClipInfo(0)[0]);
    }

    public void Rewind()
    {
        gameObject.transform.position = playerPositions.First.Value;
        gameObject.transform.rotation = playerRotations.First.Value;
        animator.Play(playerAnimState.First.Value.clip.name);

        secondBody.SetActive(secondBodyObjActive.First.Value);

        secondBody.transform.localPosition = secondBodyLocalPositions.First.Value;

        secondBodyLocalPositions.RemoveFirst();
        secondBodyObjActive.RemoveFirst();

        playerPositions.RemoveFirst();
        playerRotations.RemoveFirst();
        playerAnimState.RemoveFirst();
    }

    public void RemoveLast()
    {
        playerPositions.RemoveLast();
        playerRotations.RemoveLast();
        secondBodyLocalPositions.RemoveLast();
        secondBodyObjActive.RemoveLast();
        playerAnimState.RemoveLast();
    }
}
