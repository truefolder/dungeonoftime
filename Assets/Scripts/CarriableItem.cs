using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriableItem : MonoBehaviour, IRewindable
{
    public GameObject itemPrefab;

    private bool onTrigger = false;
    public bool isItemPickedUp = false;

    private void Awake()
    {
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            UpdateItem(true);    
        }
    }

    private void UpdateItem(bool active)
    {
        if (active)
        {
            isItemPickedUp = true;
            LevelController.instance.PickupItem(itemPrefab);
            gameObject.SetActive(false);
        }
        else
        {
            isItemPickedUp = false;
            gameObject.SetActive(true);
        }
    }

    private LinkedList<bool> itemPickedUp;
    public void Record()
    {
        itemPickedUp.AddFirst(isItemPickedUp);
    }

    public void Rewind()
    {
        if (itemPickedUp.Count == 0)
            Destroy(gameObject);
        UpdateItem(itemPickedUp.First.Value);
        itemPickedUp.RemoveFirst();
    }

    public void RemoveLast()
    {
        itemPickedUp.RemoveLast();
    }
}
