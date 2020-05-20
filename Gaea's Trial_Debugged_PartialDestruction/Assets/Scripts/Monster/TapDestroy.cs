using UnityEngine;
using System.Collections;

public class TapDestroy : MonoBehaviour {

	public GameObject destroyParticle;

	public int tap;
	[SerializeField] private int count = 0;

	public void TapToDestroy () {
		count ++;
		if (count >= tap) {
			if (destroyParticle != null) {
				GameObject particle = Instantiate (destroyParticle, this.transform.position, this.transform.rotation) as GameObject;
				Destroy (particle, 3f);
			}
			Destroy (this.transform.parent.gameObject);
		}
	}
}
