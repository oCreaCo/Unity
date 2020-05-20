using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour {
	public static WaveSpawner waveSpawner;
	private WaveDB waveDB;

	public enum SpawnState { SPAWNING, WAITING, COUNTING };

	[System.Serializable]
	public class Wave
	{
//		public string name;
		public int num;
		public Transform enemy;
		public int count;
		public float rate;
	}

	public enum MonsHealthType
	{
		ADD,
		MULTIPLY,
	}

	public MonsHealthType monsHealthType;

	public Wave[] waves;
	public int worldNum;
	public int stageNum;
	public int minGold;
	public int maxGold;
	private int nextWave = 0;
	public int round = 1;
	public float monsHealthMultiplyPercent;
	public float monsHealthAddAmount;
	public float monsHealthAdd;
	public float monsHealthMultiplier;

	public Transform[] spawnPoints;
	public Transform[] monsters;

	public float timeBetweenWaves;
	public float waveCountdown;
	private bool finished = false;

	private float searchCountdown = 1f;

	private SpawnState state = SpawnState.COUNTING;

	private int tmp;

	void Awake () {
		waveSpawner = this;
		waveDB = GetComponent<WaveDB> ();
		worldNum = PlayerPrefs.GetInt ("worldNum");
		stageNum = PlayerPrefs.GetInt ("stageNum");
		minGold = waveDB.worldManage [worldNum].minGold;
		maxGold = waveDB.worldManage [worldNum].maxGold;
		tmp = waveDB.worldManage [worldNum].stageManage [stageNum].waveList.Length;
	}

	void Start () {
		waves = new Wave[tmp];
		for (int j = 0; j < tmp; j++) {
			waves[j] = new Wave ();
		}
	}

	public void WaveStart () {
		if (spawnPoints.Length == 0) {
			Debug.LogError ("No spawn points referenced");
		}
		for (int j = 0; j < tmp; j++) {
			waves [j].enemy = monsters [waveDB.worldManage[worldNum].stageManage[stageNum].waveList[j].monsNum];
			waves [j].count = waveDB.worldManage[worldNum].stageManage[stageNum].waveList[j].monsSize;
			waves [j].rate = waveDB.worldManage[worldNum].stageManage[stageNum].waveList[j].delay;
			waves [j].num = j;
		}
		waveCountdown = timeBetweenWaves;
	}

	void Update() {
		if (worldNum == 0) {
			if (state == SpawnState.WAITING) {
				WaveCompleted();
				return;
			}
		}
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
	}

	bool EnemyIsAlive() {

		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f) {
			searchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag ("Monster") == null) {
				return false;
			}
		}
		return true;
	}

	void WaveCompleted() {
		Debug.Log("Wave Completed");

		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;

		if (nextWave + 1 > waves.Length - 1) {
			if (worldNum != 0) {
				GameMaster.gameMaster.IsFinished (true);
				finished = true;
				Debug.Log ("ALLWAVES COMPLETE!");
			} else if (worldNum == 0) {
				nextWave = -1;
				round++;
				if (round % 2 == 0) {
					Grid.grid.maxGold++;
				} else if (round % 2 != 0) {
					Grid.grid.minGold++;
				}
				monsHealthAdd += monsHealthAddAmount;
				monsHealthMultiplier += (monsHealthMultiplyPercent / 100);
			}
		}

		nextWave++;
	}

	IEnumerator SpawnWave (Wave _wave) {
		Debug.Log ("Spawning Wave: " + _wave.num);
		state = SpawnState.SPAWNING;
		//Spawn

		for (int i = 0; i < _wave.count; i++) {
			while (GameMaster.gameMaster.stopped) {
				yield return new WaitForSeconds (0.2f);
			}
			SpawnEnemy (_wave.enemy);
			yield return new WaitForSeconds (_wave.rate);

		}

		if (worldNum != 0) {
			WaveCompleted ();
		} else if (worldNum == 0) {
			state = SpawnState.WAITING;
		}

		yield break;
	}

	void SpawnEnemy (Transform _enemy) {
		Debug.Log ("Spawning Enemy: " + _enemy.name);

		Transform _sp = spawnPoints [Random.Range (0, spawnPoints.Length)];
		Instantiate(_enemy, _sp.position, _sp.rotation);
//		GameMaster.gameMaster.StartCheckFinished (0.1f);
//		if (_enemy.GetComponent<Monster_StatusController> () != null && _enemy.GetComponent<Monster_StatusController> ().goldSkillButton != null) {
//			if (GameMaster.gameMaster.goldSkilled) {
//				_enemy.GetComponent<Monster_StatusController> ().goldSkillButton.gameObject.SetActive (true);
//			} else {
//				_enemy.GetComponent<Monster_StatusController> ().goldSkillButton.gameObject.SetActive (false);
//			}
//		}
	}
}