using UnityEngine;

namespace Spine.Unity;

[CreateAssetMenu(menuName = "Spine/EventData Reference Asset", order = 100)]
public class EventDataReferenceAsset : ScriptableObject
{
	private const bool QuietSkeletonData = true;

	[SerializeField]
	protected SkeletonDataAsset skeletonDataAsset;

	[SerializeField]
	[SpineEvent("", "skeletonDataAsset", true, false, false)]
	protected string eventName;

	private EventData eventData;

	public EventData EventData
	{
		get
		{
			if (eventData == null)
			{
				Initialize();
			}
			return eventData;
		}
	}

	public void Initialize()
	{
		if (!(skeletonDataAsset == null))
		{
			eventData = skeletonDataAsset.GetSkeletonData(quiet: true).FindEvent(eventName);
			if (eventData == null)
			{
				Debug.LogWarningFormat("Event Data '{0}' not found in SkeletonData : {1}.", eventName, skeletonDataAsset.name);
			}
		}
	}

	public static implicit operator EventData(EventDataReferenceAsset asset)
	{
		return asset.EventData;
	}
}
