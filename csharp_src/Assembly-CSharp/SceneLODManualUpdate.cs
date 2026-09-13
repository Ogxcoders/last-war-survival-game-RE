using GameKit.Base;
using UnityEngine;

public class SceneLODManualUpdate : ISceneLODNode
{
	private int _currentLOD;

	private readonly float[] _manualUpdatePercent;

	public SceneLODManualUpdate(float[] percents = null)
	{
		_manualUpdatePercent = percents ?? new float[5] { 1f, 0.8f, 0.6f, 0.45f, 0.3f };
	}

	public LODType GetLODType()
	{
		return LODType.CPU;
	}

	public int CurrentLOD()
	{
		return _currentLOD;
	}

	public void UpdateLOD(int level)
	{
		if (_currentLOD != level)
		{
			_currentLOD = level;
			OnLODLevelChanged(level);
		}
	}

	public float GetCost()
	{
		return 0f;
	}

	public float GetDynamicCost()
	{
		return 0f;
	}

	private void OnLODLevelChanged(int level)
	{
		level = Mathf.Clamp(level, 0, _manualUpdatePercent.Length - 1);
		float manualUpdatePercent = _manualUpdatePercent[level];
		SingletonBehaviour<ManualUpdateManager>.Instance.manualUpdatePercent = manualUpdatePercent;
	}
}
