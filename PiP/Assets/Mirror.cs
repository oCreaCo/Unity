using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {

	[SerializeField] private GameObject ray;
	public GameObject addRayRight, addRayLeft;
	[SerializeField] private Transform raySpawnPoint;
	public GameObject rayObject;
	[SerializeField] private Quaternion rot;
	private int rayOrderInLayer;

	public enum FacingDirection
	{
		LD,
		LU,
		RD,
		RU,
	}

	public enum RayDirection
	{
		UP,
		DOWN,
		RIGHT,
		LEFT,
	}

	public FacingDirection facingDirection;

	public void MirrorRay (RayDirection _rayDirection) {
		rayOrderInLayer = GetComponent<Block> ().orderInLayer - 1;
		if (rayObject == null) {
			switch (facingDirection) {
			case FacingDirection.LD:
				if (_rayDirection == RayDirection.LEFT) {
					rot = Quaternion.Euler (0, 0, -180);
					rayOrderInLayer += 3;
					GameObject rayObjectTemp = Instantiate (ray, raySpawnPoint.position, rot, this.transform) as GameObject;
					rayObject = rayObjectTemp;
					rayObject.GetComponent<Ray> ().facingDirection = Ray.FacingDirection.DOWN;
					rayObject.GetComponent<SpriteRenderer> ().sortingOrder = rayOrderInLayer;
				} else if (_rayDirection == RayDirection.DOWN) {
					rot = Quaternion.Euler (0, 0, -270);
					GameObject rayObjectTemp = Instantiate (ray, raySpawnPoint.position, rot, this.transform) as GameObject;
					rayObject = rayObjectTemp;
					rayObject.GetComponent<Ray> ().facingDirection = Ray.FacingDirection.LEFT;
					rayObject.GetComponent<SpriteRenderer> ().sortingOrder = rayOrderInLayer;
				}
				addRayLeft.GetComponent<SpriteRenderer> ().sortingOrder = GetComponent<Block> ().orderInLayer + 2;
				addRayLeft.SetActive (true);
				break;
			case FacingDirection.LU:
				if (_rayDirection == RayDirection.LEFT) {
					rot = Quaternion.Euler (0, 0, 0);
					GameObject rayObjectTemp = Instantiate (ray, raySpawnPoint.position, rot, this.transform) as GameObject;
					rayObject = rayObjectTemp;
					rayObject.GetComponent<Ray> ().facingDirection = Ray.FacingDirection.UP;
					rayObject.GetComponent<SpriteRenderer> ().sortingOrder = rayOrderInLayer;
				} else if (_rayDirection == RayDirection.UP) {
					rot = Quaternion.Euler (0, 0, -270);
					GameObject rayObjectTemp = Instantiate (ray, raySpawnPoint.position, rot, this.transform) as GameObject;
					rayObject = rayObjectTemp;
					rayObject.GetComponent<Ray> ().facingDirection = Ray.FacingDirection.LEFT;
					rayObject.GetComponent<SpriteRenderer> ().sortingOrder = rayOrderInLayer;
				}
				addRayLeft.GetComponent<SpriteRenderer> ().sortingOrder = GetComponent<Block> ().orderInLayer - 1;
				addRayLeft.SetActive (true);
				break;
			case FacingDirection.RD:
				if (_rayDirection == RayDirection.RIGHT) {
					rot = Quaternion.Euler (0, 0, -180);
					rayOrderInLayer += 3;
					GameObject rayObjectTemp = Instantiate (ray, raySpawnPoint.position, rot, this.transform) as GameObject;
					rayObject = rayObjectTemp;
					rayObject.GetComponent<Ray> ().facingDirection = Ray.FacingDirection.DOWN;
					rayObject.GetComponent<SpriteRenderer> ().sortingOrder = rayOrderInLayer;
				} else if (_rayDirection == RayDirection.DOWN) {
					rot = Quaternion.Euler (0, 0, -90);
					GameObject rayObjectTemp = Instantiate (ray, raySpawnPoint.position, rot, this.transform) as GameObject;
					rayObject = rayObjectTemp;
					rayObject.GetComponent<Ray> ().facingDirection = Ray.FacingDirection.RIGHT;
					rayObject.GetComponent<SpriteRenderer> ().sortingOrder = rayOrderInLayer;
				}
				addRayRight.GetComponent<SpriteRenderer> ().sortingOrder = GetComponent<Block> ().orderInLayer + 2;
				addRayRight.SetActive (true);
				break;
			case FacingDirection.RU:
				if (_rayDirection == RayDirection.RIGHT) {
					rot = Quaternion.Euler (0, 0, 0);
					GameObject rayObjectTemp = Instantiate (ray, raySpawnPoint.position, rot, this.transform) as GameObject;
					rayObject = rayObjectTemp;
					rayObject.GetComponent<Ray> ().facingDirection = Ray.FacingDirection.UP;
					rayObject.GetComponent<SpriteRenderer> ().sortingOrder = rayOrderInLayer;
				} else if (_rayDirection == RayDirection.UP) {
					rot = Quaternion.Euler (0, 0, -90);
					GameObject rayObjectTemp = Instantiate (ray, raySpawnPoint.position, rot, this.transform) as GameObject;
					rayObject = rayObjectTemp;
					rayObject.GetComponent<Ray> ().facingDirection = Ray.FacingDirection.RIGHT;
					rayObject.GetComponent<SpriteRenderer> ().sortingOrder = rayOrderInLayer;
				}
				addRayRight.GetComponent<SpriteRenderer> ().sortingOrder = GetComponent<Block> ().orderInLayer - 1;
				addRayRight.SetActive (true);
				break;
			}
		}
	}

	public void DeleteRay () {
		Destroy (rayObject);
		rayObject = null;
	}
}
