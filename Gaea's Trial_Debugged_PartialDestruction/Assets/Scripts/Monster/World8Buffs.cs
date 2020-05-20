using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World8Buffs : MonoBehaviour {

	public enum Monsters
	{
		LAVASHARK,
		LAVATH,
		FIREELEMENTAL,
		DEATHKNIGHT,
		THEMINION,
	}

	public Monsters monsName;
	public bool buffed = false;

	public float lavaSharkBuffedMoveSpeed;
	public int lavaSharkAddHealth;
	public int lavathAddDamage;
	public float fireElementalMoveSpeed;
	public int fireElementalAddDamage;
	public int fireElementalAddHealth;
	private bool fireElementalFirstBuffed;
	public int deathKnightAddDamage;
	public int deathKnightAddHealth;

	public GameObject buffParticle;
	public GameObject lavaSharkCrashParticle;

	void Awake () {
		if (monsName == Monsters.FIREELEMENTAL) {
			StartCoroutine (FireElementalCoroutine (fireElementalTime));
		}
	}

	IEnumerator FireElementalCoroutine (float _time) {
		yield return new WaitForSeconds (_time);
		if (!fireElementalFirstBuffed) {
			fireElementalFirstBuffed = true;
			fireElementalTier++;
			GetComponent<Monster_Basic> ().anim.SetTrigger ("Buffed1");
			GetComponent<Monster_Basic> ().moveSpeed = 0.2f;
			GetComponent<Monster_Basic> ().moveSpeedTemp = 0.2f;
			GetComponent<Monster_Basic> ().penetration = 2;
		}
	}

	[SerializeField] private int fireElementalTier = 0;
	public float fireElementalTime;
	public GameObject buffedDeathKnightAttack;

	public void Buff () {
		buffParticle.SetActive (true);
		switch (monsName) {
		case Monsters.LAVASHARK:
			GetComponent<Monster_Basic> ().moveSpeed = lavaSharkBuffedMoveSpeed;
			GetComponent<Monster_Basic> ().moveSpeedTemp = lavaSharkBuffedMoveSpeed;
			GetComponent<Monster_Basic> ().crash = true;
			GetComponent<Monster_Basic> ().curHealth += lavaSharkAddHealth;
			if (GetComponent<Monster_Basic> ().curHealth > GetComponent<Monster_Basic> ().oriHealth) {
				GetComponent<Monster_Basic> ().curHealth = GetComponent<Monster_Basic> ().oriHealth;
			}
			if (!GetComponent<Monster_Basic> ().collided) {
				lavaSharkCrashParticle.SetActive (true);
			}
			buffed = true;
			break;
		case Monsters.LAVATH:
			GetComponent<Monster_Basic> ().anim.SetTrigger ("Buffed");
			GetComponent<MonsterRangeAttack> ().damage += lavathAddDamage;
			buffed = true;
			break;
		case Monsters.FIREELEMENTAL:
			if (fireElementalTier == 0) {
				StopCoroutine (FireElementalCoroutine (fireElementalTime));
			}
			if (fireElementalTier < 3) {
				fireElementalTier++;
			}
			switch (fireElementalTier) {
			case 1:
				GetComponent<Monster_Basic> ().anim.SetTrigger ("Buffed1");
				GetComponent<Monster_Basic> ().moveSpeed = fireElementalMoveSpeed;
				GetComponent<Monster_Basic> ().moveSpeedTemp = fireElementalMoveSpeed;
				break;
			case 2:
				GetComponent<Monster_Basic> ().anim.SetTrigger ("Buffed2");
				GetComponent<MonsterMeleeAttack> ().damage += fireElementalAddDamage;
				GetComponent<Monster_Basic> ().curHealth += fireElementalAddHealth;
				if (GetComponent<Monster_Basic> ().curHealth > GetComponent<Monster_Basic> ().oriHealth) {
					GetComponent<Monster_Basic> ().curHealth = GetComponent<Monster_Basic> ().oriHealth;
				}
				break;
			case 3:
				GetComponent<Monster_Basic> ().anim.SetTrigger ("Buffed3");
				GetComponent<MonsterMeleeAttack> ().damage += fireElementalAddDamage;
				GetComponent<Monster_Basic> ().curHealth += fireElementalAddHealth;
				GetComponent<Monster_Basic> ().penetration = 3;
				buffed = true;
				if (GetComponent<Monster_Basic> ().curHealth > GetComponent<Monster_Basic> ().oriHealth) {
					GetComponent<Monster_Basic> ().curHealth = GetComponent<Monster_Basic> ().oriHealth;
				}
				break;
			}
			break;
		case Monsters.DEATHKNIGHT:
			GetComponent<Monster_Basic> ().anim.SetTrigger ("Buffed");
			GetComponent<Monster_Basic> ().reflectPercent = 40;
			GetComponent<MonsterRangeAttack> ().projectile = buffedDeathKnightAttack;
			GetComponent<MonsterRangeAttack> ().damage += deathKnightAddDamage;
			GetComponent<MonsterMeleeAttack> ().damage += deathKnightAddDamage;
			GetComponent<Monster_Basic> ().curHealth += deathKnightAddHealth;
			if (GetComponent<Monster_Basic> ().curHealth > GetComponent<Monster_Basic> ().oriHealth) {
				GetComponent<Monster_Basic> ().curHealth = GetComponent<Monster_Basic> ().oriHealth;
			}
			buffed = true;
			break;
		}
	}

	public void BuffedTrigger () {
		if (!buffed) {
			buffed = true;
		} else {
			buffed = false;
		}
	}
}
