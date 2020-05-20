using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMantis : MonoBehaviour {

	public int crashDamage;
	public int penet;
	private bool attacked;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.GetComponent<Barricade> () != null) {
			other.GetComponent<Barricade> ().GetHurt (crashDamage, 3f, 0.2f);
			GetComponent<Monster_Basic> ().penetration = penet;
			if (other.GetComponent<Castle> () == null) {
				GetComponent<Monster_Basic> ().anim.SetTrigger ("RangeAwake");
				GetComponent<MonsterMeleeAttack> ().enabled = false;
				this.enabled = false;
			} else {
				GetComponent<Monster_Basic> ().anim.SetTrigger ("MeleeAwake");
				GetComponent<MonsterRangeAttack> ().enabled = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!attacked && GetComponent<Monster_Basic> ().curHealth < GetComponent<Monster_Basic> ().oriHealth) {
			attacked = true;
			GetComponent<Monster_Basic> ().moveSpeed = 0;
			GetComponent<Monster_Basic> ().moveSpeedTemp = 0;
			GetComponent<Monster_Basic> ().penetration = penet;
			GetComponent<MonsterRangeAttack> ().attackTime = Time.time;
			GetComponent<Monster_Basic> ().anim.SetTrigger ("RangeAwake");
			GetComponent<MonsterMeleeAttack> ().enabled = false;
			this.enabled = false;
		}
	}
}
