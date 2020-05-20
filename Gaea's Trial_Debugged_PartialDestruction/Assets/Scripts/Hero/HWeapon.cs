using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HWeapon : MonoBehaviour {
	public static HWeapon hweapon;

    public Transform basicAttack;
	public Transform firePoint;
	public int legendSkillCount;
	private Animator anim;
	private LegendSkillButton legendSkillButton;

	void Awake () {
		hweapon = this;
		anim = this.transform.FindChild("Graphic").GetComponent<Animator> ();
		if (GameMaster.gameMaster != null) {
			legendSkillButton = GameMaster.gameMaster.legendSkillBackGround.transform.FindChild ("LegendSkill").GetComponent<LegendSkillButton> ();
		}
	}

	void Update () {
		
	}

	public void Attack (int _damage, int _plusDamage)
	{
		anim.SetTrigger ("Attack");
//		AudioManager.instance.PlaySound ("Slash");
		if (legendSkillCount == 0) {
			if (GetComponent<BasicAttackChanger> () != null) {
				basicAttack = GetComponent<BasicAttackChanger> ().basicAttack;
			}
		} else if (legendSkillCount != 0) {
			basicAttack = GetComponent<BasicAttackChanger> ().specialAttack;
			if (PlayerPrefs.GetInt ("GridEquippedHWeapon") == 91) {
				switch (legendSkillCount) {
				case 4:
					this.transform.FindChild ("091_Crystal_Effect(Clone)").GetComponent<HWeapon091_Crystal> ().First ();
					break;
				case 3:
					this.transform.FindChild ("091_Crystal_Effect(Clone)").GetComponent<HWeapon091_Crystal> ().Second ();
					break;
				case 2:
					this.transform.FindChild ("091_Crystal_Effect(Clone)").GetComponent<HWeapon091_Crystal> ().Third ();
					break;
				case 1:
					this.transform.FindChild ("091_Crystal_Effect(Clone)").GetComponent<HWeapon091_Crystal> ().Fourth ();
					break;
				}
			}
			legendSkillCount--;
			legendSkillButton.SetCoolTimeBar ((float)legendSkillCount, legendSkillButton.skillCount);
			if (legendSkillCount == 0) {
				if (PlayerPrefs.GetInt ("GridEquippedHWeapon") == 91) {
					this.transform.FindChild ("091_Crystal_Effect(Clone)").GetComponent<HWeapon091_Crystal> ().anim.SetTrigger ("Break");
					Destroy (this.transform.FindChild ("091_Crystal_Effect(Clone)").gameObject, 3.5f);
				}
				legendSkillButton.coolTimeBar.GetComponent<Image> ().color = new Color32 (227, 55, 96, 255);
				legendSkillButton.StartCoroutine (legendSkillButton.SetCoolTimeBarChargeCoroutine (legendSkillButton.coolTime));
			}
		}
		Transform attack = Instantiate(basicAttack, firePoint.position, firePoint.rotation) as Transform;
		if (attack.GetComponent<GT_BasicAttack> () != null) {
			attack.GetComponent<GT_BasicAttack> ().Damage (_damage, _plusDamage);
		} else if (attack.GetComponent<DamageHeroInput> () != null) {
			attack.GetComponent<DamageHeroInput> ().Damage (_damage, _plusDamage);
		}
		if (Grid.grid.heroPrefab.GetComponent<Hero> ().copyHero != null) {
			Transform copyAttack = Instantiate(basicAttack, Grid.grid.heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().spawnPoint.position, Grid.grid.heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().spawnPoint.rotation) as Transform;
			if (copyAttack.GetComponent<GT_BasicAttack> () != null) {
				copyAttack.GetComponent<GT_BasicAttack> ().Damage (_damage, _plusDamage);
			} else if (copyAttack.GetComponent<DamageHeroInput> () != null) {
				copyAttack.GetComponent<DamageHeroInput> ().Damage (_damage, _plusDamage);
			}
			Grid.grid.heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().UltSkill (GameMaster.gameMaster.blueValue + GameMaster.gameMaster.blueSkillValue + GameMaster.gameMaster.blueWeaponValue, Random.Range (GameMaster.gameMaster.weaponMin, GameMaster.gameMaster.weaponMax + 1));
		}
    }
}
