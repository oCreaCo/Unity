using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LegendSkillButton : MonoBehaviour {

	public Button legendSkillButton;
	public float skillTime;
	public float coolTime;
	public int skillCount;
	public int skillCountTemp;
	public RectTransform coolTimeBar;

	void Awake () {
		legendSkillButton = GetComponent<Button> ();
		skillTime = (float)PlayerPrefs.GetInt ("GridEquippedHWeaponLegendSkillTime");
		coolTime = (float)PlayerPrefs.GetInt ("GridEquippedHWeaponLegendCoolTime");
		skillCount = PlayerPrefs.GetInt ("GridEquippedHWeaponLegendSkillCount");
		skillCountTemp = skillCount;
		StartCoroutine (SetCoolTimeBarChargeCoroutine (coolTime));
	}

	public void SetCoolTimeBar (float _cur, float _max) {
		float _value = 1f;
		if ((float)_cur / _max <= 1f) {
			_value = (float)_cur / _max;
		} else {
			_value = 1f;
		}
		coolTimeBar.localScale = new Vector3 (_value, coolTimeBar.localScale.y, coolTimeBar.localScale.z);
	}

	public IEnumerator SetCoolTimeBarChargeCoroutine (float _coolTime) {
		for (float i = 0; i <= _coolTime; i += 0.1f) {
			yield return new WaitForSeconds (0.1f);
			SetCoolTimeBar (i, _coolTime);
		}
		skillCountTemp = skillCount;
		legendSkillButton.interactable = true;
	}

	IEnumerator SetCoolTimeBarUseCoroutine (float _skillTime) {
		for (float i = 0; i <= _skillTime; i += 0.1f) {
			yield return new WaitForSeconds (0.1f);
			SetCoolTimeBar (_skillTime - i, _skillTime);
		}
	}

	public void LegendSkill () {
		legendSkillButton.interactable = false;
		if (skillCount == 0) {
			StartCoroutine (LegendSkillCoroutine (skillTime, coolTime));
		} else if (skillCount != 0) {
			if (HWeapon.hweapon.GetComponent<BasicAttackChanger> ().changeEffect != null) {
				Instantiate (HWeapon.hweapon.GetComponent<BasicAttackChanger> ().changeEffect, Vector3.zero, HWeapon.hweapon.transform.rotation, HWeapon.hweapon.transform);
			}
			if (PlayerPrefs.GetInt ("GridEquippedHWeapon") == 87) {
				coolTimeBar.GetComponent<Image> ().color = new Color32 (0, 225, 223, 225);
			} else if (PlayerPrefs.GetInt ("GridEquippedHWeapon") == 91) {
				coolTimeBar.GetComponent<Image> ().color = new Color32 (255, 0, 220, 255);
			}
		}
	}

	IEnumerator LegendSkillCoroutine (float _skillTime, float _coolTime) {
		for (float i = 0; i <= _skillTime; i += 0.1f) {
			SetCoolTimeBar (_skillTime - i, _skillTime);
			yield return new WaitForSeconds (0.097f);
		}
		for (float i = 0; i <= _coolTime; i += 0.1f) {
			yield return new WaitForSeconds (0.097f);
			SetCoolTimeBar (i, _coolTime);
		}
		legendSkillButton.interactable = true;
	}
}
