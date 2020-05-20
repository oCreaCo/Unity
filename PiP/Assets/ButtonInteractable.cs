using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractable : MonoBehaviour {
	
	private RaycastHit2D rayCastHit2DPlayer;
	private RaycastHit2D rayCastHit2DBlock;
	public Sprite pushedSprite, unPushedSprite;
	private float x, y;
	private Vector3 dir;
	private Transform myTransform;
	private BoxCollider2D boxCollider2D;
	public bool pushed;

	[SerializeField] private GameObject pathCollider2DObject;
	[SerializeField] private Transform upSpawnPoint;
	[SerializeField] private Transform downSpawnPoint;
	[SerializeField] private Transform rightSpawnPoint;
	[SerializeField] private Transform leftSpawnPoint;
	private GameObject pathCollider2D1, pathCollider2D2, pathCollider2D3;

	public enum FacingDirection
	{
		UP,
		DOWN,
		RIGHT,
		LEFT,
	}

	void Start () {
		myTransform = this.transform;
		boxCollider2D = GetComponent<BoxCollider2D> ();
		SetDirection ();
		SetCollider ();
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
		StartCoroutine (CheckBlockCoroutine ());
	}

	public void SetDirection () {
		switch (facingDirection) {
		case FacingDirection.UP:
			x = 0;
			y = 1;
			break;
		case FacingDirection.DOWN:
			x = 0;
			y = -1;
			break;
		case FacingDirection.RIGHT:
			x = 1;
			y = 0;
			break;
		case FacingDirection.LEFT:
			x = -1;
			y = 0;
			break;
		}
		dir = new Vector3 (x, y, 0);
	}

	public FacingDirection facingDirection;

	public void Pushed () {
		if (!pushed) {
			boxCollider2D.offset = new Vector2 (0, 0);
			boxCollider2D.size = new Vector2 (1, 1);
			GetComponent<Block> ().sprite.sprite = pushedSprite;
			pathCollider2D1.GetComponent<PathCollider2D> ().Trigger (true);
			pathCollider2D2.GetComponent<PathCollider2D> ().Trigger (true);
			pathCollider2D3.GetComponent<PathCollider2D> ().Trigger (true);
			pushed = true;
		}
	}

	public void UnPushed () {
		if (pushed) {
			SetCollider ();
			GetComponent<Block> ().sprite.sprite = unPushedSprite;
			pathCollider2D1.GetComponent<PathCollider2D> ().Trigger (false);
			pathCollider2D2.GetComponent<PathCollider2D> ().Trigger (false);
			pathCollider2D3.GetComponent<PathCollider2D> ().Trigger (false);
			pushed = false;
		}
	}

	public void SetCollider () {
		switch (facingDirection) {
		case FacingDirection.UP:
			boxCollider2D.offset = new Vector2 (0, 0.2f);
			boxCollider2D.size = new Vector2 (1, 1.4f);
			break;
		case FacingDirection.DOWN:
			boxCollider2D.offset = new Vector2 (0, -0.2f);
			boxCollider2D.size = new Vector2 (1, 1.4f);
			break;
		case FacingDirection.RIGHT:
			boxCollider2D.offset = new Vector2 (0.2f, 0);
			boxCollider2D.size = new Vector2 (1.4f, 1);
			break;
		case FacingDirection.LEFT:
			boxCollider2D.offset = new Vector2 (-0.2f, 0);
			boxCollider2D.size = new Vector2 (1.4f, 1);
			break;
		}
	}

	IEnumerator CheckBlockCoroutine () {
		while (true) {
			rayCastHit2DPlayer = Physics2D.Raycast (myTransform.position, dir, 1.1f, 1 << LayerMask.NameToLayer("Player"));
			rayCastHit2DBlock = Physics2D.Raycast (myTransform.position, dir, 1.1f, 1 << LayerMask.NameToLayer("Block"));
			if (rayCastHit2DPlayer || rayCastHit2DBlock) {
//				if (rayCastHit2D.transform.tag == "Player" || rayCastHit2D.transform.tag == "Block") {
					Pushed ();
//				}
			} else {
				UnPushed ();
			}
			yield return new WaitForSeconds (0.4f);
		}
	}
}
