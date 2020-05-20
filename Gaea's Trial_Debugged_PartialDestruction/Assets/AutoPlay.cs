using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlay : MonoBehaviour {

	[SerializeField] int matchDice;
	[SerializeField] int matchType;
	[SerializeField] int matchCountDice;
	[SerializeField] int value;

	[SerializeField] int redFourMatchCount;
	[SerializeField] int redFiveMatchCount;
	[SerializeField] int blueFourMatchCount;
	[SerializeField] int blueFiveMatchCount;
	[SerializeField] int purpleFourMatchCount;
	[SerializeField] int purpleFiveMatchCount;
	[SerializeField] int greenFourMatchCount;
	[SerializeField] int greenFiveMatchCount;

	public float fireRateMin = 1.7f;
	public float fireRateMax = 1.7f;
	public float fireRateTemp = 1.7f;

	public int redPercent;
	public int bluePercent;
	public int purplePercent;
	public int greenPercent;
	public int yellowPercent;

	public int fourMatchPercent;
	public int fiveMatchPercent;

	void Start () {
		StartCoroutine (AutoPlayCoroutine ());
	}

	IEnumerator AutoPlayCoroutine () {
		while (!GameMaster.gameMaster.allFinished) {
			fireRateTemp = Random.Range (fireRateMin, fireRateMax);
			yield return new WaitForSeconds (fireRateTemp);
			matchDice = Random.Range (0, 100);
			matchCountDice = Random.Range (0, 100);
			if (matchDice >= 0 && matchDice < redPercent) {
				matchType = 0;
				if (matchCountDice >= 0 && matchCountDice < fiveMatchPercent) {
					value = 5 * (Grid.grid.heroLevelMaxDamage + Grid.grid.heroLevelMinDamage) / 2;
				} else if (matchCountDice >= fiveMatchPercent && matchCountDice < fiveMatchPercent + fourMatchPercent) {
					value = 4 * (Grid.grid.heroLevelMaxDamage + Grid.grid.heroLevelMinDamage) / 2;
				} else {
					value = 3 * (Grid.grid.heroLevelMaxDamage + Grid.grid.heroLevelMinDamage) / 2;
				}
				GameMaster.gameMaster.GetRedValue (value);
				GameMaster.gameMaster.RedTrigger ();
				if (redFiveMatchCount != 0) {
					Grid.grid.UltClear (Grid.grid.pieces [Random.Range (0, 6), Random.Range (0, 6)]);
					redFiveMatchCount--;
				} else if (redFourMatchCount != 0) {
					int i = Random.Range (0, 2);
					if (i == 0) {
						Grid.grid.HorClear (Grid.grid.pieces [Random.Range (0, 6), Random.Range (0, 6)]);
					} else {
						Grid.grid.VerClear (Grid.grid.pieces [Random.Range (0, 6), Random.Range (0, 6)]);
					}
					redFourMatchCount--;
				}
				Grid.grid.hWeapon.gameObject.GetComponent<HeroWeapon> ().hWeaponSpawned.gameObject.GetComponent<HWeapon> ().Attack (GameMaster.gameMaster.redValue + GameMaster.gameMaster.redSkillValue + GameMaster.gameMaster.redWeaponValue + GameMaster.gameMaster.redBurningAddValue, Random.Range (GameMaster.gameMaster.weaponMin, GameMaster.gameMaster.weaponMax + 1));
			} else if (matchDice >= redPercent && matchDice < redPercent + bluePercent) {
				matchType = 1;
				if (matchCountDice >= 0 && matchCountDice < fiveMatchPercent) {
					value = 5 * (Grid.grid.heroLevelMaxDamage + Grid.grid.heroLevelMinDamage) / 2;
				} else if (matchCountDice >= fiveMatchPercent && matchCountDice < fiveMatchPercent + fourMatchPercent) {
					value = 4 * (Grid.grid.heroLevelMaxDamage + Grid.grid.heroLevelMinDamage) / 2;
				} else {
					value = 3 * (Grid.grid.heroLevelMaxDamage + Grid.grid.heroLevelMinDamage) / 2;
				}
				GameMaster.gameMaster.GetBlueValue (value);
				GameMaster.gameMaster.BlueTrigger ();
				if (blueFiveMatchCount != 0) {
					Grid.grid.heroPrefab.GetComponent<Hero> ().selectedHero.GetComponent<HeroSkill> ().UltSkill (value + GameMaster.gameMaster.blueSkillValue + GameMaster.gameMaster.blueWeaponValue + GameMaster.gameMaster.blueBurningAddValue, Random.Range (GameMaster.gameMaster.weaponMin, GameMaster.gameMaster.weaponMax + 1));
					blueFiveMatchCount--;
				} else if (blueFourMatchCount != 0) {
					int i = Random.Range (0, 2);
					if (i == 0) {
						Grid.grid.heroPrefab.GetComponent<Hero> ().selectedHero.GetComponent<HeroSkill> ().HorSkill (value + GameMaster.gameMaster.blueSkillValue + GameMaster.gameMaster.blueWeaponValue + GameMaster.gameMaster.blueBurningAddValue, Random.Range (GameMaster.gameMaster.weaponMin, GameMaster.gameMaster.weaponMax + 1));
					} else {
						Grid.grid.heroPrefab.GetComponent<Hero> ().selectedHero.GetComponent<HeroSkill> ().VerSkill (value + GameMaster.gameMaster.blueSkillValue + GameMaster.gameMaster.blueWeaponValue + GameMaster.gameMaster.blueBurningAddValue, Random.Range (GameMaster.gameMaster.weaponMin, GameMaster.gameMaster.weaponMax + 1));
					}
					blueFourMatchCount--;
				}
				Grid.grid.hWeapon.gameObject.GetComponent<HeroWeapon> ().hWeaponSpawned.gameObject.GetComponent<HWeapon> ().Attack (GameMaster.gameMaster.blueValue + GameMaster.gameMaster.blueSkillValue + GameMaster.gameMaster.blueWeaponValue + GameMaster.gameMaster.redBurningAddValue, Random.Range (GameMaster.gameMaster.weaponMin, GameMaster.gameMaster.weaponMax + 1));
			} else if (matchDice >= redPercent + bluePercent && matchDice < redPercent + bluePercent + purplePercent) {
				matchType = 2;
				if (matchCountDice >= 0 && matchCountDice < fiveMatchPercent) {
					value = 5 * (Grid.grid.heroLevelMaxDamage + Grid.grid.heroLevelMinDamage) / 2;
				} else if (matchCountDice >= fiveMatchPercent && matchCountDice < fiveMatchPercent + fourMatchPercent) {
					value = 4 * (Grid.grid.heroLevelMaxDamage + Grid.grid.heroLevelMinDamage) / 2;
				} else {
					value = 3 * (Grid.grid.heroLevelMaxDamage + Grid.grid.heroLevelMinDamage) / 2;
				}
				GameMaster.gameMaster.GetPurpleValue (value);
				GameMaster.gameMaster.PurpleTrigger ();
				if (purpleFiveMatchCount != 0) {
					Grid.grid.mWeapon.GetComponent<MagicianWeapon> ().mWeaponSpawned.transform.GetComponent<MWeaponSkill> ().UltSkill ();
					purpleFiveMatchCount--;
				} else if (purpleFourMatchCount != 0) {
					Grid.grid.mWeapon.GetComponent<MagicianWeapon> ().mWeaponSpawned.transform.GetComponent<MWeaponSkill> ().Skill ();
					purpleFourMatchCount--;
				}
				Magician.magician.Attack (GameMaster.gameMaster.purpleValue + GameMaster.gameMaster.purpleSkillValue + GameMaster.gameMaster.purpleBurningAddValue, GameMaster.gameMaster.purpleWeaponValue);
			} else if (matchDice >= redPercent + bluePercent + purplePercent && matchDice < redPercent + bluePercent + purplePercent + greenPercent) {
				matchType = 3;
				if (matchCountDice >= 0 && matchCountDice < fiveMatchPercent) {
					value = 10;
				} else if (matchCountDice >= fiveMatchPercent && matchCountDice < fiveMatchPercent + fourMatchPercent) {
					value = 8;
				} else {
					value = 6;
				}
				if (greenFiveMatchCount != 0) {
					GameMaster.gameMaster.GetGreenValue (50);
					greenFiveMatchCount--;
				} else if (greenFourMatchCount != 0) {
					GameMaster.gameMaster.GetGreenValue (20);
					greenFourMatchCount--;
				}
				GameMaster.gameMaster.GetGreenValue (value);
				GameMaster.gameMaster.GreenTrigger ();
				Castle.castle.GetHealth (GameMaster.gameMaster.greenValue + GameMaster.gameMaster.greenSkillValue + GameMaster.gameMaster.greenWeaponValue);
			} else if (matchDice >= redPercent + bluePercent + purplePercent + greenPercent && matchDice < 100) {
				matchType = 4;
				if (matchCountDice >= 0 && matchCountDice < fiveMatchPercent) {
					value = 5 * (Grid.grid.minGold + Grid.grid.maxGold) / 2;
				} else if (matchCountDice >= fiveMatchPercent && matchCountDice < fiveMatchPercent + fourMatchPercent) {
					value = 4 * (Grid.grid.minGold + Grid.grid.maxGold) / 2;
				} else {
					value = 3 * (Grid.grid.minGold + Grid.grid.maxGold) / 2;
				}
				GameMaster.gameMaster.GetYellowValue (value);
				GameMaster.gameMaster.YellowTrigger ();
				GameMaster.gameMaster.GetGold (GameMaster.gameMaster.yellowValue);
			}
			if (matchCountDice >= 0 && matchCountDice < fiveMatchPercent) {
				switch (matchType) {
				case 0:
					redFiveMatchCount++;
					break;
				case 1:
					blueFiveMatchCount++;
					break;
				case 2:
					purpleFiveMatchCount++;
					break;
				case 3:
					greenFiveMatchCount++;
					break;
				}
			} else if (matchCountDice >= fiveMatchPercent && matchCountDice < fiveMatchPercent + fourMatchPercent) {
				switch (matchType) {
				case 0:
					redFourMatchCount++;
					break;
				case 1:
					blueFourMatchCount++;
					break;
				case 2:
					purpleFourMatchCount++;
					break;
				case 3:
					greenFourMatchCount++;
					break;
				}
			}
			GameMaster.gameMaster.redValue  = 0;
			GameMaster.gameMaster.blueValue  = 0;
			GameMaster.gameMaster.greenValue  = 0;
			GameMaster.gameMaster.purpleValue  = 0;
			GameMaster.gameMaster.yellowValue  = 0;
		}
	}
}
