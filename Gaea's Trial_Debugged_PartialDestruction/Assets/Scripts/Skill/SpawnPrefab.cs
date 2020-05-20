using UnityEngine;
using System.Collections;

public class SpawnPrefab : MonoBehaviour {

	public Transform spawnPoint1;
	public Transform spawnPoint2;
	public Transform spawnPoint3;

	public GameObject prefab;

	public void Spawn () {
		Instantiate (prefab, spawnPoint1.position, spawnPoint1.rotation);
		Instantiate (prefab, spawnPoint2.position, spawnPoint2.rotation);
		Instantiate (prefab, spawnPoint3.position, spawnPoint3.rotation);
	}
}
