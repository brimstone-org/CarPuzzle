using UnityEngine;
using System.Collections;

public class UiElementFloat : MonoBehaviour
{

    RectTransform rt;
    float time = 0;

    [SerializeField]
    float distance = 2f;

    Vector3 position;

    [SerializeField]
    string prefString;

    // Use this for initialization
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        position = rt.localPosition;

        if (!string.IsNullOrEmpty(prefString))
        {
            float x = PlayerPrefs.GetFloat(prefString + "x", position.x);
            float y = PlayerPrefs.GetFloat(prefString + "y", position.y);
            float z = PlayerPrefs.GetFloat(prefString + "z", position.z);
            position = new Vector3(x, y, z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rt.localPosition = position + new Vector3(0, Mathf.Sin(time) * distance, 0);
        time += Time.deltaTime;
    }

    private void OnDisable()
    {
        if (!string.IsNullOrEmpty(prefString))
        {
            PlayerPrefs.SetFloat(prefString + "x", rt.localPosition.x);
            PlayerPrefs.SetFloat(prefString + "y", rt.localPosition.y);
            PlayerPrefs.SetFloat(prefString + "z", rt.localPosition.z);
        }
    }
}
