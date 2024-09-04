using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioPanel : MonoBehaviour {

    public GameObject standardPanel;
    public GameObject ultraWidePanel;

    public float ultraWideRatio = 1.85f;

	// Use this for initialization
	void Start () {
        CheckAspect();
	}

    void CheckAspect()
    {
        if (Camera.main.aspect >= ultraWideRatio)
        {
            standardPanel.SetActive(false);
            ultraWidePanel.SetActive(true);
        }
        else
        {
            standardPanel.SetActive(true);
            ultraWidePanel.SetActive(false);
        }
    }

#if UNITY_EDITOR

    private void Update()
    {
        CheckAspect();
    }
#endif

}
