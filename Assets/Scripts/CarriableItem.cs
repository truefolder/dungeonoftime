using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TNRD;

public class CarriableItem : MonoBehaviour, IRewindable
{
    public GameObject itemPrefab;

    private bool onTrigger = false;
    public bool isItemPickedUp = false;

    public bool firstPickup = true;

    private void Start()
    {
        itemPrefab = gameObject;
        if (!isItemPickedUp)
        {
            var reference = new SerializableInterface<IRewindable>(this);
            TimeController.instance.rewindables.Add(reference);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            onTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            onTrigger = false;
    }

    private void Update()
    {
        if (isItemPickedUp)
            return;
        transform.GetChild(0).gameObject.SetActive(onTrigger);
        if (onTrigger && Input.GetKeyDown(KeyCode.F))
        {
            UpdateItem(true);
            LevelController.instance.PickupItem(itemPrefab);
        }
    }

    private void UpdateItem(bool active)
    {
        if (active)
        {
            isItemPickedUp = true;
            gameObject.SetActive(false);
        }
        else
        {
            isItemPickedUp = false;
            gameObject.SetActive(true);
        }
    }

    private LinkedList<bool> itemPickedUp = new();
    private LinkedList<Vector3> positions = new();
    public void Record()
    {
        itemPickedUp.AddFirst(isItemPickedUp);
        positions.AddFirst(transform.position);
    }

    public void Rewind()
    {
        UpdateItem(itemPickedUp.First.Value);
        transform.position = positions.First.Value;
        positions.RemoveFirst();
        itemPickedUp.RemoveFirst();
    }

    public void RemoveLast()
    {
        positions.RemoveLast();
        itemPickedUp.RemoveLast();
    }
}
