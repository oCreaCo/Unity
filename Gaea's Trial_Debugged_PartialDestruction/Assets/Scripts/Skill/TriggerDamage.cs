using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour {

	private int damage;
	public int multiplier;

	public void Awake () {
		damage = PlayerPrefs.GetInt ("HeroLevelMaxDamage") * multiplier;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Monster_Basic> () != null) {
			other.GetComponent<Monster_Basic> ().DamageMonster (damage);
		}
	}
}
