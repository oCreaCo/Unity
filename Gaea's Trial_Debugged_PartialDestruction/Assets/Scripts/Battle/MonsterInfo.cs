using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterInfo : MonoBehaviour {

	public LanguageData[] gameDBLanguageData;
	public LanguageData thisGameDBData;

	public int curMonsNum;

	public GameObject monsterPosition;
	public GameObject monster;
	public Text monsterName;
	public Text monsterInfos;
	public GameObject[] monsters;

	// Use this for initialization
	void Awake () {
		thisGameDBData = gameDBLanguageData [PlayerPrefs.GetInt ("Language")];
		curMonsNum = PlayerPrefs.GetInt ("MonsterDictionaryMaxMonsNum");
		GameObject curMonster = Instantiate (monsters [curMonsNum], monsterPosition.transform.position, monsterPosition.transform.rotation) as GameObject;
		curMonster.transform.localScale = new Vector3 (1, 1, 1);
		curMonster.transform.parent = monsterPosition.transform;
		monsterName.text = thisGameDBData.monsterDictionary [curMonsNum].monsterName;
		monsterInfos.text = thisGameDBData.monsterDictionary [curMonsNum].monsterInfo;
	}
		
	public void Back () {
		GameMaster.gameMaster.monsterInfo.gameObject.SetActive (false);
	}
}
