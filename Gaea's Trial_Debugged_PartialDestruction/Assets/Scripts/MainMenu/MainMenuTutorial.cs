using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuTutorial : MonoBehaviour {

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
		public GameObject highlight;
	};

	public Scene[] scene;

	void Start () {
		if (PlayerPrefs.GetInt ("FirstMainMenu") == 0) {
			PlayerPrefs.SetInt ("FirstMainMenu", 1);
			sceneNum = 0;
			Time.timeScale = 0;
			tutsLanguageData = languageData [PlayerPrefs.GetInt ("Language")];
			StartCoroutine (PlayScene (delay));
			if (tutsLanguageData.tutorialsTexts [2].tutorialsTextsBundle [sceneNum].systemText != "") {
				systemText.text = tutsLanguageData.tutorialsTexts [2].tutorialsTextsBundle [sceneNum].systemText;
			}
		} else {
			Destroy (this.gameObject);
		}
	}

	IEnumerator PlayScene (float delay) {
		if (tutsLanguageData.tutorialsTexts [2].tutorialsTextsBundle [sceneNum].talkingText != "") {
			talkingBox.gameObject.SetActive (true);
			talkingText.text = tutsLanguageData.tutorialsTexts [2].tutorialsTextsBundle [sceneNum].talkingText; 
		} else {
			talkingBox.gameObject.SetActive (false);
		}
		if (tutsLanguageData.tutorialsTexts [2].tutorialsTextsBundle [sceneNum].systemText != "") {
			systemText.gameObject.SetActive (true);
		} else {
			systemText.gameObject.SetActive (false);
		}
		if (scene [sceneNum].highlight != null) {
			scene [sceneNum].highlight.SetActive (true);
		}
		Tappable = true;
		if (tutsLanguageData.tutorialsTexts [2].tutorialsTextsBundle [sceneNum].talkingText == "" && tutsLanguageData.tutorialsTexts [2].tutorialsTextsBundle [sceneNum].systemText == "") {
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
				if (scene [sceneNum - 1].highlight != null) {
					scene [sceneNum - 1].highlight.SetActive (false);
				}
				StartCoroutine (PlayScene (delay));
				if (tutsLanguageData.tutorialsTexts [2].tutorialsTextsBundle [sceneNum].systemText != "") {
					systemText.text = tutsLanguageData.tutorialsTexts [2].tutorialsTextsBundle [sceneNum].systemText;
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
