using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDB : MonoBehaviour {

	[System.Serializable]
	public struct WaveStruct
	{
		public int monsNum;
		public int monsSize;
		public int delay;
	}

	[System.Serializable]
	public struct StageManage
	{
		public WaveStruct[] waveList;
	}

	[System.Serializable]
	public struct WorldManage
	{
		public StageManage[] stageManage;
		public int minGold;
		public int maxGold;
	}

	public WorldManage[] worldManage;
}
