using UnityEngine;
using System.Collections;

namespace SlimUI.ModernMenu{
	public class CheckMusicVolume : MonoBehaviour {
		public void  Start (){
			// remember volume level from last time
			GetComponent<AudioSource>().volume = 0.5f;  // PlayerPrefs.GetFloat("MusicVolume", 0.5f);
		}

		public void UpdateVolume (){
			GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
		}
	}
}