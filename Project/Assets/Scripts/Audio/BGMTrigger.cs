using UnityEngine;
using System.Collections;

public class BGMTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider zone)
	{
<<<<<<< HEAD
		Zelda._Common._SoundManager.PlayMusic(zone);
=======
        if (SoundManager.instance != null)
		    SoundManager.instance.PlayMusic(zone);
>>>>>>> origin/master
	}

	void OnTriggerExit(Collider zone)
	{
<<<<<<< HEAD
		Zelda._Common._SoundManager.MusicFadeOut(zone);
=======
        if (SoundManager.instance != null)
		    SoundManager.instance.MusicFadeOut(zone);
>>>>>>> origin/master
	}
}