using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour {
	
	public ForceMode2D fMode;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Monster") {
			if (other.GetComponent<Monster_StatusController>() != null) {
//				other.GetComponent<Monster_StatusController> ().KnockBack ();
			}
		}
	}
}
