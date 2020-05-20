using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Function : MonoBehaviour, IEventSystemHandler {

	public Button.ButtonClickedEvent[] functions;

	public void InvokeFunction () {
		for (int i = 0; i < functions.Length; i++) {
			functions [i].Invoke ();
		}
	}
}
