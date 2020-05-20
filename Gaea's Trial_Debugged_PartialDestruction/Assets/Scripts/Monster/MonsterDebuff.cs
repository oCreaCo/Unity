using UnityEngine;
using System.Collections;

public class MonsterDebuff : MonoBehaviour {

	[SerializeField] private bool stackDebuff; 
	public float stackDebuffRate;
	public float stackDebuffDelay;
	public float stackDebuffTime;
	[SerializeField] private GameObject snowParticle, iceParticle;

	private Animator anim;

	public void StackDebuff () {
		StartCoroutine (StackDebuffCouroutine (stackDebuffDelay));
	}

	IEnumerator StackDebuffCouroutine (float delay) {
		snowParticle.SetActive (true);
		iceParticle.SetActive (true);
		GameMaster.gameMaster.stackModifiable = false;
		yield return new WaitForSeconds (delay);
		snowParticle.GetComponent<ParticleSystem> ().Stop ();
		iceParticle.GetComponent<ParticleSystem> ().Stop ();
		GameMaster.gameMaster.stackModifiable = true;
		yield return new WaitForSeconds (1.0f);
		snowParticle.SetActive (false);
		iceParticle.SetActive (false);
	}

	void Awake() {
		anim = GetComponent<Animator> ();
	}

	void Update () {
		if (stackDebuff) {
			if (Time.time >= stackDebuffTime) {
				stackDebuffTime = Time.time + stackDebuffRate;
				anim.SetTrigger ("Attack");
//				StackDebuff ();
			}
		}
	}
}