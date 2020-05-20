using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour {
	
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

	[System.Serializable]
	public struct pathCollider2DStruct
	{
		public PathCollider2D pathCollider2D;
		public bool triggeredBool;
	};

	public pathCollider2DStruct[] pathCollider2DArray;

	void Start () {
		myTransform = this.transform;
		pathCollider2DArray = new pathCollider2DStruct[4];
		upCollider = Instantiate (pathCollider2DObject, upSpawnPoint.position, upSpawnPoint.rotation, myTransform) as GameObject;
		downCollider = Instantiate (pathCollider2DObject, downSpawnPoint.position, downSpawnPoint.rotation, myTransform) as GameObject;
		rightCollider = Instantiate (pathCollider2DObject, rightSpawnPoint.position, rightSpawnPoint.rotation, myTransform) as GameObject;
		leftCollider = Instantiate (pathCollider2DObject, leftSpawnPoint.position, leftSpawnPoint.rotation, myTransform) as GameObject;
		upCollider.GetComponent<PathCollider2D> ().beacon = this;
		pathCollider2DArray [0].pathCollider2D = upCollider.GetComponent<PathCollider2D> ();
		downCollider.GetComponent<PathCollider2D> ().beacon = this;
		pathCollider2DArray [1].pathCollider2D = downCollider.GetComponent<PathCollider2D> ();
		rightCollider.GetComponent<PathCollider2D> ().beacon = this;
		pathCollider2DArray [2].pathCollider2D = rightCollider.GetComponent<PathCollider2D> ();
		leftCollider.GetComponent<PathCollider2D> ().beacon = this;
		pathCollider2DArray [3].pathCollider2D = leftCollider.GetComponent<PathCollider2D> ();
	}

	[SerializeField] private int triggeredCnt = 0;
//	[SerializeField] private PathCollider2D[] pathTemp = new PathCollider2D[3];
	public void Trigger (PathCollider2D _pathCollider2D, bool _bool) {
		Debug.LogError ("Triggered " + _bool);
		if (_bool) {
			for (int i = 0; i < pathCollider2DArray.Length; i++) {
				if (pathCollider2DArray [i].pathCollider2D == _pathCollider2D) {
					pathCollider2DArray [i].triggeredBool = true;
					triggeredCnt++;
					break;
				}
			}
		} else {
			for (int i = 0; i < pathCollider2DArray.Length; i++) {
				if (pathCollider2DArray [i].pathCollider2D == _pathCollider2D) {
					pathCollider2DArray [i].triggeredBool = false;
					triggeredCnt--;
					break;
				}
			}
		}
		if (triggeredCnt > 0) {
			triggered = true;
			spriteRenderer.color = new Color32 (0, 255, 171, 255);
			for (int i = 0; i < pathCollider2DArray.Length; i++) {
				if (!pathCollider2DArray [i].triggeredBool && pathCollider2DArray [i].pathCollider2D != _pathCollider2D) {
					pathCollider2DArray [i].pathCollider2D.Trigger (true);
				}
			}
		} else {
			triggered = false;
			spriteRenderer.color = new Color32 (75, 14, 14, 255);
			for (int i = 0; i < pathCollider2DArray.Length; i++) {
				if (!pathCollider2DArray [i].triggeredBool && pathCollider2DArray [i].pathCollider2D != _pathCollider2D) {
					pathCollider2DArray [i].pathCollider2D.Trigger (false);
				}
			}
		}
	}
}
