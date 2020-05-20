using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemInfo : MonoBehaviour {

	public LanguageData[] gameDBLanguageData;
	public LanguageData thisGameDBData;
	public ItemData itemData;

	public Transform particle;
	public Transform particleSpawnPoint;

	public GameObject selectedItem;
	public Text nameAndLevel;
	public Image itemSprite;
	public Image heroSprite;
	public Image itemRating;
	public Sprite[] itemRatingSprite = new Sprite[5];
	public Text stats;
	public Text info;
	public Text upgrade;
	public Text statusText;
	public string redAddValue = "";
	public string blueAddValue = "";
	public string greenAddValue = "";
	public string purpleAddValue = "";
	public string brownAddValue = "";
	public GameObject sell;

	public Button heroHorSkill;
	public Button heroVerSkill;
	public Button heroUltSkill;
	public Button magicianHorSkill;
	public Button magicianVerSkill;
	public Button magicianUltSkill;

	public GameObject skillInfoObject;
	public Image skillInfoBackGround;
	public Text skillInfo;

	public int itemNum;
	public int itemLevel;
	public int intItemRating;
	public int itemPenetration;
	public int upgradeCost;

	private bool showingInfo = false;

	public GameObject sellPanel;

	public void Back () {
		if (!InventoryManager.inventoryManager.inventoryClickable) {
			InventoryManager.inventoryManager.inventoryClickable = true;
			InventoryController.inventoryController.SetItemsColliders (true);
			InventoryController.inventoryController.seeingInfo = false;
			InventoryController.inventoryController.selectedItem = null;
			heroHorSkill.interactable = true;
			heroVerSkill.interactable = true;
			heroUltSkill.interactable = true;
			magicianHorSkill.interactable = true;
			magicianVerSkill.interactable = true;
			magicianUltSkill.interactable = true;
			this.gameObject.SetActive (false);
		}
	}

	public void SetItem (GameObject _selectedItem, int _itemNum, int _itemLevel, int _itemRating) {
		selectedItem = _selectedItem;
		itemNum = _itemNum;
		itemLevel = _itemLevel;
		if (InventoryManager.inventoryManager.hWeaponInventoryFront) {
			itemSprite.gameObject.SetActive (true);
			heroSprite.gameObject.SetActive (false);
			sell.SetActive(true);
			itemRating.sprite = itemRatingSprite [_itemRating];
			heroHorSkill.gameObject.SetActive (false);
			heroVerSkill.gameObject.SetActive (false);
			heroUltSkill.gameObject.SetActive (false);
			magicianHorSkill.gameObject.SetActive (false);
			magicianVerSkill.gameObject.SetActive (false);
			magicianUltSkill.gameObject.SetActive (false);
			nameAndLevel.text = thisGameDBData.gameDBHeroItemList [_itemNum].heroItemName + "\n" + "Level " + _itemLevel;
			info.text = thisGameDBData.gameDBHeroItemList [_itemNum].heroItemInfo;
			itemSprite.sprite = itemData.heroItemList [_itemNum].heroItemSprite;
			if (itemData.heroItemList [_itemNum].heroItemDamageMin [_itemLevel] != itemData.heroItemList [_itemNum].heroItemDamageMax [_itemLevel]) {
				stats.text = thisGameDBData.gameDBItemInfoText.damage + itemData.heroItemList [_itemNum].heroItemDamageMin [_itemLevel] + " ~ " + itemData.heroItemList [_itemNum].heroItemDamageMax [_itemLevel] + "\n" + thisGameDBData.gameDBItemInfoText.penetration + itemData.heroItemList [_itemNum].heroItemPenetration;
			} else if (itemData.heroItemList [_itemNum].heroItemDamageMin [_itemLevel] == itemData.heroItemList [_itemNum].heroItemDamageMax [_itemLevel]) {
				stats.text = thisGameDBData.gameDBItemInfoText.damage + itemData.heroItemList [_itemNum].heroItemDamageMin [_itemLevel] + "\n" + thisGameDBData.gameDBItemInfoText.penetration + itemData.heroItemList [_itemNum].heroItemPenetration;
			}
			if (_itemLevel < itemData.heroItemUpgradecost [_itemRating - 1].cost.Length) {
				upgrade.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + itemData.heroItemUpgradecost [_itemRating - 1].cost [_itemLevel];
				upgradeCost = itemData.heroItemUpgradecost [_itemRating - 1].cost [_itemLevel];
			} else if (_itemLevel == itemData.heroItemUpgradecost [_itemRating - 1].cost.Length) {
				upgrade.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + thisGameDBData.gameDBItemInfoText.upgradeMax;
				upgradeCost = 0;
			}
			intItemRating = _itemRating;
		} else if (InventoryManager.inventoryManager.heroInventoryFront) {
			itemSprite.gameObject.SetActive (false);
			heroSprite.gameObject.SetActive (true);
			skillInfoObject.gameObject.SetActive (false);
			heroHorSkill.gameObject.SetActive (true);
			heroVerSkill.gameObject.SetActive (true);
			heroUltSkill.gameObject.SetActive (true);
			magicianHorSkill.gameObject.SetActive (false);
			magicianVerSkill.gameObject.SetActive (false);
			magicianUltSkill.gameObject.SetActive (false);
			sell.SetActive(false);
			itemRating.sprite = itemRatingSprite [_itemRating];
			nameAndLevel.text = thisGameDBData.gameDBHeroList [_itemNum].heroName + "\n" + thisGameDBData.gameDBItemInfoText.level + (_itemLevel + 1);
			heroSprite.sprite = itemData.heroList [_itemNum].heroSprite;
			stats.text = thisGameDBData.gameDBItemInfoText.damage + itemData.heroLevelMinDamage [_itemLevel].ToString () + " ~ " + itemData.heroLevelMaxDamage [_itemLevel].ToString ();
			info.text = thisGameDBData.gameDBHeroList [_itemNum].heroInfo;
			if (_itemLevel < itemData.heroLevelUpgradeCost.Length) {
				upgrade.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + itemData.heroLevelUpgradeCost [_itemLevel];
				upgradeCost = itemData.heroLevelUpgradeCost [_itemLevel];
			} else if (_itemLevel == itemData.heroLevelUpgradeCost.Length) {
				upgrade.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + thisGameDBData.gameDBItemInfoText.upgradeMax;
				upgradeCost = 0;
			}
			intItemRating = _itemRating;
		} else if (InventoryManager.inventoryManager.mWeaponInventoryFront) {
			itemSprite.gameObject.SetActive (true);
			heroSprite.gameObject.SetActive (false);
			skillInfoObject.gameObject.SetActive (false);
			heroHorSkill.gameObject.SetActive (false);
			heroVerSkill.gameObject.SetActive (false);
			heroUltSkill.gameObject.SetActive (false);
			magicianHorSkill.gameObject.SetActive (true);
			magicianVerSkill.gameObject.SetActive (true);
			magicianUltSkill.gameObject.SetActive (true);
			sell.SetActive(true);
			itemRating.sprite = itemRatingSprite [_itemRating];
			nameAndLevel.text = thisGameDBData.gameDBMagicianItemList [_itemNum].magicianItemName + "\n" + thisGameDBData.gameDBItemInfoText.level + _itemLevel;
			itemSprite.sprite = itemData.magicianItemList [_itemNum].magicianItemSprite;
			stats.text = thisGameDBData.gameDBItemInfoText.damage + itemData.magicianItemList [itemNum].magicianItemDamage [itemLevel];
			info.text = thisGameDBData.gameDBMagicianItemList [_itemNum].magicianItemInfo;
			if (_itemLevel < itemData.magicianItemUpgradeCost [_itemRating - 1].cost.Length) {
				upgrade.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + itemData.magicianItemUpgradeCost [_itemRating - 1].cost [_itemLevel].ToString ();
				upgradeCost = itemData.magicianItemUpgradeCost [_itemRating - 1].cost [_itemLevel];
			} else if (_itemLevel == itemData.magicianItemUpgradeCost [_itemRating - 1].cost.Length) {
				upgrade.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + thisGameDBData.gameDBItemInfoText.upgradeMax;
				upgradeCost = 0;
			}
			intItemRating = _itemRating;
		}
	}

	public void UpgradeItem () {
		if (!InventoryManager.inventoryManager.inventoryClickable) {
			if (AudioManager.instance != null) {
				AudioManager.instance.PlaySound ("Click");
			}
			if (InventoryManager.inventoryManager.hWeaponInventoryFront) {
				if (selectedItem.GetComponent<Item> ().itemLevel < itemData.heroItemUpgradecost [intItemRating - 1].cost.Length) {
					if (GemGoldScript.gemGoldScript.MinusGold (upgradeCost)) {
						Instantiate (particle, particleSpawnPoint.position, particleSpawnPoint.rotation);
						if (AudioManager.instance != null) {
							AudioManager.instance.PlaySound ("Win");
						}
						selectedItem.GetComponent<Item> ().itemLevel += 1;
						itemLevel += 1;
						nameAndLevel.text = thisGameDBData.gameDBHeroItemList [itemNum].heroItemName + "\n" + thisGameDBData.gameDBItemInfoText.level + itemLevel;
						InventoryManager.inventoryManager.SetText ();
						if (itemData.heroItemList [itemNum].heroItemDamageMin [itemLevel] != itemData.heroItemList [itemNum].heroItemDamageMax [itemLevel]) {
							stats.text = thisGameDBData.gameDBItemInfoText.damage + itemData.heroItemList [itemNum].heroItemDamageMin [itemLevel] + " ~ " + itemData.heroItemList [itemNum].heroItemDamageMax [itemLevel] + "\n" + thisGameDBData.gameDBItemInfoText.penetration + itemData.heroItemList [itemNum].heroItemPenetration;
						} else if (itemData.heroItemList [itemNum].heroItemDamageMin [itemLevel] == itemData.heroItemList [itemNum].heroItemDamageMax [itemLevel]) {
							stats.text = thisGameDBData.gameDBItemInfoText.damage + itemData.heroItemList [itemNum].heroItemDamageMin [itemLevel] + "\n" + thisGameDBData.gameDBItemInfoText.penetration + itemData.heroItemList [itemNum].heroItemPenetration;
						}
						info.text = thisGameDBData.gameDBHeroItemList [itemNum].heroItemInfo;
						if (selectedItem.GetComponent<Item> ().itemLevel < itemData.heroItemUpgradecost [intItemRating - 1].cost.Length) {
							upgrade.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + itemData.heroItemUpgradecost [intItemRating - 1].cost [itemLevel];
							upgradeCost = itemData.heroItemUpgradecost [intItemRating - 1].cost [itemLevel];
						} else if (selectedItem.GetComponent<Item> ().itemLevel == itemData.heroItemUpgradecost [intItemRating - 1].cost.Length) {
							upgrade.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + thisGameDBData.gameDBItemInfoText.upgradeMax;
						}
						if (itemNum == PlayerPrefs.GetInt ("GridEquippedHWeapon")) {
							PlayerPrefs.SetInt ("GridEquippedHWeaponMinDamage", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemDamageMin [selectedItem.GetComponent<Item> ().itemLevel]);
							PlayerPrefs.SetInt ("GridEquippedHWeaponMaxDamage", itemData.heroItemList [selectedItem.GetComponent<Item> ().itemNum].heroItemDamageMax [selectedItem.GetComponent<Item> ().itemLevel]);
						}
					} else if (!GemGoldScript.gemGoldScript.MinusGold (upgradeCost)) {
						statusText.text = thisGameDBData.gameDBItemInfoText.notEnoughGold;
						statusText.GetComponent<Animator> ().SetTrigger ("Activate");
						Debug.LogError ("Not enough Gold.");
					}
				} else if (selectedItem.GetComponent<Item> ().itemLevel >= itemData.heroItemUpgradecost [intItemRating - 1].cost.Length) {
					statusText.text = thisGameDBData.gameDBItemInfoText.upgrade + " " + thisGameDBData.gameDBItemInfoText.upgradeMax;
					statusText.GetComponent<Animator> ().SetTrigger ("Activate");
					Debug.LogError ("Max Level");
				}
			}
			if (InventoryManager.inventoryManager.heroInventoryFront) {
				if (selectedItem.GetComponent<Item> ().itemLevel < itemData.heroLevelUpgradeCost.Length) {
					if (GemGoldScript.gemGoldScript.MinusGold (upgradeCost)) {
						Instantiate (particle, particleSpawnPoint.position, particleSpawnPoint.rotation);
						if (AudioManager.instance != null) {
							AudioManager.instance.PlaySound ("Win");
						}
						selectedItem.GetComponent<Item> ().itemLevel += 1;
						itemLevel += 1;
						nameAndLevel.text = thisGameDBData.gameDBHeroList [itemNum].heroName + "\n" + thisGameDBData.gameDBItemInfoText.level + (itemLevel + 1);
						InventoryManager.inventoryManager.SetText ();
						stats.text = thisGameDBData.gameDBItemInfoText.damage + itemData.heroLevelMinDamage [itemLevel].ToString () + " ~ " + itemData.heroLevelMaxDamage [itemLevel].ToString ();
						info.text = thisGameDBData.gameDBHeroList [itemNum].heroInfo;
						if (selectedItem.GetComponent<Item> ().itemLevel < itemData.heroLevelUpgradeCost.Length) {
							upgrade.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + itemData.heroLevelUpgradeCost [itemLevel].ToString ();
							upgradeCost = itemData.heroLevelUpgradeCost [itemLevel];
						} else if (selectedItem.GetComponent<Item> ().itemLevel == itemData.heroLevelUpgradeCost.Length) {
							upgrade.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + thisGameDBData.gameDBItemInfoText.upgradeMax;
						}
						if (itemNum == PlayerPrefs.GetInt ("GridEquippedHero")) {
							PlayerPrefs.SetInt ("HeroLevelMinDamage", itemData.heroLevelMinDamage [itemLevel]);
							PlayerPrefs.SetInt ("HeroLevelMaxDamage", itemData.heroLevelMaxDamage [itemLevel]);
						}
					} else if (!GemGoldScript.gemGoldScript.MinusGold (upgradeCost)) {
						statusText.text = thisGameDBData.gameDBItemInfoText.notEnoughGold;
						statusText.GetComponent<Animator> ().SetTrigger ("Activate");
						Debug.LogError ("Not enough Gold.");
					}
				} else if (selectedItem.GetComponent<Item> ().itemLevel >= itemData.heroLevelUpgradeCost.Length) {
					statusText.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + thisGameDBData.gameDBItemInfoText.upgradeMax;
					statusText.GetComponent<Animator> ().SetTrigger ("Activate");
					Debug.LogError ("Max Level");
				}
			}
			if (InventoryManager.inventoryManager.mWeaponInventoryFront) {
				if (selectedItem.GetComponent<Item> ().itemLevel < itemData.magicianItemUpgradeCost [intItemRating - 1].cost.Length) {
					if (GemGoldScript.gemGoldScript.MinusGold (upgradeCost)) {
						Instantiate (particle, particleSpawnPoint.position, particleSpawnPoint.rotation);
						if (AudioManager.instance != null) {
							AudioManager.instance.PlaySound ("Win");
						}
						selectedItem.GetComponent<Item> ().itemLevel += 1;
						itemLevel += 1;
						nameAndLevel.text = thisGameDBData.gameDBMagicianItemList [itemNum].magicianItemName + "\n" + thisGameDBData.gameDBItemInfoText.level + itemLevel;
						InventoryManager.inventoryManager.SetText ();
						stats.text = thisGameDBData.gameDBItemInfoText.damage + itemData.magicianItemList [itemNum].magicianItemDamage [itemLevel];
						info.text = thisGameDBData.gameDBMagicianItemList [itemNum].magicianItemInfo;
						if (selectedItem.GetComponent<Item> ().itemLevel < itemData.magicianItemUpgradeCost [intItemRating - 1].cost.Length) {
							upgrade.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + itemData.magicianItemUpgradeCost [intItemRating - 1].cost [itemLevel].ToString ();
							upgradeCost = itemData.magicianItemUpgradeCost [intItemRating - 1].cost [itemLevel];
						} else if (selectedItem.GetComponent<Item> ().itemLevel == itemData.magicianItemUpgradeCost [intItemRating - 1].cost.Length) {
							upgrade.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + thisGameDBData.gameDBItemInfoText.upgradeMax;
						}
						if (itemNum == PlayerPrefs.GetInt ("GridEquippedMWeapon")) {
							PlayerPrefs.SetInt ("GridEquippedMWeaponDamage", itemData.magicianItemList [selectedItem.GetComponent<Item> ().itemNum].magicianItemDamage [selectedItem.GetComponent<Item> ().itemLevel]);
						}
					} else if (!GemGoldScript.gemGoldScript.MinusGold (upgradeCost)) {
						statusText.text = thisGameDBData.gameDBItemInfoText.notEnoughGold;
						statusText.GetComponent<Animator> ().SetTrigger ("Activate");
						Debug.LogError ("Not enough Gold.");
					}
				} else if (selectedItem.GetComponent<Item> ().itemLevel >= itemData.magicianItemUpgradeCost [intItemRating - 1].cost.Length) {
					statusText.text = thisGameDBData.gameDBItemInfoText.upgrade + "\n" + thisGameDBData.gameDBItemInfoText.upgradeMax;
					statusText.GetComponent<Animator> ().SetTrigger ("Activate");
					Debug.LogError ("Max Level");
				}
			}
		}
	}

	public void SellItem () {
		if (!InventoryManager.inventoryManager.inventoryClickable) {
			if (selectedItem.transform.FindChild ("EquippedText(Clone)") == null) {
				if (AudioManager.instance != null) {
					AudioManager.instance.PlaySound ("Click");
				}
				if (InventoryManager.inventoryManager.hWeaponInventoryFront) {
					GemGoldScript.gemGoldScript.PlusGold (itemData.heroItemListSellCost [selectedItem.GetComponent<Item> ().itemRating]);
					selectedItem.GetComponent<Image> ().sprite = itemData.heroItemList [0].heroItemSprite;
				} else if (InventoryManager.inventoryManager.mWeaponInventoryFront) {
					GemGoldScript.gemGoldScript.PlusGold (itemData.magicianItemListSellCost [selectedItem.GetComponent<Item> ().itemRating]);
					selectedItem.GetComponent<Image> ().sprite = itemData.magicianItemList [0].magicianItemSprite;
				}
				selectedItem.GetComponent<Item> ().itemNum = 0;
				InventoryManager.inventoryManager.SetText ();
				selectedItem.transform.FindChild ("Rating").GetComponent<Image> ().sprite = itemData.ratingSprite [0];
				InventoryManager.inventoryManager.SaveInven ();
				sellPanel.SetActive (false);
				Back ();
			} else if (selectedItem.transform.FindChild ("EquippedText(Clone)") != null) {
				statusText.text = "Equipped";
				statusText.GetComponent<Animator> ().SetTrigger ("Activate");
			}
		}
	}

	public void SellPanelOpen () {
		sellPanel.SetActive (true);
	}

	public void SellPanelBack () {
		sellPanel.SetActive (false);

	}

	public void HorSkillInfo () {
		if (!InventoryManager.inventoryManager.inventoryClickable) {
			if (AudioManager.instance != null) {
				AudioManager.instance.PlaySound ("Click");
			}
			if (!showingInfo) {
				skillInfoObject.gameObject.SetActive (true);
				showingInfo = true;
				if (InventoryManager.inventoryManager.mWeaponInventoryFront) {
					skillInfo.text = thisGameDBData.gameDBMagicianItemList [itemNum].magicianItemSkillInfo;
					magicianVerSkill.interactable = false;
					magicianUltSkill.interactable = false;
				} else if (InventoryManager.inventoryManager.heroInventoryFront) {
					skillInfo.text = thisGameDBData.gameDBHeroList [itemNum].heroHorSkillInfo;
					heroVerSkill.interactable = false;
					heroUltSkill.interactable = false;
				}
			} else if (showingInfo) {
				if (InventoryManager.inventoryManager.mWeaponInventoryFront) {
					magicianVerSkill.interactable = true;
					magicianUltSkill.interactable = true;
				} else if (InventoryManager.inventoryManager.heroInventoryFront) {
					heroVerSkill.interactable = true;
					heroUltSkill.interactable = true;
				}
				showingInfo = false;
				skillInfoObject.gameObject.SetActive (false);
			}
		}
	}

	public void VerSkillInfo () {
		if (!InventoryManager.inventoryManager.inventoryClickable) {
			if (AudioManager.instance != null) {
				AudioManager.instance.PlaySound ("Click");
			}
			if (!showingInfo) {
				skillInfoObject.gameObject.SetActive (true);
				showingInfo = true;
				if (InventoryManager.inventoryManager.mWeaponInventoryFront) {
					skillInfo.text = thisGameDBData.gameDBMagicianItemList [itemNum].magicianItemSkillInfo;
					magicianHorSkill.interactable = false;
					magicianUltSkill.interactable = false;
				} else if (InventoryManager.inventoryManager.heroInventoryFront) {
					skillInfo.text = thisGameDBData.gameDBHeroList [itemNum].heroVerSkillInfo;
					heroHorSkill.interactable = false;
					heroUltSkill.interactable = false;
				}
			} else if (showingInfo) {
				if (InventoryManager.inventoryManager.mWeaponInventoryFront) {
					magicianHorSkill.interactable = true;
					magicianUltSkill.interactable = true;
				} else if (InventoryManager.inventoryManager.heroInventoryFront) {
					heroHorSkill.interactable = true;
					heroUltSkill.interactable = true;
				}
				showingInfo = false;
				skillInfoObject.gameObject.SetActive (false);
			}
		}
	}

	public void UltSkillInfo () {
		if (!InventoryManager.inventoryManager.inventoryClickable) {
			if (AudioManager.instance != null) {
				AudioManager.instance.PlaySound ("Click");
			}
			if (!showingInfo) {
				skillInfoObject.gameObject.SetActive (true);
				showingInfo = true;
				if (InventoryManager.inventoryManager.mWeaponInventoryFront) {
					skillInfo.text =thisGameDBData.gameDBMagicianItemList [itemNum].magicianItemUltSkillInfo;
					magicianHorSkill.interactable = false;
					magicianVerSkill.interactable = false;
				} else if (InventoryManager.inventoryManager.heroInventoryFront) {
					skillInfo.text = thisGameDBData.gameDBHeroList [itemNum].heroUltSkillInfo;
					heroHorSkill.interactable = false;
					heroVerSkill.interactable = false;
				}
			} else if (showingInfo) {
				if (InventoryManager.inventoryManager.mWeaponInventoryFront) {
					magicianHorSkill.interactable = true;
					magicianVerSkill.interactable = true;
				} else if (InventoryManager.inventoryManager.heroInventoryFront) {
					heroHorSkill.interactable = true;
					heroVerSkill.interactable = true;
				}
				showingInfo = false;
				skillInfoObject.gameObject.SetActive (false);
			}
		}
	}

	void Awake () {
		thisGameDBData = gameDBLanguageData [PlayerPrefs.GetInt ("Language")];
	}
}
