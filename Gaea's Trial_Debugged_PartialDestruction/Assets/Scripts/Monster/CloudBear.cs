using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CloudBear : MonoBehaviour, IEventSystemHandler {

	private Monster_Basic monsterBasic;
	private PartialDestruction partialDestruction;

	[SerializeField] private Transform spawnPoints;
	[SerializeField] private GameObject missile;
	[SerializeField] private float missileFireRate;

	[SerializeField] private GameObject frosty;
	public float spawnRateMin;
	public float spawnRateMax;
	public float spawnRateTemp;
	public float offSet;
	public float attackTime;

	[SerializeField] private Transform cloudPoint;
	[SerializeField] private Button.ButtonClickedEvent failedFunction;
	private bool floated;
	public bool activated = false;

	[SerializeField] private float yGrid;
	[SerializeField] private float moveSpeed;
	[SerializeField] private Vector3 ground;
	[SerializeField] private Vector3 sky;
	public bool clouded = false;
	public GameObject cloud;

	private Animator anim;

	void Awake () {
		monsterBasic = GetComponent<Monster_Basic> ();
		partialDestruction = GetComponent<PartialDestruction> ();
		CameraLerpPosition.cameraLerpPosition.failedFunction = failedFunction;
		spawnRateTemp = Random.Range(spawnRateMin, spawnRateMax);
		anim = GetComponent<Animator> ();
		attackTime = Time.time + offSet + spawnRateTemp;
		ground = new Vector3 (2.43f, 2.13f, 0f);
		sky = new Vector3 (2.43f, 3.46f, 0f);
		StartCoroutine (Shoot ());
	}

	IEnumerator Shoot () {
		while (true) {
			yield return new WaitForSeconds (missileFireRate);
			GameObject iceMissile = Instantiate (missile, spawnPoints.position, spawnPoints.rotation) as GameObject;
		}
	}

	void Update () {
		if ((GetComponent<Monster_Basic> ().curHealth / GetComponent<Monster_Basic> ().oriHealth) * 100 <= 50) {
			if (spawnRateTemp != 0 && Time.time >= attackTime && !GetComponent<Monster_Basic> ().stoppedBool) {
				Instantiate (frosty);
				spawnRateTemp = Random.Range (spawnRateMin, spawnRateMax); 
				attackTime = Time.time + spawnRateTemp;
			}
		}
	}

	public void StackModifiable () {
		GameMaster.gameMaster.stackModifiable = true;
	}

	public void HurtFunction () {
		if (((monsterBasic.curHealth / monsterBasic.oriHealth) * 100 <= 30f && !activated)) {
			cloud.SetActive (true);
			partialDestruction.ActivateButton (ref activated, 5f);
		}
	}

	public void Float () {
		StartCoroutine (FloatCoroutine (5f, 1f, false));
	}

	public void FailedFunction () {
		StartCoroutine (FloatCoroutine (0f, 1f, true));
	}

//	public void Fall () {
//		StartCoroutine (FallCoroutine (0.7f));
//	}

	public IEnumerator FloatCoroutine (float waitingTime, float _time, bool b) {
		yield return new WaitForSeconds (waitingTime);
		Debug.LogError (activated + " " + b);
		if (!partialDestruction.activated || b) {
			for (float t = 0; t <= 1 * _time; t += Time.deltaTime) {
				transform.position = Vector3.Lerp (ground, sky, t / _time);
				yield return 0;
			}
			transform.position = sky;
		}
	}

//	public IEnumerator FallCoroutine (float _time) {
//		cloud.SetActive (false);
//		for (float t = 0; t <= 1 * _time; t += Time.deltaTime) {
//			transform.position = Vector3.Lerp (sky, ground, t / _time);
//			yield return 0;
//		}
//		transform.position = ground;
//	}

	public void PartialDestructionSuccessFunction() {
		cloud.SetActive (false);
	}

//	public void FallAndFloat () {
//		StartCoroutine (FallAndFloatCoroutine ());
//	}
//
//	IEnumerator FallAndFloatCoroutine () {
//		Fall ();
//		yield return new WaitForSeconds (10f);
//		Float ();
//		GetComponent<PartialDestruction> ().healthCount = 1;
//		GetComponent<PartialDestruction> ().destroyedCount = 0;
//		GetComponent<PartialDestruction> ().destroyPart [GetComponent<PartialDestruction> ().destroyedCount].button.SetActive (true);
//		ButtonActive ();
//	}
}
