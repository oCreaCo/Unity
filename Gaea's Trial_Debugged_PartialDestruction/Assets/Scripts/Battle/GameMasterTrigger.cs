using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameMasterTrigger : MonoBehaviour, IEventSystemHandler {

	private GameMaster gameMaster;

	[SerializeField] private Button.ButtonClickedEvent redTrigger;
	[SerializeField] private Button.ButtonClickedEvent blueTrigger;
	[SerializeField] private Button.ButtonClickedEvent purpleTrigger;
	[SerializeField] private Button.ButtonClickedEvent greenTrigger;
	[SerializeField] private Button.ButtonClickedEvent yellowTrigger;
	
	void Awake () {
		gameMaster = GameMaster.gameMaster;
		gameMaster.redTrigger.Add (redTrigger);
		gameMaster.blueTrigger.Add (blueTrigger);
		gameMaster.purpleTrigger.Add (purpleTrigger);
		gameMaster.greenTrigger.Add (greenTrigger);
		gameMaster.yellowTrigger.Add (yellowTrigger);
	}
	
	public void Remove () {
		gameMaster.redTrigger.Remove (redTrigger);
		gameMaster.blueTrigger.Remove (blueTrigger);
		gameMaster.purpleTrigger.Remove (purpleTrigger);
		gameMaster.greenTrigger.Remove (greenTrigger);
		gameMaster.yellowTrigger.Remove (yellowTrigger);
	}
}
