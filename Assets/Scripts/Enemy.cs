using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IRewindable
{
    public Transform[] waypoints;
    public float moveSpeed = 5f;

    private Animator animator;
    private int currentWaypoint = 0;
    private void Start()
    {
        animator = GetComponent<Animator>();
        TimeController.instance.rewindables.Add(new TNRD.SerializableInterface<IRewindable>(this));
    }

    private void Update()
    {
        if (LevelController.instance.isLevelFailed)
            return;
        if (currentWaypoint < waypoints.Length)
        {
            MoveTowardsWaypoint();
            SetAnimation();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
            LevelController.instance.RemoveHeart();
        if (currentWaypoint != 0)
            currentWaypoint--;
    }

    private void MoveTowardsWaypoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, waypoints[currentWaypoint].position) < 0.1f)
        {
            currentWaypoint++;
        }

        // ���� ���� ������ ��������� �����, ���������� ��� �� ������
        if (currentWaypoint == waypoints.Length)
        {
            currentWaypoint = 0;
        }
    }

    private void SetAnimation()
    {
        var directionVector = (waypoints[currentWaypoint].position - transform.position).normalized;
        if (directionVector == Vector3.left)
            animator.Play("Left");
        else if (directionVector == Vector3.right)
            animator.Play("Right");
        else if (directionVector == Vector3.up)
            animator.Play("Forward");
        else if (directionVector == Vector3.down)
            animator.Play("Back");
    }

    private LinkedList<Vector3> enemyPositions = new();

    public void Record()
    {
        enemyPositions.AddFirst(gameObject.transform.position);
    }

    public void Rewind()
    {
        gameObject.transform.position = enemyPositions.First.Value;
        SetAnimation();
        enemyPositions.RemoveFirst();
    }

    public void RemoveLast()
    {
        enemyPositions.RemoveLast();
    }
}
