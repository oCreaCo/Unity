using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour {

	public Text bgmVolumeText;
	public Text effectVolumeText;
	public int bgmVolume;
	public int effectVolume;
	
	void Start () {
		bgmVolume = PlayerPrefs.GetInt ("BGMVolume");
		effectVolume = PlayerPrefs.GetInt ("EffectVolume");
		bgmVolumeText.text = bgmVolume.ToString ();
		effectVolumeText.text = effectVolume.ToString ();
	}

	public void BGMVolumePlus () {
		if (bgmVolume < 10) {
			bgmVolume++;
			PlayerPrefs.SetInt ("BGMVolume", bgmVolume);
			for (int i = 0; i < AudioManager.instance.bgms.Count; i++) {
				AudioManager.instance.bgms[i].source.volume = AudioManager.instance.bgms[i].volume * ((float)bgmVolume / 5f) * (1 + Random.Range (-AudioManager.instance.bgms[i].randomVolume / 2f, AudioManager.instance.bgms[i].randomVolume / 2f));
			}
			bgmVolumeText.text = bgmVolume.ToString ();
		}
	}

	public void BGMVolumeMinus () {
		if (bgmVolume > 0) {
			bgmVolume--;
			PlayerPrefs.SetInt ("BGMVolume", bgmVolume);
			for (int i = 0; i < AudioManager.instance.bgms.Count; i++) {
				AudioManager.instance.bgms[i].source.volume = AudioManager.instance.bgms[i].volume * ((float)bgmVolume / 5f) * (1 + Random.Range (-AudioManager.instance.bgms[i].randomVolume / 2f, AudioManager.instance.bgms[i].randomVolume / 2f));
			}
			bgmVolumeText.text = bgmVolume.ToString ();
		}
	}

	public void EffectVolumePlus () {
		if (effectVolume < 10) {
			effectVolume++;
			PlayerPrefs.SetInt ("EffectVolume", effectVolume);
			Debug.LogError ("EffectVolume: " + effectVolume);
//			for (int i = 0; i < AudioManager.instance.effects.Count; i++) {
//				AudioManager.instance.effects[i].source.volume = AudioManager.instance.effects[i].volume * ((float)effectVolume / 5f) * (1 + Random.Range (-AudioManager.instance.effects[i].randomVolume / 2f, AudioManager.instance.effects[i].randomVolume / 2f));
//			}
			effectVolumeText.text = effectVolume.ToString ();
		}
	}

	public void EffectVolumeMinus () {
		if (effectVolume > 0) {
			effectVolume--;
			PlayerPrefs.SetInt ("EffectVolume", effectVolume);
			Debug.LogError ("EffectVolume: " + effectVolume);
//			for (int i = 0; i < AudioManager.instance.effects.Count; i++) {
//				AudioManager.instance.effects[i].source.volume = AudioManager.instance.effects[i].volume * ((float)effectVolume / 5f) * (1 + Random.Range (-AudioManager.instance.effects[i].randomVolume / 2f, AudioManager.instance.effects[i].randomVolume / 2f));
//			}
			effectVolumeText.text = effectVolume.ToString ();
		}
	}

	public void Back () {
		this.gameObject.SetActive (false);
	}
}
