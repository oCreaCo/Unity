using UnityEngine;
using System.Collections;

public class MonsterRangeAttack : MonoBehaviour {

	public GameObject projectile;
	public Transform[] projectileSpawnPoint;

	public int damage;
	public int fireCount = 1;
	public float attackDelay;
	public float fireDelay;
	public float fireRateMin;
	public float fireRateMax;
	public float fireRateTemp;
	public float offSet;
	public float attackTime;
	public float cameraShakeMultiplier;
	public bool rangeAttackable = true;
	private Vector2 vector2 = new Vector2 ();

	public enum ProjectileType
	{
		SHOOT,
		THROW,
	};

	public ProjectileType projectileType;

	private Animator anim;

	void Awake() {
		fireRateTemp = Random.Range(fireRateMin, fireRateMax);
		anim = GetComponent<Animator> ();
		attackTime = Time.time + offSet + fireRateTemp;
		vector2.x = -1;
		vector2.y = 1;
		vector2.Normalize ();
	}

	void Update () {
		if (rangeAttackable && GetComponent<Monster_Basic> ().damageMonsterType == Monster_Basic.DamageMonsterType.BASIC) {
			if (fireRateTemp != 0 && Time.time >= attackTime && !GetComponent<Monster_Basic> ().stoppedBool) {
				anim.SetTrigger ("Attack");
				fireRateTemp = Random.Range (fireRateMin, fireRateMax); 
				attackTime = Time.time + fireRateTemp;
			}
		}
	}

	public void RangeAttack () {
		if (!GetComponent<Monster_Basic> ().dead && !GetComponent<Monster_Basic> ().stoppedBool) {
			StartCoroutine(RangeAttackCoroutine(attackDelay, fireDelay));
		}
	}

	IEnumerator RangeAttackCoroutine (float _attackDelay, float _fireDelay) {
		for (int i = 0; i < projectileSpawnPoint.Length; i++) {
			for (int j = 0; j < fireCount; j++) {
				GameObject eProjectile = Instantiate (projectile, projectileSpawnPoint [i].position, projectileSpawnPoint [i].rotation) as GameObject;
				if (eProjectile.GetComponent<EnemyProjectile> () != null) {
					eProjectile.GetComponent<EnemyProjectile> ().damage = damage;
					eProjectile.GetComponent<EnemyProjectile> ().cameraShakeMultiplier = cameraShakeMultiplier;
				}
				if (projectileType == ProjectileType.THROW) {
					eProjectile.GetComponent<Rigidbody2D> ().AddForce (6 * vector2, ForceMode2D.Impulse);
				}
				yield return new WaitForSeconds (_fireDelay);
			}
			yield return new WaitForSeconds (_attackDelay);
		}
	}
}
