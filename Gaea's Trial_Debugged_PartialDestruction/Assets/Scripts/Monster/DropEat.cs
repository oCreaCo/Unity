using UnityEngine;
using System.Collections;

public class DropEat : MonoBehaviour {
	
	public GameObject droppedChunk;
	public GameObject spawnObject;

	[SerializeField] private int spawnCount;
	[SerializeField] private int count;
	[SerializeField] private bool eat;

	public void DropChunk () {
		Instantiate (droppedChunk, this.transform.position, this.transform.rotation);
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (eat) {
			if (collision.collider.tag == "DroppedChunk") {
				count += 1;
				Destroy (collision.collider.gameObject);
				if (count == spawnCount) {
					Instantiate (spawnObject, this.transform.position, this.transform.rotation);
					GameMaster.gameMaster.remainingEnemies.Remove (this.gameObject);
					GameMaster.gameMaster.StartCheckFinished ( 0.5f);
					Destroy (this.gameObject);
				}
			}
		}
	}
}
