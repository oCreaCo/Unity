using UnityEngine;
using System.Collections;

public class StartBG : MonoBehaviour {

	public string BGName;

	IEnumerator waitForMusic (){
		yield return new WaitForSeconds (0.1f);
		AudioManager.instance.PlaySound (BGName);
	}

	void Awake ()
	{
		StartCoroutine (waitForMusic ());
	}
}
