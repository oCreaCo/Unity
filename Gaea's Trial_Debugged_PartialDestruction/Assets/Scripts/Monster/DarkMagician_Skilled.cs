using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMagician_Skilled : MonoBehaviour {

	public enum SkilledType
	{
		RESURRECTION,
		STRENGTHEN,
	}

	public SkilledType skilledType;

	public float resurrectFirerate;
	public float strengthenFirerate;
	public GameObject strengthenObject;
	public DarkMagician darkMagician;

	[SerializeField] private Animator anim;

	void Awake () {
		anim = GetComponent<Animator> ();
	}

	public void Trigger () {
		switch (skilledType) {
		case SkilledType.RESURRECTION:
			GetComponent<Monster_Basic> ().dead = false;
			anim.SetBool ("Dead", GetComponent<Monster_Basic> ().dead);
			darkMagician.fireRateTemp = resurrectFirerate;
			break;
		case SkilledType.STRENGTHEN:
			GameObject dandelionPotted = Instantiate (strengthenObject, this.transform.position, this.transform.rotation);
			dandelionPotted.GetComponent<Monster_Basic> ().curHealth = GetComponent<Monster_Basic> ().curHealth + 30;
			dandelionPotted.GetComponent<Monster_Basic> ().statusIndicator.SetHealth (dandelionPotted.GetComponent<Monster_Basic> ().curHealth, dandelionPotted.GetComponent<Monster_Basic> ().oriHealth);
			darkMagician.fireRateTemp = strengthenFirerate;
			darkMagician.attackTime = Time.time + darkMagician.fireRateTemp;
			GameMaster.gameMaster.remainingEnemies.Remove (this.gameObject);
			Destroy (this.gameObject);
			break;
		}
	}

	public void ResurrectionSkill () {
		this.tag = "Monster";
		GetComponent<Monster_Basic> ().moveSpeedTemp = GetComponent<Monster_Basic> ().moveSpeed;
		GetComponent<Monster_Basic> ().curHealth = GetComponent<Monster_Basic> ().oriHealth;
		GetComponent<Monster_Basic> ().statusIndicator.SetHealth (GetComponent<Monster_Basic> ().curHealth, GetComponent<Monster_Basic> ().oriHealth);
		GetComponent<Monster_Basic> ().hurtable = true;
		GameMaster.gameMaster.oriScore += 3 * GetComponent<Monster_Basic> ().score;
		GameMaster.gameMaster.remainingEnemies.Add (this.gameObject);
		darkMagician.attackTime = Time.time + darkMagician.fireRateTemp;
	}
}
