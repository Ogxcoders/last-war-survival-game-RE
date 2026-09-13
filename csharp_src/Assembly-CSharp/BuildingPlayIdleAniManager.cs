using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlayIdleAniManager
{
	public struct MixTimeData
	{
		public int startTime;

		public int endTime;

		public MixTimeData(int StartTime, int EndTime)
		{
			startTime = StartTime;
			endTime = EndTime;
		}
	}

	public struct AniData
	{
		public string aniName;

		public int aniLoopNum;

		public int isMix;

		public List<MixTimeData> mixTimeList;

		public AniData(string AniName, int AniLoopNum, int IsMix, List<MixTimeData> MixTimeList)
		{
			aniName = AniName;
			aniLoopNum = AniLoopNum;
			isMix = IsMix;
			mixTimeList = MixTimeList;
		}
	}

	public string idleAniDataStr;

	public Func<string, float, float> idleAniChangeFunc;

	public const int AniFrameNum = 30;

	public List<AniData> aniDataList = new List<AniData>();

	public int curAniIndex;

	public int curAniLoopNum;

	public float curAniTime;

	public float curAniHaveRunTime;

	public bool isPlaying;

	public float firstAniNeedMixTime;

	public void InitData(string idleAniStr, Func<string, float, float> changeFunc)
	{
		idleAniDataStr = idleAniStr;
		idleAniChangeFunc = changeFunc;
		curAniIndex = 0;
		curAniLoopNum = 0;
		curAniTime = 0f;
		curAniHaveRunTime = 0f;
		isPlaying = false;
		aniDataList.Clear();
		string[] array = idleAniDataStr.Split(new char[1] { ';' });
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new char[1] { '|' });
			if (array2.Length != 4)
			{
				continue;
			}
			int result = 0;
			int result2 = 0;
			string text = array2[3];
			List<MixTimeData> list = new List<MixTimeData>();
			if (!string.IsNullOrEmpty(text))
			{
				string[] array3 = text.Split(new char[1] { ',' });
				for (int j = 0; j < array3.Length; j++)
				{
					string[] array4 = array3[j].Split(new char[1] { '-' });
					if (array4.Length == 2)
					{
						int result3 = -1;
						int result4 = -1;
						if (int.TryParse(array4[0], out result3) && int.TryParse(array4[1], out result4))
						{
							MixTimeData item = new MixTimeData(result3, result4);
							list.Add(item);
						}
					}
				}
			}
			if (int.TryParse(array2[1], out result) && int.TryParse(array2[2], out result2))
			{
				AniData item2 = new AniData(array2[0], result, result2, list);
				aniDataList.Add(item2);
			}
		}
	}

	public void ClearAllData()
	{
		idleAniDataStr = null;
		idleAniChangeFunc = null;
		aniDataList = null;
	}

	public void OnStart(float FirstAniNeedMixTime = 0f)
	{
		curAniIndex = 0;
		curAniLoopNum = 0;
		curAniTime = 0f;
		curAniHaveRunTime = 0f;
		isPlaying = true;
		firstAniNeedMixTime = FirstAniNeedMixTime;
		AniData curAniData = GetCurAniData();
		if (idleAniChangeFunc != null)
		{
			curAniTime = idleAniChangeFunc(curAniData.aniName, firstAniNeedMixTime);
		}
		firstAniNeedMixTime = 0f;
	}

	public void OnUpdate()
	{
		if (!(curAniTime <= 0f))
		{
			curAniHaveRunTime += Time.deltaTime;
			if (curAniHaveRunTime > curAniTime)
			{
				TryToNextAni();
			}
		}
	}

	public void OnStop()
	{
		curAniIndex = 0;
		curAniLoopNum = 0;
		curAniTime = 0f;
		curAniHaveRunTime = 0f;
		isPlaying = false;
	}

	public AniData GetCurAniData()
	{
		if (curAniIndex < aniDataList.Count)
		{
			return aniDataList[curAniIndex];
		}
		return new AniData("", 0, 0, null);
	}

	public bool GetCurAniNeedMix()
	{
		bool flag = false;
		AniData curAniData = GetCurAniData();
		bool flag2 = false;
		int num = (int)(30f * curAniHaveRunTime);
		if (curAniData.mixTimeList != null)
		{
			for (int i = 0; i < curAniData.mixTimeList.Count; i++)
			{
				MixTimeData mixTimeData = curAniData.mixTimeList[i];
				if (num > mixTimeData.startTime && num < mixTimeData.endTime)
				{
					flag2 = true;
					break;
				}
			}
		}
		if (flag2)
		{
			return curAniData.isMix > 0;
		}
		return curAniData.isMix <= 0;
	}

	public void TryToNextAni()
	{
		AniData curAniData = GetCurAniData();
		curAniTime = 0f;
		curAniHaveRunTime = 0f;
		curAniLoopNum++;
		if (curAniLoopNum >= curAniData.aniLoopNum)
		{
			curAniLoopNum = 0;
			curAniIndex++;
			if (curAniIndex >= aniDataList.Count)
			{
				curAniIndex %= aniDataList.Count;
			}
		}
		AniData curAniData2 = GetCurAniData();
		if (idleAniChangeFunc != null)
		{
			curAniTime = idleAniChangeFunc(curAniData2.aniName, 0f);
		}
	}
}
