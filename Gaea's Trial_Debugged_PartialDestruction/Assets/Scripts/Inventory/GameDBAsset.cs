using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class GameDBAsset
{
	[MenuItem("Assets/Create/LanguageData")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<LanguageData> ();
	}
}

public class ItemDBAsset
{
	[MenuItem("Assets/Create/ItemData")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<ItemData> ();
	}
}

#endif