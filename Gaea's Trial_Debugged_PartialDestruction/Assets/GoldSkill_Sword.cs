using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldSkill_Sword : MonoBehaviour {

	public GameObject target;
	[SerializeField] private int multiplier;
	[SerializeField] private float shakeAmount;
	[SerializeField] private float shakeLength;

	// Use this for initialization
	void Start () {
		target = Grid.grid.groundRayCast.GetComponent<ARayCast> ().frontMonster.gameObject;
		Vector3 vector3 = new Vector3 (target.transform.position.x, 0, 0);
		this.transform.parent.position = vector3;
		target.GetComponent<Monster_Basic> ().moveSpeedTemp = 0;
		target.GetComponent<Animator> ().enabled = false;
	}

//	public void CameraShake () {
//
//	}

	public void Damage () {
		CameraShake.cameraShake.Shake (shakeAmount, shakeLength);
		if (!target.GetComponent<Monster_Basic> ().collided) {
			target.GetComponent<Monster_Basic> ().moveSpeedTemp = target.GetComponent<Monster_Basic> ().moveSpeed;
		}
		target.GetComponent<Animator> ().enabled = true;
		target.GetComponent<Monster_Basic> ().DamageMonster (PlayerPrefs.GetInt ("HeroLevelMaxDamage") * multiplier);
		target = null;
	}
}
