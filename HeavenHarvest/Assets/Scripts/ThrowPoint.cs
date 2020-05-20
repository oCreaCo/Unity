using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ThrowPoint : MonoBehaviour {

	private GameMaster gameMaster;

	public static ThrowPoint throwPoint;
	[SerializeField] private Camera mainCam;

	[SerializeField] private int score;
	[SerializeField] private int highScore;
	public int phase;
	[SerializeField] private int phaseUp;
	[SerializeField] private int phaseUpTemp;
	[SerializeField] private int[] phaseUpAddAmount;
	[SerializeField] private int green;
	[SerializeField] private int greenTemp;
	[SerializeField] private int greenAddAmount;
	[SerializeField] private float health;
	public int plantTimeSubTime;
	[SerializeField] private float lerpSpeed;
	public float amount;
	[SerializeField] private float healthMinus;
	[SerializeField] private Animator phaseUpAnim;
	[SerializeField] private Text phaseUpText, whatIsUpText;
	[SerializeField] private Text scoreText;
	[SerializeField] private Text highScoreText;
//	[SerializeField] private Text nextPhaseText;
	[SerializeField] private Image heatBar;
	[SerializeField] private Transform waterParticle;
	[SerializeField] private Transform waterBurstParticle;
	[SerializeField] private GameObject tic;
	private Vector3 waterParticleStartPos = new Vector3 (-8.36f, -4.17f, 0);
	private Vector3 waterParticleEndPos = new Vector3 (0.65f, -4.17f, 0);
	public Animator anim;
	private Coroutine healthCoroutine;

	public Transform realTarget;
	public Transform target;
	public Transform throwPointTransform;
	public GameObject ball;
	public float timeTillHit;

	public bool lerped;
	public bool peeing;
	public bool paused = false;

	[SerializeField] private PositionLerp[] lerps;

	public Transform remainingGaugeTransform;
	public Image remainingGauge;
	public ParticleSystem remainingGaugeParticle;
	[SerializeField] private Transform upPos;
	[SerializeField] private Transform downPos;

	[SerializeField] private Image feverImage;
	[SerializeField] private int feverMax;
	[SerializeField] private int fever;
	[SerializeField] private float feverTime;

	[SerializeField] private GameObject pauseUI;
	[SerializeField] private GameObject gameOverUI;
	[SerializeField] private GameObject tutUI;
	[SerializeField] private GameObject tutUIObj;
	[SerializeField] private Text tutEnabledText;
	[SerializeField] private Image tutCheckImage;
	[SerializeField] private Image tutCheckOutLineImage;
	[SerializeField] private Text gameOverScoreText, gameOverHighScoreText;

	[System.Serializable]
	public struct MountainStruct
	{
		public GameObject leaves;
		public bool spawned;
	}

	[System.Serializable]
	public struct Mountain
	{
		public MountainStruct[] mountainStructs;
		public bool allSpawned;
	}

	public Mountain[] mountain;

	public void ThrowPointStart () {
		ThrowPoint.throwPoint = this;
		gameMaster = GameMaster.gameMaster;
		highScore = PlayerPrefs.GetInt ("HighScore");
		highScoreText.text = highScore.ToString ();
		StartOnce ();
	}

	void StartOnce () {
		if (PlayerPrefs.GetInt ("Tutorial") == 0) {
			if (tutUIObj != null) {
				Destroy (tutUIObj);
			}
			tutUIObj = Instantiate (tutUI) as GameObject;
		}
		lerped = false;
		peeing = false;
		GameObject[] plants = GameObject.FindGameObjectsWithTag ("Plant");
		for (int i = 0; i < plants.Length; i++) {
			Destroy (plants [i]);
		}
		PlantSpawner.plantSpawner.crops = 0;
		PlantSpawner.plantSpawner.stems = 0;
		score = 0;
		phaseUpTemp = phaseUp;
		greenTemp = green;
		for (int i = 0; i < mountain.Length; i++) {
			for (int j = 0; j < mountain [i].mountainStructs.Length; j++) {
				mountain [i].mountainStructs [j].leaves.SetActive (false);
				mountain [i].mountainStructs [j].spawned = false;
			}
		}
		amount = 0.002f;
		lerpSpeed = 3;
		lerps [0].movingTime = lerpSpeed;
		lerps [1].movingTime = lerpSpeed;
		healthMinus = 0.00025f;
		plantTimeSubTime = 0;
		health = 1.0f;
		phase = 0;
		fever = 0;
		feverImage.fillAmount = fever / feverMax;
//		nextPhaseText.text = "Next phase : " + phaseUp.ToString ();
		scoreText.text = score.ToString ();
		PlantSpawner.plantSpawner.spawnTime = 0.1f;
		PlantSpawner.plantSpawner.started = true;
		healthCoroutine = StartCoroutine (HealthCoroutine ());
	}

	IEnumerator PeeingCoroutine () {
		remainingGaugeParticle.Play ();
		Throw ().GetComponent<Ball> ().first = true;
		while (peeing && remainingGauge.fillAmount < 1) {
			Throw ();
			yield return 0;
		}
		yield return 0;
		Throw ().GetComponent<Ball> ().last = true;
		remainingGaugeTransform.GetComponent<Animator>().SetTrigger("End");
		remainingGaugeParticle.Stop ();
	}

	private GameObject Throw () {
		float xDistance;
		xDistance = realTarget.position.x -	throwPointTransform.position.x;

		float yDistance;
		yDistance = realTarget.position.y - throwPointTransform.position.y;

		float throwAngle;

		throwAngle = Mathf.Atan ((yDistance + (4.905f * timeTillHit * timeTillHit)) / xDistance);

		float totalVelo = xDistance / Mathf.Cos (throwAngle);

		float xVelo, yVelo;
		xVelo = totalVelo * Mathf.Cos (throwAngle);
		yVelo = totalVelo * Mathf.Sin (throwAngle);

		GameObject ballObject = Instantiate (ball, throwPointTransform.position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject;
		Rigidbody2D rigid = ballObject.GetComponent<Rigidbody2D> ();

		rigid.velocity = new Vector2 (xVelo, yVelo);

		Destroy (ballObject, 3f);

		remainingGauge.fillAmount += amount;
		remainingGaugeParticle.transform.position = Vector3.Lerp (upPos.position, downPos.position, remainingGauge.fillAmount);

		return ballObject;
	}

	void OnMouseDown () {
		if (!lerped) {
			target.Find("TargetTop").GetComponent<SpriteRenderer>().color =  new Color32 (255, 255, 255, 255);
			target.Find("TargetBottom").GetComponent<SpriteRenderer>().color =  new Color32 (255, 255, 255, 255);
			for (int i = 0; i < lerps [1].GetComponent<PositionLerp> ().path.Length; i++) {
				lerps [1].GetComponent<PositionLerp> ().path [i].GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
			}
			for (int i = 0; i < lerps.Length; i++) {
				lerps [i].stopped = false;
				StartCoroutine (lerps [i].LerpCoroutine ());
			}
		} else if (lerped) {
			target.Find("TargetTop").GetComponent<SpriteRenderer>().color =  new Color32 (255, 255, 255, 0);
			target.Find("TargetBottom").GetComponent<SpriteRenderer>().color =  new Color32 (255, 255, 255, 0);
			for (int i = 0; i < lerps [1].GetComponent<PositionLerp> ().path.Length; i++) {
				lerps [1].GetComponent<PositionLerp> ().path [i].GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 0);
			}
			remainingGauge.fillAmount = 0;
			peeing = true;
			AudioManager.audioManager.transform.Find ("Sound_8_Sprinkler").GetComponent<AudioSource> ().Play ();
			remainingGaugeTransform.GetComponent<Animator>().SetTrigger("Awake");
			StartCoroutine (PeeingCoroutine ());
			Vector3 inputPos = Input.mousePosition;
			inputPos.z = 1f;
			Vector3 worldPosTemp = mainCam.ScreenToWorldPoint (inputPos);
			Vector3 worldPos;
			if (worldPosTemp.y > 1.7f) {
				worldPos = new Vector3 (worldPosTemp.x, worldPosTemp.y - 1.7f, worldPosTemp.z);
			} else {
				worldPos = new Vector3 (worldPosTemp.x, worldPosTemp.y + 1.7f, worldPosTemp.z);
			}
			remainingGaugeTransform.position = worldPos;
		}
	}

	void OnMouseUp () {
		if (!lerped) {
			if (!lerps [0].failed) {
				for (int i = 0; i < lerps.Length; i++) {
					lerps [i].stopped = true;
				}
				lerped = true;
			}
		} else if (lerped) {
			lerped = false;
			if (peeing) {
				remainingGaugeTransform.GetComponent<Animator> ().SetTrigger ("End");
			}
			peeing = false;
			AudioManager.audioManager.transform.Find ("Sound_8_Sprinkler").GetComponent<AudioSource> ().Stop ();
		}
	}

	public void SetAimSpeed (float _float) {
		lerpSpeed = _float;
		lerps [0].movingTime = lerpSpeed;
		lerps [1].movingTime = lerpSpeed;
		whatIsUpText.text = "AimSpeed+";
		phaseUpText.GetComponent<Outline> ().effectColor = new Color32 (255, 0, 90, 255);
		whatIsUpText.color = new Color32 (255, 0, 90, 255);
		whatIsUpText.GetComponent<Shadow>().effectColor = new Color32 (3, 115, 195, 255);
		phaseUpAnim.SetTrigger("PhaseUp");
	}

	public void SetAmount (float _float) {
		amount = _float;
		whatIsUpText.text = "WaterAmount+";
		phaseUpText.GetComponent<Outline> ().effectColor = new Color32 (0, 202, 255, 255);
		whatIsUpText.color = new Color32 (0, 202, 255, 255);
		whatIsUpText.GetComponent<Shadow>().effectColor = new Color32 (3, 115, 195, 255);
		phaseUpAnim.SetTrigger("PhaseUp");
	}

	public void SetLifeTime (float _float) {
		healthMinus = _float;
		whatIsUpText.text = "LifeTimer-";
		phaseUpText.GetComponent<Outline> ().effectColor = new Color32 (255, 255, 0, 255);
		whatIsUpText.color = new Color32 (255, 255, 0, 255);
		whatIsUpText.GetComponent<Shadow>().effectColor = new Color32 (152, 106, 7, 255);
		phaseUpAnim.SetTrigger("PhaseUp");
	}

	int temp;
	List<int> temp1 = new List<int> ();
	public void AddScore (int _score) {
		score += _score;
		if (score >= highScore) {
			highScore = score;
			highScoreText.text = highScore.ToString ();
		}
		scoreText.text = score.ToString ();
//		if (score >= greenTemp) {
//			while (score >= greenTemp) {
//				greenTemp += greenAddAmount;
//				if (!mountain [3].allSpawned) {
//					temp = 3;
//				} else if (!mountain [0].allSpawned) {
//					temp = 0;
//				} else if (!mountain [2].allSpawned) {
//					temp = 2;
//				} else if (!mountain [1].allSpawned) {
//					temp = 1;
//				} else {
//					temp = -1;
//				}
//				if (temp != -1) {
//					for (int i = 0; i < mountain [temp].mountainStructs.Length; i++) {
//						if (!mountain [temp].mountainStructs [i].spawned) {
//							temp1.Add (i);
//						}
//					}
//					int r1 = Random.Range (0, temp1.Count);
//					mountain [temp].mountainStructs [temp1 [r1]].leaves.SetActive (true);
//					mountain [temp].mountainStructs [temp1 [r1]].spawned = true;
//					if (temp1.Count == 1) {
//						mountain [temp].allSpawned = true;
//					}
//					temp1.Clear ();
//				}
//			}
//		}
		if (score >= phaseUpTemp) {
			if (phase < phaseUpAddAmount.Length) {
				phaseUpTemp += phaseUpAddAmount [phase];
			}
			phase++;
//			nextPhaseText.text = "Next phase : " + phaseUpTemp.ToString ();
			AudioManager.audioManager.transform.Find ("Sound_7_PhaseUpMelody").GetComponent<AudioSource> ().Play ();
			switch (phase) {
			case 1:
				SetAimSpeed (2.25f);
				break;
			case 2:
				SetAmount (0.0025f);
				plantTimeSubTime = 1;
				break;
			case 3:
				SetLifeTime (0.0003125f);
				break;
			case 4:
				SetAimSpeed (1.5f);
				break;
			case 5:
				SetAmount (0.0032f);
				plantTimeSubTime = 2;
				break;
			case 6:
				SetLifeTime (0.000375f);
				break;
			case 7:
				SetAimSpeed (1.0f);
				break;
			case 8:
				SetAmount (0.004f);
				plantTimeSubTime = 3;
				break;
			case 9:
				SetLifeTime (0.0004375f);
				break;
			case 10:
				SetAimSpeed (0.5f);
				break;
			case 11:
				SetAmount (0.005f);
				break;
			case 12:
				SetLifeTime (0.0005f);
				break;
			}
		}
	}

	public void AddFever (int _score) {
		fever += _score;
		if (fever >= feverMax) {
			fever = feverMax;
			StartCoroutine (FeverCoroutine (feverTime));
		}
		feverImage.fillAmount = (float)fever / (float)feverMax;
	}

	public void AddHealth (float _health) {
		health += _health;
		if (health > 1.0f) {
			health = 1.0f;
		}
		heatBar.fillAmount = health;
		waterBurstParticle.position = Vector3.Lerp (waterParticleStartPos, waterParticleEndPos, health);
		waterBurstParticle.GetComponent<ParticleSystem> ().Play ();
	}

	IEnumerator HealthCoroutine () {
		target.Find("TargetTop").GetComponent<SpriteRenderer>().color =  new Color32 (255, 255, 255, 0);
		target.Find("TargetBottom").GetComponent<SpriteRenderer>().color =  new Color32 (255, 255, 255, 0);
		for (int i = 0; i < lerps [1].GetComponent<PositionLerp> ().path.Length; i++) {
			lerps [1].GetComponent<PositionLerp> ().path [i].GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 0);
		}
		waterParticle.GetComponent<ParticleSystem> ().Play ();
		while (health > 0) {
			if (!paused) { 
				health -= healthMinus;
				heatBar.fillAmount = health;
				waterParticle.position = Vector3.Lerp (waterParticleStartPos, waterParticleEndPos, health);
				}
			yield return 0;
		}
		waterParticle.GetComponent<ParticleSystem> ().Stop ();
		PlantSpawner.plantSpawner.started = false;
		GameObject[] plants = GameObject.FindGameObjectsWithTag ("Plant");
		for (int i = 0; i < plants.Length; i++) {
			Destroy (plants [i]);
		}
		int high = PlayerPrefs.GetInt ("HighScore");
		gameOverHighScoreText.text = high.ToString ();
		GetComponent<Collider2D> ().enabled = false;
		GameMaster.gameMaster.pauseButton.SetActive (false);
		gameOverUI.SetActive (true);
		int t = 0;
		if (score <= 1000) {
			t = 10;
		} else if (score > 1000 && score <= 4000) {
			t = 20;
		} else if (score > 4000) {
			t = 100;
		}
		int tmp = 0;
		gameOverUI.GetComponent<Animator> ().SetBool ("Rise1", true);
		for (int i = 0; i <= score; i += t) {
			tmp++;
			gameOverScoreText.text = i.ToString ();
			if (i > high) {
				gameOverHighScoreText.text = i.ToString ();
			}
			if (score - i <= 1000) {
				t = 10;
			} else if (score - i > 1000 && score - i <= 4000) {
				t = 20;
			} else if (score - i > 4000) {
				t = 100;
			}
			if (tmp == 7) {
				tmp = 0;
				GameObject ticObject = Instantiate (tic) as GameObject;
				if (i >= high) {
					gameOverUI.GetComponent<Animator> ().SetBool ("Rise1", false);
					gameOverUI.GetComponent<Animator> ().SetBool ("Rise2", true);
					ticObject.GetComponent<AudioSource> ().pitch = 1.3f;
					ticObject.GetComponent<AudioSource> ().volume = 0.75f;
				}
			}
			yield return new WaitForSeconds (0.001f);
		}
		gameOverUI.GetComponent<Animator> ().SetBool ("Rise1", false);
		gameOverUI.GetComponent<Animator> ().SetBool ("Rise2", false);
		if (score > high) {
			gameMaster.HighScoreParticles ();
			AudioManager.audioManager.transform.Find ("Sound_5_HighScore").GetComponent<AudioSource> ().Play ();
			PlayerPrefs.SetInt ("HighScore", score);
		}
	}

	IEnumerator FeverCoroutine (float _time) {
		fever = feverMax;
		feverImage.color = new Color32 (230, 220, 0, 255);
		for (float t = 0; t < 1 * _time; t += Time.deltaTime) {
			feverImage.fillAmount = (_time - t) / _time;
			yield return 0;
		}
		fever = 0;
		feverImage.color = new Color32 (255, 33, 120, 255);
		feverImage.fillAmount = 0f;
	}

	public void Pause () {
		if (PlayerPrefs.GetInt ("Tutorial") == 0) {
			tutEnabledText.text = "Help Enabled";
			tutEnabledText.color = new Color32 (255, 255, 255, 255);
			tutCheckImage.color = new Color32 (255, 255, 255, 255);
			tutCheckOutLineImage.color = new Color32 (255, 255, 255, 255);
		} else {
			tutEnabledText.text = "Help Disabled";
			tutEnabledText.color = new Color32 (120, 120, 120, 255);
			tutCheckImage.color = new Color32 (255, 255, 255, 0);
			tutCheckOutLineImage.color = new Color32 (120, 120, 120, 255);
		}
		paused = true;
		pauseUI.SetActive (true);
		GetComponent<Collider2D> ().enabled = false;
		Time.timeScale = 0;
		AudioManager.audioManager.transform.Find ("Sound_6_Click").GetComponent<AudioSource> ().Play ();
	}

	public void Resume () {
		paused = false;
		Time.timeScale = 1;
		GetComponent<Collider2D> ().enabled = true;
		pauseUI.SetActive (false);
		AudioManager.audioManager.transform.Find ("Sound_6_Click").GetComponent<AudioSource> ().Play ();
	}

	public void Retry () {
		paused = false;
		Time.timeScale = 1;
		StopCoroutine (healthCoroutine);
		GetComponent<Collider2D> ().enabled = true;
		pauseUI.SetActive (false);
		gameOverUI.SetActive (false);
		GameMaster.gameMaster.pauseButton.SetActive (true);
		StartOnce ();
		AudioManager.audioManager.transform.Find ("Sound_6_Click").GetComponent<AudioSource> ().Play ();
	}

	public void Menu () {
		if (tutUIObj != null) {
			Destroy (tutUIObj);
		}
		lerped = false;
		peeing = false;
		target.Find("TargetTop").GetComponent<SpriteRenderer>().color =  new Color32 (255, 255, 255, 0);
		target.Find("TargetBottom").GetComponent<SpriteRenderer>().color =  new Color32 (255, 255, 255, 0);
		for (int i = 0; i < lerps [1].GetComponent<PositionLerp> ().path.Length; i++) {
			lerps [1].GetComponent<PositionLerp> ().path [i].GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 0);
		}
		GameObject[] plants = GameObject.FindGameObjectsWithTag ("Plant");
		for (int i = 0; i < plants.Length; i++) {
			Destroy (plants [i]);
		}
		PlantSpawner.plantSpawner.crops = 0;
		score = 0;
		health = 1.0f;
		scoreText.text = score.ToString ();
		PlantSpawner.plantSpawner.started = false;
		Time.timeScale = 1;
		pauseUI.SetActive (false);
		gameOverUI.SetActive (false);
		gameMaster.camAnim.SetBool ("Game", false);
		gameMaster.menuAnim.SetBool ("Game", false);
		for (int i = 0; i < mountain.Length; i++) {
			for (int j = 0; j < mountain [i].mountainStructs.Length; j++) {
				mountain [i].mountainStructs [j].leaves.SetActive (true);
				mountain [i].mountainStructs [j].spawned = true;
			}
		}
		AudioManager.audioManager.transform.Find ("Sound_6_Click").GetComponent<AudioSource> ().Play ();
	}

	public void TutEnableButton () {
		if (PlayerPrefs.GetInt ("Tutorial") == 1) {
			PlayerPrefs.SetInt ("Tutorial", 0);
			tutEnabledText.text = "Help Enabled";
			tutEnabledText.color = new Color32 (255, 255, 255, 255);
			tutCheckImage.color = new Color32 (255, 255, 255, 255);
			tutCheckOutLineImage.color = new Color32 (255, 255, 255, 255);
			tutUIObj = Instantiate (tutUI) as GameObject;
		} else {
			PlayerPrefs.SetInt ("Tutorial", 1);
			tutEnabledText.text = "Help Disabled";
			tutEnabledText.color = new Color32 (120, 120, 120, 255);
			tutCheckImage.color = new Color32 (255, 255, 255, 0);
			tutCheckOutLineImage.color = new Color32 (120, 120, 120, 255);
			if (tutUIObj != null) {
				Destroy (tutUIObj);
			}
		}
	}
}
