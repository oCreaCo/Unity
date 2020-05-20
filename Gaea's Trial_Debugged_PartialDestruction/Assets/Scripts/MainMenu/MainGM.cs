using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainGM : MonoBehaviour {
	//public static MainGM maingm;
	public Transform saveLoader;

	public int gridEquippedHero;
	public int gridEquippedHeroLevel;
	public int invenEquippedHItemNum;
	public int invenEquippedMItemNum;
	public int gridEquippedHWeapon;
	public int gridEquippedMWeapon;
	public int gridCastleLevel;

	public int[,] inventoryItemHWeapon = new int[20, 2];
	public int[,] inventoryItemMWeapon = new int[20, 2];
	public int[,] inventoryItemHero = new int[20, 2];

	public Text gemText;
	public Text goldText;

	void Start () {
		if (!AudioManager.instance.playingBGMusic) {
			AudioManager.instance.playingBGMusic = true;
			StartCoroutine (waitForMusic ());
		}
	}

	void SetText()
	{
		gemText.text = saveLoader.GetComponent<SaveLoad>().gemGoldScript.gem.ToString ();
		goldText.text = saveLoader.GetComponent<SaveLoad>().gemGoldScript.gold.ToString ();
	}

	IEnumerator waitForMusic () {
		yield return new WaitForSeconds (0.1f);

		AudioManager.instance.PlaySound ("BGMusic");
	}

	public void OnMainAwake () {
		saveLoader.GetComponent<SaveLoad> ().Load ();

		saveLoader.GetComponent<SaveLoad>().gemGoldScript.GemGoldAwake ();

		gridEquippedHero = PlayerPrefs.GetInt ("GridEquippedHero");
		gridEquippedHeroLevel = PlayerPrefs.GetInt ("GridEquippedHeroLevel");

		gridEquippedHWeapon = PlayerPrefs.GetInt ("GridEquippedHWeapon");
		gridEquippedMWeapon = PlayerPrefs.GetInt ("GridEquippedMWeapon");

		for (int i = 0; i < 20; i++) {
			inventoryItemHero [i, 0] = PlayerPrefs.GetInt ("invenHeroUnlocked" + i);
			inventoryItemHero [i, 1] = PlayerPrefs.GetInt ("invenHeroLevel" + i);

			inventoryItemHWeapon [i, 0] = PlayerPrefs.GetInt ("invenHItemNum" + i);
			inventoryItemHWeapon [i, 1] = PlayerPrefs.GetInt ("invenHItemLevel" + i);

			inventoryItemMWeapon [i, 0] = PlayerPrefs.GetInt ("invenMItemNum" + i);
			inventoryItemMWeapon [i, 1] = PlayerPrefs.GetInt ("invenMItemLevel" + i);
		}

		gridCastleLevel = PlayerPrefs.GetInt ("GridCastleHealth");

		saveLoader.GetComponent<SaveLoad> ().Save ();

		SetText ();
	}

	public void HeroTest () {
		inventoryItemHero [3, 0] = 1;
		inventoryItemHero [3, 1] = 0;
	}

	public void StageUnClear () {
		for (int i = 1; i < 9; i++) {
			for (int j = 0; j < 15; j++) {
				PlayerPrefs.SetInt ("world" + i + "Clear" + j, 0);
				PlayerPrefs.SetInt ("World" + i + "Stage" + j + "Jewels", 0);
				PlayerPrefs.SetInt ("World" + i + "Stage" + j + "Reward", 0);
			}
		}
		PlayerPrefs.SetInt ("world1Clear0", 1);
		PlayerPrefs.SetInt ("Ended", 0);
	}

	public void StageClear () {
		for (int i = 1; i < 9; i++) {
			for (int j = 0; j < 15; j++) {
				PlayerPrefs.SetInt ("world" + i + "Clear" + j, 2);
				PlayerPrefs.SetInt ("World" + i + "Stage" + j + "Jewels", 0);
				PlayerPrefs.SetInt ("World" + i + "Stage" + j + "Reward", 0);
			}
		}
		PlayerPrefs.SetInt ("world1Clear0", 1);
		PlayerPrefs.SetInt ("Ended", 1);
	}

	public void HeroReset () {
		inventoryItemHero [1, 0] = 0;
		inventoryItemHero [1, 1] = 0;
		inventoryItemHero [2, 0] = 0;
		inventoryItemHero [2, 1] = 0;
		inventoryItemHero [3, 0] = 0;
		inventoryItemHero [3, 1] = 0;
		PlayerPrefs.SetInt ("invenHeroUnlocked1", 0);
		PlayerPrefs.SetInt ("invenHeroLevel1", 0);
		PlayerPrefs.SetInt ("invenHeroUnlocked2", 0);
		PlayerPrefs.SetInt ("invenHeroLevel2", 0);
		PlayerPrefs.SetInt ("invenHeroUnlocked3", 0);
		PlayerPrefs.SetInt ("invenHeroLevel3", 0);

	}

	public void MonsDictReset () {
		PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 0);
		PlayerPrefs.SetInt ("MonsterDictionaryDeminion", 0);
		PlayerPrefs.SetInt ("MonsterDictionaryEarthElementaling", 0);
		PlayerPrefs.SetInt ("MonsterDictionarySlime", 0);
		PlayerPrefs.SetInt ("MonsterDictionaryNependeath", 0);
		PlayerPrefs.SetInt ("MonsterDictionaryEarthElemental", 0);
		PlayerPrefs.SetInt ("MonsterDictionarySandeminion", 0);
		PlayerPrefs.SetInt ("MonsterDictionaryCrazySand", 0);
		PlayerPrefs.SetInt ("MonsterDictionaryTumbleWeed", 0);
	}

	public void GetGold () {
		GemGoldScript.gemGoldScript.PlusGold (10000);
		SetText ();
	}

	public void Ended () {
		PlayerPrefs.SetInt ("Ended", 0);
	}
}
