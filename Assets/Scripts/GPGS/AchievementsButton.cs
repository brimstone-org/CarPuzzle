using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class AchievementsButton : MonoBehaviour {

	public void OnClick()
    {
		Social.ShowAchievementsUI ();
	}
}
