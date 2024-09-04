using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Tedrasoft_Rate{

	[RequireComponent(typeof(Text))]
	public class TranstatedText : MonoBehaviour {

	    public string key;

	    Text text;

		private bool skipEnable = true;
	    
	    void Start() {
	        text = GetComponent<Text>();
	        UpdateText();
			skipEnable = false;
	    }

		void OnEnable(){
			if (skipEnable)
				return;
			Debug.Log ("Enable");
			UpdateText ();
		}

	    public void UpdateText() {
			text.font = LanguageManager.instance.GetFont ();
	        text.text = LanguageManager.instance.Get(key);
	    }

	}
}