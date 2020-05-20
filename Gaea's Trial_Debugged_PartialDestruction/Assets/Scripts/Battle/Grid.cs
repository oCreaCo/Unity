using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Grid : MonoBehaviour {
	public static Grid grid;
	[SerializeField] private GameMaster gameMaster;
	[SerializeField] private Castle castle;

	[SerializeField]
	private BattleTutorial battleTutorial;

	public enum PieceType
	{
		EMPTY,
		NORMAL,
		VER_CLEAR,
		HOR_CLEAR,
		ULTIMATE,
		STATIC,
		COUNT,
	};

	[System.Serializable]
	public struct Pieceprefab
	{
		public PieceType type;
		public GameObject prefab;
	};

	public int xDim, yDim;
	public int minGold;
	public int maxGold;
	public int heroLevelMinDamage, heroLevelMaxDamage;
	public float fillTime;
	public float fillTime2;
	public bool started = false;
	public bool onceClicked = false;
	private bool needsRefill;
	public Transform heroPrefab;
	public Transform hWeapon;
	public Transform mWeapon;
	public Transform groundRayCast;
	public Transform skyRayCast;

	public Pieceprefab[] piecePrefabs;
	public GameObject backgroundPrefab;
	public Transform skyObject;
	public GameObject backGroundObject;
	public GameObject[] skyObjects;
	public Sprite[] backGroundSprites;

	private Dictionary<PieceType, GameObject> piecePrefabDict;

	public GamePiece[,] pieces;

	private bool inverse = false;
	[SerializeField] private bool firstClear;

	public GamePiece pressedPiece;
	public GamePiece enteredPiece;
	public GamePiece pressedPieceTemp;
	public GamePiece enteredPieceTemp;

	public bool debugCheck = false;

	public bool allUltClear;

	void Awake () {
		heroLevelMinDamage = PlayerPrefs.GetInt ("HeroLevelMinDamage");
		heroLevelMaxDamage = PlayerPrefs.GetInt ("HeroLevelMaxDamage");
	}

	public void Start () {
		grid = this;
		fillTime2 = fillTime;
		minGold = WaveSpawner.waveSpawner.minGold;
		maxGold = WaveSpawner.waveSpawner.maxGold;
		gameMaster.combo = 0;
		piecePrefabDict = new Dictionary<PieceType, GameObject> ();
		//Debug.LogError ("1");

		for (int i = 0; i < piecePrefabs.Length; i++) {
			if (!piecePrefabDict.ContainsKey (piecePrefabs [i].type)) {
				piecePrefabDict.Add (piecePrefabs [i].type, piecePrefabs [i].prefab);
			}
		}
		//Debug.LogError ("2");
		if (PlayerPrefs.GetInt ("worldNum") != 0) {
			backGroundObject.GetComponent<SpriteRenderer> ().sprite = backGroundSprites [PlayerPrefs.GetInt ("worldNum") - 1];
			Instantiate (skyObjects [PlayerPrefs.GetInt ("worldNum") - 1], skyObject.position, skyObject.rotation, skyObject);
		} else if (PlayerPrefs.GetInt ("worldNum") == 0) {
			int r = Random.Range (0, 6);
			backGroundObject.GetComponent<SpriteRenderer> ().sprite = backGroundSprites [r];
			Instantiate (skyObjects [r], skyObject.position, skyObject.rotation, skyObject);
		}

		Hero.hero.InstantiateHero (PlayerPrefs.GetInt ("GridEquippedHero"));
		HeroWeapon.heroWeapon.InstantiateHWeapon(PlayerPrefs.GetInt ("GridEquippedHWeapon"));
		MagicianWeapon.magicianWeapon.InstantiateMWeapon(PlayerPrefs.GetInt ("GridEquippedMWeapon"));
	}

	public void GridStart () {
		for (int x = 0; x < xDim; x++) {
			for (int y = 0; y < yDim; y++) {
				GameObject background = (GameObject)Instantiate (backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
				background.transform.parent = transform;
			}
		}

		//Debug.LogError ("3");
		pieces = new GamePiece[xDim, yDim];
		for (int x = 0; x < xDim; x++) {
			for (int y = 0; y < yDim; y++) {
				SpawnNewPiece (x, y, PieceType.EMPTY, 0, GamePiece.PieceDebuffType.NONE);
			}
		}
		//Debug.LogError ("4");
		StartCoroutine(Fill ());
		//Debug.LogError ("5");
	}

	public IEnumerator Fill()
	{
		needsRefill = true;
		//Debug.LogError ("6");
		while (needsRefill) {
			yield return new WaitForSeconds (fillTime2);
			//Debug.LogError ("7");
			while (FillStep ()) {
				inverse = !inverse;
				yield return new WaitForSeconds (fillTime2);
			}
			//Debug.LogError ("8");
			needsRefill = ClearAllValidMatches ();
//			yield return new WaitForSeconds (fillTime2);
			if (!needsRefill) {
				gameMaster.IsClickable (true);
				onceClicked = false;
			}
			//Debug.LogError ("9");
		}
		if (!started) {
			started = true;
			if (PlayerPrefs.GetInt ("FirstBattle") == 0) {
				//Time.timeScale = 0;
				//battleTutorial.gameObject.SetActive (true);
				PlayerPrefs.SetInt ("FirstBattle", 1);
			}
			yield return new WaitForSeconds (1f);
			WaveSpawner.waveSpawner.WaveStart ();
		}
	}

	public bool FillStep()
	{
		bool movedPiece = false;
		//Debug.LogError ("10");
		for (int y = yDim - 2; y >= 0; y--) {
			for (int loopX = 0; loopX < xDim; loopX++) {
				int x = loopX;

				if (inverse) {
					x = xDim - 1 - loopX;
				}
				//Debug.LogError ("11");
				GamePiece piece = pieces [x, y];

				if (piece.IsMovable ()) {
					GamePiece pieceBelow = pieces [x, y + 1];

					if (pieceBelow.Type == PieceType.EMPTY) {
						piece.MovableComponent.Move (x, y + 1, fillTime2);
						if (pieceBelow.gameObject != null) {
							Destroy (pieceBelow.gameObject);
						}
						pieces [x, y + 1] = piece;
						SpawnNewPiece (x, y, PieceType.EMPTY, 0, GamePiece.PieceDebuffType.NONE);
						movedPiece = true;
					} else {
						for (int diag = -1; diag <= 1; diag++) {
							if (diag != 0) {
								int diagX = x + diag;

								if (inverse) {
									diagX = x - diag;
								}

								if (diagX >= 0 && diagX < xDim) {
									GamePiece diagonalPiece = pieces [diagX, y + 1];

									if (diagonalPiece.Type == PieceType.EMPTY) {
										bool hasPieceAbove = true;

										for (int aboveY = y; aboveY >= 0; aboveY--) {
											GamePiece pieceAbove = pieces [diagX, aboveY];

											if (pieceAbove.IsMovable ()) {
												break;
											} else if (!pieceAbove.IsMovable() && pieceAbove.Type != PieceType.EMPTY) {
												hasPieceAbove = false;
												break;
											}
										}

										if (!hasPieceAbove) {
											Destroy (diagonalPiece.gameObject);
											piece.MovableComponent.Move (diagX, y + 1, fillTime2);
											pieces [diagX, y + 1] = piece;
											SpawnNewPiece (x, y, PieceType.EMPTY, 0, GamePiece.PieceDebuffType.NONE);
											movedPiece = true;
											break;
										}
									}
								}
							}
						}
					}
				}
				//Debug.LogError ("12");
			}
		}
		//Debug.LogError ("13");
		for (int x = 0; x < xDim; x++) {
			GamePiece pieceBelow = pieces [x, 0];

			if (pieceBelow.Type == PieceType.EMPTY) {
				Destroy (pieceBelow.gameObject);
				GameObject newPiece = (GameObject)Instantiate (piecePrefabDict [PieceType.NORMAL], GetWorldPosition (x, -1), Quaternion.identity);
				newPiece.transform.parent = transform;

				pieces [x, 0] = newPiece.GetComponent<GamePiece> ();
				pieces [x, 0].ColorComponent.SetColor ((ColorPiece.ColorType)Random.Range (0, pieces [x, 0].ColorComponent.NumColors));
				if ((pieces [x, 0].ColorComponent.Color == ColorPiece.ColorType.RED)
				    || (pieces [x, 0].ColorComponent.Color == ColorPiece.ColorType.BLUE)
				    || (pieces [x, 0].ColorComponent.Color == ColorPiece.ColorType.PURPLE)) {
					pieces [x, 0].Init (x, -1, this, PieceType.NORMAL, Random.Range (heroLevelMinDamage, heroLevelMaxDamage + 1), GamePiece.PieceDebuffType.NONE);
				} else if (pieces [x, 0].ColorComponent.Color == ColorPiece.ColorType.GREEN) {
					pieces [x, 0].Init (x, -1, this, PieceType.NORMAL, Random.Range (1, 4), GamePiece.PieceDebuffType.NONE);
				} else if (pieces [x, 0].ColorComponent.Color == ColorPiece.ColorType.YELLOW) {
					pieces [x, 0].Init (x, -1, this, PieceType.NORMAL, Random.Range (minGold, maxGold + 1), GamePiece.PieceDebuffType.NONE);
				}
				pieces [x, 0].MovableComponent.Move (x, 0, fillTime2);
				movedPiece = true;
			}
			//Debug.LogError ("14");
		}
		//Debug.LogError ("15");
		return movedPiece;
	}

	public Vector2 GetWorldPosition (int x, int y)
	{
		return new Vector2(transform.position.x - xDim/2.0f + x, transform.position.y + yDim/2.0f - y);
	}

	public GamePiece SpawnNewPiece(int x, int y, PieceType type, int _Dmg, GamePiece.PieceDebuffType _pieceDebuffType)
	{
		GameObject NewPiece = (GameObject)Instantiate (piecePrefabDict [type], GetWorldPosition (x, y), Quaternion.identity);
		NewPiece.transform.parent = transform;

		pieces [x, y] = NewPiece.GetComponent<GamePiece> ();
		pieces [x, y].Init (x, y, this, type, _Dmg, _pieceDebuffType);
		//Debug.LogError ("16");
		return pieces [x, y];
	}

	public bool IsAdjacent(GamePiece piece1, GamePiece piece2)
	{
		//Debug.LogError ("16.1");
		return (piece1.X == piece2.X && (int)Mathf.Abs (piece1.Y - piece2.Y) == 1)
			|| (piece1.Y == piece2.Y && (int)Mathf.Abs (piece1.X - piece2.X) == 1);
	}

	public void SwapPieces(GamePiece piece1, GamePiece piece2)
	{
		//Debug.LogError ("17");
		if (piece1.IsMovable () && piece2.IsMovable ()) {
			pieces [piece1.X, piece1.Y] = piece2;
			pieces [piece2.X, piece2.Y] = piece1;
			//Debug.LogError ("18");
			if (GetMatch (piece1, piece2.X, piece2.Y) != null || GetMatch (piece2, piece1.X, piece1.Y) != null) {
				if (enteredPiece.pieceDebuffType != GamePiece.PieceDebuffType.ZLIME && pressedPiece.pieceDebuffType != GamePiece.PieceDebuffType.ZLIME) {
					gameMaster.StackModifier (false);
				}
//				fillTime2 = fillTime;
//				Debug.LogError ("19");
				int piece1X = piece1.X;
				int piece1Y = piece1.Y;
				piece1.MovableComponent.Move (piece2.X, piece2.Y, fillTime2);
				piece2.MovableComponent.Move (piece1X, piece1Y, fillTime2);
//				Debug.LogError ("20");
				StartCoroutine(ClearAllValidMatchesCoroutine(fillTime2));
//				Debug.LogError ("21");
//				Debug.LogError ("22");
				pressedPieceTemp = pressedPiece;
				enteredPieceTemp = enteredPiece;
				pressedPiece = null;
				enteredPiece = null;
//				Debug.LogError ("23");
				StartCoroutine (Fill ());
//				Debug.LogError ("24");
			} else {
				if (gameMaster.stack < 3) {
					gameMaster.StackModifier (true);
//					fillTime2 += 1.0f;
				}
//				Debug.LogError ("25");
				int piece1X = piece1.X;
				int piece1Y = piece1.Y;

				piece1.MovableComponent.Move (piece2.X, piece2.Y, fillTime2);
				piece2.MovableComponent.Move (piece1X, piece1Y, fillTime2);
//				Debug.LogError ("26");
			}
		}
	}

	IEnumerator ClearAllValidMatchesCoroutine (float filltime2) {
		yield return new WaitForSeconds (filltime2);
		ClearAllValidMatches ();
	}

	public void PressPiece(GamePiece piece)
	{
		if (piece.pieceDebuffType != GamePiece.PieceDebuffType.ICE && piece.pieceDebuffType != GamePiece.PieceDebuffType.CHAINED) {
			pressedPiece = piece;
			enteredPiece = piece;
		}
		//Debug.LogError ("27");
	}

	public void EnterPiece(GamePiece piece)
	{
		if (pressedPiece != null) {
			if (piece.X > pressedPiece.X && piece.Y == pressedPiece.Y) {
				enteredPiece = pieces [pressedPiece.X + 1, pressedPiece.Y];
			} else if (piece.X == pressedPiece.X && piece.Y > pressedPiece.Y) {
				enteredPiece = pieces [pressedPiece.X, pressedPiece.Y + 1];
			} else if (piece.X < pressedPiece.X && piece.Y == pressedPiece.Y) {
				enteredPiece = pieces [pressedPiece.X - 1, pressedPiece.Y];
			} else if (piece.X == pressedPiece.X && piece.Y < pressedPiece.Y) {
				enteredPiece = pieces [pressedPiece.X, pressedPiece.Y - 1];
			} else if (piece.X == pressedPiece.X && piece.Y == pressedPiece.Y) {
				enteredPiece = pieces [pressedPiece.X, pressedPiece.Y];
			}
		}
		//Debug.LogError ("28 enteredpiece is" + enteredPiece.X + "," + enteredPiece.Y);
	}

	public void ReleasePiece()
	{
		firstClear = true;
		if (IsAdjacent (pressedPiece, enteredPiece) && (enteredPiece.pieceDebuffType != GamePiece.PieceDebuffType.ICE) && (enteredPiece.pieceDebuffType != GamePiece.PieceDebuffType.CHAINED)) {
			if (enteredPiece.pieceDebuffType == GamePiece.PieceDebuffType.ZLIME || pressedPiece.pieceDebuffType == GamePiece.PieceDebuffType.ZLIME) {
				gameMaster.stack += 3;
				fillTime2 += 3.0f;
			}
			SwapPieces (pressedPiece, enteredPiece);
			pressedPiece = null;
			enteredPiece = null;
		} else if (pressedPiece == enteredPiece) {
			gameMaster.IsClickable(true);
			if (onceClicked == false) {
				if (enteredPiece.Type == PieceType.ULTIMATE) {
					onceClicked = true;
					gameMaster.StackModifier (false);
					GetComponent<AudioController> ().PlaySound ("JewelClear");
					ClearPiece (enteredPiece.X, enteredPiece.Y);
					gameMaster.IsClickable (false);
					gameMaster.ComboAdd ();
					StartCoroutine (Fill ());
					if ((enteredPiece.ColorComponent.Color == ColorPiece.ColorType.RED)) {
						UltClear (enteredPiece);
						hWeapon.gameObject.GetComponent<HeroWeapon> ().hWeaponSpawned.gameObject.GetComponent<HWeapon> ().Attack (gameMaster.redValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					} else if ((enteredPiece.ColorComponent.Color == ColorPiece.ColorType.BLUE)) {
						gameMaster.GetBlueValue (3 * heroLevelMaxDamage - 2 * heroLevelMinDamage);
						heroPrefab.GetComponent<Hero> ().selectedHero.GetComponent<HeroSkill> ().UltSkill (3 * heroLevelMaxDamage - 2 * heroLevelMinDamage + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
						if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
							heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().UltSkill (3 * heroLevelMaxDamage - 2 * heroLevelMinDamage + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
						}
					} else if ((enteredPiece.ColorComponent.Color == ColorPiece.ColorType.PURPLE)) {
						gameMaster.GetPurpleValue (3 * heroLevelMaxDamage - 2 * heroLevelMinDamage);
						mWeapon.GetComponent<MagicianWeapon> ().mWeaponSpawned.transform.GetComponent<MWeaponSkill> ().UltSkill ();
					} else if ((enteredPiece.ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
						castle.GetHealth (50 + enteredPiece.value);
					}
					if (enteredPiece.pieceDebuffType == GamePiece.PieceDebuffType.PUFFER) {
						castle.GetComponent<Barricade> ().GetHurt (enteredPiece.transform.FindChild ("Puffer(Clone)").GetComponent<PufferBlock> ().phases [enteredPiece.transform.FindChild ("Puffer(Clone)").GetComponent<PufferBlock> ().phase].damage, 1, 0.2f);
					}
				}
				pressedPiece = null;
				enteredPiece = null;
			}
			//Debug.LogError ("IsClickable is true.");
		} 

		//Debug.LogError ("29");
	}

	public List<GamePiece> GetMatch(GamePiece piece, int newX, int newY)
	{
		//Debug.LogError ("30");
		if (piece.IsColored ()) {
			if (debugCheck) Debug.LogError ("Piece " + newX + " " + newY);
			ColorPiece.ColorType color = piece.ColorComponent.Color;
			List<GamePiece> horizontalPieces = new List<GamePiece> ();
			List<GamePiece> verticalPieces = new List<GamePiece> ();
			List<GamePiece> matchingPieces = new List<GamePiece> ();
			//Debug.LogError ("31");
			horizontalPieces.Add (piece);
			//Debug.LogError ("32");
			for (int dir = 0; dir <= 1; dir++) {
				for (int xOffset = 1; xOffset < xDim; xOffset++) {
					int x;
				
					if (dir == 0) {
						x = newX - xOffset;
						//Debug.LogError ("33");
					} else {
						x = newX + xOffset;
						//Debug.LogError ("34");
					}

					if (x < 0 || x >= xDim) {
						//Debug.LogError ("35");
						break;
					}
					if (pieces [x, newY].IsColored () && pieces [x, newY].ColorComponent.Color == color && pieces [x, newY].pieceDebuffType != GamePiece.PieceDebuffType.ICE && pieces [x, newY].pieceDebuffType != GamePiece.PieceDebuffType.CHAINED) {
						if (debugCheck) Debug.LogError (x + " " + newY);
						horizontalPieces.Add (pieces [x, newY]);
						//Debug.LogError ("36");
					} else {
						//Debug.LogError ("37");
						break;
					}
				}
				//Debug.LogError ("38");
			}
			//Debug.LogError ("39");

			if (horizontalPieces.Count >= 3) {
				for (int i = 0; i < horizontalPieces.Count; i++) {
					if (debugCheck) Debug.LogError ("matchingPiecesAdd " + horizontalPieces [i].X + " " + horizontalPieces [i].Y);
					matchingPieces.Add (horizontalPieces [i]);
					//Debug.LogError ("40");
				}
				//Debug.LogError ("41");
			}

			if (horizontalPieces.Count >= 3) {
				//Debug.LogError ("41.1");
				for (int i = 0; i < horizontalPieces.Count; i++) {
					for (int dir = 0; dir <= 1; dir++) {
						for (int yOffset = 1; yOffset < yDim; yOffset++) {
							int y;

							if (dir == 0) {
								y = newY - yOffset;
								//Debug.LogError ("42");
							} else {
								y = newY + yOffset;
								//Debug.LogError ("43");
							}

							if (y < 0 || y >= yDim) {
								//Debug.LogError ("44");
								break;
							}

									if (pieces [horizontalPieces [i].X, y].IsColored () && pieces [horizontalPieces [i].X, y].ColorComponent.Color == color && pieces [horizontalPieces [i].X, y].pieceDebuffType != GamePiece.PieceDebuffType.ICE && pieces [horizontalPieces [i].X, y].pieceDebuffType != GamePiece.PieceDebuffType.CHAINED) {
								if (debugCheck) Debug.LogError (horizontalPieces [i].X + " " + y);
								verticalPieces.Add (pieces [horizontalPieces [i].X, y]);
								//Debug.LogError ("45");
							} else {
								//Debug.LogError ("46");
								break;
							}
							//Debug.LogError ("47");
						}
					}
					if (verticalPieces.Count < 2) {
						verticalPieces.Clear ();
						//Debug.LogError ("48");
					} else {
						for (int j = 0; j < verticalPieces.Count; j++) {
							if (debugCheck) Debug.LogError ("matchingPiecesAdd " + verticalPieces [j].X + " " + verticalPieces [j].Y);
							matchingPieces.Add (verticalPieces [j]);
						}
						//Debug.LogError ("49");
						break;
					}
					verticalPieces.Clear ();
				}
			}
			//Debug.LogError ("50");

			if (matchingPieces.Count >= 3) {
				int l = matchingPieces.Count;
				for (int i = 0; i < l; i++) {
					for (int j = i + 1; j < l; j++) {
						if (matchingPieces [i] == matchingPieces [j]) {
							matchingPieces.Remove (matchingPieces [j]);
							j--;
							l--;
						}
					}
				}
				if (debugCheck) for (int i = 0; i < matchingPieces.Count; i++) Debug.LogError ("matchingPieces: " + matchingPieces [i].X + " " + matchingPieces [i].Y);
				return matchingPieces;
				//Debug.LogError ("51");
			}
			//Debug.LogError ("52");
			horizontalPieces.Clear ();
			//Debug.LogError ("53");
			verticalPieces.Clear ();
			//Debug.LogError ("54");
			matchingPieces.Clear();
			verticalPieces.Add (piece);

			for (int dir = 0; dir <= 1; dir++) {
				for (int yOffset = 1; yOffset < yDim; yOffset++) {
					int y;

					if (dir == 0) {
						y = newY - yOffset;
						//Debug.LogError ("55");
					} else {
						y = newY + yOffset;
						//Debug.LogError ("56");
					}

					if (y < 0 || y >= yDim) {
						//Debug.LogError ("57");
						break;
					}

							if (pieces [newX, y].IsColored () && pieces [newX, y].ColorComponent.Color == color && pieces [newX, y].pieceDebuffType != GamePiece.PieceDebuffType.ICE && pieces [newX, y].pieceDebuffType != GamePiece.PieceDebuffType.CHAINED) {
						if (debugCheck) Debug.LogError (newX + " " + y);
						verticalPieces.Add (pieces [newX, y]);
						//Debug.LogError ("58");
					} else {
						//Debug.LogError ("59");
						break;
					}
				}
			}

			if (verticalPieces.Count >= 3) {
				for (int i = 0; i < verticalPieces.Count; i++) {
					if (debugCheck) Debug.LogError ("matchingPiecesAdd " + verticalPieces [i].X + " " + verticalPieces [i].Y);
					matchingPieces.Add (verticalPieces [i]);
				}
				//Debug.LogError ("60");
			}
			//Debug.LogError ("61");

			if (verticalPieces.Count >= 3) {
				//Debug.LogError ("61.1");
				for (int i = 0; i < verticalPieces.Count; i++) {
					for (int dir = 0; dir <= 1; dir++) {
						for (int xOffset = 1; xOffset < xDim; xOffset++) {
							int x;

							if (dir == 0) {
								x = newX - xOffset;
								//Debug.LogError ("62");
							} else {
								x = newX + xOffset;
								//Debug.LogError ("63");
							}

							if (x < 0 || x >= xDim) {
								//Debug.LogError ("64");
								break;
							}

									if (pieces [x, verticalPieces [i].Y].IsColored () && pieces [x, verticalPieces [i].Y].ColorComponent.Color == color && pieces [x, verticalPieces [i].Y].pieceDebuffType != GamePiece.PieceDebuffType.ICE && pieces [x, verticalPieces [i].Y].pieceDebuffType != GamePiece.PieceDebuffType.CHAINED) {
								if (debugCheck) Debug.LogError (x + " " + verticalPieces [i].Y);
								horizontalPieces.Add (pieces [x, verticalPieces [i].Y]);
								//Debug.LogError ("65");
							} else {
								//Debug.LogError ("66");
								break;
							}
						}
						for (int j = 0; j < horizontalPieces.Count; j++) {
							if (debugCheck) Debug.LogError ("horizontalPieces " + horizontalPieces [j].X + " " + horizontalPieces [j].Y);
						}
					}
					if (horizontalPieces.Count < 2) {
						horizontalPieces.Clear ();
						//Debug.LogError ("67");
					} else {
						for (int j = 0; j < horizontalPieces.Count; j++) {
							if (debugCheck) Debug.LogError ("matchingPiecesAdd " + horizontalPieces [j].X + " " + horizontalPieces [j].Y);
							matchingPieces.Add (horizontalPieces [j]);
						}
						//Debug.LogError ("68");
						break;
					}
					horizontalPieces.Clear ();
				}
			}
			//Debug.LogError ("69");

			if (matchingPieces.Count >= 3) {
				//Debug.LogError ("70");
				int l = matchingPieces.Count;
				for (int i = 0; i < l; i++) {
					for (int j = i + 1; j < l; j++) {
						if (matchingPieces [i] == matchingPieces [j]) {
							matchingPieces.Remove (matchingPieces [j]);
							j--;
							l--;
						}
					}
				}
				if (debugCheck) for (int i = 0; i < matchingPieces.Count; i++) Debug.LogError ("matchingPieces: " + matchingPieces [i].X + " " + matchingPieces [i].Y);
				return matchingPieces;
			}
			horizontalPieces.Clear ();
			verticalPieces.Clear ();
			matchingPieces.Clear();
		}
		//Debug.LogError ("71");

		return null;
	}

	public void DebugCheck() {
		if (!debugCheck) debugCheck = true;
		else debugCheck = false;
	}

	public bool ClearAllValidMatches()
	{
		needsRefill = false;
		bool matched = false;
		gameMaster.redValue  = 0;
		gameMaster.blueValue  = 0;
		gameMaster.greenValue  = 0;
		gameMaster.purpleValue  = 0;
		gameMaster.yellowValue  = 0;
		gameMaster.StackModifier (false);
		//Debug.LogError ("72");
		for (int y = 0; y < yDim; y++) {
			for (int x = 0; x < xDim; x++) {
				if (pieces [x, y].IsClearable ()) { 
					List<GamePiece> match = GetMatch (pieces [x, y], x, y);
					//Debug.LogError ("73");
					if (match != null) {
						matched = true;
						PieceType specialPieceType = PieceType.COUNT;
						GamePiece randomPiece = match [Random.Range (0, match.Count)];
						int dmg;
						if ((randomPiece.ColorComponent.Color == ColorPiece.ColorType.YELLOW) || (randomPiece.ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
							dmg = Random.Range (1, 4);
						} else {
							dmg = Random.Range (heroLevelMinDamage, heroLevelMaxDamage + 1);
						}
						int specialPieceX = randomPiece.X;
						int specialPieceY = randomPiece.Y;
						//Debug.LogError ("73.1");
						if (match.Count == 4) {
							if (match [0].ColorComponent.Color != ColorPiece.ColorType.YELLOW) {
								if (pressedPiece == null || enteredPiece == null) {
									if (match [0].X == match [1].X) {
										specialPieceType = PieceType.VER_CLEAR;
									} else if (match [0].Y == match [1].Y) {
										specialPieceType = PieceType.HOR_CLEAR;
									}
									//Debug.LogError ("74");
								} else if (pressedPiece.Y == enteredPiece.Y) {
									specialPieceType = PieceType.VER_CLEAR;
									//Debug.LogError ("75");
								} else {
									specialPieceType = PieceType.HOR_CLEAR;
									//Debug.LogError ("76");
								}
							}
						}
						if (match.Count >= 5) {
							if (match [0].ColorComponent.Color != ColorPiece.ColorType.YELLOW) {
								specialPieceType = PieceType.ULTIMATE;
							}
						}
						//Debug.LogError ("77");
						for (int i = 0; i < match.Count; i++) {
							if (ClearPiece (match [i].X, match [i].Y)) {
								if (started) {
									switch (match [i].ColorComponent.Color) {
									case ColorPiece.ColorType.RED:
										if (allUltClear) {
											if (match [i] == pressedPieceTemp || match [i] == enteredPieceTemp)
												UltClear (match [i]);
											else gameMaster.GetRedValue (match [i].value);
										}
										else gameMaster.GetRedValue (match [i].value);
										break;
									case ColorPiece.ColorType.BLUE:
										if (allUltClear) {
											if (match [i] == pressedPieceTemp || match [i] == enteredPieceTemp)
												UltClear (match [i]);
											else gameMaster.GetBlueValue (match [i].value);
										}
										else gameMaster.GetBlueValue (match [i].value);
										break;
									case ColorPiece.ColorType.GREEN:
										gameMaster.GetGreenValue (match [i].value);
										break;
									case ColorPiece.ColorType.PURPLE:
										gameMaster.GetPurpleValue (match [i].value);
										break;
									case ColorPiece.ColorType.YELLOW:
										gameMaster.GetYellowValue (match [i].value);
										break;
									}
									if (match [i].pieceDebuffType == GamePiece.PieceDebuffType.PUFFER) {
										castle.GetComponent<Barricade> ().GetHurt (match [i].transform.FindChild ("Puffer(Clone)").GetComponent<PufferBlock> ().phases [match [i].transform.FindChild ("Puffer(Clone)").GetComponent<PufferBlock> ().phase].damage, 1, 0.2f);
									}
								}

								needsRefill = true;

								if (match [i] == pressedPiece || match [i] == enteredPiece) {
									specialPieceX = match [i].X;
									specialPieceY = match [i].Y;
									//Debug.LogError ("78");
								}
								//Debug.LogError ("79");
							}
						}
//						gameMaster.GetFinalDamage (1, 1);
//						if (allUltClear) UltClear(ultClearTemp[0]);
						gameMaster.GetGold (gameMaster.yellowValue);
//						gameMaster.goldText.text = (gameMaster.stageGold * 5).ToString ();
						if (started) {
							switch (match [0].ColorComponent.Color) {
							case ColorPiece.ColorType.RED:
								gameMaster.RedTrigger ();
								break;
							case ColorPiece.ColorType.BLUE:
								gameMaster.BlueTrigger ();
								break;
							case ColorPiece.ColorType.GREEN:
								gameMaster.GreenTrigger ();
								break;
							case ColorPiece.ColorType.PURPLE:
								gameMaster.PurpleTrigger ();
								break;
							case ColorPiece.ColorType.YELLOW:
								gameMaster.YellowTrigger ();
								break;
							}
						}

						GetComponent<AudioController> ().PlaySound ("JewelClear");
						if (started) {
							for (int i = 0; i < match.Count; i++) {
								if (match[i].Type == PieceType.HOR_CLEAR) {
									if ((match [0].ColorComponent.Color == ColorPiece.ColorType.RED)) {
										HorClear (match [i]);
									} else if (match [0].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
										heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().HorSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
										if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
											heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().HorSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
										}
									}
								}
								if (match[i].Type == PieceType.VER_CLEAR) {
									if ((match [0].ColorComponent.Color == ColorPiece.ColorType.RED)) {
										VerClear (match [i]);
									}
//									if ((match [0].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
//										gameMaster.GetGreenValue (20 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
//									}
									else if (match [0].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
										heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().VerSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
										if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
											heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().VerSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
										}
									}
								}
								if ((match[i].Type == PieceType.HOR_CLEAR) ||(match[i].Type == PieceType.VER_CLEAR)) {
									if (match [0].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
										mWeapon.GetComponent<MagicianWeapon>().mWeaponSpawned.transform.GetComponent<MWeaponSkill> ().Skill ();
									} else if ((match [0].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
										gameMaster.GetGreenValue (20 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
									}
								}
								if (match [i].Type == PieceType.ULTIMATE) {
									switch (match [0].ColorComponent.Color) {
									case ColorPiece.ColorType.RED:
										UltClear (match [i]);
										break;
									case ColorPiece.ColorType.BLUE:
										heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().UltSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
										if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
											heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().UltSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
										}
										break;
									case ColorPiece.ColorType.GREEN:
										gameMaster.GetGreenValue (50 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
										break;
									case ColorPiece.ColorType.PURPLE:
										mWeapon.GetComponent<MagicianWeapon>().mWeaponSpawned.transform.GetComponent<MWeaponSkill> ().UltSkill ();// magician Ult
										break;
									}
								}
							}
							switch (match [0].ColorComponent.Color) {
							case ColorPiece.ColorType.RED:
								hWeapon.gameObject.GetComponent<HeroWeapon> ().hWeaponSpawned.gameObject.GetComponent<HWeapon> ().Attack (gameMaster.redValue + gameMaster.redSkillValue + gameMaster.redWeaponValue + gameMaster.redBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
								break;
							case ColorPiece.ColorType.BLUE:
								if (allUltClear && firstClear) {
									hWeapon.gameObject.GetComponent<HeroWeapon> ().hWeaponSpawned.gameObject.GetComponent<HWeapon> ().Attack (gameMaster.redValue + gameMaster.redSkillValue + gameMaster.redWeaponValue + gameMaster.redBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
								}
								else hWeapon.gameObject.GetComponent<HeroWeapon> ().hWeaponSpawned.gameObject.GetComponent<HWeapon> ().Attack (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.redBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
								break;
							case ColorPiece.ColorType.GREEN:
								castle.GetHealth(gameMaster.greenValue + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
								break;
							case ColorPiece.ColorType.PURPLE:
								if (skyRayCast.GetComponent<ARayCast> ().hit) {
									Magician.magician.Attack (GameMaster.gameMaster.purpleValue + GameMaster.gameMaster.purpleSkillValue + GameMaster.gameMaster.purpleBurningAddValue, GameMaster.gameMaster.purpleWeaponValue);
								} else if (!skyRayCast.GetComponent<ARayCast> ().hit) {
									if (groundRayCast.GetComponent<ARayCast> ().hit) {
										Magician.magician.Attack (GameMaster.gameMaster.purpleValue + GameMaster.gameMaster.purpleSkillValue + GameMaster.gameMaster.purpleBurningAddValue, GameMaster.gameMaster.purpleWeaponValue);
									}
								}
								break;
							}
						}
						//Debug.LogError ("80");

						if (specialPieceType != PieceType.COUNT) {
							Destroy (pieces [specialPieceX, specialPieceY]);
							GamePiece newPiece = SpawnNewPiece (specialPieceX, specialPieceY, specialPieceType, dmg, GamePiece.PieceDebuffType.NONE);;
							//Debug.LogError ("81");
							if ((specialPieceType == PieceType.VER_CLEAR || specialPieceType == PieceType.HOR_CLEAR)
							   && newPiece.IsColored () && match [0].IsColored ()) {
								newPiece.ColorComponent.SetColor (match [0].ColorComponent.Color);
								//Debug.LogError ("82");
							}
							if (specialPieceType == PieceType.ULTIMATE && newPiece.IsColored () && match [0].IsColored ()) {
								newPiece.ColorComponent.SetColor (match [0].ColorComponent.Color);
							}
						}
						//Debug.LogError ("83");
					}
				}
			}
		}
		//Debug.LogError ("84");
		firstClear = false;
		if (matched) {
			gameMaster.ComboAdd ();
		}
		return needsRefill;
	}

	public bool ClearPiece(int x, int y)
	{
		if (pieces [x, y].IsClearable () && !pieces [x, y].ClearableComponent.IsBeingCleared) {
			pieces [x, y].ClearableComponent.Clear ();
			SpawnNewPiece (x, y, PieceType.EMPTY, 0, GamePiece.PieceDebuffType.NONE);
			//Debug.LogError ("85");

			ClearIces (x, y);

			return true;
		}
		//Debug.LogError ("86");
		return false;
	}

	public void ClearIces (int x, int y) {
		for (int adjacentX = x - 1; adjacentX <= x + 1; adjacentX++) {
			if (adjacentX != x && adjacentX >= 0 && adjacentX < xDim) {
				if (pieces [adjacentX, y].pieceDebuffType == GamePiece.PieceDebuffType.ICE && !pieces [adjacentX, y].IsClearable ()) {
					Destroy (pieces [adjacentX, y].transform.FindChild ("Ice(Clone)").gameObject);
					pieces [adjacentX, y].pieceDebuffType = GamePiece.PieceDebuffType.NONE;
					pieces [adjacentX, y].GetComponent<MovablePiece> ().enabled = true;
					pieces [adjacentX, y].GetComponent<ClearablePiece> ().enabled = true;
				}
			}
		}

		for (int adjacentY = y - 1; adjacentY <= y + 1; adjacentY++) {
			if (adjacentY != y && adjacentY >= 0 && adjacentY < yDim) {
				if (pieces [x, adjacentY].pieceDebuffType == GamePiece.PieceDebuffType.ICE && !pieces [x, adjacentY].IsClearable ()) {
					Destroy (pieces [x, adjacentY].transform.FindChild ("Ice(Clone)").gameObject);
					pieces [x, adjacentY].pieceDebuffType = GamePiece.PieceDebuffType.NONE;
					pieces [x, adjacentY].GetComponent<MovablePiece> ().enabled = true;
					pieces [x, adjacentY].GetComponent<ClearablePiece> ().enabled = true;
				}
			}
		}
	}

	public void HorClear (GamePiece piece)
	{
		for (int i = 0; i < xDim; i++) {
			gameMaster.GetRedValue (pieces [i, piece.Y].value);
			if (pieces [i, piece.Y].Type != PieceType.EMPTY) {
				if (pieces [i, piece.Y].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					gameMaster.GetBlueValue (pieces [i, piece.Y].value);
				} else if (pieces [i, piece.Y].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
					gameMaster.GetPurpleValue (pieces [i, piece.Y].value);
				} else if (pieces [i, piece.Y].ColorComponent.Color == ColorPiece.ColorType.GREEN) {
					gameMaster.GetGreenValue (pieces [i, piece.Y].value);
				} else if (pieces [i, piece.Y].ColorComponent.Color == ColorPiece.ColorType.YELLOW) {
					gameMaster.GetYellowValue (pieces [i, piece.Y].value);
				}
			}
		}
		castle.GetHealth(gameMaster.greenValue + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
//		gameMaster.GetFinalDamage (1, 0);
		gameMaster.GetGold (gameMaster.yellowValue);

		for (int j = 0; j < yDim; j++) {
			List<GamePiece> match = GetMatch (pieces [j, piece.Y], j, piece.Y);
			if (match != null) {
				if (allUltClear) {
					if (pieces [j, piece.Y] == pressedPieceTemp || pieces [j, piece.Y] == enteredPieceTemp) {
						if (pieces [j, piece.Y] != piece && (pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.RED || pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.BLUE)) {
							VerClear (pieces [j, piece.Y]);
						} else if (pieces [j, piece.Y] != piece) {
							for (int k = 0; k < match.Count; k++) {
								if (match [k] != pieces [j, piece.Y]) {
									switch (match [k].ColorComponent.Color) {
									case ColorPiece.ColorType.RED:
										gameMaster.GetRedValue (match [k].value);
										break;
									case ColorPiece.ColorType.BLUE:
										gameMaster.GetBlueValue (match [k].value);
										break;
									case ColorPiece.ColorType.GREEN:
										gameMaster.GetGreenValue (match [k].value);
										break;
									case ColorPiece.ColorType.PURPLE:
										gameMaster.GetPurpleValue (match [k].value);
										break;
									case ColorPiece.ColorType.YELLOW:
										gameMaster.GetYellowValue (match [k].value);
										break;
									}
									ClearPiece (match [k].X, match [k].Y);
								}
							}
						}
					}
				} else if (!allUltClear) {
					for (int k = 0; k < match.Count; k++) {
						if (match [k] != pieces [j, piece.Y])
							ClearPiece (match [k].X, match [k].Y);
					}
				}
			}
			if (pieces [j, piece.Y].Type == PieceType.HOR_CLEAR) {
				if ((pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
					gameMaster.GetGreenValue (20 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
				} else if (pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().HorSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
						heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().HorSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					}
				}
			}
			if (pieces [j, piece.Y].Type == PieceType.VER_CLEAR) {
				if ((pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.RED)) {
					ClearPiece (j, piece.Y);
					VerClear (pieces [j, piece.Y]);
				} else if ((pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
					gameMaster.GetGreenValue (20 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
				} else if (pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().VerSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
						heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().VerSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					}
				}
			}
			if ((pieces [j, piece.Y].Type == PieceType.HOR_CLEAR) ||(pieces [j, piece.Y].Type == PieceType.VER_CLEAR)) {
				if (pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
					mWeapon.GetComponent<MagicianWeapon>().mWeaponSpawned.transform.GetComponent<MWeaponSkill> ().Skill ();
				}
			}
			if (pieces [j, piece.Y].Type == PieceType.ULTIMATE) {
				if ((pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.RED)) {
					ClearPiece (j, piece.Y);
					VerClear (pieces [j, piece.Y]);
				} else if ((pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
					gameMaster.GetGreenValue (50 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
				} else if (pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
					mWeapon.GetComponent<MagicianWeapon>().mWeaponSpawned.transform.GetComponent<MWeaponSkill> ().UltSkill ();// magician Ult
				} else if (pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().UltSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
						heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().UltSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					}
				}
			}
			ClearPiece (j, piece.Y);
		}
		StartCoroutine (Fill ());
	}

	public void VerClear (GamePiece piece)
	{
		for (int i = 0; i < yDim; i++) {
			gameMaster.GetRedValue (pieces [piece.X, i].value);
			if (pieces [piece.X, i].Type != PieceType.EMPTY) {
				if (pieces [piece.X, i].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					gameMaster.GetBlueValue (pieces [piece.X, i].value);
				} else if (pieces [piece.X, i].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
					gameMaster.GetPurpleValue (pieces [piece.X, i].value);
				} else if (pieces [piece.X, i].ColorComponent.Color == ColorPiece.ColorType.GREEN) {
					gameMaster.GetGreenValue (pieces [piece.X, i].value);
				} else if (pieces [piece.X, i].ColorComponent.Color == ColorPiece.ColorType.YELLOW) {
					gameMaster.GetYellowValue (pieces [piece.X, i].value);
				}
			}
		}
		castle.GetHealth(gameMaster.greenValue + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
//		gameMaster.GetFinalDamage (1, 0);
		gameMaster.GetGold (gameMaster.yellowValue);

		for (int j = 0; j < yDim; j++) {
			List<GamePiece> match = GetMatch (pieces [piece.X, j], piece.X, j);
			if (match != null) {
				if (allUltClear) {
					if (pieces [piece.X, j] == pressedPieceTemp || pieces [piece.X, j] == enteredPieceTemp) {
						if (pieces [piece.X, j] != piece && (pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.RED || pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.BLUE)) {
							HorClear (pieces [piece.X, j]);
						} else if (pieces [piece.X, j] != piece) {
							for (int k = 0; k < match.Count; k++) {
								if (match [k] != pieces [j, piece.Y]) {
									switch (match [k].ColorComponent.Color) {
									case ColorPiece.ColorType.RED:
										gameMaster.GetRedValue (match [k].value);
										break;
									case ColorPiece.ColorType.BLUE:
										gameMaster.GetBlueValue (match [k].value);
										break;
									case ColorPiece.ColorType.GREEN:
										gameMaster.GetGreenValue (match [k].value);
										break;
									case ColorPiece.ColorType.PURPLE:
										gameMaster.GetPurpleValue (match [k].value);
										break;
									case ColorPiece.ColorType.YELLOW:
										gameMaster.GetYellowValue (match [k].value);
										break;
									}
									ClearPiece (match [k].X, match [k].Y);
								}
							}
						}
					}
				} else if (!allUltClear) {
					for (int k = 0; k < match.Count; k++) {
						if (match [k] != pieces [piece.X, j])
							ClearPiece (match [k].X, match [k].Y);
					}
				}
			}
			if (pieces [piece.X, j].Type == PieceType.HOR_CLEAR) {
				if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.RED)) {
					ClearPiece (piece.X, j);
					HorClear (pieces [piece.X, j]);
				} else if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
					gameMaster.GetGreenValue (20 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
				} else if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.YELLOW)) {
					gameMaster.GetYellowValue (20 + gameMaster.yellowSkillValue + gameMaster.yellowWeaponValue);
				} else if (pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().HorSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
						heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().HorSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					}
				}
			}
			if (pieces [piece.X, j].Type == PieceType.VER_CLEAR) {
				if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
					gameMaster.GetGreenValue (20 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
				} else if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.YELLOW)) {
					gameMaster.GetYellowValue (20 + gameMaster.yellowSkillValue + gameMaster.yellowWeaponValue);
				} else if (pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().VerSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
						heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().VerSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					}
				}
			}
			if ((pieces [piece.X, j].Type == PieceType.HOR_CLEAR) ||(pieces [piece.X, j].Type == PieceType.VER_CLEAR)) {
				if (pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
					mWeapon.GetComponent<MagicianWeapon>().mWeaponSpawned.transform.GetComponent<MWeaponSkill> ().Skill ();
				}
			}
			if (pieces [piece.X, j].Type == PieceType.ULTIMATE) {
				if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.RED)) {
					ClearPiece (piece.X, j);
					HorClear (pieces [piece.X, j]);
				} else if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
					gameMaster.GetGreenValue (50 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
				} else if (pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
					mWeapon.GetComponent<MagicianWeapon>().mWeaponSpawned.transform.GetComponent<MWeaponSkill> ().UltSkill ();// magician Ult
				} else if (pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().UltSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
						heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().UltSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					}
				}
			}
			ClearPiece (piece.X, j);
		}
		StartCoroutine (Fill ());
	}

	public void UltClear (GamePiece piece) {
		for (int i = 0; i < xDim; i++) {
			gameMaster.GetRedValue (pieces [i, piece.Y].value);
			if (pieces [i, piece.Y].Type != PieceType.EMPTY) {
				if (pieces [i, piece.Y].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					gameMaster.GetBlueValue (pieces [i, piece.Y].value);
				} else if (pieces [i, piece.Y].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
					gameMaster.GetPurpleValue (pieces [i, piece.Y].value);
				} else if (pieces [i, piece.Y].ColorComponent.Color == ColorPiece.ColorType.GREEN) {
					gameMaster.GetGreenValue (pieces [i, piece.Y].value);
				} else if (pieces [i, piece.Y].ColorComponent.Color == ColorPiece.ColorType.YELLOW) {
					gameMaster.GetYellowValue (pieces [i, piece.Y].value);
				}
			}
		}

		for (int i = 0; i < yDim; i++) {
			gameMaster.GetRedValue (pieces [piece.X, i].value);
			if (pieces [piece.X, i].Type != PieceType.EMPTY) {
				if (pieces [piece.X, i].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					gameMaster.GetBlueValue (pieces [piece.X, i].value);
				} else if (pieces [piece.X, i].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
					gameMaster.GetPurpleValue (pieces [piece.X, i].value);
				} else if (pieces [piece.X, i].ColorComponent.Color == ColorPiece.ColorType.GREEN) {
					gameMaster.GetGreenValue (pieces [piece.X, i].value);
				} else if (pieces [piece.X, i].ColorComponent.Color == ColorPiece.ColorType.YELLOW) {
					gameMaster.GetYellowValue (pieces [piece.X, i].value);
				}
			}
		}
		castle.GetHealth(gameMaster.greenValue + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
//		gameMaster.GetFinalDamage (1, 0);
		gameMaster.GetGold (gameMaster.yellowValue);

		for (int j = 0; j < xDim; j++) {
			List<GamePiece> match = GetMatch (pieces [j, piece.Y], j, piece.Y);
			if (match != null) {
				if (allUltClear) {
					if (pieces [j, piece.Y] == pressedPieceTemp || pieces [j, piece.Y] == enteredPieceTemp) {
						if (pieces [j, piece.Y] != piece && (pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.RED || pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.BLUE)) {
							VerClear (pieces [j, piece.Y]);
						} else if (pieces [j, piece.Y] != piece) {
							for (int k = 0; k < match.Count; k++) {
								if (match [k] != pieces [j, piece.Y]) {
									switch (match [k].ColorComponent.Color) {
									case ColorPiece.ColorType.RED:
										gameMaster.GetRedValue (match [k].value);
										break;
									case ColorPiece.ColorType.BLUE:
										gameMaster.GetBlueValue (match [k].value);
										break;
									case ColorPiece.ColorType.GREEN:
										gameMaster.GetGreenValue (match [k].value);
										break;
									case ColorPiece.ColorType.PURPLE:
										gameMaster.GetPurpleValue (match [k].value);
										break;
									case ColorPiece.ColorType.YELLOW:
										gameMaster.GetYellowValue (match [k].value);
										break;
									}
									ClearPiece (match [k].X, match [k].Y);
								}
							}
						}
					}
				} else if (!allUltClear) {
					for (int k = 0; k < match.Count; k++) {
						if (match [k] != pieces [j, piece.Y])
							ClearPiece (match [k].X, match [k].Y);
					}
				}
			}
			if (pieces [j, piece.Y].Type == PieceType.HOR_CLEAR) {
				if ((pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
					gameMaster.GetGreenValue (20 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
				} else if ((pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.YELLOW)) {
					gameMaster.GetYellowValue (20 + gameMaster.yellowSkillValue + gameMaster.yellowWeaponValue);
				} else if (pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().HorSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
						heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().HorSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					}
				}
			}
			if (pieces [j, piece.Y].Type == PieceType.VER_CLEAR) {
				if ((pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.RED)) {
					ClearPiece (j, piece.Y);
					VerClear (pieces [j, piece.Y]);
				} else if ((pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
					gameMaster.GetGreenValue (20 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
				} else if ((pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.YELLOW)) {
					gameMaster.GetYellowValue (20 + gameMaster.yellowSkillValue + gameMaster.yellowWeaponValue);
				} else if (pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().VerSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
						heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().VerSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					}
				}
			}
			if ((pieces [j, piece.Y].Type == PieceType.HOR_CLEAR) ||(pieces [j, piece.Y].Type == PieceType.VER_CLEAR)) {
				if (pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
					mWeapon.GetComponent<MagicianWeapon>().mWeaponSpawned.transform.GetComponent<MWeaponSkill> ().Skill ();
				}
			}
			if (pieces [j, piece.Y].Type == PieceType.ULTIMATE) {
				if ((pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.RED)) {
					ClearPiece (j, piece.Y);
					VerClear (pieces [j, piece.Y]);
				} else if ((pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
					gameMaster.GetGreenValue (50 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
				} else if (pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
					mWeapon.GetComponent<MagicianWeapon>().mWeaponSpawned.transform.GetComponent<MWeaponSkill> ().UltSkill ();// magician Ult
				} else if (pieces [j, piece.Y].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().UltSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
						heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().UltSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					}
				}
			}
			ClearPiece (j, piece.Y);
		}

		for (int j = 0; j < yDim; j++) {
			List<GamePiece> match = GetMatch (pieces [piece.X, j], piece.X, j);
			if (match != null) {
				if (allUltClear) {
					if (pieces [piece.X, j] == pressedPieceTemp || pieces [piece.X, j] == enteredPieceTemp) {
						if (pieces [piece.X, j] != piece && (pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.RED || pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.BLUE)) {
							HorClear (pieces [piece.X, j]);
						} else if (pieces [piece.X, j] != piece) {
							for (int k = 0; k < match.Count; k++) {
								if (match [k] != pieces [j, piece.Y]) {
									switch (match [k].ColorComponent.Color) {
									case ColorPiece.ColorType.RED:
										gameMaster.GetRedValue (match [k].value);
										break;
									case ColorPiece.ColorType.BLUE:
										gameMaster.GetBlueValue (match [k].value);
										break;
									case ColorPiece.ColorType.GREEN:
										gameMaster.GetGreenValue (match [k].value);
										break;
									case ColorPiece.ColorType.PURPLE:
										gameMaster.GetPurpleValue (match [k].value);
										break;
									case ColorPiece.ColorType.YELLOW:
										gameMaster.GetYellowValue (match [k].value);
										break;
									}
									ClearPiece (match [k].X, match [k].Y);
								}
							}
						}
					}
				} else if (!allUltClear) {
					for (int k = 0; k < match.Count; k++) {
						if (match [k] != pieces [piece.X, j])
							ClearPiece (match [k].X, match [k].Y);
					}
				}
			}
			if (pieces [piece.X, j].Type == PieceType.HOR_CLEAR) {
				if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.RED)) {
					ClearPiece (piece.X, j);
					HorClear (pieces [piece.X, j]);
				} else if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
					gameMaster.GetGreenValue (20 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
				} else if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.YELLOW)) {
					gameMaster.GetYellowValue (20 + gameMaster.yellowSkillValue + gameMaster.yellowWeaponValue);
				} else if (pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().HorSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
						heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().HorSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					}
				}
			}
			if (pieces [piece.X, j].Type == PieceType.VER_CLEAR) {
				if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
					gameMaster.GetGreenValue (20 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
				} else if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.YELLOW)) {
					gameMaster.GetYellowValue (20 + gameMaster.yellowSkillValue + gameMaster.yellowWeaponValue);
				} else if (pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().VerSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
						heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().VerSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					}
				}
			}
			if ((pieces [piece.X, j].Type == PieceType.HOR_CLEAR) ||(pieces [piece.X, j].Type == PieceType.VER_CLEAR)) {
				if (pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
					mWeapon.GetComponent<MagicianWeapon>().mWeaponSpawned.transform.GetComponent<MWeaponSkill> ().Skill ();
				}
			}
			if (pieces [piece.X, j].Type == PieceType.ULTIMATE) {
				if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.RED)) {
					ClearPiece (piece.X, j);
					HorClear (pieces [piece.X, j]);
				} else if ((pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.GREEN)) {
					gameMaster.GetGreenValue (50 + gameMaster.greenSkillValue + gameMaster.greenWeaponValue);
				} else if (pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
					mWeapon.GetComponent<MagicianWeapon>().mWeaponSpawned.transform.GetComponent<MWeaponSkill> ().UltSkill ();// magician Ult
				} else if (pieces [piece.X, j].ColorComponent.Color == ColorPiece.ColorType.BLUE) {
					heroPrefab.GetComponent<Hero>().selectedHero.GetComponent<HeroSkill>().UltSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					if (heroPrefab.GetComponent<Hero> ().copyHero != null) {
						heroPrefab.GetComponent<Hero> ().copyHero.GetComponent<HeroSkill> ().UltSkill (gameMaster.blueValue + gameMaster.blueSkillValue + gameMaster.blueWeaponValue + gameMaster.blueBurningAddValue, Random.Range (gameMaster.weaponMin, gameMaster.weaponMax + 1));
					}
				}
			}
			ClearPiece (piece.X, j);
		}
		StartCoroutine (Fill ());
	}
}
