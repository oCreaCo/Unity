using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameMaster : MonoBehaviour, IEventSystemHandler {
	public static GameMaster gameMaster;
	public Grid grid;
	private GemGoldScript gemGoldScript;
	public List<Barricade> barricades;

	public Transform monsterInfo;
	public Transform battleTutorial;
	public Transform legendSkillGameObjects;

	public bool isClickable = false;
	public bool isFinished = false;
	public bool yellowAlert = false;
	public bool clicked = false;
	public int worldNum;
	public int nextWorldNum;
	public int stageNum;
	public int nextStageNum;
	public int stack = 0;
	public int goldGauge = 0;

	public int redValue = 0;
	public int redSkillValue = 0;
	public int redWeaponValue = 0;
	public int blueValue = 0;
	public int blueSkillValue = 0;
	public int blueWeaponValue = 0;
	public int greenValue = 0;
	public int greenSkillValue = 0;
	public int greenWeaponValue = 0;
	public int purpleValue = 0;
	public int purpleSkillValue = 0;
	public int purpleWeaponValue = 0;
	public int yellowValue = 0;
	public int yellowSkillValue = 0;
	public int yellowWeaponValue = 0;

	public int redBurningAddValue = 0;
	public int blueBurningAddValue = 0;
	public int purpleBurningAddValue = 0;

	public bool redBool = false;
	public bool blueBool = false;
	public bool greenBool = false;
	public bool purpleBool = false;
	public bool yellowBool = false;
	public List<Button.ButtonClickedEvent> redTrigger;
	public List<Button.ButtonClickedEvent> blueTrigger;
	public List<Button.ButtonClickedEvent> purpleTrigger;
	public List<Button.ButtonClickedEvent> greenTrigger;
	public List<Button.ButtonClickedEvent> yellowTrigger;

	private List<GameObject> emptyJewels = new List<GameObject> ();

	public int oriScore;
	public int curScore;

	public int stageGold = 0;
	public int gettingGold;
	public int Gem = 0;

	[SerializeField]
	private int gridEquippedHWeapon;
	public int weaponMin = 0;
	public int weaponMax = 0;
	public int weaponType = 0;
	public int weaponChance = 0;
	public int weaponPenetration = 0;
	public int combo;
	public bool burn;
	public bool[] burningTier;
	public int[] startToBurn;
	public float[] burningTime;
	public float[] burningTimeLimit;

	public bool stackModifiable;
	public bool win;
	public bool isUI;
	public bool allFinished;

	public Text goldText;

	[SerializeField] private GameObject gameOverUI;
	[SerializeField] private GameObject pauseUI;
	[SerializeField] private GameObject yellowAlertObject;
	[SerializeField] private GameObject camera;
	public GameObject goldUI;
	public GameObject comboUI;
	public GameObject burningUI;
	public GameObject volumeUI;
	public Transform goldUISpawnPoint;
	public Transform comboUISpawnPoint;
	public Transform burningUISpawnPoint;
	public Transform burningUITransform;
	public Button goldSkill;
	public GameObject legendSkillBackGround;
	public float skillTime;
	public float coolTime;
	[SerializeField] private RectTransform goldGaugeBarRect;
	[SerializeField] private RectTransform TapGaugeBarRect;
	[SerializeField] private GameObject tapCollider;
	[SerializeField] private Text goldSkillTimer;
	[SerializeField] private GameObject goldSkillObject;
	[SerializeField] private Transform goldSkillCameraPosition;
	public Image goldSkillDarken;

	public List<GameObject> remainingEnemies;
	public GameObject[] specialEnemies;

	public Text pausedText;
	public Text resumeText;
	public Text menuText;

	private bool monsDic = false;

	void Awake ()
	{
		if (gameMaster == null)
		{
			gameMaster = this.GetComponent<GameMaster>();
		}
		gemGoldScript = GetComponent<GemGoldScript> ();
		gemGoldScript.GemGoldAwake ();
		allFinished = false;
		stackModifiable = true;
		worldNum = PlayerPrefs.GetInt ("worldNum");
		stageNum = PlayerPrefs.GetInt ("stageNum");
		weaponMin = PlayerPrefs.GetInt ("GridEquippedHWeaponMinDamage");
		weaponMax = PlayerPrefs.GetInt ("GridEquippedHWeaponMaxDamage");
		weaponType = PlayerPrefs.GetInt ("GridEquippedHWeaponType");
		weaponChance = PlayerPrefs.GetInt ("GridEquippedHWeaponChance");
		weaponPenetration = PlayerPrefs.GetInt ("GridEquippedHWeaponPenetration");
		redWeaponValue = PlayerPrefs.GetInt ("GridEquippedHWeaponRed");
		blueWeaponValue = PlayerPrefs.GetInt ("GridEquippedHWeaponBlue");
		greenWeaponValue = PlayerPrefs.GetInt ("GridEquippedHWeaponGreen");
		purpleWeaponValue = PlayerPrefs.GetInt ("GridEquippedHWeaponPurple") + PlayerPrefs.GetInt ("GridEquippedMWeaponDamage");
		yellowWeaponValue = PlayerPrefs.GetInt ("GridEquippedHWeaponBrown");
		goldText.text = (stageGold * 5).ToString ();
		SetGoldGaugeBar (0, 1);
		gridEquippedHWeapon = PlayerPrefs.GetInt ("GridEquippedHWeapon");
		skillTime = (float)PlayerPrefs.GetInt ("GridEquippedHWeaponLegendSkillTime");
		coolTime = (float)PlayerPrefs.GetInt ("GridEquippedHWeaponLegendCoolTime");
		if (gridEquippedHWeapon > 85 && gridEquippedHWeapon != 89 && gridEquippedHWeapon != 94) {
			legendSkillBackGround.SetActive (true);
		}
		if (stageNum != 14) {
			nextWorldNum = worldNum;
			nextStageNum = stageNum + 1;
		} else if (stageNum == 14) {
			nextWorldNum = worldNum + 1;
			nextStageNum = 0;
		}
		switch (worldNum) {
		case 1:
			switch (stageNum) {
			case 0:
				if (PlayerPrefs.GetInt ("MonsterDictionaryDeminion") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryDeminion", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 1);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 1;
				}
				break;
			case 1:
				if (PlayerPrefs.GetInt ("MonsterDictionaryEarthElementaling") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryEarthElementaling", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 2);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 2;
				}
				break;
			case 4:
				if (PlayerPrefs.GetInt ("MonsterDictionarySlime") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionarySlime", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 3);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 3;
				}
				break;
			case 9:
				if (PlayerPrefs.GetInt ("MonsterDictionaryNependeath") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryNependeath", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 4);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 4;
				}
				break;
			case 14:
				if (PlayerPrefs.GetInt ("MonsterDictionaryEarthElemental") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryEarthElemental", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 5);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 5;
				}
				break;
			}
			break;
		case 2:
			switch (stageNum) {
			case 0: 
				if (PlayerPrefs.GetInt("MonsterDictionarySandeminion") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionarySandeminion", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 6);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 6;
				}
				break;
			case 1:
				if (PlayerPrefs.GetInt("MonsterDictionaryCrazySand") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryCrazySand", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 7);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 7;
				}
				break;
			case 3: 
				if (PlayerPrefs.GetInt("MonsterDictionaryTumbleWeed") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryTumbleWeed", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 8);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 8;
				}
				break;
			case 5: 
				if (PlayerPrefs.GetInt("MonsterDictionaryMummy") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryMummy", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 9);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 9;
				}
				break;
			case 6: 
				if (PlayerPrefs.GetInt("MonsterDictionaryKing'sBurial") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryKing'sBurial", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 10);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 10;
				}
				break;
			case 9: 
				if (PlayerPrefs.GetInt("MonsterDictionaryCamel") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryCamel", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 11);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 11;
				}
				break;
			case 14: 
				if (PlayerPrefs.GetInt("MonsterDictionarySandWhale") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionarySandWhale", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 12);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 12;
				}
				break;
			}
			break;
		case 3:
			switch (stageNum) {
			case 0: 
				if (PlayerPrefs.GetInt("MonsterDictionaryColdeminion") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryColdeminion", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 13);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 13;
				}
				break;
			case 1: 
				if (PlayerPrefs.GetInt("MonsterDictionarySnowman") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionarySnowman", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 14);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 14;
				}
				break;
			case 4: 
				if (PlayerPrefs.GetInt("MonsterDictionaryBergbits") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryBergbits", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 15);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 15;
				}
				break;
			case 8: 
				if (PlayerPrefs.GetInt("MonsterDictionaryPenguin") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryPenguin", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 16);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 16;
				}
				break;
			case 14: 
				if (PlayerPrefs.GetInt("MonsterDictionaryCloudBear") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryCloudBear", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 17);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 17;
				}
				break;
			}
			break;
		case 4:
			switch (stageNum) {
			case 0: 
				if (PlayerPrefs.GetInt("MonsterDictionaryDarkdeminion") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryDarkeminion", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 18);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 18;
				}
				break;
			case 1: 
				if (PlayerPrefs.GetInt("MonsterDictionaryAirSlug") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryAirSlug", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 19);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 19;
				}
				break;
			case 2: 
				if (PlayerPrefs.GetInt("MonsterDictionaryZlime") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryZlime", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 22);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 22;
				}
				break;
			case 4: 
				if (PlayerPrefs.GetInt("MonsterDictionaryMimic") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryMimic", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 20);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 20;
				}
				break;
			case 6: 
				if (PlayerPrefs.GetInt("MonsterDictionaryParasuck") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryParasuck", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 21);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 21;
				}
				break;
			case 14: 
				if (PlayerPrefs.GetInt("MonsterDictionaryBabyCavian") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryBabyCavian", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 23);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 23;
				}
				break;
			}
			break;
		case 5:
			switch (stageNum) {
			case 0: 
				if (PlayerPrefs.GetInt("MonsterDictionaryWoodeminion") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryWoodeminion", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 24);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 24;
				}
				break;
			case 1: 
				if (PlayerPrefs.GetInt("MonsterDictionaryLogger") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryLogger", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 25);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 25;
				}
				break;
			case 3: 
				if (PlayerPrefs.GetInt("MonsterDictionaryRaballetia") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryRaballetia", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 26);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 26;
				}
				break;
			case 7: 
				if (PlayerPrefs.GetInt("MonsterDictionaryMushBoom") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryMushBoom", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 27);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 27;
				}
				break;
			case 14: 
				if (PlayerPrefs.GetInt("MonsterDictionaryAmazonessTree") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryAmazonessTree", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 28);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 28;
				}
				break;
			}
			break;
		case 6:
			switch (stageNum) {
			case 0: 
				if (PlayerPrefs.GetInt("MonsterDictionaryMudeminion") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryMudeminion", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 29);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 29;
				}
				break;
			case 1: 
				if (PlayerPrefs.GetInt("MonsterDictionaryPuffer") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryPuffer", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 30);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 30;
				}
				break;
			case 3: 
				if (PlayerPrefs.GetInt("MonsterDictionaryDandelion") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryDandelion", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 31);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 31;
				}
				break;
			case 4: 
				if (PlayerPrefs.GetInt("MonsterDictionaryDarkMagician") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryDarkMagician", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 32);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 32;
				}
				break;
			case 14: 
				if (PlayerPrefs.GetInt("MonsterDictionaryNecro") == 0) {
					PlayerPrefs.SetInt ("MonsterDictionaryNecro", 1);
					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", 33);
					monsDic = true;
					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = 33;
				}
				break;
			}
			break;
//		case 7:
//			switch (stageNum) {
//			case 0: 
//				if (PlayerPrefs.GetInt("MonsterDictionary") == 0) {
//					PlayerPrefs.SetInt ("MonsterDictionary", 1);
//					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", );
//					monsDic = true;
//					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = ;
//				}
//				break;
//			case 0: 
//				if (PlayerPrefs.GetInt("MonsterDictionary") == 0) {
//					PlayerPrefs.SetInt ("MonsterDictionary", 1);
//					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", );
//					monsDic = true;
//					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = ;
//				}
//				break;
//			case 0: 
//				if (PlayerPrefs.GetInt("MonsterDictionary") == 0) {
//					PlayerPrefs.SetInt ("MonsterDictionary", 1);
//					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", );
//					monsDic = true;
//					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = ;
//				}
//				break;
//			case 0: 
//				if (PlayerPrefs.GetInt("MonsterDictionary") == 0) {
//					PlayerPrefs.SetInt ("MonsterDictionary", 1);
//					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", );
//					monsDic = true;
//					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = ;
//				}
//				break;
//			case 0: 
//				if (PlayerPrefs.GetInt("MonsterDictionary") == 0) {
//					PlayerPrefs.SetInt ("MonsterDictionary", 1);
//					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", );
//					monsDic = true;
//					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = ;
//				}
//				break;
//			}
//			break;
//		case 8:
//			switch (stageNum) {
//			case 0: 
//				if (PlayerPrefs.GetInt("MonsterDictionary") == 0) {
//					PlayerPrefs.SetInt ("MonsterDictionary", 1);
//					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", );
//					monsDic = true;
//					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = ;
//				}
//				break;
//			case 0: 
//				if (PlayerPrefs.GetInt("MonsterDictionary") == 0) {
//					PlayerPrefs.SetInt ("MonsterDictionary", 1);
//					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", );
//					monsDic = true;
//					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = ;
//				}
//				break;
//			case 0: 
//				if (PlayerPrefs.GetInt("MonsterDictionary") == 0) {
//					PlayerPrefs.SetInt ("MonsterDictionary", 1);
//					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", );
//					monsDic = true;
//					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = ;
//				}
//				break;
//			case 0: 
//				if (PlayerPrefs.GetInt("MonsterDictionary") == 0) {
//					PlayerPrefs.SetInt ("MonsterDictionary", 1);
//					PlayerPrefs.SetInt ("MonsterDictionaryMaxMonsNum", );
//					monsDic = true;
//					monsterInfo.GetComponent<MonsterInfo> ().curMonsNum = ;
//				}
//				break;
//			}
//			break;
		}
	}

	void Start () {
		if (gridEquippedHWeapon == 89) StartCoroutine (DotDamage (100, 1, 3f));
		if (AudioManager.instance != null) {
			if (AudioManager.instance.playingBGMusic) {
				AudioManager.instance.playingBGMusic = false;
				AudioManager.instance.transform.FindChild ("Sound_1_BGMusic").GetComponent<AudioSource> ().Stop ();
			}
			if (!AudioManager.instance.playingStageBGMusic) {
				AudioManager.instance.playingStageBGMusic = true;
				AudioManager.instance.PlaySound ("World" + worldNum + "BGM");
			}
		}
		if (PlayerPrefs.GetInt ("FirstBattle") == 0) {
			PlayerPrefs.SetInt ("FirstBattle", 1);
			battleTutorial.gameObject.SetActive (true);
		} else {
			Destroy (battleTutorial.gameObject);
		}
		if (!monsDic) {
			grid.GridStart ();
		} else {
			monsterInfo.gameObject.SetActive (true);
		}
	}

	public void ComboAdd ()
	{
		if (grid.started) {
			combo++;
			if (combo > 1) {
				GameObject comboUIClone = Instantiate (comboUI, comboUISpawnPoint.position, comboUISpawnPoint.rotation) as GameObject;
				comboUIClone.transform.FindChild ("ComboText").GetComponent<Text> ().text = combo.ToString () + " Combos!";
				Destroy (comboUIClone.gameObject, 1f);
			}
			if (combo >= startToBurn [0] && !burningTier[0]) {
				if (!burn) {
					burn = true;
					GameObject burningUIClone = Instantiate (burningUI, burningUISpawnPoint.position, burningUISpawnPoint.rotation) as GameObject;
					burningUITransform = burningUIClone.transform;
				}
				burningTier [0] = true;
				redBurningAddValue = grid.heroLevelMinDamage;
				blueBurningAddValue = grid.heroLevelMinDamage;
				purpleBurningAddValue = grid.heroLevelMinDamage;
				burningTimeLimit [0] = Time.time + burningTime [0];
			} else if (combo >= startToBurn [1] && !burningTier[1]) {
				burningTier [1] = true;
				redBurningAddValue = grid.heroLevelMinDamage + (int)((grid.heroLevelMaxDamage - grid.heroLevelMinDamage) / 2);
				blueBurningAddValue = grid.heroLevelMinDamage + (int)((grid.heroLevelMaxDamage - grid.heroLevelMinDamage) / 2);
				purpleBurningAddValue = grid.heroLevelMinDamage + (int)((grid.heroLevelMaxDamage - grid.heroLevelMinDamage) / 2);
				burningTimeLimit [0] = Time.time + burningTime [0] + burningTime [1];
				burningTimeLimit [1] = Time.time + burningTime [1];
			} else if (combo >= startToBurn [2] && !burningTier[2]) {
				burningTier [2] = true;
				redBurningAddValue = grid.heroLevelMaxDamage;
				blueBurningAddValue = grid.heroLevelMaxDamage;
				purpleBurningAddValue = grid.heroLevelMaxDamage;
				burningTimeLimit [0] = Time.time + burningTime [0] + burningTime [1] + burningTime [2];
				burningTimeLimit [1] = Time.time + burningTime [1] + burningTime [2];
				burningTimeLimit [2] = Time.time + burningTime [2];
			} else if (combo >= startToBurn [3] && !burningTier[3]) {
				burningTier [3] = true;
				redBurningAddValue = grid.heroLevelMaxDamage + (int)((grid.heroLevelMaxDamage - grid.heroLevelMinDamage) / 2);
				blueBurningAddValue = grid.heroLevelMaxDamage + (int)((grid.heroLevelMaxDamage - grid.heroLevelMinDamage) / 2);
				purpleBurningAddValue = grid.heroLevelMaxDamage + (int)((grid.heroLevelMaxDamage - grid.heroLevelMinDamage) / 2);
				burningTimeLimit [0] = Time.time + burningTime [0] + burningTime [1] + burningTime [2] + burningTime [3];
				burningTimeLimit [1] = Time.time + burningTime [1] + burningTime [2] + burningTime [3];
				burningTimeLimit [2] = Time.time + burningTime [2] + burningTime [3];
				burningTimeLimit [3] = Time.time + burningTime [3];
			}
		}
	}

	public void IsClickable (bool a)
	{
		Debug.LogError ("isclickable " + a);
		if (a) {
			combo = 0;
			emptyJewels.AddRange (GameObject.FindGameObjectsWithTag ("EmptyJewel"));
			for (int i = 0; i < emptyJewels.Count; i++) {
				Destroy (emptyJewels [i]);
			}
			emptyJewels.Clear ();
			isClickable = true;
			//Debug.LogError ("isClickable is true.");
		}else {
			isClickable = false;
			//Debug.LogError ("isClickable is false.");
		}
	}

	public void IsFinished (bool b)
	{
		if (b) {
			isFinished = true;
		} else {
			isFinished = false;
		}
	}

	public void WinOrLose (bool wl) {
		if (wl) {
			win = true;
		} else if (!wl) {
			win = false;
		}
	}

	public void StartCheckFinished (float time) {
		StartCoroutine (CheckFinished (time));
	}

	public void StackModifier(bool a)
	{
		if (stackModifiable) {
			if (a) {
				stack++;
				grid.fillTime2 += 1.0f;
			} else {
				stack = 0;
				grid.fillTime2 = grid.fillTime;
			}
		} else if (!stackModifiable) {
			if (a) {
				stack++;
				grid.fillTime2 += 1.0f;
			} else {
				stack = 1;
				grid.fillTime2 = grid.fillTime + 1.0f;
			}
		}
	}

	public void GetRedValue (int val) {
		redValue += val ;
	}

	public void GetBlueValue (int val) {
		blueValue += val;
	}

	public void GetGreenValue (int val) {
		greenValue += val;
	}

	public void GetPurpleValue (int val) {
		purpleValue += val;
	}

	public void GetYellowValue (int val) {
		yellowValue += val;
	}

	public void RedTrigger () {
		Trigger (ref redTrigger);
	}

	public void BlueTrigger () {
		Trigger (ref blueTrigger);
	}

	public void PurpleTrigger () {
		Trigger (ref purpleTrigger);
	}

	public void GreenTrigger () {
		Trigger (ref greenTrigger);
	}

	public void YellowTrigger () {
		Trigger (ref yellowTrigger);
	}

	private void Trigger (ref List<Button.ButtonClickedEvent> _trigger) {
		for (int i = 0; i < _trigger.Count; i++) {
			_trigger [i].Invoke ();
		}
	}

	public void GetGold (int gol) {
		if (gol != 0) {
			GameObject goldUIClone = Instantiate (goldUI, goldUISpawnPoint.position, goldUISpawnPoint.rotation) as GameObject;
			goldUIClone.transform.FindChild ("GoldPlusText").GetComponent<Text> ().text = "+" + (gol * 5).ToString ();
			Destroy (goldUIClone.gameObject, 1f);
		}
		gettingGold = gol;
		goldGauge += gol;
		if (goldGauge >= 55) {
			goldGauge = 55;
		}
		if (goldGauge < 50) {
			goldSkill.interactable = false;
		} else if (goldGauge >= 50) {
			goldSkill.interactable = true;
			goldGauge = 50;
		}
		SetGoldGaugeBar (goldGauge, 50);
		stageGold += gol;
		goldText.text = (stageGold * 5).ToString ();
	}

	public void LegendSkill() {
		switch (PlayerPrefs.GetInt ("GridEquippedHWeapon")) {
		case 86:
			grid.hWeapon.FindChild("086_LuxShield(Clone)").GetComponent<HWeapon086>().Skill();
			break;
		case 87:
			legendSkillBackGround.transform.FindChild ("LegendSkill").GetComponent<LegendSkillButton> ().LegendSkill ();
			HWeapon.hweapon.legendSkillCount = legendSkillBackGround.transform.FindChild ("LegendSkill").GetComponent<LegendSkillButton> ().skillCount;
			break;
		case 88:
			grid.hWeapon.FindChild("088_BrokenWill(Clone)").GetComponent<HWeapon088>().Skill();
			break;
		case 90:
			grid.hWeapon.FindChild("090_Crystal(Clone)").GetComponent<HWeapon090>().Skill();
			break;
		case 91:
			legendSkillBackGround.transform.FindChild ("LegendSkill").GetComponent<LegendSkillButton> ().LegendSkill ();
			HWeapon.hweapon.legendSkillCount = legendSkillBackGround.transform.FindChild ("LegendSkill").GetComponent<LegendSkillButton> ().skillCount;
			break;
		case 92:
			grid.hWeapon.FindChild("092_HephaistusHammer(Clone)").GetComponent<HWeapon092>().Skill();
			break;
		case 93:
			grid.hWeapon.FindChild("093_Eve(Clone)").GetComponent<HWeapon093>().Skill();
			break;
		}
	}

	IEnumerator DotDamage (int count, int _dotDamage, float _delay) {
		for (int i = 0; i < count; i++) {
			barricades [0].GetHurt (_dotDamage, 1f, 0.2f);
			yield return new WaitForSeconds (_delay);
			if (count == 100) i--;
		}
	}

	public bool stopped = false;
	public float tapGauge;
	public void GoldSkill () {
		if (!stopped) {
			stopped = true;
			CameraLerpPosition.cameraLerpPosition.cameraToPos = goldSkillCameraPosition.position;
			for (int i = 0; i < remainingEnemies.Count; i++) {
				if (remainingEnemies [i].GetComponent<Monster_Basic> () != null) {
					remainingEnemies [i].GetComponent<Monster_Basic> ().moveSpeedTemp = 0;
				}
				if (remainingEnemies [i].GetComponent<Animator> () != null) {
					remainingEnemies [i].GetComponent<Animator> ().enabled = false;
				}
			}
			TapGaugeBarRect.gameObject.SetActive (true);
			CameraLerpPosition.cameraLerpPosition.GoLerp ();
			TapCollierToggle ();
			CameraGoldSkill.tapCollider.tapGaugeAddValue = 7.0f;
			StartCoroutine (SetGoldSkillBarCoroutine (5f));
		}
	}

	IEnumerator SetGoldSkillBarCoroutine (float _time) {
		goldGauge = 0;
		tapGauge = 0;
		goldSkill.interactable = false;
		goldSkillTimer.gameObject.SetActive (true);
		for (float i = 0; i <= _time; i += 0.01f) {
			yield return new WaitForSeconds (0.01f);
			goldSkillTimer.text = (_time - i).ToString ();
			SetGoldGaugeBar (_time - i, _time);
			tapGauge -= 0.64f;
			if (tapGauge < 0) {
				tapGauge = 0;
			}
			SetTapGaugeBar (tapGauge, 100);
			if (tapGauge >= 100f) {
				SetTapGaugeBar (1, 1);
				SetGoldGaugeBar (0, 1);
				goldGauge = 0;
				Instantiate (goldSkillObject);
				break;
			}
		}
		goldSkillTimer.text = 5.00f.ToString ();
		goldSkillTimer.gameObject.SetActive (false);
		stopped = false;
		tapGauge = 0;
		for (int i = 0; i < remainingEnemies.Count; i++) {
			if (!remainingEnemies [i].GetComponent<Monster_Basic> ().collided) {
				remainingEnemies [i].GetComponent<Monster_Basic> ().moveSpeedTemp = remainingEnemies [i].GetComponent<Monster_Basic> ().moveSpeed;
			}
			if (remainingEnemies [i].GetComponent<Animator> () != null) {
				remainingEnemies [i].GetComponent<Animator> ().enabled = true;
			}
		}
		TapGaugeBarRect.gameObject.SetActive (false);
		CameraLerpPosition.cameraLerpPosition.BackLerp ();
		TapCollierToggle ();
	}

	public void TapCollierToggle () {
		if (!tapCollider.activeInHierarchy) {
			tapCollider.SetActive (true);
		} else {
			tapCollider.SetActive (false);
		}
	}

	public void SetGoldGaugeBar (float _cur, float _max) {
		float _value = 1f;
		if ((float)_cur / _max <= 1) {
			_value = (float)_cur / _max;
		} else {
			_value = 1f;
		}

		goldGaugeBarRect.localScale = new Vector3 (_value, goldGaugeBarRect.localScale.y, goldGaugeBarRect.localScale.z);
	}

	public void SetTapGaugeBar (float _cur, float _max) {
		float _value = 1f;
		if ((float)_cur / _max <= 1) {
			_value = (float)_cur / _max;
		} else {
			_value = 1f;
		}
		TapGaugeBarRect.localScale = new Vector3 (_value, TapGaugeBarRect.localScale.y, TapGaugeBarRect.localScale.z);
	}

	void Update () {
		if (grid.groundRayCast.GetComponent<ARayCast>().monsterXPosition <= -0.01f && yellowAlert == false) {
			yellowAlert = true;
			if (grid.groundRayCast.GetComponent<ARayCast> ().frontMonster.transform.GetComponent<Monster_Basic> ().yellowAlert == null) {
				GameObject yellowAlertClone = Instantiate (yellowAlertObject, grid.groundRayCast.GetComponent<ARayCast> ().frontMonster.transform.position, grid.groundRayCast.GetComponent<ARayCast> ().frontMonster.transform.rotation) as GameObject;
				yellowAlertClone.transform.parent = grid.groundRayCast.GetComponent<ARayCast> ().frontMonster.transform;
				if (grid.groundRayCast.GetComponent<ARayCast> ().frontMonster.transform.GetComponent<Monster_Basic> () != null) {
					grid.groundRayCast.GetComponent<ARayCast> ().frontMonster.transform.GetComponent<Monster_Basic> ().yellowAlert = yellowAlertClone;
				}
			}
		}
		if (burn) {
			if (Time.time > burningTimeLimit [0]) {
				burn = false;
				burningTier [0] = false;
				redBurningAddValue = 0;
				blueBurningAddValue = 0;
				purpleBurningAddValue = 0;
				Destroy (burningUITransform.gameObject);
				burningUITransform = null;
			} else if (Time.time <= burningTimeLimit [0] && Time.time > burningTimeLimit [1]) {
				burningTier [1] = false;
				redBurningAddValue = grid.heroLevelMinDamage;
				blueBurningAddValue = grid.heroLevelMinDamage;
				purpleBurningAddValue = grid.heroLevelMinDamage;
			} else if (Time.time <= burningTimeLimit [1] && Time.time > burningTimeLimit [2]) {
				burningTier [2] = false;
				redBurningAddValue = grid.heroLevelMinDamage + (int)((grid.heroLevelMaxDamage - grid.heroLevelMinDamage) / 2);
				blueBurningAddValue = grid.heroLevelMinDamage + (int)((grid.heroLevelMaxDamage - grid.heroLevelMinDamage) / 2);
				purpleBurningAddValue = grid.heroLevelMinDamage + (int)((grid.heroLevelMaxDamage - grid.heroLevelMinDamage) / 2);
			} else if (Time.time <= burningTimeLimit [2] && Time.time > burningTimeLimit [3]) {
				burningTier [3] = false;
				redBurningAddValue = grid.heroLevelMaxDamage;
				blueBurningAddValue = grid.heroLevelMaxDamage;
				purpleBurningAddValue = grid.heroLevelMaxDamage;
			}
		}
	}

	public void Pause () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		isUI = true;
		pauseUI.SetActive (true);
		SetText ();
		Time.timeScale = 0;
	}

	public void Resume () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
		}
		isUI = false;
		pauseUI.SetActive (false);
		Time.timeScale = 1;
	}

	public void Menu () {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlaySound ("Click");
			AudioManager.instance.playingStageBGMusic = false;
			AudioManager.instance.transform.FindChild ("Sound_" + (worldNum + 3) + "_World" + worldNum + "BGM").GetComponent<AudioSource> ().Stop ();
		}
		isUI = false;
		Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex  + 3);
	}

	IEnumerator CheckFinished (float time) {
		yield return new WaitForSeconds (time);
		if (remainingEnemies.Count == 0 && isFinished == true && allFinished == false) {
			allFinished = true;
			isUI = true;
			WinOrLose (true);
			gameOverUI.SetActive (true);
			gameOverUI.GetComponent<GameOverUI> ().WinOrLose (true);
		}
	}

	private void SetText () {
		if (PlayerPrefs.GetInt ("Language") == 0) {
			pausedText.text = "PAUSED";
			resumeText.text = "Resume";
			menuText.text = "Menu";
		} else if (PlayerPrefs.GetInt ("Language") == 1) {
			pausedText.text = "일시정지";
			resumeText.text = "재개";
			menuText.text = "메뉴";
		}
	}

	public void TimeScale () {
		
		if (clicked == false) {
			Debug.LogError ("Clicked");
			clicked = true;
			Time.timeScale = 0.2f;
		} else if (clicked == true) {
			Debug.LogError ("UnClicked");
			clicked = false;
			Time.timeScale = 1;
		}
	}

	public void Volume () {
		volumeUI.SetActive (true);
	}

	public void Test () {
		bool test = false;
		TestTest (ref test);
		if (test) {
			Debug.LogError ("true");
		} else {
			Debug.LogError ("false");
		}
	}

	private void TestTest (ref bool _test) {
		_test = true;
	}
}
