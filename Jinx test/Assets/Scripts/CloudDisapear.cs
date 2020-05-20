using UnityEngine;
using System.Collections;

public class CloudDisapear : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		Destroy (this.gameObject, 0.35f);
	}
}
