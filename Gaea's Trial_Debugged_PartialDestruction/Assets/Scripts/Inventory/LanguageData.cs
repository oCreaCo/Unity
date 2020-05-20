using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageData : ScriptableObject {

	public string language;

	[System.Serializable]
	public struct GameDBHeroList
	{
		public string heroName;
		public string heroInfo;
		public string heroHorSkillInfo;
		public string heroVerSkillInfo;
		public string heroUltSkillInfo;
	}

	[System.Serializable]
	public struct GameDBHeroItemList
	{
		public string heroItemName;
		public string heroItemInfo;
	}

	[System.Serializable]
	public struct GameDBMagicianItemList
	{
		public string magicianItemName;
		public string magicianItemInfo;
		public string magicianItemSkillInfo;
		public string magicianItemUltSkillInfo;
	}

	[System.Serializable]
	public struct GameDBItemInfoText
	{
		public string damage;
		public string penetration;
		public string level;
		public string notEnoughGold;
		public string upgrade;
		public string upgradeMax;
	}

	[System.Serializable]
	public struct InventoryTextManager
	{
		public string back;
		public string hWeapon;
		public string hero;
		public string mWeapon;
		public string shop;
	}

	[System.Serializable]
	public struct SettingUIText
	{
		public string language;
		public string back;
		public string start;
		public string inventory;
		public string quit;
		public string monsterDictionary;
	}

	[System.Serializable]
	public struct WorldSelect
	{
		public string[] worldSelect;
		public string[] worldName;
		public string back;
	}

	[System.Serializable]
	public struct TutorialsTexts
	{
		public TutorialsTextsBundle[] tutorialsTextsBundle;
	}

	[System.Serializable]
	public struct TutorialsTextsBundle
	{
		public string talkingText;
		public string systemText;
	}

	[System.Serializable]
	public struct MonsterDictionary
	{
		public string monsterName;
		public string monsterInfo;
	}

	public GameDBHeroList[] gameDBHeroList;
	public GameDBHeroItemList[] gameDBHeroItemList;
	public GameDBMagicianItemList[] gameDBMagicianItemList;
	public GameDBItemInfoText gameDBItemInfoText;
	public InventoryTextManager gameDBTextManager;
	public SettingUIText settingUIText;
	public WorldSelect worldSelect;
	public TutorialsTexts[] tutorialsTexts;
	public MonsterDictionary[] monsterDictionary;
	public string[] endingGaea;
}
