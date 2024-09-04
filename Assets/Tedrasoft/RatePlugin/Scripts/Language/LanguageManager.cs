// http://www.bloodirony.com/blog/how-to-support-multiple-languages-in-unity


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Tedrasoft_Rate{

	public class LanguageManager : MonoBehaviour {

		public string language = "English";
	    private string languageTag = "";
		public bool useDeviceLanguage = false;
	    public static LanguageManager instance { get; private set; }

		public bool debugging = false;
		public string debuggingTag = "_ro";

	    Dictionary<string, string> fields;

		public Dictionary<string, string> languageTags;

		public Font defaultFont;

		[System.Serializable]
		public class FontCategory{
			public Font font;
			public List<string> languages;
		}

		public List<FontCategory> specialFonts;

	    // Use this for initialization
	    void Awake () {
	        if (instance == null) {
	            instance = this;
	            DontDestroyOnLoad(this.gameObject);
	        } else if (instance != this) {
	            Destroy(this.gameObject);
	        }
				
	        fields = new Dictionary<string, string>();

			languageTags = new Dictionary<string, string> ();
			languageTags.Add (SystemLanguage.English.ToString(), "");
			//languageTags.Add (SystemLanguage.Romanian.ToString(), "_ro");
			languageTags.Add (SystemLanguage.German.ToString(), "_de");
			languageTags.Add (SystemLanguage.French.ToString(), "_fr");
			languageTags.Add (SystemLanguage.Spanish.ToString (), "_es");
			languageTags.Add (SystemLanguage.Portuguese.ToString (), "_pt");
			languageTags.Add (SystemLanguage.Turkish.ToString (), "_tr");
			languageTags.Add (SystemLanguage.Russian.ToString (), "_ru");
			//languageTags.Add (SystemLanguage.Chinese.ToString (), "_cn_tr");
			//languageTags.Add (SystemLanguage.ChineseTraditional.ToString (), "_cn_tr");
			languageTags.Add (SystemLanguage.ChineseSimplified.ToString(), "_zh_cn");
			languageTags.Add (SystemLanguage.Japanese.ToString (), "_ja");
			languageTags.Add (SystemLanguage.Korean.ToString (), "_kr");
			languageTags.Add (SystemLanguage.Italian.ToString (), "_it");
			languageTags.Add (SystemLanguage.Dutch.ToString (), "_nl");


			if (useDeviceLanguage && languageTags.ContainsKey(Application.systemLanguage.ToString())) {
				languageTag = languageTags [Application.systemLanguage.ToString ()];

			} else {
				languageTag = languageTags [language];
			}

			if (debugging)
				languageTag = debuggingTag;

	        LoadLanguage(languageTag);
	    }

		public void SetLanguage(string language){
			if (debugging)
				return;
			this.language = language;
			languageTag = languageTags [language];
			LoadLanguage (languageTag);
		}

	    private void LoadLanguage(string lang) {
	        fields.Clear();
	        TextAsset textAsset = (TextAsset)Resources.Load("Tedrasoft/RatePlugin/values/values" + languageTag);
	        string allTexts = "";
	        if (textAsset == null) {
				textAsset = (TextAsset)Resources.Load("Tedrasoft/RatePlugin/values/values");
	        }
	        allTexts = textAsset.text;
	        string[] lines = allTexts.Split(new string[] { "\r\n", "\n" },
	            System.StringSplitOptions.None);
	        string key, value;
	        for (int i = 0; i < lines.Length; i++) {
	            if (lines[i].IndexOf("=") >= 0 && !lines[i].StartsWith("#")) {
	                key = lines[i].Substring(0, lines[i].IndexOf("="));
	                value = lines[i].Substring(lines[i].IndexOf("=") + 1,
	                        lines[i].Length - lines[i].IndexOf("=") - 1).Replace("\\n", System.Environment.NewLine);
	                fields.Add(key, value);
	            }
	        }
	    }

	    public string Get(string key) {
	        return fields[key];
	    }

		public Font GetFont(){
			for (int i = 0; i < specialFonts.Count; i++) {
				if(specialFonts[i].languages.Contains(language))
					return specialFonts[i].font;
			}
			return defaultFont;
		}

	}
}