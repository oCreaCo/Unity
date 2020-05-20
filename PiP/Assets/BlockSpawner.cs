using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {
	
	[SerializeField] private GameObject stage;
	[SerializeField] private Transform blocks;
	[SerializeField] private Transform tiles;

	[SerializeField] private GameObject normalBlock;
	[SerializeField] private GameObject shooterBlock;
	[SerializeField] private GameObject mirrorBlock;
	[SerializeField] private GameObject receiverBlock;
	[SerializeField] private GameObject buttonBlock;
	[SerializeField] private GameObject activationBlock;
	[SerializeField] private GameObject beacon;
	[SerializeField] private GameObject teleporter;
	[SerializeField] private GameObject pathBlock;
	[SerializeField] private GameObject tile;
	[SerializeField] private GameObject path;
	private Transform myTransform;
	public int xDim, yDim;
	private Vector3 spawnPoint;
	[SerializeField] private Sprite trueSprite, falseSprite;
	[SerializeField] private Sprite trueHor, falseHor;
	[SerializeField] private Sprite trueVer, falseVer;
	[SerializeField] private Sprite trueCross, falseCross;

	[SerializeField] private Sprite trueUShooter, falseUShooter;
	[SerializeField] private Sprite trueDShooter, falseDShooter;
	[SerializeField] private Sprite trueRShooter, falseRShooter;
	[SerializeField] private Sprite trueLShooter, falseLShooter;

	[SerializeField] private Sprite trueLDMirror, falseLDMirror;
	[SerializeField] private Sprite trueLUMirror, falseLUMirror;
	[SerializeField] private Sprite trueRDMirror, falseRDMirror;
	[SerializeField] private Sprite trueRUMirror, falseRUMirror;

	[SerializeField] private Sprite trueUUReceiver, falseUUReceiver;
	[SerializeField] private Sprite trueUDReceiver, falseUDReceiver;
	[SerializeField] private Sprite trueDUReceiver, falseDUReceiver;
	[SerializeField] private Sprite trueDDReceiver, falseDDReceiver;
	[SerializeField] private Sprite trueLUReceiver, falseLUReceiver;
	[SerializeField] private Sprite trueLDReceiver, falseLDReceiver;
	[SerializeField] private Sprite trueRUReceiver, falseRUReceiver;
	[SerializeField] private Sprite trueRDReceiver, falseRDReceiver;

	[SerializeField] private Sprite buttonUPushed, buttonUUnPushed;
	[SerializeField] private Sprite buttonDPushed, buttonDUnPushed;
	[SerializeField] private Sprite buttonRPushed, buttonRUnPushed;
	[SerializeField] private Sprite buttonLPushed, buttonLUnPushed;

	[SerializeField] private Sprite clockwise, unClockwise;
	[SerializeField] private Sprite horMovable, verMovable;
	[SerializeField] private Sprite ray;

	public Grid grid;

	void Awake () {
		myTransform = this.transform;
	}

	public void Spawn (int _index, int _layerCount) {
		Debug.LogError (_index + " " + _layerCount);
		GameObject stageObject = Instantiate (stage, Vector3.zero, Quaternion.Euler (0, 0, 0)) as GameObject;
		if (_layerCount == 0) {
			stageObject.GetComponent<Stage> ().rayOn = true;
		}
		stageObject.GetComponent<Stage> ().startX = grid.gridState [_index].startX;
		stageObject.GetComponent<Stage> ().startY = grid.gridState [_index].startY;
		stageObject.GetComponent<Stage> ().layerNum = _layerCount;
		GetComponent<Grid> ().stage = stageObject.transform;
		GetComponent<GameMaster> ().stages.Add (stageObject.GetComponent<Stage> ());
		blocks = stageObject.transform.FindChild ("Blocks");
		tiles = stageObject.transform.FindChild ("Tiles");
		for (int y = 0; y < yDim; y++) {
			for (int x = 0; x < xDim; x++) {
				spawnPoint = new Vector3 (x - 6, y - 6, 0);
				if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].tile) {
					GameObject tileObject = Instantiate (tile, spawnPoint, myTransform.rotation, tiles) as GameObject;
					tileObject.GetComponent<SpriteRenderer> ().sortingOrder = (_layerCount * 100);
				}
				if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].path) {
					GameObject pathObject = Instantiate (path, spawnPoint, myTransform.rotation, tiles) as GameObject;
					pathObject.GetComponent<Path> ().spriteRenderer.sortingOrder = (1 + _layerCount * 100);
					switch (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].facingDirection) {
					case Grid.FacingDirection.UP:
						pathObject.GetComponent<Path> ().facingDirection = Path.FacingDirection.UP;
						break;
					case Grid.FacingDirection.DOWN:
						pathObject.GetComponent<Path> ().facingDirection = Path.FacingDirection.DOWN;
						break;
					case Grid.FacingDirection.RIGHT:
						pathObject.GetComponent<Path> ().facingDirection = Path.FacingDirection.RIGHT;
						break;
					case Grid.FacingDirection.LEFT:
						pathObject.GetComponent<Path> ().facingDirection = Path.FacingDirection.LEFT;
						break;
					}
					switch (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].pathType) {
					case Grid.PathType.LINE:
						pathObject.GetComponent<Path> ().pathType = Path.PathType.LINE;
						break;
					case Grid.PathType.CURVE:
						pathObject.GetComponent<Path> ().pathType = Path.PathType.CURVE;
						break;
					case Grid.PathType.T:
						pathObject.GetComponent<Path> ().pathType = Path.PathType.T;
						break;
					case Grid.PathType.CROSS:
						pathObject.GetComponent<Path> ().pathType = Path.PathType.CROSS;
						break;
					}
				} else if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].teleporter) {
					GameObject teleporterObject = Instantiate (teleporter, spawnPoint, myTransform.rotation, tiles) as GameObject;
					teleporterObject.GetComponent<Teleporter> ().sprite.sortingOrder = (2 + _layerCount * 100);
				}
				if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].spawn) {
					GameObject blockObject;
					switch (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].blockSort) {
					case Grid.BlockType.NORMAL:
						blockObject = Instantiate (normalBlock, spawnPoint, myTransform.rotation, blocks) as GameObject;
						blockObject.GetComponent<Block> ().SetOrderInLayer (5 * (yDim - y) + 2 + _layerCount * 100);
						blockObject.GetComponent<Block> ().x = x - 6;
						blockObject.GetComponent<Block> ().y = y - 6;
						blockObject.GetComponent<Block> ().gridStateNum = grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum;
						stageObject.GetComponent<Stage> ().blockObjects.Add (blockObject.transform);
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable) {
							blockObject.GetComponent<Block> ().horMovable = true;
						}
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
							blockObject.GetComponent<Block> ().verMovable = true;
						}
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].inPuzzle) {
							blockObject.GetComponent<Block> ().inPuzzle = true;
							if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum != 0) {
								blockObject.GetComponent<Block> ().gridStateNum = grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum;
							}
							if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable && grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
								blockObject.GetComponent<Block> ().sprite.sprite = trueCross;
							} else if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable && !grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
								blockObject.GetComponent<Block> ().sprite.sprite = trueHor;
							} else if (!grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable && grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
								blockObject.GetComponent<Block> ().sprite.sprite = trueVer;
							} else if (!grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable && !grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
								blockObject.GetComponent<Block> ().sprite.sprite = trueSprite;
							}
						} else {
							blockObject.GetComponent<Block> ().inPuzzle = false;
							if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable && grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
								blockObject.GetComponent<Block> ().sprite.sprite = falseCross;
							} else if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable && !grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
								blockObject.GetComponent<Block> ().sprite.sprite = falseHor;
							} else if (!grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable && grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
								blockObject.GetComponent<Block> ().sprite.sprite = falseVer;
							} else if (!grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable && !grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
								blockObject.GetComponent<Block> ().sprite.sprite = falseSprite;
							}
						}
						break;
					case Grid.BlockType.SHOOTER:
						blockObject = Instantiate (shooterBlock, spawnPoint, myTransform.rotation, blocks) as GameObject;
						blockObject.GetComponent<Block> ().SetOrderInLayer (5 * (yDim - y) + 2 + _layerCount * 100);
						blockObject.GetComponent<Block> ().x = x - 6;
						blockObject.GetComponent<Block> ().y = y - 6;
						blockObject.GetComponent<Block> ().gridStateNum = grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum;
						stageObject.GetComponent<Stage> ().blockObjects.Add (blockObject.transform);
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable) {
							blockObject.GetComponent<Block> ().horMovable = true;
						}
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
							blockObject.GetComponent<Block> ().verMovable = true;
						}
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].shootBool) {
							blockObject.GetComponent<ShooterBlock> ().startShootBool = true;
						}
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].inPuzzle) {
							blockObject.GetComponent<Block> ().inPuzzle = true;
							if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum != 0) {
								blockObject.GetComponent<Block> ().gridStateNum = grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum;
							}
							switch (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].facingDirection) {
							case Grid.FacingDirection.UP:
								blockObject.GetComponent<Block> ().sprite.sprite = trueUShooter;
								blockObject.GetComponent<ShooterBlock> ().facingDirection = ShooterBlock.FacingDirection.UP;
								break;
							case Grid.FacingDirection.DOWN:
								blockObject.GetComponent<Block> ().sprite.sprite = trueDShooter;
								blockObject.GetComponent<ShooterBlock> ().facingDirection = ShooterBlock.FacingDirection.DOWN;
								break;
							case Grid.FacingDirection.RIGHT:
								blockObject.GetComponent<Block> ().sprite.sprite = trueRShooter;
								blockObject.GetComponent<ShooterBlock> ().facingDirection = ShooterBlock.FacingDirection.RIGHT;
								break;
							case Grid.FacingDirection.LEFT:
								blockObject.GetComponent<Block> ().sprite.sprite = trueLShooter;
								blockObject.GetComponent<ShooterBlock> ().facingDirection = ShooterBlock.FacingDirection.LEFT;
								break;
							}
						} else {
							blockObject.GetComponent<Block> ().inPuzzle = false;
							switch (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].facingDirection) {
							case Grid.FacingDirection.UP:
								blockObject.GetComponent<Block> ().sprite.sprite = falseUShooter;
								blockObject.GetComponent<ShooterBlock> ().facingDirection = ShooterBlock.FacingDirection.UP;
								break;
							case Grid.FacingDirection.DOWN:
								blockObject.GetComponent<Block> ().sprite.sprite = falseDShooter;
								blockObject.GetComponent<ShooterBlock> ().facingDirection = ShooterBlock.FacingDirection.DOWN;
								break;
							case Grid.FacingDirection.RIGHT:
								blockObject.GetComponent<Block> ().sprite.sprite = falseRShooter;
								blockObject.GetComponent<ShooterBlock> ().facingDirection = ShooterBlock.FacingDirection.RIGHT;
								break;
							case Grid.FacingDirection.LEFT:
								blockObject.GetComponent<Block> ().sprite.sprite = falseLShooter;
								blockObject.GetComponent<ShooterBlock> ().facingDirection = ShooterBlock.FacingDirection.LEFT;
								break;
							}
						}
						break;
					case Grid.BlockType.MIRROR:
						blockObject = Instantiate (mirrorBlock, spawnPoint, myTransform.rotation, blocks) as GameObject;
						blockObject.GetComponent<Block> ().SetOrderInLayer (5 * (yDim - y) + 2 + _layerCount * 100);
						blockObject.GetComponent<Block> ().x = x - 6;
						blockObject.GetComponent<Block> ().y = y - 6;
						blockObject.GetComponent<Block> ().gridStateNum = grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum;
						stageObject.GetComponent<Stage> ().blockObjects.Add (blockObject.transform);
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable) {
							blockObject.GetComponent<Block> ().horMovable = true;
						}
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
							blockObject.GetComponent<Block> ().verMovable = true;
						}
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].inPuzzle) {
							blockObject.GetComponent<Block> ().inPuzzle = true;
							if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum != 0) {
								blockObject.GetComponent<Block> ().gridStateNum = grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum;
							}
							switch (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].mirrorFacingDirection) {
							case Grid.MirrorFacingDirection.LD:
								blockObject.GetComponent<Block> ().sprite.sprite = trueLDMirror;
								blockObject.GetComponent<Mirror> ().facingDirection = Mirror.FacingDirection.LD;
								break;
							case Grid.MirrorFacingDirection.LU:
								blockObject.GetComponent<Block> ().sprite.sprite = trueLUMirror;
								blockObject.GetComponent<Mirror> ().facingDirection = Mirror.FacingDirection.LU;
								break;
							case Grid.MirrorFacingDirection.RD:
								blockObject.GetComponent<Block> ().sprite.sprite = trueRDMirror;
								blockObject.GetComponent<Mirror> ().facingDirection = Mirror.FacingDirection.RD;
								break;
							case Grid.MirrorFacingDirection.RU:
								blockObject.GetComponent<Block> ().sprite.sprite = trueRUMirror;
								blockObject.GetComponent<Mirror> ().facingDirection = Mirror.FacingDirection.RU;
								break;
							}
						} else {
							blockObject.GetComponent<Block> ().inPuzzle = false;
							switch (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].mirrorFacingDirection) {
							case Grid.MirrorFacingDirection.LD:
								blockObject.GetComponent<Block> ().sprite.sprite = falseLDMirror;
								blockObject.GetComponent<Mirror> ().facingDirection = Mirror.FacingDirection.LD;
								break;
							case Grid.MirrorFacingDirection.LU:
								blockObject.GetComponent<Block> ().sprite.sprite = falseLUMirror;
								blockObject.GetComponent<Mirror> ().facingDirection = Mirror.FacingDirection.LU;
								break;
							case Grid.MirrorFacingDirection.RD:
								blockObject.GetComponent<Block> ().sprite.sprite = falseRDMirror;
								blockObject.GetComponent<Mirror> ().facingDirection = Mirror.FacingDirection.RD;
								break;
							case Grid.MirrorFacingDirection.RU:
								blockObject.GetComponent<Block> ().sprite.sprite = falseRUMirror;
								blockObject.GetComponent<Mirror> ().facingDirection = Mirror.FacingDirection.RD;
								break;
							}
						}
						break;
					case Grid.BlockType.RECEIVER:
						blockObject = Instantiate (receiverBlock, spawnPoint, myTransform.rotation, blocks) as GameObject;
						blockObject.GetComponent<Block> ().SetOrderInLayer (5 * (yDim - y) + _layerCount * 100);
						blockObject.GetComponent<Block> ().x = x - 6;
						blockObject.GetComponent<Block> ().y = y - 6;
						blockObject.GetComponent<Block> ().gridStateNum = grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum;
						stageObject.GetComponent<Stage> ().blockObjects.Add (blockObject.transform);
						blockObject.GetComponent<ReceiverBlock> ().SetOrderInLayer (5 * (yDim - y) + 2 + _layerCount * 100);
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable) {
							blockObject.GetComponent<Block> ().horMovable = true;
						}
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
							blockObject.GetComponent<Block> ().verMovable = true;
						}
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].inPuzzle) {
							blockObject.GetComponent<Block> ().inPuzzle = true;
							if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum != 0) {
								blockObject.GetComponent<Block> ().gridStateNum = grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum;
							}
							switch (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].facingDirection) {
							case Grid.FacingDirection.UP:
								blockObject.GetComponent<Block> ().sprite.sprite = trueUUReceiver;
								blockObject.GetComponent<ReceiverBlock> ().sprite.sprite = trueUDReceiver;
								blockObject.GetComponent<ReceiverBlock> ().facingDirection = ReceiverBlock.FacingDirection.UP;
								break;
							case Grid.FacingDirection.DOWN:
								blockObject.GetComponent<Block> ().sprite.sprite = trueDUReceiver;
								blockObject.GetComponent<ReceiverBlock> ().sprite.sprite = trueDDReceiver;
								blockObject.GetComponent<ReceiverBlock> ().facingDirection = ReceiverBlock.FacingDirection.DOWN;
								break;
							case Grid.FacingDirection.RIGHT:
								blockObject.GetComponent<Block> ().sprite.sprite = trueRUReceiver;
								blockObject.GetComponent<ReceiverBlock> ().sprite.sprite = trueRDReceiver;
								blockObject.GetComponent<ReceiverBlock> ().facingDirection = ReceiverBlock.FacingDirection.RIGHT;
								break;
							case Grid.FacingDirection.LEFT:
								blockObject.GetComponent<Block> ().sprite.sprite = trueLUReceiver;
								blockObject.GetComponent<ReceiverBlock> ().sprite.sprite = trueLDReceiver;
								blockObject.GetComponent<ReceiverBlock> ().facingDirection = ReceiverBlock.FacingDirection.LEFT;
								break;
							}
						} else {
							blockObject.GetComponent<Block> ().inPuzzle = false;
							switch (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].facingDirection) {
							case Grid.FacingDirection.UP:
								blockObject.GetComponent<Block> ().sprite.sprite = falseUUReceiver;
								blockObject.GetComponent<ReceiverBlock> ().sprite.sprite = falseUDReceiver;
								blockObject.GetComponent<ReceiverBlock> ().facingDirection = ReceiverBlock.FacingDirection.UP;
								break;
							case Grid.FacingDirection.DOWN:
								blockObject.GetComponent<Block> ().sprite.sprite = falseDUReceiver;
								blockObject.GetComponent<ReceiverBlock> ().sprite.sprite = falseDDReceiver;
								blockObject.GetComponent<ReceiverBlock> ().facingDirection = ReceiverBlock.FacingDirection.DOWN;
								break;
							case Grid.FacingDirection.RIGHT:
								blockObject.GetComponent<Block> ().sprite.sprite = falseRUReceiver;
								blockObject.GetComponent<ReceiverBlock> ().sprite.sprite = falseRDReceiver;
								blockObject.GetComponent<ReceiverBlock> ().facingDirection = ReceiverBlock.FacingDirection.RIGHT;
								break;
							case Grid.FacingDirection.LEFT:
								blockObject.GetComponent<Block> ().sprite.sprite = falseLUReceiver;
								blockObject.GetComponent<ReceiverBlock> ().sprite.sprite = falseLDReceiver;
								blockObject.GetComponent<ReceiverBlock> ().facingDirection = ReceiverBlock.FacingDirection.LEFT;
								break;
							}
						}
						break;
					case Grid.BlockType.BUTTON:
						blockObject = Instantiate (buttonBlock, spawnPoint, myTransform.rotation, blocks) as GameObject;
						blockObject.GetComponent<Block> ().SetOrderInLayer (5 * (yDim - y) + 2 + _layerCount * 100);
						blockObject.GetComponent<Block> ().x = x - 6;
						blockObject.GetComponent<Block> ().y = y - 6;
						blockObject.GetComponent<Block> ().gridStateNum = grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum;
						stageObject.GetComponent<Stage> ().blockObjects.Add (blockObject.transform);
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable) {
							blockObject.GetComponent<Block> ().horMovable = true;
						}
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
							blockObject.GetComponent<Block> ().verMovable = true;
						}
						switch (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].facingDirection) {
						case Grid.FacingDirection.UP:
							blockObject.GetComponent<Block> ().sprite.sprite = buttonUUnPushed;
							blockObject.GetComponent<ButtonInteractable> ().pushedSprite = buttonUPushed;
							blockObject.GetComponent<ButtonInteractable> ().unPushedSprite = buttonUUnPushed;
							blockObject.GetComponent<ButtonInteractable> ().facingDirection = ButtonInteractable.FacingDirection.UP;
							break;
						case Grid.FacingDirection.DOWN:
							blockObject.GetComponent<Block> ().sprite.sprite = buttonDUnPushed;
							blockObject.GetComponent<ButtonInteractable> ().pushedSprite = buttonDPushed;
							blockObject.GetComponent<ButtonInteractable> ().unPushedSprite = buttonDUnPushed;
							blockObject.GetComponent<ButtonInteractable> ().facingDirection = ButtonInteractable.FacingDirection.DOWN;
							break;
						case Grid.FacingDirection.RIGHT:
							blockObject.GetComponent<Block> ().sprite.sprite = buttonRUnPushed;
							blockObject.GetComponent<ButtonInteractable> ().pushedSprite = buttonRPushed;
							blockObject.GetComponent<ButtonInteractable> ().unPushedSprite = buttonRUnPushed;
							blockObject.GetComponent<ButtonInteractable> ().facingDirection = ButtonInteractable.FacingDirection.RIGHT;
							break;
						case Grid.FacingDirection.LEFT:
							blockObject.GetComponent<Block> ().sprite.sprite = buttonLUnPushed;
							blockObject.GetComponent<ButtonInteractable> ().pushedSprite = buttonLPushed;
							blockObject.GetComponent<ButtonInteractable> ().unPushedSprite = buttonLUnPushed;
							blockObject.GetComponent<ButtonInteractable> ().facingDirection = ButtonInteractable.FacingDirection.LEFT;
							break;
						}
						break;
					case Grid.BlockType.BEACON:
						blockObject = Instantiate (beacon, spawnPoint, myTransform.rotation, blocks) as GameObject;
						blockObject.GetComponent<Block> ().SetOrderInLayer (5 * (yDim - y) + 2 + _layerCount * 100);
						blockObject.GetComponent<Block> ().x = x - 6;
						blockObject.GetComponent<Block> ().y = y - 6;
						blockObject.GetComponent<Block> ().gridStateNum = grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum;
						stageObject.GetComponent<Stage> ().blockObjects.Add (blockObject.transform);
						blockObject.GetComponent<Beacon> ().spriteRenderer.sortingOrder = (5 * (yDim - y) + 3 + _layerCount * 100);
						break;
					case Grid.BlockType.ACTIVATER:
						blockObject = Instantiate (activationBlock, spawnPoint, myTransform.rotation, blocks) as GameObject;
						blockObject.GetComponent<Block> ().SetOrderInLayer (5 * (yDim - y) + 2 + _layerCount * 100);
						blockObject.GetComponent<ActivationBlock> ().SetOrderInLayer (5 * (yDim - y) + 3 + _layerCount * 100);
						blockObject.GetComponent<Block> ().x = x - 6;
						blockObject.GetComponent<Block> ().y = y - 6;
						blockObject.GetComponent<Block> ().gridStateNum = grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum;
						stageObject.GetComponent<Stage> ().blockObjects.Add (blockObject.transform);
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].horMovable) {
							blockObject.GetComponent<Block> ().horMovable = true;
						}
						if (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].verMovable) {
							blockObject.GetComponent<Block> ().verMovable = true;
						}
						switch (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].activateeType) {
						case Grid.ActivateType.CLOCKWISE:
							blockObject.GetComponent<ActivationBlock> ().activateeType = ActivationBlock.ActivateType.CLOCKWISE;
							blockObject.GetComponent<ActivationBlock> ().sprite.sprite = clockwise;
							break;
						case Grid.ActivateType.UNCLOCKWISE:
							blockObject.GetComponent<ActivationBlock> ().activateeType = ActivationBlock.ActivateType.UNCLOCKWISE;
							blockObject.GetComponent<ActivationBlock> ().sprite.sprite = unClockwise;
							break;
						case Grid.ActivateType.HORMOVEABLE:
							blockObject.GetComponent<ActivationBlock> ().activateeType = ActivationBlock.ActivateType.HORMOVEABLE;
							blockObject.GetComponent<ActivationBlock> ().sprite.sprite = horMovable;
							break;
						case Grid.ActivateType.VERMOVABLE:
							blockObject.GetComponent<ActivationBlock> ().activateeType = ActivationBlock.ActivateType.VERMOVABLE;
							blockObject.GetComponent<ActivationBlock> ().sprite.sprite = verMovable;
							break;
						case Grid.ActivateType.RAY:
							blockObject.GetComponent<ActivationBlock> ().activateeType = ActivationBlock.ActivateType.RAY;
							blockObject.GetComponent<ActivationBlock> ().sprite.sprite = ray;
							break;
						}
						break;
					case Grid.BlockType.PATHBLOCK:
						blockObject = Instantiate (pathBlock, spawnPoint, myTransform.rotation, blocks) as GameObject;
						blockObject.GetComponent<Block> ().SetOrderInLayer (5 * (yDim - y) + 2 + _layerCount * 100);
						blockObject.transform.FindChild ("Colliders").GetComponent<PathBlock> ().spriteRenderer.sortingOrder = (5 * (yDim - y) + 3 + _layerCount * 100);
						blockObject.GetComponent<Block> ().x = x - 6;
						blockObject.GetComponent<Block> ().y = y - 6;
						blockObject.GetComponent<Block> ().gridStateNum = grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].gridStateNum;
						stageObject.GetComponent<Stage> ().blockObjects.Add (blockObject.transform);
						switch (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].facingDirection) {
						case Grid.FacingDirection.UP:
							blockObject.transform.FindChild ("Colliders").GetComponent<PathBlock> ().facingDirection = PathBlock.FacingDirection.UP;
							break;
						case Grid.FacingDirection.DOWN:
							blockObject.transform.FindChild ("Colliders").GetComponent<PathBlock> ().facingDirection = PathBlock.FacingDirection.DOWN;
							break;
						case Grid.FacingDirection.RIGHT:
							blockObject.transform.FindChild ("Colliders").GetComponent<PathBlock> ().facingDirection = PathBlock.FacingDirection.RIGHT;
							break;
						case Grid.FacingDirection.LEFT:
							blockObject.transform.FindChild ("Colliders").GetComponent<PathBlock> ().facingDirection = PathBlock.FacingDirection.LEFT;
							break;
						}
						switch (grid.gridState [_index].yBlockStruct [y].xBlockStruct [x].pathType) {
						case Grid.PathType.LINE:
							blockObject.transform.FindChild ("Colliders").GetComponent<PathBlock> ().pathType = PathBlock.PathType.LINE;
							break;
						case Grid.PathType.CURVE:
							blockObject.transform.FindChild ("Colliders").GetComponent<PathBlock> ().pathType = PathBlock.PathType.CURVE;
							break;
						case Grid.PathType.T:
							blockObject.transform.FindChild ("Colliders").GetComponent<PathBlock> ().pathType = PathBlock.PathType.T;
							break;
						case Grid.PathType.CROSS:
							blockObject.transform.FindChild ("Colliders").GetComponent<PathBlock> ().pathType = PathBlock.PathType.CROSS;
							break;
						}
						break;
					}
				}
			}
		}
	}
}
