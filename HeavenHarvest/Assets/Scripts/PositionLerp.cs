using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLerp : MonoBehaviour {

	[SerializeField] private float startXPosition;
	[SerializeField] private float startYPosition;
	[SerializeField] private float startZPosition;
	[SerializeField] private float endXPosition;
	[SerializeField] private float endYPosition;
	[SerializeField] private float endZPosition;

	public float movingTime;

	public Transform[] path;
	public Transform throwPoint;

	private Vector3 startPos, endPos;

	public bool failed;
	public bool stopped;

	[SerializeField] private GameObject crankTic;
	[SerializeField] private float lengthAddAmount;
	[SerializeField] private float length;

	void Start () {
		startPos = new Vector3 (startXPosition, startYPosition, startZPosition);
		endPos = new Vector3 (endXPosition, endYPosition, endZPosition);
	}

	public IEnumerator LerpCoroutine () {
		length = startPos.x;
		failed = false;
		this.transform.position = startPos;
		for (float t = 0; t < 1 * movingTime; t += Time.deltaTime) {
			if (stopped) {
				break;
			}
			this.transform.position = Vector3.Lerp (startPos, endPos, t / movingTime);
			if (path.Length != 0) {
				for (int i = 0; i < path.Length; i++) {
					float x = throwPoint.position.x + (float)i * ((this.transform.position.x - throwPoint.position.x) / (path.Length - 1));
					float y = path [i].position.y;
					path [i].position = new Vector3 (x, y, 0f);
				}
			}
			if (crankTic != null && transform.position.x > length) {
				Instantiate (crankTic);
				length += lengthAddAmount;
			}
			yield return 0;
		}
		if (!stopped) {
			if (path.Length != 0) {
				ThrowPoint.throwPoint.target.Find("TargetTop").GetComponent<SpriteRenderer>().color =  new Color32 (255, 255, 255, 0);
				ThrowPoint.throwPoint.target.Find("TargetBottom").GetComponent<SpriteRenderer>().color =  new Color32 (255, 255, 255, 0);
				for (int i = 0; i < path.Length; i++) {
					path [i].position = path [i].GetComponent<ParabolaDot> ().startPos;
					path [i].GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 0);
				}
			}
			ThrowPoint.throwPoint.anim.SetTrigger ("Whine");
			failed = true;
			this.transform.position = startPos;
			ThrowPoint.throwPoint.remainingGaugeParticle.Stop ();
		}
	}
}
