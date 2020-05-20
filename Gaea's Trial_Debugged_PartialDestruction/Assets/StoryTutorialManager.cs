using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryTutorialManager : MonoBehaviour {

	[SerializeField]
	private LanguageData[] languageData;
	private LanguageData tutsLanguageData;
	public Animator[] sceneAnims;
	public Transform[] cameraPosition;
	public float cameraMoveTime;

	public int sceneNum;
	public bool tappable = false;

	public Text[] systemText;
	public float textDelay;
	private string curText;

	public Vector3 startPos, endPos;

	void Start () {
		sceneNum = 0;
		tutsLanguageData = languageData [PlayerPrefs.GetInt ("Language")];
		startPos = cameraPosition [0].position;
		endPos = cameraPosition [0].position;
		StartCoroutine (PlayAnim ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMouseDown () {
		if (tappable) {
			if (sceneNum == 6) {
				SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex  + 1);
			} else {
				curText = "";
				systemText [sceneNum].text = curText;
				tappable = false;
				sceneNum++;
				startPos = this.transform.parent.position;
				endPos = cameraPosition [sceneNum].position;
				StartCoroutine (PlayAnim ());
			}
		}

	}

	IEnumerator PlayAnim () {
		if (sceneNum != 0 && sceneNum != 1 && sceneNum != 3) {
			sceneAnims [sceneNum - 1].SetTrigger ("Exit");
		} else if (sceneNum == 3) {
			sceneAnims [sceneNum - 1].SetTrigger ("Phase2");
		}
		for (float t = 0; t <= 1 * cameraMoveTime; t += Time.deltaTime)
		{
			this.transform.parent.position = Vector3.Lerp (startPos, endPos, t / cameraMoveTime);
			yield return 0;
		}
		this.transform.parent.position = endPos;
		if (sceneNum != 3 && sceneNum != 6) {
			sceneAnims [sceneNum].gameObject.SetActive (true);
		}
		for (int i = 0; i < tutsLanguageData.tutorialsTexts [0].tutorialsTextsBundle [sceneNum].systemText.Length; i++) {
			curText = tutsLanguageData.tutorialsTexts [0].tutorialsTextsBundle [sceneNum].systemText.Substring (0, i);
			systemText [sceneNum].text = curText;
			yield return new WaitForSeconds (textDelay);
		}
		tappable = true;
	}

	public void Skip () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex  + 1);
	}
}
