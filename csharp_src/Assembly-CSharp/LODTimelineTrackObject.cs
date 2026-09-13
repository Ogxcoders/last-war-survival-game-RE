using System;
using UnityEngine;
using UnityEngine.Timeline;

[Serializable]
public class LODTimelineTrackObject
{
	public TrackAsset track;

	[HideInInspector]
	public Vector2Int range = new Vector2Int(0, 2);

	public int index;

	public bool isMinVisible
	{
		get
		{
			return minLOD <= 0;
		}
		set
		{
			if (value)
			{
				range.x = 0;
			}
			else if (range.x == 0)
			{
				range.x = Math.Min(1, range.y);
				range.y = Math.Max(1, range.y);
			}
		}
	}

	public bool isMidVisible
	{
		get
		{
			if (minLOD <= 1)
			{
				return maxLOD >= 1;
			}
			return false;
		}
		set
		{
			if (value)
			{
				if (range.x > 1)
				{
					range.x = 1;
				}
				if (range.y < 1)
				{
					range.y = 1;
				}
			}
			else if (range.x < 1)
			{
				range.x = 0;
				range.y = 0;
			}
			else if (range.y > 1)
			{
				range.x = 2;
				range.y = 2;
			}
		}
	}

	public bool isMaxVisible
	{
		get
		{
			return maxLOD >= 2;
		}
		set
		{
			if (value)
			{
				range.y = 2;
			}
			else if (range.y == 2)
			{
				range.x = Math.Min(1, range.x);
				range.y = Math.Max(1, range.x);
			}
		}
	}

	public bool isMain
	{
		get
		{
			if (minLOD <= 0)
			{
				return maxLOD >= 999;
			}
			return false;
		}
		set
		{
			if (isMain != value)
			{
				if (value)
				{
					range.x = 0;
					range.y = 999;
				}
				else
				{
					range.x = 0;
					range.y = 2;
				}
			}
		}
	}

	public int minLOD => range.x;

	public int maxLOD => range.y;

	public void UpdateLODLevel(TimelineAsset asset, int level)
	{
		if (track == null)
		{
			track = asset.GetRootTrack(index);
		}
		track.muted = !isMain && (level < minLOD || level > maxLOD);
	}

	public bool IsValidLODLevel(int level)
	{
		if (level >= minLOD)
		{
			return level <= maxLOD;
		}
		return false;
	}
}
