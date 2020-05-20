using UnityEngine;
using System.Collections;

public class HorizontalRandomStop : MonoBehaviour {

	public float xMin, xMax, y, z;
	[SerializeField] private Vector3 stopPosition;

	// Use this for initialization
	void Awake () {
		stopPosition = new Vector3 (Random.Range (xMin, xMax), y, z);
	}

	void Update () {
		if (this.transform.position.x <= stopPosition.x) {
			GetComponent<Monster_Basic> ().moveSpeed = 0;
			GetComponent<Monster_Basic> ().moveSpeedTemp = 0;
			Destroy (this);
		}
	}
}
