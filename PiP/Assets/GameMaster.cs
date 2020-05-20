using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

	public static GameMaster gameMaster;
	public Grid grid;
	public GameObject deadObject;
	public GameObject pauseObject;

	public Transform player;
	public Transform camera;
	public Transform stage;
	public Transform blocks, tiles;
	public List<Stage> stages = new List<Stage> ();
	public List<Transform> temp = new List<Transform> ();
	[SerializeField] private Transform parentTemp;
	[SerializeField] private float scaleChangeTime;
	private Vector3 camPosTemp;
	private Vector3 biggerScale = new Vector3 (16, 16, 1);
	private Vector3 smallerScale = new Vector3 (0.0625f, 0.0625f, 1);
	private Vector3 vectorTemp;

	private Vector3 curVector;

	public int layerCount;

	void Awake () {
		GameMaster.gameMaster = this;
		temp.Clear ();
		layerCount = 0;
		curVector = Vector3.one;
	}

	public void PuzzleIn (Transform _inPuzzle, Transform _stage) {
		player.GetComponent<Player> ().AddSubOrderInLayer (100, true);
		stages [layerCount].rayOn = false;
		player.GetComponent<Player> ().BlockNull ();
		player.GetComponent<Player> ().movable = false;
		stage = _stage;
		blocks = _stage.FindChild ("Blocks");
		tiles = _stage.FindChild ("Tiles");
		temp.Add (_inPuzzle);
		temp [layerCount].GetComponent<Collider2D> ().enabled = false;
		player.SetParent (parentTemp);
		blocks.SetParent (parentTemp);
		tiles.SetParent (parentTemp);
		vectorTemp = new Vector3 (temp [layerCount].position.x, temp [layerCount].position.y + 0.5f, temp [layerCount].position.z);
		stage.position = vectorTemp;
		blocks.SetParent (stage);
		tiles.SetParent (stage);
		layerCount++;
		if (stages.Count == layerCount) {
			GetComponent<BlockSpawner> ().Spawn (_inPuzzle.GetComponent<Block> ().gridStateNum, layerCount);
			stages [layerCount].SetColliders (false);
		} else {
			stages [layerCount].gameObject.SetActive (true);
		}
		player.GetComponent<Player> ().blocks = stages [layerCount].transform.FindChild("Blocks");
		stages [layerCount].transform.position = new Vector3 (_inPuzzle.position.x - 0.095f, _inPuzzle.transform.position.y + 0.405f, 0);
		stages [layerCount].transform.localScale = smallerScale;
		stages [layerCount].transform.SetParent (_inPuzzle);
		StartCoroutine (PlayerLerpCoroutine (player.position, new Vector3 (stages [layerCount].startX - 1.5f + _inPuzzle.position.x, stages [layerCount].startY - 1 + _inPuzzle.position.y, 0), scaleChangeTime));
		StartCoroutine (ScaleLerpCoroutine (Vector3.one, biggerScale, scaleChangeTime, true));
	}

	public void PuzzleOut() {
		player.GetComponent<Player> ().AddSubOrderInLayer (100, false);
		player.GetComponent<Player> ().movable = false;
		temp [layerCount - 1].GetComponent<Collider2D> ().enabled = true;
		player.GetComponent<Player> ().BlockNull ();
		player.SetParent (parentTemp);
		blocks = stage.FindChild ("Blocks");
		tiles = stage.FindChild ("Tiles");
		stages [layerCount].SetColliders (false);
		stages [layerCount].transform.SetParent (stages [layerCount - 1].transform);
		Vector3 toPos;
		if (layerCount > 0) {
			toPos = new Vector3 (temp [layerCount - 1].GetComponent<Block> ().outX, temp [layerCount - 1].GetComponent<Block> ().outY, 0);
		} else {
			toPos = new Vector3 (temp [layerCount - 1].GetComponent<Block> ().outX - 0.5f, temp [layerCount - 1].GetComponent<Block> ().outY, 0);
		}
		StartCoroutine (PlayerLerpCoroutine (player.position, toPos, scaleChangeTime));
		StartCoroutine (ScaleLerpCoroutine (biggerScale, Vector3.one, scaleChangeTime, false));
		temp.Remove (temp [layerCount - 1]);
	}

	IEnumerator PlayerLerpCoroutine (Vector3 _playerPos, Vector3 _toPos, float _changingTime) {
		Debug.LogError (_toPos);
		for (float t = 0; t < 1 * _changingTime; t += Time.deltaTime) {
			player.position = Vector3.Lerp (_playerPos, _toPos, t / _changingTime);
			yield return 0;
		}
		player.position = _toPos;
//		player.GetComponent<SpriteRenderer>()
	}

	IEnumerator ScaleLerpCoroutine (Vector3 _startScale, Vector3 _endScale, float _changingTime, bool _bool) {
		for (float t = 0; t < 1 * _changingTime; t += Time.deltaTime) {
			stage.localScale = Vector3.Lerp (_startScale, _endScale, t / _changingTime);
			yield return 0;
		}
		stage.localScale = _endScale;
		if (_bool) {
			player.SetParent (stage);
			curVector *= 0.0625f;
			stages [layerCount].SetColliders (true);
			stages [layerCount].rayOn = true;
			if (layerCount > 0) {
				stages [layerCount].transform.SetParent (parentTemp);
			}
		} else if (!_bool) {
			curVector *= 16;
			player.SetParent (parentTemp);
			blocks.SetParent (parentTemp);
			tiles.SetParent (parentTemp);
			stage.position = Vector3.zero;
			player.SetParent (stage);
			blocks.SetParent (stage);
			tiles.SetParent (stage);
			stage = null;
			player.GetComponent<Player> ().blocks = stages [layerCount - 1].transform.FindChild("Blocks");
			player.GetComponent<Player> ().CheckBlock ();
			stages [layerCount].gameObject.SetActive (false);
			stages [layerCount - 1].rayOn = true;
			layerCount--;
			if (layerCount > 0) {
				stage = stages [layerCount - 1].transform;
			}
		}
		player.GetComponent<Player> ().movable = true;
	}

	public void Reset () {
		PlayerPrefs.SetInt("Index", 0);
		Time.timeScale = 1;
		SceneManager.LoadScene (0);
	}

	public void Retry () {
		Time.timeScale = 1;
		SceneManager.LoadScene (0);
	}

	public void Dead () {
		StartCoroutine (DeadCoroutine ());
	}

	IEnumerator DeadCoroutine () {
		yield return new WaitForSeconds (0.3f);
		Time.timeScale = 0;
		deadObject.SetActive (true);
	}

	public void Pause () {
		Time.timeScale = 0;
		pauseObject.SetActive (true);
	}

	public void Cancel () {
		Time.timeScale = 1;
		pauseObject.SetActive (false);
	}
}
