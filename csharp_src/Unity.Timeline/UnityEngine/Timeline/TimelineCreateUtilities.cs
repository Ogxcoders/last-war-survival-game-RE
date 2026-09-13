using System;
using System.Collections.Generic;

namespace UnityEngine.Timeline;

internal static class TimelineCreateUtilities
{
	public static string GenerateUniqueActorName(List<ScriptableObject> tracks, string name)
	{
		if (!tracks.Exists((ScriptableObject x) => (object)x != null && x.name == name))
		{
			return name;
		}
		int result = 0;
		string text = name;
		if (!string.IsNullOrEmpty(name) && name[name.Length - 1] == ')')
		{
			int num = name.LastIndexOf('(');
			if (num > 0 && int.TryParse(name.Substring(num + 1, name.Length - num - 2), out result))
			{
				result++;
				text = name.Substring(0, num);
			}
		}
		text = text.TrimEnd(Array.Empty<char>());
		for (int num2 = result; num2 < result + 5000; num2++)
		{
			if (num2 > 0)
			{
				string result2 = $"{text} ({num2})";
				if (!tracks.Exists((ScriptableObject x) => (object)x != null && x.name == result2))
				{
					return result2;
				}
			}
		}
		return name;
	}

	public static void SaveAssetIntoObject(Object childAsset, Object masterAsset)
	{
		if (!(childAsset == null) && !(masterAsset == null))
		{
			if ((masterAsset.hideFlags & HideFlags.DontSave) != HideFlags.None)
			{
				childAsset.hideFlags |= HideFlags.DontSave;
			}
			else
			{
				childAsset.hideFlags |= HideFlags.HideInHierarchy;
			}
		}
	}

	public static void RemoveAssetFromObject(Object childAsset, Object masterAsset)
	{
		if (!(childAsset == null))
		{
			_ = masterAsset == null;
		}
	}

	public static AnimationClip CreateAnimationClipForTrack(string name, TrackAsset track, bool isLegacy)
	{
		TimelineAsset timelineAsset = ((track != null) ? track.timelineAsset : null);
		HideFlags hideFlags = ((track != null) ? track.hideFlags : HideFlags.None);
		AnimationClip obj = new AnimationClip
		{
			legacy = isLegacy,
			name = name,
			frameRate = ((timelineAsset == null) ? ((float)TimelineAsset.EditorSettings.kDefaultFrameRate) : ((float)timelineAsset.editorSettings.frameRate))
		};
		SaveAssetIntoObject(obj, timelineAsset);
		obj.hideFlags = hideFlags & ~HideFlags.HideInHierarchy;
		return obj;
	}

	public static bool ValidateParentTrack(TrackAsset parent, Type childType)
	{
		if (childType == null || !typeof(TrackAsset).IsAssignableFrom(childType))
		{
			return false;
		}
		if (parent == null)
		{
			return true;
		}
		if (parent is ILayerable && !parent.isSubTrack && parent.GetType() == childType)
		{
			return true;
		}
		if (!(Attribute.GetCustomAttribute(parent.GetType(), typeof(SupportsChildTracksAttribute)) is SupportsChildTracksAttribute supportsChildTracksAttribute))
		{
			return false;
		}
		if (supportsChildTracksAttribute.childType == null)
		{
			return true;
		}
		if (childType == supportsChildTracksAttribute.childType)
		{
			int num = 0;
			TrackAsset trackAsset = parent;
			while (trackAsset != null && trackAsset.isSubTrack)
			{
				num++;
				trackAsset = trackAsset.parent as TrackAsset;
			}
			return num < supportsChildTracksAttribute.levels;
		}
		return false;
	}
}
