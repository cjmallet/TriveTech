using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioSource MusicSource;

	public static AudioManager _instance = null;

	public List<AudioClip> audioClips = new List<AudioClip>();

	[HideInInspector]public AudioClip breakDestructibleObject, buttBoosterReady, buttBoost, LevelComplete;
	

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

		//DontDestroyOnLoad(gameObject);

		audioClips = Resources.LoadAll("Sounds/", typeof(AudioClip)).Cast<AudioClip>().ToList();

		foreach (AudioClip clip in audioClips)
        {
        }

		//SetSounds();
	}

	private void SetSounds()
    {
		breakDestructibleObject = Resources.Load<AudioClip>("Sounds/BreakDestructibleObject");
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
