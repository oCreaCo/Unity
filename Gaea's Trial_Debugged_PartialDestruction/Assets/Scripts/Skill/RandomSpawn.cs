using UnityEngine;
using System.Collections;

public class RandomSpawn : MonoBehaviour {

	public int count;
	private int countTemp = 0;
	public float fireRate;
	private float spawnTime;
	public string sound;
	public GameObject spawn;

	void Awake () {
		spawnTime = Time.time + 0.2f;
	}

	void Update () {
		if (Time.time >= spawnTime && countTemp < count) {
			spawnTime = Time.time + fireRate;
			GetComponent<AudioController> ().PlaySound (sound);
			GameObject clone =  Instantiate (spawn, this.transform.position, this.transform.rotation) as GameObject;
			if (GetComponent<DamagePurpleInput> () != null) {
				clone.transform.parent = this.transform;
				clone.GetComponent<DamagePurpleInput> ().damage = this.gameObject.GetComponent<DamagePurpleInput> ().damage;
			}
			countTemp += 1;
		}
	}
}
