using UnityEngine;
using System.Collections;

namespace Tedrasoft_Rate{

	public class RatePlugin : MonoBehaviour {

		public static RatePlugin instance { get; private set; }

		public string androidPackageName = "";
		public string iOSAppId = "";
		private string shareLinkAndroid = "https://play.google.com/store/apps/details?id=";
		private string shareLinkiOS = "https://itunes.apple.com/app/id";
		public string shareLinkOther = "";

		public string email = "office@tedrasoft.com";
		public string subjectNormal = "Normal rate";
		public string subjectBad = "Bad rate";

		public Transform rateCanvas;

		public int gamesPlayed = 0;
		public int minGamesBeforeShow = 4;
		public int showRate = 5;

		bool rateEnabled = true;

		void Awake(){
			if (instance == null) {
				instance = this;
				DontDestroyOnLoad(this.gameObject);
			} else if (instance != this) {
				Destroy(this.gameObject);
			}
		}

		// Use this for initialization
		void Start () {
			if (PlayerPrefs.GetInt ("Rated") == 1)
				rateEnabled = false;
			shareLinkAndroid += androidPackageName;
			shareLinkiOS += iOSAppId;
		}

		public void UpdateRate(){
			if (!rateEnabled)
				return;

			gamesPlayed++;
			if ((gamesPlayed - minGamesBeforeShow) % showRate == 0)
				OpenRatePanel ();
		}

		public void OpenRatePanel(){
			rateCanvas.gameObject.SetActive (true);
		}

		public void CloseRatePanel(){
			rateCanvas.gameObject.SetActive (false);
		}

		public void LikeButtonClick(){
			#if UNITY_ANDROID
			Application.OpenURL(shareLinkAndroid);
			#elif UNITY_IOS
			Application.OpenURL(shareLinkiOS);
			#else
			Application.OpenURL(iOSAppId);
			#endif

			PlayerPrefs.SetInt ("Rated", 1);

		}

		public void NormalButtonClick(){
			SendEmail (email, subjectNormal);
		}
		
		public void BadButtonClick(){
			SendEmail (email, subjectBad);
		}

		public void SendEmail (string email, string subject)
		{
			subject = MyEscapeURL(Application.productName + " " + subject);
			Application.OpenURL("mailto:" + email + "?subject=" + subject);
		}
		string MyEscapeURL (string url)
		{
			return WWW.EscapeURL(url).Replace("+","%20");
		}

	}
}
