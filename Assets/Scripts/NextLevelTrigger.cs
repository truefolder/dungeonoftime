using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LevelController.instance.NextLevel(sceneName);
    }
}
