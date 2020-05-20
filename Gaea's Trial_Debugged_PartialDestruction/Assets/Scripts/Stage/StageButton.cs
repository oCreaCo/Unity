using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour {

	public int worldNum;
	public int stageNum;
	private Button stageButton;
	public GameObject[] stars;
	public Text stageNumText;

	void Awake () {
		stageButton = this.GetComponent<Button> ();
	}

	public void GoToStage () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		PlayerPrefs.SetInt ("worldNum", worldNum);
		PlayerPrefs.SetInt ("stageNum", stageNum);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 2);
	}

	public void SetButton (int _unlocked) {
		if (_unlocked == 0) {
			stageButton.interactable = false;
		} else {

		}
	}
}
