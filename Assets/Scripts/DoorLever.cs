using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLever : Door
{
    [System.Serializable]
    public class LeverSequence
    {
        public Lever lever;
        public bool neededCondition;
    }
    public LeverSequence[] leverSequence;

    private void Update()
    {
        bool flagOpened = true;

        foreach (var lever in leverSequence)
        {
            if (lever.lever.activated != lever.neededCondition)
                flagOpened = false;
        }

        opened = flagOpened;

        SetSprite();
        SetCollider();
    }
}
