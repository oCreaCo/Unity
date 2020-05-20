using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroWeapon : MonoBehaviour {
	public static  HeroWeapon heroWeapon;

    public Transform hWeaponSpawned;

    public Transform spawnPoint;

    public int equipped;

	[System.Serializable]
	public struct WeaponHero
	{
		public Transform weaponPrefab;
	};

	public WeaponHero[] hWeapon;

	void Awake()
	{
        spawnPoint = transform.FindChild("HWeaponSpawnPoint");
		heroWeapon = this;
	}

    public void InstantiateHWeapon(int a)
    {
        if (hWeaponSpawned == null)
        {
			hWeaponSpawned = Instantiate(hWeapon[a].weaponPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, this.transform) as Transform;
        }
    }
}
