using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraLerpPosition : MonoBehaviour, IEventSystemHandler {

	public static CameraLerpPosition cameraLerpPosition;
	public PartialDestruction partialDestruction;

	[SerializeField] private GameObject camera;
	[SerializeField] private RectTransform TapGaugeBarRect;
	[SerializeField] private GameObject tapCollider;
	public GameObject followingObject;
	public Animator partialAnim;

	private Vector3 cameraOriPos;
	public Vector3 cameraToPos;
	public float tapGaugeAddingValue;
	public float movingTime;
	bool triggered;
	public float tapGauge;
	private bool succeeded = false;
	public Button.ButtonClickedEvent failedFunction;

	public void Start () {
		if (cameraLerpPosition == null) {
			cameraLerpPosition = this;
		}
		camera = this.gameObject;
		cameraOriPos = this.transform.position;
	}

	public void PartialDestroy () {
		if (!GameMaster.gameMaster.stopped) {
			GameMaster.gameMaster.stopped = true;
			for (int i = 0; i < GameMaster.gameMaster.remainingEnemies.Count; i++) {
				GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_Basic> ().moveSpeedTemp = 0;
				if (GameMaster.gameMaster.remainingEnemies [i].GetComponent<Animator> () != null) {
					GameMaster.gameMaster.remainingEnemies [i].GetComponent<Animator> ().enabled = false;
				}
				if (GameMaster.gameMaster.remainingEnemies [i].GetComponent<Collider2D> () != null) {
					GameMaster.gameMaster.remainingEnemies [i].GetComponent<Collider2D> ().enabled = false;
				}
			}
			TapGaugeBarRect.gameObject.SetActive (true);
			GoLerp ();
			TapCollierToggle ();
			CameraGoldSkill.tapCollider.tapGaugeAddValue = tapGaugeAddingValue;
			StartCoroutine (SetTapGaugeBarCoroutine (5f));
		}
	}

	IEnumerator SetTapGaugeBarCoroutine (float _time) {
		tapGauge = 0;
		succeeded = false;
		for (float i = 0; i <= _time; i += 0.01f) {
			yield return new WaitForSeconds (0.01f);
			tapGauge -= 0.64f;
			if (tapGauge < 0) {
				tapGauge = 0;
			}
			SetTapGaugeBar (tapGauge, 100);
			if (tapGauge >= 100f) {
				succeeded = true;
				SetTapGaugeBar (1, 1);
				partialDestruction.PartialDestructionFunction ();
				break;
			}
		}
		tapGauge = 0;
		TapGaugeBarRect.gameObject.SetActive (false);
		if (!succeeded) {
			partialDestruction.activated = false;
			if (followingObject != null) {
				Destroy (followingObject);
				followingObject = null;
			}
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
			if (failedFunction != null) {
				failedFunction.Invoke ();
			}
			BackLerp ();
		}
		TapCollierToggle ();
	}

	public void TapCollierToggle () {
		if (!tapCollider.activeInHierarchy) {
			tapCollider.SetActive (true);
		} else {
			tapCollider.SetActive (false);
		}
	}

	public void SetTapGaugeBar (float _cur, float _max) {
		float _value = 1f;
		if ((float)_cur / _max <= 1) {
			_value = (float)_cur / _max;
		} else {
			_value = 1f;
		}
		TapGaugeBarRect.localScale = new Vector3 (_value, TapGaugeBarRect.localScale.y, TapGaugeBarRect.localScale.z);
	}

	public void GoLerp () {
		StartCoroutine (GoLerpCoroutine (movingTime));
	}

	IEnumerator GoLerpCoroutine (float _time) {
		triggered = true;
		camera.transform.FindChild ("Main Camera").GetComponent<Animator> ().SetBool ("Skill", true);
		for (float t = 0; t <= 1 * _time; t += Time.deltaTime) {
			this.transform.position = Vector3.Lerp (cameraOriPos, cameraToPos, (Mathf.Sin (Mathf.PI * (t / _time - 0.5f)) + 1) * 0.5f);
			yield return 0;
		}
		this.transform.position = cameraToPos;
	}

	public void BackLerp () {
		StartCoroutine (BackLerpCoroutine (movingTime));
	}

	IEnumerator BackLerpCoroutine (float _time) {
		triggered = false;
		camera.transform.FindChild ("Main Camera").GetComponent<Animator> ().SetBool ("Skill", false);
		for (float t = 0; t <= 1 * _time; t += Time.deltaTime) {
			this.transform.position = Vector3.Lerp (cameraToPos, cameraOriPos, (Mathf.Sin (Mathf.PI * (t / _time - 0.5f)) + 1) * 0.5f);
			yield return 0;
		}
		this.transform.position = cameraOriPos;
	}

	public void Follow (float _time) {
		StartCoroutine (FollowCoroutine (_time));
	}

	IEnumerator FollowCoroutine (float _time) {
		for (float i = 0; i < _time; i += Time.deltaTime) {
			this.transform.position = followingObject.transform.position;
			yield return 0;
		}
		cameraToPos = this.transform.position;
	}
}
