using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class InventoryManager : MonoBehaviour {
	
	public static InventoryManager inventoryManager;
	private GemGoldScript gemGoldScript;

	public Text goldText;
	public Text gemText;

	public int gold = 0;

	public bool heroInventoryFront = true;
	public bool hWeaponInventoryFront = false;
	public bool mWeaponInventoryFront = false;

	public bool inventoryClickable = true;
	public InventoryController inventory;

	public int preInven;
	public int emptyHWeaponSlotNum;
	public int emptyMWeaponSlotNum;

	public void InvenFront (int inven) {
		switch (preInven) {
		case 0:
			heroInventoryFront = false;
			break;
		case 1:
			hWeaponInventoryFront = false;
			break;
		case 2:
			mWeaponInventoryFront = false;
			break;
		}
		SaveInven ();
		preInven = inven;
		switch (inven) {
		case 0:
			heroInventoryFront = true;
			break;
		case 1:
			hWeaponInventoryFront = true;
			break;
		case 2:
			mWeaponInventoryFront = true;
			break;
		}
		inventory.ClearAndGetItem (inven);
	}

	public void HeroFront () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		InvenFront (0);
	}

	public void HWeaponFront () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		InvenFront (1);
	}

	public void MWeaponFront () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		InvenFront (2);
	}

	public void SaveInven () {
		switch (preInven) {
		case 0:
			for (int i = 0; i < 20; i++) {
				PlayerPrefs.SetInt ("invenHeroUnlocked" + i, inventory.SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemUnlocked);
				PlayerPrefs.SetInt ("invenHeroLevel" + i, inventory.SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemLevel);
			}
			break;
		case 1:
			for (int i = 0; i < 20; i++) {
				PlayerPrefs.SetInt ("invenHItemNum" + i, inventory.SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemNum);
				PlayerPrefs.SetInt ("invenHItemLevel" + i, inventory.SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemLevel);
			}
			break;
		case 2:
			for (int i = 0; i < 20; i++) {
				PlayerPrefs.SetInt ("invenMItemNum" + i, inventory.SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemNum);
				PlayerPrefs.SetInt ("invenMItemLevel" + i, inventory.SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemLevel);
			}
			break;
		}
	}

	public void Back ()
	{
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		SaveInven ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 2);
	}

	public void Shop () {
		SaveInven ();
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 3);
	}

	void Start () {
		inventoryManager = this;
		gemGoldScript = GetComponent<GemGoldScript> ();
		gemGoldScript.GemGoldAwake ();
		gold = gemGoldScript.gold;
		SetText ();
//		StartCoroutine (waitForMusic ());
		preInven = 1;
//		if (hWeaponInventoryFront) {
//			for (int i = 0; i < 21; i++) {
//				if (i >= 0 && i < 20) {
//					if (inventory.SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemNum == 0) {
//						emptyHWeaponSlotNum = i;
//						break;
//					}
//				} else if (i == 20) {
//					emptyHWeaponSlotNum = i;
//					break;
//				}
//			}
//		} else if (mWeaponInventoryFront) {
//			for (int i = 0; i < 21; i++) {
//				if (i >= 0 && i < 20) {
//					if (inventory.SlotArray [i].transform.FindChild ("Item(Clone)").GetComponent<Item> ().itemNum == 0) {
//						emptyMWeaponSlotNum = i;
//						break;
//					}
//				} else if (i == 20) {
//					emptyMWeaponSlotNum = i;
//					break;
//				}
//			}
//		}
		//HWeaponFront ();
	}

	public void SetText() {
		goldText.text = gemGoldScript.gold.ToString();
		gemText.text = gemGoldScript.gem.ToString ();
	}

//	IEnumerator waitForMusic () {
//		yield return new WaitForSeconds (0.1f);
//		AudioManager.instance.PlaySound ("BGMusic");
//	}
}
