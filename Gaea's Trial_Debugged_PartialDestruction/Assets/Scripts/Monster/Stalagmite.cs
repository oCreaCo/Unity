using UnityEngine;
using System.Collections;

public class Stalagmite : MonoBehaviour {

	public Cavian cavian;
	private Transform head;
	public Animator anim;
	private Rigidbody2D rb;
	
	public void Awake () {
		cavian = FindObjectOfType<Cavian>();
		head = cavian.transform.FindChild ("Animator").FindChild ("Graphic").FindChild ("Body");
		rb = GetComponent<Rigidbody2D> ();
		cavian.stalagmiteCount++;
		cavian.stalagmites.Add (this.gameObject);
		anim = GetComponent<Animator> ();
	}

	public void ListRemove() {
		cavian.stalagmites.Remove (this.gameObject);
		cavian.stalagmiteCount--;
	}

	public void Move () {
		rb.AddForce (head.position - this.transform.position, ForceMode2D.Impulse);
		GameMaster.gameMaster.remainingEnemies.Remove (this.gameObject);
		Destroy (this.gameObject, 1f);
	}
}
