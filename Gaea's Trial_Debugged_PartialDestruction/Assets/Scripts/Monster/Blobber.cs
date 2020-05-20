using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blobber : MonoBehaviour {

	public float fireRateMin;
	public float fireRateMax;
	public float fireRateTemp;
	public float offSet;
	private float attackTime;

	public GameObject blob;
	public int blobCount;
	public Transform blobPoint;
	public float x, y;
	Vector2 vector2 = new Vector2 ();

	private Animator anim;

	void Awake() {
		fireRateTemp = Random.Range(fireRateMin, fireRateMax);
		anim = GetComponent<Animator> ();
		attackTime = Time.time + offSet + fireRateTemp;
	}

	void Update () {
		if (GetComponent<Monster_Basic> ().damageMonsterType == Monster_Basic.DamageMonsterType.BASIC) {
			if (Time.time >= attackTime && !GetComponent<Monster_Basic> ().stoppedBool) {
				anim.SetTrigger ("Attack");
				fireRateTemp = Random.Range (fireRateMin, fireRateMax); 
				attackTime = Time.time + fireRateTemp;
			}
		}
	}

	public void BlobBlob () {
		for (int i = 0; i < blobCount; i++) {
			x = Random.Range (-1.5f, -3.1f);
			y = Random.Range (2f, 5.1f);
			vector2.x = x;
			vector2.y = y;
			GameObject b = Instantiate (blob, blobPoint.position, blobPoint.rotation);
			b.GetComponent<Rigidbody2D> ().AddForce (vector2, ForceMode2D.Impulse);
		}
	}
}
