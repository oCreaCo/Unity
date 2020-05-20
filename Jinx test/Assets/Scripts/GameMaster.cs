using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;


	[SerializeField]
	private int maxLives = 3;
	private static int _remainingLives = 3;
	public static int RemainingLives
	{
		get { return _remainingLives; }
	}

	void Awake () 
	{
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}

	public Transform playerPrefab;
	public Transform spawnPoint;
	public int spawnDelay= 2;
	public Transform spawnPrefab;

	public CameraShake cameraShake;

	[SerializeField]
	private GameObject gameoverUI;

	void Start()
	{
		_remainingLives = maxLives;

		if (cameraShake == null) {
			Debug.LogError ("No 'cameraShake' object!");
		}
	}

	public void EndGame ()
	{
		Debug.LogError ("Gameover");
		gameoverUI.SetActive (true);
	}

	public IEnumerator RespawnPlayer () 
	{
		yield return new WaitForSeconds (spawnDelay);

		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		Transform clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
		Destroy (clone.gameObject, 2f);
	}

	public static void KillPlayer (Player player) 
	{
		Destroy (player.gameObject);
		_remainingLives -= 1;
		if (_remainingLives <= 0) {
			gm.EndGame ();
		} else {
			gm.StartCoroutine (gm.RespawnPlayer ());
		}
	}

	public static void KillEnemy (Enemy enemy)
	{
		Destroy (enemy.gameObject, 5);
	}
}
