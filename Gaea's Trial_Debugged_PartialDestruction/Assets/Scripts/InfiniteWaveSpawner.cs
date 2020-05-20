using UnityEngine;
using System.Collections;

public class InfiniteWaveSpawner : MonoBehaviour {

	public enum SpawnState { SPAWNING, WAITING, COUNTING };

	[System.Serializable]
	public class Wave
	{
		public string name;
		public Transform enemy;
		public int count;
		public float rate;
	}

	public Wave[] waves;
	private int nextWave = 0;

	public Transform[] spawnPoints;
	public Transform[] monsters;

	public float timeBetweenWaves;
	public float waveCountdown;
	private bool finished = false;

	private float searchCountdown = 1f;

	private SpawnState state = SpawnState.COUNTING;

	void Start () {
		if (spawnPoints.Length == 0) {
			Debug.LogError ("No spawn points referenced");
		}

		for (int j = 0; j < waves.Length; j++) {
			waves [j].enemy = monsters [PlayerPrefs.GetInt("monsNum" + j)];
			waves [j].count = PlayerPrefs.GetInt ("monsSize" + j);
			waves [j].rate = PlayerPrefs.GetInt ("delay" + j);
		}

		waveCountdown = timeBetweenWaves;
	}

	void Update() {
			if (state == SpawnState.WAITING) {
				if (!EnemyIsAlive ()) {
					// Begin a new round
					WaveCompleted();
				} 
				else {
					return;
				}
			}
		//		if (Grid.grid.Started) {
		if (!finished) {
			if (waveCountdown <= 0) {
				if (state != SpawnState.SPAWNING) {
					StartCoroutine (SpawnWave (waves [nextWave]));
					// Start spawning wave
				}
			}
		} else {
			waveCountdown -= Time.deltaTime;
		}
		//		}
	}

	void WaveCompleted() {
		Debug.Log("Wave Completed");

		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;

		if (nextWave + 1 > waves.Length - 1) {
			nextWave = 0;
//			GameMaster.gamemaster.IsFinished (true);
			finished = true;
			Debug.Log ("ALLWAVES COMPLETE!");
		}

		nextWave++;
	}

	bool EnemyIsAlive() {
		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f) {
			searchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag ("GT_Monster") == null) {
				return false;
			}
		}
		return true;
	}

	IEnumerator SpawnWave (Wave _wave) {
		Debug.Log ("Spawning Wave: " + _wave.name);
		state = SpawnState.SPAWNING;
		//Spawn

		for (int i = 0; i < _wave.count; i++) {
			SpawnEnemy (_wave.enemy);
			yield return new WaitForSeconds (_wave.rate);
		}

		WaveCompleted();
			state = SpawnState.WAITING;

		yield break;
	}

	void SpawnEnemy (Transform _enemy) {
		// Spawn enemy
		Debug.Log ("Spawning Enemy: " + _enemy.name);

		Transform _sp = spawnPoints [Random.Range (0, spawnPoints.Length)];
		Instantiate(_enemy, _sp.position, _sp.rotation);
		GameMaster.gameMaster.StartCheckFinished (0.1f);
//		if (_enemy.GetComponent<Monster_StatusController> () != null) {
//			if (GameMaster.gameMaster.goldSkilled) {
//				_enemy.GetComponent<Monster_StatusController> ().goldSkillButton.gameObject.SetActive (true);
//			} else {
//				_enemy.GetComponent<Monster_StatusController> ().goldSkillButton.gameObject.SetActive (false);
//			}
//		}
	}
}
