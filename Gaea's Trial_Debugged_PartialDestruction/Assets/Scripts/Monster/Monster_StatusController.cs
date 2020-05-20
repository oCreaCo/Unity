using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_StatusController : MonoBehaviour {
	
	public GameObject stunnedAnim;
	public Monster_Basic monsterBasic;

	[SerializeField] private float stunnedTime;
	public bool goldSkilled;

	public GameObject fireParticle;
	public GameObject iceParticle;
	public GameObject electricParticle;

	private Animator anim;
//	public Button goldSkillButton;

	public void Fired (int fireDamage, int count) {
		StartCoroutine (FiredCoroutine (fireDamage, count));
	}

	IEnumerator FiredCoroutine (int fireDamage, int count) {
		fireParticle.SetActive (true);
			for (int i = 0; i < count; i++) {
			yield return new WaitForSeconds (1f);
				if (monsterBasic.monsterType != Monster_Basic.MonsterType.FIRE) {
					GetComponent<Monster_Basic> ().DamageMonster (fireDamage);
				}
			}
			yield return new WaitForSeconds (1f);
			fireParticle.SetActive (false);
	}

	public void Iced (float icedTime) {
			StartCoroutine (IcedCoroutine (icedTime));
	}

	IEnumerator IcedCoroutine (float icedTime) {
		iceParticle.SetActive (true);
		if (monsterBasic.monsterType != Monster_Basic.MonsterType.ICE) {
			GetComponent<Monster_Basic> ().moveSpeedTemp = 0;
			GetComponent<Monster_Basic> ().stoppedBool = true;
			StartCoroutine (Stunned (icedTime));
			yield return new WaitForSeconds (icedTime);
			if (!GetComponent<Monster_Basic> ().collided || !GetComponent<Monster_Basic> ().stoppedBool || !GetComponent<Monster_Basic> ().dead) {
				GetComponent<Monster_Basic> ().moveSpeedTemp = GetComponent<Monster_Basic> ().moveSpeed;
			}
		} else {
			yield return new WaitForSeconds (icedTime);
		}
		iceParticle.SetActive (false);
	}

	public void ElectricShocked (float shockedTime) {
		StartCoroutine (ElectricShockedCoroutine (shockedTime));
	}

	IEnumerator ElectricShockedCoroutine (float shockedTime) {
		GetComponent<Monster_Basic> ().moveSpeedTemp = GetComponent<Monster_Basic> ().moveSpeed / 2;
		electricParticle.SetActive (true);
		if (GetComponent<MonsterMeleeAttack> () != null) {
			GetComponent<MonsterMeleeAttack> ().fireRateTemp /= 2;
		}
		if (GetComponent<MonsterRangeAttack> () != null) {
			GetComponent<MonsterRangeAttack> ().fireRateTemp /= 2;
		}
		yield return new WaitForSeconds (shockedTime);
		if (!GetComponent<Monster_Basic> ().collided || !GetComponent<Monster_Basic> ().stoppedBool || !GetComponent<Monster_Basic> ().dead) {
			GetComponent<Monster_Basic> ().moveSpeedTemp = GetComponent<Monster_Basic> ().moveSpeed;
		}
		if (GetComponent<MonsterMeleeAttack> () != null) {
			GetComponent<MonsterMeleeAttack> ().fireRateTemp =GetComponent<MonsterMeleeAttack> ().fireRate;
		}
		if (GetComponent<MonsterRangeAttack> () != null) {
			GetComponent<MonsterRangeAttack> ().fireRateTemp = Random.Range (GetComponent<MonsterRangeAttack> ().fireRateMin, GetComponent<MonsterRangeAttack> ().fireRateMax);
		}
		electricParticle.SetActive (false);
	}

	public void Blood (int bloodDamage, int count)
	{
		StartCoroutine (BloodCoroutine (bloodDamage, count));
	}

	IEnumerator BloodCoroutine (int bloodDamage, int count) {
		for (int i = 0; i < count; i++) {
			yield return new WaitForSeconds (1f);
			GetComponent<Monster_Basic> ().DamageMonster (bloodDamage);
		}
	}

	private bool bool1 = false, bool2 = false;

	public void SpeedBuff (float buffTime, float buffSpeed, bool buffBool, int type) {
		StartCoroutine (SpeedBuffCoroutine (buffTime, buffSpeed, buffBool, type));
	}

	IEnumerator SpeedBuffCoroutine (float buffTime, float buffSpeed, bool buffBool, int type) {
		if (GetComponent<Monster_Basic> ().moveSpeed != 0) {
			if (type == 1) {
				if (buffBool && !bool1) {
					GetComponent<Monster_Basic> ().moveSpeedTemp += buffSpeed;
					bool1 = true;
				} else if (!buffBool && bool1) {
					bool1 = false;
					yield return new WaitForSeconds (buffTime);
					if (!GetComponent<Monster_Basic> ().stoppedBool) {
						GetComponent<Monster_Basic> ().moveSpeedTemp -= buffSpeed;
						if (GetComponent<Monster_Basic> ().moveSpeedTemp < GetComponent<Monster_Basic> ().moveSpeed) {
							GetComponent<Monster_Basic> ().moveSpeedTemp = GetComponent<Monster_Basic> ().moveSpeed;
						}
					}
				}
			} else if (type == 2) {
				if (buffBool && !bool2) {
					GetComponent<Monster_Basic> ().moveSpeedTemp += buffSpeed;
					bool2 = true;
				} else if (!buffBool && bool2) {
					bool2 = false;
					yield return new WaitForSeconds (buffTime);
					if (!GetComponent<Monster_Basic> ().stoppedBool) {
						GetComponent<Monster_Basic> ().moveSpeedTemp -= buffSpeed;
						if (GetComponent<Monster_Basic> ().moveSpeedTemp < GetComponent<Monster_Basic> ().moveSpeed) {
							GetComponent<Monster_Basic> ().moveSpeedTemp = GetComponent<Monster_Basic> ().moveSpeed;
						}
					}
				}
			}
		}
	}

	IEnumerator Stunned (float stunned) {
		anim.SetLayerWeight (1, 1);
		yield return new WaitForSeconds (stunned);
		anim.SetLayerWeight (1, 0);
		GetComponent<Monster_Basic> ().stoppedBool = false;
		anim.SetBool ("Stopped", GetComponent<Monster_Basic> ().stoppedBool);
		if (!GetComponent<Monster_Basic> ().collided) {
			GetComponent<Monster_Basic> ().moveSpeedTemp = GetComponent<Monster_Basic> ().moveSpeed;
		}
	}

	void Awake() {
		stunnedTime += PlayerPrefs.GetInt ("GridEquippedRelicStunPlusTime") / 100;
		anim = GetComponent<Animator> ();
		monsterBasic = GetComponent<Monster_Basic> ();
	}
}
