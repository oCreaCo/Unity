using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour {
	public static InventoryController inventoryController;
	public ItemData itemData;

	public Transform[] tmp;

	public Transform selectedItem, selectedSlot, equippedSlot, originalSlot, equippedPanel;

	public GameObject itemInfo;
	public GameObject slotPrefab, itemPrefab, itemRatingPrefab, equippedText;
	public Vector2 inventorySize = new Vector2 (5, 4);
	public float slotSize;
	public Vector2 windowSize;
	public bool canDragItem = false;
	public bool seeingInfo = false;
	public GameObject[] SlotArray = new GameObject[20];
	public Sprite[] invenSprites;

	public int preInven;
	public int invenType;

	public Transform hero;
	public Transform hWeapon;
	public Transform mWeapon;

	private Vector3 tempVector1 = new Vector3 (0, 0, 0);
	private Vector3 tempVector2;

	void Awake () {
		inventoryController = this;
		preInven = invenType;
		CreateInventory ();
		GetItems (invenType);
		GetEquippedText ();
	}

	void Update () {
		if (InventoryManager.inventoryManager.inventoryClickable) {
			if (Input.GetMouseButtonDown (0) && selectedItem != null) {
				canDragItem = true;
				originalSlot = selectedItem.parent;
				selectedItem.GetComponent<Collider2D> ().enabled = false;
				SetItemsColliders (false);
				selectedItem.GetComponent<Image>().color = new Color32 (255, 255, 255, 132);
				selectedItem.transform.FindChild("Rating").GetComponent<Image>().color = new Color32 (255, 255, 255, 180);
				if (selectedItem.GetComponent<Item> ().itemNum != 0) {
					switch (invenType) {
					case 1:
						equippedSlot = SlotArray [PlayerPrefs.GetInt ("invenEquippedHItemNum")].transform;
						break;
					case 2:
						equippedSlot = SlotArray [PlayerPrefs.GetInt ("invenEquippedMItemNum")].transform;
						break;
					}
				}
				else if (selectedItem.GetComponent<Item> ().itemUnlocked != 0) equippedSlot = SlotArray [PlayerPrefs.GetInt ("GridEquippedHero")].transform;
			}
			if (Input.GetMouseButton (0) && selectedItem != null && canDragItem) {
				tempVector1 = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				tempVector2 = new Vector3 (tempVector1.x, tempVector1.y, tempVector1.z + 7.5f);
				selectedItem.position = tempVector2;
			} else if (Input.GetMouseButtonUp (0) && selectedItem != null && canDragItem) {
				canDragItem = false;
				SetItemsColliders (true);
				selectedItem.GetComponent<Image>().color = new Color32 (255, 255, 255, 255);
				selectedItem.transform.FindChild("Rating").GetComponent<Image>().color = new Color32 (255, 255, 255, 255);
				if (invenType == 0) {
					selectedItem.parent = originalSlot;
					if (selectedSlot == originalSlot) {
						if (selectedSlot.FindChild ("Item(Clone)").GetComponent<Item> ().itemUnlocked != 0) {
							itemInfo.SetActive (true);
							SetItemsColliders (false);
							seeingInfo = true;
							InventoryManager.inventoryManager.inventoryClickable = false;
							itemInfo.GetComponent<ItemInfo> ().SetItem (selectedItem.gameObject, selectedItem.GetComponent<Item> ().slotNum, selectedItem.GetComponent<Item> ().itemLevel, 3);
						}
					}
				} else {
					if (selectedSlot == null) {
						selectedItem.parent = originalSlot;
					} else if (selectedSlot == originalSlot && selectedItem.GetComponent<Item> ().itemNum != 0) {
						SetItemsColliders (false);
						seeingInfo = true;
						InventoryManager.inventoryManager.inventoryClickable = false;
						itemInfo.SetActive (true);
						itemInfo.GetComponent<ItemInfo> ().SetItem (selectedItem.gameObject, selectedItem.GetComponent<Item> ().itemNum, selectedItem.GetComponent<Item> ().itemLevel, selectedItem.GetComponent<Item> ().itemRating);
					} else {
						selectedSlot.FindChild ("Item(Clone)").GetComponent<Item> ().itemX = originalSlot.GetComponent<SlotController> ().slotX;
						selectedSlot.FindChild ("Item(Clone)").GetComponent<Item> ().itemY = originalSlot.GetComponent<SlotController> ().slotY;
						selectedSlot.FindChild ("Item(Clone)").GetComponent<Item> ().slotNum = (originalSlot.GetComponent<SlotController> ().slotX + (originalSlot.GetComponent<SlotController> ().slotY - 1) * 5 - 1);
						if (selectedSlot.FindChild ("Item(Clone)").FindChild ("EquippedText(Clone)") != null) {
							if (invenType == 1) PlayerPrefs.SetInt ("invenEquippedHItemNum", originalSlot.transform.FindChild ("Item(Clone)").GetComponent<Item> ().slotNum);
							else if (invenType == 2) PlayerPrefs.SetInt ("invenEquippedMItemNum", originalSlot.transform.FindChild ("Item(Clone)").GetComponent<Item> ().slotNum);
							equippedSlot = originalSlot;
						}
						selectedSlot.FindChild ("Item(Clone)").parent = originalSlot;
						selectedItem.parent = selectedSlot;
						selectedItem.GetComponent<Item> ().itemX = selectedSlot.GetComponent<SlotController> ().slotX;
						selectedItem.GetComponent<Item> ().itemY = selectedSlot.GetComponent<SlotController> ().slotY;
						selectedItem.GetComponent<Item> ().slotNum = (selectedItem.GetComponent<Item> ().itemX + (selectedItem.GetComponent<Item> ().itemY - 1) * 5 - 1);
						if (selectedItem.transform.FindChild ("EquippedText(Clone)") != null) {
							if (invenType == 1) PlayerPrefs.SetInt ("invenEquippedHItemNum", selectedItem.transform.GetComponent<Item> ().slotNum);
							else if (invenType == 2) PlayerPrefs.SetInt ("invenEquippedMItemNum", selectedItem.transform.GetComponent<Item> ().slotNum);
							equippedSlot = selectedItem.parent;
						}
						//}
						InventoryManager.inventoryManager.SaveInven ();
					}
					originalSlot.FindChild ("Item(Clone)").localPosition = new Vector3 (0, 0, -5f);
				}
				selectedItem.localPosition = new Vector3 (0, 0, -5f);
				selectedItem.GetComponent<Collider2D> ().enabled = true;
				if (equippedPanel != null) {
					equippedPanel.GetComponent<EquipPanel> ().equipItemCover.SetActive (false);
					equippedSlot.transform.FindChild ("Item(Clone)").transform.FindChild ("EquippedText(Clone)").parent = originalSlot.transform.FindChild ("Item(Clone)");
					originalSlot.transform.FindChild ("Item(Clone)").transform.FindChild ("EquippedText(Clone)").localPosition = new Vector3 (0, 0, -5f);
					switch (invenType) {
					case 0:
						PlayerPrefs.SetInt ("GridEquippedHero", (selectedItem.GetComponent<Item> ().slotNum));
						PlayerPrefs.SetInt ("GridEquippedHeroLevel", (selectedItem.GetComponent<Item> ().itemLevel));
						PlayerPrefs.SetInt ("HeroLevelMinDamage", itemData.heroLevelMinDamage [selectedItem.GetComponent<Item> ().itemLevel]);
						PlayerPrefs.SetInt ("HeroLevelMaxDamage", itemData.heroLevelMaxDamage [selectedItem.GetComponent<Item> ().itemLevel]);
						hero.GetComponent<Animator>().runtimeAnimatorController = itemData.heroList [PlayerPrefs.GetInt ("GridEquippedHero")].animController;
						break;
					case 1:
						hWeapon.GetComponent<SpriteRenderer> ().sprite = itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemSprite;
						PlayerPrefs.SetInt ("invenEquippedHItemNum", selectedItem.GetComponent<Item> ().slotNum);
						PlayerPrefs.SetInt ("GridEquippedHWeapon", selectedItem.GetComponent<Item> ().itemNum);
						int i = 0;
						switch (itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].Type) {
						case ItemData.spAttack.NONE:
							i = 0;
							break;
						case ItemData.spAttack.FLAME:
							i = 1;
							break;
						case ItemData.spAttack.ICE:
							i = 2;
							break;
						case ItemData.spAttack.LIGHTNING:
							i = 3;
							break;
						case ItemData.spAttack.WOUNDING:
							i = 4;
							break;
						case ItemData.spAttack.PIERCE:
							i = 5;
							break;
						}
						PlayerPrefs.SetInt ("GridEquippedHWeaponType", i);
						PlayerPrefs.SetInt ("GridEquippedHWeaponChance", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].chance);
						PlayerPrefs.SetInt ("GridEquippedHWeaponRed", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemRed);
						PlayerPrefs.SetInt ("GridEquippedHWeaponBlue", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemBlue);
						PlayerPrefs.SetInt ("GridEquippedHWeaponGreen", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemGreen);
						PlayerPrefs.SetInt ("GridEquippedHWeaponPurple", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemPurple);
						PlayerPrefs.SetInt ("GridEquippedHWeaponBrown", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemYellow);
						PlayerPrefs.SetInt ("GridEquippedHWeaponMinDamage", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemDamageMin [selectedItem.GetComponent<Item> ().itemLevel]);
						PlayerPrefs.SetInt ("GridEquippedHWeaponMaxDamage", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemDamageMax [selectedItem.GetComponent<Item> ().itemLevel]);
						PlayerPrefs.SetInt ("GridEquippedHWeaponPenetration", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemPenetration);
						PlayerPrefs.SetInt ("GridEquippedHWeaponLegendSkillTime", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemLegendSkillTime);
						PlayerPrefs.SetInt ("GridEquippedHWeaponLegendCoolTime", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemLegendCoolTime);
						PlayerPrefs.SetInt ("GridEquippedHWeaponLegendSkillCount", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemLegendSkillCount);
//						equippedSlot = SlotArray [PlayerPrefs.GetInt ("invenEquippedHItemNum")].transform;
						break;
					case 2:
						mWeapon.GetComponent<SpriteRenderer> ().sprite = itemData.magicianItemList [selectedItem.GetComponent<Item> ().itemNum].magicianItemSprite;
						PlayerPrefs.SetInt ("invenEquippedMItemNum", selectedItem.GetComponent<Item> ().slotNum);
						PlayerPrefs.SetInt ("GridEquippedMWeapon", selectedItem.GetComponent<Item> ().itemNum);
						PlayerPrefs.SetInt ("GridEquippedMWeaponDamage", itemData.magicianItemList [selectedItem.GetComponent<Item> ().itemNum].magicianItemDamage [selectedItem.GetComponent<Item> ().itemLevel]);
//						equippedSlot = SlotArray [PlayerPrefs.GetInt ("invenEquippedMItemNum")].transform;
						break;
					}
				}
//				selectedItem = null;
			}
		}
	}

	public void Equip () {
		equippedPanel.GetComponent<EquipPanel> ().equipItemCover.SetActive (false);
		equippedSlot.transform.FindChild ("Item(Clone)").transform.FindChild ("EquippedText(Clone)").parent = originalSlot.transform.FindChild ("Item(Clone)");
		originalSlot.transform.FindChild ("Item(Clone)").transform.FindChild ("EquippedText(Clone)").localPosition = new Vector3 (0, 0, -5f);
		switch (invenType) {
		case 0:
			PlayerPrefs.SetInt ("GridEquippedHero", (selectedItem.GetComponent<Item> ().slotNum));
			PlayerPrefs.SetInt ("GridEquippedHeroLevel", (selectedItem.GetComponent<Item> ().itemLevel));
			PlayerPrefs.SetInt ("HeroLevelMinDamage", itemData.heroLevelMinDamage [selectedItem.GetComponent<Item> ().itemLevel]);
			PlayerPrefs.SetInt ("HeroLevelMaxDamage", itemData.heroLevelMaxDamage [selectedItem.GetComponent<Item> ().itemLevel]);
			hero.GetComponent<Animator>().runtimeAnimatorController = itemData.heroList [PlayerPrefs.GetInt ("GridEquippedHero")].animController;
			//						equippedSlot = SlotArray [PlayerPrefs.GetInt ("invenEquippedHero")].transform;
			break;
		case 1:
			hWeapon.GetComponent<SpriteRenderer> ().sprite = itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemSprite;
			PlayerPrefs.SetInt ("invenEquippedHItemNum", selectedItem.GetComponent<Item> ().slotNum);
			PlayerPrefs.SetInt ("GridEquippedHWeapon", selectedItem.GetComponent<Item> ().itemNum);
			int i = 0;
			switch (itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].Type) {
			case ItemData.spAttack.NONE:
				i = 0;
				break;
			case ItemData.spAttack.FLAME:
				i = 1;
				break;
			case ItemData.spAttack.ICE:
				i = 2;
				break;
			case ItemData.spAttack.LIGHTNING:
				i = 3;
				break;
			case ItemData.spAttack.WOUNDING:
				i = 4;
				break;
			case ItemData.spAttack.PIERCE:
				i = 5;
				break;
			}
			PlayerPrefs.SetInt ("GridEquippedHWeaponType", i);
			PlayerPrefs.SetInt ("GridEquippedHWeaponChance", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].chance);
			PlayerPrefs.SetInt ("GridEquippedHWeaponRed", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemRed);
			PlayerPrefs.SetInt ("GridEquippedHWeaponBlue", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemBlue);
			PlayerPrefs.SetInt ("GridEquippedHWeaponGreen", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemGreen);
			PlayerPrefs.SetInt ("GridEquippedHWeaponPurple", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemPurple);
			PlayerPrefs.SetInt ("GridEquippedHWeaponBrown", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemYellow);
			PlayerPrefs.SetInt ("GridEquippedHWeaponMinDamage", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemDamageMin [selectedItem.GetComponent<Item> ().itemLevel]);
			PlayerPrefs.SetInt ("GridEquippedHWeaponMaxDamage", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemDamageMax [selectedItem.GetComponent<Item> ().itemLevel]);
			PlayerPrefs.SetInt ("GridEquippedHWeaponPenetration", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemPenetration);
			PlayerPrefs.SetInt ("GridEquippedHWeaponLegendSkillTime", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemLegendSkillTime);
			PlayerPrefs.SetInt ("GridEquippedHWeaponLegendCoolTime", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemLegendCoolTime);
			PlayerPrefs.SetInt ("GridEquippedHWeaponLegendSkillCount", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemLegendSkillCount);
			//						equippedSlot = SlotArray [PlayerPrefs.GetInt ("invenEquippedHItemNum")].transform;
			break;
		case 2:
			mWeapon.GetComponent<SpriteRenderer> ().sprite = itemData.magicianItemList [selectedItem.GetComponent<Item> ().itemNum].magicianItemSprite;
			PlayerPrefs.SetInt ("invenEquippedMItemNum", selectedItem.GetComponent<Item> ().slotNum);
			PlayerPrefs.SetInt ("GridEquippedMWeapon", selectedItem.GetComponent<Item> ().itemNum);
			PlayerPrefs.SetInt ("GridEquippedMWeaponDamage", itemData.magicianItemList [selectedItem.GetComponent<Item> ().itemNum].magicianItemDamage [selectedItem.GetComponent<Item> ().itemLevel]);
			//						equippedSlot = SlotArray [PlayerPrefs.GetInt ("invenEquippedMItemNum")].transform;
			break;
		}
		itemInfo.GetComponent<ItemInfo>().statusText.text = "Equipped";
		itemInfo.GetComponent<ItemInfo>().statusText.GetComponent<Animator> ().SetTrigger ("Activate");
	}

	public void SetItemsColliders (bool b) {
		foreach (GameObject item in GameObject.FindGameObjectsWithTag("Item")) {
			item.GetComponent<Collider2D> ().enabled = b;
		}
	}

	public void GetItem (int invenType, int _slotNum, int _itemNum, int _unlocked, int _itemRating, int _itemLevel) {
		GameObject item = Instantiate (itemPrefab, SlotArray[_slotNum].transform) as GameObject;
		switch (invenType) {
		case 0:
			item.GetComponent<Item> ().itemUnlocked = _unlocked;
			if (_unlocked == 1) {
				item.GetComponent<Item> ().itemLevel = _itemLevel;
				item.GetComponent<Item> ().sprite = itemData.heroSprite [_slotNum];
				item.GetComponent<Image> ().sprite = itemData.heroSprite [_slotNum];
				item.GetComponent<RectTransform> ().sizeDelta = new Vector2 (120, 90);
				item.transform.FindChild ("Rating").GetComponent<Image> ().sprite = itemData.ratingSprite [3];
			} else if (_unlocked == 0) {
				item.GetComponent<Item> ().sprite = itemData.heroSprite [20];
				item.GetComponent<Image> ().sprite = itemData.heroSprite [20];
			}
			item.GetComponent<Item> ().invenType = 0;
			break;
		case 1:
			item.GetComponent<Item> ().sprite = itemData.heroItemList [_itemNum].heroItemSprite;
			item.GetComponent<Image> ().sprite = itemData.heroItemList [_itemNum].heroItemSprite;
//			item.GetComponent<SpriteRenderer> ().sprite = gameDB.heroItemList [_itemNum].heroItemSprite;
			item.GetComponent<Item> ().invenType = 1;
			break;
		case 2:
			item.GetComponent<Item> ().sprite = itemData.magicianItemList [_itemNum].magicianItemSprite;
			item.GetComponent<Image> ().sprite = itemData.magicianItemList [_itemNum].magicianItemSprite;
			item.GetComponent<Item> ().invenType = 2;
			break;
		}
		item.GetComponent<Item> ().slotNum = _slotNum;
		item.GetComponent<Item> ().itemNum = _itemNum;
		if (_itemNum != 0) {
			item.GetComponent<Item> ().itemRating = _itemRating;
			item.GetComponent<Item> ().itemLevel = _itemLevel;
			item.transform.FindChild ("Rating").GetComponent<Image> ().sprite = itemData.ratingSprite [_itemRating];
//			item.transform.FindChild ("Rating").GetComponent<SpriteRenderer> ().sprite = gameDB.ratingSprite [_itemRating];
		}
		item.GetComponent<RectTransform> ().localPosition = new Vector3 (0, 0, -5f);
		item.GetComponent<RectTransform> ().localScale = Vector3.one;
//		item.GetComponent<Transform> ().localPosition = new Vector3 (0, -0.2f, 0);
//		item.GetComponent<Transform> ().localScale = Vector3.one;
		Item itemScript = item.GetComponent<Item> ();
	}

	public void GetItems (int inven) {
		switch (inven) {
		case 0:
			for (int i = 0; i < 20; i++) GetItem (0, i, 0, PlayerPrefs.GetInt("invenHeroUnlocked" + i), 0, PlayerPrefs.GetInt("invenHeroLevel" + i));
			break;
		case 1:
			for (int i = 0; i < 20; i++) GetItem (1, i, PlayerPrefs.GetInt("invenHItemNum" + i), 0, itemData.heroItemList[PlayerPrefs.GetInt("invenHItemNum" + i)].heroItemRating, PlayerPrefs.GetInt("invenHItemLevel" + i));
			break;
		case 2:
			for (int i = 0; i < 20; i++) GetItem (2, i, PlayerPrefs.GetInt("invenMItemNum" + i), 0, itemData.magicianItemList[PlayerPrefs.GetInt("invenMItemNum" + i)].magicianItemRating, PlayerPrefs.GetInt("invenMItemLevel" + i));
			break;
		}
		Debug.LogError ("GetItems");
	}

	public void GetEquippedText () {
		GameObject E = Instantiate (equippedText) as GameObject;
		switch (invenType) {
		case 0:
			E.transform.parent = SlotArray[PlayerPrefs.GetInt("GridEquippedHero")].transform.FindChild("Item(Clone)").transform;
			Debug.LogError ("Hero TextEquipped " + PlayerPrefs.GetInt ("GridEquippedHero"));
			break;
		case 1:
			E.transform.parent = SlotArray[PlayerPrefs.GetInt ("invenEquippedHItemNum")].transform.FindChild("Item(Clone)").transform;
			Debug.LogError ("HWeapon TextEquipped " + PlayerPrefs.GetInt ("invenEquippedHItemNum"));
			break;
		case 2:
			E.transform.parent = SlotArray[PlayerPrefs.GetInt ("invenEquippedMItemNum")].transform.FindChild("Item(Clone)").transform;
			Debug.LogError ("MWeapon TextEquipped " + PlayerPrefs.GetInt ("invenEquippedMItemNum"));
			break;
		}
		E.GetComponent<RectTransform> ().anchoredPosition = Vector3.zero;
		E.GetComponent<RectTransform> ().localScale = Vector3.one;
	}

	public void ClearItem (int inven) {
		switch (inven) {
		case 0:
			for (int i = 0; i < 20; i++) {
				PlayerPrefs.SetInt ("invenHeroUnlocked" + i, SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemUnlocked);
				PlayerPrefs.SetInt ("invenHeroLevel" + i, SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemLevel);
				Destroy (SlotArray [i].transform.FindChild ("Item(Clone)").gameObject);
			}
			break;
		case 1:
			for (int i = 0; i < 20; i++) {
				PlayerPrefs.SetInt ("invenHItemNum" + i, SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemNum);
				PlayerPrefs.SetInt ("invenHItemLevel" + i, SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemLevel);
				Destroy (SlotArray [i].transform.FindChild ("Item(Clone)").gameObject);
			}
			break;
		case 2:
			for (int i = 0; i < 20; i++) {
				PlayerPrefs.SetInt ("invenMItemNum" + i, SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemNum);
				PlayerPrefs.SetInt ("invenMItemLevel" + i, SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemLevel);
				Destroy (SlotArray [i].transform.FindChild ("Item(Clone)").gameObject);
			}
			break;
		}
	}

	public void ClearAndGetItem (int inven) {
		GetComponent<Image> ().sprite = invenSprites [inven];
		invenType = inven;
		ClearItem (preInven);
		GetItems (inven);
		preInven = inven;
		StartCoroutine (GetEquippedTextCoroutine ());
	}

	IEnumerator GetEquippedTextCoroutine () {
		yield return new WaitForSeconds (0.01f);
		GetEquippedText ();
	}

	public void CreateInventory()
	{
		foreach (Transform t in this.transform) {
			Destroy (t.gameObject);
		}
		for (int x = 1; x <= inventorySize.x; x++) {
			for (int y = 1; y <= inventorySize.y; y++) {
				//int k = 0;
				GameObject slot = Instantiate (slotPrefab, this.transform) as GameObject;
				SlotArray [x + (y - 1) * 5 - 1] = slot;
				slot.name = "slot_0" + x + "_" + y;
				slot.GetComponent<RectTransform>().anchoredPosition = new Vector3 ((windowSize.x / (inventorySize.x + 1) * x) - windowSize.x / 2, (windowSize.y / (inventorySize.y + 1) * -y) + windowSize.y/2 - 0.25f, 0);
				slot.GetComponent<RectTransform> ().localScale = Vector3.one;
//				slot.GetComponent<Transform>().position = new Vector3 ((windowSize.x / (inventorySize.x + 1) * x) - windowSize.x / 2, (windowSize.y / (inventorySize.y + 1) * -y) + windowSize.y/2 - 2f, 0);
//				slot.GetComponent<Transform> ().localScale = Vector3.one;
				slot.GetComponent<SlotController> ().slotX = x;
				slot.GetComponent<SlotController> ().slotY = y;
			}
		}
	}
}
