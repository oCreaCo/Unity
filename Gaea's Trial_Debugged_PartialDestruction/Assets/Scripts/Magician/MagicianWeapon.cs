using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicianWeapon : MonoBehaviour {
	public static MagicianWeapon magicianWeapon;

	public Transform mWeaponSpawned;

	public Transform spawnPoint;

	public int equipped;

	[System.Serializable]
	public struct WeaponMagician
	{
		public Transform weaponPrefab;
	};

	public WeaponMagician[] mWeapon;

	void Awake()
	{
		spawnPoint = transform.FindChild("MWeaponSpawnPoint");
		magicianWeapon = this;

//		InstantiateMWeapon(MainGM.maingm.inventoryItemHWeapon [MainGM.maingm.GridEquippedHWeapon, 0] - 1);
//		InstantiateMWeapon(PlayerPrefs.GetInt ("invenMItemNum" + PlayerPrefs.GetInt ("GridEquippedMWeapon")) - 1);
	}

	public void InstantiateMWeapon(int a)
	{
		if (mWeaponSpawned == null)
		{
			mWeaponSpawned = Instantiate(mWeapon[a].weaponPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, this.transform) as Transform;
			mWeaponSpawned.GetComponent<MWeaponSkill>().firePoint = transform.FindChild("MWeaponFirePoint");
		}
	}
}
