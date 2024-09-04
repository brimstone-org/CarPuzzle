using UnityEngine;
using System.Collections;

public abstract class BaseTile : MonoBehaviour {

	//protected LevelSerializer.TileInfo tile = new LevelSerializer.TileInfo ();

    protected bool passable;
    public bool Passable
    {
        get { return passable; }
        set
        {
            passable = value;
        }
    }

	public int x{ get; private set; }
	public int y{ get; private set; }

	private int rotation = 0;
	public int Rotation{
		get{ return rotation; }
		set{
			rotation = value % 4;
			transform.localEulerAngles = new Vector3 (0, 0, 90 * rotation);
		}
	}

	protected virtual void Start () {
		passable = false;
        x = (int)transform.position.x;
        y = (int)-transform.position.y;
        //Debug.Log("X: " + x + " Y: " + y);



        if (MapHolder.instance.map[x, y] == null)
        {
            MapHolder.instance.map[x, y] = this;
        }
        else if (this.GetType().Equals(typeof(BuildingTile)))
        {
            Debug.Log(this);
            MapHolder.instance.map[x, y] = this;
        }
		//PrepareLevelData ();
	}
	/*protected virtual void PrepareLevelData(){
		Debug.Log ("Preparing Level Data");

		tile.tileId = LevelBuilder.instance.GetId (transform.name);
		tile.rotation = (int)transform.localEulerAngles.z;
		tile.x = x;
		tile.y = y;
		LevelSerializer.instance.level.tiles.Add (tile);
	}*/

	protected virtual void Update () {
	}

	public abstract void TimeStep ();

	public abstract void TileEffect (CarScript car);

	public abstract void TileUpdate (CarScript car);

	protected virtual void OnEnable(){
		GameLogic.instance.stepEvent += TimeStep;
	}

	protected virtual void OnDisable(){
		GameLogic.instance.stepEvent -= TimeStep;
	}

	public void SetMatrixPosition(int x, int y){
		this.x = y;
		this.y = x;
	}
}
