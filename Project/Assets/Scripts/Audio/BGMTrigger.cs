using UnityEngine;
using System.Collections;

public class BGMTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider zone)
	{
		Zelda._Common._SoundManager.PlayMusic(zone);
	}

	void OnTriggerExit(Collider zone)
	{
		Zelda._Common._SoundManager.MusicFadeOut(zone);
	}
}