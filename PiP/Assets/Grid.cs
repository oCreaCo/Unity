using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public enum BlockType
	{
		NORMAL,
		SHOOTER,
		MIRROR,
		RECEIVER,
		BUTTON,
		BEACON,
		ACTIVATER,
		PATHBLOCK,
	}

	public enum FacingDirection
	{
		UP,
		DOWN,
		RIGHT,
		LEFT,
	}

	public enum MirrorFacingDirection
	{
		LD,
		LU,
		RD,
		RU,
	}

	public enum PathType
	{
		LINE,
		CURVE,
		T,
		CROSS,
	}

	public enum ActivateType
	{
		CLOCKWISE,
		UNCLOCKWISE,
		HORMOVEABLE,
		VERMOVABLE,
		RAY,
	}

	[System.Serializable]
	public struct XBlockStruct
	{
		public bool spawn;
		public bool tile;
		public bool path;
		public bool teleporter;
		public bool inPuzzle;
		public bool horMovable;
		public bool verMovable;
		public int gridStateNum;
		public bool shootBool;
		public BlockType blockSort;
		public FacingDirection facingDirection;
		public MirrorFacingDirection mirrorFacingDirection;
		public ActivateType activateeType;
		public PathType pathType;
	}

	[System.Serializable]
	public struct YBlockStruct
	{
		public XBlockStruct[] xBlockStruct;
	}

	[System.Serializable]
	public struct StageStruct
	{
		public YBlockStruct[] yBlockStruct;
		public float startX, startY;
		public int layerCount;
	}

	public StageStruct[] gridDB;
	public StageStruct[] gridState;
	public StageStruct dummyGrid;
	public BlockSpawner blockSpawner;
	public Animator anim;

	[SerializeField] private GameMaster gameMaster;
	[SerializeField] private GameObject player;
	[SerializeField] private float startingX, startingY;
	public int[] indexes;
	private int index;

	public Transform stage;

	void Awake () {
		for (int i = 0; i < gridDB.Length; i++) {
			gridState [i] = gridDB [i];
		}
		index = PlayerPrefs.GetInt ("Index");
		blockSpawner.Spawn (indexes [index] , 0);
		anim = gameMaster.camera.GetComponent<Animator> ();
		SpawnPlayer (gridState [indexes [index]].startX, gridState [indexes [index]].startY, 0);
		gameMaster.camera.transform.position = new Vector3 (startingX, startingY, -10);
		StartCoroutine (CamMoveCorouitne (gameMaster.camera.transform.position, new Vector3 (gameMaster.player.position.x, gameMaster.player.position.y, -10), 1.5f));
	}

	public void SpawnPlayer (float _x, float _y, int _layerCount) {
		GameObject playerTemp = Instantiate (player, new Vector3 (_x, _y, 0), Quaternion.Euler (0, 0, 0), stage) as GameObject;
		playerTemp.GetComponent<Player> ().SetOrderInLayer ((int)(5 * (10 - _y) + 3) + _layerCount * 100);
		playerTemp.GetComponent<Player> ().blocks = stage.FindChild ("Blocks");
		gameMaster.player = playerTemp.transform;
		gameMaster.camera.SetParent (playerTemp.transform);
	}

	IEnumerator CamMoveCorouitne (Vector3 _startPos, Vector3 _endPos, float _movingTime) {
		if (index > 0) {
			blockSpawner.Spawn (indexes [index - 1], 1);
			gameMaster.stages [1].transform.localScale = new Vector3 (0.0625f, 0.0625f, 1);
			gameMaster.stages [1].transform.position = new Vector3 (gameMaster.camera.transform.position.x, gameMaster.camera.transform.position.y, 0);
			yield return new WaitForSeconds (3f);
			anim.SetTrigger ("Awake");
			yield return new WaitForSeconds (1.5f);
			GameObject temp = gameMaster.stages [1].gameObject;
			gameMaster.stages.Remove (gameMaster.stages [1]);
			Destroy (temp);
		} else {
			anim.SetTrigger ("Idle");
		}
		for (float t = 0; t < 1 * _movingTime; t += Time.deltaTime) {
			gameMaster.camera.transform.position = Vector3.Lerp (_startPos, _endPos, t / _movingTime);
			yield return 0;
		}
		gameMaster.camera.transform.position = _endPos;
		gameMaster.camera.transform.SetParent (gameMaster.player);
	}
}
