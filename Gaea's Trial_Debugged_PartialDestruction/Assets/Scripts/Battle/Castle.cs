using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Castle : MonoBehaviour {
	public static Castle castle;

	public GameObject healUI;
	public Transform healTextSpawnPoint;

	public StatusIndicator statusIndicator;
	public CastleHealthGraph castleHealthGraph;
	public GameObject gameOverUI;

	public float shakeAmount;
	public float shakeLength;
//	private bool attacked = false;
	public Sprite[] sprite = new Sprite[4];

	private Animator anim;

	void Awake () {
		castle = this;
	}

	public void GetHealth (int heal) {
//		AudioManager.instance.PlaySound ("Heal");
		GetComponent<Barricade>().curHealth += heal;

		statusIndicator.SetHealth (GetComponent<Barricade>().curHealth, GetComponent<Barricade>().oriHealth);
		castleHealthGraph.SetHealth (GetComponent<Barricade>().curHealth, GetComponent<Barricade>().oriHealth);
		if (heal != 0) {
			GameObject healUIClone = Instantiate (healUI, healTextSpawnPoint.position, healTextSpawnPoint.rotation) as GameObject;
			healUIClone.transform.FindChild ("HealText").GetComponent<Text> ().text = "+" + heal.ToString ();
			Destroy (healUIClone.gameObject, 1f);}

		if (GetComponent<Barricade>().curHealth >= GetComponent<Barricade>().oriHealth) {
			GetComponent<Barricade>().curHealth = GetComponent<Barricade>().oriHealth;
		}

		if (GetComponent<Barricade>().curHealth > 75 && GetComponent<Barricade>().curHealth <= 100) {
			this.transform.FindChild ("Castle").GetComponent<Image> ().sprite = sprite [0];
		} else if (GetComponent<Barricade>().curHealth > 50 && GetComponent<Barricade>().curHealth <= 75) {
			this.transform.FindChild ("Castle").GetComponent<Image> ().sprite = sprite [1];
		} else if (GetComponent<Barricade>().curHealth > 25 && GetComponent<Barricade>().curHealth <= 50) {
			this.transform.FindChild ("Castle").GetComponent<Image> ().sprite = sprite [2];
		} else if (GetComponent<Barricade>().curHealth > 0 && GetComponent<Barricade>().curHealth <= 25) {
			this.transform.FindChild ("Castle").GetComponent<Image> ().sprite = sprite [3];
		}
	}
}
