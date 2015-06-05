using UnityEngine;
using System.Collections;

public class BGMTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider zone)
	{
        if (SoundManager.instance != null)
		    SoundManager.instance.PlayMusic(zone);
	}

	void OnTriggerExit(Collider zone)
	{
        if (SoundManager.instance != null)
		    SoundManager.instance.MusicFadeOut(zone);
	}
}