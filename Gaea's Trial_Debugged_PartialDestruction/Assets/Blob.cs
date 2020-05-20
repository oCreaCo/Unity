using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour {

	public float x, y;
	Vector2 vector2 = new Vector2 ();

	void OnCollisionEnter2D (Collision2D other) {
		if (!GetComponent<Monster_Basic> ().dead) {
			if (other.transform.tag == "Ground") {
				GetComponent<Animator> ().SetTrigger ("Awake");
			}
		}
	}

	public void Die () {
		x = Random.Range (-0.5f, 2f);
		y = Random.Range (3f, 5f);
		vector2.x = x;
		vector2.y = y;
		GetComponent<Rigidbody2D> ().AddForce (vector2, ForceMode2D.Impulse);
	}
}
