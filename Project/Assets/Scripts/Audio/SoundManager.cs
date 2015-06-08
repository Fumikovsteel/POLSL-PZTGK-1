﻿using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	public AudioClip[] BGMClipArray;

	// Use this for initialization
	void Awake () {
		Zelda._Common._GameplayEvents._OnSceneWillChange += OnSceneWillChange;
	}

	private void OnSceneWillChange(SceneManager.ESceneName newScene)
	{
		if (newScene != SceneManager.ESceneName.Game)
			Destroy(gameObject);
	}
	
	private void OnDestroy()
	{
		if (Zelda._Common != null)
			Zelda._Common._GameplayEvents._OnSceneWillChange -= OnSceneWillChange;
	}

	public void PlaySound(AudioClip clip)
	{
		//efxSource.clip = clip;
		//efxSource.Play();
	}

	public void PlayMusic(Collider zone)
	{
		for (int i = 0; i < BGMClipArray.Length; i++) 
		{
			if (zone.gameObject.name == BGMClipArray[i].name)
			{
				AudioSource musicSource = gameObject.AddComponent<AudioSource>();
				musicSource.volume = 1;
				musicSource.loop = true;
				musicSource.clip = BGMClipArray[i];
				musicSource.Play ();
			}
		}
	}

	public void MusicFadeOut(Collider zone)
	{
		AudioSource[] audioSourceArray;
		audioSourceArray = GetComponents<AudioSource>();
		for (int i = 0; i < audioSourceArray.Length; i++) 
		{
			if (zone.gameObject.name == audioSourceArray[i].clip.name)
			{
				iTween.AudioTo(gameObject, iTween.Hash(
					"audiosource", audioSourceArray[i],
					"volume", 0,
					"time", 2f,
					"easetype", iTween.EaseType.easeOutQuad
					));
				Destroy(audioSourceArray[i], 3);
			}
		}
	}
}