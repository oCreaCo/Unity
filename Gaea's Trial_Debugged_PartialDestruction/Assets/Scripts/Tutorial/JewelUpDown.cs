using UnityEngine;
using System.Collections;

public class JewelUpDown : MonoBehaviour {

	public enum Direction
	{
		UP,
		DOWN,
	}

	public Direction direction;

	void OnMouseEnter()
	{
		if (direction == Direction.DOWN) {
			this.transform.parent.GetComponent<JewelClear> ().downCheck = true;
		}
	}

	void OnMouseExit()
	{
		if (direction == Direction.DOWN) {
			this.transform.parent.GetComponent<JewelClear> ().downCheck = false;
		}
	}

	void OnMouseDown()
	{
		if (direction == Direction.UP) {
			this.transform.parent.GetComponent<JewelClear> ().upCheck = true;
		}
	}

	void OnMouseUp()
	{
		this.transform.parent.GetComponent<JewelClear> ().Clear ();
	}
}
