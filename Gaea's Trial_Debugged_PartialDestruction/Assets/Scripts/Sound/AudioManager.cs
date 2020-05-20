using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound {

	public string name;
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = 0.7f;
	[Range(0.5f, 1.5f)]
	public float pitch = 1f;

	[Range(0f, 0.5f)]
	public float randomVolume = 0.1f;
	[Range(0f, 0.5f)]
	public float randomPitch = 0.1f;

	public bool loop = false;

	public AudioSource source;

	public enum VolumeType {
		BGM,
		EFFECT,
	}
	public VolumeType volumeType;
//	public SceneType sceneType;

	public void SetSource (AudioSource _source)
	{
		source = _source;
		source.clip = clip;
		source.loop = loop;
		if (volumeType == VolumeType.BGM) {
			Debug.LogError ("BGM: " + _source);
			if (AudioManager.instance != null && !AudioManager.instance.bgms.Contains(this)) {
				AudioManager.instance.bgms.Add (this);
			}
		}
	}

	public void Play () {
		if (volumeType == VolumeType.BGM) {
			source.volume = volume * (1 + Random.Range (-randomVolume / 2f, randomVolume / 2f)) * ((float)(PlayerPrefs.GetInt("BGMVolume") / 5f));
		} else if (volumeType == VolumeType.EFFECT) {
			source.volume = volume * (1 + Random.Range (-randomVolume / 2f, randomVolume / 2f)) * ((float)(PlayerPrefs.GetInt("EffectVolume") / 5f));
		}
		source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
		source.Play();
	}

}

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;
	public bool playingBGMusic = false;
	public bool playingStageBGMusic = false;
	public bool playingEndingMusic = false;

	public float BGMusicVolume;
	public float effectVolume;

	[SerializeField]
	Sound[] sounds;

	public List<Sound> bgms = new List<Sound> ();
	public bool spawned;

	void Start ()
	{
		if (instance != null)
		{
			Debug.LogError("More than one AudioManager in the scene.");
			Destroy (this.gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (this.gameObject);
			Debug.LogError ("Spawned: " + spawned);
			bgms.Clear ();
			if (!spawned) {
				for (int i = 0; i < sounds.Length; i++) {
					GameObject _go = new GameObject ("Sound_" + i + "_" + sounds [i].name);
					_go.transform.SetParent (this.transform);
					sounds [i].SetSource (_go.AddComponent<AudioSource> ());
				}
				spawned = true;
			}
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
		Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
	}
}
