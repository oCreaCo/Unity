using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	[SerializeField] int moveSpeed = 10;
	[SerializeField] public int Damage = 10;

	public Transform bloodPrefab;
	
	void Update ()
	{
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
		Destroy (this.gameObject, 1);
	}
}
