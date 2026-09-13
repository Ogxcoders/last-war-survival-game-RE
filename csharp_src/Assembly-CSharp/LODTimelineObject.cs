using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class LODTimelineObject
{
	[HideInInspector]
	public GameObject gameObject;

	public PlayableDirector director;

	public List<LODTimelineTrackObject> tracks = new List<LODTimelineTrackObject>();

	public void UpdateLODLevel(int level)
	{
		TimelineAsset timelineAsset = director.playableAsset as TimelineAsset;
		if (timelineAsset == null)
		{
			Log.Warning("TimelineAsset is null " + gameObject.transform.GetFullPath());
			return;
		}
		int rootTrackCount = timelineAsset.rootTrackCount;
		foreach (LODTimelineTrackObject track in tracks)
		{
			if (track.index >= rootTrackCount)
			{
				Log.Warning($"LODTimelineObject UpdateLODLevel track index {track.index} is out of range {rootTrackCount} {gameObject.transform.GetFullPath()}");
				return;
			}
			track.UpdateLODLevel(timelineAsset, level);
		}
		double time = director.time;
		director.gameObject.SetActive(value: false);
		director.gameObject.SetActive(value: true);
		director.time = time;
		director.Evaluate();
		director.Play();
	}

	public int GetLODLevelValidTrackCount(int level)
	{
		int num = 0;
		foreach (LODTimelineTrackObject track in tracks)
		{
			if (track.IsValidLODLevel(level))
			{
				num++;
			}
		}
		return num;
	}

	public void Refresh()
	{
		if (director == null)
		{
			return;
		}
		TimelineAsset timelineAsset = director.playableAsset as TimelineAsset;
		if (timelineAsset == null)
		{
			return;
		}
		int rootTrackCount = timelineAsset.rootTrackCount;
		for (int i = 0; i < rootTrackCount; i++)
		{
			TrackAsset track = timelineAsset.GetRootTrack(i);
			LODTimelineTrackObject lODTimelineTrackObject = tracks.Find((LODTimelineTrackObject t) => t.track == track);
			if (lODTimelineTrackObject == null)
			{
				lODTimelineTrackObject = new LODTimelineTrackObject();
				lODTimelineTrackObject.track = track;
				lODTimelineTrackObject.isMain = track is PlayableTrack;
				tracks.Add(lODTimelineTrackObject);
			}
			lODTimelineTrackObject.index = i;
		}
		List<TrackAsset> list = new List<TrackAsset>();
		foreach (LODTimelineTrackObject track2 in tracks)
		{
			bool flag = false;
			for (int num = 0; num < rootTrackCount; num++)
			{
				TrackAsset rootTrack = timelineAsset.GetRootTrack(num);
				if (track2.track == rootTrack)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				list.Add(track2.track);
			}
		}
		foreach (TrackAsset trackAsset in list)
		{
			tracks.Remove(tracks.Find((LODTimelineTrackObject t) => t.track == trackAsset));
		}
		list.Clear();
	}
}
