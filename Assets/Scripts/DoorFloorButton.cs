using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFloorButton : Door
{
    public FloorButton button;

    private void Update()
    {
        opened = button.isPressed;
        SetSprite();
        SetCollider();
    }
}
