using UnityEngine;
using System.Collections;

namespace Tedrasoft_Rate
{

	public class RateButtonWrapper : MonoBehaviour
	{

		public void OnClick ()
		{
			if (RatePlugin.instance != null)
				RatePlugin.instance.OpenRatePanel ();
		}
	}
}