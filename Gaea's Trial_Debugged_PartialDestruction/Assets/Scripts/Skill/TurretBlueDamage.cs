using UnityEngine;
using System.Collections;

public class TurretBlueDamage : MonoBehaviour {

	public GameObject projectile;
	public Transform[] projectileSpawnPoint;

	public float fireRateMin;
	public float fireRateMax;
	public float fireRateTemp;
	public float offSet;
	private float attackTime;

	public void Awake () {
		fireRateTemp = Random.Range(fireRateMin, fireRateMax);
		attackTime = Time.time + offSet + fireRateTemp;
	}

	public void Update () {
		if (Time.time >= attackTime) {
			Attack ();
			fireRateTemp = Random.Range(fireRateMin, fireRateMax); 
			attackTime = Time.time + fireRateTemp;
		}
	}

	public void Attack () {
		for (int i = 0; i < projectileSpawnPoint.Length; i++) {
			GameObject missile = Instantiate (projectile, projectileSpawnPoint [i].position, projectileSpawnPoint [i].rotation) as GameObject;
		}
	}
}
