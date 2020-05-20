using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theminion : MonoBehaviour {

	private Monster_Basic monsterBasic;
	public Animator anim;

	[SerializeField]
	public int monsNum, monsSize, monsSpawnSize;
	public float waveDelay, spawnDelay;
	public bool canSpawn;
	public Transform monsterSpawnPoint;

	public float leftSkillDelay;
	public float leftSkillRateMin;
	public float leftSkillRateMax;
	public float leftSkillRateTemp;
	public float leftSkillOffSet;
	public float leftSkillAttackTime;
	public GameObject leftSkillSpawnObject;
	private GameObject leftSkillTemp;
	public Transform leftSkillSpawnPoint;
	public int leftSkillDamage;

	public float rightSkillDelay;
	public float rightSkillRateMin;
	public float rightSkillRateMax;
	public float rightSkillRateTemp;
	public float rightSkillOffSet;
	public float rightSkillAttackTime;
	public GameObject fireWall;
	public Transform handL;

	public GameObject darken;
	public ParticleSystem tearParticle;

	public GameObject endingGaea;

	void Awake () {
		monsterBasic = GetComponent<Monster_Basic> ();
		anim = GetComponent<Animator> ();
		monsterSpawnPoint = WaveSpawner.waveSpawner.spawnPoints [0];
		leftSkillRateTemp = Random.Range(leftSkillRateMin, leftSkillRateMax);
		rightSkillRateTemp = Random.Range(rightSkillRateMin, rightSkillRateMax);
		leftSkillAttackTime = Time.time + leftSkillOffSet + leftSkillRateTemp;
		rightSkillAttackTime = Time.time + rightSkillOffSet + rightSkillRateTemp;
	}

	void Update () {
		if (GetComponent<Monster_Basic> ().damageMonsterType == Monster_Basic.DamageMonsterType.BASIC) {
			if (leftSkillRateTemp != 0 && Time.time >= leftSkillAttackTime && !GetComponent<Monster_Basic> ().stoppedBool) {
				anim.SetTrigger ("Skill1_Awake");
				leftSkillRateTemp = Random.Range (leftSkillRateMin, leftSkillRateMax); 
				leftSkillAttackTime = Time.time + leftSkillRateTemp;
			}
			if (rightSkillRateTemp != 0 && Time.time >= rightSkillAttackTime && !GetComponent<Monster_Basic> ().stoppedBool) {
				anim.SetTrigger ("Skill2_Awake");
			}
		}
	}

	public void SpawnMonsterTrigger () {
		if (!canSpawn) {
			canSpawn = true;
			StartCoroutine (SpawnMonsters ());
		} else {
			canSpawn = false;
			StopCoroutine (SpawnMonsters ());
		}
	}

	IEnumerator SpawnMonsters () {
		while (canSpawn) {
			if (!GetComponent<Monster_Basic> ().dead) {
				monsSpawnSize = Random.Range (1, 3);
				for (int i = 0; i < monsSpawnSize; i++) {
					monsNum = Random.Range (45, 49);
					monsSize = Random.Range (3, 6);
					for (int j = 0; j < monsSize; j++) {
						if (!GetComponent<Monster_Basic> ().dead) {
							Instantiate (WaveSpawner.waveSpawner.monsters [monsNum], monsterSpawnPoint.position, monsterSpawnPoint.rotation);
						}
						yield return new WaitForSeconds (spawnDelay);
					}
				}
			}
			waveDelay = Random.Range (7, 11);
			yield return new WaitForSeconds (waveDelay);
		}
	}

	public void LeftSkill () {
		StartCoroutine (LeftSkillCoroutine ());
	}

	public void RightSkillSpawnFireWall () {
		GameObject fireWallObject = Instantiate (fireWall, handL.localPosition, handL.localRotation, handL) as GameObject;
		fireWallObject.GetComponent<FireWall> ().theminion = this;
	}

	IEnumerator LeftSkillCoroutine () {
		GameObject tmp = Instantiate (leftSkillSpawnObject, leftSkillSpawnPoint.position, leftSkillSpawnPoint.rotation) as GameObject;
		tmp.GetComponent<TheminionLeftSkill> ().theminion = this;
		tmp.GetComponent<TheminionLeftSkill> ().leftSkillDamage = leftSkillDamage;
		leftSkillTemp = tmp;
		yield return new WaitForSeconds (3f);
		Castle.castle.GetComponent<Barricade> ().GetHurt (leftSkillDamage, 2, 0.1f);
		yield return new WaitForSeconds (leftSkillDelay);
		Instantiate (leftSkillSpawnObject, leftSkillSpawnPoint.position, leftSkillSpawnPoint.rotation);
		yield return new WaitForSeconds (3f);
		Castle.castle.GetComponent<Barricade> ().GetHurt (leftSkillDamage, 2, 0.1f);
		yield return new WaitForSeconds (leftSkillDelay);
		Instantiate (leftSkillSpawnObject, leftSkillSpawnPoint.position, leftSkillSpawnPoint.rotation);
		yield return new WaitForSeconds (3f);
		Castle.castle.GetComponent<Barricade> ().GetHurt (leftSkillDamage, 2, 0.1f);
		yield return new WaitForSeconds (leftSkillDelay);
		anim.SetTrigger ("Skill1_Exit");
	}

	public void DeleteTmp () {
		if (leftSkillTemp != null) {
			Destroy (leftSkillTemp);
			leftSkillTemp = null;
		}
	}

	public void DeleteFireWall () {
		if (handL.FindChild ("FireWall(Clone)") != null) {
			Destroy (handL.FindChild ("FireWall(Clone)").gameObject);
		}
	}

	public void RightSkillBuff () {
		for (int i = 0; i < GameMaster.gameMaster.remainingEnemies.Count; i++) {
			if (GameMaster.gameMaster.remainingEnemies [i].GetComponent<World8Buffs> () != null) {
				GameMaster.gameMaster.remainingEnemies [i].GetComponent<World8Buffs> ().Buff ();
			}
		}
	}

	public void Magma () {
		if (!darken.activeInHierarchy) {
			darken.SetActive (true);
			canSpawn = false;
			if (GameMaster.gameMaster.remainingEnemies.Count > 1) {
				for (int i = 1; i < GameMaster.gameMaster.remainingEnemies.Count; i++) {
					if (!GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_Basic> ().collided) {
						GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_Basic> ().moveSpeedTemp = 0;
					}
					if (GameMaster.gameMaster.remainingEnemies [i].GetComponent<Animator> () != null) {
						GameMaster.gameMaster.remainingEnemies [i].GetComponent<Animator> ().enabled = false;
					}
					if (GameMaster.gameMaster.remainingEnemies [i].GetComponent<Collider2D> () != null) {
						GameMaster.gameMaster.remainingEnemies [i].GetComponent<Collider2D> ().enabled = false;
					}
				}
			}
			tearParticle.gameObject.SetActive (true);
			for (int x = 0; x < 6; x++) {
				for (int y = 4; y < 6; y++) {
					Grid.grid.pieces [x, y].GetComponent<Collider2D> ().enabled = false;
					Grid.grid.pieces [x, y].GetComponent<GamePiece> ().pieceDebuffType = GamePiece.PieceDebuffType.CHAINED;
					Grid.grid.pieces [x, y].GetComponent<MovablePiece> ().enabled = false;
					Grid.grid.pieces [x, y].GetComponent<ClearablePiece> ().enabled = false;
				}
			}
		} else {
			darken.SetActive (false);
			canSpawn = true;
			if (GameMaster.gameMaster.remainingEnemies.Count > 1) {
				for (int i = 1; i < GameMaster.gameMaster.remainingEnemies.Count; i++) {
					if (!GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_Basic> ().collided) {
						GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_Basic> ().moveSpeedTemp = GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_Basic> ().moveSpeed;
					}
					if (GameMaster.gameMaster.remainingEnemies [i].GetComponent<Animator> () != null) {
						GameMaster.gameMaster.remainingEnemies [i].GetComponent<Animator> ().enabled = true;
					}
					if (GameMaster.gameMaster.remainingEnemies [i].GetComponent<Collider2D> () != null) {
						GameMaster.gameMaster.remainingEnemies [i].GetComponent<Collider2D> ().enabled = true;
					}
				}
			}
		}
	}

	public IEnumerator TearStopCoroutine () {
		yield return new WaitForSeconds (3.6f);
		tearParticle.Stop ();
		yield return new WaitForSeconds (1f);
		tearParticle.gameObject.SetActive (false);
	}

	public void AnimAwake () {
		anim.SetTrigger ("Awake");
		anim.SetLayerWeight (2, 1);
		anim.SetLayerWeight (3, 1);
	}

	public void LeftExit () {
		anim.SetTrigger ("Skill1_Exit");
	}

	public void Ending () {
		if (PlayerPrefs.GetInt ("Ended") == 0) {
			Instantiate (endingGaea);
			int jewel = 1;
			PlayerPrefs.SetInt ("world" + GameMaster.gameMaster.worldNum + "Clear" + GameMaster.gameMaster.stageNum, 2);
			if (PlayerPrefs.GetInt ("world" + GameMaster.gameMaster.nextWorldNum + "Clear" + GameMaster.gameMaster.nextStageNum) == 0) {
				PlayerPrefs.SetInt ("world" + GameMaster.gameMaster.nextWorldNum + "Clear" + GameMaster.gameMaster.nextStageNum, 1);
			}
			if ((float)(Castle.castle.GetComponent<Barricade> ().curHealth) > (float)(Castle.castle.GetComponent<Barricade> ().oriHealth) * 0.33f) {
				jewel = 2;
			}
			if ((float)(Castle.castle.GetComponent<Barricade> ().curHealth) > (float)(Castle.castle.GetComponent<Barricade> ().oriHealth) * 0.66f) {
				jewel = 3;
			}
			int temp = PlayerPrefs.GetInt ("World" + GameMaster.gameMaster.worldNum + "Stage" + GameMaster.gameMaster.stageNum + "Jewels");
			if (jewel > temp) {
				PlayerPrefs.SetInt ("World" + GameMaster.gameMaster.worldNum + "Stage" + GameMaster.gameMaster.stageNum + "Jewels", jewel);
			}
			GemGoldScript.gemGoldScript.PlusGold(GameMaster.gameMaster.stageGold * 5);
			GameMaster.gameMaster.stageGold = 0;
		}
	}
}
