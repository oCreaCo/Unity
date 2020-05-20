using UnityEngine;
using System.Collections;

public class Buff : MonoBehaviour {
	public static Buff buff;

	public bool buffOn;
	public float destroyTime;
	public int redAddingValue;
	public int blueAddingValue;
	public int greenAddingValue;
	public int purpleAddingValue;
	public int yellowAddingValue;
	public int healthDebuff;
	public float curTime;

	void Awake () {
		buff = this;
		curTime = Time.time;
		GameMaster.gameMaster.redSkillValue += redAddingValue;
		GameMaster.gameMaster.blueSkillValue += blueAddingValue;
		GameMaster.gameMaster.greenSkillValue += greenAddingValue;
		GameMaster.gameMaster.purpleSkillValue += purpleAddingValue;
		GameMaster.gameMaster.yellowSkillValue += yellowAddingValue;
		Castle.castle.transform.GetComponent<Barricade> ().addDamage += healthDebuff;
		destroyTime = Time.time + this.transform.GetComponent<AwakeDestroy> ().destroyTime;
		buffOn = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= destroyTime - 0.5f) {
			if (buffOn) {
				GameMaster.gameMaster.redSkillValue -= redAddingValue;
				GameMaster.gameMaster.blueSkillValue -= blueAddingValue;
				GameMaster.gameMaster.greenSkillValue -= greenAddingValue;
				GameMaster.gameMaster.purpleSkillValue -= purpleAddingValue;
				GameMaster.gameMaster.yellowSkillValue -= yellowAddingValue;
				Castle.castle.GetComponent<Barricade> ().addDamage -= healthDebuff;
				buffOn = false;
			}
		}
	}
}
