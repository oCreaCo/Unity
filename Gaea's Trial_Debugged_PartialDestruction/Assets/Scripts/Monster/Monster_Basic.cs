using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Monster_Basic : MonoBehaviour, IEventSystemHandler {

	public GameObject damageUI;
	public GameObject yellowAlert;

	public enum DamageMonsterType
	{
		BASIC,
		REFLECT,
		MIMIC,
	}

	public enum MonsterType
	{
		NORMAL,
		OBJECT,
		FIRE,
		ICE,
	}

	public float moveSpeed = 0.5f;
	[SerializeField]
	private float moveOffset = 0;
	public StatusIndicator statusIndicator;

	public float moveSpeedTemp;
	public bool crash;
	public ParticleSystem crashParticle;
	public int crashDamage;
	private int reflectDice;
	public int reflectPercent;
	public float oriHealth = 20;
	public float healthAdd;
	public float healthMultiplier; 
	public float curHealth;
	public float penetration = 1;
	public int score;
	public float destroyTime;
	public float checkTime;
	public bool collided;
	public bool dead = false;
	public GameObject deathParticle;
	public bool stoppedBool;
	public bool hurtable;
	public bool hurtAnimatable;
	public Transform damagmeUISpawnpoint;
	public Button.ButtonClickedEvent hurtFunction;

	public bool phase2;
	public int phase2Percent;

	public DamageMonsterType damageMonsterType;
	public MonsterType monsterType;
	public Animator anim;

	[System.Serializable]
	public struct Sound
	{
		public string soundName;
		public AudioClip audioClip;
	}

	public AudioSource audioSource;
	public Sound[] sound;

	public void DamageMonster (int _damage)
	{
		if (hurtable) {
			if (damageMonsterType == DamageMonsterType.REFLECT) {
				reflectDice = Random.Range (1, 101);
				if (reflectDice <= reflectPercent) {
//					GetComponent<MonsterRangeAttack> ().damage = _damage;
					penetration = 10;
					anim.SetTrigger ("Reflect");
					_damage = 0;
				} else
					penetration = 1;
			}
			if (anim != null && hurtAnimatable) {
				anim.SetTrigger ("Hurt");
			}
			curHealth -= _damage;
			if (statusIndicator != null) {
				statusIndicator.SetHealth (curHealth, oriHealth);
			}
			if (hurtFunction != null) {
				hurtFunction.Invoke ();
			}
			if (GetComponent<Recovery> () != null && !GetComponent<Recovery> ().healed && curHealth < oriHealth / 2) {
				GetComponent<Recovery> ().RecoveryHealth (anim, this);
			}
			GameObject damageUIClone = Instantiate (damageUI, damagmeUISpawnpoint.position, damagmeUISpawnpoint.rotation) as GameObject;
			damageUIClone.transform.FindChild ("DamageText").GetComponent<Text> ().text = _damage.ToString ();
			Destroy (damageUIClone.gameObject, 1f);
			if (curHealth < phase2Percent * (oriHealth / 100) && phase2) {
				phase2 = false;
				anim.SetTrigger ("Phase2");
			}
			if (curHealth <= 0) {
				if (statusIndicator != null) {
					statusIndicator.SetHealth (0, oriHealth);
				}
				dead = true;
				hurtable = false;
				if (GetComponent<SpawnPrefab> () != null) {
					GetComponent<SpawnPrefab> ().Spawn ();
				}
				if (GetComponent<MonsterDebuffBlock> () != null) {
					if (GetComponent<MonsterDebuffBlock> ().debuffCondition == MonsterDebuffBlock.DebuffCondition.DEATH) {
						GetComponent<MonsterDebuffBlock> ().Debuff ();
					}
				}
				if (GetComponent<Stalagmite> () != null) {
					GetComponent<Stalagmite> ().ListRemove ();
				}
				moveSpeedTemp = 0;
				if (anim != null) {
					anim.SetBool ("Dead", dead);
				}
				if (GameMaster.gameMaster != null) {
					GameMaster.gameMaster.yellowAlert = false;
					if (this.transform.position.x >= 1f) {
						GameMaster.gameMaster.curScore += 3 * score;
					} else if (this.transform.position.x < 1f && this.transform.position.x >= -0.5f) {
						GameMaster.gameMaster.curScore += 2 * score;
					} else if (this.transform.position.x < -0.5f) {
						GameMaster.gameMaster.curScore += 1 * score;
					}
					if (GetComponent<DarkMagician_Skilled> () != null) {
						this.tag = "Corps";
					} else {
						this.tag = "Untagged";
					}
					StartCoroutine (RemoveRemainingEnemies (checkTime));
				}
				if (yellowAlert != null) {
					yellowAlert = null;
					Destroy (this.gameObject.transform.FindChild ("yellowAlert(Clone)").gameObject);
				}
				if (audioSource!= null && sound.Length != 0) {
					audioSource.volume *= ((float)(PlayerPrefs.GetInt("EffectVolume") / 5f));
					PlayAudioSource (sound [0].audioClip);
				}
				Destroy (this.gameObject, destroyTime);
			}
		}
	}

	IEnumerator RemoveRemainingEnemies (float _time) {
		if (monsterType != MonsterType.OBJECT) {
			yield return new WaitForSeconds (_time);
			GameMaster.gameMaster.remainingEnemies.Remove (this.gameObject);
			GameMaster.gameMaster.StartCheckFinished (0.5f);
		}
	}

	IEnumerator MoveOffset (float moveOffset) {
		if (monsterType != MonsterType.OBJECT) {
			yield return new WaitForSeconds (moveOffset);
			if (GameMaster.gameMaster != null) {
				GameMaster.gameMaster.oriScore += 3 * score;
				GameMaster.gameMaster.remainingEnemies.Add (this.gameObject);
			}
		}
		collided = false;
		hurtable = true;
	}

	public void PlayAudioSource (AudioClip _clip) {
		audioSource.clip = _clip;
		audioSource.Play ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.GetComponent<Barricade> () != null) {
			collided = true;
			if (crash) {
				other.GetComponent<Barricade> ().GetHurt (crashDamage, 3f, 0.2f);
				crashParticle.Stop ();
			}
			if (GetComponent<MonsterMeleeAttack> () != null) {
				GetComponent<MonsterMeleeAttack> ().barricade = other.GetComponent<Barricade> ();
				GetComponent<MonsterMeleeAttack> ().collided = true;
				GetComponent<MonsterMeleeAttack> ().shouldAttackBarricade = true;
				GetComponent<MonsterMeleeAttack> ().attack = true;
				GetComponent<MonsterMeleeAttack> ().attackTime = Time.time + GetComponent<MonsterMeleeAttack> ().fireRateTemp;
			}
			if (GetComponent<MonsterMeleeAttack> () != null) {
				moveSpeedTemp = 0;
			}
			if (other.GetComponent<CollideMeleeAttack> () != null) {
				other.GetComponent<CollideMeleeAttack> ().triggeredMonster.Add (this.gameObject);
			}
		}

	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.GetComponent<Barricade> () != null) {
			collided = false;
			if (GetComponent<MonsterMeleeAttack> () != null) {
				GetComponent<MonsterMeleeAttack> ().barricade = null;
				GetComponent<MonsterMeleeAttack> ().collided = false;
				GetComponent<MonsterMeleeAttack> ().shouldAttackBarricade = false;
				GetComponent<MonsterMeleeAttack> ().attack = false;
			}
			if (curHealth > 0) moveSpeedTemp = moveSpeed;
		}
	}

	void Awake() {
		moveSpeedTemp = moveSpeed;
		if (WaveSpawner.waveSpawner != null) {
			healthMultiplier = WaveSpawner.waveSpawner.monsHealthMultiplier;
			healthAdd = WaveSpawner.waveSpawner.monsHealthAdd;
			if (WaveSpawner.waveSpawner.monsHealthType == WaveSpawner.MonsHealthType.ADD) {
				oriHealth += healthAdd;
			} else if (WaveSpawner.waveSpawner.monsHealthType == WaveSpawner.MonsHealthType.MULTIPLY) {
				oriHealth *= healthMultiplier;
			}
		}
		curHealth = oriHealth;
		if (GetComponent<Animator> () != null) {
			anim = GetComponent<Animator> ();
		}
		collided = true;
		StartCoroutine (MoveOffset (moveOffset));
	}

	void Update () {
		if (!collided) {
			transform.Translate (Vector3.left * Time.deltaTime * moveSpeedTemp);
		}

		if (Grid.grid != null) {
			if (this.transform.position.x > Grid.grid.groundRayCast.GetComponent<ARayCast> ().monsterXPosition && yellowAlert != null) {
				Destroy (this.gameObject.transform.FindChild ("yellowAlert(Clone)").gameObject);
				yellowAlert = null;
				GameMaster.gameMaster.yellowAlert = false;
			}
		}
	}

	public void HurtableTrigger () {
		if (GetComponent<Monster_Basic> ().hurtable) {
			GetComponent<Monster_Basic> ().hurtable = false;
		} else if (!GetComponent<Monster_Basic> ().hurtable) {
			GetComponent<Monster_Basic> ().hurtable = true;
		}
	}
}
