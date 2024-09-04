using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundButton : MonoBehaviour {

	[SerializeField]
	Image image;

	[SerializeField]
	Sprite soundOn;
	[SerializeField]
	Sprite soundOff;

	void Awake(){
		UpdateVisuals ();
	}

	public void OnClick(){
		OptionsManager.instance.ToggleSound ();
		UpdateVisuals ();
	}

	void UpdateVisuals(){
		if (OptionsManager.instance.hasSound)
			image.sprite = soundOn;
		else
			image.sprite = soundOff;
	}
}
