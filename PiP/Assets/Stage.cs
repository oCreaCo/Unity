using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {

	public float startX, startY;
	public Vector3 playerStartPos;

	public Transform blocks;
	public Transform tiles;

	public List<Transform> blockObjects = new List<Transform> ();
	public List<Transform> tileObjects = new List<Transform> ();

	public int layerNum;

	public bool rayOn;

	public void SetColliders (bool _bool) {
		for (int i = 0; i < blockObjects.Count; i++) {
			blockObjects [i].GetComponent<Block> ().SetColliderTrigger (_bool);
		}
	}
}
