using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {

	public static BGM bgm;

	void Awake () {
		if (BGM.bgm == null) {
			BGM.bgm = this;
		} else {
			Destroy (this.gameObject);
		}
		DontDestroyOnLoad (this.gameObject);
	}
}
