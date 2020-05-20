using UnityEngine;
using System.Collections;

public class EquipPanel : MonoBehaviour {
	public GameObject equipItemCover;
	private Transform thisTransform;

	void Awake () {
		thisTransform = this.transform;
	}

	// Use this for initialization
	void OnMouseEnter () {
//		if (InventoryManager.inventoryManager.heroInventoryFront == true) {
//			HeroInventoryController.heroInventoryController.equippedPanel = this.transform;
//		} else if (InventoryManager.inventoryManager.hWeaponInventoryFront == true) {
//			HWeaponInventoryController.hWeaponInventoryController.equippedPanel = this.transform;
//		} else if (InventoryManager.inventoryManager.mWeaponInventoryFront == true) {
//			MWeaponInventoryController.mWeaponInventoryController.equippedPanel = this.transform;
//		}
		InventoryController.inventoryController.equippedPanel = thisTransform;
//		thisTransform.SetParent (InventoryController.inventoryController.equippedPanel);
		if (InventoryController.inventoryController.canDragItem) {
			equipItemCover.SetActive (true);
		}
	}

	void OnMouseExit () {
//		if (InventoryManager.inventoryManager.heroInventoryFront == true) {
//			HeroInventoryController.heroInventoryController.equippedPanel = null;
//		} else if (InventoryManager.inventoryManager.hWeaponInventoryFront == true) {
//			HWeaponInventoryController.hWeaponInventoryController.equippedPanel = null;
//		} else if (InventoryManager.inventoryManager.mWeaponInventoryFront == true) {
//			MWeaponInventoryController.mWeaponInventoryController.equippedPanel = null;
//		}
		InventoryController.inventoryController.equippedPanel = null;
		if (InventoryController.inventoryController.canDragItem) {
			equipItemCover.SetActive (false);
		}
	}
}
