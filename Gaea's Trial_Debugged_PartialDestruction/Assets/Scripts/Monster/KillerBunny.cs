using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerBunny : MonoBehaviour {

	private PartialDestruction partialDestruction;

	public float winkRateMin;
	public float winkRateMax;
	public float winkRateTemp;
	public float winkOffSet;
	private float winkAttackTime;

	public int damage;
	public float fireRateMin;
	public float fireRateMax;
	public float fireRateTemp;
	public float offSet;
	private float attackTime;
	public float cameraShakeMultiplier;
	public float cameraShakeLength = 0.2f;
	public bool activated;

	private Animator anim;

	void Awake() {
		anim = GetComponent<Animator> ();
		partialDestruction = GetComponent<PartialDestruction> ();
		winkRateTemp = Random.Range(winkRateMin, winkRateMax);
		winkAttackTime = Time.time + winkOffSet + winkRateTemp;
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<Monster_Basic> ().damageMonsterType == Monster_Basic.DamageMonsterType.BASIC) {
			if (Time.time >= winkAttackTime && !GetComponent<Monster_Basic> ().stoppedBool) {
				anim.SetTrigger ("Wink");
				winkRateTemp = Random.Range (winkRateMin, winkRateMax); 
				winkAttackTime = Time.time + winkRateTemp;
			}
			if (Time.time >= attackTime && !GetComponent<Monster_Basic> ().stoppedBool) {
				anim.SetTrigger ("Attack");
				fireRateTemp = Random.Range (fireRateMin, fireRateMax); 
				attackTime = Time.time + fireRateTemp;
			}
		}
	}

	public void Wink () {
		activated = false;
		GameMaster.gameMaster.IsClickable (false);
		for (int x = 0; x < 6; x++) {
			for (int y = 0; y < 6; y++) {
				Grid.grid.pieces [x, y].value = 0;
				Grid.grid.pieces [x, y].transform.FindChild ("Graphic").GetComponent<DamageIndicator> ().SetDamage (Grid.grid.pieces [x, y].value);
			}
		}
		GameMaster.gameMaster.IsClickable (true);
		partialDestruction.ActivateButton (ref activated, 5f);
	}

	public void ShockWave () {
		Castle.castle.GetComponent<Barricade> ().GetHurt (damage, cameraShakeMultiplier, cameraShakeLength);
	}

//	private IEnumerator ButtonActiveCoroutine () {
//		GetComponent<PartialDestruction> ().destroyedCount = 0;
//		GetComponent<PartialDestruction> ().destroyPart [GetComponent<PartialDestruction> ().destroyedCount].isDestroyed = false;
//		GetComponent<PartialDestruction> ().destroyPart [GetComponent<PartialDestruction> ().destroyedCount].button.SetActive (true);
//		CameraLerpPosition.cameraLerpPosition.tapGaugeAddingValue = GetComponent<PartialDestruction> ().destroyPart [GetComponent<PartialDestruction> ().destroyedCount].tapGaugeAddingValue;
//		CameraLerpPosition.cameraLerpPosition.cameraToPos = GetComponent<PartialDestruction> ().destroyPart [GetComponent<PartialDestruction> ().destroyedCount].partPosition.position;
//		CameraLerpPosition.cameraLerpPosition.partialAnim = anim;
//		yield return new WaitForSeconds (3f);
//		GetComponent<PartialDestruction> ().destroyedCount = 0;
//		GetComponent<PartialDestruction> ().destroyPart [GetComponent<PartialDestruction> ().destroyedCount].button.SetActive (false);
//	}
//
//	public void Activate () {
//		GetComponent<PartialDestruction> ().healthCount = 0;
//		GameObject magicBallObject = Instantiate (magicBall, magicBallSpawnPoint.position, magicBallSpawnPoint.rotation);
//		CameraLerpPosition.cameraLerpPosition.followingObject = magicBallObject;
//		CameraLerpPosition.cameraLerpPosition.PartialDestroy ();
//	}
//
//	public void PartialDestruction () {
////		GetComponent<PartialDestruction> ().healthCount++;
//		StartCoroutine (HeroAttackMoveCoroutine (1f));
//	}
//
//	IEnumerator HeroAttackMoveCoroutine (float _time) {
//		Vector3 startPos, endPos;
//		startPos = magicBallSpawnPoint.position;
//		endPos = magicAttackPoint.position;
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
//		CameraLerpPosition.cameraLerpPosition.BackLerp ();
	//	}

	public void PartialDestructionSuccessFunction() {
		GetComponent<Monster_Basic> ().DamageMonster (100);
	}

	public void JumpSound () {
		GetComponent<Monster_Basic> ().PlayAudioSource (GetComponent<Monster_Basic> ().sound [0].audioClip);
	}
}
