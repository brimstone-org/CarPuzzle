using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RandomSprite : MonoBehaviour {

	[SerializeField]
	MeshRenderer meshRenderer;
	[SerializeField]
	Image image;

	public Material main;
	public Material second;

	public Sprite mainSprite;
	public Sprite secondSprite;

	public bool useSprites = false;

	[Range(0,100)]
	public float chance = 15f;

	// Use this for initialization
	void Start () {
		int number = Random.Range (0, 100);

		if (!useSprites) {
			if (number <= chance)
				meshRenderer.sharedMaterial = second;
			else
				meshRenderer.sharedMaterial = main;
		} else {
			if (number <= chance)
				image.sprite = secondSprite;
			else
				image.sprite = mainSprite;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
