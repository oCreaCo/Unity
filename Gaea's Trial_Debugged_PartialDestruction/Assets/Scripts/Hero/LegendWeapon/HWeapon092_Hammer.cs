using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HWeapon092_Hammer : MonoBehaviour {

	public GameMaster gameMaster;
	public Castle castle;
	[SerializeField] private float shakeAmount, shakeLength;
	[SerializeField] private int heroLevelMinDamage, heroLevelMaxDamage;
	[SerializeField] private GameObject particle;

	void Awake () {
		gameMaster = GameMaster.gameMaster;
		castle = gameMaster.barricades [0].GetComponent<Castle> ();
		heroLevelMinDamage = PlayerPrefs.GetInt ("HeroLevelMinDamage");
		heroLevelMaxDamage = PlayerPrefs.GetInt ("HeroLevelMaxDamage");
		Destroy (this.gameObject, 2.5f);
	}

	public void Damage () {
		CameraShake.cameraShake.Shake (shakeAmount, shakeLength);
		for (int i = 0; i < gameMaster.remainingEnemies.Count; i++) {
			if (gameMaster.remainingEnemies [i] != null) {
				gameMaster.remainingEnemies [i].GetComponent<Monster_Basic> ().DamageMonster (2 * (3 * heroLevelMaxDamage - 2 * heroLevelMinDamage));
			}
		}
	}

	public void Heal () {
		CameraShake.cameraShake.Shake (shakeAmount, shakeLength);
		castle.GetHealth (30);
	}

	public void ParticleActive () {
		particle.SetActive (true);
	}

	public void Fire () {
		for (int i = 0; i < gameMaster.remainingEnemies.Count; i++) {
			if (gameMaster.remainingEnemies [i] != null && gameMaster.remainingEnemies [i].GetComponent<Monster_StatusController>() != null) {
				gameMaster.remainingEnemies [i].GetComponent<Monster_StatusController> ().Fired (2 * heroLevelMaxDamage, 3);
			}
		}
	}
}
