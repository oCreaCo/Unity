using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
	
	private GemGoldScript gemGoldScript;
	[SerializeField] private MainGM mainGM;
	public void Start () {
		//Debug.LogError ("menumanager Awake");
		gemGoldScript = GetComponent<GemGoldScript> ();
		if (PlayerPrefs.GetInt ("FirstTimePlaying") == 0) {
			SaveLoad.saveload.FirstPlay ();
			mainGM.OnMainAwake ();
			PlayerPrefs.SetInt ("FirstTimePlaying", 1);
			SaveLoad.saveload.Save ();
		} else if (PlayerPrefs.GetInt ("FirstTimePlaying") == 1) {
			gemGoldScript.GemGoldAwake ();
			mainGM.OnMainAwake ();
		}
//		SaveLoad.saveload.FirstPlay ();

		Hero.hero.InstantiateHero (mainGM.gridEquippedHero);
		HeroWeapon.heroWeapon.InstantiateHWeapon(mainGM.gridEquippedHWeapon);
		MagicianWeapon.magicianWeapon.InstantiateMWeapon(mainGM.gridEquippedMWeapon);
	}

	public void StartGame ()
	{
		AudioManager.instance.PlaySound ("Click");
		PlayerPrefs.SetInt ("Gold", gemGoldScript.gold);
		Debug.LogError (gemGoldScript.gold);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 4);
	}
		
	public void Inventory ()
	{
		AudioManager.instance.PlaySound ("Click");
		for (int i = 0; i < 20; i++) {
			PlayerPrefs.SetInt("invenHeroUnlocked" + i, mainGM.inventoryItemHero[i, 0]);
			PlayerPrefs.SetInt("invenHeroLevel" + i, mainGM.inventoryItemHero[i, 1]);

			PlayerPrefs.SetInt("invenHItemNum" + i, mainGM.inventoryItemHWeapon[i, 0]);
			PlayerPrefs.SetInt("invenHItemLevel" + i, mainGM.inventoryItemHWeapon[i, 1]);

			PlayerPrefs.SetInt("invenMItemNum" + i, mainGM.inventoryItemMWeapon[i, 0]);
			PlayerPrefs.SetInt("invenMItemLevel" + i, mainGM.inventoryItemMWeapon[i, 1]);
		}

		PlayerPrefs.SetInt ("Gem", gemGoldScript.gem);
		PlayerPrefs.SetInt ("Gold", gemGoldScript.gold);

		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 2);
	}

	public void Shop () {
		AudioManager.instance.PlaySound ("Click");
		for (int i = 0; i < 20; i++) {
			PlayerPrefs.SetInt("invenHeroUnlocked" + i, mainGM.inventoryItemHero[i, 0]);
			PlayerPrefs.SetInt("invenHeroLevel" + i, mainGM.inventoryItemHero[i, 1]);

			PlayerPrefs.SetInt("invenHItemNum" + i, mainGM.inventoryItemHWeapon[i, 0]);
			PlayerPrefs.SetInt("invenHItemLevel" + i, mainGM.inventoryItemHWeapon[i, 1]);

			PlayerPrefs.SetInt("invenMItemNum" + i, mainGM.inventoryItemMWeapon[i, 0]);
			PlayerPrefs.SetInt("invenMItemLevel" + i, mainGM.inventoryItemMWeapon[i, 1]);
		}

		PlayerPrefs.SetInt ("Gem", gemGoldScript.gem);
		PlayerPrefs.SetInt ("Gold", gemGoldScript.gold);

		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 5);
	}

	public void QuitGame ()
	{
		AudioManager.instance.PlaySound ("Click");
		Debug.Log ("Quitting");
		Application.Quit ();
	}

	public void TutsReset () {
		PlayerPrefs.SetInt ("FirstMainMenu", 0);
		PlayerPrefs.SetInt ("FirstStageSelect", 0);
		PlayerPrefs.SetInt ("FirstBattle", 0);
		PlayerPrefs.SetInt ("FirstInventory", 0);
		Debug.LogError("Reset");
	}
}
