using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GT_BasicAttack : MonoBehaviour {
	
	public GameObject slashEffect;
	[SerializeField] private GameMaster gameMaster;
	
	[SerializeField] float moveSpeed = 3.5f;
	public int damage;
	public int plusDamage;
	public float penetration;
	public int type;
	public int chance;
	public int fireDamage;
	public int bloodDamage;
	public float icedTime;
	public float electricShockedTime;
	public int dice;
	public int multiplier = 1;
	public bool end = false;
	private Animator anim;

	private bool specialTrigger;

	public enum PenetrationType
	{
		WEAPON,
		INFINITE,
	}

	public PenetrationType penetrationType;

	void OnTriggerEnter2D (Collider2D other)
	{
		GetComponent<AudioController> ().PlaySound ("BasicAttack");
		if (other.GetComponent<Monster_Basic> () != null) {
			other.GetComponent<Monster_Basic> ().DamageMonster (damage);
			if (penetrationType == PenetrationType.WEAPON) {
				Penetration (other.GetComponent<Monster_Basic> ().penetration);
			}
			if (specialTrigger && other.GetComponent<Monster_StatusController> () != null) {
				switch (type) {
				case 1:
					other.GetComponent<Monster_StatusController> ().Fired (fireDamage, 3);
					break;
				case 2:
					other.GetComponent<Monster_StatusController> ().Iced (icedTime);
					break;
				case 3:
					other.GetComponent<Monster_StatusController> ().ElectricShocked (electricShockedTime);
					break;
				case 4: 
					other.GetComponent<Monster_StatusController> ().Blood (bloodDamage, 5);
					break;
				}
			}
		}
		GameObject slashEffectClone = Instantiate (slashEffect, this.transform.position, this.transform.rotation) as GameObject;
		Destroy (slashEffectClone.gameObject, 1f);
		if (penetrationType == PenetrationType.WEAPON) {
			if (penetration <= 0) {
				end = true;
				if (anim != null) {
					anim.SetBool ("End", end);
				}
				GetComponent<Collider2D> ().enabled = false;
				Destroy (this.gameObject, 0.1f);
			}
		}
		if (other.tag == "End") Destroy (this.gameObject);
	}

	public void Damage (int _dmg, int _plusDamage)
	{
		damage = (_dmg * multiplier) + _plusDamage + plusDamage;
	}

	public void Penetration (float pen)
	{
		penetration -= pen;
	}

	void Awake()
	{
		anim = GetComponent<Animator> ();
		gameMaster = GameMaster.gameMaster;
		if (TutorialManager.tutorialManager == null) {
			penetration = (float)gameMaster.weaponPenetration;
			type = gameMaster.weaponType;
			chance = gameMaster.weaponChance;
			fireDamage = gameMaster.grid.heroLevelMaxDamage;
			bloodDamage = gameMaster.grid.heroLevelMinDamage;
			dice = Random.Range (1, 101);
			if (type != 0 && dice < chance) {
				specialTrigger = true;
				switch (type) {
				case 1:
					GetComponent<SpriteRenderer> ().color = new Color32 (255, 145, 9, 255);
					break;
				case 2:
					GetComponent<SpriteRenderer> ().color = new Color32 (167, 226, 255, 255);
					break;
				case 3:
					GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 0, 255);
					break;
				case 4: 
					GetComponent<SpriteRenderer> ().color = new Color32 (234, 0, 0, 255);
					break;
				}
			}
		} else if (TutorialManager.tutorialManager.animDelay != null) {
			penetration = 3;
		}
		Destroy (this.gameObject, 5);
	}

	void Update ()
	{
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
	}
}
