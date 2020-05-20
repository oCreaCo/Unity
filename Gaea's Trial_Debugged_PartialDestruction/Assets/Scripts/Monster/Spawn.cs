using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	public GameObject spawnObject;
	public Transform[] spawnPoint;

	public float fireRateMin;
	public float fireRateMax;
	public float fireRateTemp;
	public float offSet;
	private float attackTime;

	private Animator anim;

	void Awake() {
		fireRateTemp = Random.Range(fireRateMin, fireRateMax);
		anim = this.transform.GetComponent<Animator> ();
		attackTime = Time.time + offSet + fireRateTemp;
	}

	void Update () {
		if (GetComponent<Monster_Basic> ().damageMonsterType == Monster_Basic.DamageMonsterType.BASIC) {
			if (Time.time >= attackTime && !GetComponent<Monster_Basic> ().stoppedBool) {
				for (int i = 0; i < spawnPoint.Length; i++) {
					GameObject eSpawnObject = Instantiate (spawnObject, spawnPoint [i].position, spawnPoint [i].rotation) as GameObject;
				}
				fireRateTemp = Random.Range (fireRateMin, fireRateMax); 
				attackTime = Time.time + fireRateTemp;
			}
		}
	}
}
