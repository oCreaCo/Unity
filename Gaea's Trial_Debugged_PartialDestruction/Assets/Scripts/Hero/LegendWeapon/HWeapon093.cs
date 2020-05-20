using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HWeapon093 : MonoBehaviour {

	[SerializeField] private GameMaster gameMaster;
	[SerializeField] private LegendSkillButton legendSkillButton;
	[SerializeField] private Transform eveSpikes;
	private int heroLevelMinDamage, heroLevelMaxDamage;

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
			heroLevelMinDamage = PlayerPrefs.GetInt ("HeroLevelMaxDamage");
			heroLevelMaxDamage = PlayerPrefs.GetInt ("HeroLevelMinDamage");
		}
	}

	public void Skill () {
		legendSkillButton.LegendSkill ();
		Transform eveSpikesTransform = Instantiate (eveSpikes) as Transform;
		eveSpikesTransform.GetComponent<DamageHeroInput> ().Damage (3 * (3 * heroLevelMinDamage - 2 * heroLevelMaxDamage), 0);
	}
}
