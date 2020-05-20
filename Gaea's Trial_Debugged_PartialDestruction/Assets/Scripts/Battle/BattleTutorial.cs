using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTutorial : MonoBehaviour {

	[SerializeField]
	private LanguageData[] languageData;
	private LanguageData tutsLanguageData;

	public int sceneNum;
	public bool Tappable;
	public Button tapButton;
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
		sceneNum = 0;
		tutsLanguageData = languageData [PlayerPrefs.GetInt ("Language")];
		StartCoroutine (PlayScene (delay));
		if (tutsLanguageData.tutorialsTexts [3].tutorialsTextsBundle [sceneNum].systemText != "") {
			systemText.text = tutsLanguageData.tutorialsTexts [3].tutorialsTextsBundle [sceneNum].systemText;
		}
		if (tutsLanguageData.tutorialsTexts [3].tutorialsTextsBundle [sceneNum].talkingText != "") {
			talkingText.text = tutsLanguageData.tutorialsTexts [3].tutorialsTextsBundle [sceneNum].talkingText;
		}
	}

	IEnumerator PlayScene (float delay) {
		if (scene [sceneNum].highlight != null) {
			scene [sceneNum].highlight.SetActive (true);
		}
		Tappable = true;
		if (tutsLanguageData.tutorialsTexts [3].tutorialsTextsBundle [sceneNum].systemText == "" && tutsLanguageData.tutorialsTexts [3].tutorialsTextsBundle [sceneNum].talkingText == "") {
			yield return new WaitForSeconds (0.5f);
			Tap ();
		}
	}

	public void Tap () {
		if (Tappable) {
			if (sceneNum < scene.Length - 1) {
				sceneNum++;
				Tappable = false;
				if (scene [sceneNum - 1].highlight != null) {
					scene [sceneNum - 1].highlight.SetActive (false);
				}
				StartCoroutine (PlayScene (delay));
				if (tutsLanguageData.tutorialsTexts [3].tutorialsTextsBundle [sceneNum].systemText != "") {
					systemText.text = tutsLanguageData.tutorialsTexts [3].tutorialsTextsBundle [sceneNum].systemText;
				}
				if (tutsLanguageData.tutorialsTexts [3].tutorialsTextsBundle [sceneNum].talkingText != "") {
					talkingText.text = tutsLanguageData.tutorialsTexts [3].tutorialsTextsBundle [sceneNum].talkingText;
				}
			} else if (sceneNum == scene.Length - 1) {
				Destroy (this.gameObject);
			}
		}
	}

	public void Skip () {
		Destroy (this.gameObject);
	}
}
