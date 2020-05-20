using UnityEngine;
using System.Collections;

public class MBasicAttack : MonoBehaviour {

	[SerializeField] int moveSpeed = 10;
	[SerializeField] public int damage = 10;
//	public int plusDamage;
	public GameObject particle;

	public void Damage (int _dmg, int _plusDamage)
	{
		damage = (2 * _dmg) + _plusDamage;
	}

	void Awake () {
		Destroy (this.gameObject, 5f);
	}

	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
	}

	void OnTriggerEnter2D (Collider2D other) {

		GameObject particleEffect = Instantiate (particle, this.transform.position, this.transform.rotation) as GameObject;
		Destroy (particleEffect.gameObject, 1f);

		if (other.GetComponent<Monster_Basic> () != null) {
			Magician.magician.GetComponent<AudioController> ().PlaySound ("MBasicAttack");
			other.GetComponent<Monster_Basic> ().DamageMonster (damage);
			Destroy (this.gameObject);
		}
		if (other.tag == "End") Destroy (this.gameObject);
	}
}
