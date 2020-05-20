using UnityEngine;
using System.Collections;

public class YGridSpawn : MonoBehaviour {
	
	public float stopYGrid;
	public Transform effect;

	void Update () {
		if (this.transform.position.y <= stopYGrid) {
			Instantiate (effect, this.transform.position, this.transform.rotation);
			Destroy (this.gameObject);
		}
	}
}
