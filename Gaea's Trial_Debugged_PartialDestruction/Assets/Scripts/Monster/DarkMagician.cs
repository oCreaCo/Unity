using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMagician : MonoBehaviour {

	[SerializeField] GameMaster gameMaster;
	[SerializeField] private Animator anim;

	public enum TriggerType
	{
		MUDEMINION,
		DANDELION,
	};

	public TriggerType triggerType;

	public float fireRateTemp;
	public float offSet;
	public float attackTime;
	public int dice;
	public List<GameObject> skilledMonsters;

	public void Awake () {
		gameMaster = GameMaster.gameMaster;
		anim = GetComponent<Animator> ();
		attackTime = Time.time + offSet + fireRateTemp;
	}

	void Update () {
		if (GetComponent<Monster_Basic> ().damageMonsterType == Monster_Basic.DamageMonsterType.BASIC) {
			if (Time.time >= attackTime && !GetComponent<Monster_Basic> ().stoppedBool) {
				skilledMonsters.Clear ();
				if (triggerType == TriggerType.MUDEMINION) {
					skilledMonsters.AddRange (GameObject.FindGameObjectsWithTag ("Corps"));
				} else {
					for (int i = 0; i < gameMaster.remainingEnemies.Count; i++) {
						if (gameMaster.remainingEnemies [i].GetComponent<DarkMagician_Skilled> () != null && gameMaster.remainingEnemies [i].tag != "Corps") {
							skilledMonsters.Add (gameMaster.remainingEnemies [i]);
						}
					}
				}
				anim.SetTrigger ("Attack");
			}
		}
	}

	public void Skill () {
		dice = Random.Range (0, skilledMonsters.Count);
		skilledMonsters [dice].GetComponent<DarkMagician_Skilled> ().darkMagician = this;
		skilledMonsters [dice].GetComponent<DarkMagician_Skilled> ().Trigger ();
	}
}
