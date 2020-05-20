using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmazonessTree : MonoBehaviour {

	public float sporeFireRateMin;
	public float sporeFireRateMax;
	public float sporeFireRateTemp;
	public float sporeOffSet;
	private float sporeAttackTime;
	public int sporeCount;
	public GameObject spore;

	public float loggerFireRateMin;
	public float loggerFireRateMax;
	public float loggerFireRateTemp;
	public float loggerOffSet;
	private float loggerAttackTime;
	public GameObject logger;
	public Transform loggerSpawnPoint;
	private bool spawnLogger = false;
	public GameObject root;

	private Animator anim;

	void Awake() {
		sporeFireRateTemp = Random.Range(sporeFireRateMin, sporeFireRateMax);
		loggerFireRateTemp = Random.Range(loggerFireRateMin, loggerFireRateMax);
		anim = GetComponent<Animator> ();
		sporeAttackTime = Time.time + sporeOffSet + sporeFireRateTemp;
		loggerAttackTime = Time.time + loggerOffSet + loggerFireRateTemp;
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<Monster_Basic> ().damageMonsterType == Monster_Basic.DamageMonsterType.BASIC) {
			if (Time.time >= sporeAttackTime && !GetComponent<Monster_Basic> ().stoppedBool) {
				anim.SetTrigger ("Spore");
				sporeFireRateTemp = Random.Range (sporeFireRateMin, sporeFireRateMax); 
				sporeAttackTime = Time.time + sporeFireRateTemp;
			}
			if (Time.time >= loggerAttackTime && !GetComponent<Monster_Basic> ().stoppedBool && spawnLogger) {
				anim.SetTrigger ("Logger");
				loggerFireRateTemp = Random.Range (loggerFireRateMin, loggerFireRateMax); 
				loggerAttackTime = Time.time + loggerFireRateTemp;
			}
			if (GetComponent<Monster_Basic> ().curHealth < GetComponent<Monster_Basic> ().oriHealth / 2 && !spawnLogger) {
				anim.SetTrigger ("Root");
			}
		}
	}

	public void SpawnSpore () {
		StartCoroutine (SpawnSporeCoroutine ());
	}

	IEnumerator SpawnSporeCoroutine () {
		for (int i = 0; i < sporeCount; i++) {
			Instantiate (spore);
			yield return new WaitForSeconds (0.5f);
		}
	}

	public void SpawnLogger () {
		Instantiate (logger, loggerSpawnPoint.position, loggerSpawnPoint.rotation);
	}

	public void StartSpawnLogger () {
		spawnLogger = true;
//		Destroy (root);
	}
}
