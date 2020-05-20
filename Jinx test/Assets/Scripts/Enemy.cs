using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int damage = 25;

	bool dead = false;

	public Transform deathSmoke;

	public Transform bloodPrefab;

	Rigidbody2D phys;
	Animator anim;

	[System.Serializable]
	public class EnemyStats {
		public int maxHealth = 100;

		private int _curHealth;
		public int curHealth
		{
			get { return _curHealth; }
			set { _curHealth = Mathf.Clamp (value, 0, maxHealth); }
		}

		public void Init()
		{
			curHealth = maxHealth;
		}
	}

	public EnemyStats stats = new EnemyStats ();

	[Header("Optional: ")]
	[SerializeField]
	private StatusIndicator_Enemy statusIndicator;

	void Awake()
	{
		phys = GetComponent<Rigidbody2D> ();
		anim = transform.FindChild("Graphic").FindChild("Body").GetComponent <Animator> ();
	}

	void Start()
	{
		stats.Init ();

		if (statusIndicator != null) {
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}
	}

	void FixedUpdate()
	{
		
	}

	public void DamageEnemy (int damage) {
		stats.curHealth -= damage;
		if (stats.curHealth <= 0) {
			GetComponent<BoxCollider2D> ().enabled = false;
			GetComponent<EnemyAI> ().enabled = false;
			dead = true;
			anim.SetBool ("isDead", dead);
			phys.gravityScale = 1;
			GameMaster.KillEnemy (this);
		} else {
			TriggerHurt (0.5f);
		}

		if (statusIndicator != null) {
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		Player player = collision.collider.GetComponent<Player> ();
		Bullet bullet = collision.collider.GetComponent<Bullet> ();
		if (player != null) {
			GameMaster.gm.cameraShake.Shake(damage);
			player.DamagePlayer (damage);
			Transform clone = Instantiate (deathSmoke, this.transform.position, this.transform.rotation) as Transform;
			Destroy (clone.gameObject, 1);
			Destroy (this.gameObject);
		}
		if (bullet != null) {
			GameMaster.gm.cameraShake.Shake(bullet.Damage);
			DamageEnemy (bullet.Damage);
			Transform clone = Instantiate (bloodPrefab, bullet.transform.position, bullet.transform.rotation) as Transform;
			Destroy (clone.gameObject, 0.7f);
			Destroy (bullet.gameObject);
		}
	}

	public void TriggerHurt(float hurtTime)
	{
		StartCoroutine(HurtBlinker(hurtTime));
	}

	IEnumerator HurtBlinker(float hurtTime)
	{
		anim.SetLayerWeight (1, 1);

		yield return new WaitForSeconds (hurtTime);

		anim.SetLayerWeight (1, 0);
	}
}
