using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingUI : MonoBehaviour {

	[SerializeField]
	private LanguageData[] languageData;
	[SerializeField]
	private LanguageData settingUILanguageData;

	public GameObject settingUI;
	public GameObject volumeUI;
	public GameObject monsterDictionary;

	public Text languageText;
	public Text backText;
	public Text startText;
	public Text invenText;
	public Text quitText;
	public Text monsterDicText;

	void Awake () {
		settingUILanguageData = languageData [PlayerPrefs.GetInt ("Language")];
		SetText ();
	}

	public void Setting () {
		settingUI.SetActive (true);
		if (PlayerPrefs.GetInt ("MonsterDictionaryMaxMonsNum") != 0) {
			monsterDictionary.SetActive (true);
		}
		SetText ();
	}

	public void SetKorean () {
		PlayerPrefs.SetInt ("Language", 1);
		settingUILanguageData = languageData [PlayerPrefs.GetInt ("Language")];
		SetText ();
	}

	public void SetEnglish () {
		PlayerPrefs.SetInt ("Language", 0);
		settingUILanguageData = languageData [PlayerPrefs.GetInt ("Language")];
		SetText ();
	}

	private void SetText () {
		languageText.text = settingUILanguageData.settingUIText.language;
		backText.text = settingUILanguageData.settingUIText.back;
		startText.text = settingUILanguageData.settingUIText.start;
		invenText.text = settingUILanguageData.settingUIText.inventory;
		quitText.text = settingUILanguageData.settingUIText.quit;
		monsterDicText.text = settingUILanguageData.settingUIText.monsterDictionary;
	}

	public void SettingBack () {
		settingUI.SetActive (false);
	}

	public void Volume () {
		volumeUI.SetActive (true);
	}
}
