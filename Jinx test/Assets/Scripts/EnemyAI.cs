using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
[RequireComponent (typeof (SpriteRenderer))]

public class EnemyAI : MonoBehaviour {

	private bool facingRight = true;
	private bool seeingRight = true;
	private bool shootingAnim = false;
	private Animator anim;

	public Transform target;

	[SerializeField]
	Transform firePoint;

	public float updateRate = 2f;

	private Seeker seeker;
	private Rigidbody2D rb;

	//경로
	public Path path;

	//AI 1초당 속도
	public float speed = 1000f;
	private float speedTemp = 1000f;
	public ForceMode2D fMode;

	[HideInInspector]
	public bool pathIsEnded = false;

	//AI의 다음 waypoint까지의 최대거리
	public float nextWaypointDistance = 3;

	//현재 나의 이동방향
	private int currentWaypoint = 0;

	private bool searchingForPlayer = false;

	[SerializeField] private LayerMask notToHit;

	public float timeToStop = 3f;
	public float timeToMove = 3f;
	public float timeToFire = 3f;

	[SerializeField] Transform bulletPrafab;

	float timeToSpawnEffect = 0;

	private void Awake() {
		anim = transform.FindChild("Graphic").FindChild("Body").GetComponent <Animator> ();
	}

	void Start () {
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();

		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchingForPlayer());
			}
			return;
		}

		StartCoroutine (UpdatePath ());
	}

	IEnumerator SearchingForPlayer () {
		GameObject sResult = GameObject.FindGameObjectWithTag ("Player");
		if (sResult == null) {
			yield return new WaitForSeconds (0.5f);
			StartCoroutine (SearchingForPlayer ());
		} 
		else {
			target = sResult.transform;
			searchingForPlayer = false;
			StartCoroutine (UpdatePath ());
			yield return false;
		}
	}

	IEnumerator UpdatePath () {

		seeker.StartPath (transform.position, target.position, OnPathComplete);

		yield return new WaitForSeconds ( 1f/updateRate);
		StartCoroutine (UpdatePath() );

	}

	public void OnPathComplete (Path p) {
		Debug.Log ("경로를 찾았습니다. 오류가 발생했습니까?" + p.error);
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}
		
	void FixedUpdate () {
		anim.SetBool ("IsShoot", shootingAnim);

		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchingForPlayer());
			}
			return;
		}

		if (path == null) {
			return;
		}

		if (currentWaypoint >= path.vectorPath.Count) {
			if (pathIsEnded)
				return;

			Debug.Log ("경로의 끝에 다다랐습니다");
			pathIsEnded = true;
			return;
		}
		pathIsEnded = false;
		Vector3 dlr = (path.vectorPath [currentWaypoint] - transform.position).normalized;
		dlr *= speedTemp * Time.fixedDeltaTime;

		rb.AddForce (dlr, fMode);

		float dist = Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]);
			
		if (dist < nextWaypointDistance) {
				currentWaypoint++;
				return;
		}
	}

	void Update () {
		if (Time.time > timeToStop) {
			timeToStop = Time.time + 5f;
			StopMoving();
		}
		if (Time.time > timeToFire) {
			timeToFire += 4f;
			shootingAnim = false;
			Shoot();
		}
		if (Time.time > timeToMove) {
			timeToMove += 3f;
			speedTemp = speed;
		}
	}

	void Shoot () {
		Vector3 difference = target.position - firePoint.position;
		difference.Normalize ();

		float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		firePoint.rotation = Quaternion.Euler (0f, 0f, rotZ);

		Instantiate (bulletPrafab, firePoint.position, firePoint.rotation);
	}

	void StopMoving () {
		timeToFire = Time.time + 1f;
		timeToMove = Time.time + 2f;
		shootingAnim = true;
		speedTemp = 0f;
	}
}

