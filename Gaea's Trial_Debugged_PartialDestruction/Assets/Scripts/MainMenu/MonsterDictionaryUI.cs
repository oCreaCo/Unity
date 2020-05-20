using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterDictionaryUI : MonoBehaviour {

	public GameObject monsterDictionary;

	public Button next;
	public Button previous;

	[SerializeField] private int curMonsNum;
	[SerializeField] private int minMonsNum, maxMonsNum;

	public GameObject monsterPosition;
	public GameObject monster;
	public GameObject[] monsters;

	public void MonsterDictionary () {
		monsterDictionary.SetActive (true);
	}

	public void Back () {
		curMonsNum = 1;
		monsterDictionary.SetActive (false);
	}

	void Awake () {
//		PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 17);
		minMonsNum = 1;
		curMonsNum = 1;
		maxMonsNum = PlayerPrefs.GetInt ("MonsterDictionaryMaxMonsNum");
//		monsterPosition = monsters [curMonsNum].transform;
		GameObject curMonster = Instantiate (monsters [curMonsNum], monsterPosition.transform.position, monsterPosition.transform.rotation) as GameObject;
		curMonster.transform.parent = monsterPosition.transform;
		monster = curMonster;
		if (curMonsNum == maxMonsNum) {
			next.gameObject.SetActive (false);
		}
	}

	public void Previous () {
		curMonsNum--;
		if (curMonsNum == minMonsNum) {
			previous.gameObject.SetActive (false);
		}
		if (curMonsNum != maxMonsNum) {
			next.gameObject.SetActive (true);
		}
		Destroy (monster.gameObject);
		GameObject curMonster = Instantiate (monsters [curMonsNum], monsterPosition.transform.position, monsterPosition.transform.rotation) as GameObject;
		curMonster.transform.parent = monsterPosition.transform;
		monster = curMonster;
	}
	
	public void Next () {
		curMonsNum++;
		if (curMonsNum != minMonsNum) {
			previous.gameObject.SetActive (true);
		}
		if (curMonsNum == maxMonsNum) {
			next.gameObject.SetActive (false);
		}
		Destroy (monster.gameObject);
		GameObject curMonster = Instantiate (monsters [curMonsNum], monsterPosition.transform.position, monsterPosition.transform.rotation) as GameObject;
		curMonster.transform.parent = monsterPosition.transform;
		monster = curMonster;
	}
}
