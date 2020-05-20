using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class Cavian : MonoBehaviour, IEventSystemHandler {

	private Monster_Basic monsterBasic;
	private PartialDestruction partialDestruction;

	public List<GameObject> stalagmites = new List<GameObject> ();
	public int stalagmiteCount = 0;
	private int stalagmiteCountTemp = 0;
	public GameObject projectile;
	public Transform[] projectileSpawnPoint;
	public float shootingDelay;
	public float fireRateMin;
	public float fireRateMax;
	public float fireRateTemp;
	public float offSet;
	private float attackTime;
	public int stalagmiteDamage;
	public int damage;

	[SerializeField] private Button.ButtonClickedEvent failedFunction;
	public bool activated = false;
	private bool shootable = true;

	private Animator anim;

	void Awake () {
		anim = GetComponent<Animator> ();
		monsterBasic = GetComponent<Monster_Basic> ();
		partialDestruction = GetComponent<PartialDestruction> ();
		attackTime = Time.time + offSet + fireRateTemp;
	}

	void Update () {
		if (GetComponent<Monster_Basic> ().damageMonsterType == Monster_Basic.DamageMonsterType.BASIC) {
			if (Time.time >= attackTime && !GetComponent<Monster_Basic> ().stoppedBool && !GameMaster.gameMaster.stopped) {
				if (shootable) {
					anim.SetTrigger ("Shoot");
					if (stalagmiteCount > 0) {
						stalagmiteCountTemp = stalagmiteCount;
					}
					stalagmiteCount = 0;
					for (int i = 0; i < stalagmites.Count; i++) {
						stalagmites [i].GetComponent<Stalagmite> ().Move ();
					}
					stalagmites.Clear ();
				}
				fireRateTemp = Random.Range (fireRateMin, fireRateMax); 
				attackTime = Time.time + fireRateTemp;
			}
		}
	}

	public void CavianShoot() {
		if (!GetComponent<Monster_Basic> ().dead && !GetComponent<Monster_Basic> ().stoppedBool) {
			for (int i = 0; i < projectileSpawnPoint.Length; i++) {
				StartCoroutine (CavianShoot (i, shootingDelay));
			}
		}
	}

	IEnumerator CavianShoot (int i, float shootingDelay) {
		for (int j = 0; j < stalagmiteCountTemp; j++) {
			GameObject eProjectile = Instantiate (projectile, projectileSpawnPoint [i].position, projectileSpawnPoint [i].rotation) as GameObject;
			if (eProjectile.GetComponent<EnemyProjectile> () != null) {
				eProjectile.GetComponent<EnemyProjectile> ().damage = stalagmiteDamage;
			}
			yield return new WaitForSeconds (shootingDelay);
		}
		stalagmiteCountTemp = 0;
	}

	public void HurtFunction () {
		if (((monsterBasic.curHealth / monsterBasic.oriHealth) * 100 <= 30f && !activated)) {
			partialDestruction.ActivateButton (ref activated, 5f);
		}
	}

//	public void ButtonActive () {
//		StartCoroutine (ButtonActiveCoroutine ());
//	}
//
//	private IEnumerator ButtonActiveCoroutine () {
//		GetComponent<PartialDestruction> ().destroyedCount = 0;
//		GetComponent<PartialDestruction> ().destroyPart [GetComponent<PartialDestruction> ().destroyedCount].isDestroyed = false;
//		GetComponent<PartialDestruction> ().destroyPart [GetComponent<PartialDestruction> ().destroyedCount].button.SetActive (true);
//		CameraLerpPosition.cameraLerpPosition.cameraToPos = GetComponent<PartialDestruction> ().destroyPart [GetComponent<PartialDestruction> ().destroyedCount].partPosition.position;
//		CameraLerpPosition.cameraLerpPosition.partialAnim = anim;
//		yield return new WaitForSeconds (3f);
//		GetComponent<PartialDestruction> ().destroyedCount = 0;
//		GetComponent<PartialDestruction> ().destroyPart [GetComponent<PartialDestruction> ().destroyedCount].button.SetActive (false);
//	}
//
//	public void Activate () {
//		activated = true;
//		shootable = false;
//		GetComponent<MonsterRangeAttack> ().rangeAttackable = false;
//		if (GetComponent<PartialDestruction> ().healthCount != GetComponent<PartialDestruction> ().destroyPart.Length) {
//			GetComponent<PartialDestruction> ().finished = false;
//		}
//		GetComponent<PartialDestruction> ().healthCount = 1;
//		GameObject heroAttackObject = Instantiate (heroAttack, heroAttackSpawnPoint.position, heroAttackSpawnPoint.rotation);
//		CameraLerpPosition.cameraLerpPosition.followingObject = heroAttackObject;
//		GetComponent<PartialDestruction> ().destroyPart [GetComponent<PartialDestruction> ().destroyedCount].button.SetActive (false);
//		CameraLerpPosition.cameraLerpPosition.PartialDestroy ();
//	}
//
//	public void PartialDestruction () {
//		StartCoroutine (MagicBallMoveCoroutine (1f));
//	}
//
//	IEnumerator MagicBallMoveCoroutine (float _time) {
//		Vector3 startPos, endPos;
//		startPos = heroAttackSpawnPoint.position;
//		endPos = headPoint.position;
//		CameraLerpPosition.cameraLerpPosition.Follow (_time);
//		for (float t = 0; t < 1 * _time; t += Time.deltaTime) {
//			CameraLerpPosition.cameraLerpPosition.followingObject.transform.position = Vector3.Lerp (startPos, endPos, t / _time);
//			yield return 0;
//		}
//		CameraLerpPosition.cameraLerpPosition.followingObject.transform.position = endPos;
//		CameraLerpPosition.cameraLerpPosition.followingObject.GetComponent<Animator> ().SetTrigger ("End");
//		Destroy (CameraLerpPosition.cameraLerpPosition.followingObject, 2f);
//		CameraLerpPosition.cameraLerpPosition.followingObject = null;
//		GameMaster.gameMaster.stopped = false;
//		for (int i = 0; i < GameMaster.gameMaster.remainingEnemies.Count; i++) {
//			if (!GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_Basic> ().collided) {
//				GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_Basic> ().moveSpeedTemp = GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_Basic> ().moveSpeed;
//			}
//			if (GameMaster.gameMaster.remainingEnemies [i].GetComponent<Animator> () != null) {
//				GameMaster.gameMaster.remainingEnemies [i].GetComponent<Animator> ().enabled = true;
//			}
//			if (GameMaster.gameMaster.remainingEnemies [i].GetComponent<Collider2D> () != null) {
//				GameMaster.gameMaster.remainingEnemies [i].GetComponent<Collider2D> ().enabled = true;
//			}
//		}
//		activated = false;
//		StartCoroutine (Hide ());
//		GetComponent<PartialDestruction> ().finished = true;
//		CameraLerpPosition.cameraLerpPosition.BackLerp ();
//	}

	public void PartialDestructionSuccessFunction() {
		StartCoroutine (Hide ());
	}

	IEnumerator Hide () {
		anim.SetTrigger("HideAwake");
		yield return new WaitForSeconds (10f);
		shootable = true;
		GetComponent<MonsterRangeAttack> ().rangeAttackable = true;
		anim.SetTrigger("HideExit");
	}
}
