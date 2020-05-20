using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puffer : MonoBehaviour {

	[SerializeField] private int ranX, ranY;
	public GameObject debuffEffect;

	public void PufferDebuff () {
		ranX = Random.Range (0, 6);
		ranY = Random.Range (0, 6);
		if (Grid.grid.pieces [ranX, ranY].GetComponent<GamePiece> ().pieceDebuffType != GamePiece.PieceDebuffType.PUFFER) {
			Grid.grid.pieces [ranX, ranY].GetComponent<GamePiece> ().pieceDebuffType = GamePiece.PieceDebuffType.PUFFER;
			Instantiate (debuffEffect, Grid.grid.pieces [ranX, ranY].transform.position, Grid.grid.pieces [ranX, ranY].transform.rotation, Grid.grid.pieces [ranX, ranY].transform);
		} else {
			PufferDebuff ();
		}
	}
}
