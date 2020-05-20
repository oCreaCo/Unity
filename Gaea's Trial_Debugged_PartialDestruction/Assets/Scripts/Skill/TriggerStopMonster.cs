using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStopMonster : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other) {
		if (other.GetComponent<Monster_Basic> () != null) {
			other.GetComponent<Monster_Basic> ().moveSpeedTemp = 0;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.GetComponent<Monster_Basic> () != null) {
			other.GetComponent<Monster_Basic> ().moveSpeedTemp = other.GetComponent<Monster_Basic> ().moveSpeed;
		}
	}
}
