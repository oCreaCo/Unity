using UnityEngine;
using System.Collections;

public class AwakeDestroyBoxCollider : MonoBehaviour {

	public float destroyTime;
	[SerializeField] private float awakeTime;
	public bool dead = false;
	private Vector3 hell = new Vector3 (100f, 200f, 300f);

	void Awake () {
		awakeTime = Time.time;
		Destroy (this.gameObject, destroyTime);
	}

	void Update () {
		if (this.transform.FindChild ("Graphic").GetComponent<BoxCollider> () != null) {
			if (Time.time >= awakeTime + destroyTime - 1f) {
				dead = true;
				this.transform.FindChild ("Graphic").GetComponent<Animator> ().SetBool ("Dead", dead);
				//this.transform.FindChild ("Graphic").GetComponent<BoxCollider> ().center = hell;
			}
		}
	}
}
