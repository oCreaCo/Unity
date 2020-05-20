using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PartialDestruction : MonoBehaviour, IEventSystemHandler {
	
	private Animator anim;
	[SerializeField] private GameObject activeButton;
	public bool activated;
	[SerializeField] private GameObject heroAttack;
	[SerializeField] private Transform heroAttackSpawnPoint;
	[SerializeField] private Transform heroAttackCameraPoint;
	[SerializeField] private GameObject magicBall;
	[SerializeField] private Transform magicBallSpawnPoint;
	[SerializeField] private Transform magicBallCameraPoint;
	[SerializeField] private Transform endPosTransform;
	[SerializeField] private float tapGaugeAddingValue;
	private Vector3 startPos, endPos;
	public Button.ButtonClickedEvent buttonAwakeFunction;
	public Button.ButtonClickedEvent successedFunction;

	public enum AttackType
	{
		HEROATTACK,
		MAGICBALL,
	};

	public AttackType attackType;

	void Awake () {
		anim = GetComponent<Animator> ();
		if (attackType == AttackType.HEROATTACK) {
			startPos = heroAttackCameraPoint.position;
		} else {
			startPos = magicBallCameraPoint.position;
		}
		CameraLerpPosition.cameraLerpPosition.partialDestruction = this;
	}

	public void ActivateButton (ref bool buttonActivated, float _time) {
		buttonActivated = true;
		activated = false;
		StartCoroutine (ActivateButtonCoroutine (_time));
	}

	private IEnumerator ActivateButtonCoroutine (float _time) {
		if (buttonAwakeFunction != null) {
			buttonAwakeFunction.Invoke ();
		}
		activeButton.SetActive (true);
		yield return new WaitForSeconds (_time);
		activeButton.SetActive (false);
	}

	public void Activate () {
		activeButton.SetActive (false);
		activated = true;
		CameraLerpPosition.cameraLerpPosition.tapGaugeAddingValue = tapGaugeAddingValue;
		CameraLerpPosition.cameraLerpPosition.cameraToPos = startPos;
		CameraLerpPosition.cameraLerpPosition.partialAnim = anim;
		if (attackType == AttackType.HEROATTACK) {
			GameObject heroAttackObject = Instantiate (heroAttack, heroAttackSpawnPoint.position, heroAttackSpawnPoint.rotation);
			CameraLerpPosition.cameraLerpPosition.followingObject = heroAttackObject;
		} else {
			GameObject magicBallObject = Instantiate (magicBall, magicBallSpawnPoint.position, magicBallSpawnPoint.rotation);
			CameraLerpPosition.cameraLerpPosition.followingObject = magicBallObject;
		}
		CameraLerpPosition.cameraLerpPosition.PartialDestroy ();
	}
	
	public void PartialDestructionFunction () {
		StartCoroutine (PartialDestructionCoroutine (2f));
	}

	IEnumerator PartialDestructionCoroutine (float _time) {
		CameraLerpPosition.cameraLerpPosition.Follow (_time);
		endPos = endPosTransform.position;
		Debug.LogError (startPos);
		Debug.LogError (endPos);
		for (float t = 0; t < 1 * _time; t += Time.deltaTime) {
			CameraLerpPosition.cameraLerpPosition.followingObject.transform.position = Vector3.Lerp (startPos, endPos, t / _time);
			yield return 0;
		}
		CameraLerpPosition.cameraLerpPosition.followingObject.transform.position = endPos;
		CameraLerpPosition.cameraLerpPosition.followingObject.GetComponent<Animator> ().SetTrigger ("End");
		anim.SetTrigger ("End");
		Destroy (CameraLerpPosition.cameraLerpPosition.followingObject, 2f);
		if (successedFunction != null) {
			successedFunction.Invoke ();
		}
		CameraLerpPosition.cameraLerpPosition.followingObject = null;
		GameMaster.gameMaster.stopped = false;
		for (int i = 0; i < GameMaster.gameMaster.remainingEnemies.Count; i++) {
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
		CameraLerpPosition.cameraLerpPosition.BackLerp ();
		activated = false;
	}
}
