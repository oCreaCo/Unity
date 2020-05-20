using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationBlock : MonoBehaviour {

	public SpriteRenderer sprite;
	public int orderInLayer;
	[SerializeField] private bool activated;
	private Transform myTransform;

	[SerializeField] private GameObject pathCollider2DObject;
	[SerializeField] private Transform upSpawnPoint;
	[SerializeField] private Transform downSpawnPoint;
	[SerializeField] private Transform rightSpawnPoint;
	[SerializeField] private Transform leftSpawnPoint;
	[SerializeField] private GameObject upCollider, downCollider, rightCollider, leftCollider;

	public enum ActivateType
	{
		CLOCKWISE,
		UNCLOCKWISE,
		HORMOVEABLE,
		VERMOVABLE,
		RAY,
	}

	public ActivateType activateeType;
	[SerializeField] private Sprite trueSprite;
	[SerializeField] private Sprite trueHor;
	[SerializeField] private Sprite trueVer;

	void Awake () {
		myTransform = this.transform;
		upCollider = Instantiate (pathCollider2DObject, upSpawnPoint.position, upSpawnPoint.rotation, myTransform) as GameObject;
		downCollider = Instantiate (pathCollider2DObject, downSpawnPoint.position, downSpawnPoint.rotation, myTransform) as GameObject;
		rightCollider = Instantiate (pathCollider2DObject, rightSpawnPoint.position, rightSpawnPoint.rotation, myTransform) as GameObject;
		leftCollider = Instantiate (pathCollider2DObject, leftSpawnPoint.position, leftSpawnPoint.rotation, myTransform) as GameObject;
		upCollider.GetComponent<PathCollider2D> ().actiBlock = this;
		downCollider.GetComponent<PathCollider2D> ().actiBlock = this;
		rightCollider.GetComponent<PathCollider2D> ().actiBlock = this;
		leftCollider.GetComponent<PathCollider2D> ().actiBlock = this;
	}

	public void Trigger (bool _bool) {
		if ((_bool && !activated) || (!_bool && activated)) {
			if (_bool) {
				sprite.color = new Color32 (0, 255, 171, 255);
				activated = true;
			} else {
				sprite.color = new Color32 (0, 0, 0, 255);
				activated = false;
			}
		switch (activateeType) {
		case ActivateType.CLOCKWISE:
		
			break;
		case ActivateType.UNCLOCKWISE:
		
			break;
		case ActivateType.HORMOVEABLE:
			GameMaster.gameMaster.temp [GameMaster.gameMaster.layerCount - 1].GetComponent<Block> ().horMovable = _bool;
			if (_bool) {
				GameMaster.gameMaster.temp [GameMaster.gameMaster.layerCount - 1].GetComponent<Block> ().sprite.sprite = trueHor;
			} else {
				GameMaster.gameMaster.temp [GameMaster.gameMaster.layerCount - 1].GetComponent<Block> ().sprite.sprite = trueSprite;
			}
			break;
		case ActivateType.VERMOVABLE:
			GameMaster.gameMaster.temp [GameMaster.gameMaster.layerCount - 1].GetComponent<Block> ().verMovable = _bool;
			if (_bool) {
				GameMaster.gameMaster.temp [GameMaster.gameMaster.layerCount - 1].GetComponent<Block> ().sprite.sprite = trueVer;
			} else {
				GameMaster.gameMaster.temp [GameMaster.gameMaster.layerCount - 1].GetComponent<Block> ().sprite.sprite = trueSprite;
			}
			break;
		case ActivateType.RAY:
			GameMaster.gameMaster.temp [GameMaster.gameMaster.layerCount - 1].GetComponent<ShooterBlock> ().Activate (_bool);
			break;
			}
		}
	}

	public void SetOrderInLayer (int _layerNum) {
		orderInLayer = _layerNum;
		sprite.sortingOrder = _layerNum;
	}

	public void AddSubOrderInLayer (bool _bool) {
		if (_bool) {
			orderInLayer += 5;
		} else {
			orderInLayer -= 5;
		}
		sprite.sortingOrder = orderInLayer;
	}
}
