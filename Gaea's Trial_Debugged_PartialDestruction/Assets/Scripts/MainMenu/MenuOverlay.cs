using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuOverlay : MonoBehaviour {

	public Text gold;

	// Use this for initialization
	void Awake () {
		if (gold == null) {
			Debug.LogError ("no 'GoldText' object!");
		}

		//gold.text = MainGM.maingm.Gold.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
