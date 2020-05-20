using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryTutorial : MonoBehaviour {

	[SerializeField]
	private LanguageData[] languageData;
	private LanguageData tutsLanguageData;
	[SerializeField]
	private ItemData itemData;
	[SerializeField]
	private InventoryController realInventory;

	public int sceneNum;
	public bool Tappable;
	public Button tapButton;
	public Image talkingBox;
	public Text talkingText;
	public Text systemText;
	public float delay;

	public GameObject hWeaponInventory;
	public GameObject heroInventory;
	public GameObject mWeaponInventory;
	public Button hWeaponButton;
	public Button heroButton;
	public Button mWeaponButton;

	public Button itemInformationAwakeButton;
	public GameObject itemInformation;
	public Button itemInformationBackButton;
	public Text itemNameAndLevel;
	public Text itemStat;
	public Text itemInfo;

	public GameObject shopButton;

	public void HWeaponFront () {
		hWeaponInventory.SetActive (true);
		heroInventory.SetActive (false);
		mWeaponInventory.SetActive (false);
	}

	public void HeroFront () {
		hWeaponInventory.SetActive (false);
		heroInventory.SetActive (true);
		mWeaponInventory.SetActive (false);
	}

	public void MWeaponFront () {
		hWeaponInventory.SetActive (false);
		heroInventory.SetActive (false);
		mWeaponInventory.SetActive (true);
	}

	[System.Serializable]
	public struct Scene
	{
		public GameObject highlight;
	};

	public Scene[] scene;

	void Start () {
		if (PlayerPrefs.GetInt ("FirstInventory") == 0) {
			sceneNum = 0;
			tutsLanguageData = languageData [PlayerPrefs.GetInt ("Language")];
			StartCoroutine (PlayScene (delay));
			if (tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [sceneNum].systemText != "") {
				systemText.text = tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [sceneNum].systemText;
			}
			if (tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [sceneNum].talkingText != "") {
				talkingText.text = tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [sceneNum].talkingText;
			}
			itemNameAndLevel.text = tutsLanguageData.gameDBHeroItemList [2].heroItemName + "\n" + "Level 0";
			itemStat.text = tutsLanguageData.gameDBItemInfoText.damage + itemData.heroItemList [2].heroItemDamageMin [0] + "\n" + tutsLanguageData.gameDBItemInfoText.penetration + itemData.heroItemList [2].heroItemPenetration;
			itemInfo.text = tutsLanguageData.gameDBHeroItemList [2].heroItemInfo;
			realInventory.SetItemsColliders (false);
		} else {
			Destroy (this.gameObject);
		}
	}

	IEnumerator PlayScene (float delay) {
		if (tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [sceneNum].talkingText != "") {
			talkingBox.gameObject.SetActive (true);
			talkingText.text = tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [sceneNum].talkingText; 
		} else {
			talkingBox.gameObject.SetActive (false);
		}
		if (tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [sceneNum].systemText != "") {
			systemText.gameObject.SetActive (true);
		} else {
			systemText.gameObject.SetActive (false);
		}
		if (scene [sceneNum].highlight != null) {
			scene [sceneNum].highlight.SetActive (true);
		}
		Tappable = true;
		if (tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [sceneNum].talkingText == "" && tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [sceneNum].systemText == "") {
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
				if (tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [sceneNum].systemText != "null") {
					systemText.text = tutsLanguageData.tutorialsTexts [4].tutorialsTextsBundle [sceneNum].systemText;
				}
				switch (sceneNum) {
				case 1:
					tapButton.gameObject.SetActive (false);
					itemInformationAwakeButton.interactable = true;
					break;
				case 2:
					tapButton.gameObject.SetActive (true);
					itemInformationAwakeButton.interactable = false;
					itemInformation.SetActive (true);
					break;
				case 4:
					tapButton.gameObject.SetActive (false);
					itemInformationBackButton.interactable = true;
					break;
				case 5:
					tapButton.gameObject.SetActive (true);
					itemInformation.SetActive (false);
					tapButton.gameObject.SetActive (false);
					heroButton.interactable = true;
					break;
				case 6:
					heroButton.interactable = false;
					mWeaponButton.interactable = true;
					break;
				case 7:
					mWeaponButton.interactable = false;
					hWeaponButton.interactable = true;
					break;
				case 8:
					hWeaponButton.interactable = false;
					tapButton.gameObject.SetActive (true);
					tapButton.gameObject.SetActive (false);
					shopButton.SetActive (true);
					break;
				}
			} else if (sceneNum == scene.Length - 1) {
				realInventory.SetItemsColliders (true);
				Destroy (this.gameObject);
			}
		}
	}

	public void Skip () {
		realInventory.SetItemsColliders (true);
		Destroy (this.gameObject);
	}
}
