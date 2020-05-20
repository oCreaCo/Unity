using UnityEngine;
using System.Collections;

public class PuppetAnim : MonoBehaviour {
	private Animator anim;

	void Awake () {
		anim = this.transform.GetComponent<Animator> ();
	}

	void Update () {
		
	}
}
