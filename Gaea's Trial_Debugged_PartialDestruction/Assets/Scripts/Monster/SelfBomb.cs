using UnityEngine;
using System.Collections;

public class SelfBomb : MonoBehaviour {

	public int damage;
	public GameObject destroyParticle;
	public float destroytime = 0.5f;
	public Barricade barricade;
	public float cameraShakeMultiplier;
	public float cameraShakeLength = 0.1f;

	[SerializeField] private Animator anim;

	public void Awake () {
		anim = GetComponent<Animator> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			if (barricade == null) {
				barricade = other.GetComponent<Barricade> ();
			}
			if (GetComponent<Monster_Basic> () != null && GetComponent<Monster_Basic> ().audioSource!= null && GetComponent<Monster_Basic> ().sound.Length != 0) {
				GetComponent<Monster_Basic> ().PlayAudioSource (GetComponent<Monster_Basic> ().sound [0].audioClip);
			}

			barricade.GetHurt (damage, cameraShakeMultiplier, cameraShakeLength);
			if (destroyParticle != null) {
				GameObject particle = Instantiate (destroyParticle, this.transform.position, this.transform.rotation) as GameObject;
				Destroy (particle, 3f);
			}
			anim.SetBool ("Dead", true);
			if (GetComponent<Monster_Basic> () != null) {
				GameMaster.gameMaster.remainingEnemies.Remove (this.gameObject);
				GameMaster.gameMaster.StartCheckFinished (0.1f);
			}
			Destroy (this.gameObject, destroytime);
		}

	}
}
