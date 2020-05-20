using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyHeroScript : MonoBehaviour, IEventSystemHandler {

	public GemGoldScript gemGoldScript;
	public GameObject boughtText;
	private bool bought = false;
	public int heroNum;
	public int priceGem;
	public Button.ButtonClickedEvent buyingEvent;
	public Button buyHeroButton;

	public void Awake () {
		if (PlayerPrefs.GetInt ("invenHeroUnlocked" + heroNum) == 1) {
			bought = true;
			boughtText.SetActive (true);
			this.GetComponent<Button> ().interactable = false;
		}
	}

	public void BuyBox () {
		ShopManager.shopManager.buyBox.SetActive (true);
		buyHeroButton.onClick = buyingEvent;
	}

	public void BuyHero () {
		if (!bought && gemGoldScript.gem >= priceGem) {
			bought = true;
			boughtText.SetActive (true);
			gemGoldScript.MinusGem (priceGem);
			ShopManager.shopManager.SetText ();
			PlayerPrefs.SetInt ("invenHeroUnlocked" + heroNum, 1);
			PlayerPrefs.SetInt ("invenHeroLevel" + heroNum, 0);
			this.GetComponent<Button> ().interactable = false;
			ShopManager.shopManager.buyBox.SetActive (false);
		} else if (gemGoldScript.gem< priceGem) {
			Debug.LogError ("Nope");
			ShopManager.shopManager.alertBox.SetActive (true);
		}
	}
}
