using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Tedrasoft_Rate
{

	public class StarScript : MonoBehaviour
	{
		[SerializeField]
		Image[] stars;

		[SerializeField]
		Color pressedColor;

		[SerializeField]
		Color normalColor;

		public void OnMouseOverStart(){
			for (int i = 0; i < stars.Length; i++) {
				stars [i].color = pressedColor;
			}
		}

		public void OnMouseOverEnd(){
			for (int i = 0; i < stars.Length; i++) {
				stars [i].color = normalColor;
			}
		}

	}
}
