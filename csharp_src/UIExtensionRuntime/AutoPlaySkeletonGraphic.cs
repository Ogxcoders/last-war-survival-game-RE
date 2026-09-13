using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(SkeletonGraphic))]
public class AutoPlaySkeletonGraphic : MonoBehaviour
{
	public enum LoopMode
	{
		Once,
		LoopLastOnly,
		LoopAll
	}

	[SerializeField]
	private SkeletonGraphic skeletonGraphic;

	[SpineAnimation("", "skeletonGraphic", true, false)]
	[SerializeField]
	[Tooltip("可以调整播放顺序，或者删除不需要播放的动画")]
	private List<string> animationNames = new List<string>();

	public LoopMode loopMode;

	[Tooltip("延迟指定时间播放，单位秒")]
	public float delay;

	[Tooltip("在延迟播放前隐藏 spine 效果，更好展示登场效果")]
	public bool hideBeforePlay = true;

	private bool initialized;

	private Coroutine playCoroutine;

	private void Awake()
	{
		if (skeletonGraphic == null)
		{
			skeletonGraphic = GetComponent<SkeletonGraphic>();
		}
	}

	private void Start()
	{
		InitializeIfNeeded();
		StartPlaySequenceWithDelay();
	}

	private void OnEnable()
	{
		if (initialized && skeletonGraphic.AnimationState != null)
		{
			StartPlaySequenceWithDelay();
		}
	}

	private void OnDisable()
	{
		if (playCoroutine != null)
		{
			StopCoroutine(playCoroutine);
			playCoroutine = null;
		}
		if (skeletonGraphic != null)
		{
			skeletonGraphic.AnimationState?.ClearTracks();
			if (hideBeforePlay && delay > 0f)
			{
				SetGraphicVisible(visible: false);
			}
		}
	}

	private void InitializeIfNeeded()
	{
		if (initialized)
		{
			return;
		}
		if (skeletonGraphic == null)
		{
			Debug.LogError("[AutoPlaySkeletonGraphic] Missing SkeletonGraphic on " + base.gameObject.name);
			return;
		}
		skeletonGraphic.Initialize(overwrite: false);
		if (skeletonGraphic.SkeletonData == null)
		{
			Debug.LogWarning("[AutoPlaySkeletonGraphic] SkeletonData is null in " + base.gameObject.name);
		}
		initialized = true;
	}

	private void StartPlaySequenceWithDelay()
	{
		if (playCoroutine != null)
		{
			StopCoroutine(playCoroutine);
			playCoroutine = null;
		}
		if (delay > 0f)
		{
			playCoroutine = StartCoroutine(PlaySequenceCoroutine());
		}
		else
		{
			PlaySequence();
		}
	}

	private IEnumerator PlaySequenceCoroutine()
	{
		if (hideBeforePlay)
		{
			SetGraphicVisible(visible: false);
		}
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		PlaySequence();
		playCoroutine = null;
	}

	private void PlaySequence()
	{
		if (!initialized)
		{
			InitializeIfNeeded();
		}
		if (animationNames == null || animationNames.Count <= 0)
		{
			Debug.LogWarning("[AutoPlaySkeletonGraphic] No animations configured in " + base.gameObject.name);
			return;
		}
		SetGraphicVisible(visible: true);
		skeletonGraphic.AnimationState.ClearTracks();
		PlayInternal();
	}

	private void PlayInternal(bool isLoop = false)
	{
		if (skeletonGraphic == null || animationNames == null || animationNames.Count <= 0)
		{
			return;
		}
		Spine.AnimationState animationState = skeletonGraphic.AnimationState;
		string animationName = animationNames[0];
		TrackEntry trackEntry = animationState.SetAnimation(0, animationName, loop: false);
		if (!isLoop)
		{
			skeletonGraphic.Update();
		}
		int count = animationNames.Count;
		if (count == 1)
		{
			trackEntry.Loop = loopMode == LoopMode.LoopAll || loopMode == LoopMode.LoopLastOnly;
			return;
		}
		trackEntry.Loop = false;
		for (int i = 1; i < count; i++)
		{
			string animationName2 = animationNames[i];
			bool loop = loopMode == LoopMode.LoopLastOnly && i == count - 1;
			TrackEntry trackEntry2 = animationState.AddAnimation(0, animationName2, loop, 0f);
			trackEntry2.Complete -= OnLastComplete;
			if (loopMode == LoopMode.LoopAll && i == count - 1)
			{
				trackEntry2.Complete += OnLastComplete;
			}
		}
	}

	private void OnLastComplete(TrackEntry entry)
	{
		if (loopMode == LoopMode.LoopAll)
		{
			PlayInternal(isLoop: true);
		}
	}

	private void SetGraphicVisible(bool visible)
	{
		if (!(skeletonGraphic == null) && skeletonGraphic.enabled != visible)
		{
			skeletonGraphic.enabled = visible;
		}
	}

	public void ScanAnimations()
	{
		if (skeletonGraphic == null)
		{
			skeletonGraphic = GetComponent<SkeletonGraphic>();
		}
		skeletonGraphic.Initialize(overwrite: false);
		SkeletonData skeletonData = skeletonGraphic.SkeletonData;
		if (skeletonData == null)
		{
			Debug.LogWarning("[AutoPlaySkeletonGraphic] Cannot scan animations, SkeletonData is null");
			return;
		}
		animationNames.Clear();
		foreach (Spine.Animation animation in skeletonData.Animations)
		{
			animationNames.Add(animation.Name);
		}
	}

	private void AutoInit()
	{
		if (skeletonGraphic == null || animationNames == null || animationNames.Count == 0)
		{
			ScanAnimations();
		}
	}
}
