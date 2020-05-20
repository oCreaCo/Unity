using UnityEngine;
using System.Collections;

public class MonsterDebuffBlock : MonoBehaviour {
	
	public float fireRateMin;
	public float fireRateMax;
	public float fireRateTemp;
	public float offSet;
	private float attackTime;
	[SerializeField] private int ranX, ranY;
	public int percent;

	private ColorPiece.ColorType colorTypeTemp;
	public GameObject debuffEffect;
	private int value;

	public enum DebuffCondition
	{
		TIME,
		DEATH,
	};

	public enum DebuffType
	{
		ICE,
		ZLIME,
	};

	public DebuffCondition debuffCondition;
	public DebuffType debuffType;

	private Animator anim;

	void Awake() {
		fireRateTemp = Random.Range(fireRateMin, fireRateMax);
		anim = GetComponent<Animator> ();
		attackTime = Time.time + offSet + fireRateTemp;
	}

	void Update () {
		if (Time.time >= attackTime && !GetComponent<Monster_Basic>().stoppedBool && debuffCondition == DebuffCondition.TIME) {
			anim.SetTrigger ("Debuff");
			fireRateTemp = Random.Range(fireRateMin, fireRateMax); 
			attackTime = Time.time + fireRateTemp;
		}
	}

	public void Debuff () {
		if (!GetComponent<Monster_Basic> ().stoppedBool || debuffCondition == DebuffCondition.DEATH) {
			int i = Random.Range (1, 101);
			if (i <= percent) {
				ranX = Random.Range (0, 6);
				ranY = Random.Range (0, 6);
				if (Grid.grid.pieces [ranX, ranY].GetComponent<GamePiece> ().pieceDebuffType != GamePiece.PieceDebuffType.ICE && Grid.grid.pieces [ranX, ranY].GetComponent<GamePiece> ().pieceDebuffType != GamePiece.PieceDebuffType.ZLIME) {
					if (debuffType == DebuffType.ICE) {
						Grid.grid.pieces [ranX, ranY].GetComponent<GamePiece> ().pieceDebuffType = GamePiece.PieceDebuffType.ICE;
						Grid.grid.pieces [ranX, ranY].GetComponent<MovablePiece> ().enabled = false;
						Grid.grid.pieces [ranX, ranY].GetComponent<ClearablePiece> ().enabled = false;
					} else if (debuffType == DebuffType.ZLIME) {
						Grid.grid.pieces [ranX, ranY].GetComponent<GamePiece> ().pieceDebuffType = GamePiece.PieceDebuffType.ZLIME;
					}
					Instantiate (debuffEffect, Grid.grid.pieces [ranX, ranY].transform.position, Grid.grid.pieces [ranX, ranY].transform.rotation, Grid.grid.pieces [ranX, ranY].transform);
				} else {
					Debuff ();
				}
			}
		}
	}
}
