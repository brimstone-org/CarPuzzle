using UnityEngine;
using System.Collections;

public class RateButton : MonoBehaviour {

	public void OnClick(){
		if (Tedrasoft_Rate.RatePlugin.instance != null)
			Tedrasoft_Rate.RatePlugin.instance.OpenRatePanel ();
	}
}
