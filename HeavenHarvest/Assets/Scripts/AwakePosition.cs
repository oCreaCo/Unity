using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakePosition : MonoBehaviour {

	[SerializeField] private float xMin;
	[SerializeField] private float xMax;
	[SerializeField] private float yMin;
	[SerializeField] private float yMax;
	[SerializeField] private float zMin;
	[SerializeField] private float zMax;

	void Awake () {
		float x = Random.Range (xMin, xMax);
		float y = Random.Range (yMin, yMax);
		float z = Random.Range (zMin, zMax);
		this.transform.position = new Vector3 (x, y, z);
	}
}
