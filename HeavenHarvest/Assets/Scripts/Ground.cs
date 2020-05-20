using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

	public GameObject rainParticle;
	[SerializeField] private GameObject waterDropSound;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Rain") {
			Instantiate (waterDropSound);
			GameObject rainParticleObj = Instantiate (rainParticle, this.transform.position, Quaternion.Euler(-100, 0, 0)) as GameObject;
			Destroy (rainParticleObj, 1f);
		}
	}
}
