using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGameObject : MonoBehaviour {

	public GameObject spawnObject;
	public Transform[] spawnPoint;

	public float fireRateMin;
	public float fireRateMax;
	public float fireRateTemp;
	public float offSet;
	private float attackTime;

	void Awake() {
		fireRateTemp = Random.Range(fireRateMin, fireRateMax);
		attackTime = Time.time + offSet + fireRateTemp;
	}



	void Update () {
		if (Time.time >= attackTime) {
			for (int i = 0; i < spawnPoint.Length; i++) {
				GameObject eSpawnObject = Instantiate (spawnObject, spawnPoint [i].position, spawnPoint [i].rotation) as GameObject;
			}
			fireRateTemp = Random.Range (fireRateMin, fireRateMax); 
			attackTime = Time.time + fireRateTemp;
		}
	}
}
