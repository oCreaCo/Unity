using UnityEngine;
using System.Collections;

public class DamagePurpleInput : MonoBehaviour {
	public static DamagePurpleInput damagePurpleInput;
	
	public int damage = 10;
	public int plusDamage;
	public float multiplier;

	public void Damage (int dmg)
	{
		damage = (int)(multiplier * dmg) + plusDamage;
	}

	void Awake () {
		damagePurpleInput = this;
		plusDamage = GameMaster.gameMaster.purpleWeaponValue;
		Damage (GameMaster.gameMaster.purpleValue + GameMaster.gameMaster.purpleSkillValue + GameMaster.gameMaster.purpleBurningAddValue);
	}
}
