using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Slider soundSlider;

    public enum DotType
    {
        White,
        Toxin,
        Magic,
        Pyre
    }


    public void Start()
    {
        //set the value of the sound slider to the current volume
        soundSlider.value = AudioListener.volume;
    }

    public void HandleSoundButtonClick()
    {
        //enable the sound slider
        soundSlider.gameObject.SetActive(!soundSlider.gameObject.activeSelf);
    }

    public void HandlePlayButtonClick()
    {
        //load the dot selection scene

        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

    public void HandleBackButtonClick()
    {
        //load the previous scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void SetSelectedDot(string dotType)
    {
        //set the selected dot type
        Debug.Log(dotType);
        PlayerPrefs.SetString("SelectedDot", dotType);
    }

    public void SetSelectedMap(int map)
    {
        //set the selected map
        Debug.Log(map);
        PlayerPrefs.SetInt("SelectedMap", map);
    }
}
