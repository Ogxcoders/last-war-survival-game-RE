using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline;

internal static class NotificationUtilities
{
	public static ScriptPlayable<TimeNotificationBehaviour> CreateNotificationsPlayable(PlayableGraph graph, IEnumerable<IMarker> markers, PlayableDirector director)
	{
		return CreateNotificationsPlayable(graph, markers, null, director);
	}

	public static ScriptPlayable<TimeNotificationBehaviour> CreateNotificationsPlayable(PlayableGraph graph, IEnumerable<IMarker> markers, TimelineAsset timelineAsset)
	{
		return CreateNotificationsPlayable(graph, markers, timelineAsset, null);
	}

	private static ScriptPlayable<TimeNotificationBehaviour> CreateNotificationsPlayable(PlayableGraph graph, IEnumerable<IMarker> markers, IPlayableAsset asset, PlayableDirector director)
	{
		ScriptPlayable<TimeNotificationBehaviour> result = ScriptPlayable<TimeNotificationBehaviour>.Null;
		DirectorWrapMode loopMode = ((director != null) ? director.extrapolationMode : DirectorWrapMode.None);
		bool flag = false;
		double num = 0.0;
		foreach (IMarker marker in markers)
		{
			if (marker is INotification payload)
			{
				if (!flag)
				{
					num = ((director != null) ? director.playableAsset.duration : asset.duration);
					flag = true;
				}
				if (result.Equals(ScriptPlayable<TimeNotificationBehaviour>.Null))
				{
					result = TimeNotificationBehaviour.Create(graph, num, loopMode);
				}
				DiscreteTime discreteTime = (DiscreteTime)marker.time;
				DiscreteTime discreteTime2 = (DiscreteTime)num;
				if (discreteTime >= discreteTime2 && discreteTime <= discreteTime2.OneTickAfter() && discreteTime2 != 0)
				{
					discreteTime = discreteTime2.OneTickBefore();
				}
				if (marker is INotificationOptionProvider notificationOptionProvider)
				{
					result.GetBehaviour().AddNotification((double)discreteTime, payload, notificationOptionProvider.flags);
				}
				else
				{
					result.GetBehaviour().AddNotification((double)discreteTime, payload);
				}
			}
		}
		return result;
	}

	public static bool TrackTypeSupportsNotifications(Type type)
	{
		TrackBindingTypeAttribute trackBindingTypeAttribute = (TrackBindingTypeAttribute)Attribute.GetCustomAttribute(type, typeof(TrackBindingTypeAttribute));
		if (trackBindingTypeAttribute != null)
		{
			if (!typeof(Component).IsAssignableFrom(trackBindingTypeAttribute.type))
			{
				return typeof(GameObject).IsAssignableFrom(trackBindingTypeAttribute.type);
			}
			return true;
		}
		return false;
	}
}
