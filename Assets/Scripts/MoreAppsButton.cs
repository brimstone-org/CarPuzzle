using UnityEngine;
using System.Collections;

public class MoreAppsButton : MonoBehaviour {

    public void OnClick()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Tedra+Apps");
#elif UNITY_IOS
        Application.OpenURL("https://itunes.apple.com/cv/developer/dragos-cosmineanu/id952227558");
#endif
    }
}