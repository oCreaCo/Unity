using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HWeapon090 : MonoBehaviour {

	[SerializeField] private GameMaster gameMaster;
	[SerializeField] private Grid grid;
	[SerializeField] private LegendSkillButton legendSkillButton;
	[SerializeField] private GameObject jewelFlare;

	public float skillTime;
	public float coolTime;
	public int skillCount;
	private int skillCountTemp;

	void Start () {
		gameMaster = GameMaster.gameMaster;
		grid = gameMaster.grid;
		if (gameMaster != null) {
			legendSkillButton = gameMaster.legendSkillBackGround.transform.FindChild ("LegendSkill").GetComponent<LegendSkillButton> ();
			skillTime = legendSkillButton.skillTime;
			coolTime = legendSkillButton.coolTime;
			skillCount = legendSkillButton.skillCount;
			skillCountTemp = skillCount;
		}
	}

	public void Skill () {
		StartCoroutine (CrystalChange (skillTime));
	}

	IEnumerator CrystalChange (float time) {
		int count = 2, temp = -1;
		List<GamePiece> purpleNormalJewels = new List<GamePiece> ();
		List<int> rInts = new List<int> ();
		List<GamePiece> purpleUltJewels = new List<GamePiece> ();
		for (int x = 0; x < 6; x++) {
			for (int y = 0; y < 6; y++) {
				if (grid.pieces [x, y].Type == Grid.PieceType.NORMAL && grid.pieces [x, y].ColorComponent.Color == ColorPiece.ColorType.PURPLE) {
					purpleNormalJewels.Add (grid.pieces [x, y]);
				}
			}
		}
		if (purpleNormalJewels.Count >= count) {
			legendSkillButton.LegendSkill ();
			for (int i = 0; i < purpleNormalJewels.Count; i++) rInts.Add (i);
			for (int i = 0; i < count; i++) {
				temp = Random.Range (0, rInts.Count);
				purpleUltJewels.Add (purpleNormalJewels [rInts [temp]]);
				rInts.Remove (rInts [temp]);
			}
			for (int i = 0; i < count; i++) {
				Instantiate (jewelFlare, purpleUltJewels [i].transform.position, purpleUltJewels [i].transform.rotation);
			}
			yield return new WaitForSeconds (time);
			for (int i = 0; i < count; i++) {
				int newX, newY, value;
				newX = purpleUltJewels [i].X;
				newY = purpleUltJewels [i].Y;
				value = purpleUltJewels [i].value;
				Destroy (grid.pieces [newX, newY].gameObject);
				grid.SpawnNewPiece (newX, newY, Grid.PieceType.ULTIMATE, value, GamePiece.PieceDebuffType.NONE).ColorComponent.Color = ColorPiece.ColorType.PURPLE;
			}
		}
	}
}
