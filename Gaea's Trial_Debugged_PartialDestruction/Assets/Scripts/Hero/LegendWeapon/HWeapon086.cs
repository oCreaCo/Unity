using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HWeapon086 : MonoBehaviour {

	[SerializeField] private GameMaster gameMaster;
	[SerializeField] private LegendSkillButton legendSkillButton;
	public Transform luxShield;
	public GameObject castleLuxShield;

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
			castleLuxShield = GameObject.FindGameObjectWithTag ("LegendSkillObjects").transform.FindChild ("LuxShield").gameObject;
		}
	}

	public void Skill () {
		legendSkillButton.LegendSkill ();
		castleLuxShield.SetActive (true);
		StartCoroutine (PowerOverWheling (skillTime));
	}

	IEnumerator PowerOverWheling (float time) {
		for (int i = 1; i < gameMaster.barricades.Count; i++) {
			gameMaster.barricades [i].powerOverwhelming = true;
			Instantiate (luxShield, gameMaster.barricades [i].transform.position, gameMaster.barricades [i].transform.rotation, gameMaster.barricades [i].transform);
		}
		yield return new WaitForSeconds (time);
		for (int i = 1; i < gameMaster.barricades.Count; i++) {
			gameMaster.barricades [i].powerOverwhelming = false;
			Destroy (gameMaster.barricades [i].transform.FindChild ("LuxShield(Clone)").gameObject);
		}
		castleLuxShield.SetActive (false);
	}
}
