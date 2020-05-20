using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InfiniteStage : MonoBehaviour {

	public int worldNum;

	void Awake () {
		if (PlayerPrefs.GetInt ("Ended") == 0) {
			this.gameObject.SetActive (false);
		}
	}

	[System.Serializable]
	public struct InfiniteWaveStruct
	{
		public int monsNum;
		public int monsSize;
		public int delay;
	}

	public InfiniteWaveStruct[] infiniteWaveList;

	public void GoToInfiniteStage () {
        if (AudioManager.instance != null) {
            AudioManager.instance.PlaySound("Click");
        }
		PlayerPrefs.SetInt ("worldNum", worldNum);
		PlayerPrefs.SetInt ("stageNum", 0);
		PlayerPrefs.SetInt ("minGold", 1);
		PlayerPrefs.SetInt ("maxGold", 3);
		for (int i = 0; i < 30; i++) {
			PlayerPrefs.SetInt ("monsNum" + i, infiniteWaveList [i].monsNum);
			PlayerPrefs.SetInt ("monsSize" + i, infiniteWaveList [i].monsSize);
			PlayerPrefs.SetInt ("delay" + i, infiniteWaveList [i].delay);
		}
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 3);
	}
}
