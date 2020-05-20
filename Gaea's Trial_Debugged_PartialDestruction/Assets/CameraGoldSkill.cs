using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGoldSkill : MonoBehaviour {
	
	public static CameraGoldSkill tapCollider;

	public float tapGaugeAddValue = 7.0f;
	[SerializeField] private Animator cameraAnim;

	public void Awake () {
		CameraGoldSkill.tapCollider = this;
	}

	public void OnMouseDown () {
		GameMaster.gameMaster.tapGauge += tapGaugeAddValue;
		CameraLerpPosition.cameraLerpPosition.tapGauge += tapGaugeAddValue;
		cameraAnim.SetTrigger ("Tap");
	}
}
