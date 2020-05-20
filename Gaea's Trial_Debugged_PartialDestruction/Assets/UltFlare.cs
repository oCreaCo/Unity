using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltFlare : MonoBehaviour {

	[SerializeField] private SpriteRenderer glow;

	void Start () {
		switch (GetComponent<ColorPiece> ().Color) {
		case ColorPiece.ColorType.RED:
			glow.color = new Color (1f, 0, 0, (float)150/255);
			break;
		case ColorPiece.ColorType.BLUE:
			glow.color = new Color (0, 0, 1f, (float)150/255);
			break;
		case ColorPiece.ColorType.GREEN:
			glow.color = new Color (0, 1f, 0, (float)150/255);
			break;
		case ColorPiece.ColorType.PURPLE:
			glow.color = new Color (1f, 0, 1f, (float)150/255);
			break;
		}
	}
}
