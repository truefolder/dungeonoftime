using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TNRD;

public class CarriableItem : MonoBehaviour, IRewindable
{
    public GameObject itemPrefab;

    private bool onTrigger = false;
    public bool isItemPickedUp = false;

    SerializableInterface<IRewindable> reference;
    private void Start()
    {
        itemPrefab = gameObject;
        if (!isItemPickedUp)
		{
            reference = new SerializableInterface<IRewindable>(this);
            TimeController.instance.rewindables.Add(reference);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onTrigger = false;
    }

    private void Update()
    {
        if (isItemPickedUp)
            return;
        if (onTrigger && Input.GetKeyDown(KeyCode.F))
		{
            LevelController.instance.PickupItem(itemPrefab);
            UpdateItem(true);
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
    public void Record()
    {
        itemPickedUp.AddFirst(isItemPickedUp);
    }

    public void Rewind()
    {
        if (itemPickedUp.Count == 0)
		{
            UpdateItem(true);
            return;
        }
        UpdateItem(itemPickedUp.First.Value);
        itemPickedUp.RemoveFirst();
    }

    public void RemoveLast()
    {
        itemPickedUp.RemoveLast();
    }
}
