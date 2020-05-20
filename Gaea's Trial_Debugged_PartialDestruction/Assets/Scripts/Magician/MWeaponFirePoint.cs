using UnityEngine;
using System.Collections;

public class MWeaponFirePoint : MonoBehaviour {
	public static MWeaponFirePoint mWeaponFirePoint;
	public Vector2 thisVector2;

	[SerializeField] private int offset = 0;

	// Update is called once per frame
	void Update ()
	{ 
		thisVector2 = new Vector2 (this.transform.position.x, this.transform.position.y);
		if (Grid.grid.skyRayCast.GetComponent<ARayCast>().hit) {
			Vector2 difference = (thisVector2 - Grid.grid.skyRayCast.GetComponent<ARayCast>().monsterVector2);
			difference.Normalize ();

			float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (0f, 0f, rotZ + offset);
		}
		else if (!Grid.grid.skyRayCast.GetComponent<ARayCast>().hit) {
			if (Grid.grid.groundRayCast.GetComponent<ARayCast>().hit) {
				Vector2 difference = (thisVector2 - Grid.grid.groundRayCast.GetComponent<ARayCast>().monsterVector2);
				difference.Normalize ();

				float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler (0f, 0f, rotZ + offset);
			}
		}
	}
}
