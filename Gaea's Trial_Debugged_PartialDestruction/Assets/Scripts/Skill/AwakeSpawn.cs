using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeSpawn : MonoBehaviour {

	public enum SpawnPosition
	{
		GRID,
		FRONTMONSTER,
	}

	public SpawnPosition spawnPosition;

	public bool local = false;

	public float positionXMin;
	public float positionXMax;
	public float positionYMin;
	public float positionYMax;
	public float positionZMin;
	public float positionZMax;
	public float rotationXMin;
	public float rotationXMax;
	public float rotationYMin;
	public float rotationYMax;
	public float rotationZMin;
	public float rotationZMax;

	void Start () {
		if (spawnPosition == SpawnPosition.GRID) {
			if (!local) {
				this.transform.position = new Vector3 (Random.Range (positionXMin, positionXMax), Random.Range (positionYMin, positionYMax), Random.Range (positionZMin, positionZMax));
				this.transform.rotation = Quaternion.Euler (Random.Range (rotationXMin, rotationXMax), Random.Range (rotationYMin, rotationYMax), Random.Range (rotationZMin, rotationZMax));
			} else {
				this.transform.localPosition = new Vector3 (Random.Range (positionXMin, positionXMax), Random.Range (positionYMin, positionYMax), Random.Range (positionZMin, positionZMax));
				this.transform.localRotation = Quaternion.Euler (Random.Range (rotationXMin, rotationXMax), Random.Range (rotationYMin, rotationYMax), Random.Range (rotationZMin, rotationZMax));
			}
		} else if (spawnPosition == SpawnPosition.FRONTMONSTER) {
			this.transform.position = Grid.grid.groundRayCast.GetComponent<ARayCast> ().frontMonster.position;
			this.transform.rotation = Quaternion.Euler (Random.Range (rotationXMin, rotationXMax), Random.Range (rotationYMin, rotationYMax), Random.Range (rotationZMin, rotationZMax));
		}
	}
}
