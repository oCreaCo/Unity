using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPButton : MonoBehaviour {

	public void Buy100Gems ()
	{
		IAPManager.Instance.Buy100Gems ();
	}

	public void Buy200Gems ()
	{
		IAPManager.Instance.Buy200Gems ();
	}

	public void Buy400Gems ()
	{
		IAPManager.Instance.Buy400Gems ();
	}

	public void Buy1000Gems ()
	{
		IAPManager.Instance.Buy1000Gems ();
	}

	public void Buy2000Gems ()
	{
		IAPManager.Instance.Buy2000Gems ();
	}
}
