using UnityEngine;
using UnityEngine.UI;

public class LivesCounter : MonoBehaviour {

	[SerializeField]
	private Text livesText;

	void Awake () {
		livesText = GetComponent<Text> ();
	}

	void Update () {
		livesText.text = "LIVES: " + GameMaster.RemainingLives.ToString();
	}
}
