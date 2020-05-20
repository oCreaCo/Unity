using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCollider2D : MonoBehaviour {

	public Path path;
	public Beacon beacon;
	public ActivationBlock actiBlock;
	public Teleporter teleporter;
	public PathBlock pathBlock;
	public PathCollider2D collided;
	public bool touched;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Path") {
			touched = true;
			collided = other.GetComponent<PathCollider2D> ();
//			path.Trigger (this, true);
		}
	}

	public void Trigger (bool _bool) {
		Debug.LogError ("PathCollider2D Trigger " + _bool);
		if (collided != null) {
			if (collided.path != null) {
				collided.path.Trigger (collided, _bool);
			} else if (collided.beacon != null) {
				collided.beacon.Trigger (collided, _bool);
			} else if (collided.actiBlock != null) {
				collided.actiBlock.Trigger (_bool);
			} else if (collided.teleporter != null) {
				collided.teleporter.Trigger (_bool);
			} else if (collided.pathBlock != null) {
				collided.pathBlock.Trigger (collided, _bool);
			}
		}
	}
}
