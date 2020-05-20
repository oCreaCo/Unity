using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour {

	public GameObject chainedJewel;
	public GameObject chainedSprite;
	public Transform chainSpriteSpawnPosition;
	public Transform magicPoint;
	public Necro necro;
	public Vector3 vector3 = new Vector3 (0, 0, 0);
	public float x;
	private bool spawning = false;

	void Update () {
		if (this.transform.position.y <= chainedJewel.transform.position.y) {
			GetComponent<Monster_Basic> ().moveSpeed = 0;
			GetComponent<Monster_Basic> ().moveSpeedTemp = 0;
		} else if (!spawning && !necro.partialDestruction.activated) {
			StartCoroutine (SpawnSpriteCoroutine ());
		}
		if (GetComponent<Monster_Basic> ().dead) {
			necro.chains.Remove (this.gameObject);
			chainedJewel.GetComponent<GamePiece> ().pieceDebuffType = GamePiece.PieceDebuffType.NONE;
			chainedJewel.GetComponent<ClearablePiece> ().enabled = true;
		}
	}

	IEnumerator SpawnSpriteCoroutine () {
		spawning = true;
		x += 0.268f;
		vector3.x = x;
		chainSpriteSpawnPosition.localPosition = vector3;
		magicPoint.localPosition = vector3;
		magicPoint.rotation = Quaternion.Euler (0, 0, 0);
		yield return new WaitForSeconds (0.015f);
		GameObject chain = Instantiate (chainedSprite, chainSpriteSpawnPosition.position, this.transform.rotation, this.transform) as GameObject;
		spawning = false;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "End") Destroy (this.gameObject);
	}
}
