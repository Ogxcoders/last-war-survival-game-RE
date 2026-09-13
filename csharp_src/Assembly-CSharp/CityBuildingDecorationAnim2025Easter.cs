using System;
using System.Collections.Generic;
using UnityEngine;

public class CityBuildingDecorationAnim2025Easter : CityBuildingDecorationAnimBase
{
	private string _rawPara;

	private int _statusId;

	private List<string> _anims;

	private List<int> _groupCount;

	private float _totalDuration;

	private int _totalGroupCount;

	private int _startGroupIndex;

	private int _startAnimIndex;

	private int _curPlayingAnimIndex;

	private float _curPlayingAnimDuration;

	public CityBuildingDecorationAnim2025Easter(CityBuilding building, int skinId, string rawData, int statusId)
		: base(building, skinId)
	{
		_rawPara = rawData;
		_statusId = statusId;
	}

	public override void Start()
	{
		_anims = new List<string>();
		_groupCount = new List<int>();
		long startTime = GetStartTime();
		string[] array = _rawPara.Split(new char[1] { '|' });
		_totalGroupCount = array.Length;
		_startGroupIndex = (int)(startTime % _totalGroupCount);
		_startAnimIndex = 0;
		_totalDuration = 0f;
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (i == _startGroupIndex)
			{
				_startAnimIndex = _anims.Count;
			}
			num = 0;
			string[] array2 = array[i].Split(new char[1] { ';' });
			for (int j = 0; j < array2.Length; j++)
			{
				string[] array3 = array2[j].Split(new char[1] { ',' });
				if (array3.Length == 2)
				{
					string text = array3[0];
					int num2 = Convert.ToInt32(array3[1]);
					_totalDuration += GetAnimationLength(text) * (float)num2;
					while (num2 > 0)
					{
						_anims.Add(text);
						num2--;
						num++;
					}
				}
			}
			_groupCount.Add(num);
		}
		timer = 0f;
		float num3 = (float)Math.Max(GameEntry.Timer.GetServerTime() - startTime, 0L) / 1000f % _totalDuration;
		float num4 = 0f;
		float num5 = 0f;
		_curPlayingAnimIndex = -1;
		_curPlayingAnimDuration = 0f;
		for (int k = 0; k < _anims.Count; k++)
		{
			int num6 = (_startAnimIndex + k) % _anims.Count;
			if (num6 < _anims.Count)
			{
				float animationLength = GetAnimationLength(_anims[num6]);
				num5 = num4 + animationLength;
				if (num4 <= num3 && num3 < num5)
				{
					_curPlayingAnimIndex = num6;
					_curPlayingAnimDuration = animationLength - (num3 - num4);
					break;
				}
				num4 = num5;
			}
		}
		if (_curPlayingAnimIndex >= 0)
		{
			if (building != null && building.CheckPlayAnimation())
			{
				building.PlayCrossFadeAnim(_anims[_curPlayingAnimIndex], 0f);
			}
			return;
		}
		int num7 = UnityEngine.Random.Range(0, _totalGroupCount);
		_curPlayingAnimIndex = 0;
		for (int l = 0; l < _groupCount.Count && l != num7; l++)
		{
			_curPlayingAnimIndex += _groupCount[l];
		}
		if (_curPlayingAnimIndex < _anims.Count)
		{
			_curPlayingAnimDuration = GetAnimationLength(_anims[_curPlayingAnimIndex]);
			if (building.CheckPlayAnimation())
			{
				building.PlayCrossFadeAnim(_anims[_curPlayingAnimIndex], 0f);
			}
		}
	}

	public override StateType Update(float deltaTime)
	{
		timer += deltaTime;
		if (_curPlayingAnimDuration > 0f && _anims != null && _anims.Count > 0 && timer > _curPlayingAnimDuration)
		{
			_curPlayingAnimIndex++;
			if (_curPlayingAnimIndex >= _anims.Count)
			{
				_curPlayingAnimIndex = 0;
			}
			_curPlayingAnimDuration = GetAnimationLength(_anims[_curPlayingAnimIndex]);
			building.PlayCrossFadeAnim(_anims[_curPlayingAnimIndex], 0f);
			timer = 0f;
		}
		return StateType.Continue;
	}

	private long GetStartTime()
	{
		long num = 0L;
		long num2 = GameEntry.Lua.CallWithReturn<long, int>("CSharpCallLuaInterface.GetStatusEndTime", _statusId);
		if (num2 > 0)
		{
			int num3 = GameEntry.ConfigCache.GetTemplateData("lw_status", _statusId, "time").ToInt();
			return num2 - num3;
		}
		return GameEntry.Timer.GetServerTime();
	}

	private float GetAnimationLength(string anim)
	{
		float result = 0f;
		if ((bool)building)
		{
			result = building.GetAnimationLength(anim);
		}
		return result;
	}
}
