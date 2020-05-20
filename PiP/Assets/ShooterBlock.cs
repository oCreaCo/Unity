using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBlock : MonoBehaviour {

	public bool startShootBool;
	[SerializeField] private GameObject ray;
	[SerializeField] private Transform raySpawnPoint;
	[SerializeField] private Transform rayUpSpawnPoint;
	[SerializeField] private Transform rayDownSpawnPoint;
	[SerializeField] private Transform rayHorSpawnPoint;
	public GameObject rayObject;
	public bool activated;
	[SerializeField] private Quaternion rot;
	private int rayOrderInLayer;

	public enum FacingDirection
	{
		UP,
		DOWN,
		RIGHT,
		LEFT,
	}

	public FacingDirection facingDirection;

	void Start () {
		rayOrderInLayer = GetComponent<Block> ().orderInLayer - 1;
		switch (facingDirection) {
		case FacingDirection.UP:
			rot = Quaternion.Euler (0, 0, 0);
			raySpawnPoint = rayUpSpawnPoint;
			break;
		case FacingDirection.DOWN:
			rot = Quaternion.Euler (0, 0, -180);
			raySpawnPoint = rayDownSpawnPoint;
			rayOrderInLayer += 3;
			break;
		case FacingDirection.RIGHT:
			rot = Quaternion.Euler (0, 0, -90);
			raySpawnPoint = rayHorSpawnPoint;
			break;
		case FacingDirection.LEFT:
			rot = Quaternion.Euler (0, 0, -270);
			raySpawnPoint = rayHorSpawnPoint;
			break;
		}
		Activate (startShootBool);
	}

	public void Activate (bool _bool) {
		if (_bool) {
			if (!activated) {
				activated = true;
				rayObject = Instantiate (ray, raySpawnPoint.position, rot, this.transform) as GameObject;
				rayObject.GetComponent<SpriteRenderer> ().sortingOrder = rayOrderInLayer;
				switch (facingDirection) {
				case FacingDirection.UP:
					rayObject.GetComponent<Ray> ().facingDirection = Ray.FacingDirection.UP;
					break;
				case FacingDirection.DOWN:
					rayObject.GetComponent<Ray> ().facingDirection = Ray.FacingDirection.DOWN;
					break;
				case FacingDirection.RIGHT:
					rayObject.GetComponent<Ray> ().facingDirection = Ray.FacingDirection.RIGHT;
					break;
				case FacingDirection.LEFT:
					rayObject.GetComponent<Ray> ().facingDirection = Ray.FacingDirection.LEFT;
					break;
				}
			}
		} else {
			if (activated) {
				activated = false;
				Destroy (rayObject);
				rayObject = null;
			}
		}
	}
}
