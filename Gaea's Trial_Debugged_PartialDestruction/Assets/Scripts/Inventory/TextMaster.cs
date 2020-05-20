using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextMaster : MonoBehaviour {

	[SerializeField] private GameDB gameDB;

	//MainScreen
	public Text mainBackText;
	public Text hWeaponButtonText;
	public Text heroButtonText;
	public Text mWeaponButtonText;
	public Text shopButtonText;
	public Text itemInfoBackText;

	//Shop
//	public Text itemNameText;
//	public Text rollItemText;
//	public Text purchase1Text;
//	public Text purchase2Text;
//	public Text purchase3Text;
//	public Text purchase4Text;
//	public Text purchase5Text;
//	public Text purchase6Text;
//	public Text shopBackText;

	void Awake () {
		LanguageData textGameDBData = gameDB.gameDBLanguageData [PlayerPrefs.GetInt ("Language")];
		mainBackText.text = textGameDBData.gameDBTextManager.back;
		hWeaponButtonText.text = textGameDBData.gameDBTextManager.hWeapon;
		heroButtonText.text = textGameDBData.gameDBTextManager.hero;
		mWeaponButtonText.text = textGameDBData.gameDBTextManager.mWeapon;
		shopButtonText.text = textGameDBData.gameDBTextManager.shop;
		itemInfoBackText.text = textGameDBData.gameDBTextManager.back;
		if (PlayerPrefs.GetInt ("Language") == 0) {

		} else if (PlayerPrefs.GetInt ("Language") == 1) {

		}
	}

//	public void SetShopText ()
//	{
//		if (PlayerPrefs.GetInt ("Language") == 0) {
//			itemNameText.text = "";
//			rollItemText.text = "Roll Item\n10 Gems";
//			purchase1Text.text = "";
//			purchase2Text.text = "";
//			purchase3Text.text = "";
//			purchase4Text.text = "";
//			purchase5Text.text = "";
//			purchase6Text.text = "";
//			shopBackText.text = "Back";
//		} else if (PlayerPrefs.GetInt ("Language") == 1) {
//			itemNameText.text = "";
//			rollItemText.text = "아이템 뽑기\n10 보석";
//			purchase1Text.text = "";
//			purchase2Text.text = "";
//			purchase3Text.text = "";
//			purchase4Text.text = "";
//			purchase5Text.text = "";
//			purchase6Text.text = "";
//			shopBackText.text = "뒤로";
//		}
//	}
}
