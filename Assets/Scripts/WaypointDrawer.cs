using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointDrawer : MonoBehaviour
{

    public static WaypointDrawer instance { get; private set; }

    public int AStarMaximumPathLength = 4;

    public CarScript[] cars { get; private set; }

    public LayerMask waypointLayer;
    public LayerMask carLayer;

    private bool selectionPhase = false;
    public bool SelectionPhase
    {
        get { return selectionPhase; }
        set {
            selectionPhase = value;
            StartSelection();
        }
    }

    public bool inputEnabled = true;

    private int selectedCar;
    public int SelectedCar
    {
        get {
            return selectedCar;
        }
        set {
            /*for (int i = 0; i < cars.Length; i++) {
                if (cars[i] == null)
                    continue;
                cars[i].circle.gameObject.SetActive(false);
            }*/
            selectedCar = value;
            cars[selectedCar].circle.gameObject.SetActive(true);
            SelectedMenu.instance.UpdateValues(cars[selectedCar].carSprite, cars[selectedCar].speed, cars[selectedCar].startingTime);
            lastHit = new RaycastHit2D();
        }
    }

    public bool dragging { get; private set; }
    public bool adding { get; private set; }
    public bool removing { get; private set; }

    private RaycastHit2D lastHit;

    [System.Serializable]
    struct selectInfo
    {
        public int x;
        public int y;
        public void Reset() {
            x = -1;
            y = -1;
        }
    }

    [SerializeField]
    selectInfo lastSelect;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

    }

    void Start() {

    }

    void StartSelection() {
        cars = GameLogic.instance.cars;
        lastSelect.x = -1;
        lastSelect.y = -1;
        for (int i = 0; i < cars.Length; i++)
            if (cars[i] != null) {
                selectedCar = i;
                SelectedCar = i;
            }
    }

    // Update is called once per frame
    void Update() {

        if (GameLogic.instance.playing)
            return;

        if (!selectionPhase)
            return;

        if (!inputEnabled)
            return;

        if (Input.GetMouseButtonDown(0)) {
            SelectCar(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetMouseButton(0)) {
            UpdateTileList(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            cars[selectedCar].UpdateCircle();
        }

        if (Input.GetMouseButtonUp(0)) {
            dragging = false;
            adding = false;
            removing = false;
            cars[selectedCar].UpdateCircle();
        }
    }

    public void SelectCar(Vector3 position) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector3.forward, 2, carLayer);
        List<int> selectableCars = new List<int>();

        for (int i = 0; i < hits.Length; i++) {
            for (int j = 0; j < cars.Length; j++) {
                if (cars[j] == null)
                    continue;
                if ((hits[i].transform == cars[j].transform || hits[i].transform.parent == cars[j].transform) && !selectableCars.Contains(j)) {
                    selectableCars.Add(j);
                    //break;
                }
            }
        }

        if (selectableCars.Count == 0)
            return;

        //Debug.Log (selectableCars.Count);
        if (selectableCars.Count == 1) {
            SelectedCar = selectableCars[0];
        } else if (selectableCars.Count > 1) {
            //Debug.Log (cars [selectableCars [0]].transform.position);
            CarScript car = cars[selectableCars[0]];
            if (lastSelect.x == (int)car.checkPointDraw[car.checkPointDraw.Count - 1].transform.position.x && lastSelect.y == (int)car.checkPointDraw[car.checkPointDraw.Count - 1].transform.position.y)
                return;
            CarSelection.instance.Show(selectableCars);
            lastSelect.x = (int)car.checkPointDraw[car.checkPointDraw.Count - 1].transform.position.x;
            lastSelect.y = (int)car.checkPointDraw[car.checkPointDraw.Count - 1].transform.position.y;
        }
    }

    public void UpdateTileList(Vector3 position) {
        if (CarSelection.instance.isActive)
            return;

        var hit = Physics2D.Raycast(position, Vector3.forward, 1f, waypointLayer);
        //Debug.Log (hit.transform);
        //Debug.Log (lastHit.transform);

        if (hit.transform != lastHit.transform || !dragging) {
            lastHit = hit;

            CarScript car = cars[selectedCar];
            //Debug.Log (selectedCar);

            Transform targetTransform = car.checkPoints[car.checkPoints.Count - 1].transform;
            //Debug.Log (targetTransform);

            if (hit.transform == null)
                return;

            if (hit.transform == targetTransform) {
                //Debug.Log (hit.transform);
                //Debug.Log (targetTransform);
                dragging = !dragging;
                return;
            }

            bool shouldRemove = false;
            for (int i = 0 > car.checkPoints.Count - AStarMaximumPathLength ? 0 : car.checkPoints.Count - AStarMaximumPathLength; i < car.checkPoints.Count - 1; i++) {
                if (hit.transform == car.checkPoints[i].transform && dragging && !adding)
                    shouldRemove = true;
            }

            if (shouldRemove) {
                removing = true;
                bool doneRemoving = false;
                int i = car.checkPoints.Count - 1;
                while (!doneRemoving) {
                    lastSelect.Reset();
                    car.RemoveTile(i);
                    i--;
                    if (car.checkPoints[i].transform == hit.transform)
                        doneRemoving = true;

                }
                return;
            }

            bool nextToLast = false;

            float rayLength = 1f;

            var hit1 = Physics2D.Raycast(hit.transform.position + Vector3.up, Vector3.forward, rayLength, waypointLayer);
            if (hit1.transform == targetTransform)
                nextToLast = true;
            hit1 = Physics2D.Raycast(hit.transform.position + Vector3.down, Vector3.forward, rayLength, waypointLayer);
            if (hit1.transform == targetTransform)
                nextToLast = true;
            hit1 = Physics2D.Raycast(hit.transform.position + Vector3.left, Vector3.forward, rayLength, waypointLayer);
            if (hit1.transform == targetTransform)
                nextToLast = true;
            hit1 = Physics2D.Raycast(hit.transform.position + Vector3.right, Vector3.forward, rayLength, waypointLayer);
            if (hit1.transform == targetTransform)
                nextToLast = true;

            if (nextToLast && Vector3.Distance(car.transform.position - car.transform.right, hit.transform.position) <= 1f && car.checkPoints.Count <= 1) {

                return;
            }

            if (nextToLast && !removing && dragging) {
                BaseTile nextTile = hit.transform.GetComponent<BaseTile>();
                if ((car.checkPoints.Count > 1 && car.checkPoints[car.checkPoints.Count - 2] != nextTile || car.checkPoints.Count == 1) && MapHolder.instance.map[nextTile.x,nextTile.y].Passable) {
                    car.AddTile(nextTile);
                    adding = true;
                }
            }


            if (!nextToLast && !removing && dragging) {
                //Debug.Log ("ASTAR");

                Vector3 back = car.transform.position - car.transform.right;

                //Debug.Log (car.transform.position + " / " + car.transform.right);

                //Debug.Log (back.x + " " + back.y);

                if (car.checkPoints.Count > 1)
                    back = -Vector3.one;

                List<BaseTile> list = AStar.instance.finder.Find(Mathf.RoundToInt(targetTransform.position.x), Mathf.RoundToInt(targetTransform.position.y), Mathf.RoundToInt(hit.transform.position.x), Mathf.RoundToInt(hit.transform.position.y), Mathf.RoundToInt(back.x), Mathf.RoundToInt(-back.y));

                if (list.Count == 0)
                    return;

                if (list.Count > AStarMaximumPathLength)
                    return;

                int lastCommonItem = -1;
                for (int i = 0; i < list.Count; i++) {
                    if (car.checkPoints.Count - 1 - i >= 0 && car.checkPoints[car.checkPoints.Count - 1 - i] == list[i])
                        lastCommonItem = i;
                }
                for (int i = 0; i < list.Count; i++) {
                    if (i == lastCommonItem)
                        continue;
                    if (car.checkPoints.Count != 0 && car.checkPoints[car.checkPoints.Count - 1].Equals(list[i]))
                        car.RemoveTile(car.checkPoints.Count - 1);
                    else if(!list[lastCommonItem].Equals(car.checkPoints[0]) || (!list[lastCommonItem].Equals(car.checkPoints[0]) && car.checkPoints.Count > 1))
                        car.AddTile(list[i]);
                    lastSelect.Reset();
                    adding = true;
                }
            }

        } else {
            removing = false;
            adding = false;
        }
    }

    public void NextCar() {
        if (selectedCar + 1 < cars.Length) {
            SelectedCar++;
        } else {
            SelectedCar = 0;
        }
    }

    public void PreviousCar() {
        if (selectedCar - 1 >= 0) {
            SelectedCar--;
        } else {
            SelectedCar = cars.Length - 1;
        }
    }

    public void ResetSelectedCar() {
        CarScript c = cars[selectedCar];
        for(int i = c.checkPoints.Count-1; i > 0; i--) {
            c.RemoveTile(i);
        }
    }


    public void ResetCar(int index)
    {
        CarScript c = cars[index];
        for (int i = c.checkPoints.Count - 1; i > 0; i--)
        {
            c.RemoveTile(i);
        }
        c.Reset();
        c.UpdateCircle();
    }
}
