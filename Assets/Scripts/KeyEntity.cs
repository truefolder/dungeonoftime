using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEntity : MonoBehaviour
{
    public string name;

    private bool onTrigger;

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
        if (onTrigger)
        {
            LevelController.instance.AddKey(name);
            Destroy(gameObject);
        }
    }
}
