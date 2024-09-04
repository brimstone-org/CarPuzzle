using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoadSceneScript : MonoBehaviour {

    public string nextScene;

    public bool loadOnStart = false;

    public CarSelection carSelection;

	public void Start()
    {
        if(loadOnStart)
            SceneManager.LoadScene(nextScene);
    }

    public void Load()
    {
        SceneManager.LoadScene(nextScene);
        /*
        CarScript[] carsScripts = GameLogic.instance.cars;
        List<int> indexes = new List<int>();
        for (int i = 0; i < carsScripts.Length; i++)
            if (carsScripts[i] != null)
                indexes.Add(i);

        carSelection.ShowForReset(indexes);
        */
    }
}
