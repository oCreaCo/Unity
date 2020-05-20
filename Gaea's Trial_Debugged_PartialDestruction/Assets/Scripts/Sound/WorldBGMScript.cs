using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBGMScript : MonoBehaviour {

	public GameObject[] bgms;

	public void Start () {
		if (AudioManager.instance != null && AudioManager.instance.playingBGMusic) {
			AudioManager.instance.playingBGMusic = false;
			AudioManager.instance.transform.FindChild ("Sound_1_BGMusic").GetComponent<AudioSource>().Stop();
		}
		GameObject bgm = Instantiate (bgms [GameMaster.gameMaster.worldNum]) as GameObject;
		bgm.transform.SetParent (GameMaster.gameMaster.transform);
		bgm.GetComponent<AudioSource> ().Play ();
	}
}