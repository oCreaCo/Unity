using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	public bool toggle;

	void OnMouseEnter () {
		toggle = true;
	}

//	void OnMouseExit () {
//		toggle = false;
//	}
	
	// Update is called once per frame
	void Update () {
		if (toggle) {
			if (Input.GetMouseButton (0)) {
				this.transform.position = Input.mousePosition;
			}
			if (Input.GetMouseButtonUp (0)) {
				toggle = false;
			}
		}
	}
}
