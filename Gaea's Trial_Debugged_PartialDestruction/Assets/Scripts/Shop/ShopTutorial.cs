using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopTutorial : MonoBehaviour {

	[SerializeField]
	private LanguageData[] languageData;
	private LanguageData tutsLanguageData;
	[SerializeField]
	private ItemData itemData;
	[SerializeField]
	private ShopCameraPosition camera;

	public int sceneNum;
	public bool Tappable;
	public Button tapButton;
	public Text talkingText;
	public Text systemText;
	public float delay;

	public GameObject backToMain;
	public GameObject hero;
	public GameObject weaponRoulette;
	public GameObject castle;
	public GameObject gemStore;

	[System.Serializable]
	public struct Scene
	{
		public GameObject highlight;
	};

	public Scene[] scene;

	void Start () {
		if (PlayerPrefs.GetInt ("FirstInventory") == 0) {
			PlayerPrefs.SetInt ("FirstInventory", 1);
			sceneNum = 0;
			tutsLanguageData = languageData [PlayerPrefs.GetInt ("Language")];
			StartCoroutine (PlayScene (delay));
			if (tutsLanguageData.tutorialsTexts [5].tutorialsTextsBundle [sceneNum].systemText != "") {
				systemText.text = tutsLanguageData.tutorialsTexts [5].tutorialsTextsBundle [sceneNum].systemText;
			}
			if (tutsLanguageData.tutorialsTexts [5].tutorialsTextsBundle [sceneNum].talkingText != "") {
				talkingText.text = tutsLanguageData.tutorialsTexts [5].tutorialsTextsBundle [sceneNum].talkingText;
			}
		} else {
			Destroy (this.gameObject);
		}
	}

	IEnumerator PlayScene (float delay) {
		if (tutsLanguageData.tutorialsTexts [5].tutorialsTextsBundle [sceneNum].talkingText != "") {
			talkingText.text = tutsLanguageData.tutorialsTexts [5].tutorialsTextsBundle [sceneNum].talkingText; 
		}
		if (tutsLanguageData.tutorialsTexts [5].tutorialsTextsBundle [sceneNum].systemText != "") {
			systemText.gameObject.SetActive (true);
		} else {
			systemText.gameObject.SetActive (false);
		}
		if (scene [sceneNum].highlight != null) {
			scene [sceneNum].highlight.SetActive (true);
		}
		Tappable = true;
		if (tutsLanguageData.tutorialsTexts [5].tutorialsTextsBundle [sceneNum].talkingText == "" && tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [sceneNum].systemText == "") {
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
				if (tutsLanguageData.tutorialsTexts [5].tutorialsTextsBundle [sceneNum].systemText != "null") {
					systemText.text = tutsLanguageData.tutorialsTexts [5].tutorialsTextsBundle [sceneNum].systemText;
				}
				switch (sceneNum) {
				case 1:
					backToMain.SetActive (true);
					break;
				case 2:
					hero.SetActive (true);
					backToMain.SetActive (false);
					break;
				case 3:
					hero.SetActive (false);
					camera.GoToHero ();
					break;
				case 5:
					camera.GoToShopMain ();
					break;
				case 6:
					weaponRoulette.SetActive (true);
					break;
				case 7:
					camera.GoToWeapons ();
					weaponRoulette.SetActive (false);
					break;
				case 9:
					camera.GoToShopMain ();
					break;
				case 10:
					castle.SetActive (true);
					break;
				case 11:
					camera.GoToCastle ();
					castle.SetActive (false);
					break;
				case 13:
					camera.GoToShopMain ();
					break;
				case 14:
					gemStore.SetActive (true);
					break;
				case 15:
					camera.GoToGemStore ();
					gemStore.SetActive (false);
					break;
				case 16:
					camera.GoToShopMain ();
					break;
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
