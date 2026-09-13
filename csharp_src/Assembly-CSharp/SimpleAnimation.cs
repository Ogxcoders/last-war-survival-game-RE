using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class SimpleAnimation : MonoBehaviour, IAnimationClipSource
{
	public interface State
	{
		bool enabled { get; set; }

		bool isValid { get; }

		float time { get; set; }

		float normalizedTime { get; set; }

		float speed { get; set; }

		string name { get; set; }

		float weight { get; set; }

		float length { get; }

		AnimationClip clip { get; }

		WrapMode wrapMode { get; set; }
	}

	private class StateEnumerable : IEnumerable<State>, IEnumerable
	{
		private class StateEnumerator : IEnumerator<State>, IEnumerator, IDisposable
		{
			private SimpleAnimation m_Owner;

			private IEnumerator<SimpleAnimationPlayable.IState> m_Impl;

			object IEnumerator.Current => GetCurrent();

			State IEnumerator<State>.Current => GetCurrent();

			public StateEnumerator(SimpleAnimation owner)
			{
				m_Owner = owner;
				m_Impl = m_Owner.m_Playable.GetStates().GetEnumerator();
				Reset();
			}

			private State GetCurrent()
			{
				return new StateImpl(m_Impl.Current, m_Owner);
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				return m_Impl.MoveNext();
			}

			public void Reset()
			{
				m_Impl.Reset();
			}
		}

		private SimpleAnimation m_Owner;

		public StateEnumerable(SimpleAnimation owner)
		{
			m_Owner = owner;
		}

		public IEnumerator<State> GetEnumerator()
		{
			return new StateEnumerator(m_Owner);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new StateEnumerator(m_Owner);
		}
	}

	private class StateImpl : State
	{
		private SimpleAnimationPlayable.IState m_StateHandle;

		private SimpleAnimation m_Component;

		bool State.enabled
		{
			get
			{
				return m_StateHandle.enabled;
			}
			set
			{
				m_StateHandle.enabled = value;
				if (value)
				{
					m_Component.Kick();
				}
			}
		}

		bool State.isValid => m_StateHandle.IsValid();

		float State.time
		{
			get
			{
				return m_StateHandle.time;
			}
			set
			{
				m_StateHandle.time = value;
				m_Component.Kick();
			}
		}

		float State.normalizedTime
		{
			get
			{
				return m_StateHandle.normalizedTime;
			}
			set
			{
				m_StateHandle.normalizedTime = value;
				m_Component.Kick();
			}
		}

		float State.speed
		{
			get
			{
				return m_StateHandle.speed;
			}
			set
			{
				m_StateHandle.speed = value;
				m_Component.Kick();
			}
		}

		string State.name
		{
			get
			{
				return m_StateHandle.name;
			}
			set
			{
				m_StateHandle.name = value;
			}
		}

		float State.weight
		{
			get
			{
				return m_StateHandle.weight;
			}
			set
			{
				m_StateHandle.weight = value;
				m_Component.Kick();
			}
		}

		float State.length => m_StateHandle.length;

		AnimationClip State.clip => m_StateHandle.clip;

		WrapMode State.wrapMode
		{
			get
			{
				return m_StateHandle.wrapMode;
			}
			set
			{
				Debug.LogError("Not Implemented");
			}
		}

		public StateImpl(SimpleAnimationPlayable.IState handle, SimpleAnimation component)
		{
			m_StateHandle = handle;
			m_Component = component;
		}
	}

	[Serializable]
	public class EditorState
	{
		public AnimationClip clip;

		public string name;

		public bool defaultState;
	}

	private const string kDefaultStateName = "Default";

	protected PlayableGraph m_Graph;

	protected PlayableHandle m_LayerMixer;

	protected PlayableHandle m_TransitionMixer;

	protected Animator m_Animator;

	protected bool m_Initialized;

	protected bool m_IsPlaying;

	protected SimpleAnimationPlayable m_Playable;

	[SerializeField]
	protected bool m_PlayAutomatically;

	[SerializeField]
	protected bool m_AnimatePhysics;

	[SerializeField]
	protected AnimatorCullingMode m_CullingMode = AnimatorCullingMode.CullUpdateTransforms;

	[SerializeField]
	protected WrapMode m_WrapMode;

	[SerializeField]
	protected AnimationClip m_Clip;

	[SerializeField]
	private EditorState[] m_States;

	private bool m_UpdateManual;

	public static HashSet<SimpleAnimation> s_DirtySimpleAnims = new HashSet<SimpleAnimation>();

	public static HashSet<SimpleAnimation> s_DirtySimpleCrossAnims = new HashSet<SimpleAnimation>();

	public Animator animator
	{
		get
		{
			if (m_Animator == null)
			{
				m_Animator = GetComponent<Animator>();
			}
			return m_Animator;
		}
	}

	public bool animatePhysics
	{
		get
		{
			return m_AnimatePhysics;
		}
		set
		{
			m_AnimatePhysics = value;
			animator.updateMode = (m_AnimatePhysics ? AnimatorUpdateMode.AnimatePhysics : AnimatorUpdateMode.Normal);
		}
	}

	public AnimatorCullingMode cullingMode
	{
		get
		{
			return animator.cullingMode;
		}
		set
		{
			m_CullingMode = value;
			animator.cullingMode = m_CullingMode;
		}
	}

	public bool isPlaying
	{
		get
		{
			if (m_Playable != null)
			{
				return m_Playable.IsPlaying();
			}
			return false;
		}
	}

	public bool playAutomatically
	{
		get
		{
			return m_PlayAutomatically;
		}
		set
		{
			m_PlayAutomatically = value;
		}
	}

	public AnimationClip clip
	{
		get
		{
			return m_Clip;
		}
		set
		{
			LegacyClipCheck(value);
			m_Clip = value;
		}
	}

	public WrapMode wrapMode
	{
		get
		{
			return m_WrapMode;
		}
		set
		{
			m_WrapMode = value;
		}
	}

	public State this[string name] => GetState(name);

	public bool UpdateManual
	{
		get
		{
			return m_UpdateManual;
		}
		set
		{
			m_UpdateManual = value;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public (bool stateDirty, string origStateName, float normalizedTime, float nonNormalizedTime) GetCurrentState()
	{
		bool isStateDirtyByAnimCmd = m_Playable.IsStateDirtyByAnimCmd;
		(string, float, float) currentState = m_Playable.GetCurrentState();
		return (stateDirty: isStateDirtyByAnimCmd, origStateName: currentState.Item1, normalizedTime: currentState.Item2, nonNormalizedTime: currentState.Item3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public (bool stateDirty, string origStateName, float normalizedTime, float nonNormalizedTime) GetLastState()
	{
		bool isStateDirtyByAnimCmd = m_Playable.IsStateDirtyByAnimCmd;
		(string, float, float) lastState = m_Playable.GetLastState();
		return (stateDirty: isStateDirtyByAnimCmd, origStateName: lastState.Item1, normalizedTime: lastState.Item2, nonNormalizedTime: lastState.Item3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public (bool stateDirty, string interruptStateName, float interruptTime, float interruptWeight, string lastStateName, float lastTime) GetInterruptingState()
	{
		bool isStateDirtyByAnimCmd = m_Playable.IsStateDirtyByAnimCmd;
		(string, float, float, string, float) interruptingState = m_Playable.GetInterruptingState();
		return (stateDirty: isStateDirtyByAnimCmd, interruptStateName: interruptingState.Item1, interruptTime: interruptingState.Item2, interruptWeight: interruptingState.Item3, lastStateName: interruptingState.Item4, lastTime: interruptingState.Item5);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsCrossFading()
	{
		return m_Playable.IsCrossFading();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool InterruptingCrossFading()
	{
		return m_Playable.InterruptingCrossFading();
	}

	public void AddClip(AnimationClip clip, string newName)
	{
		LegacyClipCheck(clip);
		AddState(clip, newName);
	}

	public void Blend(string stateName, float targetWeight, float fadeLength)
	{
		if (!m_UpdateManual)
		{
			m_Animator.enabled = true;
		}
		Kick();
		m_Playable.Blend(stateName, targetWeight, fadeLength);
	}

	public void CrossFade(string stateName, float fadeLength)
	{
		if (!m_UpdateManual)
		{
			m_Animator.enabled = true;
		}
		Kick();
		m_Playable.Crossfade(stateName, fadeLength);
	}

	public void CrossFadeQueued(string stateName, float fadeLength, QueueMode queueMode)
	{
		if (!m_UpdateManual)
		{
			m_Animator.enabled = true;
		}
		Kick();
		m_Playable.CrossfadeQueued(stateName, fadeLength, queueMode);
	}

	public int GetClipCount()
	{
		return m_Playable.GetClipCount();
	}

	public bool IsPlaying(string stateName)
	{
		if (m_Playable == null)
		{
			return false;
		}
		return m_Playable.IsPlaying(stateName);
	}

	public void Stop()
	{
		if (m_Playable != null)
		{
			m_Playable.StopAll();
		}
	}

	public void Stop(string stateName)
	{
		if (m_Playable != null)
		{
			m_Playable.Stop(stateName);
		}
	}

	public void Sample()
	{
		m_Graph.Evaluate();
	}

	public void Sample(float time)
	{
		m_Graph.Evaluate(time);
	}

	public bool Play()
	{
		if (!m_Initialized || m_Playable == null)
		{
			return false;
		}
		if (!m_UpdateManual)
		{
			m_Animator.enabled = true;
		}
		Kick();
		if (m_Clip != null && m_PlayAutomatically)
		{
			m_Playable.Play("Default");
		}
		return false;
	}

	public void AddState(AnimationClip clip, string name)
	{
		LegacyClipCheck(clip);
		Kick();
		if (m_Playable.AddClip(clip, name))
		{
			RebuildStates();
		}
	}

	public void RemoveState(string name)
	{
		if (m_Playable.RemoveClip(name))
		{
			RebuildStates();
		}
	}

	public void SetStateSpeed(string aniName, float speed)
	{
		if (GetState(aniName) != null)
		{
			GetState(aniName).speed = speed;
		}
	}

	public bool Play(string stateName)
	{
		if (!m_Initialized || m_Playable == null)
		{
			return false;
		}
		if (!m_UpdateManual)
		{
			m_Animator.enabled = true;
		}
		Kick();
		return m_Playable.Play(stateName);
	}

	public void PlayQueued(string stateName, QueueMode queueMode = QueueMode.CompleteOthers)
	{
		if (m_Initialized)
		{
			if (!m_UpdateManual)
			{
				m_Animator.enabled = true;
			}
			Kick();
			m_Playable.PlayQueued(stateName, queueMode);
		}
	}

	public void RemoveClip(AnimationClip clip)
	{
		if (clip == null)
		{
			throw new NullReferenceException("clip");
		}
		if (m_Playable.RemoveClip(clip))
		{
			RebuildStates();
		}
	}

	public void RewindAndPlay(string stateName)
	{
		Rewind(stateName);
		Play(stateName);
	}

	public void Rewind()
	{
		if (m_Initialized)
		{
			Kick();
			m_Playable.Rewind();
		}
	}

	public void ForceCleanAllQueuedStates()
	{
		if (m_Initialized && m_Playable != null)
		{
			m_Playable.ForceCleanAllQueuedStates();
		}
	}

	public void Rewind(string stateName)
	{
		if (m_Initialized)
		{
			Kick();
			m_Playable.Rewind(stateName);
		}
	}

	public State GetState(string stateName)
	{
		if (!m_Initialized)
		{
			return null;
		}
		SimpleAnimationPlayable.IState state = m_Playable.GetState(stateName);
		if (state == null)
		{
			return null;
		}
		return new StateImpl(state, this);
	}

	public IEnumerable<State> GetStates()
	{
		return new StateEnumerable(this);
	}

	public float GetClipLength(string stateName)
	{
		if (!m_Initialized)
		{
			return 0f;
		}
		return m_Playable.GetState(stateName)?.length ?? 0f;
	}

	public float GetClipTime(string stateName)
	{
		if (!m_Initialized)
		{
			return 0f;
		}
		return m_Playable.GetState(stateName)?.time ?? 0f;
	}

	public bool SampleAnimationAtTime(string stateName, float normalizedTime)
	{
		if (!m_Initialized)
		{
			return false;
		}
		SimpleAnimationPlayable.IState state = m_Playable.GetState(stateName);
		if (state == null)
		{
			return false;
		}
		if (state.clip != null)
		{
			state.clip.SampleAnimation(animator.gameObject, normalizedTime * state.clip.length);
			state.normalizedTime = normalizedTime;
			return true;
		}
		return false;
	}

	public void SampleAnimationAtTimeInEditor(string name, float time)
	{
		EditorState editorStates = GetEditorStates(name);
		PlayableGraph graph = PlayableGraph.Create("Preview Graph");
		graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
		AnimationClipPlayable value = AnimationClipPlayable.Create(graph, editorStates.clip);
		AnimationPlayableOutput.Create(graph, "Preview Output", animator).SetSourcePlayable(value);
		graph.GetRootPlayable(0).SetTime(time);
		graph.Evaluate();
		graph.Destroy();
	}

	protected void Kick()
	{
		if (!m_IsPlaying)
		{
			m_Graph.Play();
			m_IsPlaying = true;
		}
	}

	public EditorState GetEditorStates(string name)
	{
		for (int i = 0; i < m_States.Length; i++)
		{
			EditorState editorState = m_States[i];
			if (editorState.name == name)
			{
				return editorState;
			}
		}
		return null;
	}

	protected virtual void OnEnable()
	{
		Initialize();
		m_Graph.Play();
		if (m_PlayAutomatically)
		{
			Stop();
			Play();
		}
	}

	protected virtual void OnDisable()
	{
		if (m_Initialized)
		{
			Stop();
			m_Graph.Stop();
		}
	}

	private void Reset()
	{
		if (m_Graph.IsValid())
		{
			m_Graph.Destroy();
		}
		m_Initialized = false;
	}

	public void Initialize()
	{
		if (m_Initialized)
		{
			return;
		}
		m_Animator = GetComponent<Animator>();
		m_Animator.updateMode = (m_AnimatePhysics ? AnimatorUpdateMode.AnimatePhysics : AnimatorUpdateMode.Normal);
		m_Animator.cullingMode = m_CullingMode;
		m_Animator.runtimeAnimatorController = null;
		m_Graph = PlayableGraph.Create(base.name);
		SimpleAnimationPlayable template = new SimpleAnimationPlayable();
		ScriptPlayable<SimpleAnimationPlayable> scriptPlayable = ScriptPlayable<SimpleAnimationPlayable>.Create(m_Graph, template, 1);
		m_Playable = scriptPlayable.GetBehaviour();
		SimpleAnimationPlayable playable = m_Playable;
		playable.onDone = (Action)Delegate.Combine(playable.onDone, new Action(OnPlayableDone));
		m_Playable.onStateDirtyCallback = OnStateDirty;
		if (m_States == null)
		{
			m_States = new EditorState[1];
			m_States[0] = new EditorState();
			m_States[0].defaultState = true;
			m_States[0].name = "Default";
		}
		if (m_States != null)
		{
			EditorState[] states = m_States;
			foreach (EditorState editorState in states)
			{
				if ((bool)editorState.clip)
				{
					m_Playable.AddClip(editorState.clip, editorState.name);
				}
			}
		}
		EnsureDefaultStateExists();
		if (m_UpdateManual)
		{
			m_Graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
		}
		else
		{
			AnimationPlayableOutput.Create(m_Graph, "AnimationClip", animator).SetSourcePlayable((Playable)scriptPlayable, 0);
			m_Graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
			m_Graph.Play();
		}
		Play();
		Kick();
		m_Initialized = true;
	}

	private void EnsureDefaultStateExists()
	{
		if (m_Playable != null && m_Clip != null && m_Playable.GetState("Default") == null)
		{
			m_Playable.AddClip(m_Clip, "Default");
			Kick();
		}
	}

	protected virtual void Awake()
	{
		Initialize();
	}

	protected void OnDestroy()
	{
		if (m_Graph.IsValid())
		{
			m_Graph.Destroy();
		}
		m_Playable.Dispose();
		s_DirtySimpleAnims.Remove(this);
		s_DirtySimpleCrossAnims.Remove(this);
	}

	private void OnPlayableDone()
	{
		m_Graph.Stop();
		m_IsPlaying = false;
	}

	private void RebuildStates()
	{
		IEnumerable<State> states = GetStates();
		List<EditorState> list = new List<EditorState>();
		foreach (State item in states)
		{
			EditorState editorState = new EditorState();
			editorState.clip = item.clip;
			editorState.name = item.name;
			list.Add(editorState);
		}
		m_States = list.ToArray();
	}

	private EditorState CreateDefaultEditorState()
	{
		return new EditorState
		{
			name = "Default",
			clip = m_Clip,
			defaultState = true
		};
	}

	private static void LegacyClipCheck(AnimationClip clip)
	{
		if ((bool)clip && clip.legacy)
		{
			throw new ArgumentException($"Legacy clip {clip} cannot be used in this component. Set .legacy property to false before using this clip");
		}
	}

	private void InvalidLegacyClipError(string clipName, string stateName)
	{
		Debug.LogErrorFormat(base.gameObject, "Animation clip {0} in state {1} is Legacy. Set clip.legacy to false, or reimport as Generic to use it with SimpleAnimationComponent", clipName, stateName);
	}

	private void OnValidate()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if ((bool)m_Clip && m_Clip.legacy)
		{
			Debug.LogErrorFormat(base.gameObject, "Animation clip {0} is Legacy. Set clip.legacy to false, or reimport as Generic to use it with SimpleAnimationComponent", m_Clip.name);
			m_Clip = null;
		}
		if (m_States == null || m_States.Length == 0)
		{
			m_States = new EditorState[1];
		}
		if (m_States[0] == null)
		{
			m_States[0] = CreateDefaultEditorState();
		}
		if (!m_States[0].defaultState || m_States[0].name != "Default")
		{
			EditorState[] states = m_States;
			m_States = new EditorState[states.Length + 1];
			m_States[0] = CreateDefaultEditorState();
			states.CopyTo(m_States, 1);
		}
		if (m_States[0].clip != m_Clip)
		{
			m_States[0].clip = m_Clip;
		}
		for (int i = 1; i < m_States.Length; i++)
		{
			if (m_States[i] == null)
			{
				m_States[i] = new EditorState();
			}
			m_States[i].defaultState = false;
		}
		int num = m_States.Length;
		string[] array = new string[num];
		for (int j = 0; j < num; j++)
		{
			EditorState editorState = m_States[j];
			if (editorState.name == "" && (bool)editorState.clip)
			{
				editorState.name = editorState.clip.name;
			}
			array[j] = editorState.name;
			if ((bool)editorState.clip && editorState.clip.legacy)
			{
				InvalidLegacyClipError(editorState.clip.name, editorState.name);
				editorState.clip = null;
			}
		}
		m_Animator = GetComponent<Animator>();
		if ((bool)m_Animator)
		{
			m_Animator.updateMode = (m_AnimatePhysics ? AnimatorUpdateMode.AnimatePhysics : AnimatorUpdateMode.Normal);
			m_Animator.cullingMode = m_CullingMode;
			return;
		}
		Transform parent = base.transform;
		string text = parent.name;
		while (parent.parent != null)
		{
			parent = parent.parent;
			text = parent.name + "/" + text;
		}
		Debug.LogError("MissingComponentException:" + text + " Animator is null");
	}

	public void GetAnimationClips(List<AnimationClip> results)
	{
		EditorState[] states = m_States;
		foreach (EditorState editorState in states)
		{
			if (editorState.clip != null)
			{
				results.Add(editorState.clip);
			}
		}
	}

	public EditorState[] GetEditorStates()
	{
		return m_States;
	}

	public EditorState GetEditorState(string name)
	{
		for (int i = 0; i < m_States.Length; i++)
		{
			if (m_States[i].name == name)
			{
				return m_States[i];
			}
		}
		return null;
	}

	public EditorState GetEditorState(AnimationClip clip)
	{
		for (int i = 0; i < m_States.Length; i++)
		{
			if (m_States[i].clip == clip)
			{
				return m_States[i];
			}
		}
		return null;
	}

	private void OnStateDirty()
	{
		s_DirtySimpleAnims.Add(this);
	}
}
