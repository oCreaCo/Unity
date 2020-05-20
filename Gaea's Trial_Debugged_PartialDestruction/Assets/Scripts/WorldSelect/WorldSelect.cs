using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WorldSelect : MonoBehaviour {

	[SerializeField]
	private LanguageData[] languageData;
	[SerializeField]
	private LanguageData worldSelectLanguageData;

	public Button[] world;

	public Text worldSelectText;
	public Text[] worldName;
	public Text backText;

	void Awake () {
		worldSelectLanguageData = languageData [PlayerPrefs.GetInt ("Language")];
		SetText ();
		for (int i = 2; i < 9; i++) {
			if (PlayerPrefs.GetInt ("world" + i + "Clear0") != 0) {
				world [i - 1].gameObject.SetActive (true);
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

	public void Back () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 4);
	}

	private void SetText () {
		worldSelectText.text = worldSelectLanguageData.worldSelect.worldSelect[0] + "\n" + worldSelectLanguageData.worldSelect.worldSelect[1];
		worldName [0].text = worldSelectLanguageData.worldSelect.worldName [0];
		worldName [1].text = worldSelectLanguageData.worldSelect.worldName [1];
		worldName [2].text = worldSelectLanguageData.worldSelect.worldName [2];
		worldName [3].text = worldSelectLanguageData.worldSelect.worldName [3];
		worldName [4].text = worldSelectLanguageData.worldSelect.worldName [4];
		worldName [5].text = worldSelectLanguageData.worldSelect.worldName [5];
		worldName [6].text = worldSelectLanguageData.worldSelect.worldName [6];
		worldName [7].text = worldSelectLanguageData.worldSelect.worldName [7];
		backText.text = worldSelectLanguageData.worldSelect.back;
	}
}
