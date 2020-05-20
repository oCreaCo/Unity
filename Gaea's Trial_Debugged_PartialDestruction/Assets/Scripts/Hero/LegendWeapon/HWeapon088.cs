using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HWeapon088 : MonoBehaviour {

	[SerializeField] private GameMaster gameMaster;
	[SerializeField] private Grid grid;
	[SerializeField] private LegendSkillButton legendSkillButton;
	public Transform gloryJewels;
	public Sprite brokenWill, unBrokenWill;

	public float skillTime;
	public float coolTime;
	public int skillCount;
	private int skillCountTemp;

	void Start () {
		gameMaster = GameMaster.gameMaster;
		grid = gameMaster.grid;
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
		StartCoroutine (TheGloryDays (skillTime));
	}

	IEnumerator TheGloryDays (float time) {
		Instantiate (gloryJewels);
		yield return new WaitForSeconds (1.9f);
		grid.hWeapon.FindChild ("088_BrokenWill(Clone)").FindChild ("Graphic").FindChild ("HWeapon").GetComponent<SpriteRenderer> ().sprite = unBrokenWill;
		grid.allUltClear = true;
		yield return new WaitForSeconds (time - 1.9f);
		grid.hWeapon.FindChild ("088_BrokenWill(Clone)").FindChild ("Graphic").FindChild ("HWeapon").GetComponent<SpriteRenderer> ().sprite = brokenWill;
		grid.allUltClear = false;
	}
}
