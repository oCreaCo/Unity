using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HWeapon092 : MonoBehaviour {

	[SerializeField] private GameMaster gameMaster;
	[SerializeField] private LegendSkillButton legendSkillButton;
	[SerializeField] private Transform hephaistusHammer;

	public float skillTime;
	public float coolTime;
	public int skillCount;
	private int skillCountTemp;

	void Start () {
		gameMaster = GameMaster.gameMaster;
		if (gameMaster != null) {
			legendSkillButton = gameMaster.legendSkillBackGround.transform.FindChild ("LegendSkill").GetComponent<LegendSkillButton> ();
			skillTime = legendSkillButton.skillTime;
			coolTime = legendSkillButton.coolTime;
			skillCount = legendSkillButton.skillCount;
			skillCountTemp = skillCount;
		}
	}

	public void Skill () {
		legendSkillButton.LegendSkill ();
		Instantiate (hephaistusHammer);
	}
}
