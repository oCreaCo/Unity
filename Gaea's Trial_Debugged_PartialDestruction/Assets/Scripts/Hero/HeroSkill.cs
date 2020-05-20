using UnityEngine;
using System.Collections;

public class HeroSkill : MonoBehaviour {

	public GameObject horSkill;
	public GameObject verSkill;
	public GameObject ultSkill;

	public Transform spawnPoint;

	public void HorSkill (int _damage,  int _plusDamage)
	{
		if (horSkill != null) {
			GameObject horizonSkill =  Instantiate (horSkill, spawnPoint.position, spawnPoint.rotation) as GameObject;
			if (horizonSkill.GetComponent<DamageHeroInput>() != null) horizonSkill.GetComponent<DamageHeroInput> ().Damage (_damage, _plusDamage);
		}
	}

	public void VerSkill (int _damage,  int _plusDamage)
	{
		if (verSkill != null) {
			GameObject verticalSkill = Instantiate (verSkill, spawnPoint.position, spawnPoint.rotation) as GameObject;
			if (verticalSkill.GetComponent<DamageHeroInput>() != null) verticalSkill.GetComponent<DamageHeroInput> ().Damage (_damage, _plusDamage);
		}
	}

	public void UltSkill (int _damage,  int _plusDamage)
		{
		if (ultSkill != null) {
			GameObject ultimateSkill = Instantiate (ultSkill, spawnPoint.position, spawnPoint.rotation) as GameObject;
			if (ultimateSkill.GetComponent<DamageHeroInput>() != null) ultimateSkill.GetComponent<DamageHeroInput> ().Damage (_damage, _plusDamage);
		}
	}
}
