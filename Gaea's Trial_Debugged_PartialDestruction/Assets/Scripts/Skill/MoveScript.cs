using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

	[SerializeField] private float chargeTime;
	public bool charged = false;

	public float moveSpeed;
	public float moveSpeedTemp;
	public bool move = false;
	public Animator anim;

	public enum Direction
	{
		RIGHT,
		LEFT,
		UP,
		DOWN,
	};

	public Direction direction;

	void Awake () {
		moveSpeedTemp = moveSpeed;
		StartCoroutine (Charge ());
	}

	void Update () {
		if (charged) {
			if (direction == Direction.RIGHT) {
				transform.Translate (Vector3.right * Time.deltaTime * moveSpeedTemp);
			} else if (direction == Direction.LEFT) {
				transform.Translate (Vector3.left * Time.deltaTime * moveSpeedTemp);
			} else if (direction == Direction.UP) {
				transform.Translate (Vector3.up * Time.deltaTime * moveSpeedTemp);
			} else if (direction == Direction.DOWN) {
				transform.Translate (Vector3.down * Time.deltaTime * moveSpeedTemp);
			}
		}
	}

	IEnumerator Charge () {
		yield return new WaitForSeconds (chargeTime);
		charged = true;
		move = true;
		if (anim != null) {
			anim.SetBool ("Move", move);
		}
	}
}
