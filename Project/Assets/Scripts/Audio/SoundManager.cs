using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	public AudioClip[] SoundsClipArray;
	public enum SoundName
	{
		PlayerAttack, PotionUse
	}

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

	public void PlaySound(SoundName enumClip)
	{
		for (int i = 0; i < SoundsClipArray.Length; i++)
		{
			if (enumClip.ToString() == SoundsClipArray[i].name)
			{
				AudioSource soundSource = gameObject.AddComponent<AudioSource>();
				soundSource.PlayOneShot (SoundsClipArray[i]);
				Destroy (soundSource, SoundsClipArray[i].length);
			}
		}
	}
}