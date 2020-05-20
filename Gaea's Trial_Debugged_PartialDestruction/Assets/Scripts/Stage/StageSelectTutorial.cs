using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectTutorial : MonoBehaviour {

	[SerializeField]
	private LanguageData[] languageData;
	private LanguageData tutsLanguageData;

	public int sceneNum;
	public bool Tappable;
	public Image gaea;
	public Button tapButton;
	public Image talkingBox;
	public Text talkingText;
	public Text systemText;
	public float delay;

	[System.Serializable]
	public struct Scene
	{
		public GameObject stageButton;
		public string sceneTalkingText;
		public string sceneSystemText;
	};

	public Scene[] scene;

	void Start () {
		if (PlayerPrefs.GetInt ("FirstStageSelect") == 0) {
			PlayerPrefs.SetInt ("FirstStageSelect", 1);
			sceneNum = 0;
			Time.timeScale = 0;
			tutsLanguageData = languageData [PlayerPrefs.GetInt ("Language")];
			for (int i = 0; i < scene.Length; i++) {
				scene [i].sceneTalkingText = tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [i].talkingText;
				scene [i].sceneSystemText = tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [i].systemText;
			}
			StartCoroutine (PlayScene (delay));
			if (scene [sceneNum].sceneSystemText != "null") {
				systemText.text = scene [sceneNum].sceneSystemText;
			}
		} else {
			Destroy (this.gameObject);
		}
	}

	IEnumerator PlayScene (float delay) {
		if (scene [sceneNum].sceneTalkingText != "null") {
			talkingBox.gameObject.SetActive (true);
			talkingText.text = scene [sceneNum].sceneTalkingText; 
		} else {
			talkingBox.gameObject.SetActive (false);
		}
		if (scene [sceneNum].sceneSystemText != "null") {
			systemText.gameObject.SetActive (true);
		} else {
			systemText.gameObject.SetActive (false);
		}
		if (scene [sceneNum].stageButton != null) {
			scene [sceneNum].stageButton.SetActive (true);
		}
		Tappable = true;
		if (scene [sceneNum].sceneTalkingText == "null" && scene [sceneNum].sceneSystemText == "null") {
			yield return new WaitForSeconds (0.5f);
			Tap ();
		}
	}

	public void Tap () {
		if (Tappable) {
			if (sceneNum < scene.Length - 1) {
				Debug.LogError ("Tap");
				sceneNum++;
				Tappable = false;
				if (scene [sceneNum - 1].stageButton != null) {
					scene [sceneNum - 1].stageButton.SetActive (false);
				}
				StartCoroutine (PlayScene (delay));
				if (scene [sceneNum].sceneSystemText != "null") {
					systemText.text = scene [sceneNum].sceneSystemText;
				}
			} else if (sceneNum == scene.Length - 1) {
				Time.timeScale = 1;
				Destroy (this.gameObject);
			}
		}
	}

	public void Skip () {
		Time.timeScale = 1;
		Destroy (this.gameObject);
	}
}
