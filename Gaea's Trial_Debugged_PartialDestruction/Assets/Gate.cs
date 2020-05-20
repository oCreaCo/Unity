using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

	[SerializeField] private GameObject gate;

	void Awake () {
		if (PlayerPrefs.GetInt ("Ended") == 1) {
			Destroy (gate.gameObject);
		}
	}
}
