using UnityEngine;
using System.Collections;

public class GamePiece : MonoBehaviour {
	public static GamePiece gamepiece;

	private int x, y;
	public int value;
	public bool pressed = false;
	public PieceDebuffType pieceDebuffType;
	[SerializeField] private DamageIndicator damageIndicator;

	public enum PieceDebuffType
	{
		NONE,
		ICE,
		ZLIME,
		PUFFER,
		CHAINED,
	}

	public int X
	{
		get { return x; }
		set {
			if (IsMovable ()) {
				x = value;
			}
		}
	}

	public int Y
	{
		get { return y; }
		set {
			if (IsMovable ()) {
				y = value;
			}
		}
	}

	private Grid.PieceType type;

	public Grid.PieceType Type
	{
		get { return type; }
	}

	private Grid grid;

	public Grid GridRef
	{
		get { return grid; }
	}

	[SerializeField] private MovablePiece movableComponent;

	public MovablePiece MovableComponent
	{
		get { return movableComponent; }
	}

	private ColorPiece colorComponent;

	public ColorPiece ColorComponent
	{
		get { return colorComponent; }
	}

	private ClearablePiece clearableComponent;

	public ClearablePiece ClearableComponent {
		get { return clearableComponent; }
	}

	void Awake() {
		movableComponent = GetComponent<MovablePiece> ();
		colorComponent = GetComponent<ColorPiece> ();
		clearableComponent = GetComponent<ClearablePiece> ();
	}

	// Use this for initialization
	void Start () {
		if (damageIndicator != null) {
			damageIndicator.SetDamage (value);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Init(int _x, int _y, Grid _grid, Grid.PieceType _type, int _Value, PieceDebuffType _pieceDebuffType)
	{
		x = _x;
		y = _y;
		grid = _grid;
		type = _type;
		value = _Value;
		pieceDebuffType = _pieceDebuffType;
	}

	void OnMouseEnter()
	{
		if (GameMaster.gameMaster.isClickable) {
			grid.EnterPiece (this);
		}
//		Debug.LogError ("MouseEnter");
	}

	void OnMouseDown()
	{
		if (GameMaster.gameMaster.isClickable) {
			pressed = true;
			grid.PressPiece (this);
//			Debug.LogError ("MouseDown");
		}
	}

	void OnMouseUp()
	{
		if (!GameMaster.gameMaster.isUI) {
			if (Grid.grid.pressedPiece != null && Grid.grid.enteredPiece.IsMovable () && Grid.grid.pressedPiece.IsMovable ()) {
				GameMaster.gameMaster.IsClickable (false);
				//Debug.LogError ("IsClickable is false.");
				pressed = false;
				grid.ReleasePiece ();
//				Debug.LogError ("MouseUp");
			}
		}
	}

	public bool IsMovable() {
		if (movableComponent != null) {
			if (movableComponent.enabled) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	public bool IsColored() {
		return colorComponent != null;
	}

	public bool IsClearable()
	{
		if (clearableComponent != null) {
			if (clearableComponent.enabled) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
}
