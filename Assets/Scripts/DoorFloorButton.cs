using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class DoorFloorButton : Door
{
    public FloorButton[] buttons;

    private void Update()
    {
        var lastOpened = opened;
        opened = !buttons.Any(d => !d.isPressed);

        if (lastOpened != opened)
            PlaySound();
        SetSprite();
        SetCollider();
    }
}
