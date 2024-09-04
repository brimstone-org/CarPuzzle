using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectableObject : MonoBehaviour {
	public int carNumber = -1;
    public int carNumberOrg = -1;
	public Image sprite;

	public void OnClick()
    {
		WaypointDrawer.instance.SelectedCar = carNumber;
		CarSelection.instance.Close ();
	}

    public void Toogle(bool ok)
    {
        if (ok)
            carNumber = carNumberOrg;
        else
            carNumber = -1;
    }

}
