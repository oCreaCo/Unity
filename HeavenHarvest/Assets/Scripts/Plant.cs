using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour {

	public Animator anim;

	public int width;
	public int s, e;

	public int score;
	public int fever;
	public float healthPlus;
	[SerializeField] private Text scoreText;
	[SerializeField] private Text stemRemainingText;

	[SerializeField] private int waterCount;
	[SerializeField] private float waterTime;
	private float t;
	[SerializeField] private int waterAmountClass;
	[SerializeField] private float waterAmount;
	[SerializeField] private float harvestMin;
	[SerializeField] private float harvestMax;
	[SerializeField] private float harvestGreatMin;
	[SerializeField] private Image waterAmountImage;
	[SerializeField] private Image waterTimer;

	[SerializeField] private Transform area1, area2;
	[SerializeField] private Image coverImage;
	[SerializeField] private Sprite[] coverImages;

	[SerializeField] private bool timerStop;
	[SerializeField] private bool watering;
	[SerializeField] private bool withered;
	[SerializeField] private bool watered;
	[SerializeField] private bool dead;
	private bool started = false;
	[SerializeField] private bool stem;

	[SerializeField] private GameObject crop;
	[SerializeField] private GameObject timer;
	[SerializeField] private GameObject waterGauge;
	private bool triggered;

	[SerializeField] private GameObject witherDryParticle;
	[SerializeField] private GameObject witherWetParticle;
	[SerializeField] private GameObject harvestParticle;
	[SerializeField] private GameObject waterGaugeParticle;
	[SerializeField] private GameObject waterDropParticle;
	[SerializeField] private ParticleSystem countParticle;
	[SerializeField] private ParticleSystem remainParticle;

	void SetRemainingText (int _waterCount) {
		stemRemainingText.text = _waterCount.ToString ();
		switch (_waterCount) {
		case 1:
			stemRemainingText.color = new Color32 (231, 34, 61, 255);
			break;
		case 2:
			stemRemainingText.color = new Color32 (251, 227, 30, 255);
			break;
		case 3:
			stemRemainingText.color = new Color32 (126, 242, 26, 255);
			break;
		}
	}

	void Start () {
		if (stem) {
			PlantSpawner.plantSpawner.stems++;
			SetRemainingText (waterCount);
		}
		scoreText.text = score.ToString ();
		switch (waterAmountClass) {
		case 0:
			area1.gameObject.SetActive (false);
			area2.gameObject.SetActive (false);
			harvestMin = 0;
			harvestMax = 0;
			harvestGreatMin = 0;
			break;
		case 1:
			area1.rotation = Quaternion.Euler (0, 0, -90);
			area2.rotation = Quaternion.Euler (0, 0, -90);
			harvestMin = 0;
			harvestMax = 0.25f;
			harvestGreatMin = 0.125f;
			break;
		case 2:
			area1.rotation = Quaternion.Euler (0, 0, -180);
			area2.rotation = Quaternion.Euler (0, 0, -180);
			harvestMin = 0.25f;
			harvestMax = 0.5f;
			harvestGreatMin = 0.275f;
			break;
		case 3:
			area1.rotation = Quaternion.Euler (0, 0, -270);
			area2.rotation = Quaternion.Euler (0, 0, -270);
			harvestMin = 0.5f;
			harvestMax = 0.75f;
			harvestGreatMin = 0.575f;
			break;
		case 4:
			area1.rotation = Quaternion.Euler (0, 0, 0);
			area2.rotation = Quaternion.Euler (0, 0, 0);
			harvestMin = 0.75f;
			harvestMax = 1.2f;
			harvestGreatMin = 0.775f;
			break;
		}
		coverImage.sprite = coverImages [waterAmountClass];
		anim.enabled = true;
		waterTime *= Mathf.Pow(0.85f, ThrowPoint.throwPoint.plantTimeSubTime);
		StartCoroutine (TimerCoroutine ());
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (!dead && started && other.tag == "Rain") {
			if (waterAmount < 1) {
				waterAmount += ThrowPoint.throwPoint.amount;
				waterAmountImage.fillAmount = waterAmount;
				anim.SetTrigger ("Bump");
				if (waterAmount > harvestMax) {
					dead = true;
					anim.SetBool ("WetDead", true);
					anim.SetTrigger ("Wither");
					waterGaugeParticle.GetComponent<ParticleSystem> ().Stop ();
					ThrowPoint.throwPoint.anim.SetTrigger ("Whine");
					for (int i = s; i <= e; i++) {
						PlantSpawner.plantSpawner.gridBools [i] = false;
					}
					PlantSpawner.plantSpawner.crops--;
					if (stem) {
						PlantSpawner.plantSpawner.stems--;
					}
					AudioManager.audioManager.transform.Find ("Sound_2_WetFail").GetComponent<AudioSource> ().Play ();
				} else {
					timerStop = true;
					if (!watered) {
						watered = true;
						withered = false;
						anim.SetTrigger ("Change");
					}
					if (!other.GetComponent<Ball> ().last) {
						watering = true;
						anim.SetBool ("WaterGauge", true);
						if (other.GetComponent<Ball> ().first) {
							waterGaugeParticle.GetComponent<ParticleSystem> ().Play ();
						}
					} else if (other.GetComponent<Ball> ().last) {
						waterGaugeParticle.GetComponent<ParticleSystem> ().Stop ();
						waterDropParticle.GetComponent<ParticleSystem> ().Play ();
						watering = false;
						if (waterAmount >= harvestMin && waterAmount <= harvestMax) {
							waterDropParticle.GetComponent<ParticleSystem> ().Stop ();
							anim.SetBool ("WaterGauge", false);
							AudioManager.audioManager.transform.Find ("Sound_1_PlantSuccess").GetComponent<AudioSource> ().Play ();
							waterCount--;
//							if (waterAmount >= harvestGreatMin) {
//								ThrowPoint.throwPoint.AddFever (fever);
//							}
							if (waterCount == 0) {
								dead = true;
								for (int i = s; i <= e; i++) {
									PlantSpawner.plantSpawner.gridBools [i] = false;
								}
								PlantSpawner.plantSpawner.crops--;
								if (stem) {
									PlantSpawner.plantSpawner.stems--;
								}
								ThrowPoint.throwPoint.anim.SetTrigger ("Smile");
								ThrowPoint.throwPoint.AddHealth (healthPlus);
								anim.SetTrigger ("Harvest");
							} else {
								waterAmount = 0;
								remainParticle.Play ();
								SetRemainingText (waterCount);
								t = 1 * waterTime;
								timerStop = false;
								anim.SetTrigger ("Count" + waterCount);
							}
						} else {
							StartCoroutine (WaterCoroutine ());
						}
					}
				}
			} else {
				dead = true;
				for (int i = s; i <= e; i++) {
					PlantSpawner.plantSpawner.gridBools [i] = false;
				}
				PlantSpawner.plantSpawner.crops--;
				if (stem) {
					PlantSpawner.plantSpawner.stems--;
				}
				ThrowPoint.throwPoint.anim.SetTrigger ("Smile");
				ThrowPoint.throwPoint.AddHealth (healthPlus);
				anim.SetTrigger ("Harvest");
				AudioManager.audioManager.transform.Find ("Sound_1_PlantSuccess").GetComponent<AudioSource> ().Play ();
			}
		}
	}

	public void AddScore () {
		ThrowPoint.throwPoint.AddScore (score);
	}

	public void SpriteChanged () {
		watered = true;
	}

	public void CountParticlePlay () {
		countParticle.Play ();
	}

	IEnumerator TimerCoroutine () {
		started = true;
		t = 1 * waterTime;
		while (t >= 0) {
			if (!timerStop) {
				t -= Time.deltaTime;
				waterTimer.fillAmount = t / waterTime;
				if (t <= waterTime * 0.33f && !withered) {
					withered = true;
					watered = false;
					anim.SetTrigger ("Wither");
				}
			}
			yield return 0;
		}
		dead = true;
		for (int i = s; i <= e; i++) {
			PlantSpawner.plantSpawner.gridBools [i] = false;
		}
		PlantSpawner.plantSpawner.crops--;
		if (stem) {
			PlantSpawner.plantSpawner.stems--;
		}
		if (!watered) {
			ThrowPoint.throwPoint.anim.SetTrigger ("Whine");
			anim.SetTrigger ("DryDead");
			AudioManager.audioManager.transform.Find ("Sound_3_DryFail").GetComponent<AudioSource> ().Play ();
		} else {
			ThrowPoint.throwPoint.anim.SetTrigger ("Whine");
			anim.SetBool ("WetDead", true);
			AudioManager.audioManager.transform.Find ("Sound_2_WetFail").GetComponent<AudioSource> ().Play ();
		}
	}

	public IEnumerator WaterCoroutine () {
		while (waterAmount >= 0) {
			if (!watering) {
				waterAmount -= Time.deltaTime / waterTime;
				waterAmountImage.fillAmount = waterAmount;
			}
			yield return 0;
			if (dead) {
				break;
			}
		}
		timerStop = false;
		waterDropParticle.GetComponent<ParticleSystem> ().Stop ();
		anim.SetBool ("WaterGauge", false);
	}

	public void Trigger () {
		if (!triggered) {
			triggered = true;
			crop.SetActive (false);
			timer.SetActive (true);
		} else {
			triggered = false;
			crop.SetActive (true);
			timer.SetActive (false);
		}
	}

	public void WitherDryDie () {
		witherDryParticle.SetActive (true);
		Destroy (this.gameObject, 3f);
	}

	public void WitherWetDie () {
		witherWetParticle.SetActive (true);
		Destroy (this.gameObject, 2f);
	}

	public void Harvest () {
		harvestParticle.SetActive (true);
		Destroy (this.gameObject, 2f);
	}
}
