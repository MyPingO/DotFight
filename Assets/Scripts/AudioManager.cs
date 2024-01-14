using System.Collections.Generic;
using UnityEngine;

public enum SoundEffect
{
	YouWin,
	YouLose,
	Explosion,
	Jump,
	// Add more sound effects as needed
}

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioSource musicSource;
	public AudioSource sfxSource;
	
	public AudioClip youWin;
	public AudioClip youLose;
	public AudioClip respawn;
	public AudioClip death;
	public AudioClip buttonHover;
	public AudioClip buttonClick;
	
	public AudioClip backgroundMusic;
	public Dictionary<SoundEffect, AudioClip> soundEffects = new();

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			PlayBackgroundMusic();
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void PlayBackgroundMusic()
	{
		musicSource.clip = backgroundMusic;
		musicSource.loop = true;
		musicSource.Play();
	}

	public void PlaySFX(AudioClip clip)
	{
		sfxSource.PlayOneShot(clip);
	}
}
