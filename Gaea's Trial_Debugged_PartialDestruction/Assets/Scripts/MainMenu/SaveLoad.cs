using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour {
	public static SaveLoad saveload;
	public GemGoldScript gemGoldScript;
	[SerializeField] private MainGM mainGM;

	// Use this for initialization
	void Awake () {
		if (saveload == null)
		{
			//saveload = GameObject.FindGameObjectWithTag("SaveLoad").GetComponent<SaveLoad>();
			saveload = this.GetComponent<SaveLoad>();
		}
		//Debug.LogError ("saveload Awake");
	}

	public void Save ()
	{
		BinaryFormatter binary = new BinaryFormatter ();
		FileStream fstream = File.Create (Application.persistentDataPath + "/saveFile.click");

		SaveManager saver = new SaveManager ();
		saver.gold = gemGoldScript.gold;
		saver.gem = gemGoldScript.gem;
		saver.gridEquippedHero = mainGM.gridEquippedHero;
		saver.gridEquippedHeroLevel = mainGM.gridEquippedHeroLevel;
		saver.invenEquippedHItemNum = mainGM.invenEquippedHItemNum;
		saver.invenEquippedMItemNum = mainGM.invenEquippedMItemNum;
		saver.gridEquippedHWeapon = mainGM.gridEquippedHWeapon;
		saver.gridEquippedMWeapon = mainGM.gridEquippedMWeapon;
		saver.gridCastleLevel = mainGM.gridCastleLevel;
		for (int i = 0; i < 20; i++) {
			saver.invenHeroUnlocked [i] = mainGM.inventoryItemHero [i, 0];
			saver.invenHeroLevel [i] = mainGM.inventoryItemHero [i, 1];

			saver.invenHItemNum [i] = mainGM.inventoryItemHWeapon [i, 0];
			saver.invenHItemLevel [i] = mainGM.inventoryItemHWeapon [i, 1];

			saver.invenMItemNum [i] = mainGM.inventoryItemMWeapon [i, 0];
			saver.invenMItemLevel [i] = mainGM.inventoryItemMWeapon [i, 1];
		}

		binary.Serialize (fstream, saver);
		Debug.LogError ("Saved.");
		fstream.Close ();
	}

	public void Load ()
	{
		if (File.Exists(Application.persistentDataPath + "/saveFile.click")) {
			BinaryFormatter binary = new BinaryFormatter ();
			FileStream fstream = File.Open (Application.persistentDataPath + "/saveFile.click", FileMode.Open);
			SaveManager saver = (SaveManager)binary.Deserialize (fstream);
			Debug.LogError ("Loaded.");
			fstream.Close ();

			gemGoldScript.gold = saver.gold;
			gemGoldScript.gem = saver.gem;
			mainGM.gridEquippedHero = saver.gridEquippedHero;
			mainGM.gridEquippedHeroLevel = saver.gridEquippedHeroLevel;
			mainGM.invenEquippedHItemNum = saver.invenEquippedHItemNum;
			mainGM.invenEquippedMItemNum = saver.invenEquippedMItemNum;
			mainGM.gridEquippedHWeapon = saver.gridEquippedHWeapon;
			mainGM.gridEquippedMWeapon = saver.gridEquippedMWeapon;
			mainGM.gridCastleLevel = saver.gridCastleLevel;
			for (int i = 0; i < 20; i++) {
				mainGM.inventoryItemHero [i, 0] = saver.invenHeroUnlocked [i];
				mainGM.inventoryItemHero [i, 1] = saver.invenHeroLevel [i];

				mainGM.inventoryItemHWeapon [i, 0] = saver.invenHItemNum [i];
				mainGM.inventoryItemHWeapon [i, 1] = saver.invenHItemLevel [i];

				mainGM.inventoryItemMWeapon [i, 0] = saver.invenMItemNum [i];
				mainGM.inventoryItemMWeapon [i, 1] = saver.invenMItemLevel [i];
			}
		}
	}

	public void FirstPlay () {
		Debug.Log ("First");
		PlayerPrefs.SetInt ("Ended", 0);
		for (int i = 0; i < 20; i++) {
			mainGM.inventoryItemHero [i, 0] = 0;
			mainGM.inventoryItemHero [i, 1] = 0;

			mainGM.inventoryItemHWeapon [i, 0] = 0;
			mainGM.inventoryItemHWeapon [i, 1] = 0;

			mainGM.inventoryItemMWeapon [i, 0] = 0;
			mainGM.inventoryItemMWeapon [i, 1] = 0;
		}

		for (int i = 1; i < 9; i++) {
			for (int j = 0; j < 15; j++) {
				PlayerPrefs.SetInt ("world" + i + "Clear" + j, 2);
				PlayerPrefs.SetInt ("World" + i + "Stage" + j + "Jewels", 0);
				PlayerPrefs.SetInt ("World" + i + "Stage" + j + "Reward", 0);
			}
		}
		PlayerPrefs.SetInt ("world1Clear0", 1);
//		PlayerPrefs.SetInt ("FirstEnding", 0);

		PlayerPrefs.SetInt ("FirstMainMenu", 0);
		PlayerPrefs.SetInt ("FirstStageSelect", 0);
		PlayerPrefs.SetInt ("FirstBattle", 0);

		mainGM.inventoryItemHWeapon [0, 0] = 1;
		mainGM.inventoryItemHWeapon [0, 1] = 0;
		mainGM.inventoryItemHWeapon [1, 0] = 2;
		mainGM.inventoryItemHWeapon [1, 1] = 0;
		mainGM.inventoryItemHWeapon [1, 1] = 0;
		mainGM.inventoryItemMWeapon [0, 0] = 5;
		mainGM.inventoryItemHero [0, 0] = 1;
		mainGM.inventoryItemHero [0, 1] = 0;

		mainGM.gridEquippedHero = 0;
		mainGM.gridEquippedHWeapon = 1;
		mainGM.gridEquippedMWeapon = 0;
		PlayerPrefs.SetInt ("GridEquippedHero", 0);
		PlayerPrefs.SetInt ("HeroLevelMinDamage", 1);
		PlayerPrefs.SetInt ("HeroLevelMaxDamage", 3);
		PlayerPrefs.SetInt ("invenEquippedHItemNum", 1);
		PlayerPrefs.SetInt ("invenEquippedMItemNum", 0);
		PlayerPrefs.SetInt ("GridEquippedHWeapon", 2);
		PlayerPrefs.SetInt ("GridEquippedMWeapon", 5);
		PlayerPrefs.SetInt ("GridEquippedHWeaponPenetration", 3);
		PlayerPrefs.SetInt ("GridEquippedHWeaponMinDamage", 2);
		PlayerPrefs.SetInt ("GridEquippedHWeaponMaxDamage", 2);
		PlayerPrefs.SetInt ("GridEquippedMWeaponDamage", 0);
		PlayerPrefs.SetInt ("GridCastleLevel", 0);
		PlayerPrefs.SetInt ("Gem", 0);
		PlayerPrefs.SetInt ("Gold", 0);

		for (int i = 0; i < 20; i++) {
			PlayerPrefs.SetInt("invenHeroUnlocked" + i, mainGM.inventoryItemHero[i, 0]);
			PlayerPrefs.SetInt("invenHeroLevel" + i, mainGM.inventoryItemHero[i, 1]);

			PlayerPrefs.SetInt("invenHItemNum" + i, mainGM.inventoryItemHWeapon[i, 0]);
			PlayerPrefs.SetInt("invenHItemLevel" + i, mainGM.inventoryItemHWeapon[i, 1]);

			PlayerPrefs.SetInt("invenMItemNum" + i, mainGM.inventoryItemMWeapon[i, 0]);
			PlayerPrefs.SetInt("invenMItemLevel" + i, mainGM.inventoryItemMWeapon[i, 1]);
		}

		PlayerPrefs.SetInt ("BGMVolume", 5);
		PlayerPrefs.SetInt ("EffectVolume", 5);
	}

	IEnumerator WaitToChange () {
		yield return new WaitForSeconds (0.1f);


	}
}

[Serializable]
class SaveManager {
	public int gold;
	public int gem;
	public int gridEquippedHero;
	public int gridEquippedHeroLevel;
	public int invenEquippedHItemNum;
	public int invenEquippedMItemNum;
	public int gridEquippedHWeapon;
	public int gridEquippedMWeapon;
	public int gridCastleLevel;

	public int[] invenHeroUnlocked = new int[20];
	public int[] invenHeroLevel = new int[20];

	public int[] invenRelicNum = new int[20];
	public int[] invenRelicRating = new int[20];

	public int[] invenHItemNum = new int[20];
	public int[] invenHItemLevel = new int[20];

	public int[] invenMItemNum = new int[20];
	public int[] invenMItemLevel = new int[20];
}
