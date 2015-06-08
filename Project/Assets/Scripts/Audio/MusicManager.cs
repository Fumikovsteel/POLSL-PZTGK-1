using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
	
	public AudioClip[] BGMClipArray;
	
	// Use this for initialization
	void Awake () {
		Zelda._Common._GameplayEvents._OnSceneWillChange += OnSceneWillChange;
	}
	
	private void OnSceneWillChange(SceneManager.ESceneName newScene)
	{
		if (newScene != SceneManager.ESceneName.Game) 
		{
			AudioSource[] audioSourceArray = GetComponents<AudioSource>();
			foreach (AudioSource source in audioSourceArray)
			{
				Destroy(source);
			}
		}
	}
	
	private void OnDestroy()
	{
		if (Zelda._Common != null)
			Zelda._Common._GameplayEvents._OnSceneWillChange -= OnSceneWillChange;
	}
	
	public void PlayMusic(Collider zone)
	{
		for (int i = 0; i < BGMClipArray.Length; i++) 
		{
			if (zone.gameObject.name == BGMClipArray[i].name)
			{
				AudioSource musicSource = gameObject.AddComponent<AudioSource>();
				musicSource.volume = 0.5f;
				musicSource.loop = true;
				musicSource.clip = BGMClipArray[i];
				musicSource.Play ();
			}
		}
	}
	
	public void MusicFadeOut(Collider zone)
	{
		AudioSource[] audioSourceArray =  GetComponents<AudioSource>();
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