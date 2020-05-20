using UnityEngine;
using System.Collections;

public class LerpRotation : MonoBehaviour {

	public Transform from;
	public Transform to;
	public float speed = 0.1F;
	[SerializeField] private int offset = 0;
	float t;

	void Awake () {
		t = Time.time;
		if (from == null) {
			from = this.transform;
		}
		if (to == null) {
			to = Castle.castle.transform;
		}
	}

	void Update() {
		Vector2 difference = (from.position - to.position);
		difference.Normalize ();

		float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
//		to.rotation = Quaternion.Euler (0f, 0f, rotZ + offset);

		transform.rotation = Quaternion.Lerp(from.rotation, Quaternion.Euler (0f, 0f, rotZ + offset), (Time.time - t) * speed);
	}
}
