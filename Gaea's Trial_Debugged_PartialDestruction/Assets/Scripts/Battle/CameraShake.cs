using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	public static CameraShake cameraShake;

	public Camera mainCam;

	float shakeAmount;
	float amtRate = 0.1f;

	void Awake()
	{
		cameraShake = this;
		if (mainCam == null)
			mainCam = Camera.main;
	}

	public void Shake(float amount, float length)
	{
		shakeAmount = amtRate * amount;
		InvokeRepeating ("BeginShake", 0, shakeAmount);
		Invoke ("StopShake", length);
	}

	void BeginShake()
	{
		if (shakeAmount > 0) {
			Vector3 camPos = mainCam.transform.position;

			float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
			float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
			camPos.x = offsetX;
			camPos.y = offsetY;

			mainCam.transform.position = camPos;
		}
	}

	void StopShake()
	{
		CancelInvoke ("BeginShake");
		mainCam.transform.localPosition = new Vector3 (0, 0, -10);
	}
}
