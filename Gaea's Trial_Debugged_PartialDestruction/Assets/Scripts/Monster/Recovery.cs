using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recovery : MonoBehaviour {

	private Monster_Basic monsterBasic;
	private Animator anim;
	private bool moveSpeedSwitch = false;
	public bool healed = false;

	public void RecoveryHealth (Animator _anim, Monster_Basic _monsterBasic) {
		healed = true;
		monsterBasic = _monsterBasic;
		anim = _anim;
		anim.SetBool ("Recovery", true);
		StartCoroutine (RecoveryCoroutine (3f));
	}

	IEnumerator RecoveryCoroutine (float _time) {
		for (float i = 0; i < _time; i += Time.deltaTime) {
			if (monsterBasic.dead) {
				break;
			}
			monsterBasic.curHealth += (monsterBasic.oriHealth * 0.5f) * Time.deltaTime / _time;
			monsterBasic.statusIndicator.SetHealth (monsterBasic.curHealth, monsterBasic.oriHealth);
			yield return 0;
		}
		monsterBasic.statusIndicator.SetHealth (monsterBasic.curHealth, monsterBasic.oriHealth);
		anim.SetBool ("Recovery", false);
	}

	public void MoveSpeedSet () {
		if (!moveSpeedSwitch) {
			monsterBasic.stoppedBool = true;
			monsterBasic.moveSpeedTemp = 0;
			GetComponent<Monster_Basic> ().penetration += 1;
			moveSpeedSwitch = true;
		} else if (moveSpeedSwitch) {
			monsterBasic.stoppedBool = false;
			monsterBasic.moveSpeedTemp = monsterBasic.moveSpeed;
			GetComponent<Monster_Basic> ().penetration -= 1;
			moveSpeedSwitch = false;
		}
	}
}
