using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World8BuffObject : MonoBehaviour {

	[SerializeField] private ParticleSystem particle;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.GetComponent<World8Buffs> () != null && !other.GetComponent<World8Buffs> ().buffed) {
			other.GetComponent<World8Buffs> ().Buff ();
		} else if (other.tag == "Ground") {
			GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;
		}
		GetComponent<Animator> ().SetTrigger ("End");
		particle.Stop ();
		Destroy (this.gameObject, 1f);
	}
}
