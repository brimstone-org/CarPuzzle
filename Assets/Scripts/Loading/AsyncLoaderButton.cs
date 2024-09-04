using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncLoaderButton : MonoBehaviour {

	public void OnClick(string scene)
    {
        PlayerPrefs.SetString(AsyncLoader.LoadingScenePref, scene);
        UnityEngine.SceneManagement.SceneManager.LoadScene(AsyncLoader.LoadingSceneName);
    }

    public static void LoadScene(string scene)
    {
        PlayerPrefs.SetString(AsyncLoader.LoadingScenePref, scene);
        UnityEngine.SceneManagement.SceneManager.LoadScene(AsyncLoader.LoadingSceneName);
    }
}
