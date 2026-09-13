using System;
using System.Collections.Generic;
using Protobuf;
using UnityEngine;

public class BuildingPlayAnimationStatus2025Easter : StatusStateBase
{
	private WorldBuilding _worldBuilding;

	private string _rawPara;

	private List<string> _anims;

	private List<int> _groupCount;

	private float _totalDuration;

	private int _totalGroupCount;

	private int _startGroupIndex;

	private int _startAnimIndex;

	private int _curPlayingAnimIndex;

	private float _curPlayingAnimDuration;

	public BuildingPlayAnimationStatus2025Easter(long startTime, long endTime, WorldBuilding worldBuilding, int statusId, string para)
		: base(statusId, startTime, endTime)
	{
		_worldBuilding = worldBuilding;
		_rawPara = para;
	}

	public override void Start()
	{
		RestartAnimationFromStartTime();
	}

	public override void UpdateStatus(Status newData)
	{
		base.UpdateStatus(newData);
		RestartAnimationFromStartTime();
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
			_worldBuilding.PlayAnimationAndEffectReturnTime(_anims[_curPlayingAnimIndex]);
			timer = 0f;
		}
		return StateType.Continue;
	}

	public override void Dispose()
	{
		base.Dispose();
		_worldBuilding = null;
		statusId = -1;
		_curPlayingAnimDuration = 0f;
	}

	private float GetAnimationLength(string anim)
	{
		float result = 0f;
		if ((bool)_worldBuilding)
		{
			result = _worldBuilding.GetAnimationLength(anim);
		}
		return result;
	}

	private void RestartAnimationFromStartTime()
	{
		_worldBuilding.ClearPlayIdleAniManager();
		_anims = new List<string>();
		_groupCount = new List<int>();
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
			float animationLength2 = GetAnimationLength(_anims[_curPlayingAnimIndex]);
			if (_worldBuilding.CheckPlayAnimation())
			{
				_worldBuilding.PlayAnimationAndEffectReturnTime(_anims[_curPlayingAnimIndex], (num3 - num4) / animationLength2, num3 - num4);
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
			if (_worldBuilding.CheckPlayAnimation())
			{
				_worldBuilding.PlayAnimationAndEffectReturnTime(_anims[_curPlayingAnimIndex]);
			}
		}
	}
}
