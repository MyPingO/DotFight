using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button playButton;
    public Button soundButton;
    public Button aboutButton;
    public Slider soundSlider;

    public void HandleSoundButtonClick()
    {
        //enable the sound slider
        soundSlider.gameObject.SetActive(!soundSlider.gameObject.activeSelf);
    }

    public void HandlePlayButtonClick()
    {
        //load the dot selection scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("DotSelection");
    }
    public void HandleSoundSliderValueChange()
    {
        //set the volume of the audio source
        AudioListener.volume = soundSlider.value;
    }

    public void HandleAboutButtonClick()
    {
        //load the about scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("About");
    }
}
