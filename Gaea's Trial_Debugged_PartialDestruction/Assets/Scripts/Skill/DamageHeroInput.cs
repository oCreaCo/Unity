using UnityEngine;
using System.Collections;

public class DamageHeroInput : MonoBehaviour {
	public static DamageHeroInput damageHeroInput;

	public enum DamageType
	{
		INT,
		VALUE,
	}

	public DamageType damageType;

	[SerializeField] public int damage = 10;
	public int plusDamage;
	public int multiplier;

	public void Damage (int _dmg, int _plusDamage)
	{
		if (damageType == DamageType.VALUE) {
			damage = multiplier * _dmg + _plusDamage + plusDamage;
		}
	}

	void Awake () {
		damageHeroInput = this;
	}
}
