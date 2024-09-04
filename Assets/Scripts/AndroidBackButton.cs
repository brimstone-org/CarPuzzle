using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AndroidBackButton : MonoBehaviour
{

    [SerializeField]
    string sceneName;

    [SerializeField]
    bool exit;

    IEnumerator ExitRoutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Application.Quit();
    }

    //void Quit(ChartboostSDK.CBLocation location = null)
    //{
    //    Application.Quit();
    //}

    //void Quit(ChartboostSDK.CBLocation location, ChartboostSDK.CBImpressionError error)
    //{
    //    Application.Quit();
    //}
}
