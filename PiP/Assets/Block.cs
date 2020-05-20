using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	public int x, y;
	public float outX, outY;
	public int gridStateNum;
	public int orderInLayer;
	public SpriteRenderer sprite;
	public bool inPuzzle;
	public bool horMovable, verMovable;
	private bool colliderTriggered;
	public BoxCollider2D boxCol;

	void Start () {
		colliderTriggered = true;
		boxCol = GetComponent<BoxCollider2D> ();
	}

	public void SetOrderInLayer (int _layerNum) {
		orderInLayer = _layerNum;
		sprite.sortingOrder = _layerNum;
	}

	public void AddSubOrderInLayer (bool _bool) {
		if (_bool) {
			orderInLayer += 5;
			if (GetComponent<ShooterBlock> () != null) {
				GetComponent<ShooterBlock> ().rayObject.GetComponent<SpriteRenderer> ().sortingOrder += 5;
			} else if (GetComponent<Mirror> () != null) {
				if (GetComponent<Mirror> ().rayObject != null) {
					GetComponent<Mirror> ().rayObject.GetComponent<SpriteRenderer> ().sortingOrder += 5;
				}
				GetComponent<Mirror> ().addRayLeft.GetComponent<SpriteRenderer> ().sortingOrder += 5;
				GetComponent<Mirror> ().addRayRight.GetComponent<SpriteRenderer> ().sortingOrder += 5;
			} else if (GetComponent<ReceiverBlock> () != null) {
				GetComponent<ReceiverBlock> ().AddSubOrderInLayer (_bool);
			} else if (GetComponent<ActivationBlock> () != null) {
				GetComponent<ActivationBlock> ().AddSubOrderInLayer (_bool);
			}
		} else {
			orderInLayer -= 5;
			if (GetComponent<ShooterBlock> () != null) {
				GetComponent<ShooterBlock> ().rayObject.GetComponent<SpriteRenderer> ().sortingOrder -= 5;
			} else if (GetComponent<Mirror> () != null) {
				if (GetComponent<Mirror> ().rayObject != null) {
					GetComponent<Mirror> ().rayObject.GetComponent<SpriteRenderer> ().sortingOrder -= 5;
				}
				GetComponent<Mirror> ().addRayLeft.GetComponent<SpriteRenderer> ().sortingOrder -= 5;
				GetComponent<Mirror> ().addRayRight.GetComponent<SpriteRenderer> ().sortingOrder -= 5;
			} else if (GetComponent<ReceiverBlock> () != null) {
				GetComponent<ReceiverBlock> ().AddSubOrderInLayer (_bool);
			} else if (GetComponent<ActivationBlock> () != null) {
				GetComponent<ActivationBlock> ().AddSubOrderInLayer (_bool);
			}
		}
		sprite.sortingOrder = orderInLayer;
	}

	public void SetColliderTrigger (bool _bool) {
		if (_bool) {
			if (!colliderTriggered) {
				colliderTriggered = true;
				boxCol.enabled = true;
			}
		} else {
			if (colliderTriggered) {
				colliderTriggered = false;
				boxCol.enabled = false;
			}
		}
	}
}
