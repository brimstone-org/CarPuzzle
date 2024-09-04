using UnityEngine;
using System.Collections;

public class SoundObject : MonoBehaviour {

	[SerializeField]
	protected AudioSource sound;

	protected virtual void Awake(){
		UpdateSettings ();
	}

	protected virtual void OnEnable(){
		if (OptionsManager.instance == null)
			return;
		OptionsManager.instance.onSoundChange += UpdateSettings;
	}

	protected virtual void OnDisable(){
		if (OptionsManager.instance == null)
			return;
		OptionsManager.instance.onSoundChange -= UpdateSettings;
	}

	protected void UpdateSettings(){
		if (OptionsManager.instance == null)
			return;
		sound.enabled = OptionsManager.instance.hasSound;
	}
		
}
