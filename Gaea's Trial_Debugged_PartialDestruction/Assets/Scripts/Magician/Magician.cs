using UnityEngine;
using System.Collections;

public class Magician : MonoBehaviour {
	public static Magician magician;

	public GameObject basicAttack;
	public GameObject spawnEffect;
	public Transform firePoint;

	void Awake () {
		magician = this;
	}

	public void  Attack (int _dmg, int _plusDamage) {
		GetComponent<AudioController> ().PlaySound ("Cast");
		GameObject mBasicAttack = Instantiate(basicAttack, firePoint.position, firePoint.rotation) as GameObject;
		mBasicAttack.GetComponent<MBasicAttack> ().Damage (_dmg, _plusDamage);
		GameObject effectClone = Instantiate (spawnEffect, firePoint.position, firePoint.rotation) as GameObject;
		Destroy (effectClone, 1f);
	}
}
