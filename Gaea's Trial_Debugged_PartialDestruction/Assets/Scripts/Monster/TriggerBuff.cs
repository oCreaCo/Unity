using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBuff : MonoBehaviour {

	public float buffTime;
	public float buffSpeed;
	public int type;
	public bool autoDestroy;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.GetComponent<Monster_StatusController>() != null) {
			other.GetComponent<Monster_StatusController> ().SpeedBuff (buffTime, buffSpeed, true, type);
			if (autoDestroy) Destroy (this.gameObject, 5f);
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.GetComponent<Monster_StatusController>() != null) {
			other.GetComponent<Monster_StatusController> ().SpeedBuff (buffTime, buffSpeed, false, type);
		}
	}
}
