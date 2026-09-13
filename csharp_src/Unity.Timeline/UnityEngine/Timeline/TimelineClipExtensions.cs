using System;

namespace UnityEngine.Timeline;

public static class TimelineClipExtensions
{
	private static readonly string k_UndoSetParentTrackText = "Move Clip";

	public static void MoveToTrack(this TimelineClip clip, TrackAsset destinationTrack)
	{
		if (clip == null)
		{
			throw new ArgumentNullException("'this' argument for MoveToTrack cannot be null.");
		}
		if (destinationTrack == null)
		{
			throw new ArgumentNullException("Cannot move TimelineClip to a null track.");
		}
		TrackAsset parentTrack = clip.GetParentTrack();
		Object asset = clip.asset;
		if (asset == null)
		{
			throw new InvalidOperationException("Cannot move a TimelineClip to a different track if the TimelineClip's PlayableAsset is null.");
		}
		if (parentTrack == destinationTrack)
		{
			throw new InvalidOperationException("TimelineClip is already on " + destinationTrack.name + ".");
		}
		if (!destinationTrack.ValidateClipType(asset.GetType()))
		{
			throw new InvalidOperationException("Track " + destinationTrack.name + " cannot contain clips of type " + clip.GetType().Name + ".");
		}
		MoveToTrack_Impl(clip, destinationTrack, asset, parentTrack);
	}

	public static bool TryMoveToTrack(this TimelineClip clip, TrackAsset destinationTrack)
	{
		if (clip == null)
		{
			throw new ArgumentNullException("'this' argument for TryMoveToTrack cannot be null.");
		}
		if (destinationTrack == null)
		{
			throw new ArgumentNullException("Cannot move TimelineClip to a null parent.");
		}
		TrackAsset parentTrack = clip.GetParentTrack();
		Object asset = clip.asset;
		if (asset == null)
		{
			return false;
		}
		if (parentTrack != destinationTrack)
		{
			if (!destinationTrack.ValidateClipType(asset.GetType()))
			{
				return false;
			}
			MoveToTrack_Impl(clip, destinationTrack, asset, parentTrack);
			return true;
		}
		return false;
	}

	private static void MoveToTrack_Impl(TimelineClip clip, TrackAsset destinationTrack, Object asset, TrackAsset parentTrack)
	{
		_ = parentTrack != null;
		clip.SetParentTrack_Internal(destinationTrack);
		if (parentTrack == null)
		{
			TimelineCreateUtilities.SaveAssetIntoObject(asset, destinationTrack);
		}
		else if (parentTrack.timelineAsset != destinationTrack.timelineAsset)
		{
			TimelineCreateUtilities.RemoveAssetFromObject(asset, parentTrack);
			TimelineCreateUtilities.SaveAssetIntoObject(asset, destinationTrack);
		}
	}
}
