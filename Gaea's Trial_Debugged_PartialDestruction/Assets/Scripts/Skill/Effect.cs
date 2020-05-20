using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {

	private float startTime, finishTime;
	public float playTime;
	private Animator anim;
	public bool finished = false;

	void Awake () {
		finishTime = Time.time + playTime;
		anim = GetComponent<Animator> ();
	}

	void Update () {
		anim.SetBool ("Finished", finished);
		if (Time.time >= finishTime) {
			finished = true;
			Destroy (this.gameObject.transform.parent.gameObject, 2f);
		}
	}
}
