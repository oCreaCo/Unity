using UnityEngine;
using System.Collections;

public class CopyHero : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		if (Grid.grid.heroPrefab.GetComponent<Hero> ().copyHero == null) {
			Grid.grid.heroPrefab.GetComponent<Hero> ().copyHero = this.transform;
		} else if (Grid.grid.heroPrefab.GetComponent<Hero> ().copyHero != null) {
			Destroy (Grid.grid.heroPrefab.GetComponent<Hero> ().copyHero.gameObject);
			Grid.grid.heroPrefab.GetComponent<Hero> ().copyHero = this.transform;
		}
	}
}
