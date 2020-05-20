using UnityEngine;
using System.Collections;

public class MWeaponSkill : MonoBehaviour {

	public Transform skill;
	public Transform ultSkill;
	public Transform firePoint;

	public void Skill () {
		Instantiate (skill, firePoint.position, firePoint.rotation);
	}

	public void UltSkill () {
		Instantiate (ultSkill, firePoint.position, firePoint.rotation);
	}
}
