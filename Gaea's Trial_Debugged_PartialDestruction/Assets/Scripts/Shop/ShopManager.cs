using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour {
	public static ShopManager shopManager;
	public GemGoldScript gemGoldScript;

	public ItemData itemData;
	public Transform[] rouletteBackGrounds;
	public Transform rolledItem;

	public Text goldText;
	public Text gemText;
	public Text castleLevelText;

	public GameObject freeGemButton;
	public GameObject roulette;
	public GameObject rollSkipButton;
	public GameObject rollBackButton;
	public GameObject rollButtons;
	public GameObject buyBox;
	public GameObject alertBox;

	public int freeGemDice;
	public int diceType, diceRating, diceItem;
	public int shopItemRating;
	public int emptyHWeaponSlotNum;
	public int emptyMWeaponSlotNum;
	public int castleLevel;

	public void checkEmptySlotNum (int num) {
		if (num == 0) {
			for (int i = 0; i < 21; i++) {
				if (i >= 0 && i < 20) {
					if (PlayerPrefs.GetInt ("invenHItemNum" + i) == 0) {
						emptyHWeaponSlotNum = i;
						break;
					}
				} else if (i == 20) {
					emptyHWeaponSlotNum = i;
					break;
				}
			}
		} else if (num == 1) {
			for (int i = 0; i < 21; i++) {
				if (i >= 0 && i < 20) {
					if (PlayerPrefs.GetInt ("invenMItemNum" + i) == 0) {
						emptyMWeaponSlotNum = i;
						break;
					}
				} else if (i == 20) {
					emptyMWeaponSlotNum = i;
					break;
				}
			}
		}
	}

	public void RollGoldItem () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		checkEmptySlotNum (0);
		checkEmptySlotNum (1);
		if (gemGoldScript.MinusGold (5000)) {
			if (emptyHWeaponSlotNum != 20 && emptyMWeaponSlotNum != 20) {
				for (int i = 0; i < rouletteBackGrounds.Length; i++) {
					int diceItemTemp = 0, diceRatingTemp = 0, diceTypeTemp;
					diceTypeTemp = Random.Range (0, 2);
					if (diceTypeTemp == 0) {
						diceRatingTemp = Random.Range (1, 101);
						if (diceRatingTemp >= 1 && diceRatingTemp < 58) {
							diceRatingTemp = 1;
							diceItemTemp = Random.Range (1, 20);
						} else if (diceRatingTemp >= 58 && diceRatingTemp < 83) {
							diceRatingTemp = 2;
							diceItemTemp = Random.Range (31, 47);
						} else if (diceRatingTemp >= 83 && diceRatingTemp < 101) {
							diceRatingTemp = 3;
							diceItemTemp = Random.Range (61, 72);
						}
						rouletteBackGrounds[i].FindChild ("Item").GetComponent<Image> ().sprite = itemData.heroItemList [diceItemTemp].heroItemSprite;
					} else if (diceTypeTemp == 1) {
						diceRatingTemp = Random.Range (1, 101);
						if (diceRatingTemp >= 1 && diceRatingTemp < 58) {
							diceRatingTemp = 1;
							diceItemTemp = Random.Range (1, 13);
						} else if (diceRatingTemp >= 58 && diceRatingTemp < 83) {
							diceRatingTemp = 2;
							diceItemTemp = Random.Range (16, 28);
						} else if (diceRatingTemp >= 83 && diceRatingTemp < 101) {
							diceRatingTemp = 3;
							diceItemTemp = Random.Range (31, 43);
						}
						rouletteBackGrounds[i].FindChild ("Item").GetComponent<Image> ().sprite = itemData.magicianItemList [diceItemTemp].magicianItemSprite;
					}
					rouletteBackGrounds [i].FindChild ("Rating").GetComponent<Image> ().sprite = itemData.ratingSprite [diceRatingTemp];
				}
				diceType = Random.Range (1, 101); // 1 ~ 101
				if (diceType >= 1 && diceType < 61) {
					diceRating = Random.Range (1, 101);
					if (diceRating >= 1 && diceRating < 78) {
						shopItemRating = 1;
						diceItem = Random.Range (1, 20);
					} else if (diceRating >= 78 && diceRating < 98) {
						shopItemRating = 2;
						diceItem = Random.Range (31, 47);
					} else if (diceRating >= 98 && diceRating < 101) {
						shopItemRating = 3;
						diceItem = Random.Range (61, 72); // 61 ~ 71
					}
					PlayerPrefs.SetInt ("invenHItemNum" + emptyHWeaponSlotNum, diceItem);
					rolledItem.FindChild ("Item").GetComponent<Image> ().sprite = itemData.heroItemList [diceItem].heroItemSprite;
					checkEmptySlotNum (0);
				} else if (diceType >= 61 && diceType < 101) {
					diceRating = Random.Range (1, 101);
					if (diceRating >= 1 && diceRating < 78) {
						shopItemRating = 1;
						diceItem = Random.Range (1, 13);
					} else if (diceRating >= 78 && diceRating < 98) {
						shopItemRating = 2;
						diceItem = Random.Range (16, 28);
					} else if (diceRating >= 98 && diceRating < 101) {
						shopItemRating = 3;
						diceItem = Random.Range (31, 43);
					}
					PlayerPrefs.SetInt ("invenMItemNum" + emptyMWeaponSlotNum, diceItem);
					rolledItem.FindChild ("Item").GetComponent<Image> ().sprite = itemData.magicianItemList [diceItem].magicianItemSprite;
					checkEmptySlotNum (1);
				}
				rolledItem.FindChild ("Rating").GetComponent<Image> ().sprite = itemData.ratingSprite [shopItemRating];
				roulette.GetComponent<Animator> ().SetTrigger ("Roll");
				SetText ();
				rollButtons.SetActive (false);
				rollSkipButton.SetActive (true);
			} else {
				Debug.LogError ("Inventory is full.");
			}
		}
	}

	public void RollLessGemItem () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		checkEmptySlotNum (0);
		checkEmptySlotNum (1);
		if (gemGoldScript.MinusGem (50)) {
			if (emptyHWeaponSlotNum != 20 && emptyMWeaponSlotNum != 20) {
				for (int i = 0; i < rouletteBackGrounds.Length; i++) {
					int diceItemTemp = 0, diceRatingTemp = 0, diceTypeTemp;
					diceTypeTemp = Random.Range (0, 2);
					if (diceTypeTemp == 0) {
						diceRatingTemp = Random.Range (1, 101);
						if (diceRatingTemp >= 1 && diceRatingTemp < 71) {
							diceRatingTemp = 2;
							diceItemTemp = Random.Range (31, 47);
						} else if (diceRatingTemp >= 71 && diceRatingTemp < 91) {
							diceRatingTemp = 3;
							diceItemTemp = Random.Range (61, 72);
						} else if (diceRatingTemp >= 91 && diceRatingTemp < 101) {
							diceRatingTemp = 4;
							diceItemTemp = Random.Range (86, 95);
						}
						rouletteBackGrounds[i].FindChild ("Item").GetComponent<Image> ().sprite = itemData.heroItemList [diceItemTemp].heroItemSprite;
					} else if (diceTypeTemp == 1) {
						diceRatingTemp = Random.Range (1, 101);
						if (diceRatingTemp >= 1 && diceRatingTemp < 71) {
							diceRatingTemp = 2;
							diceItemTemp = Random.Range (16, 28);
						} else if (diceRatingTemp >= 71 && diceRatingTemp < 91) {
							diceRatingTemp = 3;
							diceItemTemp = Random.Range (31, 43);
						} else if (diceRatingTemp >= 91 && diceRatingTemp < 101) {
							diceRatingTemp = 4;
							diceItemTemp = Random.Range (46, 58);
						}
						rouletteBackGrounds[i].FindChild ("Item").GetComponent<Image> ().sprite = itemData.magicianItemList [diceItemTemp].magicianItemSprite;
					}
					rouletteBackGrounds [i].FindChild ("Rating").GetComponent<Image> ().sprite = itemData.ratingSprite [diceRatingTemp];
				}
				diceType = Random.Range (1, 101); // 1 ~ 101
				if (diceType >= 1 && diceType < 61) {
					diceRating = Random.Range (1, 101);
					if (diceRating >= 1 && diceRating < 83) {
						shopItemRating = 2;
						diceItem = Random.Range (31, 47);
					} else if (diceRating >= 83 && diceRating < 98) {
						shopItemRating = 3;
						diceItem = Random.Range (61, 72); // 61 ~ 71
					} else if (diceRating >= 98 && diceRating < 101) {
						shopItemRating = 4;
						diceItem = Random.Range (86, 95);
					}
					PlayerPrefs.SetInt ("invenHItemNum" + emptyHWeaponSlotNum, diceItem);
					rolledItem.FindChild ("Item").GetComponent<Image> ().sprite = itemData.heroItemList [diceItem].heroItemSprite;
					checkEmptySlotNum (0);
				} else if (diceType >= 61 && diceType < 101) {
					diceRating = Random.Range (1, 101);
					if (diceRating >= 1 && diceRating < 83) {
						shopItemRating = 2;
						diceItem = Random.Range (16, 28);
					} else if (diceRating >= 83 && diceRating < 98) {
						shopItemRating = 3;
						diceItem = Random.Range (31, 43);
					} else if (diceRating >= 98 && diceRating < 101) {
						shopItemRating = 4;
						diceItem = Random.Range (46, 58);
					}
					PlayerPrefs.SetInt ("invenMItemNum" + emptyMWeaponSlotNum, diceItem);
					rolledItem.FindChild ("Item").GetComponent<Image> ().sprite = itemData.magicianItemList [diceItem].magicianItemSprite;
					checkEmptySlotNum (1);
				}
				rolledItem.FindChild ("Rating").GetComponent<Image> ().sprite = itemData.ratingSprite [shopItemRating];
				roulette.GetComponent<Animator> ().SetTrigger ("Roll");
				SetText ();
				rollButtons.SetActive (false);
				rollSkipButton.SetActive (true);
			} else {
				Debug.LogError ("Inventory is full.");
			}
		}
	}

	public void RollLotsGemItem () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		checkEmptySlotNum (0);
		checkEmptySlotNum (1);
		if (gemGoldScript.MinusGem (200)) {
			if (emptyHWeaponSlotNum != 20 && emptyMWeaponSlotNum != 20) {
				for (int i = 0; i < rouletteBackGrounds.Length; i++) {
					int diceItemTemp = 0, diceRatingTemp = 0, diceTypeTemp;
					diceTypeTemp = Random.Range (0, 2);
					if (diceTypeTemp == 0) {
						diceRatingTemp = Random.Range (1, 101);
						if (diceRatingTemp >= 1 && diceRatingTemp < 21) {
							diceRatingTemp = 2;
							diceItemTemp = Random.Range (31, 47);
						} else if (diceRatingTemp >= 21 && diceRatingTemp < 76) {
							diceRatingTemp = 3;
							diceItemTemp = Random.Range (61, 72);
						} else if (diceRatingTemp >= 76 && diceRatingTemp < 101) {
							diceRatingTemp = 4;
							diceItemTemp = Random.Range (86, 95);
						}
						rouletteBackGrounds[i].FindChild ("Item").GetComponent<Image> ().sprite = itemData.heroItemList [diceItemTemp].heroItemSprite;
					} else if (diceTypeTemp == 1) {
						diceRatingTemp = Random.Range (1, 101);
						if (diceRatingTemp >= 1 && diceRatingTemp < 21) {
							diceRatingTemp = 2;
							diceItemTemp = Random.Range (16, 28);
						} else if (diceRatingTemp >= 21 && diceRatingTemp < 76) {
							diceRatingTemp = 3;
							diceItemTemp = Random.Range (31, 43);
						} else if (diceRatingTemp >= 76 && diceRatingTemp < 101) {
							diceRatingTemp = 4;
							diceItemTemp = Random.Range (46, 58);
						}
						rouletteBackGrounds[i].FindChild ("Item").GetComponent<Image> ().sprite = itemData.magicianItemList [diceItemTemp].magicianItemSprite;
					}
					rouletteBackGrounds [i].FindChild ("Rating").GetComponent<Image> ().sprite = itemData.ratingSprite [diceRatingTemp];
				}
				diceType = Random.Range (1, 101); // 1 ~ 101
				if (diceType >= 1 && diceType < 61) {
					diceRating = Random.Range (1, 101);
					if (diceRating >= 1 && diceRating < 31) {
						shopItemRating = 2;
						diceItem = Random.Range (31, 47);
					} else if (diceRating >= 31 && diceRating < 81) {
						shopItemRating = 3;
						diceItem = Random.Range (61, 72); // 61 ~ 71
					} else if (diceRating >= 81 && diceRating < 101) {
						shopItemRating = 4;
						diceItem = Random.Range (86, 95);
					}
					PlayerPrefs.SetInt ("invenHItemNum" + emptyHWeaponSlotNum, diceItem);
					rolledItem.FindChild ("Item").GetComponent<Image> ().sprite = itemData.heroItemList [diceItem].heroItemSprite;
					checkEmptySlotNum (0);
				} else if (diceType >= 61 && diceType < 101) {
					diceRating = Random.Range (1, 101);
					if (diceRating >= 1 && diceRating < 31) {
						shopItemRating = 2;
						diceItem = Random.Range (16, 28);
					} else if (diceRating >= 31 && diceRating < 81) {
						shopItemRating = 3;
						diceItem = Random.Range (31, 43);
					} else if (diceRating >= 81 && diceRating < 101) {
						shopItemRating = 4;
						diceItem = Random.Range (46, 58);
					}
					PlayerPrefs.SetInt ("invenMItemNum" + emptyMWeaponSlotNum, diceItem);
					rolledItem.FindChild ("Item").GetComponent<Image> ().sprite = itemData.magicianItemList [diceItem].magicianItemSprite;
					checkEmptySlotNum (1);
				}
				rolledItem.FindChild ("Rating").GetComponent<Image> ().sprite = itemData.ratingSprite [shopItemRating];
				roulette.GetComponent<Animator> ().SetTrigger ("Roll");
				SetText ();
				rollButtons.SetActive (false);
				rollSkipButton.SetActive (true);
			} else {
				Debug.LogError ("Inventory is full.");
			}
		}
	}

	public void RollSkip () {
		roulette.GetComponent<Animator> ().SetTrigger ("Skip");
		rollSkipButton.SetActive (false);
		rollBackButton.SetActive (true);
	}

	public void RollBack () {
		roulette.GetComponent<Animator> ().SetTrigger ("Back");
		rollBackButton.SetActive (false);
		rollButtons.SetActive (true);
	}

	public void WatchAds () {
		Debug.LogError ("Ads!!!");
		freeGemButton.SetActive (false);
	}

	public void CastleUpgrade () {
		if (gemGoldScript.MinusGem (15)) {
			castleLevel++;
			PlayerPrefs.SetInt ("GridCastleLevel", castleLevel);
			SetText ();
		}
	}

	public void BuyBoxBack () {
		buyBox.SetActive (false);
	}

	public void AlertBoxBack () {
		alertBox.SetActive (false);
	}

	public void SetText() {
		goldText.text = gemGoldScript.gold.ToString();
		gemText.text = gemGoldScript.gem.ToString ();
		castleLevelText.text = "Lv. " + castleLevel.ToString () + "\n" + (100 + castleLevel * 15).ToString ()  + "\nUpgrade";
	}

	public void Back () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 3);
	}

	void Awake () {
		shopManager = this;
		for (int i = 0; i < 20; i++) {
			if (PlayerPrefs.GetInt("invenHItemNum" + i) == 0) {
				emptyHWeaponSlotNum = i;
				break;
			}
		}
		for (int i = 0; i < 20; i++) {
			if (PlayerPrefs.GetInt("invenMItemNum" + i) == 0) {
				emptyMWeaponSlotNum = i;
				break;
			}
		}
		castleLevel = PlayerPrefs.GetInt ("GridCastleLevel");
		gemGoldScript.GemGoldAwake ();
		SetText ();
	}

	void Start () {
		freeGemDice = Random.Range (1, 101);
		if (freeGemDice > 90)
			freeGemButton.gameObject.SetActive (true);
	}
}
