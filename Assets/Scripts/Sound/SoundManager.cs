using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance { get; private set; }

	[SerializeField]
	AudioSource backgroundMusic;

	[SerializeField]
	float musicMaxVolume = 0.3f;

	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
			UpdateSettings ();
		}
		else
			Destroy (gameObject);
	}
		
	void OnEnable(){
		OptionsManager.instance.onSoundChange += UpdateSettings;
	}

	void OnDisable(){
		OptionsManager.instance.onSoundChange -= UpdateSettings;
	}

	void UpdateSettings(){
		backgroundMusic.enabled = OptionsManager.instance.hasSound;
		backgroundMusic.volume = musicMaxVolume * OptionsManager.instance.musicVolume;
	}

}
