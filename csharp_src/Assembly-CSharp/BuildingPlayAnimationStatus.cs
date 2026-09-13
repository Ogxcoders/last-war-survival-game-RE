using GameFramework;

public class BuildingPlayAnimationStatus : StatusStateBase
{
	private WorldBuilding _worldBuilding;

	private float _duration;

	private int _actionCount;

	private float _aniDuration;

	private string _actName = "";

	private const float CROSS_TIME = 0.3f;

	public BuildingPlayAnimationStatus(long startTime, long endTime, WorldBuilding worldBuilding, int statusId)
		: base(statusId, startTime, endTime)
	{
		_worldBuilding = worldBuilding;
	}

	public override void Start()
	{
		timer = 0f;
		long serverTime = GameEntry.Timer.GetServerTime();
		string text = GameEntry.ConfigCache.GetTemplateData("lw_status", statusId, "para1");
		string[] array = text.Split(new char[1] { '|' });
		if (array.Length == 1)
		{
			text = array[0];
		}
		else if (array.Length == 2)
		{
			text = array[0];
			if (!int.TryParse(array[1], out _actionCount))
			{
				_actionCount = 1;
			}
		}
		else if (array.Length == 3)
		{
			text = array[1];
			if (!int.TryParse(array[2], out _actionCount))
			{
				_actionCount = 1;
			}
		}
		else
		{
			Log.Error("LWStatus config error:  param1 : " + text);
		}
		_aniDuration = 0f;
		float num = (float)(serverTime - startTime) / 1000f;
		float num2 = 0f;
		_aniDuration = _worldBuilding.GetAnimationLength(text);
		_actName = text;
		if (num < 0f)
		{
			num = 0f;
		}
		int num3 = (int)(num / _aniDuration);
		if (_actionCount > 1 && num > 0f && num3 > 0)
		{
			if (num3 < _actionCount)
			{
				_actionCount -= num3;
				num -= (float)num3 * _aniDuration;
			}
			else
			{
				_actionCount = 0;
				_aniDuration = 0f;
			}
		}
		if (_aniDuration > 0f && _worldBuilding.CheckPlayAnimation())
		{
			if (endTime > serverTime && num < _aniDuration)
			{
				long num4 = (int)(_aniDuration * 1000f) + startTime + (int)((float)num3 * _aniDuration * 1000f);
				if (serverTime < num4)
				{
					_duration = (float)(num4 - serverTime) / 1000f;
					num2 = num / _aniDuration;
					if (_duration > 0.3f && _actionCount == 1)
					{
						_duration -= 0.3f;
					}
					_worldBuilding.PlayAnimationAndEffectReturnTime(text, num2, num);
					string templateData = GameEntry.ConfigCache.GetTemplateData("lw_status", statusId, "music_sound");
					if (num < 0.3f && !string.IsNullOrEmpty(templateData))
					{
						int result = 0;
						if (int.TryParse(templateData, out result) && result > 0)
						{
							GameEntry.Lua.Call("DataCenter.LWSoundManager:PlaySound", result, param2: false);
						}
					}
				}
			}
			else
			{
				_duration = 0f;
				num2 = 1f;
			}
		}
		else
		{
			_duration = 0f;
			num2 = 1f;
		}
		_actionCount--;
	}

	public override StateType Update(float deltaTime)
	{
		timer += deltaTime;
		if (timer > _duration)
		{
			if (_actionCount > 0)
			{
				_actionCount--;
				if (_actionCount == 0)
				{
					_duration = _aniDuration - 0.3f;
				}
				else
				{
					_duration = _aniDuration;
				}
				timer = 0f;
				_worldBuilding.PlayAnimationAndEffectReturnTime(_actName);
				return StateType.Continue;
			}
			_worldBuilding.PlayAnimationAndEffectReturnTime("idle", 0f, 0f, 0.3f);
			return StateType.Finish;
		}
		return StateType.Continue;
	}

	public override void Dispose()
	{
		base.Dispose();
		_worldBuilding = null;
		statusId = -1;
		_duration = 0f;
	}
}
