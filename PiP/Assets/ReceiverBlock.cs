using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverBlock : MonoBehaviour {

	public SpriteRenderer sprite;
	public int orderInLayer;
	[SerializeField] private bool activated;
	private Transform myTransform;

	[SerializeField] private GameObject pathCollider2DObject;
	[SerializeField] private Transform upSpawnPoint;
	[SerializeField] private Transform downSpawnPoint;
	[SerializeField] private Transform rightSpawnPoint;
	[SerializeField] private Transform leftSpawnPoint;
	[SerializeField] private GameObject pathCollider2D1, pathCollider2D2, pathCollider2D3;

	public enum FacingDirection
	{
		UP,
		DOWN,
		RIGHT,
		LEFT,
	}

	public FacingDirection facingDirection;

	void Start () {
		myTransform = this.transform;
		switch (facingDirection) {
		case FacingDirection.UP:
			pathCollider2D1 = Instantiate (pathCollider2DObject, downSpawnPoint.position, downSpawnPoint.rotation, myTransform) as GameObject;
			pathCollider2D2 = Instantiate (pathCollider2DObject, rightSpawnPoint.position, rightSpawnPoint.rotation, myTransform) as GameObject;
			pathCollider2D3 = Instantiate (pathCollider2DObject, leftSpawnPoint.position, leftSpawnPoint.rotation, myTransform) as GameObject;
			break;
		case FacingDirection.DOWN:
			pathCollider2D1 = Instantiate (pathCollider2DObject, upSpawnPoint.position, upSpawnPoint.rotation, myTransform) as GameObject;
			pathCollider2D2 = Instantiate (pathCollider2DObject, rightSpawnPoint.position, rightSpawnPoint.rotation, myTransform) as GameObject;
			pathCollider2D3 = Instantiate (pathCollider2DObject, leftSpawnPoint.position, leftSpawnPoint.rotation, myTransform) as GameObject;
			break;
		case FacingDirection.RIGHT:
			pathCollider2D1 = Instantiate (pathCollider2DObject, leftSpawnPoint.position, leftSpawnPoint.rotation, myTransform) as GameObject;
			pathCollider2D2 = Instantiate (pathCollider2DObject, upSpawnPoint.position, upSpawnPoint.rotation, myTransform) as GameObject;
			pathCollider2D3 = Instantiate (pathCollider2DObject, downSpawnPoint.position, downSpawnPoint.rotation, myTransform) as GameObject;
			break;
		case FacingDirection.LEFT:
			pathCollider2D1 = Instantiate (pathCollider2DObject, upSpawnPoint.position, upSpawnPoint.rotation, myTransform) as GameObject;
			pathCollider2D2 = Instantiate (pathCollider2DObject, downSpawnPoint.position, downSpawnPoint.rotation, myTransform) as GameObject;
			pathCollider2D3 = Instantiate (pathCollider2DObject, rightSpawnPoint.position, rightSpawnPoint.rotation, myTransform) as GameObject;
			break;
		}
	}

	public void Activate (bool _bool) {
		if (_bool) {
			if (activated == false) {
				activated = true;
				pathCollider2D1.GetComponent<PathCollider2D> ().Trigger (true);
				pathCollider2D2.GetComponent<PathCollider2D> ().Trigger (true);
				pathCollider2D3.GetComponent<PathCollider2D> ().Trigger (true);
			}
		} else {
			if (activated == true) {
				activated = false;
				pathCollider2D1.GetComponent<PathCollider2D> ().Trigger (false);
				pathCollider2D2.GetComponent<PathCollider2D> ().Trigger (false);
				pathCollider2D3.GetComponent<PathCollider2D> ().Trigger (false);
			}
		}
	}

	public void SetOrderInLayer (int _layerNum) {
		orderInLayer = _layerNum;
		sprite.sortingOrder = _layerNum;
	}

	public void AddSubOrderInLayer (bool _bool) {
		if (_bool) {
			orderInLayer += 5;
		} else {
			orderInLayer -= 5;
		}
		sprite.sortingOrder = orderInLayer;
	}
}
