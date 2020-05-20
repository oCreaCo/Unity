using UnityEngine;
using System.Collections;

public class HorizontalRandom : MonoBehaviour {
	public float xMin, xMax, y, z;

	// Use this for initialization
	void Awake () {
		this.transform.position = new Vector3 (Random.Range (xMin, xMax), y, z);
	}
}
