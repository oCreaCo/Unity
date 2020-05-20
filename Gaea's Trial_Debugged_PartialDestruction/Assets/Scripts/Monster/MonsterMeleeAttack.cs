using UnityEngine;
using System.Collections;

public class MonsterMeleeAttack : MonoBehaviour {

	public int damage;
	public float fireRate;
	public float fireRateTemp;
	public float offSet;
	public float attackTime;
	public float cameraShakeMultiplier;
	public float cameraShakeLength = 0.1f;

	public bool collided;
	public bool shouldAttackBarricade;
	public bool attack;

	private Animator anim;
	public Barricade barricade;

	void Awake() {
		fireRateTemp = fireRate;
		anim = GetComponent<Animator> ();
	}

	void Update () {
		if (shouldAttackBarricade && !GetComponent<Monster_Basic>().dead && !GetComponent<Monster_Basic>().stoppedBool) {
			if (Time.time >= attackTime) {
				attackTime = Time.time + fireRateTemp;
				anim.SetTrigger ("Attack");
			}
		}
	}

	public void MeleeAttack () {
		if (barricade != null) {
			barricade.GetHurt (damage, cameraShakeMultiplier, cameraShakeLength);
		}
	}
}
