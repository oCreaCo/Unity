using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private bool searchingStatus = false;

	[System.Serializable]
	public class PlayerStats {
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



	public PlayerStats playerStats = new PlayerStats();


	public Animator anim;

	[Header("Optional: ")]
	[SerializeField]
	private StatusIndicator_Player statusIndicator;

	void Awake()
	{
		anim = GetComponent<Animator> ();

		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("EnemyBullet"), LayerMask.NameToLayer ("Player"), false);

		if (statusIndicator == null) {
			statusIndicator = GameObject.FindGameObjectWithTag ("Status").GetComponent<StatusIndicator_Player>();
			statusIndicator.SetHealth (playerStats.curHealth, playerStats.maxHealth);
		}
	}

	void Start()
	{
		playerStats.Init ();
		
		if (statusIndicator != null) {
			statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
		}
	}

	public int fallBoundary = -20;

	void Update () {
		if (transform.position.y <= fallBoundary) {
			GameMaster.gm.cameraShake.Shake (playerStats.curHealth);
			DamagePlayer (9999);
		}
	}

	public void DamagePlayer (int damage) {
		playerStats.curHealth -= damage;

		if (playerStats.curHealth <= 0) {
			GameMaster.KillPlayer (this);
		} else {
			TriggerHurt (1f);
		}

		if (statusIndicator != null) {
			statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
		}
	}

	public void TriggerHurt(float hurtTime)
	{
		StartCoroutine (HurtBlinker (hurtTime));
	}

	IEnumerator HurtBlinker(float hurtTime)
	{
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("EnemyBullet"), LayerMask.NameToLayer ("Player"));

		anim.SetLayerWeight (1, 1);

		yield return new WaitForSeconds (hurtTime);

		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("EnemyBullet"), LayerMask.NameToLayer ("Player"), false);

		anim.SetLayerWeight (1, 0);
	}
}
