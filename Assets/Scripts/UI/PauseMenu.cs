using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	[SerializeField]
	private GameObject pauseMenuPanel;

	[SerializeField]
	private GameObject darkPanel;
	
	[SerializeField]
	private GameObject gameOverPanel;

	[SerializeField]
	private AudioManager audioManager;

	public Slider musicSlider;

	public Slider sfxSlider;

	void Awake()
	{
		audioManager = AudioManager.instance;

		musicSlider.gameObject.SetActive(false);
		sfxSlider.gameObject.SetActive(false);
	}

	void Start()
	{
		//set the value of the sound slider to the current volume
		musicSlider.value = audioManager.musicSource.volume;
		sfxSlider.value = audioManager.sfxSource.volume;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !gameOverPanel.activeSelf)
		{
			TogglePauseMenu();
		}
		if (pauseMenuPanel.activeSelf)
		{
			//set the value of the sound slider to the current volume
			audioManager.musicSource.volume = musicSlider.value;
			audioManager.sfxSource.volume = sfxSlider.value;
		}
	}

	public void TogglePauseMenu()
	{
		darkPanel.SetActive(!darkPanel.activeSelf);
		pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
		musicSlider.gameObject.SetActive(!musicSlider.gameObject.activeSelf);
		sfxSlider.gameObject.SetActive(!sfxSlider.gameObject.activeSelf);
		Time.timeScale = pauseMenuPanel.activeSelf ? 0 : 1;
	}
	
}
