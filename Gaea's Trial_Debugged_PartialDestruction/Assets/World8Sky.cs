using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World8Sky : MonoBehaviour {

	public Transform volcano;
	public Transform volcanoStartPos;
	public Transform volcanoEndPos;
	public int stageNum;
	public Transform gate;
	public Transform gateStartPos;
	public Transform gateEndPos;

	void Start () {
		stageNum = GameMaster.gameMaster.stageNum;
		volcano.position = Vector3.Lerp (volcanoStartPos.position, volcanoEndPos.position, (float)stageNum / 14.0f);
		if (PlayerPrefs.GetInt ("Ended") == 0) {
			gate.position = Vector3.Lerp (gateStartPos.position, gateEndPos.position, (float)stageNum / 14.0f);
		} else {
			Destroy (gate.gameObject);
		}
	}
}
