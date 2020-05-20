using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GemGoldScript : MonoBehaviour {
	public static GemGoldScript gemGoldScript;

	public int gem;
	public int gold;

	public void GemGoldAwake () {
		gemGoldScript = this;
		gem = PlayerPrefs.GetInt ("Gem");
		gold = PlayerPrefs.GetInt ("Gold");
	}

	public void PlusGem (int value) {
		gem += value;
		PlayerPrefs.SetInt ("Gem", gem);
		gem = PlayerPrefs.GetInt ("Gem");
	}

	public bool MinusGem (int value) {
		if (gem >= value) {
			gem -= value;
			PlayerPrefs.SetInt ("Gem", gem);
			gem = PlayerPrefs.GetInt ("Gem");
			return true;
		} else {
			Debug.LogError ("NoGem... Hack NoGem...");
			return false;
		}
	}

	public void PlusGold (int value) {
		gold += value;
		PlayerPrefs.SetInt ("Gold", gold);
		gold = PlayerPrefs.GetInt ("Gold");
	}

	public bool MinusGold (int value) {
		if (gold >= value) {
			gold -= value;
			PlayerPrefs.SetInt ("Gold", gold);
			gold = PlayerPrefs.GetInt ("Gold");
			return true;
		} else {
			Debug.LogError ("NoGold... Hack NoGold...");
			return false;
		}
	}
}
