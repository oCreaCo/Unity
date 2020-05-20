using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsScript : MonoBehaviour {

	[SerializeField] private string playerPrefs;

	public void PrefsDel () {
		if (playerPrefs == null) {
			Debug.LogError ("The String is null");
		} else {
			if (PlayerPrefs.HasKey (playerPrefs)) {
				PlayerPrefs.DeleteKey (playerPrefs);
				Debug.LogError ("Deleted Successfully");
			} else {
				Debug.LogError ("The key isn't being");
			}
		}
	}
}
