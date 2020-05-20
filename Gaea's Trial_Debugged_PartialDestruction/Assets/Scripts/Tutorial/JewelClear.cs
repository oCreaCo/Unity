using UnityEngine;
using System.Collections;

public class JewelClear : MonoBehaviour {

	public Animator thisAnim, cursorAnim;

	public bool upCheck, downCheck;

	public GameObject up, down;
	public GameObject[] jewels;

	public void Clear () {
		if (upCheck == true && downCheck == true) {
			thisAnim.SetTrigger ("JewelClear");
			Debug.LogError ("JewelClear");
		} else {
			cursorAnim.SetTrigger ("Down");
		}
		upCheck = false;
		downCheck = false;
	}

	public void ClearEffect () {
		for (int i = 0; i < jewels.Length; i++) {
			jewels [i].GetComponent<TutorialClearablePiece> ().Clear ();
		}
	}

	public void Tap () {
//		this.transform.parent.FindChild ("TutorialManager").GetComponent<TutorialManager> ().Tap ();
		up.SetActive(false);
		down.SetActive (false);
		TutorialManager.tutorialManager.Tap ();
		TutorialManager.tutorialManager.tapButton.gameObject.SetActive (true);
	}
}
