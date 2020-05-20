using UnityEngine;
using System.Collections;

public class CollideStop : MonoBehaviour {

	[SerializeField] private MoveScript moveScript;
	[SerializeField] private  Barricade barricade;

	void Awake () {
		moveScript = this.transform.parent.GetComponent<MoveScript> ();
		barricade  = this.transform.parent.GetComponent<Barricade> ();
	}

	void Update () {
	
	}

	public void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Monster") {
			moveScript.moveSpeedTemp = 0f;
			moveScript.move = false;
			moveScript.anim.SetBool ("Move", moveScript.move);
		}
	}

	public void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Monster") {
			moveScript.moveSpeedTemp = moveScript.moveSpeed;
			if (barricade != null && barricade.curHealth != 0) {
				moveScript.move = true;
				moveScript.anim.SetBool ("Move", moveScript.move);
			}
			GetComponent <CollideMeleeAttack> ().triggeredMonster.Clear ();
		}
	}
}
