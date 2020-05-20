using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	
	public string name;

	public Sprite sprite;

	public int itemX = 0, itemY = 0;
	public int slotNum = 0, itemNum = 0;
	public int itemUnlocked = 0, itemRating = 0, itemLevel = 0;

	public int invenType;

	void OnMouseEnter () {
		//if (this.transform.parent.parent.GetComponent<InventoryController> ().selectedSlot != null) {
			if (invenType == 0) {
				if (itemUnlocked != 0 && InventoryManager.inventoryManager.inventoryClickable && !transform.parent.parent.GetComponent<InventoryController> ().seeingInfo) {
					transform.parent.parent.GetComponent<InventoryController> ().selectedItem = this.transform;
				}
			} else {
				if (itemNum != 0 && InventoryManager.inventoryManager.inventoryClickable && !transform.parent.parent.GetComponent<InventoryController> ().seeingInfo) {
					transform.parent.parent.GetComponent<InventoryController> ().selectedItem = this.transform;
				}
			}
		//Debug.LogError (slotNum + " Selected");
		//}
	}

	void OnMouseExit () {
		if (!transform.parent.parent.GetComponent<InventoryController> ().canDragItem && !transform.parent.parent.GetComponent<InventoryController> ().seeingInfo) { 
			transform.parent.parent.GetComponent<InventoryController> ().selectedItem = null;
		}
	}
}
