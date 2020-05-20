using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject {

	public enum spAttack
	{
		NONE,
		FLAME,
		ICE,
		LIGHTNING,
		WOUNDING,
		PIERCE,
	}

	public int[] heroLevelMinDamage = new int[10];
	public int[] heroLevelMaxDamage = new int[10];

	public int[] heroLevelUpgradeCost = new int[9];

	[System.Serializable]
	public struct HeroItemUpgradeCost
	{
		public int[] cost;
	}

	[System.Serializable]
	public struct MagicianItemUpgradeCost
	{
		public int[] cost;
	}

	[System.Serializable]
	public struct HeroList
	{
		public Sprite heroSprite;
		public RuntimeAnimatorController animController;
	}

	[System.Serializable]
	public struct HeroItemList
	{
		public Sprite heroItemSprite;
		public spAttack Type;
		public int chance;
		public int heroItemRed;
		public int heroItemBlue;
		public int heroItemPurple;
		public int heroItemGreen;
		public int heroItemYellow;
		public int heroItemRating;
		public int[] heroItemDamageMin;
		public int[] heroItemDamageMax;
		public int heroItemPenetration;
		public int heroItemLegendSkillTime;
		public int heroItemLegendCoolTime;
		public int heroItemLegendSkillCount;
	}

	[System.Serializable]
	public struct MagicianItemList
	{
		public Sprite magicianItemSprite;
		public int[] magicianItemDamage;
		public int magicianItemRating;
	}

	public int[] heroItemListSellCost;
	public int[] magicianItemListSellCost;

	public HeroItemUpgradeCost[] heroItemUpgradecost = new HeroItemUpgradeCost[4];
	public MagicianItemUpgradeCost[] magicianItemUpgradeCost = new MagicianItemUpgradeCost[4];

	public HeroList[] heroList;
	public HeroItemList[] heroItemList;
	public MagicianItemList[] magicianItemList;
	public Sprite[] heroSprite;

	public Sprite[] ratingSprite = new Sprite[5];
}
