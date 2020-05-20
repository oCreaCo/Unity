using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour {
	
	public SpriteRenderer sprite;
	[SerializeField] private bool triggered;
	private Transform myTransform;

	[SerializeField] private Sprite unTriggeredSprite, triggeredSprite;

	[SerializeField] private GameObject pathCollider2DObject;
	[SerializeField] private Transform upSpawnPoint;
	[SerializeField] private Transform downSpawnPoint;
	[SerializeField] private Transform rightSpawnPoint;
	[SerializeField] private Transform leftSpawnPoint;
	private RaycastHit2D rayCastHit2D;

	private GameObject upCollider;
	private GameObject downCollider;
	private GameObject rightCollider;
	private GameObject leftCollider;

	public enum TeleporterType
	{
		BLOCKEXIT,
		STAGEEXIT,
	}

	public TeleporterType teleporterType;

	void Start () {
		myTransform = this.transform;
		if (GameMaster.gameMaster.layerCount > 0) {
			teleporterType = TeleporterType.BLOCKEXIT;
		} else {
			teleporterType = TeleporterType.STAGEEXIT;
		}
		if (teleporterType == TeleporterType.STAGEEXIT) {
			upCollider = Instantiate (pathCollider2DObject, upSpawnPoint.position, upSpawnPoint.rotation, myTransform) as GameObject;
			downCollider = Instantiate (pathCollider2DObject, downSpawnPoint.position, downSpawnPoint.rotation, myTransform) as GameObject;
			rightCollider = Instantiate (pathCollider2DObject, rightSpawnPoint.position, rightSpawnPoint.rotation, myTransform) as GameObject;
			leftCollider = Instantiate (pathCollider2DObject, leftSpawnPoint.position, leftSpawnPoint.rotation, myTransform) as GameObject;
			upCollider.GetComponent<PathCollider2D> ().teleporter = this;
			downCollider.GetComponent<PathCollider2D> ().teleporter = this;
			rightCollider.GetComponent<PathCollider2D> ().teleporter = this;
			leftCollider.GetComponent<PathCollider2D> ().teleporter = this;
		} else {
			sprite.sprite = triggeredSprite;
			triggered = true;
		}
	}

	public void Trigger (bool _bool) {
		if ((_bool && !triggered) || (!_bool && triggered)) {
			if (_bool) {
				sprite.sprite = triggeredSprite;
				triggered = true;
			} else {
				sprite.sprite = triggeredSprite;
				triggered = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (triggered && GameMaster.gameMaster.player.GetComponent<Player> ().movable && other.tag == "Player") {
			if (teleporterType == TeleporterType.STAGEEXIT) {
				int i = PlayerPrefs.GetInt ("Index");
				i++;
				PlayerPrefs.SetInt ("Index", i);
				GameMaster.gameMaster.player.GetComponent<Player> ().movable = false;
				StartCoroutine (CameraLerpCoroutine (GameMaster.gameMaster.camera.position, new Vector3 (0, 0, -10), 1.5f));
				Debug.LogError ("Win");
			} else {
				if (GameMaster.gameMaster.layerCount > 0) {
					GameMaster.gameMaster.PuzzleOut ();
				}
			}
		}
	}

	IEnumerator CameraLerpCoroutine (Vector3 _startPos, Vector3 _endPos, float _changingTime) {
		yield return new WaitForSeconds (1f);
		Debug.LogError (_endPos);
		for (float t = 0; t < 1 * _changingTime; t += Time.deltaTime) {
			GameMaster.gameMaster.camera.position = Vector3.Lerp (_startPos, _endPos, t / _changingTime);
			yield return 0;
		}
		GameMaster.gameMaster.camera.position = _endPos;
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene (0);
	}
}
