using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IRewindable
{
    public Transform[] waypoints;
    public float moveSpeed = 5f;

    private int currentWaypoint = 0;

    private void Update()
    {
        if (currentWaypoint < waypoints.Length)
        {
            MoveTowardsWaypoint();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
            LevelController.instance.RemoveHeart();
    }

    public void MoveTowardsWaypoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, waypoints[currentWaypoint].position) < 0.1f)
        {
            currentWaypoint++;
        }

        // Если враг достиг последней точки, возвращаем его на первую
        if (currentWaypoint == waypoints.Length)
        {
            currentWaypoint = 0;
        }
    }

    private LinkedList<Vector3> enemyPositions = new();

    public void Record()
    {
        enemyPositions.AddFirst(gameObject.transform.position);
    }

    public void Rewind()
    {
        gameObject.transform.position = enemyPositions.First.Value;
        enemyPositions.RemoveFirst();
    }

    public void RemoveLast()
    {
        enemyPositions.RemoveLast();
    }
}
