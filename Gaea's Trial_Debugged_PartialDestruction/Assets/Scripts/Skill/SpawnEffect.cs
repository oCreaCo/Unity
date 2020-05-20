using UnityEngine;
using System.Collections;

public class SpawnEffect : MonoBehaviour {

	public float x, y, z;
	public GameObject effect;
	private Vector3 thisPosition;

	void Awake () {
		thisPosition = new Vector3 (x, y, z);
		if (effect != null) {
			Instantiate (effect, thisPosition, this.transform.rotation);
		}
	}

	void Update () {
		
	}
}
