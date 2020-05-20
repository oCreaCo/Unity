using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWall : MonoBehaviour {

	private PartialDestruction partialDestruction;
	public Theminion theminion;

	[SerializeField] private GameObject heroAttack;
	[SerializeField] private Transform heroAttackSpawnPoint;
	[SerializeField] private Transform fireWallPoint;
	public bool activated;

	public void Awake () {
		partialDestruction = GetComponent<PartialDestruction> ();
		partialDestruction.ActivateButton (ref activated, 5f);
	}

	public void Exit () {
		theminion.rightSkillRateTemp = Random.Range (theminion.rightSkillRateMin, theminion.rightSkillRateMax); 
		theminion.rightSkillAttackTime = Time.time + theminion.rightSkillRateTemp;
		theminion.anim.SetTrigger ("Skill2_Exit");
	}

	public void PartialDestructionSuccessFunction() {
		GetComponent<Monster_Basic> ().DamageMonster (50000);
	}
}
