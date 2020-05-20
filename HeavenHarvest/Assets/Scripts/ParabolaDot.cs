using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaDot : MonoBehaviour {
	
	public Vector3 startPos;

	void Awake () {
		startPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
	}
}
