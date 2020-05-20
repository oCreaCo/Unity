using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	
	[SerializeField] private float x, y;
	[SerializeField] private float width;
	[SerializeField] private float height;
	[SerializeField] private float movingTime;
	[SerializeField] private float t;
	[SerializeField] private Vector2 input;
	[SerializeField] private Vector3 startPos;
	[SerializeField] private Vector3 endPos;
	[SerializeField] private bool moving;
	[SerializeField] private bool pressed;
	[SerializeField] private Transform upBlock;
	[SerializeField] private Transform downBlock;
	[SerializeField] private Transform rightBlock;
	[SerializeField] private Transform leftBlock;
	[SerializeField] private SpriteRenderer sprite;
	[SerializeField] private int orderInLayer;
	public bool movable;

	[SerializeField] private Animator anim;
	private Transform myTransform;

	public Transform blocks;

	public enum MovingDirection
	{
		UP,
		DOWN,
		RIGHT,
		LEFT,
	}

	public enum FacingDirection
	{
		UP,
		DOWN,
		RIGHT,
		LEFT,
	}

	public FacingDirection facingDirection;

	public void BlockNull () {
		if (upBlock != null) {
			upBlock.SetParent (blocks);
			upBlock = null;
		}
		if (downBlock != null) {
			downBlock.SetParent (blocks);
			downBlock = null;
		}
		if (rightBlock != null) {
			rightBlock.SetParent (blocks);
			rightBlock = null;
		}
		if (leftBlock != null) {
			leftBlock.SetParent (blocks);
			leftBlock = null;
		}
	}

	public void CheckBlock () {
		BlockNull ();
		RaycastHit2D upHit = Physics2D.Raycast (myTransform.position, Vector2.up, 1, 1 << LayerMask.NameToLayer("Block"));
		RaycastHit2D downHit = Physics2D.Raycast (myTransform.position, Vector2.down, 1, 1 << LayerMask.NameToLayer("Block"));
		RaycastHit2D rightHit = Physics2D.Raycast (myTransform.position, Vector2.right, 1, 1 << LayerMask.NameToLayer("Block"));
		RaycastHit2D leftHit = Physics2D.Raycast (myTransform.position, Vector2.left, 1, 1 << LayerMask.NameToLayer("Block"));
		if (upHit) {
			upBlock = upHit.transform;
			upHit.transform.SetParent (myTransform);
		}
		if (downHit) {
			downBlock = downHit.transform;
			downHit.transform.SetParent (myTransform);
		}
		if (rightHit) {
			rightBlock = rightHit.transform;
			rightHit.transform.SetParent (myTransform);
		}
		if (leftHit) {
			leftBlock = leftHit.transform;
			leftHit.transform.SetParent (myTransform);
		}
	}

	[SerializeField] private MovingDirection movingDirection;

	public void SetOrderInLayer (int _i) {
		orderInLayer = _i;
		sprite.sortingOrder = orderInLayer;
	}

	public void AddSubOrderInLayer (int _i, bool _bool) {
		if (_bool) {
			orderInLayer += _i;
		} else {
			orderInLayer -= _i;
		}
		sprite.sortingOrder = orderInLayer;
	}

	void Awake () {
		myTransform = this.transform;
		CheckBlock ();
		movable = true;
	}

	void FixedUpdate () {
		
		if (!moving && movable) {
			if (Input.GetButtonDown ("Interact")) {
				switch (facingDirection) {
				case FacingDirection.UP:
					if (upBlock != null && upBlock.GetComponent<Block> ().inPuzzle) {
						upBlock.GetComponent<Block> ().outX = upBlock.GetComponent<Block> ().x;
						upBlock.GetComponent<Block> ().outY = upBlock.GetComponent<Block> ().y - 1;
						upBlock.SetParent (blocks);
						GameMaster.gameMaster.PuzzleIn (upBlock, upBlock.parent.parent);
						upBlock = null;
					}
					break;
				case FacingDirection.DOWN:
					if (downBlock != null && downBlock.GetComponent<Block> ().inPuzzle) {
						downBlock.GetComponent<Block> ().outX = downBlock.GetComponent<Block> ().x;
						downBlock.GetComponent<Block> ().outY = downBlock.GetComponent<Block> ().y + 1;
						downBlock.SetParent (blocks);
						GameMaster.gameMaster.PuzzleIn (downBlock, downBlock.parent.parent);
						downBlock = null;
					}
					break;
				case FacingDirection.RIGHT:
					if (rightBlock != null && rightBlock.GetComponent<Block> ().inPuzzle) {
						rightBlock.GetComponent<Block> ().outX = rightBlock.GetComponent<Block> ().x - 1;
						rightBlock.GetComponent<Block> ().outY = rightBlock.GetComponent<Block> ().y;
						rightBlock.SetParent (blocks);
						GameMaster.gameMaster.PuzzleIn (rightBlock, rightBlock.parent.parent);
						rightBlock = null;
					}
					break;
				case FacingDirection.LEFT:
					if (leftBlock != null && leftBlock.GetComponent<Block> ().inPuzzle) {
						leftBlock.GetComponent<Block> ().outX = leftBlock.GetComponent<Block> ().x + 1;;
						leftBlock.GetComponent<Block> ().outY = leftBlock.GetComponent<Block> ().y;
						leftBlock.SetParent (blocks);
						GameMaster.gameMaster.PuzzleIn (leftBlock, leftBlock.parent.parent);
						leftBlock = null;
					}
					break;
				}
			} else if (Input.GetButtonDown ("R")) {
				GameMaster.gameMaster.Pause ();
			} else if (Input.GetButtonDown ("Horizontal")) {
				if (Input.GetAxis ("Horizontal") > 0) {
					facingDirection = FacingDirection.RIGHT;
					anim.SetTrigger ("FacingRight");
				} else if (Input.GetAxis ("Horizontal") < 0) {
					facingDirection = FacingDirection.LEFT;
					anim.SetTrigger ("FacingLeft");
				}
				CheckBlock ();
			} else if (Input.GetButtonDown ("Vertical")) {
				if (Input.GetAxis ("Vertical") > 0) {
					facingDirection = FacingDirection.UP;
					anim.SetTrigger ("FacingUp");
				} else if (Input.GetAxis ("Vertical") < 0) {
					facingDirection = FacingDirection.DOWN;
					anim.SetTrigger ("FacingDown");
				}
				CheckBlock ();
			} else {
				if (Input.GetAxis ("Horizontal") > 0.5f) {
					x = width;
				} else if (Input.GetAxis ("Horizontal") < -0.5f) {
					x = width * -1f;
				} else {
					x = 0;
				}
				if (Input.GetAxis ("Vertical") > 0.5f) {
					y = height;
				} else if (Input.GetAxis ("Vertical") < -0.5f) {
					y = height * -1f;
				} else {
					y = 0;
				}
				input = new Vector2 (x, y);
				if (Mathf.Abs (input.x) > Mathf.Abs (input.y)) {
					input.y = 0;
				} else if (Mathf.Abs (input.x) < Mathf.Abs (input.y)) {
					input.x = 0;
				} else if (Mathf.Abs (input.x) == Mathf.Abs (input.y)) {
					input.x = 0;
					input.y = 0;
				}
				if (input != Vector2.zero) {

					if (x > 0) {
						facingDirection = FacingDirection.RIGHT;
						if (rightBlock != null) {
							if (rightBlock != null && rightBlock.GetComponent<Block> ().horMovable) {
								RaycastHit2D rightHit = Physics2D.Raycast (rightBlock.position, Vector2.right, 1, 1 << LayerMask.NameToLayer ("Block"));
								if (rightHit && rightHit.transform.tag == "Block") {
									input.x = 0;
									anim.SetTrigger ("FacingRight");
								} else {
									rightBlock.GetComponent<Block> ().x++;
								}
							} else {
								input.x = 0;
								anim.SetTrigger ("FacingRight");
							}
						}
						if (input.x > 0) {
							if (upBlock != null) {
								upBlock.SetParent (blocks);
								upBlock = null;
							}
							if (downBlock != null) {
								downBlock.SetParent (blocks);
								downBlock = null;
							}
							if (leftBlock != null) {
								leftBlock.SetParent (blocks);
								leftBlock = null;
							}
							anim.SetTrigger ("Right");
						}
					} else if (x < 0) {
						facingDirection = FacingDirection.LEFT;
						if (leftBlock != null) {
							if (leftBlock != null && leftBlock.GetComponent<Block> ().horMovable) {
								RaycastHit2D leftHit = Physics2D.Raycast (leftBlock.position, Vector2.left, 1, 1 << LayerMask.NameToLayer ("Block"));
								if (leftHit && leftHit.transform.tag == "Block") {
									input.x = 0;
									anim.SetTrigger ("FacingLeft");
								} else {
									leftBlock.GetComponent<Block> ().x--;
								}
							} else {
								input.x = 0;
								anim.SetTrigger ("FacingLeft");
							}
						}
						if (input.x < 0) {
							if (upBlock != null) {
								upBlock.SetParent (blocks);
								upBlock = null;
							}
							if (downBlock != null) {
								downBlock.SetParent (blocks);
								downBlock = null;
							}
							if (rightBlock != null) {
								rightBlock.SetParent (blocks);
								rightBlock = null;
							}
							anim.SetTrigger ("Left");
						}
					} else if (y > 0) {
						facingDirection = FacingDirection.UP;
						if (upBlock != null) {
							if (upBlock != null && upBlock.GetComponent<Block> ().verMovable) {
								RaycastHit2D upHit = Physics2D.Raycast (upBlock.position, Vector2.up, 1, 1 << LayerMask.NameToLayer ("Block"));
								if (upHit && upHit.transform.tag == "Block") {
									input.y = 0;
									anim.SetTrigger ("FacingUp");
								} else {
									upBlock.GetComponent<Block> ().AddSubOrderInLayer (false);
									upBlock.GetComponent<Block> ().y++;
								}
							} else {
								input.y = 0;
								anim.SetTrigger ("FacingUp");
							}
						}
						if (input.y > 0) {
							if (downBlock != null) {
								downBlock.SetParent (blocks);
								downBlock = null;
							}
							if (rightBlock != null) {
								rightBlock.SetParent (blocks);
								rightBlock = null;
							}
							if (leftBlock != null) {
								leftBlock.SetParent (blocks);
								leftBlock = null;
							}
							anim.SetTrigger ("Up");
						}
					} else if (y < 0) {
						facingDirection = FacingDirection.DOWN;
						if (downBlock != null) {
							if (downBlock != null && downBlock.GetComponent<Block> ().verMovable) {
								RaycastHit2D downHit = Physics2D.Raycast (downBlock.position, Vector2.down, 1, 1 << LayerMask.NameToLayer ("Block"));
								if (downHit && downHit.transform.tag == "Block") {
									input.y = 0;
									anim.SetTrigger ("FacingDown");
								} else {
									downBlock.GetComponent<Block> ().AddSubOrderInLayer (true);
									downBlock.GetComponent<Block> ().y--;
								}
							} else {
								input.y = 0;
								anim.SetTrigger ("FacingDown");
							}
						}
						if (input.y < 0) {
							if (upBlock != null) {
								upBlock.SetParent (blocks);
								upBlock = null;
							}
							if (rightBlock != null) {
								rightBlock.SetParent (blocks);
								rightBlock = null;
							}
							if (leftBlock != null) {
								leftBlock.SetParent (blocks);
								leftBlock = null;
							}
							anim.SetTrigger ("Down");
						}
					}

					if (input.y > 0) {
						orderInLayer -= 5;
						sprite.sortingOrder = orderInLayer;
					} else if (input.y < 0) {
						orderInLayer += 5;
						sprite.sortingOrder = orderInLayer;
					}

					if (!(input.x == 0 && input.y == 0)) {
						StartCoroutine (MoveCoroutine ());
					}
				}
			}
		}
	}

	IEnumerator MoveCoroutine () {
		moving = true;

		startPos = myTransform.position;
		t = 0;

		endPos = new Vector3 (startPos.x + input.x, startPos.y + input.y, startPos.z);


		while (t < 1.0f * movingTime) {
			t += Time.deltaTime;
			myTransform.position = Vector3.Lerp (startPos, endPos, t / movingTime);
			yield return null;
		}

		CheckBlock ();

		moving = false;

		yield return 0;
	}
}
