using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class AdManager : MonoBehaviour {

	public int num;

	void Awake () {
		Advertisement.Initialize ("1321182", false);
//		Advertisement.Initialize ("1321183", false);
	}

	public void ShowAd()
	{
		#if UNITY_ADS
		if (!Advertisement.IsReady ()) {
			GemGoldScript.gemGoldScript.PlusGold (50);
			ShopManager.shopManager.SetText ();
			Debug.LogError ("AdFailed");
			return;
		} else {
			switch (num) {
			case 0:
				Advertisement.Show ("rewardedVideo", new ShowOptions (){ resultCallback = TwiceGems });
				break;
			case 1:
				Advertisement.Show ("rewardedVideo", new ShowOptions (){ resultCallback = FiveGems });
				break;
			}
		}
		#endif
	}

	private void TwiceGems (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			Debug.Log ("보사아Ang");
			break;
		case ShowResult.Skipped:
			Debug.Log ("광고 넘김. 손모가지 날아갈래?");
			break;
		case ShowResult.Failed:
			Debug.Log ("Internet problem??");
			break;
		}
	}

	private void FiveGems (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			GemGoldScript.gemGoldScript.PlusGem (5);
			ShopManager.shopManager.SetText ();
			break;
		case ShowResult.Skipped:
			Debug.Log ("광고 넘김. 손모가지 날아갈래?");
			break;
		case ShowResult.Failed:
			Debug.Log ("Internet problem??");
			break;
		}
	}
}
