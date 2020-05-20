using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SandWhale : MonoBehaviour, IEventSystemHandler {

	public PartialDestruction partialDestruction;

	public Transform monsterSpawnPoint;
	public Animator anim;

	public bool opened = false;

	[SerializeField]
	public int monsNum, monsSize, monsSpawnSize;

	public float getHealth, healDelay;

	public float waveDelay, spawnDelay;
	[SerializeField] private bool spawning = false;
	[SerializeField] private bool activated = false;

	[SerializeField] private Button.ButtonClickedEvent failedFunction;
	[SerializeField] private Monster_Basic monsterBasic;

	public bool spawned1, spawned2, spawned3;
	public float spawned1Percent, spawned2Percent, spawned3Percent;

	void Awake () {
		anim = GetComponent<Animator> ();
		monsterBasic = GetComponent<Monster_Basic> ();
		partialDestruction = GetComponent<PartialDestruction> ();
		CameraLerpPosition.cameraLerpPosition.failedFunction = failedFunction;
		StartCoroutine (SpawnMonsters ());
		StartCoroutine (HealCoroutine (healDelay));
	}

	IEnumerator SpawnMonsters () {
		while (true) {
			waveDelay = Random.Range (7, 11);
			yield return new WaitForSeconds (waveDelay);
			if (!partialDestruction.activated && !spawning && !GetComponent<Monster_Basic> ().dead) {
				opened = true;
				anim.SetBool ("Opened", opened);
				monsSpawnSize = Random.Range (1, 3);
				for (int i = 0; i < monsSpawnSize; i++) {
					monsNum = Random.Range (6, 10);
					monsSize = Random.Range (3, 6);
					for (int j = 0; j < monsSize; j++) {
						if (!partialDestruction.activated && !spawning && !GetComponent<Monster_Basic> ().dead) {
							Instantiate (WaveSpawner.waveSpawner.monsters [monsNum], monsterSpawnPoint.position, monsterSpawnPoint.rotation);
						}
						yield return new WaitForSeconds (spawnDelay);
					}
				}
				if (!partialDestruction.activated && !spawning && !GetComponent<Monster_Basic> ().dead) {
					opened = false;
					anim.SetBool ("Opened", opened);
				}
			}
		}
	}

	public void HurtFunction () {
		if (((monsterBasic.curHealth / monsterBasic.oriHealth) * 100 <= spawned1Percent && !spawned1) ||
			((monsterBasic.curHealth / monsterBasic.oriHealth) * 100 <= spawned2Percent && !spawned2) ||
			((monsterBasic.curHealth / monsterBasic.oriHealth) * 100 <= spawned3Percent && !spawned3)) {
			SpawnLotsMonsters ();
		}
	}

	IEnumerator	HealCoroutine (float _time) {
		while (true) {
			yield return new WaitForSeconds (_time);
			if (!activated) {
				Heal ();
			}
		}
	}

	public void Heal () {
		monsterBasic.curHealth += getHealth;
		if (monsterBasic.curHealth > monsterBasic.oriHealth) {
			monsterBasic.curHealth = monsterBasic.oriHealth;
		}
		if (monsterBasic.statusIndicator != null) {
			monsterBasic.statusIndicator.SetHealth (monsterBasic.curHealth, monsterBasic.oriHealth);
		}
		if (spawned1 && (monsterBasic.curHealth / monsterBasic.oriHealth) * 100 >= spawned1Percent) {
			spawned1 = false;
		} else if (spawned2 && (monsterBasic.curHealth / monsterBasic.oriHealth) * 100 < spawned1Percent && (monsterBasic.curHealth / monsterBasic.oriHealth) * 100 >= spawned2Percent) {
			spawned2 = false;
		} else if (spawned3 && (monsterBasic.curHealth / monsterBasic.oriHealth) * 100 < spawned2Percent && (monsterBasic.curHealth / monsterBasic.oriHealth) * 100 >= spawned3Percent) {
			spawned3 = false;
		} 
	}

	public void SpawnLotsMonsters () {
		opened = true;
		spawning = true;
		anim.SetBool ("Opened", opened);
		if (!spawned1) {
			partialDestruction.ActivateButton (ref spawned1, 5f);
		} else if (!spawned2) {
			partialDestruction.ActivateButton (ref spawned2, 5f);
		} else if (!spawned3) {
			partialDestruction.ActivateButton (ref spawned3, 5f);
		}
		StartCoroutine (SpawnLotsMonstersCoroutine (5.0f));
	}

	IEnumerator SpawnLotsMonstersCoroutine (float _time) {
		yield return new WaitForSeconds (_time);
		if (!partialDestruction.activated) {
			monsterBasic.hurtable = false;
			for (int i = 0; i < 5; i++) {
				monsNum = Random.Range (6, 10);
				monsSize = Random.Range (3, 6);
				for (int j = 0; j < monsSize; j++) {
					Instantiate (WaveSpawner.waveSpawner.monsters [monsNum], monsterSpawnPoint.position, monsterSpawnPoint.rotation);
					yield return new WaitForSeconds (1f);
				}
			}
			monsterBasic.hurtable = true;
			opened = false;
			anim.SetBool ("Opened", opened);
			spawning = false;
		}
	}

	public void FailedFunction () {
		partialDestruction.activated = false;
		StartCoroutine (SpawnLotsMonstersCoroutine (0.0f));
	}

	public void PartialDestructionSuccessFunction() {
		opened = false;
		anim.SetBool ("Opened", opened);
		spawning = false;
	}
}
