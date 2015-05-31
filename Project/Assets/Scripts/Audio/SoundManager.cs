using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {


	public AudioSource musicSource;
	public AudioSource musicSource2;
	public AudioSource efxSource;
	public static SoundManager instance = null;
	public bool fading1 = false;
	public bool fading2 = false;
	public AudioClip VillageBGM;
	public AudioClip ForestBGM;
	public AudioClip CatacombEntranceBGM;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	public void PlaySound(AudioClip clip)
	{
		efxSource.clip = clip;
		efxSource.Play();
	}

	public void PlayMusic(Collider zone)
	{
		if (zone.CompareTag("VillageAudioTrigger"))
		{
			musicSource.volume = 0;
			musicSource.clip = VillageBGM;
			musicSource.Play ();
			fading1 = false;
		}
		if (zone.CompareTag("ForestAudioTrigger"))
		{
			musicSource2.volume = 0;
			musicSource2.clip = ForestBGM;
			musicSource2.Play ();
			fading2 = false;
		}
		if (zone.CompareTag("CatacombEntranceAudioTrigger"))
		{
			musicSource.volume = 0;
			musicSource.clip = CatacombEntranceBGM;
			musicSource.Play ();
			fading1 = false;
		}
	}

	public void MusicFadeOut(Collider zone)
	{
		if (zone.CompareTag("ForestAudioTrigger"))
			fading2 = true;
		else
			fading1 = true;
	}

	void Update()
	{
		if (!fading1 && musicSource.volume < 1)
		{
			musicSource.volume += 0.5f * Time.deltaTime;
		}

		if (fading1 && musicSource.volume > 0)
		{
			musicSource.volume -= 0.5f * Time.deltaTime;
		}

		if (!fading2 && musicSource2.volume < 1)
		{
			musicSource2.volume += 0.5f * Time.deltaTime;
		}
		
		if (fading2 && musicSource2.volume > 0)
		{
			musicSource2.volume -= 0.5f * Time.deltaTime;
		}
	}
}