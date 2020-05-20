using UnityEngine;
using System.Collections;

public class ARayCast : MonoBehaviour {

	public bool sky;
	public bool hit = false;
	public Vector2 monsterVector2;
	public Vector2 thisVectror2;
	public float monsterXPosition = 0;
	public Transform frontMonster;
	public float distance = 8;

	void Update () {
		thisVectror2 = new Vector2 (transform.position.x, transform.position.y);
		Ray2D magicianShootingRay = new Ray2D (thisVectror2, Vector2.right);
		RaycastHit2D monster = Physics2D.Raycast (thisVectror2, Vector2.right, distance, 1 << LayerMask.NameToLayer("Monster"));

		if (monster) {
			hit = true;
			monsterVector2 = monster.transform.FindChild ("MagicPoint").position;
			monsterXPosition = monster.transform.position.x;
			frontMonster = monster.transform;
		} else {
			hit = false;
		}
	}
}
