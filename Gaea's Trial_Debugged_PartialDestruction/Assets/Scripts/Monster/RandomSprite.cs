using UnityEngine;
using System.Collections;

public class RandomSprite : MonoBehaviour {

	public Sprite[] sprites;
	public GameObject sprite;
	private int i;
	
	void Awake () {
		i = Random.Range (0, sprites.Length);
		sprite.GetComponent<SpriteRenderer> ().sprite = sprites [i];
	}
}
