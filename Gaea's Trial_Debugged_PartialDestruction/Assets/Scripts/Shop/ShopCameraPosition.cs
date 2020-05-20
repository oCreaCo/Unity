using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCameraPosition : MonoBehaviour {

	public Transform shopMain;
	public Transform heroCameraPosition;
	public Transform weaponsCameraPosition;
	public Transform castleCameraPosition;
	public Transform gemStoreCameraPosition;
	public GameObject goToShopMainButton;

	public void GoToHero () {
		this.transform.position = heroCameraPosition.position;
		goToShopMainButton.SetActive (true);
	}

	public void GoToWeapons () {
		this.transform.position = weaponsCameraPosition.position;
		goToShopMainButton.SetActive (true);
	}

	public void GoToCastle () {
		this.transform.position = castleCameraPosition.position;
		goToShopMainButton.SetActive (true);
	}

	public void GoToGemStore () {
		this.transform.position = gemStoreCameraPosition.position;
		goToShopMainButton.SetActive (true);
	}

	public void GoToShopMain () {
		this.transform.position = shopMain.position;
		goToShopMainButton.SetActive (false);
	}
}
