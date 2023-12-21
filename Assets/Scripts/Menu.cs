using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnPlayPressed()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnSettingsPressed()
    {

    }
}
