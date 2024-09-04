using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public class AsyncLoader : MonoBehaviour {

    AsyncOperation load;
    [SerializeField]
    Text loadingText;

    private static StringBuilder builder = new StringBuilder();

    public const string LoadingSceneName = "Loading";
    public const string LoadingScenePref = "p_loading";

    public void Start()
    {
        string scene = PlayerPrefs.GetString(LoadingScenePref);
        loadingText.text = "";
        LoadAsync(scene);
    }

    public void LoadAsync(string scene)
    {
       load = SceneManager.LoadSceneAsync(scene);
    }

}
