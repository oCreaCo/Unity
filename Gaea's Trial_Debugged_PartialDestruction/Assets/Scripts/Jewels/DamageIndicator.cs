using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour {

	public Text DamageText;

	// Use this for initialization
	void Start () {
		if (DamageText == null) {
			Debug.LogError("There is no 'DamageText' object!");
		}
	}

	public void SetDamage (int _Dmg)
	{
		DamageText.text = _Dmg.ToString ();
	}
}
