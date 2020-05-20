using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionTest : MonoBehaviour {

	public Transform temp;
	private Vector3 tempVector1 = new Vector3 (0, 0, 0);
	private Vector3 tempVector2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetMouseButton (0)) {
			tempVector1 = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			tempVector2 = new Vector3 (tempVector1.x, tempVector1.y, tempVector1.z + 10f);
			temp.position = tempVector2;
		}
	}
}
