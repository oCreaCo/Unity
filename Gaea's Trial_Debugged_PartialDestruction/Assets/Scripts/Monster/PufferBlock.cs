using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferBlock : MonoBehaviour {

	[System.Serializable]
	public struct Phases
	{
		public Sprite sprite;
		public int damage;
	}

	public Phases[] phases;
	public int phase = 2;

	void Start () {
		StartCoroutine (PhaseCoroutine ());
	}

	IEnumerator PhaseCoroutine () {
		yield return new WaitForSeconds (7f);
		phase--;
		this.transform.FindChild ("Puffer").GetComponent<SpriteRenderer> ().sprite = phases [phase].sprite;
		yield return new WaitForSeconds (7f);
		phase--;
		this.transform.FindChild ("Puffer").GetComponent<SpriteRenderer> ().sprite = phases [phase].sprite;
	}
}
