using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necro : MonoBehaviour {

	public PartialDestruction partialDestruction;

	public float dandelionPottedSpawnRateMin;
	public float dandelionPottedSpawnRateMax;
	public float dandelionPottedSpawnRateTemp;
	public float dandelionPottedSpawnOffSet;
	private float dandelionPottedSpawnAttackTime;
	public GameObject dandelionPotted;
	public List<GameObject> dandelionPotteds;
	private bool spawning = true;

	public float fireRateMin;
	public float fireRateMax;
	public float fireRateTemp;
	public float offSet;
	private float attackTime;
	public GameObject chain;
	public GameObject chainedJewel;
	public Transform chainSpawnPoint;
	public List<GameObject> chains;
	public bool activated = false;


	[SerializeField] private int ranX, ranY;

	private bool dead = false;

	private Animator anim;

	void Awake() {
		anim = GetComponent<Animator> ();
		partialDestruction = GetComponent<PartialDestruction> ();
		dandelionPottedSpawnRateTemp = Random.Range(dandelionPottedSpawnRateMin, dandelionPottedSpawnRateMax);
		dandelionPottedSpawnAttackTime = Time.time + dandelionPottedSpawnOffSet + dandelionPottedSpawnRateTemp;
	}

	void Update () {
		if (GetComponent<Monster_Basic> ().damageMonsterType == Monster_Basic.DamageMonsterType.BASIC && GetComponent<Monster_Basic> ().hurtable) {
			if (Time.time >= dandelionPottedSpawnAttackTime && !GetComponent<Monster_Basic> ().stoppedBool && spawning) {
				GameObject dan =  Instantiate(dandelionPotted) as GameObject;
				dandelionPotteds.Add (dan);
				dandelionPottedSpawnRateTemp = Random.Range (dandelionPottedSpawnRateMin, dandelionPottedSpawnRateMax); 
				dandelionPottedSpawnAttackTime = Time.time + dandelionPottedSpawnRateTemp;
			}
			if (Time.time >= attackTime && !GetComponent<Monster_Basic> ().stoppedBool) {
				anim.SetTrigger ("Attack");
				fireRateTemp = Random.Range (fireRateMin, fireRateMax); 
				attackTime = Time.time + fireRateTemp;
			}
		}
		if (GetComponent<Monster_Basic> ().dead && !dead) {
			spawning = false;
			dead = true;
			for (int i = 0; i < dandelionPotteds.Count; i++) {
				GameMaster.gameMaster.remainingEnemies.Remove (dandelionPotteds [i]);
				Destroy (dandelionPotteds [i]);
			}
			for (int i = 0; i < chains.Count; i++) {
				GameMaster.gameMaster.remainingEnemies.Remove (chains [i]);
				Destroy (chains [i]);
			}
		}
	}

	public void DandelionPotted () {
		spawning = false;
		StartCoroutine (DandelionPottedCoroutine ());
	}

	IEnumerator DandelionPottedCoroutine () {
		for (int i = 0; i < dandelionPotteds.Count; i++) {
			dandelionPotteds [i].GetComponent<Animator> ().SetTrigger ("Awake");
		}
		yield return new WaitForSeconds (1f);
		for (int i = 0; i < dandelionPotteds.Count; i++) {
			dandelionPotteds [i].GetComponent<Monster_Basic> ().moveSpeed = 0.3f;
			dandelionPotteds [i].GetComponent<Monster_Basic> ().moveSpeedTemp = 0.3f;
		}
		dandelionPotteds.Clear ();
		spawning = true;
	}

	public void FiveChains () {
		for (int i = chains.Count; i < 5; i++) {
			Chain ();
		}
		partialDestruction.ActivateButton (ref activated, 5f);
	}

	private void Chain () {
		activated = false;
		ranX = Random.Range (0, 6);
		ranY = Random.Range (0, 6);
		if (Grid.grid.pieces [ranX, ranY].GetComponent<GamePiece> ().pieceDebuffType != GamePiece.PieceDebuffType.CHAINED) {
			Grid.grid.pieces [ranX, ranY].GetComponent<GamePiece> ().pieceDebuffType = GamePiece.PieceDebuffType.CHAINED;
			Grid.grid.pieces [ranX, ranY].GetComponent<MovablePiece> ().enabled = false;
			Grid.grid.pieces [ranX, ranY].GetComponent<ClearablePiece> ().enabled = false;
			GameObject chainObject = Instantiate (chain, chainSpawnPoint.position, Quaternion.Euler (0, 0, Mathf.Atan2 (chainSpawnPoint.position.y - Grid.grid.pieces [ranX, ranY].transform.position.y, chainSpawnPoint.position.x - Grid.grid.pieces [ranX, ranY].transform.position.x) * Mathf.Rad2Deg)) as GameObject;
			chainObject.GetComponent<Chain> ().chainedJewel = Grid.grid.pieces [ranX, ranY].gameObject;
			chains.Add (chainObject);
			chainObject.GetComponent<Chain> ().necro = this;
			Instantiate (chainedJewel, Grid.grid.pieces [ranX, ranY].transform.position, Grid.grid.pieces [ranX, ranY].transform.rotation, Grid.grid.pieces [ranX, ranY].transform);
		} else {
			Chain ();
		}
	}

	public void PartialDestructionSuccessFunction() {
		for (int i = 4; i >= 0; i--) {
			chains [i].GetComponent<Monster_Basic> ().DamageMonster (50000);
		}
	}
}
