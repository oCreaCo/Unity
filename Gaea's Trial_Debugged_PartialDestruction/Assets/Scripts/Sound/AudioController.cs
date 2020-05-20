using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	[SerializeField]
	Sound[] sounds;

	void Awake ()
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
			_go.transform.SetParent(this.transform);
			sounds[i].SetSource (_go.AddComponent<AudioSource>());
		}
	}

	public void PlaySound (string _name)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			if (sounds[i].name == _name)
			{
				sounds[i].Play();
				return;
			}
		}
		Debug.LogWarning("AudioController: Sound not found in list, " + _name);
	}

}
