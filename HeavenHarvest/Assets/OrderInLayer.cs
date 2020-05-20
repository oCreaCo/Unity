using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInLayer : MonoBehaviour {

	public void SetOne () {
		transform.Find ("PhaseUpCanvas").GetComponent<Canvas> ().sortingOrder = 1;
	}

	public void SetSix () {
		transform.Find ("PhaseUpCanvas").GetComponent<Canvas> ().sortingOrder = 6;
	}
}
