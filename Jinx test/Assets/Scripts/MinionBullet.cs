using UnityEngine;
using System.Collections;

public class MinionBullet : MonoBehaviour {

	[SerializeField] int moveSpeed = 10;
	[SerializeField] public int Damage = 10;

	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
		Destroy (this.gameObject, 10);
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		Player enemy = collision.collider.GetComponent<Player> ();
		Debug.LogError ("Got");
		if (enemy != null) {
			GameMaster.gm.cameraShake.Shake (Damage);
			enemy.DamagePlayer (Damage);
			Debug.LogError ("Hit");
			Destroy (this.gameObject);
		}
	}
}
