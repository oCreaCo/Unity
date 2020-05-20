using UnityEngine;
using System.Collections;

public class TriggerHeroDamage : MonoBehaviour {

	public GameObject slashEffect;

	public enum PenType
	{
		ADD,
		INT,
	};

	public int damage;
	public bool infinite;
	public float penetration;
	public PenType penType;
	public float addPenetration;

	void Start () {
		damage = DamageHeroInput.damageHeroInput.damage;
		if (!infinite) {
			if (penType == PenType.ADD) {
				penetration = PlayerPrefs.GetInt ("GridEquippedHWeaponPenetration") + addPenetration;
			} else if (penType == PenType.INT) {
				penetration = penetration;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Monster_Basic> () != null) {
			other.GetComponent<Monster_Basic> ().DamageMonster (damage);
			penetration -= other.GetComponent<Monster_Basic> ().penetration;
		}
		if (slashEffect != null) {
			GameObject slashEffectClone = Instantiate (slashEffect, this.transform.position, this.transform.rotation) as GameObject;
			Destroy (slashEffectClone.gameObject, 1f);
		}
		if (!infinite) {
			if (penetration <= 0) {
				GetComponent<Collider2D> ().enabled = false;
				Destroy (this.transform.parent.gameObject, 0.1f);
			}
		}
		if (other.tag == "End") Destroy (this.transform.parent.gameObject);
	}
}
