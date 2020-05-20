using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WorldButton : MonoBehaviour {

	public int worldNum;

	public void GoToStage () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		PlayerPrefs.SetInt ("worldNum", worldNum);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);
	}
}
