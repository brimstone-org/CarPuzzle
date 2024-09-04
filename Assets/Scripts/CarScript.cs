using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarScript : MonoBehaviour {

	public List<BaseTile> checkPoints;
	public List<GameObject> checkPointDraw;

	public GameObject checkMarkStraight;
	public GameObject checkMarkCurved;
	public GameObject circle;
    SpriteRenderer circleRenderer;

	public float markingsOffset = -50f;

	public Sprite carSprite;

    public Sprite completedSprite;
    public Sprite incompleteSprite;
    public Sprite neutralSprite;

	public int currentCheckPoint = -1;

	public int remainingObjectives = 0;

	public float speed = 1;

	public int startingTime = 1;

	private bool destinationReached = false;

	public bool DestinationReached{
		get{ return destinationReached; }
		set{
			destinationReached = value;

			if (destinationReached) {
				GameLogic.instance.CheckGameState ();
				gameObject.SetActive (false);
			}
			else
				gameObject.SetActive (true);
		}
	}

    [SerializeField]
    private float percentToNextTile = 0.7f;

	[SerializeField]
	private float currentStepTime = 0;
	public float GetTimeStep{
		get{ return currentStepTime; }
	}
	private Vector3 targetPosition;
	private Vector3 targetRotation;
	private Vector3 startingCheckpointPosition;
	private Quaternion startingCheckpointRotation;

	private Vector3 startingPosition;
	private Quaternion startingRotation;

	public LayerMask layer;

    public bool debug = false;

	public bool wait;

    Color carColor;

    void Awake() {
        circleRenderer = circle.GetComponent<SpriteRenderer>();
        carColor = circleRenderer.color;
    }

	private void Start()
    {
        checkPoints = new List<BaseTile> ();
		checkPointDraw = new List<GameObject> ();
		var hit = Physics2D.Raycast (transform.position, Vector3.forward, 1, layer);
		BaseTile tile = hit.transform.GetComponent<BaseTile> ();
		AddTile (tile);
		destinationReached = false;
		wait = true;
		startingPosition = transform.position;
		startingRotation = transform.rotation;
        //checkPoints.Add (tile);
    }

	public void Reset()
    {
		//gameObject.SetActive (true);
		remainingObjectives = 0;
		wait = true;
		DestinationReached = false;
		transform.position = startingPosition;
		transform.rotation = startingRotation;
		for(int i=0; i<checkPointDraw.Count; i++)
        {
			checkPointDraw[i].transform.parent = transform;
		}
		for (int i = 0; i < checkPointDraw.Count; i++) {
			//checkPointDraw [i].SetActive (false);
			Color color = checkPointDraw [i].GetComponent<SpriteRenderer>().color;
			color.a = 95f/255f;
			checkPointDraw [i].GetComponent<SpriteRenderer> ().color = color;
		}
	}

	void OnGameplayStart(){
        for(int i=0; i<checkPointDraw.Count; i++) {
            checkPointDraw[i].transform.parent = null;
        }

		currentCheckPoint = 0;

		wait = false;


		GameLogic.instance.CarWait (startingTime, gameObject);

		startingCheckpointPosition = checkPoints[0].transform.position;
		startingCheckpointRotation = transform.rotation;

		if (checkPoints.Count <= 1) {
			targetPosition = startingCheckpointPosition;
			targetRotation = startingCheckpointRotation.eulerAngles;
			return;
		}

		targetRotation = transform.localEulerAngles + new Vector3(0, 0, CalculateRotationAngle(checkPoints[currentCheckPoint + 1]));
		targetPosition = checkPoints[currentCheckPoint + 1].transform.position;
		wait = false;
		currentStepTime = 0;
		circle.SetActive (false);
		for (int i = 0; i < checkPointDraw.Count; i++) {
			//checkPointDraw [i].SetActive (false);
			Color color = checkPointDraw [i].GetComponent<SpriteRenderer>().color;
			color.a = 0.15f;
			checkPointDraw [i].GetComponent<SpriteRenderer> ().color = color;
		}


	}

	private void OnEnable(){
		GameLogic.instance.stepEvent += TimeStep;
		GameLogic.instance.startGameEvent += OnGameplayStart;
		GameLogic.instance.resetGameEvent += Reset;
	}

	private void OnDisable(){
		GameLogic.instance.stepEvent -= TimeStep;
		GameLogic.instance.startGameEvent -= OnGameplayStart;
		GameLogic.instance.resetGameEvent -= Reset;
	}

    public void UpdateCircle() {
        Vector3 v1 = checkPoints[checkPoints.Count - 1].transform.position;
        v1.z = 0;
        Vector3 v2 = GameLogic.instance.objectives[WaypointDrawer.instance.SelectedCar].transform.position;
        v2.z = 0;
        circleRenderer.color = Color.white;
        if (WaypointDrawer.instance.dragging) {
            circleRenderer.sprite = neutralSprite;
            circleRenderer.color = carColor;
        } else if (Vector3.Distance(v1, v2) < 1)
            circleRenderer.sprite = completedSprite;
        else if (checkPoints.Count > 1)
            circleRenderer.sprite = incompleteSprite;
        else {
            circleRenderer.sprite = neutralSprite;
            circleRenderer.color = carColor;
        }
    }

	public void AddTile(BaseTile tile){
		//Debug.Log (tile.transform);
		GameObject obj = (GameObject)Instantiate (checkMarkStraight, tile.transform.position+Vector3.forward*markingsOffset, Quaternion.identity);
		obj.transform.parent = transform;
		if (checkPoints.Count > 1) {
			Vector3 rotation = Vector3.zero;
			if (checkPoints [checkPoints.Count-1].transform.position.x == tile.transform.position.x)
				rotation.z = 90;
			obj.transform.eulerAngles = rotation;
		} else {
			obj.transform.eulerAngles = transform.eulerAngles;
		}

		obj.SetActive (true);
		if (checkPointDraw.Count > 0) {
			checkPointDraw [checkPointDraw.Count - 1].GetComponent<Collider2D> ().enabled = false;
		}
		checkPoints.Add (tile);
		checkPointDraw.Add (obj);
		obj.GetComponent<Collider2D> ().enabled = true;
		circle.transform.position = tile.transform.position + Vector3.forward*markingsOffset;
		MakeCurves ();
	}

	public void RemoveTile(int index){
		checkPoints.RemoveAt (index);
		Destroy (checkPointDraw [index]);
		checkPointDraw.RemoveAt (index);
		circle.transform.position = checkPoints [checkPoints.Count - 1].transform.position + Vector3.forward * markingsOffset;
		checkPointDraw [checkPointDraw.Count - 1].GetComponent<Collider2D> ().enabled = true;
	}
		
	private void TimeStep(){
		if (currentCheckPoint < 0 || currentCheckPoint == checkPoints.Count-1)
			return;
        //Debug.Log ("Car Time Step");
        checkPoints [currentCheckPoint].TileEffect (this);
	}

	private void NextAction(){
		//currentStepTime = 0;
		currentStepTime -= (GameLogic.instance.StepDuration / speed) * percentToNextTile;
		//Debug.Log (currentStepTime);

		if (!GameLogic.instance.playing)
			return;

        if (destinationReached)
            return;

        if (wait)
            return;

		if (checkPoints.Count <= 1)
			return;

        currentCheckPoint++;
        if (currentCheckPoint == checkPoints.Count - 1) {
            if (!debug) {
                //destinationReached = true;
				GameLogic.instance.CheckGameState ();
				return;
            } else {
                currentCheckPoint = 0;
                transform.position = checkPoints[0].transform.position;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, CalculateRotationAngle(checkPoints[0])));
            }
        }

        //Debug.Log(currentCheckPoint);
		targetRotation = transform.localEulerAngles + new Vector3(0,0,CalculateRotationAngle(checkPoints [currentCheckPoint + 1]));
		targetPosition = checkPoints [currentCheckPoint + 1].transform.position;
		startingCheckpointPosition = transform.position;
		startingCheckpointRotation = transform.rotation;
		checkPoints [currentCheckPoint].TileEffect (this);
	}

	void FixedUpdate(){

        circle.transform.eulerAngles = Vector3.zero;

		if (!GameLogic.instance.playing)
			return;

		if (wait) {
			//currentStepTime = 0;
			return;
		}
		if(currentCheckPoint+1<checkPoints.Count)
			checkPoints [currentCheckPoint+1].TileUpdate (this);

		currentStepTime += Time.fixedDeltaTime;

        if (!destinationReached) {
			transform.rotation = Quaternion.Lerp(startingCheckpointRotation, Quaternion.Euler(targetRotation), currentStepTime / (GameLogic.instance.StepDuration / speed));
			transform.position = Vector3.LerpUnclamped(startingCheckpointPosition, targetPosition, currentStepTime / (GameLogic.instance.StepDuration / speed));
			//rb.position = Vector3.Lerp(startingPosition, targetPosition, currentStepTime / (GameLogic.instance.StepDuration / speed));

        }

        if (currentStepTime >= (GameLogic.instance.StepDuration / speed) * percentToNextTile) {
			NextAction ();
		}
	}

	private float CalculateRotationAngle(BaseTile tile){
		Vector3 direction = transform.InverseTransformPoint (tile.transform.position);
		return 90 * direction.y;
	}

	private void MakeCurves(){
			if (checkPoints.Count <= 2)
				return;

			int lastIndex = checkPoints.Count - 1;
			BaseTile last = checkPoints [lastIndex];
			BaseTile last2 = checkPoints [lastIndex - 1];
			BaseTile last3 = checkPoints [lastIndex - 2];
			if (last.transform.position.y > last2.transform.position.y) {
				if (last2.transform.position.x > last3.transform.position.x) {
					Destroy (checkPointDraw [lastIndex - 1]);
					checkPointDraw [lastIndex - 1] = (GameObject)Instantiate (checkMarkCurved, checkPoints [lastIndex - 1].transform.position, Quaternion.identity);
					checkPointDraw [lastIndex - 1].SetActive (true);
					checkPointDraw [lastIndex - 1].transform.localEulerAngles = new Vector3 (0, 0, 0);
				}else if (last2.transform.position.x < last3.transform.position.x) {
					Destroy (checkPointDraw [lastIndex - 1]);
					checkPointDraw [lastIndex - 1] = (GameObject)Instantiate (checkMarkCurved, checkPoints [lastIndex - 1].transform.position, Quaternion.identity);
					checkPointDraw [lastIndex - 1].SetActive (true);
					checkPointDraw [lastIndex - 1].transform.localEulerAngles = new Vector3 (0, 0, -90);
				}else{
					Destroy (checkPointDraw [lastIndex - 1]);
					checkPointDraw [lastIndex - 1] = (GameObject)Instantiate (checkMarkStraight, checkPoints [lastIndex - 1].transform.position, Quaternion.identity);
					checkPointDraw [lastIndex - 1].SetActive (true);
					checkPointDraw [lastIndex - 1].transform.localEulerAngles = new Vector3 (0, 0, 90);
				}
			} else if (last.transform.position.y < last2.transform.position.y) {
				if (last2.transform.position.x > last3.transform.position.x) {
					Destroy (checkPointDraw [lastIndex - 1]);
					checkPointDraw [lastIndex - 1] = (GameObject)Instantiate (checkMarkCurved, checkPoints [lastIndex - 1].transform.position, Quaternion.identity);
					checkPointDraw [lastIndex - 1].SetActive (true);
					checkPointDraw [lastIndex - 1].transform.localEulerAngles = new Vector3 (0, 0, 90);
				} else if (last2.transform.position.x < last3.transform.position.x) {
					Destroy (checkPointDraw [lastIndex - 1]);
					checkPointDraw [lastIndex - 1] = (GameObject)Instantiate (checkMarkCurved, checkPoints [lastIndex - 1].transform.position, Quaternion.identity);
					checkPointDraw [lastIndex - 1].SetActive (true);
					checkPointDraw [lastIndex - 1].transform.localEulerAngles = new Vector3 (0, 0, 180);
				} else {
					Destroy (checkPointDraw [lastIndex - 1]);
					checkPointDraw [lastIndex - 1] = (GameObject)Instantiate (checkMarkStraight, checkPoints [lastIndex - 1].transform.position, Quaternion.identity);
					checkPointDraw [lastIndex - 1].SetActive (true);
					checkPointDraw [lastIndex - 1].transform.localEulerAngles = new Vector3 (0, 0, 90);
				}
			}else if (last.transform.position.x > last2.transform.position.x) {
				if (last2.transform.position.y < last3.transform.position.y) {
					Destroy (checkPointDraw [lastIndex - 1]);
					checkPointDraw [lastIndex - 1] = (GameObject)Instantiate (checkMarkCurved, checkPoints [lastIndex - 1].transform.position, Quaternion.identity);
					checkPointDraw [lastIndex - 1].SetActive (true);
					checkPointDraw [lastIndex - 1].transform.localEulerAngles = new Vector3 (0, 0, 270);
				} else if (last2.transform.position.y > last3.transform.position.y) {
					Destroy (checkPointDraw [lastIndex - 1]);
					checkPointDraw [lastIndex - 1] = (GameObject)Instantiate (checkMarkCurved, checkPoints [lastIndex - 1].transform.position, Quaternion.identity);
					checkPointDraw [lastIndex - 1].SetActive (true);
					checkPointDraw [lastIndex - 1].transform.localEulerAngles = new Vector3 (0, 0, 180);
				} else {
					Destroy (checkPointDraw [lastIndex - 1]);
					checkPointDraw [lastIndex - 1] = (GameObject)Instantiate (checkMarkStraight, checkPoints [lastIndex - 1].transform.position, Quaternion.identity);
					checkPointDraw [lastIndex - 1].SetActive (true);
					checkPointDraw [lastIndex - 1].transform.localEulerAngles = new Vector3 (0, 0, 0);
				}
			} else if (last.transform.position.x < last2.transform.position.x) {
				if (last2.transform.position.y > last3.transform.position.y) {
					Destroy (checkPointDraw [lastIndex - 1]);
					checkPointDraw [lastIndex - 1] = (GameObject)Instantiate (checkMarkCurved, checkPoints [lastIndex - 1].transform.position, Quaternion.identity);
					checkPointDraw [lastIndex - 1].SetActive (true);
					checkPointDraw [lastIndex - 1].transform.localEulerAngles = new Vector3 (0, 0, 90);
				}else if (last2.transform.position.y < last3.transform.position.y) {
					Destroy (checkPointDraw [lastIndex - 1]);
					checkPointDraw [lastIndex - 1] = (GameObject)Instantiate (checkMarkCurved, checkPoints [lastIndex - 1].transform.position, Quaternion.identity);
					checkPointDraw [lastIndex - 1].SetActive (true);
					checkPointDraw [lastIndex - 1].transform.localEulerAngles = new Vector3 (0, 0, 0);
				}else {
					Destroy (checkPointDraw [lastIndex - 1]);
					checkPointDraw [lastIndex - 1] = (GameObject)Instantiate (checkMarkStraight, checkPoints [lastIndex - 1].transform.position, Quaternion.identity);
					checkPointDraw [lastIndex - 1].SetActive (true);
					checkPointDraw [lastIndex - 1].transform.localEulerAngles = new Vector3 (0, 0, 0);
				}
			}

		checkPointDraw [lastIndex - 1].transform.position += Vector3.forward * markingsOffset;
		checkPointDraw [lastIndex - 1].transform.parent = transform;
	}
}
