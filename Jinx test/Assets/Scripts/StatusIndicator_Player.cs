using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator_Player : MonoBehaviour {

	[SerializeField]
	private RectTransform healthBar;

	void Start()
	{
		if (healthBar == null) {
			Debug.LogError("StatusIndicator: no 'healthBar' object!");
		}
	}

	public void SetHealth(int _cur, int _max)
	{
		float _value = (float)_cur / _max;

		healthBar.localScale = new Vector3 (_value, healthBar.localScale.y, healthBar.localScale.z);
	}
}
