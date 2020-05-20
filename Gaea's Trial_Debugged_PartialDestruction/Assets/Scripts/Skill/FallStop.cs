using UnityEngine;
using System.Collections;

public class FallStop : MonoBehaviour {

	public float stopYGrid;
	public float shakeAmount;
	public float shakeLength;

	void Update () {
		if (this.transform.position.y <= stopYGrid) {
			this.transform.GetComponent<MoveScript> ().moveSpeedTemp = 0;
			CameraShake.cameraShake.Shake (shakeAmount, shakeLength);
			Destroy (this.GetComponent<FallStop> ());
		}
	}
}
