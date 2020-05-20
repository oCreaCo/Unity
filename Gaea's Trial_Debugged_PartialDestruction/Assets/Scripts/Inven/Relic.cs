using UnityEngine;
using System.Collections;

public class Relic : MonoBehaviour {
	public static Relic relic;

	public Transform relicSpawned;

	public Transform spawnPoint;

	public int equipped;

	[System.Serializable]
	public struct RelicPrefab
	{
		public Transform relicSprite;
	};

	public RelicPrefab[] relicPrefab;

	void Awake()
	{
		spawnPoint = transform.FindChild("RelicSpawnPoint");
		relic = this;

		//		InstantiateMWeapon(MainGM.maingm.inventoryItemHWeapon [MainGM.maingm.GridEquippedHWeapon, 0] - 1);
		//		InstantiateMWeapon(PlayerPrefs.GetInt ("invenMItemNum" + PlayerPrefs.GetInt ("GridEquippedMWeapon")) - 1);
	}

	public void InstantiateRelic(int a)
	{
		if (relicSpawned == null)
		{
			relicSpawned = Instantiate(relicPrefab[a].relicSprite, spawnPoint.transform.position, spawnPoint.transform.rotation) as Transform;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
