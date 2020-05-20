using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrect : MonoBehaviour {

	[SerializeField] private Animator anim;

	void Awake () {
		anim = GetComponent<Animator> ();
	}

	public void Trigger () {
		GetComponent<Monster_Basic> ().dead = false;
		anim.SetBool ("Dead", GetComponent<Monster_Basic> ().dead);
	}

	public void ReAliveSkill () {
		this.tag = "Monster";
		GetComponent<Monster_Basic> ().moveSpeedTemp = GetComponent<Monster_Basic> ().moveSpeed;
		GetComponent<Monster_Basic> ().curHealth = GetComponent<Monster_Basic> ().oriHealth;
		GetComponent<Monster_Basic> ().statusIndicator.SetHealth (GetComponent<Monster_Basic> ().curHealth, GetComponent<Monster_Basic> ().oriHealth);
	}
}
