using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMover : MonoBehaviour {
    [SerializeField]
    List<CloudPath> usablePaths;

    [SerializeField]
    float duration = 20;
    [SerializeField]
    float durationRandomness = 2f;

    List<bool> swap;

    [System.Serializable]
    class CloudPath
    {
        public RectTransform start;
        public RectTransform end;
        public float duration;
        public float currentTime;
        public RectTransform obj;
    }

    void Start() {
        Spawn();
    }

    void Update() {
        for (int i = 0; i < usablePaths.Count; i++) {
            CloudPath path = usablePaths[i];
            path.currentTime += Time.deltaTime;
            path.obj.position = Vector3.Lerp(path.start.position, path.end.position, path.currentTime / path.duration);
            if (path.currentTime > path.duration) {
                path.currentTime = 0;
                path.duration = duration + Random.Range(-durationRandomness, durationRandomness);
            }
        }
    }

    void Spawn() {

        swap = new List<bool>();
        for (int i = 0; i < usablePaths.Count; i++) {
            if (i < usablePaths.Count / 2)
                swap.Add(true);
            else
                swap.Add(false);
        }

        for (int i = 0; i < usablePaths.Count; i++) {
            CloudPath path = usablePaths[i];
            path.duration = duration + Random.Range(-durationRandomness, durationRandomness);
            int sIndex = Random.Range(0, this.swap.Count - 1);
            bool swap = this.swap[sIndex];
            if(swap) {
                SwapTransforms(ref path.start, ref path.end);
            }
            this.swap.RemoveAt(sIndex);
        }
    }

    void SwapTransforms(ref RectTransform v1, ref RectTransform v2) {
        RectTransform aux = v2;
        v2 = v1;
        v1 = aux;
    }
	
}
