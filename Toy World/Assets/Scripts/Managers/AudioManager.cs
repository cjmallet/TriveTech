using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	//public AudioSource MusicSource;

	public static AudioManager Instance = null;

	[HideInInspector]public List<AudioClip> audioClips = new List<AudioClip>();
	private GameObject coreBlock;
	private List<UtilityPart> allUtilityParts = new List<UtilityPart>();



	public enum clips
	{
		BreakDestructibleObject,
		ButtBoosterReady,
		ButtBoost,
		SpringSound,
		PlacePart,
		RemovePart,
		PartDestruction,
		MenuOpen,
		MenuClose,
		LevelComplete
	};

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		audioClips = Resources.LoadAll("Sounds/", typeof(AudioClip)).Cast<AudioClip>().ToList();
	}

    /// <summary>
    /// Play a single clip through the sound effects source.
    /// </summary>	
    public void Play(clips clipName, AudioSource source)
	{
		// Checks the enum in the method parameter to one of the clips in the Resource/Sounds/ folder.
		AudioClip toBePlayedClip = audioClips.Where(clip => clip.name.Contains(clipName.ToString())).FirstOrDefault();

		coreBlock = GameObject.FindWithTag("CoreBlock");

		if (allUtilityParts.Count == 0)
		allUtilityParts = coreBlock.GetComponent<ActivatePartActions>().allUtilityParts;

		// Checks each active utility part before playing the sound
		// and stops the audioSource if multiple of the same Utility part are activated.
		foreach (UtilityPart utilityPart in allUtilityParts)
        {
			if (utilityPart.GetComponent<AudioSource>().isPlaying && 
				utilityPart.GetComponent<AudioSource>().clip.name == clipName.ToString())
            {
				Stop(utilityPart.GetComponent<AudioSource>());
			}
        }

		source.clip = toBePlayedClip;
		source.Play();
	}

	/// <summary>
	/// Stops the sounds played by the audio source.
	/// </summary>
	/// <param name="source"></param>
    public void Stop(AudioSource source)
    {
        source.Stop();
    }

    /// <summary>
    /// Play a single clip through the music source.
    /// </summary>
    /// <param name="clip"></param>
    public void PlayMusic(AudioClip clip)
	{
		//MusicSource.clip = clip;
		//MusicSource.Play();
	}
}
