using System.Collections.Generic;

public class BuildAnimatorManager
{
	public class BuildAnimatorParam
	{
		public long startTime;

		public long endTime;

		public int posIndex;
	}

	private Dictionary<int, BuildAnimatorParam> _buildBuilding;

	public BuildAnimatorManager()
	{
		_buildBuilding = new Dictionary<int, BuildAnimatorParam>();
	}

	public void Shutdown()
	{
		_buildBuilding = new Dictionary<int, BuildAnimatorParam>();
	}

	public void AddOneBuild(int posIndex, long startTime = -1L, long endTime = -1L)
	{
		if (startTime <= 0 && endTime <= 0)
		{
			startTime = GameEntry.Timer.GetServerTime();
			endTime = startTime + 10000;
		}
		if (_buildBuilding.ContainsKey(posIndex))
		{
			_buildBuilding[posIndex].startTime = startTime;
			_buildBuilding[posIndex].endTime = endTime;
			return;
		}
		BuildAnimatorParam value = new BuildAnimatorParam
		{
			startTime = startTime,
			endTime = endTime,
			posIndex = posIndex
		};
		_buildBuilding.Add(posIndex, value);
	}

	public void RemoveOneBuild(int posIndex)
	{
		if (_buildBuilding.ContainsKey(posIndex))
		{
			_buildBuilding.Remove(posIndex);
		}
	}

	public BuildAnimatorParam GetBuildingParam(int posIndex)
	{
		if (_buildBuilding.ContainsKey(posIndex))
		{
			return _buildBuilding[posIndex];
		}
		return null;
	}

	public bool IsBuilding(int posIndex)
	{
		return _buildBuilding.ContainsKey(posIndex);
	}
}
