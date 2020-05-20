using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour {
	
	private float dist;
	private Stage stage;
	RaycastHit2D rayCastHit2D;
	private Transform myTransform;
	[SerializeField] private GameObject mirror;
	[SerializeField] private GameObject receiver;
	public bool colliderTriggered;
	public BoxCollider2D boxCol;

	public enum FacingDirection
	{
		UP,
		DOWN,
		RIGHT,
		LEFT,
	}

	public FacingDirection facingDirection;

	// Use this for initialization
	void Start () {
		myTransform = this.transform;
		stage = myTransform.parent.parent.parent.GetComponent<Stage> ();
		if (stage.layerNum != 0) {
			colliderTriggered = true;
		}
		boxCol = GetComponent<BoxCollider2D> ();
		StartCoroutine (ShootRayCoroutine ());
	}

	void SetColliderTrigger (bool _bool) {
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

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player" && GameMaster.gameMaster.player.GetComponent<Player> ().movable) {
			GameMaster.gameMaster.Dead ();
		}
	}

	IEnumerator ShootRayCoroutine () {
		while (true) {
			if (stage.rayOn) {
				SetColliderTrigger (true);
				rayCastHit2D = Physics2D.Raycast (myTransform.position, myTransform.up, 22, 1 << LayerMask.NameToLayer ("Block"));
				if (rayCastHit2D.transform.GetComponent<Mirror> () != null) {
					mirror = rayCastHit2D.transform.gameObject;
					switch (facingDirection) {
					case FacingDirection.UP:
						mirror.GetComponent<Mirror> ().MirrorRay (Mirror.RayDirection.DOWN);
						break;
					case FacingDirection.DOWN:
						mirror.GetComponent<Mirror> ().MirrorRay (Mirror.RayDirection.UP);
						break;
					case FacingDirection.RIGHT:
						mirror.GetComponent<Mirror> ().MirrorRay (Mirror.RayDirection.LEFT);
						break;
					case FacingDirection.LEFT:
						mirror.GetComponent<Mirror> ().MirrorRay (Mirror.RayDirection.RIGHT);
						break;
					}
				} else if (rayCastHit2D.transform.GetComponent<ReceiverBlock> () != null) {
					if (facingDirection == FacingDirection.UP && rayCastHit2D.transform.GetComponent<ReceiverBlock> ().facingDirection == ReceiverBlock.FacingDirection.DOWN
					    || facingDirection == FacingDirection.DOWN && rayCastHit2D.transform.GetComponent<ReceiverBlock> ().facingDirection == ReceiverBlock.FacingDirection.UP
					    || facingDirection == FacingDirection.RIGHT && rayCastHit2D.transform.GetComponent<ReceiverBlock> ().facingDirection == ReceiverBlock.FacingDirection.LEFT
					    || facingDirection == FacingDirection.LEFT && rayCastHit2D.transform.GetComponent<ReceiverBlock> ().facingDirection == ReceiverBlock.FacingDirection.RIGHT) {
						receiver = rayCastHit2D.transform.gameObject;
						rayCastHit2D.transform.GetComponent<ReceiverBlock> ().Activate (true);
					}
				} else {
					if (mirror != null) {
						mirror.GetComponent<Mirror> ().DeleteRay ();
						mirror.GetComponent<Mirror> ().addRayRight.SetActive (false);
						mirror.GetComponent<Mirror> ().addRayLeft.SetActive (false);
						mirror = null;
					}
					if (receiver != null) {
						receiver.GetComponent<ReceiverBlock> ().Activate (false);
						receiver = null;
					}
				}
				dist = 3.75f * rayCastHit2D.distance + 1.0f;
				myTransform.localScale = new Vector3 (1, dist, 1);
			} else {
				SetColliderTrigger (false);
			}
			yield return new WaitForSeconds (0.4f);
		}
	}
}
