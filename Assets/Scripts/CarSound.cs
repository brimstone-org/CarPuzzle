using UnityEngine;
using System.Collections;

public class CarSound : SoundObject {

	[SerializeField]
	CarScript car;

	[SerializeField]
	float waitingPitch = 0.5f;
	[SerializeField]
	float runningPitch = 1.2f;
	//[SerializeField]
	//AudioSource sound;
	[SerializeField]
	float pitchTransitionTime = 0.5f;

	bool lastWait = false;

	protected override void Awake(){
		base.Awake ();
		sound.pitch = 0;
	}

	void Update(){

		if (GameLogic.instance.losePanel.activeInHierarchy || GameLogic.instance.winPanel.activeInHierarchy)
			sound.enabled = false;

		if (lastWait != car.wait) {
			if (car.wait)
				StartCoroutine (SmoothPitch (waitingPitch));
			else
				StartCoroutine (SmoothPitch (runningPitch));
		}
		lastWait = car.wait;
	}
		

	IEnumerator SmoothPitch(float targetPitch){
		float pitch = sound.pitch;
		float time = 0;
		while (time < pitchTransitionTime) {
			time += Time.deltaTime;
			sound.pitch = Mathf.Lerp (pitch, targetPitch, time / pitchTransitionTime);
			yield return null;
		}
	}

	public void Reset(){
		sound.pitch = 0;
		lastWait = false;
		if(OptionsManager.instance != null)
			sound.enabled = OptionsManager.instance.hasSound;

	}

	protected override void OnEnable(){
		GameLogic.instance.resetGameEvent += Reset;
		base.OnEnable ();
	}

	protected override void OnDisable(){
		base.OnDisable ();
		GameLogic.instance.resetGameEvent -= Reset;
	}

}
