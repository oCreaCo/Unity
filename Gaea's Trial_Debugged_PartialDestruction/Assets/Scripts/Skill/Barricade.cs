using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Barricade : MonoBehaviour {

	public int curHealth;
	public int oriHealth = 20;
	public int addDamage;
	public bool dead = false;
	public bool powerOverwhelming = false;
	private Animator anim;
	private Vector3 hell = new Vector3 (100f, 200f, 300f);

	void Awake () {
		anim = this.transform.GetComponent<Animator> ();
		if (GetComponent<Castle> () != null) {
			oriHealth = 100 + PlayerPrefs.GetInt ("GridCastleLevel") * 15;
		}
		curHealth = oriHealth;
	}

	void Start () {
		if (GameMaster.gameMaster != null) {
			GameMaster.gameMaster.barricades.Add (this);
		}
	}

	public void GetHurt (int Dmg, float cameraShakeMultiplier, float camerShakeLength) {
		if (!GameMaster.gameMaster.stopped) {
			if (curHealth > 0 && !powerOverwhelming) {
				curHealth -= (Dmg + addDamage);
				if (anim != null) {
					anim.SetTrigger ("Attacked");
				}
			}
			if (curHealth <= 0) {
				dead = true;
				GameMaster.gameMaster.barricades.Remove (this);
				if (GetComponent<MoveScript> () != null) {
					GetComponent<MoveScript> ().moveSpeed = 0;
				}
				if (anim != null) {
					anim.SetBool ("Dead", dead);
				}
				Destroy (this.transform.parent.gameObject, 2f);
			}
			if (GetComponent<Castle> () != null) {
				if (Dmg != 0) {
//				AudioManager.instance.PlaySound ("CastleHit");

					GetComponent<Castle> ().statusIndicator.SetHealth (curHealth, oriHealth);
					GetComponent<Castle> ().castleHealthGraph.SetHealth (curHealth, oriHealth);
					CameraShake.cameraShake.Shake (GetComponent<Castle> ().shakeAmount * cameraShakeMultiplier, camerShakeLength);

					if (curHealth > 50 && curHealth <= 75) {
						this.transform.FindChild ("Castle").GetComponent<Image> ().sprite = GetComponent<Castle> ().sprite [1];
					} else if (curHealth > 25 && curHealth <= 50) {
						this.transform.FindChild ("Castle").GetComponent<Image> ().sprite = GetComponent<Castle> ().sprite [2];
					} else if (curHealth > 0 && curHealth <= 25) {
						this.transform.FindChild ("Castle").GetComponent<Image> ().sprite = GetComponent<Castle> ().sprite [3];
					} else if (curHealth <= 0 && GameMaster.gameMaster.allFinished == false) {
						if (GameMaster.gameMaster.worldNum == 0) {
							GameMaster.gameMaster.allFinished = true;
							GemGoldScript.gemGoldScript.PlusGold (GameMaster.gameMaster.stageGold * 5);
							GameMaster.gameMaster.WinOrLose (true);
						} else if (GameMaster.gameMaster.worldNum != 0) {
							GameMaster.gameMaster.allFinished = true;
							GameMaster.gameMaster.WinOrLose (false);
						}
						GetComponent<Castle> ().gameOverUI.GetComponent<GameOverUI> ().WinOrLose (false);
						GetComponent<Castle> ().gameOverUI.SetActive (true);
					}
				}
			}
		}
	}
}
