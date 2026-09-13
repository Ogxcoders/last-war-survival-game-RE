using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FibMatrix;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class SimpleAnimationPlayable : PlayableBehaviour, IDisposable
{
	private class StateEnumerable : IEnumerable<IState>, IEnumerable
	{
		private class StateEnumerator : IEnumerator<IState>, IEnumerator, IDisposable
		{
			private int m_Index = -1;

			private int m_Version;

			private SimpleAnimationPlayable m_Owner;

			object IEnumerator.Current => GetCurrentHandle(m_Index);

			IState IEnumerator<IState>.Current => GetCurrentHandle(m_Index);

			public StateEnumerator(SimpleAnimationPlayable owner)
			{
				m_Owner = owner;
				m_Version = m_Owner.m_StatesVersion;
				Reset();
			}

			private bool IsValid()
			{
				if (m_Owner != null)
				{
					return m_Version == m_Owner.m_StatesVersion;
				}
				return false;
			}

			private IState GetCurrentHandle(int index)
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("The collection has been modified, this Enumerator is invalid");
				}
				if (index < 0 || index >= m_Owner.m_States.Count)
				{
					throw new InvalidOperationException("Enumerator is invalid");
				}
				StateInfo stateInfo = m_Owner.m_States[index];
				if (stateInfo == null)
				{
					throw new InvalidOperationException("Enumerator is invalid");
				}
				return new StateHandle(m_Owner, stateInfo.index, stateInfo.playable);
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("The collection has been modified, this Enumerator is invalid");
				}
				do
				{
					m_Index++;
				}
				while (m_Index < m_Owner.m_States.Count && m_Owner.m_States[m_Index] == null);
				return m_Index < m_Owner.m_States.Count;
			}

			public void Reset()
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("The collection has been modified, this Enumerator is invalid");
				}
				m_Index = -1;
			}
		}

		private SimpleAnimationPlayable m_Owner;

		public StateEnumerable(SimpleAnimationPlayable owner)
		{
			m_Owner = owner;
		}

		public IEnumerator<IState> GetEnumerator()
		{
			return new StateEnumerator(m_Owner);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new StateEnumerator(m_Owner);
		}
	}

	public interface IState
	{
		bool enabled { get; set; }

		float time { get; set; }

		float normalizedTime { get; set; }

		float speed { get; set; }

		string name { get; set; }

		float weight { get; set; }

		float length { get; }

		AnimationClip clip { get; }

		WrapMode wrapMode { get; }

		bool IsValid();
	}

	public class StateHandle : IState
	{
		private SimpleAnimationPlayable m_Parent;

		private int m_Index;

		private Playable m_Target;

		public bool enabled
		{
			get
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return m_Parent.m_States[m_Index].enabled;
			}
			set
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				if (value)
				{
					m_Parent.m_States.EnableState(m_Index);
				}
				else
				{
					m_Parent.m_States.DisableState(m_Index);
				}
			}
		}

		public float time
		{
			get
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return m_Parent.m_States.GetStateTime(m_Index);
			}
			set
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				m_Parent.m_States.SetStateTime(m_Index, value);
			}
		}

		public float normalizedTime
		{
			get
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				float num = m_Parent.m_States.GetClipLength(m_Index);
				if (num == 0f)
				{
					num = 1f;
				}
				return m_Parent.m_States.GetStateTime(m_Index) / num;
			}
			set
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				float num = m_Parent.m_States.GetClipLength(m_Index);
				if (num == 0f)
				{
					num = 1f;
				}
				m_Parent.m_States.SetStateTime(m_Index, value *= num);
			}
		}

		public float speed
		{
			get
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return m_Parent.m_States.GetStateSpeed(m_Index);
			}
			set
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				m_Parent.m_States.SetStateSpeed(m_Index, value);
			}
		}

		public string name
		{
			get
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return m_Parent.m_States.GetStateName(m_Index);
			}
			set
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				if (value == null)
				{
					throw new ArgumentNullException("A null string is not a valid name");
				}
				m_Parent.m_States.SetStateName(m_Index, value);
			}
		}

		public float weight
		{
			get
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return m_Parent.m_States[m_Index].weight;
			}
			set
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				if (value < 0f)
				{
					throw new ArgumentException("Weights cannot be negative");
				}
				m_Parent.m_States.SetInputWeight(m_Index, value);
			}
		}

		public float length
		{
			get
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return m_Parent.m_States.GetStateLength(m_Index);
			}
		}

		public AnimationClip clip
		{
			get
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return m_Parent.m_States.GetStateClip(m_Index);
			}
		}

		public WrapMode wrapMode
		{
			get
			{
				if (!IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return m_Parent.m_States.GetStateWrapMode(m_Index);
			}
		}

		public int index => m_Index;

		public StateHandle(SimpleAnimationPlayable s, int index, Playable target)
		{
			m_Parent = s;
			m_Index = index;
			m_Target = target;
		}

		public bool IsValid()
		{
			return m_Parent.ValidateInput(m_Index, m_Target);
		}
	}

	private class StateInfo : IDisposable
	{
		private bool m_Enabled;

		private int m_Index;

		private string m_StateName;

		private bool m_Fading;

		private float m_Time;

		private float m_TargetWeight;

		private float m_Weight;

		private float m_FadeSpeed;

		private AnimationClip m_Clip;

		private Playable m_Playable;

		private WrapMode m_WrapMode;

		private bool m_IsClone;

		private bool m_ReadyForCleanup;

		public StateHandle m_ParentState;

		private bool m_WeightDirty;

		private bool m_EnabledDirty;

		private bool m_TimeIsUpToDate;

		public bool enabled => m_Enabled;

		public int index
		{
			get
			{
				return m_Index;
			}
			set
			{
				m_Index = value;
			}
		}

		public string stateName
		{
			get
			{
				return m_StateName;
			}
			set
			{
				m_StateName = value;
			}
		}

		public string origStateName { get; private set; }

		public bool fading => m_Fading;

		public float crossFadeStartTimeSinceLevelLoad { get; set; }

		public float targetWeight => m_TargetWeight;

		public float weight => m_Weight;

		public float fadeSpeed => m_FadeSpeed;

		public float speed
		{
			get
			{
				return (float)m_Playable.GetSpeed();
			}
			set
			{
				m_Playable.SetSpeed(value);
			}
		}

		public float playableDuration => (float)m_Playable.GetDuration();

		public AnimationClip clip => m_Clip;

		public bool isDone => m_Playable.IsDone();

		public Playable playable => m_Playable;

		public WrapMode wrapMode => m_WrapMode;

		public bool isClone => m_IsClone;

		public bool isReadyForCleanup => m_ReadyForCleanup;

		public StateHandle parentState => m_ParentState;

		public bool enabledDirty => m_EnabledDirty;

		public bool weightDirty => m_WeightDirty;

		public void Initialize(string name, AnimationClip clip, WrapMode wrapMode, string origName)
		{
			m_StateName = name;
			m_Clip = clip;
			m_WrapMode = wrapMode;
			origStateName = ((origName != null) ? origName : name);
		}

		public float GetTime()
		{
			if (m_TimeIsUpToDate)
			{
				return m_Time;
			}
			m_Time = (float)m_Playable.GetTime();
			m_TimeIsUpToDate = true;
			return m_Time;
		}

		public void SetTime(float newTime)
		{
			m_Time = newTime;
			m_Playable.ResetTime(m_Time);
			m_Playable.SetDone((double)m_Time >= m_Playable.GetDuration());
		}

		public void Enable()
		{
			if (!m_Enabled)
			{
				m_EnabledDirty = true;
				m_Enabled = true;
			}
		}

		public void Disable()
		{
			if (m_Enabled)
			{
				m_EnabledDirty = true;
				m_Enabled = false;
			}
		}

		public void Pause()
		{
			m_Playable.SetPlayState(PlayState.Paused);
		}

		public void Play()
		{
			m_Playable.SetPlayState(PlayState.Playing);
		}

		public void Stop()
		{
			m_FadeSpeed = 0f;
			ForceWeight(0f);
			Disable();
			SetTime(0f);
			m_Playable.SetDone(value: false);
			if (isClone)
			{
				m_ReadyForCleanup = true;
			}
		}

		public void ForceWeight(float weight)
		{
			m_TargetWeight = weight;
			m_Fading = false;
			m_FadeSpeed = 0f;
			SetWeight(weight);
		}

		public void SetWeight(float weight)
		{
			m_Weight = weight;
			m_WeightDirty = true;
		}

		public void FadeTo(float weight, float speed)
		{
			m_Fading = Mathf.Abs(speed) > 0f;
			m_FadeSpeed = speed;
			m_TargetWeight = weight;
		}

		public void DestroyPlayable()
		{
			if (m_Playable.IsValid())
			{
				m_Playable.GetGraph().DestroySubgraph(m_Playable);
			}
		}

		public void SetAsCloneOf(StateHandle handle)
		{
			m_ParentState = handle;
			m_IsClone = true;
		}

		public void SetPlayable(Playable playable)
		{
			m_Playable = playable;
		}

		public void ResetDirtyFlags()
		{
			m_EnabledDirty = false;
			m_WeightDirty = false;
		}

		public void InvalidateTime()
		{
			m_TimeIsUpToDate = false;
		}

		public void Dispose()
		{
			DestroyPlayable();
			m_StateName = null;
			m_Clip = null;
			m_WrapMode = WrapMode.Default;
			origStateName = null;
			m_TimeIsUpToDate = false;
			m_EnabledDirty = false;
			m_WeightDirty = false;
			m_ParentState = null;
			m_ReadyForCleanup = false;
			m_IsClone = false;
			m_Playable = Playable.Null;
			m_FadeSpeed = 0f;
			m_Weight = 0f;
			m_TargetWeight = 0f;
			m_Time = 0f;
			m_Fading = false;
			m_Index = 0;
			m_Enabled = false;
		}
	}

	private class StateManagement : IDisposable
	{
		private List<StateInfo> m_States;

		private int m_Count;

		private static ObjectPool<StateInfo> msStateInfoPool = new ObjectPool<StateInfo>(10);

		public int Count => m_Count;

		public StateInfo this[int i] => m_States[i];

		public StateManagement()
		{
			m_States = new List<StateInfo>();
		}

		public StateInfo InsertState()
		{
			StateInfo stateInfo = msStateInfoPool.Allocate();
			int num = m_States.FindIndex((StateInfo s) => s == null);
			if (num == -1)
			{
				num = m_States.Count;
				m_States.Add(stateInfo);
			}
			else
			{
				m_States[num] = stateInfo;
			}
			stateInfo.index = num;
			m_Count = m_States.Count;
			return stateInfo;
		}

		public bool AnyStatePlaying()
		{
			return m_States.FindIndex((StateInfo s) => s?.enabled ?? false) != -1;
		}

		public void RemoveState(int index)
		{
			StateInfo t = m_States[index];
			m_States[index] = null;
			msStateInfoPool.Recycle(t);
			m_Count = m_States.Count;
		}

		public bool RemoveClip(AnimationClip clip)
		{
			bool result = false;
			for (int i = 0; i < m_States.Count; i++)
			{
				StateInfo stateInfo = m_States[i];
				if (stateInfo != null && stateInfo.clip == clip)
				{
					RemoveState(i);
					result = true;
				}
			}
			return result;
		}

		public StateInfo FindState(string name)
		{
			for (int i = 0; i < m_States.Count; i++)
			{
				if (m_States[i] != null && m_States[i].stateName == name)
				{
					return m_States[i];
				}
			}
			return null;
		}

		public void EnableState(int index)
		{
			m_States[index].Enable();
		}

		public void DisableState(int index)
		{
			m_States[index].Disable();
		}

		public void SetInputWeight(int index, float weight)
		{
			m_States[index].SetWeight(weight);
		}

		public void SetStateTime(int index, float time)
		{
			m_States[index].SetTime(time);
		}

		public float GetStateTime(int index)
		{
			return m_States[index].GetTime();
		}

		public bool IsCloneOf(int potentialCloneIndex, int originalIndex)
		{
			StateInfo stateInfo = m_States[potentialCloneIndex];
			if (stateInfo.isClone)
			{
				return stateInfo.parentState.index == originalIndex;
			}
			return false;
		}

		public float GetStateSpeed(int index)
		{
			return m_States[index].speed;
		}

		public void SetStateSpeed(int index, float value)
		{
			m_States[index].speed = value;
		}

		public float GetInputWeight(int index)
		{
			return m_States[index].weight;
		}

		public float GetStateLength(int index)
		{
			AnimationClip clip = m_States[index].clip;
			if (clip == null)
			{
				return 0f;
			}
			float speed = m_States[index].speed;
			if (speed == 0f)
			{
				return float.PositiveInfinity;
			}
			return clip.length / speed;
		}

		public float GetClipLength(int index)
		{
			AnimationClip clip = m_States[index].clip;
			if (clip == null)
			{
				return 0f;
			}
			return clip.length;
		}

		public float GetStatePlayableDuration(int index)
		{
			return m_States[index].playableDuration;
		}

		public AnimationClip GetStateClip(int index)
		{
			return m_States[index].clip;
		}

		public WrapMode GetStateWrapMode(int index)
		{
			return m_States[index].wrapMode;
		}

		public string GetStateName(int index)
		{
			return m_States[index].stateName;
		}

		public void SetStateName(int index, string name)
		{
			m_States[index].stateName = name;
		}

		public void StopState(int index, bool cleanup)
		{
			if (cleanup)
			{
				RemoveState(index);
			}
			else
			{
				m_States[index].Stop();
			}
		}

		public void Dispose()
		{
			for (int i = 0; i < m_States.Count; i++)
			{
				StateInfo stateInfo = m_States[i];
				if (stateInfo != null)
				{
					msStateInfoPool.Recycle(stateInfo);
				}
			}
			msStateInfoPool.Release(100);
			m_States.Clear();
			m_Count = 0;
		}
	}

	private struct QueuedState
	{
		public StateHandle state;

		public float fadeTime;

		public QueuedState(StateHandle s, float t)
		{
			state = s;
			fadeTime = t;
		}
	}

	private LinkedList<QueuedState> m_StateQueue;

	private StateManagement m_States;

	private bool m_Initialized;

	private bool m_KeepStoppedPlayablesConnected = true;

	private float lastCrossFadeTime;

	public const int InvalidAnimationIndex = -1;

	private int m_LastAnimCmdFrameCount = -1;

	private int m_LastAnimCrossDelay;

	protected Playable m_ActualPlayable;

	private AnimationMixerPlayable m_Mixer;

	public Action onDone;

	public Action onStateDirtyCallback;

	private int m_StatesVersion;

	public bool keepStoppedPlayablesConnected
	{
		get
		{
			return m_KeepStoppedPlayablesConnected;
		}
		set
		{
			if (value != m_KeepStoppedPlayablesConnected)
			{
				m_KeepStoppedPlayablesConnected = value;
			}
		}
	}

	public float lastCrossFadeWeight { get; private set; }

	public int interruptingAnimationIndex { get; private set; }

	public float interruptingAnimationTime { get; private set; }

	public int lastAnimationIndex { get; private set; }

	public float lastAnimationTime { get; private set; }

	public int currAnimationIndex { get; private set; }

	public bool IsStateDirtyByAnimCmd => m_LastAnimCmdFrameCount == Time.frameCount;

	public bool StateDirtyCrossDelay => m_LastAnimCmdFrameCount + m_LastAnimCrossDelay > Time.frameCount;

	protected Playable self => m_ActualPlayable;

	public Playable playable => self;

	protected PlayableGraph graph => self.GetGraph();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public (string origStateName, float normalizedTime, float nonNormalizedTime) GetCurrentState()
	{
		if (currAnimationIndex != -1)
		{
			StateInfo stateInfo = m_States[currAnimationIndex];
			return (origStateName: stateInfo.origStateName, normalizedTime: stateInfo.GetTime() / stateInfo.clip.length, nonNormalizedTime: Time.timeSinceLevelLoad - stateInfo.crossFadeStartTimeSinceLevelLoad);
		}
		return default((string, float, float));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public (string origStateName, float normalizedTime, float nonNormalizedTime) GetLastState()
	{
		if (lastAnimationIndex != -1)
		{
			StateInfo stateInfo = m_States[lastAnimationIndex];
			return (origStateName: stateInfo.origStateName, normalizedTime: stateInfo.GetTime() / stateInfo.clip.length, nonNormalizedTime: Time.timeSinceLevelLoad - stateInfo.crossFadeStartTimeSinceLevelLoad);
		}
		return default((string, float, float));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public (string interruptStateName, float interruptTime, float interruptWeight, string lastStateName, float lastTime) GetInterruptingState()
	{
		if (interruptingAnimationIndex != -1 && lastAnimationIndex != -1)
		{
			StateInfo stateInfo = m_States[lastAnimationIndex];
			return (interruptStateName: m_States[interruptingAnimationIndex].origStateName, interruptTime: interruptingAnimationTime, interruptWeight: lastCrossFadeWeight, lastStateName: stateInfo.origStateName, lastTime: lastAnimationTime);
		}
		return default((string, float, float, string, float));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsCrossFading()
	{
		return lastAnimationIndex != -1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool InterruptingCrossFading()
	{
		return interruptingAnimationIndex != -1;
	}

	private void UpdateStoppedPlayablesConnections()
	{
		for (int i = 0; i < m_States.Count; i++)
		{
			StateInfo stateInfo = m_States[i];
			if (stateInfo != null && !stateInfo.enabled)
			{
				if (keepStoppedPlayablesConnected)
				{
					ConnectInput(stateInfo.index);
				}
				else
				{
					DisconnectInput(stateInfo.index);
				}
			}
		}
	}

	public SimpleAnimationPlayable()
	{
		m_States = new StateManagement();
		m_StateQueue = new LinkedList<QueuedState>();
		currAnimationIndex = -1;
		lastAnimationIndex = -1;
		lastCrossFadeTime = 0f;
		lastCrossFadeWeight = 0f;
		interruptingAnimationIndex = -1;
		interruptingAnimationTime = 0f;
		lastAnimationTime = 0f;
	}

	public Playable GetInput(int index)
	{
		if (index >= m_Mixer.GetInputCount())
		{
			return Playable.Null;
		}
		return m_Mixer.GetInput(index);
	}

	public override void OnPlayableCreate(Playable playable)
	{
		m_ActualPlayable = playable;
		AnimationMixerPlayable mixer = AnimationMixerPlayable.Create(graph, 1, normalizeWeights: true);
		m_Mixer = mixer;
		self.SetInputCount(1);
		self.SetInputWeight(0, 1f);
		graph.Connect(m_Mixer, 0, self, 0);
	}

	public IEnumerable<IState> GetStates()
	{
		return new StateEnumerable(this);
	}

	public IState GetState(string name)
	{
		StateInfo stateInfo = m_States.FindState(name);
		if (stateInfo == null)
		{
			return null;
		}
		return new StateHandle(this, stateInfo.index, stateInfo.playable);
	}

	private StateInfo DoAddClip(string name, AnimationClip clip, string origName = null)
	{
		StateInfo stateInfo = m_States.InsertState();
		stateInfo.Initialize(name, clip, clip.wrapMode, origName);
		int index = stateInfo.index;
		if (index == m_Mixer.GetInputCount())
		{
			m_Mixer.SetInputCount(index + 1);
		}
		AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(graph, clip);
		animationClipPlayable.SetApplyFootIK(value: false);
		animationClipPlayable.SetApplyPlayableIK(value: false);
		if (!clip.isLooping || stateInfo.wrapMode == WrapMode.Once)
		{
			animationClipPlayable.SetDuration(clip.length);
		}
		stateInfo.SetPlayable(animationClipPlayable);
		stateInfo.Pause();
		if (keepStoppedPlayablesConnected)
		{
			ConnectInput(stateInfo.index);
		}
		return stateInfo;
	}

	public bool AddClip(AnimationClip clip, string name)
	{
		if (m_States.FindState(name) != null)
		{
			Debug.LogError($"Cannot add state with name {name}, because a state with that name already exists");
			return false;
		}
		DoAddClip(name, clip);
		UpdateDoneStatus();
		InvalidateStates();
		return true;
	}

	public bool RemoveClip(string name)
	{
		StateInfo stateInfo = m_States.FindState(name);
		if (stateInfo == null)
		{
			Debug.LogError($"Cannot remove state with name {name}, because a state with that name doesn't exist");
			return false;
		}
		RemoveClones(stateInfo);
		InvalidateStates();
		m_States.RemoveState(stateInfo.index);
		return true;
	}

	public bool RemoveClip(AnimationClip clip)
	{
		InvalidateStates();
		return m_States.RemoveClip(clip);
	}

	public bool Play(string name)
	{
		StateInfo stateInfo = m_States.FindState(name);
		if (stateInfo == null)
		{
			Debug.LogError($"Cannot play state with name {name} because there is no state with that name");
			return false;
		}
		return Play(stateInfo.index);
	}

	private bool Play(int index)
	{
		for (int i = 0; i < m_States.Count; i++)
		{
			StateInfo stateInfo = m_States[i];
			if (stateInfo != null && stateInfo.index == index)
			{
				stateInfo.Enable();
				stateInfo.ForceWeight(1f);
				stateInfo.crossFadeStartTimeSinceLevelLoad = 0f;
			}
			else
			{
				DoStop(i);
			}
		}
		lastAnimationIndex = -1;
		lastCrossFadeTime = 0f;
		lastCrossFadeWeight = 0f;
		interruptingAnimationIndex = -1;
		interruptingAnimationTime = 0f;
		lastAnimationTime = 0f;
		currAnimationIndex = index;
		m_LastAnimCmdFrameCount = Time.frameCount;
		m_LastAnimCrossDelay = 0;
		onStateDirtyCallback?.Invoke();
		return true;
	}

	public bool PlayQueued(string name, QueueMode queueMode)
	{
		StateInfo stateInfo = m_States.FindState(name);
		if (stateInfo == null)
		{
			Debug.LogError($"Cannot queue Play to state with name {name} because there is no state with that name");
			return false;
		}
		return PlayQueued(stateInfo.index, queueMode);
	}

	private bool PlayQueued(int index, QueueMode queueMode)
	{
		StateInfo stateInfo = CloneState(index);
		if (queueMode == QueueMode.PlayNow)
		{
			Play(stateInfo.index);
			return true;
		}
		m_StateQueue.AddLast(new QueuedState(StateInfoToHandle(stateInfo), 0f));
		return true;
	}

	public void Rewind(string name)
	{
		StateInfo stateInfo = m_States.FindState(name);
		if (stateInfo == null)
		{
			Debug.LogError($"Cannot Rewind state with name {name} because there is no state with that name");
		}
		else
		{
			Rewind(stateInfo.index);
		}
	}

	private void Rewind(int index)
	{
		m_States.SetStateTime(index, 0f);
		m_LastAnimCmdFrameCount = Time.frameCount;
		m_States[index].crossFadeStartTimeSinceLevelLoad = 0f;
		m_LastAnimCrossDelay = 0;
		onStateDirtyCallback?.Invoke();
	}

	public void Rewind()
	{
		for (int i = 0; i < m_States.Count; i++)
		{
			if (m_States[i] != null)
			{
				m_States.SetStateTime(i, 0f);
				m_States[i].crossFadeStartTimeSinceLevelLoad = 0f;
			}
		}
		m_LastAnimCmdFrameCount = Time.frameCount;
		m_LastAnimCrossDelay = 0;
		onStateDirtyCallback?.Invoke();
	}

	private void RemoveClones(StateInfo state)
	{
		LinkedListNode<QueuedState> linkedListNode = m_StateQueue.First;
		while (linkedListNode != null)
		{
			LinkedListNode<QueuedState> next = linkedListNode.Next;
			StateInfo stateInfo = m_States[linkedListNode.Value.state.index];
			if (stateInfo != null && stateInfo.parentState.index == state.index)
			{
				m_StateQueue.Remove(linkedListNode);
				DoStop(stateInfo.index);
			}
			linkedListNode = next;
		}
	}

	public bool Stop(string name)
	{
		StateInfo stateInfo = m_States.FindState(name);
		if (stateInfo == null)
		{
			Debug.LogError($"Cannot stop state with name {name} because there is no state with that name");
			return false;
		}
		DoStop(stateInfo.index);
		UpdateDoneStatus();
		return true;
	}

	private void DoStop(int index)
	{
		if (lastAnimationIndex == index)
		{
			lastAnimationIndex = -1;
			lastCrossFadeTime = 0f;
		}
		if (interruptingAnimationIndex == index)
		{
			interruptingAnimationIndex = -1;
			lastCrossFadeTime = 0f;
			lastCrossFadeWeight = 0f;
			interruptingAnimationTime = 0f;
			lastAnimationTime = 0f;
		}
		StateInfo stateInfo = m_States[index];
		if (stateInfo != null)
		{
			m_States.StopState(index, stateInfo.isClone);
			stateInfo.crossFadeStartTimeSinceLevelLoad = 0f;
			if (!stateInfo.isClone)
			{
				RemoveClones(stateInfo);
			}
			m_LastAnimCmdFrameCount = Time.frameCount;
			m_LastAnimCrossDelay = 0;
			onStateDirtyCallback?.Invoke();
		}
	}

	public bool StopAll()
	{
		for (int i = 0; i < m_States.Count; i++)
		{
			DoStop(i);
		}
		playable.SetDone(value: true);
		return true;
	}

	public bool IsPlaying()
	{
		return m_States.AnyStatePlaying();
	}

	public bool IsPlaying(string stateName)
	{
		StateInfo stateInfo = m_States.FindState(stateName);
		if (stateInfo == null)
		{
			return false;
		}
		if (!stateInfo.enabled)
		{
			return IsClonePlaying(stateInfo);
		}
		return true;
	}

	private bool IsClonePlaying(StateInfo state)
	{
		for (int i = 0; i < m_States.Count; i++)
		{
			StateInfo stateInfo = m_States[i];
			if (stateInfo != null && stateInfo.isClone && stateInfo.enabled && stateInfo.parentState.index == state.index)
			{
				return true;
			}
		}
		return false;
	}

	public int GetClipCount()
	{
		int num = 0;
		for (int i = 0; i < m_States.Count; i++)
		{
			if (m_States[i] != null)
			{
				num++;
			}
		}
		return num;
	}

	private void SetupLerp(StateInfo state, float targetWeight, float time)
	{
		float num = Mathf.Abs(state.weight - targetWeight);
		float num2 = ((time != 0f) ? (num / time) : float.PositiveInfinity);
		if (!state.fading || !Mathf.Approximately(state.targetWeight, targetWeight) || !(num2 < state.fadeSpeed))
		{
			state.FadeTo(targetWeight, num2);
		}
	}

	private float NeedBreakCrossFading(int index)
	{
		if (lastAnimationIndex != -1 && currAnimationIndex != index)
		{
			StateInfo stateInfo = m_States[lastAnimationIndex];
			if (stateInfo == null)
			{
				return -1f;
			}
			float num = Time.timeSinceLevelLoad - stateInfo.crossFadeStartTimeSinceLevelLoad;
			if (num > 0f && num < 0.2f)
			{
				return num;
			}
		}
		return -1f;
	}

	private bool Crossfade(int index, float time)
	{
		lastCrossFadeWeight = 0f;
		interruptingAnimationIndex = -1;
		interruptingAnimationTime = 0f;
		lastAnimationTime = 0f;
		float num = NeedBreakCrossFading(index);
		if (num > 0f)
		{
			interruptingAnimationIndex = lastAnimationIndex;
			interruptingAnimationTime = Time.time - num;
			lastCrossFadeWeight = num / 0.2f;
		}
		lastAnimationIndex = -1;
		for (int i = 0; i < m_States.Count; i++)
		{
			StateInfo stateInfo = m_States[i];
			if (stateInfo != null)
			{
				if (stateInfo.index == index)
				{
					m_States.EnableState(index);
					stateInfo.crossFadeStartTimeSinceLevelLoad = Time.timeSinceLevelLoad;
				}
				if (stateInfo.enabled)
				{
					float targetWeight = ((stateInfo.index == index) ? 1f : 0f);
					SetupLerp(stateInfo, targetWeight, time);
				}
			}
		}
		if (currAnimationIndex == index)
		{
			return false;
		}
		if (!StateDirtyCrossDelay)
		{
			if (currAnimationIndex >= 0)
			{
				StateInfo stateInfo2 = m_States[currAnimationIndex];
				lastAnimationIndex = ((stateInfo2 != null && stateInfo2.enabled) ? currAnimationIndex : (-1));
				if (num > 0f)
				{
					float num2 = stateInfo2.GetTime() / stateInfo2.clip.length;
					float num3 = Time.timeSinceLevelLoad - (num2 - Mathf.Floor(num2)) * stateInfo2.clip.length;
					lastAnimationTime = Time.time - num3;
				}
				lastCrossFadeTime = time;
			}
			currAnimationIndex = index;
			m_LastAnimCmdFrameCount = Time.frameCount;
			m_LastAnimCrossDelay = 3;
			onStateDirtyCallback?.Invoke();
		}
		return true;
	}

	private StateInfo CloneState(int index)
	{
		StateInfo stateInfo = m_States[index];
		string name = stateInfo.stateName + "Queued Clone";
		StateInfo stateInfo2 = DoAddClip(name, stateInfo.clip, stateInfo.stateName);
		stateInfo2.SetAsCloneOf(new StateHandle(this, stateInfo.index, stateInfo.playable));
		return stateInfo2;
	}

	public bool Crossfade(string name, float time)
	{
		StateInfo stateInfo = m_States.FindState(name);
		if (stateInfo == null)
		{
			Debug.LogError($"Cannot crossfade to state with name {name} because there is no state with that name");
			return false;
		}
		if (time == 0f)
		{
			return Play(stateInfo.index);
		}
		return Crossfade(stateInfo.index, time);
	}

	public bool CrossfadeQueued(string name, float time, QueueMode queueMode)
	{
		StateInfo stateInfo = m_States.FindState(name);
		if (stateInfo == null)
		{
			Debug.LogError($"Cannot queue crossfade to state with name {name} because there is no state with that name");
			return false;
		}
		return CrossfadeQueued(stateInfo.index, time, queueMode);
	}

	private bool CrossfadeQueued(int index, float time, QueueMode queueMode)
	{
		StateInfo stateInfo = CloneState(index);
		if (queueMode == QueueMode.PlayNow)
		{
			Crossfade(stateInfo.index, time);
			return true;
		}
		m_StateQueue.AddLast(new QueuedState(StateInfoToHandle(stateInfo), time));
		return true;
	}

	private bool Blend(int index, float targetWeight, float time)
	{
		StateInfo stateInfo = m_States[index];
		if (!stateInfo.enabled)
		{
			m_States.EnableState(index);
		}
		if (time == 0f)
		{
			stateInfo.ForceWeight(targetWeight);
		}
		else
		{
			SetupLerp(stateInfo, targetWeight, time);
		}
		m_LastAnimCmdFrameCount = Time.frameCount;
		stateInfo.crossFadeStartTimeSinceLevelLoad = 0f;
		onStateDirtyCallback?.Invoke();
		return true;
	}

	public bool Blend(string name, float targetWeight, float time)
	{
		StateInfo stateInfo = m_States.FindState(name);
		if (stateInfo == null)
		{
			Debug.LogError($"Cannot blend state with name {name} because there is no state with that name");
			return false;
		}
		return Blend(stateInfo.index, targetWeight, time);
	}

	public override void OnGraphStop(Playable playable)
	{
		if (!self.IsValid())
		{
			return;
		}
		for (int i = 0; i < m_States.Count; i++)
		{
			StateInfo stateInfo = m_States[i];
			if (stateInfo != null && stateInfo.fadeSpeed == 0f && stateInfo.targetWeight == 0f)
			{
				Playable input = m_Mixer.GetInput(stateInfo.index);
				if (!input.Equals(Playable.Null))
				{
					input.ResetTime(0f);
				}
			}
		}
	}

	private void UpdateDoneStatus()
	{
		if (!m_States.AnyStatePlaying())
		{
			bool num = playable.IsDone();
			playable.SetDone(value: true);
			if (!num && onDone != null)
			{
				onDone();
			}
		}
	}

	private void CleanClonedStates()
	{
		for (int num = m_States.Count - 1; num >= 0; num--)
		{
			StateInfo stateInfo = m_States[num];
			if (stateInfo != null && stateInfo.isReadyForCleanup)
			{
				graph.Disconnect(m_Mixer, stateInfo.index);
				graph.DestroyPlayable(stateInfo.playable);
				m_States.RemoveState(num);
			}
		}
	}

	private void DisconnectInput(int index)
	{
		if (keepStoppedPlayablesConnected)
		{
			m_States[index].Pause();
		}
		graph.Disconnect(m_Mixer, index);
	}

	private void ConnectInput(int index)
	{
		StateInfo stateInfo = m_States[index];
		graph.Connect(stateInfo.playable, 0, m_Mixer, stateInfo.index);
		m_Mixer.SetInputWeight(stateInfo.index, 0f);
	}

	private void UpdateStates(float deltaTime)
	{
		bool flag = false;
		float num = 0f;
		for (int i = 0; i < m_States.Count; i++)
		{
			StateInfo stateInfo = m_States[i];
			if (stateInfo == null)
			{
				continue;
			}
			if (stateInfo.fading)
			{
				stateInfo.SetWeight(Mathf.MoveTowards(stateInfo.weight, stateInfo.targetWeight, stateInfo.fadeSpeed * deltaTime));
				if (Mathf.Approximately(stateInfo.weight, stateInfo.targetWeight))
				{
					stateInfo.ForceWeight(stateInfo.targetWeight);
					if (stateInfo.weight == 0f)
					{
						stateInfo.Stop();
					}
				}
			}
			if (stateInfo.enabledDirty)
			{
				if (stateInfo.enabled)
				{
					stateInfo.Play();
				}
				else
				{
					stateInfo.Pause();
				}
				if (!keepStoppedPlayablesConnected)
				{
					Playable input = m_Mixer.GetInput(i);
					if (input.IsValid() && !stateInfo.enabled)
					{
						DisconnectInput(i);
					}
					else if (stateInfo.enabled && !input.IsValid())
					{
						ConnectInput(stateInfo.index);
					}
				}
			}
			if (stateInfo.enabled && stateInfo.wrapMode == WrapMode.Once)
			{
				bool isDone = stateInfo.isDone;
				float speed = stateInfo.speed;
				float time = stateInfo.GetTime();
				float playableDuration = stateInfo.playableDuration;
				if (isDone || (speed < 0f && time < 0f) || (speed >= 0f && time >= playableDuration))
				{
					stateInfo.Stop();
					stateInfo.Disable();
					if (!keepStoppedPlayablesConnected)
					{
						DisconnectInput(stateInfo.index);
					}
				}
			}
			num += stateInfo.weight;
			if (stateInfo.weightDirty)
			{
				flag = true;
			}
			stateInfo.ResetDirtyFlags();
		}
		if (!flag)
		{
			return;
		}
		bool flag2 = num > 0f;
		for (int j = 0; j < m_States.Count; j++)
		{
			StateInfo stateInfo2 = m_States[j];
			if (stateInfo2 != null)
			{
				float weight = (flag2 ? (stateInfo2.weight / num) : 0f);
				m_Mixer.SetInputWeight(stateInfo2.index, weight);
			}
		}
	}

	private float CalculateQueueTimes()
	{
		float num = -1f;
		for (int i = 0; i < m_States.Count; i++)
		{
			StateInfo stateInfo = m_States[i];
			if (stateInfo != null && stateInfo.enabled && stateInfo.playable.IsValid())
			{
				if (stateInfo.wrapMode == WrapMode.Loop)
				{
					return float.PositiveInfinity;
				}
				float speed = stateInfo.speed;
				float stateTime = m_States.GetStateTime(stateInfo.index);
				float num2 = ((speed > 0f) ? ((stateInfo.clip.length - stateTime) / speed) : ((!(speed < 0f)) ? float.PositiveInfinity : (stateTime / speed)));
				if (num2 > num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	public void ForceCleanAllQueuedStates()
	{
		ClearQueuedStates();
	}

	private void ClearQueuedStates()
	{
		foreach (QueuedState item in m_StateQueue)
		{
			m_States.StopState(item.state.index, cleanup: true);
		}
		m_StateQueue.Clear();
	}

	private void UpdateQueuedStates()
	{
		bool flag = true;
		float num = -1f;
		LinkedListNode<QueuedState> linkedListNode = m_StateQueue.First;
		while (linkedListNode != null)
		{
			if (flag)
			{
				num = CalculateQueueTimes();
				flag = false;
			}
			QueuedState value = linkedListNode.Value;
			if (value.fadeTime >= num)
			{
				Crossfade(value.state.index, value.fadeTime);
				flag = true;
				m_StateQueue.RemoveFirst();
				linkedListNode = m_StateQueue.First;
			}
			else
			{
				linkedListNode = linkedListNode.Next;
			}
		}
	}

	private void InvalidateStateTimes()
	{
		int count = m_States.Count;
		for (int i = 0; i < count; i++)
		{
			m_States[i]?.InvalidateTime();
		}
	}

	public override void PrepareFrame(Playable owner, FrameData data)
	{
		InvalidateStateTimes();
		UpdateQueuedStates();
		UpdateStates(data.deltaTime);
		UpdateDoneStatus();
		CleanClonedStates();
	}

	public bool ValidateInput(int index, Playable input)
	{
		if (!ValidateIndex(index))
		{
			return false;
		}
		StateInfo stateInfo = m_States[index];
		if (stateInfo == null || !stateInfo.playable.IsValid() || stateInfo.playable.GetHandle() != input.GetHandle())
		{
			return false;
		}
		return true;
	}

	public bool ValidateIndex(int index)
	{
		if (index >= 0)
		{
			return index < m_States.Count;
		}
		return false;
	}

	public void Dispose()
	{
		ClearQueuedStates();
		m_States.Dispose();
	}

	private void InvalidateStates()
	{
		m_StatesVersion++;
	}

	private StateHandle StateInfoToHandle(StateInfo info)
	{
		return new StateHandle(this, info.index, info.playable);
	}
}
