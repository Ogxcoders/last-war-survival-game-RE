using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline;

[Serializable]
[ExcludeFromPreset]
public class TimelineAsset : PlayableAsset, ISerializationCallbackReceiver, ITimelineClipAsset, IPropertyPreview
{
	private enum Versions
	{
		Initial
	}

	private static class TimelineAssetUpgrade
	{
	}

	[Obsolete("MediaType has been deprecated. It is no longer required, and will be removed in a future release.", false)]
	public enum MediaType
	{
		Animation = 0,
		Audio = 1,
		Texture = 2,
		[Obsolete("Use Texture MediaType instead. (UnityUpgradable) -> UnityEngine.Timeline.TimelineAsset/MediaType.Texture", false)]
		Video = 2,
		Script = 3,
		Hybrid = 4,
		Group = 5
	}

	public enum DurationMode
	{
		BasedOnClips,
		FixedLength
	}

	[Serializable]
	public class EditorSettings
	{
		internal static readonly double kMinFrameRate = TimeUtility.kFrameRateEpsilon;

		internal static readonly double kMaxFrameRate = 1000.0;

		internal static readonly double kDefaultFrameRate = 60.0;

		[HideInInspector]
		[SerializeField]
		[FrameRateField]
		private double m_Framerate = kDefaultFrameRate;

		[HideInInspector]
		[SerializeField]
		private bool m_ScenePreview = true;

		[Obsolete("EditorSettings.fps has been deprecated. Use editorSettings.frameRate instead.", false)]
		public float fps
		{
			get
			{
				return (float)m_Framerate;
			}
			set
			{
				m_Framerate = Mathf.Clamp(value, (float)kMinFrameRate, (float)kMaxFrameRate);
			}
		}

		public double frameRate
		{
			get
			{
				return m_Framerate;
			}
			set
			{
				m_Framerate = GetValidFrameRate(value);
			}
		}

		public bool scenePreview
		{
			get
			{
				return m_ScenePreview;
			}
			set
			{
				m_ScenePreview = value;
			}
		}

		public void SetStandardFrameRate(StandardFrameRates enumValue)
		{
			FrameRate frameRate = TimeUtility.ToFrameRate(enumValue);
			if (frameRate.IsValid())
			{
				throw new ArgumentException($"StandardFrameRates {enumValue.ToString()}, is not defined");
			}
			m_Framerate = frameRate.rate;
		}
	}

	private const int k_LatestVersion = 0;

	[SerializeField]
	[HideInInspector]
	private int m_Version;

	[HideInInspector]
	[SerializeField]
	private List<ScriptableObject> m_Tracks;

	[HideInInspector]
	[SerializeField]
	private double m_FixedDuration;

	[NonSerialized]
	[HideInInspector]
	private TrackAsset[] m_CacheOutputTracks;

	[NonSerialized]
	[HideInInspector]
	private List<TrackAsset> m_CacheRootTracks;

	[NonSerialized]
	[HideInInspector]
	private TrackAsset[] m_CacheFlattenedTracks;

	[HideInInspector]
	[SerializeField]
	private EditorSettings m_EditorSettings = new EditorSettings();

	[SerializeField]
	private DurationMode m_DurationMode;

	[HideInInspector]
	[SerializeField]
	private MarkerTrack m_MarkerTrack;

	public EditorSettings editorSettings => m_EditorSettings;

	public override double duration
	{
		get
		{
			if (m_DurationMode == DurationMode.BasedOnClips)
			{
				DiscreteTime discreteTime = CalculateItemsDuration();
				if (discreteTime <= 0)
				{
					return 0.0;
				}
				return (double)discreteTime.OneTickBefore();
			}
			return m_FixedDuration;
		}
	}

	public double fixedDuration
	{
		get
		{
			DiscreteTime discreteTime = (DiscreteTime)m_FixedDuration;
			if (discreteTime <= 0)
			{
				return 0.0;
			}
			return (double)discreteTime.OneTickBefore();
		}
		set
		{
			m_FixedDuration = Math.Max(0.0, value);
		}
	}

	public DurationMode durationMode
	{
		get
		{
			return m_DurationMode;
		}
		set
		{
			m_DurationMode = value;
		}
	}

	public override IEnumerable<PlayableBinding> outputs
	{
		get
		{
			foreach (TrackAsset outputTrack in GetOutputTracks())
			{
				foreach (PlayableBinding output in outputTrack.outputs)
				{
					yield return output;
				}
			}
		}
	}

	public ClipCaps clipCaps
	{
		get
		{
			ClipCaps clipCaps = ClipCaps.All;
			foreach (TrackAsset rootTrack in GetRootTracks())
			{
				TimelineClip[] clips = rootTrack.clips;
				foreach (TimelineClip timelineClip in clips)
				{
					clipCaps &= timelineClip.clipCaps;
				}
			}
			return clipCaps;
		}
	}

	public int outputTrackCount
	{
		get
		{
			UpdateOutputTrackCache();
			return m_CacheOutputTracks.Length;
		}
	}

	public int rootTrackCount
	{
		get
		{
			UpdateRootTrackCache();
			return m_CacheRootTracks.Count;
		}
	}

	internal TrackAsset[] flattenedTracks
	{
		get
		{
			if (m_CacheFlattenedTracks == null)
			{
				List<TrackAsset> allTracks = new List<TrackAsset>(m_Tracks.Count * 2);
				UpdateRootTrackCache();
				allTracks.AddRange(m_CacheRootTracks);
				for (int i = 0; i < m_CacheRootTracks.Count; i++)
				{
					AddSubTracksRecursive(m_CacheRootTracks[i], ref allTracks);
				}
				m_CacheFlattenedTracks = allTracks.ToArray();
			}
			return m_CacheFlattenedTracks;
		}
	}

	public MarkerTrack markerTrack => m_MarkerTrack;

	internal List<ScriptableObject> trackObjects => m_Tracks;

	private void UpgradeToLatestVersion()
	{
	}

	private void OnValidate()
	{
		editorSettings.frameRate = GetValidFrameRate(editorSettings.frameRate);
	}

	public TrackAsset GetRootTrack(int index)
	{
		UpdateRootTrackCache();
		return m_CacheRootTracks[index];
	}

	public IEnumerable<TrackAsset> GetRootTracks()
	{
		UpdateRootTrackCache();
		return m_CacheRootTracks;
	}

	public TrackAsset GetOutputTrack(int index)
	{
		UpdateOutputTrackCache();
		return m_CacheOutputTracks[index];
	}

	public IEnumerable<TrackAsset> GetOutputTracks()
	{
		UpdateOutputTrackCache();
		return m_CacheOutputTracks;
	}

	private static double GetValidFrameRate(double frameRate)
	{
		return Math.Min(Math.Max(frameRate, EditorSettings.kMinFrameRate), EditorSettings.kMaxFrameRate);
	}

	private void UpdateRootTrackCache()
	{
		if (m_CacheRootTracks != null)
		{
			return;
		}
		if (m_Tracks == null)
		{
			m_CacheRootTracks = new List<TrackAsset>();
			return;
		}
		m_CacheRootTracks = new List<TrackAsset>(m_Tracks.Count);
		if (markerTrack != null)
		{
			m_CacheRootTracks.Add(markerTrack);
		}
		foreach (ScriptableObject track in m_Tracks)
		{
			TrackAsset trackAsset = track as TrackAsset;
			if (trackAsset != null)
			{
				m_CacheRootTracks.Add(trackAsset);
			}
		}
	}

	private void UpdateOutputTrackCache()
	{
		if (m_CacheOutputTracks != null)
		{
			return;
		}
		List<TrackAsset> list = new List<TrackAsset>();
		TrackAsset[] array = flattenedTracks;
		foreach (TrackAsset trackAsset in array)
		{
			if (trackAsset != null && trackAsset.GetType() != typeof(GroupTrack) && !trackAsset.isSubTrack)
			{
				list.Add(trackAsset);
			}
		}
		m_CacheOutputTracks = list.ToArray();
	}

	internal void AddTrackInternal(TrackAsset track)
	{
		m_Tracks.Add(track);
		track.parent = this;
		Invalidate();
	}

	internal void RemoveTrack(TrackAsset track)
	{
		m_Tracks.Remove(track);
		Invalidate();
		TrackAsset trackAsset = track.parent as TrackAsset;
		if (trackAsset != null)
		{
			trackAsset.RemoveSubTrack(track);
		}
	}

	public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
	{
		bool autoRebalance = false;
		bool createOutputs = graph.GetPlayableCount() == 0;
		ScriptPlayable<TimelinePlayable> scriptPlayable = TimelinePlayable.Create(graph, GetOutputTracks(), go, autoRebalance, createOutputs);
		scriptPlayable.SetDuration(duration);
		scriptPlayable.SetPropagateSetTime(value: true);
		if (!scriptPlayable.IsValid())
		{
			return Playable.Null;
		}
		return scriptPlayable;
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		m_Version = 0;
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		Invalidate();
		if (m_Version < 0)
		{
			UpgradeToLatestVersion();
		}
	}

	private void __internalAwake()
	{
		if (m_Tracks == null)
		{
			m_Tracks = new List<ScriptableObject>();
		}
		for (int num = m_Tracks.Count - 1; num >= 0; num--)
		{
			TrackAsset trackAsset = m_Tracks[num] as TrackAsset;
			if (trackAsset != null)
			{
				trackAsset.parent = this;
			}
		}
	}

	public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
	{
		foreach (TrackAsset outputTrack in GetOutputTracks())
		{
			if (!outputTrack.mutedInHierarchy)
			{
				outputTrack.GatherProperties(director, driver);
			}
		}
	}

	public void CreateMarkerTrack()
	{
		if (m_MarkerTrack == null)
		{
			m_MarkerTrack = ScriptableObject.CreateInstance<MarkerTrack>();
			TimelineCreateUtilities.SaveAssetIntoObject(m_MarkerTrack, this);
			m_MarkerTrack.parent = this;
			m_MarkerTrack.name = "Markers";
			Invalidate();
		}
	}

	internal void RemoveMarkerTrack()
	{
		if (m_MarkerTrack != null)
		{
			MarkerTrack childAsset = m_MarkerTrack;
			m_MarkerTrack = null;
			TimelineCreateUtilities.RemoveAssetFromObject(childAsset, this);
			Invalidate();
		}
	}

	internal void Invalidate()
	{
		m_CacheRootTracks = null;
		m_CacheOutputTracks = null;
		m_CacheFlattenedTracks = null;
	}

	internal void UpdateFixedDurationWithItemsDuration()
	{
		m_FixedDuration = (double)CalculateItemsDuration();
	}

	private DiscreteTime CalculateItemsDuration()
	{
		DiscreteTime discreteTime = new DiscreteTime(0);
		TrackAsset[] array = flattenedTracks;
		foreach (TrackAsset trackAsset in array)
		{
			if (!trackAsset.muted)
			{
				discreteTime = DiscreteTime.Max(discreteTime, (DiscreteTime)trackAsset.end);
			}
		}
		if (discreteTime <= 0)
		{
			return new DiscreteTime(0);
		}
		return discreteTime;
	}

	private static void AddSubTracksRecursive(TrackAsset track, ref List<TrackAsset> allTracks)
	{
		if (track == null)
		{
			return;
		}
		allTracks.AddRange(track.GetChildTracks());
		foreach (TrackAsset childTrack in track.GetChildTracks())
		{
			AddSubTracksRecursive(childTrack, ref allTracks);
		}
	}

	public TrackAsset CreateTrack(Type type, TrackAsset parent, string name)
	{
		if (parent != null && parent.timelineAsset != this)
		{
			throw new InvalidOperationException("Addtrack cannot parent to a track not in the Timeline");
		}
		if (!typeof(TrackAsset).IsAssignableFrom(type))
		{
			throw new InvalidOperationException("Supplied type must be a track asset");
		}
		if (parent != null && !TimelineCreateUtilities.ValidateParentTrack(parent, type))
		{
			throw new InvalidOperationException("Cannot assign a child of type " + type.Name + " to a parent of type " + parent.GetType().Name);
		}
		string text = name;
		if (string.IsNullOrEmpty(text))
		{
			text = type.Name;
		}
		string text2 = text;
		text2 = ((!(parent != null)) ? TimelineCreateUtilities.GenerateUniqueActorName(trackObjects, text) : TimelineCreateUtilities.GenerateUniqueActorName(parent.subTracksObjects, text));
		return AllocateTrack(parent, text2, type);
	}

	public T CreateTrack<T>(TrackAsset parent, string trackName) where T : TrackAsset, new()
	{
		return (T)CreateTrack(typeof(T), parent, trackName);
	}

	public T CreateTrack<T>(string trackName) where T : TrackAsset, new()
	{
		return (T)CreateTrack(typeof(T), null, trackName);
	}

	public T CreateTrack<T>() where T : TrackAsset, new()
	{
		return (T)CreateTrack(typeof(T), null, null);
	}

	public bool DeleteClip(TimelineClip clip)
	{
		if (clip == null || clip.GetParentTrack() == null)
		{
			return false;
		}
		if (this != clip.GetParentTrack().timelineAsset)
		{
			Debug.LogError("Cannot delete a clip from this timeline");
			return false;
		}
		if (clip.curves != null)
		{
			TimelineUndo.PushDestroyUndo(this, clip.GetParentTrack(), clip.curves);
		}
		if (clip.asset != null)
		{
			DeleteRecordedAnimation(clip);
			TimelineUndo.PushDestroyUndo(this, clip.GetParentTrack(), clip.asset);
		}
		TrackAsset parentTrack = clip.GetParentTrack();
		parentTrack.RemoveClip(clip);
		parentTrack.CalculateExtrapolationTimes();
		return true;
	}

	public bool DeleteTrack(TrackAsset track)
	{
		if (track.timelineAsset != this)
		{
			return false;
		}
		_ = track.parent as TrackAsset != null;
		foreach (TrackAsset childTrack in track.GetChildTracks())
		{
			DeleteTrack(childTrack);
		}
		DeleteRecordedAnimation(track);
		foreach (TimelineClip item in new List<TimelineClip>(track.clips))
		{
			DeleteClip(item);
		}
		RemoveTrack(track);
		TimelineUndo.PushDestroyUndo(this, this, track);
		return true;
	}

	internal void MoveLastTrackBefore(TrackAsset asset)
	{
		if (m_Tracks == null || m_Tracks.Count < 2 || asset == null)
		{
			return;
		}
		ScriptableObject scriptableObject = m_Tracks[m_Tracks.Count - 1];
		if (scriptableObject == asset)
		{
			return;
		}
		for (int i = 0; i < m_Tracks.Count - 1; i++)
		{
			if (m_Tracks[i] == asset)
			{
				for (int num = m_Tracks.Count - 1; num > i; num--)
				{
					m_Tracks[num] = m_Tracks[num - 1];
				}
				m_Tracks[i] = scriptableObject;
				Invalidate();
				break;
			}
		}
	}

	private TrackAsset AllocateTrack(TrackAsset trackAssetParent, string trackName, Type trackType)
	{
		if (trackAssetParent != null && trackAssetParent.timelineAsset != this)
		{
			throw new InvalidOperationException("Addtrack cannot parent to a track not in the Timeline");
		}
		if (!typeof(TrackAsset).IsAssignableFrom(trackType))
		{
			throw new InvalidOperationException("Supplied type must be a track asset");
		}
		TrackAsset trackAsset = (TrackAsset)ScriptableObject.CreateInstance(trackType);
		trackAsset.name = trackName;
		PlayableAsset masterAsset = ((trackAssetParent != null) ? ((PlayableAsset)trackAssetParent) : ((PlayableAsset)this));
		TimelineCreateUtilities.SaveAssetIntoObject(trackAsset, masterAsset);
		if (trackAssetParent != null)
		{
			trackAssetParent.AddChild(trackAsset);
		}
		else
		{
			AddTrackInternal(trackAsset);
		}
		return trackAsset;
	}

	private void DeleteRecordedAnimation(TrackAsset track)
	{
		AnimationTrack animationTrack = track as AnimationTrack;
		if (animationTrack != null && animationTrack.infiniteClip != null)
		{
			TimelineUndo.PushDestroyUndo(this, track, animationTrack.infiniteClip);
		}
		if (track.curves != null)
		{
			TimelineUndo.PushDestroyUndo(this, track, track.curves);
		}
	}

	private void DeleteRecordedAnimation(TimelineClip clip)
	{
		if (clip == null)
		{
			return;
		}
		if (clip.curves != null)
		{
			TimelineUndo.PushDestroyUndo(this, clip.GetParentTrack(), clip.curves);
		}
		if (clip.recordable)
		{
			AnimationPlayableAsset animationPlayableAsset = clip.asset as AnimationPlayableAsset;
			if (!(animationPlayableAsset == null) && !(animationPlayableAsset.clip == null))
			{
				TimelineUndo.PushDestroyUndo(this, animationPlayableAsset, animationPlayableAsset.clip);
			}
		}
	}
}
