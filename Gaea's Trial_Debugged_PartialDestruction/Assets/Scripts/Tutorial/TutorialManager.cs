using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialManager : MonoBehaviour {

	public static TutorialManager tutorialManager;

	[SerializeField]
	private LanguageData[] languageData;
	private LanguageData tutsLanguageData;

	public Transform hero;
	public Transform magician;
	public int sceneNum;
	public Button tapButton;
	public Image talkingBox;
	public Text talkingText;
	public Text systemText;
	public float animDelay;
	public bool Tappable;

	public Camera mainCamera;

	public Transform[] cameraPositions;

	public int currentPosition;

	public GameObject deminions;
	public GameObject hBasicAttackFake;
	public GameObject hBasicAttack;
	public GameObject mBasicAttack;
	public GameObject mBasicAttackEffect;
	public GameObject jewels3Match, jewels4Match, jewels5Match, specialJewels;
	public GameObject redJewel, blueJewel, purpleJewel, greenJewel, yellowJewel;
	public GameObject castle, heal, goldButton;
	public GameObject[] blessParticles;
	public GameObject penetration;

	public enum Anims
	{
		NONE,
		IDLE,
		BORN,
		JUMP,
		SWING,
		RUNOUT,
	};

	[System.Serializable]
	public struct Scene
	{
		public Anims[] heroAnims;
		public Anims[] magicianAnims;
	};

	public Scene[] scene;

	// Use this for initialization
	void Awake () {
		tutorialManager = this;
		sceneNum = 0;
		tutsLanguageData = languageData [PlayerPrefs.GetInt ("Language")];
		StartCoroutine (PlayAnim (animDelay));
		if (tutsLanguageData.tutorialsTexts [1].tutorialsTextsBundle [sceneNum].systemText != "") {
			systemText.text = tutsLanguageData.tutorialsTexts [1].tutorialsTextsBundle [sceneNum].systemText;
		}
		GoToCameraPosition (currentPosition);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator PlayAnim (float delay) {
		if (tutsLanguageData.tutorialsTexts [1].tutorialsTextsBundle [sceneNum].talkingText != "") {
			talkingBox.gameObject.SetActive (true);
			talkingText.text = tutsLanguageData.tutorialsTexts [1].tutorialsTextsBundle [sceneNum].talkingText; 
		} else {
			talkingBox.gameObject.SetActive (false);
		}
		if (tutsLanguageData.tutorialsTexts [1].tutorialsTextsBundle [sceneNum].systemText != "") {
			systemText.gameObject.SetActive (true);
		} else {
			systemText.gameObject.SetActive (false);
		}
		if (scene [sceneNum].heroAnims.Length >= scene [sceneNum].magicianAnims.Length) {
			for (int i = 0; i < scene [sceneNum].heroAnims.Length; i++) {
				if (scene [sceneNum].heroAnims.Length != 0) {
					hero.FindChild ("Animator").GetComponent<Animator> ().SetTrigger (scene [sceneNum].heroAnims [i].ToString ());
				}
				if (scene [sceneNum].magicianAnims.Length != 0) {
					magician.FindChild ("Animator").GetComponent<Animator> ().SetTrigger (scene [sceneNum].magicianAnims [i].ToString ());
				}
				yield return new WaitForSeconds (delay);
			}
		} else if (scene [sceneNum].heroAnims.Length < scene [sceneNum].magicianAnims.Length) {
			for (int i = 0; i < scene [sceneNum].magicianAnims.Length; i++) {
				if (scene [sceneNum].heroAnims.Length != 0) {
					hero.FindChild ("Animator").GetComponent<Animator> ().SetTrigger (scene [sceneNum].heroAnims [i].ToString ());
				}
				if (scene [sceneNum].magicianAnims.Length != 0) {
					magician.FindChild ("Animator").GetComponent<Animator> ().SetTrigger (scene [sceneNum].magicianAnims [i].ToString ());
				}
				yield return new WaitForSeconds (delay);
			}
		}
		Tappable = true;
		if (tutsLanguageData.tutorialsTexts [1].tutorialsTextsBundle [sceneNum].talkingText == "" && tutsLanguageData.tutorialsTexts [1].tutorialsTextsBundle [sceneNum].systemText == "") {
			yield return new WaitForSeconds (0.5f);
			Tap ();
		}
	}

	IEnumerator SeeDeminion () {
		deminions.SetActive (true);
		Tappable = false;
		for (int i = 0; i < 60; i++) {
			yield return new WaitForSeconds (0.018f);
			mainCamera.transform.position += (cameraPositions [1].position - cameraPositions [0].position) / 60;
		}
		yield return new WaitForSeconds (1f);
		for (int i = 0; i < 60; i++) {
			yield return new WaitForSeconds (0.018f);
			mainCamera.transform.position -= (cameraPositions [1].position - cameraPositions [0].position) / 60;
		}
		deminions.SetActive (false);
		yield return new WaitForSeconds (0.5f);
		Tappable = true;
		Tap ();
	}

	IEnumerator BlessEffect (GameObject _blessParticle) {
		Tappable = false;
		_blessParticle.SetActive (true);
		yield return new WaitForSeconds (2.0f);
		_blessParticle.GetComponent<ParticleSystem> ().Stop ();
		yield return new WaitForSeconds (1.0f);
		_blessParticle.SetActive (false);
		Tappable = true;
		Tap ();
	}

	public void Tap () {
		if (Tappable) {
			if (sceneNum < scene.Length - 1) {
				sceneNum++;
				Tappable = false;
				StartCoroutine (PlayAnim (animDelay));
				if (tutsLanguageData.tutorialsTexts [1].tutorialsTextsBundle [sceneNum].systemText != "") {
					systemText.text = tutsLanguageData.tutorialsTexts [1].tutorialsTextsBundle [sceneNum].systemText;
				}
				switch (sceneNum) {
				case 1:
					StartCoroutine (SeeDeminion ());
					break;
				case 8:
					StartCoroutine (FakeCoroutine ());
					break;
				case 15:
					StartCoroutine (BlessEffect (blessParticles [0]));
					break;
				case 20:
					jewels3Match.SetActive (true);
					tapButton.gameObject.SetActive (false);
					break;
				case 21:
					StartCoroutine (SeeDeminion ());
					hBasicAttack.SetActive (true);
					jewels3Match.SetActive (false);
					break;
				case 24:
					jewels4Match.SetActive (true);
					tapButton.gameObject.SetActive (false);
					break;
				case 25:
					jewels4Match.SetActive (false);
					break;
				case 26:
					jewels5Match.SetActive (true);
					tapButton.gameObject.SetActive (false);
					break;
				case 27:
					jewels5Match.SetActive (false);
					break;
				case 28:
					specialJewels.SetActive (true);
					break;
				case 30:
					specialJewels.transform.FindChild ("VerJewel").gameObject.SetActive (false);
					specialJewels.transform.FindChild ("HorJewel").gameObject.SetActive (false);
					break;
				case 31:
					specialJewels.transform.FindChild ("VerJewel").gameObject.SetActive (true);
					specialJewels.transform.FindChild ("HorJewel").gameObject.SetActive (true);
					specialJewels.SetActive (false);
					break;
				case 32:
					StartCoroutine (SeeDeminion ());
					break;
				case 36:
					penetration.SetActive (true);
					break;
				case 37:
					penetration.SetActive (false);
					break;
				case 41:
					mBasicAttack.SetActive (true);
					mBasicAttackEffect.SetActive (true);
					StartCoroutine (SeeDeminion ());
					break;
				case 50:
					StartCoroutine (BlessEffect (blessParticles [1]));
					break;
//				case 53:
//					redJewel.SetActive (true);
//					blueJewel.SetActive (true);
//					break;
//				case 56:
//					redJewel.SetActive (false);
//					blueJewel.SetActive (false);
//					break;
//				case 57:
//					purpleJewel.SetActive (true);
//					break;
//				case 58:
//					purpleJewel.SetActive (false);
//					break;
//				case 59:
//					greenJewel.SetActive (true);
//					castle.SetActive (true);
//					heal.SetActive (true);
//					break;
//				case 60:
//					greenJewel.SetActive (false);
//					castle.GetComponent<Animator> ().SetTrigger ("Out");
//					heal.SetActive (false);
//					yellowJewel.SetActive (true);
//					goldButton.SetActive (true);
//					break;
				case 52:
					castle.SetActive (false);
					yellowJewel.SetActive (false);
					goldButton.SetActive (false);
					break;
				}
			} else if (sceneNum == scene.Length - 1) {
				SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex  + 1);
			}
		}
	}

	IEnumerator FakeCoroutine () {
		yield return new WaitForSeconds (1.0f);
		hBasicAttackFake.SetActive (true);
	}

	public void Skip () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex  + 1);
	}

	public void GoToCameraPosition (int num) {
		this.transform.position = cameraPositions [num].position;
	}
}
