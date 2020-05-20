using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	
	public bool first, last;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Ground") {
			Destroy (this.gameObject);
		}
	}
}
