using UnityEngine;
using System.Collections;

public class TriggerPurpleDamage : MonoBehaviour {

	public int damage;
	public int dotCount;
	public int penetration;

	public enum DamageType
	{
		ONCE,
		FIRE,
		BLOOD,
		THUNDER,
		DOT,
	}

	public DamageType damageType;

	public enum PenetrationType
	{
		WEAPON,
		INFINITE,
	}

	public PenetrationType penetrationType;

	void Awake () {
		damage = DamagePurpleInput.damagePurpleInput.damage;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Monster_Basic> () != null) {
			switch (damageType) {
			case DamageType.ONCE :
				other.GetComponent<Monster_Basic> ().DamageMonster (damage);
				break;
			case DamageType.FIRE:
				other.GetComponent<Monster_StatusController> ().Fired (damage, dotCount);
				break;
			case DamageType.BLOOD :
				other.GetComponent<Monster_StatusController> ().Blood (damage, dotCount);
				break;
			case DamageType.THUNDER:
				if (dotCount > GameMaster.gameMaster.remainingEnemies.Count) {
					dotCount = GameMaster.gameMaster.remainingEnemies.Count;
				}
				for (int i = 0; i < dotCount; i++) {
					if (damage < GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_Basic> ().curHealth) {
						GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_StatusController> ().ElectricShocked (5f);
					}
					GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_Basic> ().DamageMonster (damage);
				}
				break;
			case DamageType.DOT:
				StartCoroutine (DotDamageCoroutine (other, damage, dotCount));
				break;
			}
			if (penetrationType == PenetrationType.WEAPON) {
				penetration--;
				if (penetration <= 0) {
					GetComponent<Collider2D> ().enabled = false;
					Destroy (this.gameObject, 0.1f);
				}
			}
		} else if (other.tag == "End") Destroy (this.gameObject);
	}

	IEnumerator DotDamageCoroutine (Collider2D _other, int _damage, int _dotCount) {
		for (int i = 0; i < _dotCount; i++) {
			_other.GetComponent<Monster_Basic> ().DamageMonster (damage);
			yield return new WaitForSeconds (1.0f);
		}
	}
}
