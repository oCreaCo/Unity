using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

	public GameObject jewel1;
	public GameObject jewel2;
	public GameObject jewel3;
	public GameObject nextButton;
	public GameObject stageReward;
	public bool win;
	public Text stageText;
	public Text winOrLoseText;
	public Text retryText;
	public Text scoreText;
	public Text backText;

	void Awake () {
		jewel1.SetActive (false);
		jewel2.SetActive (false);
		jewel3.SetActive (false);
		if (GameMaster.gameMaster.worldNum == 8 && GameMaster.gameMaster.stageNum == 14) {
			nextButton.SetActive (false);
		}
		if (PlayerPrefs.GetInt ("Language") == 0) {
			retryText.text = "Retry";
			backText.text = "Back";
		} else if (PlayerPrefs.GetInt ("Language") == 1) {
			retryText.text = "다시";
			backText.text = "뒤로";
		}
	}

	public void WinOrLose (bool wl) {
		if (GameMaster.gameMaster.worldNum == 0) {
			stageText.text = "Infinite";
		} else if (GameMaster.gameMaster.worldNum != 0) {
			stageText.text = PlayerPrefs.GetInt ("worldNum").ToString () + " - " + (PlayerPrefs.GetInt ("stageNum") + 1).ToString ();
		}
		if (wl) {
			int jewel = 1;
			GameMaster.gameMaster.IsClickable (false);
			if (PlayerPrefs.GetInt ("Language") == 0) {
				winOrLoseText.text = "Win";
			} else if (PlayerPrefs.GetInt ("Language") == 1) {
				winOrLoseText.text = "승리";
			}
			win = true;
			PlayerPrefs.SetInt ("world" + GameMaster.gameMaster.worldNum + "Clear" + GameMaster.gameMaster.stageNum, 2);
			if (PlayerPrefs.GetInt ("world" + GameMaster.gameMaster.nextWorldNum + "Clear" + GameMaster.gameMaster.nextStageNum) == 0) {
				PlayerPrefs.SetInt ("world" + GameMaster.gameMaster.nextWorldNum + "Clear" + GameMaster.gameMaster.nextStageNum, 1);
			}
			if ((float)(Castle.castle.GetComponent<Barricade> ().curHealth) > (float)(Castle.castle.GetComponent<Barricade> ().oriHealth) * 0.66f) {
				jewel = 3;
			} else if ((float)(Castle.castle.GetComponent<Barricade> ().curHealth) > (float)(Castle.castle.GetComponent<Barricade> ().oriHealth) * 0.33f) {
				jewel = 2;
			}
			int temp = PlayerPrefs.GetInt ("World" + GameMaster.gameMaster.worldNum + "Stage" + GameMaster.gameMaster.stageNum + "Jewels");
			if (jewel > temp) {
				PlayerPrefs.SetInt ("World" + GameMaster.gameMaster.worldNum + "Stage" + GameMaster.gameMaster.stageNum + "Jewels", jewel);
			}
			if (jewel == 3 && PlayerPrefs.GetInt("World" + GameMaster.gameMaster.worldNum + "Stage" + GameMaster.gameMaster.stageNum + "Reward") == 0) {
				PlayerPrefs.SetInt ("World" + GameMaster.gameMaster.worldNum + "Stage" + GameMaster.gameMaster.stageNum + "Reward", 1);
				stageReward.SetActive (true);
				GemGoldScript.gemGoldScript.PlusGem (5);
			}
			if (GameMaster.gameMaster.stageNum == 14) {
				nextButton.SetActive (false);
			}
			GemGoldScript.gemGoldScript.PlusGold(GameMaster.gameMaster.stageGold * 5);
			GameMaster.gameMaster.stageGold = 0;
			StartCoroutine (ActivateJewel (jewel));
		} else if (!wl) {
			GameMaster.gameMaster.IsClickable (false);
			nextButton.SetActive (false);
			if (GameMaster.gameMaster.worldNum == 0) {
				if (PlayerPrefs.GetInt ("Language") == 0) {
					winOrLoseText.text = "Finish";
				} else if (PlayerPrefs.GetInt ("Language") == 1) {
					winOrLoseText.text = "종료";
				}
				scoreText.gameObject.SetActive (true);
				scoreText.text = "Score\n" + GameMaster.gameMaster.curScore;
				win = true;
			} else if (GameMaster.gameMaster.worldNum != 0) {
				if (PlayerPrefs.GetInt ("Language") == 0) {
					winOrLoseText.text = "Lose";
				} else if (PlayerPrefs.GetInt ("Language") == 1) {
					winOrLoseText.text = "패배";
				}
				if (AudioManager.instance != null) {
					AudioManager.instance.PlaySound ("Lose");
				}
				win = false;
			}
		}
	}

	IEnumerator ActivateJewel (int _jewel) {
		yield return new WaitForSeconds (0.5f);

		jewel1.SetActive (true);
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Win");
		}

		if (_jewel >= 2) {
			yield return new WaitForSeconds (0.5f);
			jewel2.SetActive (true);
			if (AudioManager.instance != null) {
				AudioManager.instance.PlaySound ("Win");
			}
		}

		if (_jewel >= 3) {
			yield return new WaitForSeconds (0.5f);
			jewel3.SetActive (true);
			if (AudioManager.instance != null) {
				AudioManager.instance.PlaySound ("Win");
			}
		}
	}

	public void Retry ()
	{
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		GameMaster.gameMaster.isUI = false;
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

	public void Back ()
	{
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
			AudioManager.instance.playingStageBGMusic = false;
			AudioManager.instance.transform.FindChild ("Sound_" + (GameMaster.gameMaster.worldNum + 3) + "_World" + GameMaster.gameMaster.worldNum + "BGM").GetComponent<AudioSource> ().Stop ();
		}
		GameMaster.gameMaster.isUI = false;
		if (GameMaster.gameMaster.worldNum == 0) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 3);
		}
		if (GameMaster.gameMaster.worldNum != 0) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 2);
		}
	}
	public void Next() {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		GameMaster.gameMaster.isUI = false;
		if (GameMaster.gameMaster.stageNum != 14) {
			PlayerPrefs.SetInt ("stageNum", GameMaster.gameMaster.stageNum + 1);
		}
//		else if (GameMaster.gameMaster.stageNum == 14) {
//			PlayerPrefs.SetInt ("worldNum", GameMaster.gameMaster.worldNum + 1);
//			PlayerPrefs.SetInt ("stageNum", 0);
//		}
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}
}
