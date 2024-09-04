using UnityEngine;
using System.Collections;

public class RoadLightTile : BaseTile {

	private int greenTime = 2;
	private int redTime = 2;
	private int stepTime = 0;

	public int GreenTime{
		get { return greenTime; }
		set { greenTime = value; 
			if (greenTime < 1)
				greenTime = 1;
		}
	}

	public int RedTime{
		get { return redTime; }
		set { redTime = value; 
			if (redTime < 2)
				redTime = 2;
		}
	}

	public int startingStepTime { get; set; }

	[SerializeField]
	private float carOnTileThreshold = 0.3f;

	[SerializeField]
	private GameObject greenSprite;
	[SerializeField]
	private GameObject yellowSprite;
	[SerializeField]
	private GameObject redSprite;

	[SerializeField]
	GameObject[] people;

    //public bool green{ get; private set; }
	private bool startingGreen;
    private bool green;
	public bool Green {
		get{ return green; }
		set{ green = startingGreen = value; }
	}

	// Use this for initialization
	protected override void Start () {
		/*tile.startingStepTime = startingStepTime;
		tile.redTime = redTime;
		tile.greenTime = greenTime;
		tile.green = green;*/
        base.Start();

		passable = true;
		//stepTime = startingStepTime;
		//TimeStep ();
		StartingColorSettings();
	}

	void Reset(){
		green = startingGreen;
		StartingColorSettings();
	}

	protected override void OnEnable(){
		base.OnEnable ();
		GameLogic.instance.resetGameEvent += Reset;
	}

	protected override void OnDisable(){
		base.OnDisable ();
		GameLogic.instance.resetGameEvent -= Reset;
	}

	public void StartingColorSettings(){
		stepTime = startingStepTime;

		//change to green
		if (green) {
			greenSprite.SetActive (true);
			yellowSprite.SetActive (false);
			redSprite.SetActive (false);
			return;
		}
		//change to yellow
		if (!green && stepTime == 0) {
			greenSprite.SetActive (false);
			yellowSprite.SetActive (true);
			redSprite.SetActive (false);
			return;
		}
		//change to red
		if (!green) {
			greenSprite.SetActive (false);
			yellowSprite.SetActive (false);
			redSprite.SetActive (true);
			return;
		}
	}

	public override void TimeStep(){
		//Debug.Log ("Road Tile Step");
		stepTime++;

		if ((!green && redTime - stepTime >= 2) || (green && stepTime >= greenTime)) {
			foreach (var p in people) {
				int rand = Random.Range (0, 100);
				if (rand >= 50)
					p.SetActive (true);
			}
		}

		//change to yellow / stop moving
		if (green && stepTime >= greenTime) {
			green = false;
			greenSprite.SetActive (false);
			yellowSprite.SetActive (true);
			redSprite.SetActive (false);
			stepTime = 0;

			return;
		}
		//change to red
		if (!green && stepTime == 1) {
			greenSprite.SetActive (false);
			yellowSprite.SetActive (false);
			redSprite.SetActive (true);
			return;
		}
		//change to green
		if (!green && stepTime >= redTime) {
			green = true;
			stepTime = 0;
			greenSprite.SetActive (true);
			yellowSprite.SetActive (false);
			redSprite.SetActive (false);
			return;
		}
	}

	public override void TileEffect(CarScript car){

		if (Vector3.Distance (car.transform.position, transform.position) < carOnTileThreshold)
			foreach (var p in people) {
				if (p.activeInHierarchy)
					p.SetActive (false);
			}

        //Debug.Log(Vector3.Dot(car.transform.right, transform.right));
		if (Vector3.Dot (car.transform.right, transform.right) < 0.7f) {
			return;
		}
			
		if (green) {
			car.wait = false;
		} else {
			car.wait = true;
		}
	}

	public override void TileUpdate(CarScript car){
		if (Vector3.Dot (car.transform.right, transform.right) < 0.7f)
			foreach (var p in people) {
				if (p.activeInHierarchy)
					p.SetActive (false);
			}
	}
}
