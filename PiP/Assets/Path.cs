using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

	public Sprite line, curve, t, cross;
	public SpriteRenderer spriteRenderer;
	private bool triggered;
	private Transform myTransform;

	[SerializeField] private GameObject pathCollider2DObject;
	[SerializeField] private Transform upSpawnPoint;
	[SerializeField] private Transform downSpawnPoint;
	[SerializeField] private Transform rightSpawnPoint;
	[SerializeField] private Transform leftSpawnPoint;

	private GameObject upCollider;
	private GameObject downCollider;
	private GameObject rightCollider;
	private GameObject leftCollider;

	[SerializeField] private Quaternion rot;

	public enum PathType
	{
		LINE,
		CURVE,
		T,
		CROSS,
	}

	public PathType pathType;

	public enum FacingDirection
	{
		UP,
		DOWN,
		RIGHT,
		LEFT,
	}

	public FacingDirection facingDirection;

	[System.Serializable]
	public struct pathCollider2DStruct
	{
		public PathCollider2D pathCollider2D;
		public bool triggeredBool;
	};

	public pathCollider2DStruct[] pathCollider2DArray;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		myTransform = this.transform;
		switch (facingDirection) {
		case FacingDirection.UP:
			rot = Quaternion.Euler (0, 0, 0);
			break;
		case FacingDirection.DOWN:
			rot = Quaternion.Euler (0, 0, -180);
			break;
		case FacingDirection.RIGHT:
			rot = Quaternion.Euler (0, 0, -90);
			break;
		case FacingDirection.LEFT:
			rot = Quaternion.Euler (0, 0, -270);
			break;
		}
		switch (pathType) {
		case PathType.LINE:
			spriteRenderer.sprite = line;
			pathCollider2DArray = new pathCollider2DStruct[2];
			rightCollider = Instantiate (pathCollider2DObject, rightSpawnPoint.position, rightSpawnPoint.rotation, myTransform) as GameObject;
			leftCollider = Instantiate (pathCollider2DObject, leftSpawnPoint.position, leftSpawnPoint.rotation, myTransform) as GameObject;
			rightCollider.GetComponent<PathCollider2D> ().path = this;
			pathCollider2DArray [0].pathCollider2D = rightCollider.GetComponent<PathCollider2D> ();
			leftCollider.GetComponent<PathCollider2D> ().path = this;
			pathCollider2DArray [1].pathCollider2D = leftCollider.GetComponent<PathCollider2D> ();
			break;
		case PathType.CURVE:
			spriteRenderer.sprite = curve;
			pathCollider2DArray = new pathCollider2DStruct[2];
			upCollider = Instantiate (pathCollider2DObject, upSpawnPoint.position, upSpawnPoint.rotation, myTransform) as GameObject;
			rightCollider = Instantiate (pathCollider2DObject, rightSpawnPoint.position, rightSpawnPoint.rotation, myTransform) as GameObject;
			upCollider.GetComponent<PathCollider2D> ().path = this;
			pathCollider2DArray [0].pathCollider2D = upCollider.GetComponent<PathCollider2D> ();
			rightCollider.GetComponent<PathCollider2D> ().path = this;
			pathCollider2DArray [1].pathCollider2D = rightCollider.GetComponent<PathCollider2D> ();
			break;
		case PathType.T:
			spriteRenderer.sprite = t;
			pathCollider2DArray = new pathCollider2DStruct[3];
			downCollider = Instantiate (pathCollider2DObject, downSpawnPoint.position, downSpawnPoint.rotation, myTransform) as GameObject;
			rightCollider = Instantiate (pathCollider2DObject, rightSpawnPoint.position, rightSpawnPoint.rotation, myTransform) as GameObject;
			leftCollider = Instantiate (pathCollider2DObject, leftSpawnPoint.position, leftSpawnPoint.rotation, myTransform) as GameObject;
			downCollider.GetComponent<PathCollider2D> ().path = this;
			pathCollider2DArray [0].pathCollider2D = downCollider.GetComponent<PathCollider2D> ();
			rightCollider.GetComponent<PathCollider2D> ().path = this;
			pathCollider2DArray [1].pathCollider2D = rightCollider.GetComponent<PathCollider2D> ();
			leftCollider.GetComponent<PathCollider2D> ().path = this;
			pathCollider2DArray [2].pathCollider2D = leftCollider.GetComponent<PathCollider2D> ();
			break;
		case PathType.CROSS:
			spriteRenderer.sprite = cross;
			pathCollider2DArray = new pathCollider2DStruct[4];
			upCollider = Instantiate (pathCollider2DObject, upSpawnPoint.position, upSpawnPoint.rotation, myTransform) as GameObject;
			downCollider = Instantiate (pathCollider2DObject, downSpawnPoint.position, downSpawnPoint.rotation, myTransform) as GameObject;
			rightCollider = Instantiate (pathCollider2DObject, rightSpawnPoint.position, rightSpawnPoint.rotation, myTransform) as GameObject;
			leftCollider = Instantiate (pathCollider2DObject, leftSpawnPoint.position, leftSpawnPoint.rotation, myTransform) as GameObject;
			upCollider.GetComponent<PathCollider2D> ().path = this;
			pathCollider2DArray [0].pathCollider2D = upCollider.GetComponent<PathCollider2D> ();
			downCollider.GetComponent<PathCollider2D> ().path = this;
			pathCollider2DArray [1].pathCollider2D = downCollider.GetComponent<PathCollider2D> ();
			rightCollider.GetComponent<PathCollider2D> ().path = this;
			pathCollider2DArray [2].pathCollider2D = rightCollider.GetComponent<PathCollider2D> ();
			leftCollider.GetComponent<PathCollider2D> ().path = this;
			pathCollider2DArray [3].pathCollider2D = leftCollider.GetComponent<PathCollider2D> ();
			break;
		}
		myTransform.rotation = rot;
	}

	[SerializeField] private int triggeredCnt = 0;
	[SerializeField] private PathCollider2D pathTemp;
	public void Trigger (PathCollider2D _pathCollider2D, bool _bool) {
		Debug.LogError ("Triggered " + _bool);
		if (_bool) {
			for (int i = 0; i < pathCollider2DArray.Length; i++) {
				if (pathCollider2DArray [i].pathCollider2D == _pathCollider2D && !pathCollider2DArray [i].triggeredBool) {
					pathCollider2DArray [i].triggeredBool = true;
					triggeredCnt++;
					break;
				}
			}
		} else {
			for (int i = 0; i < pathCollider2DArray.Length; i++) {
				if (pathCollider2DArray [i].pathCollider2D == _pathCollider2D && pathCollider2DArray [i].triggeredBool) {
					pathCollider2DArray [i].triggeredBool = false;
					triggeredCnt--;
					break;
				}
			}
		}
		if (triggeredCnt == pathCollider2DArray.Length - 1) {
			triggered = true;
			spriteRenderer.color = new Color32 (0, 255, 171, 255);
			for (int i = 0; i < pathCollider2DArray.Length; i++) {
				if (!pathCollider2DArray [i].triggeredBool) {
					pathTemp = pathCollider2DArray [i].pathCollider2D;
					pathTemp.Trigger (true);
					break;
				}
			}
		} else {
			triggered = false;
			if (pathTemp != null) {
				pathTemp.Trigger (false);
				pathTemp = null;
			}
			spriteRenderer.color = new Color32 (65, 65, 65, 255);
		}
	}
}
