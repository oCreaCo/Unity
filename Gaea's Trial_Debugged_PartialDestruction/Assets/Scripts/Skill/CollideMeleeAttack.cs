using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollideMeleeAttack : MonoBehaviour {

	public int damage;
	public float fireRate;
	public float fireRateTemp;
	public float offSet;
	public float attackTime;
	public bool triggered;

	public List<GameObject> triggeredMonster = new List<GameObject> ();

	[SerializeField] private MoveScript moveScript;
	[SerializeField] private Animator anim;

	void Awake () {
		fireRateTemp = fireRate;
		anim = this.transform.GetComponent<Animator> ();
		moveScript = this.transform.parent.GetComponent<MoveScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!moveScript.move && moveScript.charged) {
			if (Time.time >= attackTime) {
				attackTime = Time.time + fireRateTemp;
				anim.SetTrigger ("Attack");
			}
		}
	}

	public void Attack () {
		for (int i = 0; i < triggeredMonster.Count; i++) {
			triggeredMonster [i].GetComponent<Monster_Basic> ().DamageMonster (damage);
		}
	}
}
