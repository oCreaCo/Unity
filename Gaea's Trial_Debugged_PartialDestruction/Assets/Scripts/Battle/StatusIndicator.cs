using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusIndicator : MonoBehaviour {

	[SerializeField]
	private RectTransform healthBarRect;

	public void SetHealth (float _cur, float _max) {
		float _value = 1f;
		if ((float)_cur / _max <= 1) {
			_value = (float)_cur / _max;
		} else {
			_value = 1f;
		}

		healthBarRect.localScale = new Vector3 (_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
	}
}
