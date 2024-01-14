using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
	public Slider musicSlider;
	public Slider sfxSlider;
	public AudioManager audioManager;

	public Button[] menuButtons;

	// TODO: Change dot type names
	public enum DotType
	{
		White,
		Toxin,
		Magic,
		Pyre
	}

	private void Awake()
	{
		audioManager = AudioManager.instance;

		musicSlider = GameObject.FindGameObjectWithTag("MusicSlider").GetComponent<Slider>();
		sfxSlider = GameObject.FindGameObjectWithTag("SFXSlider").GetComponent<Slider>();

		musicSlider.gameObject.SetActive(false);
		sfxSlider.gameObject.SetActive(false);

		foreach (Button button in menuButtons)
		{
			EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
			// Add OnPointerEnter event
			EventTrigger.Entry entry = new EventTrigger.Entry
			{
				eventID = EventTriggerType.PointerEnter
			};
			entry.callback.AddListener(
				(BaseEventData data) =>
				{
					AudioManager.instance.PlaySFX(AudioManager.instance.buttonHover);
				}
			);
			trigger.triggers.Add(entry);

			button.onClick.AddListener(
				() => AudioManager.instance.PlaySFX(AudioManager.instance.buttonClick)
			);
		}
	}

	private void Start()
	{
		//set the value of the sound slider to the current volume
		musicSlider.value = audioManager.musicSource.volume;
		sfxSlider.value = audioManager.sfxSource.volume;
		
		// Reset player prefs if in MainMenu
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
		{
			PlayerPrefs.DeleteAll();
		}
	}

	private void Update()
	{
		//set the value of the sound slider to the current volume
		audioManager.musicSource.volume = musicSlider.value;
		audioManager.sfxSource.volume = sfxSlider.value;
	}

	public void HandleSoundButtonClick()
	{
		//enable the sound slider
		musicSlider.gameObject.SetActive(!musicSlider.gameObject.activeSelf);
		sfxSlider.gameObject.SetActive(!sfxSlider.gameObject.activeSelf);
	}

	public void LoadNextScene()
	{
		//l Check if the player has selected a dot
		if (
			SceneManager.GetActiveScene() == SceneManager.GetSceneByName("PlayerSelection")
			&& !PlayerPrefs.HasKey("SelectedDot")
		)
			return;
		UnityEngine.SceneManagement.SceneManager.LoadScene(
			SceneManager.GetActiveScene().buildIndex + 1
		);
	}

	public void LoadChosenLevel()
	{
		// Check if the player has selected a map
		if (!PlayerPrefs.HasKey("SelectedMap"))
			return;
		UnityEngine.SceneManagement.SceneManager.LoadScene(PlayerPrefs.GetInt("SelectedMap"));
	}

	public void HandleSoundSliderValueChange()
	{
		//set the volume of the audio source
		AudioListener.volume = musicSlider.value;
	}

	public void HandleAboutButtonClick()
	{
		//load the about scene
		UnityEngine.SceneManagement.SceneManager.LoadScene("About");
	}

	public void HandleBackButtonClick()
	{
		//load the previous scene
		UnityEngine.SceneManagement.SceneManager.LoadScene(
			SceneManager.GetActiveScene().buildIndex - 1
		);
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
