using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
	[HideInInspector] public AudioSource musicSource1;
	[HideInInspector] public AudioSource musicSource2;

	public static AudioManager Instance = null;

	[HideInInspector]public List<AudioClip> audioClips = new List<AudioClip>();
	private GameObject coreBlock;
	private List<UtilityPart> allUtilityParts = new List<UtilityPart>();
	private Button[] buttonList;

	[HideInInspector]public List<GameObject> audioSourceObjects;
	public GameObject audioSourceObjectToPool;
	public int amountToPool;

	public clips currentMusicClip;
	[Range(0f, 0.5f)]
	public float musicVolume;
	public float timeToFade;
	private float timeElapsed = 0f;

	public float accelerationSoundTime;
	public float decelerationSoundTime;

	[HideInInspector] public Vector3 vehicleMove;

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
		LevelComplete,
		GameOver,
		BuildingMusic,
		DrivingMusic,
		EngineSound,
		MenuButtonClick,
		MissileExplosion
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
		AudioListener.volume = 1;
	}

	private void Start()
	{
		musicSource1 = gameObject.AddComponent<AudioSource>();
		musicSource2 = gameObject.AddComponent<AudioSource>();

		musicSource1.loop = true;
		musicSource2.loop = true;

		SetMusic(currentMusicClip);

		buttonList = GameObject.FindObjectsOfType<Button>();

		foreach (Button button in buttonList)
		{
			button.onClick.AddListener(() =>
			{
				AudioManager.Instance.MenuButtonClickSound();
			});
		}

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
		else if (clipName.ToString().Contains("Destructible") || clipName.ToString().Contains("MissileExplosion"))
        {			
            StartCoroutine(WaitForEndOfSound(source.gameObject, toBePlayedClip.length));
        }

		source.clip = toBePlayedClip;
		if (source.isActiveAndEnabled)
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
			if (utilityPart.GetComponent<AudioSource>() != null)
            {
				if (utilityPart.GetComponent<AudioSource>().isPlaying &&
					utilityPart.GetComponent<AudioSource>().clip.name == clipName.ToString())
				{
					Stop(utilityPart.GetComponent<AudioSource>());
				}
			}
		}
	}


	public IEnumerator EngineSounds()
    {
		coreBlock = GameObject.FindWithTag("CoreBlock");
		AudioClip engineSound = audioClips.Where(clip => clip.name.Contains(clips.EngineSound.ToString())).FirstOrDefault();
		coreBlock.GetComponent<AudioSource>().clip = engineSound;
		coreBlock.GetComponent<AudioSource>().Play();

		float t = 0;

		while (GameManager.Instance.stateManager.CurrentGameState == GameStateManager.GameState.Playing)
        {
			while (vehicleMove.z != 0)
			{
				coreBlock.GetComponent<AudioSource>().pitch = Mathf.Lerp(0.5f, 1.4f, t / accelerationSoundTime);
				t += Time.deltaTime;

				if (t >= accelerationSoundTime)
					t = accelerationSoundTime;
				yield return null; 
			}
			while (vehicleMove.z == 0)
			{
				coreBlock.GetComponent<AudioSource>().pitch = Mathf.Lerp(0.5f, 1.4f, t / decelerationSoundTime);
				t -= Time.deltaTime;

				if (t <= 0)
					t = 0;
				yield return null; 
			}

			yield return null;
		}
	}

	/// <summary>
	/// Play a single clip through the looping music source.
	/// </summary>
	/// <param name="clip"></param>
	public void SetMusic(clips clipName)
	{
		// Checks and matches the enum in the method parameter to one of the clips in the Resource/Sounds/ folder.
		AudioClip toBePlayedClip = audioClips.Where(clip => clip.name.Contains(clipName.ToString())).FirstOrDefault();

		StopAllCoroutines();
		StartCoroutine(FadeMusic(clipName, toBePlayedClip));
	}

	/// <summary>
	/// The coroutine that handles the fading in and out of the building and driving soundtracks.
	/// </summary>
	/// <param name="clipName"></param>
	/// <param name="toBePlayedClip"></param>
	/// <returns></returns>
	private IEnumerator FadeMusic(clips clipName, AudioClip toBePlayedClip)
	{
		timeElapsed = 0f;

		if (clipName == clips.BuildingMusic)
        {
			musicSource1.clip = toBePlayedClip;
			musicSource1.Play();
			
            while (timeElapsed < timeToFade)
            {
				musicSource1.volume = Mathf.Lerp(0, musicVolume, timeElapsed / timeToFade);
				musicSource2.volume = Mathf.Lerp(musicVolume, 0, timeElapsed / timeToFade);
				timeElapsed += Time.deltaTime;
				yield return null;
			}

			musicSource2.Stop();
		}
		else if (clipName == clips.DrivingMusic)
        {
			musicSource2.clip = toBePlayedClip;
			musicSource2.Play();

			while (timeElapsed < timeToFade)
			{
				musicSource2.volume = Mathf.Lerp(0, musicVolume, timeElapsed / timeToFade);
				musicSource1.volume = Mathf.Lerp(musicVolume, 0, timeElapsed / timeToFade);
				timeElapsed += Time.deltaTime;
				yield return null;
			}

			musicSource1.Stop();
		}
	}

	public void MenuButtonClickSound()
    {
		GameObject audioSource = AudioManager.Instance.GetPooledAudioSourceObject();
		audioSource.transform.localPosition = gameObject.transform.position;
		audioSource.SetActive(true);

		Play(clips.MenuButtonClick, audioSource.GetComponent<AudioSource>());

		StartCoroutine(WaitForEndOfSound(audioSource, audioSource.GetComponent<AudioSource>().clip.length));
    }

	/// <summary>
	/// Stops the sounds played by the audio source.
	/// </summary>
	/// <param name="source"></param>
	public void Stop(AudioSource source)
	{
		source.Stop();
	}
}
