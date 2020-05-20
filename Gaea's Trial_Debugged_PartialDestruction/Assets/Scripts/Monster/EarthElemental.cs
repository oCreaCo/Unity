using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EarthElemental : MonoBehaviour {

	public PartialDestruction partialDestruction;

	public int rangeDamage;
	public float rangeFireRate;
	public float rangeFireRateTemp;
	public float offSet;
	private float attackTime;
	public float destroytime;
	public bool buttonActivated;

	private Animator anim;
	private Monster_Basic monsterBasic;
	public Barricade barricade;
	public float cameraShakeMultiplier;
	public float cameraShakeLength = 0.1f;

	[SerializeField] private GameObject body;
	[SerializeField] private Sprite brokenBody;
	[SerializeField] private Transform rangeAttackPoint;
	private bool rangeAttackable = true;

	void Awake() {
		rangeFireRateTemp = rangeFireRate;
		attackTime = Time.time + rangeFireRateTemp;
		anim = GetComponent<Animator> ();
		monsterBasic = GetComponent<Monster_Basic> ();
		barricade = Castle.castle.transform.GetComponent<Barricade> ();
		partialDestruction = GetComponent<PartialDestruction> ();
	}

	void Update () {
		if (!monsterBasic.collided) {
			if (rangeAttackable && Time.time >= attackTime && !monsterBasic.stoppedBool) {
				attackTime = Time.time + rangeFireRateTemp;
				anim.SetTrigger ("RangeAttack");
			}
		}

	}

	public void HurtFunction () {
		if (monsterBasic.curHealth / monsterBasic.oriHealth <= 0.3f && !buttonActivated) {
			partialDestruction.ActivateButton (ref buttonActivated, 3f);
		}
	}

	public void RangeAttack()
	{
		barricade.GetHurt (rangeDamage, cameraShakeMultiplier, cameraShakeLength);
	}

	public void PartialDestructionSuccessFunction() {
		body.GetComponent<SpriteRenderer> ().sprite = brokenBody;
		rangeAttackable = false;
	}
}
