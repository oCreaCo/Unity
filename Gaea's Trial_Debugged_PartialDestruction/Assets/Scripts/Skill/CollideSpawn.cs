using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideSpawn : MonoBehaviour {

	public string sound;
	public Transform effect;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.GetComponent<Monster_Basic> ()) {
//			AudioManager.instance.PlaySound (sound);
			Instantiate (effect, this.transform.position, this.transform.rotation);
			GetComponent<Collider2D> ().enabled = false;
			Destroy (this.gameObject, 0);
		}
		if (other.tag == "End") Destroy (this.gameObject);
	}
}
