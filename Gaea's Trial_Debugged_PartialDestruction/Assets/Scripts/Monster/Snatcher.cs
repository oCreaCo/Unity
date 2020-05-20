using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snatcher : MonoBehaviour {

	public GameObject snatcher;
	public GameObject frontSnatcher;
	public GameObject backSnatcher;
	public int snatchersCount;
	public Transform snatcherSpawnPoint;
	public GameObject snatchedJewel;

	// Use this for initialization
	void Start () {
		Spawn ();
		if (frontSnatcher == null) {
			GetComponent<Monster_Basic> ().statusIndicator.gameObject.SetActive (true);
			if (backSnatcher != null) {
				GetComponent<Monster_Basic> ().anim.SetTrigger ("FaceSmile");
			} else if (backSnatcher == null) {
				GetComponent<Monster_Basic> ().anim.SetTrigger ("FaceTears");
			}
		} else if (frontSnatcher != null) {
			GetComponent<Collider2D> ().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (backSnatcher != null) {
			backSnatcher.GetComponent<Monster_Basic> ().moveSpeedTemp = GetComponent<Monster_Basic> ().moveSpeedTemp;
		}
	}

	public void Spawn () {
		if (snatchersCount > 1) {
			snatchersCount--;
			GameObject snatcherTemp = Instantiate (snatcher, snatcherSpawnPoint.position, this.transform.rotation) as GameObject;
			backSnatcher = snatcherTemp;
			snatcherTemp.GetComponent<Snatcher> ().frontSnatcher = this.gameObject;
			snatcherTemp.GetComponent<Snatcher> ().snatchersCount = snatchersCount;
			if (snatchersCount == 1) {
				snatcherTemp.GetComponent<Monster_Basic> ().destroyTime = 1.5f;
				snatcherTemp.GetComponent<Animator> ().SetBool ("Last", true);
			}
		}
	}

	public void Die () {
		GetComponent<Monster_Basic> ().moveSpeedTemp = GetComponent<Monster_Basic> ().moveSpeed;
		if (backSnatcher != null) {
			backSnatcher.GetComponent<Monster_Basic> ().statusIndicator.gameObject.SetActive (true);
			if (backSnatcher.GetComponent<Snatcher>().backSnatcher != null) {
				backSnatcher.GetComponent<Monster_Basic> ().anim.SetTrigger ("FaceSmile");
			} else if (backSnatcher.GetComponent<Snatcher>().backSnatcher == null) {
				backSnatcher.GetComponent<Monster_Basic> ().anim.SetTrigger ("FaceTears");
			}
			backSnatcher.GetComponent<Collider2D> ().enabled = true;
		} else if (backSnatcher == null) {
			List<GameObject> ultJewels = new List<GameObject> ();
			List<GameObject> skillJewels = new List<GameObject> ();
			List<GameObject> norJewels = new List<GameObject> ();
			int x = 0, y = 0;
			for (x = 0; x < 6; x++) {
				for (y = 0; y < 6; y++) {
					if (Grid.grid.pieces [x, y].Type == Grid.PieceType.ULTIMATE) {
						ultJewels.Add (Grid.grid.pieces [x, y].gameObject);
					} else if (Grid.grid.pieces [x, y].Type == Grid.PieceType.HOR_CLEAR || Grid.grid.pieces [x, y].Type == Grid.PieceType.VER_CLEAR) {
						skillJewels.Add (Grid.grid.pieces [x, y].gameObject);
					} else {
						norJewels.Add (Grid.grid.pieces [x, y].gameObject);
					}
				}
			}
			if (ultJewels.Count != 0) {
				int i = Random.Range (0, ultJewels.Count);
				Instantiate (snatchedJewel, ultJewels [i].transform.position, ultJewels [i].transform.rotation, ultJewels [i].transform);
			} else {
				if (skillJewels.Count != 0) {
					int i = Random.Range (0, skillJewels.Count);
					Instantiate (snatchedJewel, skillJewels [i].transform.position, skillJewels [i].transform.rotation, skillJewels [i].transform);
				} else {
					int i = Random.Range (0, norJewels.Count);
					Instantiate (snatchedJewel, norJewels [i].transform.position, norJewels [i].transform.rotation, norJewels [i].transform);
				}
			}
		}
	}
}
