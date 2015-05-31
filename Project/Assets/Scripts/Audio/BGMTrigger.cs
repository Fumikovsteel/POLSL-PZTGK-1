using UnityEngine;
using System.Collections;

public class BGMTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider zone)
	{
		SoundManager.instance.PlayMusic(zone);
	}

	void OnTriggerExit(Collider zone)
	{
		SoundManager.instance.MusicFadeOut(zone);
	}
}