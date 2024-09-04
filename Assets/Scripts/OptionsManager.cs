using UnityEngine;
using System.Collections;

public class OptionsManager : MonoBehaviour {

	public static OptionsManager instance { get; private set; }

	public bool hasSound{ get; private set; }
	public float musicVolume {get; private set;}

	public delegate void OnSoundChange();
	public event OnSoundChange onSoundChange;

	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
			Initialize ();
		}
		else
			Destroy (gameObject);
	}

	void Initialize(){

		if (PlayerPrefs.HasKey ("hasSound")) {
			int s = PlayerPrefs.GetInt ("hasSound");
			hasSound = s == 1 ? true : false;
		} else {
			hasSound = true;
		}
		if (PlayerPrefs.HasKey ("musicVolume"))
			musicVolume = PlayerPrefs.GetFloat ("musicVolume");
		else
			musicVolume = 1;
	}

	public void ToggleSound(){
		hasSound = !hasSound;
		int s = hasSound == true ? 1 : 0;
		PlayerPrefs.SetInt ("hasSound", s);
		onSoundChange ();
	}

	public void MusicVolume(float vol){
		musicVolume = vol;
		PlayerPrefs.SetFloat ("musicVolume", vol);
		onSoundChange ();
	}
		
}
