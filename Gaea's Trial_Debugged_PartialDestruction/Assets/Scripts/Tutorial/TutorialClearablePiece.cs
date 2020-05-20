using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialClearablePiece : MonoBehaviour {

	public AnimationClip clearAnimation;

	private bool isBeingCleared = false;

	public bool IsBeingCleared {
		get { return isBeingCleared; }
	}

	protected GamePiece piece;

	void Awake() {
		piece = GetComponent<GamePiece> ();
	}

	public void Clear()
	{
		isBeingCleared = true;
		StartCoroutine (ClearCoroutine ());
	}

	private IEnumerator ClearCoroutine()
	{
		Animator animator = GetComponent<Animator> ();

		if (animator) {
			animator.Play (clearAnimation.name);
			this.transform.FindChild("Graphic").GetComponent<DamageIndicator>().DamageText.text = " ";

			yield return new WaitForSeconds (clearAnimation.length);
		}
	}
}
