using UnityEngine;
using System.Collections;

public class Sight : MonoBehaviour {

	private bool facingRight = true;
	private bool seeingRight = true;

	public Transform target;

	private bool searchingForPlayer = false;

	IEnumerator SearchingForPlayer () {
		GameObject sResult = GameObject.FindGameObjectWithTag ("Player");
		if (sResult == null) {
			yield return new WaitForSeconds (0.5f);
			StartCoroutine (SearchingForPlayer ());
		} 
		else {
			target = sResult.transform;
			searchingForPlayer = false;
			yield return false;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ((target.position.x - transform.position.x) > 0 && seeingRight == false) {
			seeingRight = true;
			Flip ();
		}

		if ((target.position.x - transform.position.x) < 0 && seeingRight == true) {
			seeingRight = false;
			Flip ();
		}
	}

	void FixedUpdate () {
		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchingForPlayer());
			}
			return;
		}
	}

	void Flip () {
		// Switch the way the player is labelled as facing
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
