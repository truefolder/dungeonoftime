using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class DoorFloorButton : Door
{
    public FloorButton[] buttons;

    private void Update()
    {
        opened = !buttons.Any(d => !d.isPressed);
        SetSprite();
        SetCollider();
    }
}
