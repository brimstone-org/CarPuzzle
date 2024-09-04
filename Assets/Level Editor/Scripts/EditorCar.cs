using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EditorCar : MonoBehaviour {

	LevelSerializer.CarInfo car = new LevelSerializer.CarInfo();

	public int carId;
	public int startingTime;

	void Start(){
		if(LevelSerializer.instance != null)
			LevelSerializer.instance.onWrite += PrepareLevelData;
	}

    void OnDisable() {
        if (LevelSerializer.instance != null)
            LevelSerializer.instance.onWrite -= PrepareLevelData;
    }

	public virtual void PrepareLevelData(){

		car.carId = carId;
		car.startingX = (int)transform.position.x;
		car.startingY = (int)-transform.position.y;
		car.startingTime = startingTime;
		car.startingRotation = (int)transform.eulerAngles.z;

		LevelSerializer.instance.level.cars.Add (car);
	}

	void Update(){
		Vector3 position = transform.position;
		position.x = Mathf.RoundToInt (position.x);
		position.y = Mathf.RoundToInt (position.y);
		transform.position = position;
	}

}
