using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public static GameMaster gameMaster;

	public Animator camAnim;
	public Animator menuAnim;
	public GameObject pauseButton;
	[SerializeField] private GameObject throwPoint;

	[System.Serializable]
	public struct ParticleStruct
	{
		public ParticleSystem[] particle;
	}

	public ParticleStruct[] particleStructs;

	void Awake () {
		GameMaster.gameMaster = this;
	}

	public void Play () {
		camAnim.SetBool ("Game", true);
		menuAnim.SetBool ("Game", true);
		pauseButton.SetActive (true);
		AudioManager.audioManager.transform.Find ("Sound_0_GameStart").GetComponent<AudioSource> ().Play ();
	}

	public void Quit () {
		AudioManager.audioManager.transform.Find ("Sound_6_Click").GetComponent<AudioSource> ().Play ();
		Application.Quit ();
	}

	public void SetThrowPointTrue () {
		throwPoint.SetActive (true);
		throwPoint.GetComponent<ThrowPoint> ().ThrowPointStart ();
		throwPoint.GetComponent<BoxCollider2D>().enabled = true;
		ThrowPoint.throwPoint.paused = false;
	}

	public void SetThrowPointFalse () {
		throwPoint.SetActive (false);
	}

	public void HighScoreParticles () {
		StartCoroutine (HighScoreParticlesCoroutine ());
	}

	IEnumerator HighScoreParticlesCoroutine () {
		yield return new WaitForSeconds (1.7f);
		for (int i = 0; i < particleStructs.Length; i++) {
			for (int j = 0; j < particleStructs [i].particle.Length; j++) {
				particleStructs [i].particle [j].Play ();
			}
			yield return new WaitForSeconds (0.2f);
		}
	}
}
