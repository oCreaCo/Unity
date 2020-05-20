using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelParticle : MonoBehaviour {

	[SerializeField] private ParticleSystem particleSystem;

	void Start () {
		switch (GetComponent<ColorPiece> ().Color) {
		case ColorPiece.ColorType.RED:
			particleSystem.startColor = new Color (1f, 0, 0, 1f);
			break;
		case ColorPiece.ColorType.BLUE:
			particleSystem.startColor = new Color (0, 0, 1f, 1f);
			break;
		case ColorPiece.ColorType.GREEN:
			particleSystem.startColor = new Color (0, 1f, 0, 1f);
			break;
		case ColorPiece.ColorType.PURPLE:
			particleSystem.startColor = new Color (1f, 0, 1f, 1f);
			break;
		}
	}
}
