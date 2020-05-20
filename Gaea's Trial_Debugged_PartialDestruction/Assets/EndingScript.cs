using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScript : MonoBehaviour {

	[SerializeField] private CameraPosition camPos;
	[SerializeField] float delay;

	[SerializeField] private GameObject[] worlds;
	[SerializeField] private GameObject[] texts;
	[SerializeField] private GameObject backGround;

	IEnumerator EndingCoroutine (float _dealy) {
		yield return new WaitForSeconds (_dealy);
		worlds [1].SetActive (true);
		texts [1].SetActive (true);
		camPos.UpCameraPosiiton ();
		worlds [0].SetActive (false);
		texts [0].SetActive (false);
		yield return new WaitForSeconds (_dealy);
		worlds [2].SetActive (true);
		texts [2].SetActive (true);
		camPos.UpCameraPosiiton ();
		worlds [1].SetActive (false);
		texts [1].SetActive (false);
		yield return new WaitForSeconds (_dealy);
		worlds [3].SetActive (true);
		texts [3].SetActive (true);
		camPos.UpCameraPosiiton ();
		worlds [2].SetActive (false);
		texts [2].SetActive (false);
		yield return new WaitForSeconds (_dealy);
		worlds [4].SetActive (true);
		texts [4].SetActive (true);
		camPos.UpCameraPosiiton ();
		worlds [3].SetActive (false);
		texts [3].SetActive (false);
		yield return new WaitForSeconds (_dealy);
		worlds [5].SetActive (true);
		texts [5].SetActive (true);
		camPos.UpCameraPosiiton ();
		worlds [4].SetActive (false);
		texts [4].SetActive (false);
		yield return new WaitForSeconds (_dealy);
		worlds [6].SetActive (true);
		texts [6].SetActive (true);
		camPos.UpCameraPosiiton ();
		worlds [5].SetActive (false);
		texts [5].SetActive (false);
		yield return new WaitForSeconds (_dealy);
		worlds [7].SetActive (true);
		camPos.UpCameraPosiiton ();
		worlds [6].SetActive (false);
		texts [6].SetActive (false);
		backGround.SetActive (false);
//		yield return new WaitForSeconds (4f);
//		worlds [8].SetActive (true);
//		texts [7].SetActive (true);
//		camPos.UpCameraPosiiton ();
//		worlds [7].SetActive (false);
		PlayerPrefs.SetInt ("Ended", 1);
		yield return new WaitForSeconds (10f);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 6);
	}

	void Start () {
		StartCoroutine (EndingCoroutine (delay));
	}
}
