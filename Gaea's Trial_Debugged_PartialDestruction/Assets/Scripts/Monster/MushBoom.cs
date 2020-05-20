using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushBoom : MonoBehaviour {

	public float fireRateMin;
	public float fireRateMax;
	public float fireRateTemp;
	public float offSet;
	private float attackTime;
	public int tier = 0;
	public bool charging = false;
	public GameObject[] booms;
	public Transform boomSpawnPoint;
	private Vector2 vector2 = new Vector2 ();

	private Animator anim;

	void Awake () {
		fireRateTemp = Random.Range(fireRateMin, fireRateMax);
		anim = GetComponent<Animator> ();
		attackTime = Time.time + offSet + fireRateTemp;
		vector2.x = -2;
		vector2.y = 4;
	}

	void Update () {
		if (GetComponent<Monster_Basic> ().damageMonsterType == Monster_Basic.DamageMonsterType.BASIC) {
			if (Time.time >= attackTime && !GetComponent<Monster_Basic> ().stoppedBool) {
				anim.SetTrigger ("Charge");
				GetComponent<Monster_Basic> ().stoppedBool = true;
			}
		}
	}

	public void Charging () {
		charging = true;
		GetComponent<Monster_Basic> ().hurtAnimatable = true;
	}

	public void TierUp () {
		tier++;
	}

	public void Shoot () {
		GameObject boom = Instantiate (booms [tier], boomSpawnPoint.position, boomSpawnPoint.rotation) as GameObject;
		tier = 0;
		charging = false;
		GetComponent<Monster_Basic> ().hurtAnimatable = false;
		fireRateTemp = Random.Range (fireRateMin, fireRateMax); 
		attackTime = Time.time + fireRateTemp;
		GetComponent<Monster_Basic> ().stoppedBool = false;
	}
}
