using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {

	public int damage;
	public GameObject destroyParticle;
	public float cameraShakeMultiplier;
	public float cameraShakeLength = 0.1f;

	// Use this for initialization
	void Awake () {
	
	}

	void OnCollisionEnter2D (Collision2D collision) {
		Castle castle = collision.collider.GetComponent<Castle> ();
		Barricade barricade = collision.collider.GetComponent<Barricade> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.GetComponent<Barricade>() != null) {
			other.GetComponent<Barricade>().GetHurt (damage, cameraShakeMultiplier, cameraShakeLength);
			if (destroyParticle != null) {
				GameObject particle = Instantiate (destroyParticle) as GameObject;
				Destroy (particle, 3f);
			}
			Destroy (this.gameObject);
		}
	}
}
