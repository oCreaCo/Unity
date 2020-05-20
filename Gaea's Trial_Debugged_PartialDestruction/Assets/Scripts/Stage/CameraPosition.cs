using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraPosition : MonoBehaviour {

	public Vector3[] cameraPositions;

	public Text backText;

	public int currentPosition;
	public bool stageSelect;

	// Use this for initialization
	void Awake () {
		if (backText != null) {
			if (PlayerPrefs.GetInt ("Language") == 0) {
				backText.text = "Back";
			} else if (PlayerPrefs.GetInt ("Language") == 1) {
				backText.text = "뒤로";
			}
		}
		if (stageSelect) {
			currentPosition = PlayerPrefs.GetInt ("worldNum") - 1;
		}
		GoToCameraPosition (currentPosition);
	}

	public void UpCameraPosiiton () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		currentPosition += 1;
		GoToCameraPosition (currentPosition);
	}

	public void DownCameraPosition () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		currentPosition -= 1;
		GoToCameraPosition (currentPosition);
	}

	public void GoToCameraPosition (int num) {
		if (num >= 0) {
			this.transform.position = cameraPositions [num];
		} else {
			this.transform.position = cameraPositions [0];
		}
	}
}
