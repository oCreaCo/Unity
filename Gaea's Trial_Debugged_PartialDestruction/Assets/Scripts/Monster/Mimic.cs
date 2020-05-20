using UnityEngine;
using System.Collections;

public class Mimic : MonoBehaviour {

	[SerializeField] private float moveSpeed;
	[SerializeField] private float moveTime;

	[SerializeField] private GameMaster gamemaster;

	[SerializeField] private Animator anim;

	[SerializeField] private bool attack = false;

	void Awake () {
		anim = GetComponent<Animator> ();
		gamemaster = GameMaster.gameMaster;
		Move ();
	}

	public void MimicAttackTrigger () {
		anim.SetTrigger ("MimicAttack");
	}

	public void Move () {
		StartCoroutine (MoveCoroutine ());
	}

	IEnumerator MoveCoroutine () {
		GetComponent<Monster_Basic> ().moveSpeedTemp = moveSpeed;
		yield return new WaitForSeconds (moveTime);
		GetComponent<Monster_Basic> ().moveSpeedTemp = 0f;
	}
}
