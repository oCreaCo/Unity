using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World8Buffer : MonoBehaviour {

	public GameObject buffObject;

	public float buffDelay;
	public int buffFixCountMin;
	public int buffFixCountMax;
	private int buffFixCountTemp;
	public int buffRandCountMin;
	public int buffRandCountMax;
	private int buffRandCountTemp;
	private Vector2 vector2 = new Vector2 ();
	[SerializeField] private ParticleSystem[] particle;

	void Awake () {
		this.transform.position = Grid.grid.skyObject.FindChild ("World8Sky(Clone)").FindChild ("Volcano").FindChild ("BufferSpawnPoint").position;
	}

	public void Buff () {
		buffFixCountTemp = Random.Range (buffFixCountMin, buffFixCountMax + 1);
		buffRandCountTemp = Random.Range (buffRandCountMin, buffRandCountMax + 1);
		StartCoroutine(BuffCoroutine(buffDelay));
	}

	IEnumerator BuffCoroutine (float _missileDelay) {
		for (int i = 0; i < buffFixCountTemp; i++) {
			if (GameMaster.gameMaster.remainingEnemies.Count != 0) {
				GameObject temp = GameMaster.gameMaster.remainingEnemies [Random.Range (0, GameMaster.gameMaster.remainingEnemies.Count)];
				vector2.x = temp.transform.position.x;
				vector2.y = 6f;
				GameObject buffOb = Instantiate (buffObject, vector2, this.transform.rotation) as GameObject;
				yield return new WaitForSeconds (_missileDelay);
			}
		}
		for (int i = 0; i < buffRandCountTemp; i++) {
			vector2.x = Random.Range (-2f, 3f);
			vector2.y = 6f;
			GameObject buffOb = Instantiate (buffObject, vector2, this.transform.rotation) as GameObject;
			yield return new WaitForSeconds (_missileDelay);
		}
		yield return new WaitForSeconds (1f);
		GameMaster.gameMaster.StartCheckFinished (0.5f);
		Destroy (this.gameObject);
	}

	public void ParticleStop () {
		for (int i = 0; i < particle.Length; i++) {
			particle [i].Stop ();
		}
	}
}
