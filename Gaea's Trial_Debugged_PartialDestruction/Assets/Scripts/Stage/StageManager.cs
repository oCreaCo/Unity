using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageManager : MonoBehaviour {
	public static StageManager stageManager;

	public Transform[] buttonPos;
	public GameObject stages;
	public int world;
	public Sprite[] buttonSprite = new Sprite[3];
	public Button nextWorld;

	[System.Serializable]
	public struct StageManage
	{
		public int unLocked;
	}

	public GameObject stage;
	public StageManage[] stageManage;

	void Awake () {
		stageManager = this;

		for (int i = 0; i < stageManage.Length; i++) {
			stageManage [i].unLocked = PlayerPrefs.GetInt ("world" + world + "Clear" + i);
			GetButton (i);
		}
		if (stageManage [14].unLocked == 2) {
			if (nextWorld != null) {
				nextWorld.gameObject.SetActive (true);
			}
		}
		if (AudioManager.instance != null && !AudioManager.instance.playingBGMusic) {
			AudioManager.instance.playingBGMusic = true;
			StartCoroutine (waitForMusic ());
		}
	}

	IEnumerator waitForMusic () {
		yield return new WaitForSeconds (0.1f);

		AudioManager.instance.PlaySound ("BGMusic");
	}

	public void GetButton (int _stageNum) {
		GameObject button = Instantiate (stage, buttonPos [_stageNum].position, buttonPos [_stageNum].rotation, stages.transform) as GameObject;
		button.GetComponent<StageButton> ().worldNum = world;
		button.GetComponent<StageButton> ().stageNum = _stageNum;
		button.GetComponent<StageButton> ().stageNumText.text = (_stageNum + 1).ToString ();
		if (stageManage [_stageNum].unLocked == 0) {
			Button testButton = button.GetComponent<Button> ();
			if (button.GetComponent<Button> () != null) {
				button.GetComponent<Button> ().enabled = false;
			}
		} else if (stageManage [_stageNum].unLocked == 1) {
			button.GetComponent<Image> ().sprite = buttonSprite [1];
		} else if (stageManage [_stageNum].unLocked == 2) {
			button.GetComponent<Image> ().sprite = buttonSprite [2];
			if (PlayerPrefs.GetInt ("World" + world + "Stage" + _stageNum + "Jewels") > 0) {
				button.GetComponent<StageButton> ().stars [0].SetActive (true);
			}
			if (PlayerPrefs.GetInt ("World" + world + "Stage" + _stageNum + "Jewels") > 1) {
				button.GetComponent<StageButton> ().stars [1].SetActive (true);
			}
			if (PlayerPrefs.GetInt ("World" + world + "Stage" + _stageNum + "Jewels") > 2) {
				button.GetComponent<StageButton> ().stars [2].SetActive (true);
			}
			button.GetComponent<StageButton> ().stageNumText.color = new Color (0, 0, 0, 255);
		}
	}

	public void Back () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}
}
