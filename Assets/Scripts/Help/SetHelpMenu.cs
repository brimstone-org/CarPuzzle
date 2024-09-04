using UnityEngine;
using System.Collections;

public class SetHelpMenu : MonoBehaviour {

	[SerializeField]
	LoadSceneScript script;

	void Start(){
		PlayerPrefs.SetInt ("FromMenu", 0);
		if (PlayerPrefs.GetInt ("HadHelp") == 0)
			script.nextScene = "Help";
	}

	public void OnClick(){
		PlayerPrefs.SetInt ("FromMenu", 1);
	}
}
