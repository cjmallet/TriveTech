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

	[HideInInspector]public List<GameObject> audioSourceObjects;
	public GameObject audioSourceObjectToPool;
	public int amountToPool;

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

    private void Start()
    {
		audioSourceObjects = new List<GameObject>();

		GameObject tmp;

		for (int i = 0; i < amountToPool; i++)
		{
			tmp = Instantiate(audioSourceObjectToPool, GameObject.Find("AudioManager").transform);
			tmp.SetActive(false);
			audioSourceObjects.Add(tmp);
		}
	}

	/// <summary>
	/// Method that gets a not used audio source object from the audio source ObjectPool
	/// </summary>
	/// <returns></returns>
	public GameObject GetPooledAudioSourceObject()
	{
		for (int i = 0; i < amountToPool; i++)
		{
			if (!audioSourceObjects[i].activeInHierarchy)
			{
				return audioSourceObjects[i];
			}
		}
		return null;
	}

	/// <summary>
	/// Play a single clip through the sound effects source.
	/// </summary>	
	public void Play(clips clipName, AudioSource source)
	{
		// Checks and matches the enum in the method parameter to one of the clips in the Resource/Sounds/ folder.
		AudioClip toBePlayedClip = audioClips.Where(clip => clip.name.Contains(clipName.ToString())).FirstOrDefault();

		coreBlock = GameObject.FindWithTag("CoreBlock");

		// If a utlity part needs to play a sound do the check to see if there is overlap in the audio.
		if (clipName.ToString().Contains("Boost") || clipName.ToString().Contains("Spring")) 
        {
			NoOverlappingUtilityAudio(clipName);
		}
		// Start a coroutine for disabling of the audioSource when the destructible clip is done playing
		else if (clipName.ToString().Contains("Destructible"))
        {			
            StartCoroutine(WaitForEndOfSound(source.gameObject, toBePlayedClip.length));
        }

		source.clip = toBePlayedClip;
		source.Play();
	}

	/// <summary>
	/// Enumerator that disables the audio source from the audioSourcesPool after the duration of the playing clip
	/// </summary>
	public IEnumerator WaitForEndOfSound(GameObject audioSourceObj, float clipDuration)
	{
		yield return new WaitForSeconds(clipDuration);

		if (audioSourceObj != null)
        {
			audioSourceObj.SetActive(false);
		}
	}

	/// <summary>
	/// Method that checks is there is overlapping utility audio playing, if so disable the excess audiosources
	/// </summary>
	/// <param name="clipName"></param>
	private void NoOverlappingUtilityAudio(clips clipName)
    {
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
