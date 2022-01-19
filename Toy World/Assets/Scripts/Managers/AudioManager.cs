using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioSource MusicSource;

	public static AudioManager _instance = null;
	

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);



	}

	/// <summary>
	/// Play a single clip through the sound effects source.
	/// </summary>	
	public void Play(AudioClip clip, AudioSource source)
	{
		source.clip = clip;
		source.Play();
	}

	/// <summary>
	/// Play a single clip through the music source.
	/// </summary>
	/// <param name="clip"></param>
	public void PlayMusic(AudioClip clip)
	{
		MusicSource.clip = clip;
		MusicSource.Play();
	}
}
