using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snatched : MonoBehaviour {

	public float time;

	// Use this for initialization
	void Start () {
		this.transform.parent.GetComponent<MovablePiece> ().enabled = false;
		StartCoroutine (Snatch ());
	}

	IEnumerator Snatch () {
		yield return new WaitForSeconds (time);
		Grid.grid.ClearPiece (this.transform.parent.GetComponent<GamePiece> ().X, this.transform.parent.GetComponent<GamePiece> ().Y);
		Grid.grid.fillTime2 = Grid.grid.fillTime;
		Grid.grid.StartCoroutine (Grid.grid.Fill ());
	}
}
