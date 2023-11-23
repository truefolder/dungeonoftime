using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    public bool isPressed;
    private bool onTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Box")
            onTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Box")
            onTrigger = false;
    }

    private void Update()
    {
        isPressed = onTrigger;
    }
}
