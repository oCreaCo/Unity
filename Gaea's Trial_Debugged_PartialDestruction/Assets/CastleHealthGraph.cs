using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealthGraph : MonoBehaviour {

	[SerializeField] private Image healthGraph;

	public void SetHealth (float _cur, float _max) {
			healthGraph.fillAmount = (float)_cur / _max;
	}
}
