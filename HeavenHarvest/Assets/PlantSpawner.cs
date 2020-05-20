using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpawner : MonoBehaviour {

	public static PlantSpawner plantSpawner;

	public bool started;
	[SerializeField] private int maxCrops;
	public int crops;
	[SerializeField] private float xMin;
	[SerializeField] private float xMax;
	private float widMin, widMax;
	[SerializeField] private float gridWidth;
	[SerializeField] private int s, e;
	public int stems;

	public GameObject[] crop;
	public float spawnRate;
	public float spawnTime;

	public bool[] gridBools;

	void Awake () {
		PlantSpawner.plantSpawner = this;
	}

	void Update () {
		if (started && crops < maxCrops && Time.time >= spawnTime) {
			spawnTime = Time.time + spawnRate;
			Spawn ();
		}
	}

	void Spawn () {
		s = 0;
		e = 0;
		bool tmp = false;
		int r;
		if (stems < 2) {
			r = Random.Range (0, crop.Length);
		} else {
			r = Random.Range (0, 3);
		}
		int x = Random.Range ((crop [r].GetComponent<Plant> ().width / 2), ((gridBools.Length - (crop [r].GetComponent<Plant> ().width / 2))));
		tmp = true;
		s = x - (crop [r].GetComponent<Plant> ().width / 2);
		e = x + (crop [r].GetComponent<Plant> ().width / 2);
		for (int i = s; i <= e; i++) {
			if (gridBools [i]) {
				break;
			} else if (i == e) {
				tmp = false;
			}
		}
		if (tmp) {
			Spawn ();
		} else {
			for (int j = s; j <= e; j++) {
				gridBools [j] = true;
			}
			crops++;
			GameObject plantTemp = Instantiate (crop [r], new Vector3 (xMin + (x * gridWidth), -3.435f, 0), Quaternion.Euler (0, 0, 0)) as GameObject;
			plantTemp.GetComponent<Plant> ().s = s;
			plantTemp.GetComponent<Plant> ().e = e;
		}
	}
}
