using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public void OnPlayPressed()
    {
        FadeTransition.FadeScreen(Color.black, 0, 1, 0.5f, () => SceneManager.LoadScene("Level1"));
    }
    
    public void OnSliderVolumeChanged()
    {
        SceneVariables.volumeMultiplier = volumeSlider.value;
    }

    public void OnSettingsPressed()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnMainMenuPressed()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}
