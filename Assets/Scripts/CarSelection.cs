using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour {

	public static CarSelection instance;

	public GameObject holder;
    public string nextScene;
    public bool isActive{ get; private set; }

	[SerializeField]
	private List<SelectableObject> entries;

	void Awake()
    {
		instance = this;
		isActive = false;
	}
		

	public void Show(List<int> cars)
    {
		ClearEntries ();

		for (int i = 0; i < cars.Count; i++) {
			entries [i].carNumber = cars [i];
			entries [i].sprite.sprite = WaypointDrawer.instance.cars [cars [i]].carSprite;
			entries [i].gameObject.SetActive (true);
		}

		holder.SetActive (true);
		isActive = true;
	}

    public void ShowForReset()
    {
        CarScript[] carsScripts = GameLogic.instance.cars;
        List<int> indexes = new List<int>();
        for (int i = 0; i < carsScripts.Length; i++)
            if (carsScripts[i] != null)
                indexes.Add(i);

        ShowForReset(indexes);
    } 

    public void ShowForReset(List<int> cars)
    {
        ClearEntries();

        for (int i = 0; i < cars.Count; i++)
        {
            entries[i].carNumberOrg = cars[i];
            entries[i].carNumber = cars[i];

            entries[i].GetComponent<Toggle>().isOn = true;

            entries[i].sprite.sprite = LevelBuilder.instance.cars[cars[i]].GetComponent<CarScript>().carSprite;
            entries[i].gameObject.SetActive(true);
        }

        holder.SetActive(true);
        isActive = true;
    }

    public void ReloadScene()
    {
        bool reloadAll = true;

        int index = -1;
        List<CarScript> toReset = new List<CarScript>();
        for(int i = 0;  i < GameLogic.instance.cars.Length; i++)
        {
            CarScript c = GameLogic.instance.cars[i];
            if (c != null)
            {
                index++;
                Debug.Log(i);
                if(entries[index].carNumber == -1)
                {
                    Debug.Log("f");
                    reloadAll = false;
                }
                else
                {
                    Debug.Log("Reset:" + c.name);
                    WaypointDrawer.instance.ResetCar(i);
                    //toReset.Add(c);
                }
            }
        }
        
        Close();

        /*
        if (reloadAll)
            SceneManager.LoadScene(nextScene);
        else
        {
            toReset.ForEach(y => {
                Debug.Log("Reset:" + y.name);
                y.Reset();

            });
            Close();
        }
        */
            
    }

    public void Close(){
		holder.SetActive (false);
		isActive = false;
	}

	void ClearEntries(){
		for (int i = 0; i < entries.Count; i++) {
			entries [i].gameObject.SetActive (false);
		}
	}

}
