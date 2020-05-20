using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	[SerializeField] private float fireRate = 0;
	[SerializeField] private LayerMask whatToHit;


	float timeToFire = 0;
	Transform firePoint;

	[SerializeField] Transform bulletPrafab;
	[SerializeField] Transform MuzzleFlashPrefab;
	float timeToSpawnEffect = 0;
	[SerializeField] float effectSpawnRate = 10;

	// Use this for initialization
	void Awake () {
		firePoint = transform.FindChild ("FirePoint");
		if (firePoint == null) {
			Debug.LogError ("No 'FirePoint' object");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (fireRate == 0) {
			if (Input.GetButtonDown ("Fire1")) {
				Shoot ();
			}
		} else {
			if (Input.GetButton ("Fire1") && Time.time > timeToFire){
				timeToFire = Time.time + 1/fireRate;
				Shoot ();
			}
		}
	}

	void Shoot () {
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
		if (Time.time >= timeToSpawnEffect) {
			Effect ();
			timeToSpawnEffect = Time.time + 1/effectSpawnRate;
		}
		Debug.DrawLine (firePointPosition, mousePosition);
		if (hit.collider != null) {
			Debug.DrawLine (firePointPosition, hit.point, Color.red);
		}
	}

	void Effect () {
		Instantiate (bulletPrafab, firePoint.position, firePoint.rotation);
		Transform clone = Instantiate (MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
		clone.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, 0);
		Destroy (clone.gameObject, 0.03f);
	}
}
