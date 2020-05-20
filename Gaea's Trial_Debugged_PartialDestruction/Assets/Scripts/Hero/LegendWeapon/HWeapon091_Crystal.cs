using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HWeapon091_Crystal : MonoBehaviour {

	public GameObject[] letters;
	public Animator anim;

	public void Awake () {
		anim = GetComponent<Animator> ();
	}

	public void First() {
		anim.SetTrigger ("Top");
		letters [0].SetActive (false);
	}

	public void Second() {
		anim.SetTrigger ("Mid");
		letters [1].SetActive (false);
	}

	public void Third() {
		anim.SetTrigger ("Mid");
		letters [2].SetActive (false);
	}

	public void Fourth() {
		anim.SetTrigger ("Bot");
		letters [3].SetActive (false);
	}
}
