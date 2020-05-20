using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : MonoBehaviour {
	public static Hero hero;
	
	public Transform selectedHero;
	public Transform copyHero;

	public Transform HeroSpawnPoint;

	public int heroNum;

	public enum HeroType
	{
		HERO,
		NINJA,
		PIRATE,
		BABARIAN,
	};

	[System.Serializable]
	public struct Heroprefab
	{
		public HeroType type;
		public Transform prefab;
	};

	public Heroprefab[] heroprefab;


	private HeroType type;

	public HeroType Type
	{
		get { return type;}
	}

	private Dictionary<HeroType, Transform> heroPrefabDict;

	// Use this for initialization
	void Awake () {
		heroPrefabDict = new Dictionary<HeroType, Transform> ();
		hero = this;

		for (int i = 0; i < heroprefab.Length; i++) {
			if (!heroPrefabDict.ContainsKey (heroprefab[i].type)){
				heroPrefabDict.Add (heroprefab [i].type, heroprefab [i].prefab);
			}
		}

//		InstantiateHero (PlayerPrefs.GetInt ("GridEquippedHero"));
	}

	public void InstantiateHero(int a)
	{
		if (selectedHero == null)
		{
			selectedHero = Instantiate(heroPrefabDict[heroprefab[a].type], HeroSpawnPoint.transform.position, HeroSpawnPoint.transform.rotation, this.transform) as Transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
