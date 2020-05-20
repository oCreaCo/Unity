using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class EndingGaea : MonoBehaviour {
	
	[SerializeField]
	private LanguageData[] languageData;
	private LanguageData endingLanguageData;

	public Transform hero;
	public Transform magician;
	public int sceneNum;
	public Button tapButton;
	public Text talkingText;
	public Text systemText;
	public float animDelay;
	public bool Tappable;
	private int scene;
	
	void Awake () {
		GameMaster.gameMaster.remainingEnemies.Add (this.gameObject);
		endingLanguageData = languageData [PlayerPrefs.GetInt ("Language")];
		scene = endingLanguageData.endingGaea.Length;
		sceneNum = 0;
		StartCoroutine (JewelClearCoroutine ());
	}

	IEnumerator JewelClearCoroutine () {
		List<GameObject> jewels = new List<GameObject> ();
		jewels.AddRange (GameObject.FindGameObjectsWithTag ("Jewel"));
		for (int i = 0; i < jewels.Count; i++) {
			jewels [i].GetComponent<Animator> ().SetTrigger ("Close");
		}
		yield return new WaitForSeconds (1.2f);
		for (int i = 0; i < jewels.Count; i++) {
			Destroy (jewels [i]);
		}
		jewels.Clear ();
		yield return new WaitForSeconds (1.8f);
		if (endingLanguageData.endingGaea [sceneNum] != "") {
			talkingText.text = endingLanguageData.endingGaea [sceneNum];
		}
		Tappable = true;
	}

	public void Tap () {
		if (Tappable) {
			Debug.LogError ("Tap");
			sceneNum++;
			if (sceneNum < scene) {
				if (endingLanguageData.endingGaea[sceneNum] != "") {
					talkingText.text = endingLanguageData.endingGaea [sceneNum];
				}
				switch (sceneNum) {
				case 1:
					
					break;
				}
			} else if (sceneNum == scene) {
				SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex  + 5);
//				Debug.LogError("End");
			}
		}
	}
}
