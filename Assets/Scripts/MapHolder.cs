using UnityEngine;
using System.Collections;

public class MapHolder : MonoBehaviour {

    public static MapHolder instance { get; private set; }

    public int sizeX, sizeY;

    public BaseTile[,] map;

	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
        }
        map = new BaseTile[sizeX, sizeY];
	}
}
