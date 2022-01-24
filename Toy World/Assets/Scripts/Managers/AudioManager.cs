using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	//public AudioSource MusicSource;

	public static AudioManager Instance = null;

	[HideInInspector]public List<AudioClip> audioClips = new List<AudioClip>();
	private ActivatePartActions coreBlock;
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
	}

    private void Start()
    {
		audioClips = Resources.LoadAll("Sounds/", typeof(AudioClip)).Cast<AudioClip>().ToList();
		coreBlock = FindObjectsOfType<ActivatePartActions>().First();	
	}

    /// <summary>
    /// Play a single clip through the sound effects source.
    /// </summary>	
    public void Play(clips clipName, AudioSource source)
	{
		AudioClip toBePlayedClip = audioClips.Where(clip => clip.name.Contains(clipName.ToString())).FirstOrDefault();

		if (allUtilityParts.Count == 0)
		allUtilityParts = coreBlock.allUtilityParts;

		Debug.Log(allUtilityParts.Count);

		foreach (UtilityPart utilityPart in allUtilityParts)
        {
			Debug.Log(utilityPart.gameObject.name);
			if (utilityPart.GetComponent<AudioSource>().isPlaying && 
				utilityPart.GetComponent<AudioSource>().clip.name == clipName.ToString())
            {
				Debug.Log("test");
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
