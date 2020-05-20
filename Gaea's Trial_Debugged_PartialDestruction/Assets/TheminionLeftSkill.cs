using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheminionLeftSkill : MonoBehaviour {

	public Theminion theminion;
	public int leftSkillDamage;

	void Start () {
		GameMaster.gameMaster.remainingEnemies.Add (this.gameObject);
	}

	public void Attack () {
		Castle.castle.GetComponent<Barricade> ().GetHurt (leftSkillDamage, 2, 0.1f);
	}
	
	public void Out () {
		theminion.anim.SetTrigger ("Skill1_Exit");
		GameMaster.gameMaster.remainingEnemies.Remove (this.gameObject);
		Destroy (this.gameObject);
	}
}
