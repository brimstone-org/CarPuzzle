using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EditorObjective : MonoBehaviour {

	LevelSerializer.ObjectiveInfo objective = new LevelSerializer.ObjectiveInfo();

	public int objectiveId;
	public bool isDestination;

	void Start(){
		if(LevelSerializer.instance != null)
			LevelSerializer.instance.onWrite += PrepareLevelData;
	}

    void OnDisable() {
        if (LevelSerializer.instance != null)
            LevelSerializer.instance.onWrite -= PrepareLevelData;
    }

    protected virtual void PrepareLevelData(){

		objective.objectiveId = objectiveId;
		objective.isDestination = isDestination;
		objective.x = (int)transform.position.x;
		objective.y = (int)-transform.position.y;
        objective.rotation = (int)transform.rotation.eulerAngles.z;

		LevelSerializer.instance.level.objectives.Add (objective);
	}

	void Update(){
		Vector3 position = transform.position;
		position.x = Mathf.RoundToInt (position.x);
		position.y = Mathf.RoundToInt (position.y);
		transform.position = position;
	}
}
