using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusTriggerAllMonsters : MonoBehaviour {

	public enum TriggerType
	{
		FIRE,
		ICED,
		THUNDER,
		BLOOD,
	}

	public TriggerType triggerType;

	public int dotCount;
	public float triggerTime;
	public int damage;

	public void TriggerAllMonsters () {
		switch (triggerType) {
		case TriggerType.FIRE:
			for (int i = 0; i < GameMaster.gameMaster.remainingEnemies.Count; i++) {
				GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_StatusController> ().Fired (damage, dotCount);
			}
			break;
		case TriggerType.ICED:
			for (int i = 0; i < GameMaster.gameMaster.remainingEnemies.Count; i++) {
				GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_StatusController> ().Iced (triggerTime);
			}
			break;
		case TriggerType.THUNDER:
			for (int i = 0; i < GameMaster.gameMaster.remainingEnemies.Count; i++) {
				GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_StatusController> ().ElectricShocked (triggerTime);
			}
			break;
		case TriggerType.BLOOD:
			for (int i = 0; i < GameMaster.gameMaster.remainingEnemies.Count; i++) {
				GameMaster.gameMaster.remainingEnemies [i].GetComponent<Monster_StatusController> ().Blood (damage, dotCount);
			}
			break;
		}
	}
}
