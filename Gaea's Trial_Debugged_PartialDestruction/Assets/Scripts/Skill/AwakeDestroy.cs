using UnityEngine;
using System.Collections;

public class AwakeDestroy : MonoBehaviour {

	public float destroyTime;
	public string sound;

	void Awake () {
		Destroy (this.gameObject, destroyTime);
	}

	void Start () {
		if (GetComponent<AudioController> () != null) {
			GetComponent<AudioController> ().PlaySound (sound);
		}
	}
}
