using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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

    private void SetAnimation()
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
}
