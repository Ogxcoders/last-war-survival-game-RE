using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Spine.Unity.Playables;

public class SpineAnimationStateMixerBehaviour : PlayableBehaviour
{
	private float[] lastInputWeights;

	private bool lastAnyClipPlaying;

	public int trackIndex;

	private ScriptPlayable<SpineAnimationStateBehaviour>[] startingClips = new ScriptPlayable<SpineAnimationStateBehaviour>[2];

	private IAnimationStateComponent animationStateComponent;

	private bool pauseWithDirector = true;

	private bool endAtClipEnd = true;

	private float endMixOutDuration = 0.1f;

	private bool isPaused;

	private TrackEntry pausedTrackEntry;

	private float previousTimeScale = 1f;

	private TrackEntry timelineStartedTrackEntry;

	private AnimationState dummyAnimationState;

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (pauseWithDirector)
		{
			if (!isPaused)
			{
				HandlePause(playable);
			}
			isPaused = true;
		}
	}

	public override void OnGraphStop(Playable playable)
	{
		if (!isPaused && endAtClipEnd)
		{
			HandleClipEnd();
		}
	}

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		if (isPaused)
		{
			HandleResume(playable);
		}
		isPaused = false;
	}

	protected void HandlePause(Playable playable)
	{
		if (!animationStateComponent.IsNullOrDestroyed())
		{
			TrackEntry current = animationStateComponent.AnimationState.GetCurrent(trackIndex);
			if (current != null && current == timelineStartedTrackEntry)
			{
				previousTimeScale = current.TimeScale;
				current.TimeScale = 0f;
				pausedTrackEntry = current;
			}
		}
	}

	protected void HandleResume(Playable playable)
	{
		if (!animationStateComponent.IsNullOrDestroyed())
		{
			TrackEntry current = animationStateComponent.AnimationState.GetCurrent(trackIndex);
			if (current != null && current == pausedTrackEntry)
			{
				current.TimeScale = previousTimeScale;
			}
		}
	}

	protected void HandleClipEnd()
	{
		if (animationStateComponent.IsNullOrDestroyed())
		{
			return;
		}
		AnimationState animationState = animationStateComponent.AnimationState;
		if (endAtClipEnd && timelineStartedTrackEntry != null && timelineStartedTrackEntry == animationState.GetCurrent(trackIndex))
		{
			if (endMixOutDuration >= 0f)
			{
				animationState.SetEmptyAnimation(trackIndex, endMixOutDuration);
			}
			else
			{
				timelineStartedTrackEntry.TimeScale = 0f;
			}
			timelineStartedTrackEntry = null;
		}
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		SkeletonAnimation skeletonAnimation = playerData as SkeletonAnimation;
		SkeletonGraphic skeletonGraphic = playerData as SkeletonGraphic;
		animationStateComponent = playerData as IAnimationStateComponent;
		ISkeletonComponent skeletonComponent = playerData as ISkeletonComponent;
		if (animationStateComponent.IsNullOrDestroyed() || skeletonComponent == null)
		{
			return;
		}
		_ = skeletonComponent.Skeleton;
		AnimationState animationState = animationStateComponent.AnimationState;
		if (!Application.isPlaying)
		{
			PreviewEditModePose(playable, skeletonComponent, animationStateComponent, skeletonAnimation, skeletonGraphic);
			return;
		}
		int inputCount = playable.GetInputCount();
		if (lastInputWeights == null || lastInputWeights.Length < inputCount)
		{
			lastInputWeights = new float[inputCount];
			for (int i = 0; i < inputCount; i++)
			{
				lastInputWeights[i] = 0f;
			}
		}
		float[] array = lastInputWeights;
		int num = 0;
		bool flag = false;
		for (int j = 0; j < inputCount; j++)
		{
			float num2 = array[j];
			float inputWeight = playable.GetInputWeight(j);
			bool num3 = num2 == 0f && inputWeight > 0f;
			if (inputWeight > 0f)
			{
				flag = true;
			}
			array[j] = inputWeight;
			if (num3 && num < 2)
			{
				ScriptPlayable<SpineAnimationStateBehaviour> scriptPlayable = (ScriptPlayable<SpineAnimationStateBehaviour>)playable.GetInput(j);
				startingClips[num++] = scriptPlayable;
			}
		}
		if (num == 2)
		{
			ScriptPlayable<SpineAnimationStateBehaviour> scriptPlayable2 = startingClips[0];
			ScriptPlayable<SpineAnimationStateBehaviour> scriptPlayable3 = startingClips[1];
			if (scriptPlayable2.GetDuration() > scriptPlayable3.GetDuration())
			{
				startingClips[0] = scriptPlayable3;
				startingClips[1] = scriptPlayable2;
			}
		}
		for (int k = 0; k < num; k++)
		{
			ScriptPlayable<SpineAnimationStateBehaviour> playable2 = startingClips[k];
			SpineAnimationStateBehaviour behaviour = playable2.GetBehaviour();
			pauseWithDirector = !behaviour.dontPauseWithDirector;
			endAtClipEnd = !behaviour.dontEndWithClip;
			endMixOutDuration = behaviour.endMixOutDuration;
			if (behaviour.animationReference == null)
			{
				float mixDuration = (behaviour.customDuration ? GetCustomMixDuration(behaviour) : animationState.Data.DefaultMix);
				animationState.SetEmptyAnimation(trackIndex, mixDuration);
			}
			else if (behaviour.animationReference.Animation != null)
			{
				TrackEntry current = animationState.GetCurrent(trackIndex);
				float num4 = (behaviour.customDuration ? GetCustomMixDuration(behaviour) : 0f);
				TrackEntry trackEntry;
				if (current == null && num4 > 0f)
				{
					animationState.SetEmptyAnimation(trackIndex, 0f);
					trackEntry = animationState.AddAnimation(trackIndex, behaviour.animationReference.Animation, behaviour.loop, 0f);
				}
				else
				{
					trackEntry = animationState.SetAnimation(trackIndex, behaviour.animationReference.Animation, behaviour.loop);
				}
				trackEntry.EventThreshold = behaviour.eventThreshold;
				trackEntry.DrawOrderThreshold = behaviour.drawOrderThreshold;
				trackEntry.TrackTime = (float)playable2.GetTime() * (float)playable2.GetSpeed();
				trackEntry.TimeScale = (float)playable2.GetSpeed();
				trackEntry.AttachmentThreshold = behaviour.attachmentThreshold;
				trackEntry.HoldPrevious = behaviour.holdPrevious;
				if (behaviour.customDuration)
				{
					trackEntry.MixDuration = num4;
				}
				timelineStartedTrackEntry = trackEntry;
			}
			if ((bool)skeletonAnimation)
			{
				skeletonAnimation.Update(0f);
				skeletonAnimation.LateUpdate();
			}
			else if ((bool)(UnityEngine.Object)(object)skeletonGraphic)
			{
				skeletonGraphic.Update(0f);
				skeletonGraphic.LateUpdate();
			}
		}
		startingClips[0] = (startingClips[1] = ScriptPlayable<SpineAnimationStateBehaviour>.Null);
		if (lastAnyClipPlaying && !flag)
		{
			HandleClipEnd();
		}
		lastAnyClipPlaying = flag;
	}

	public void PreviewEditModePose(Playable playable, ISkeletonComponent skeletonComponent, IAnimationStateComponent animationStateComponent, SkeletonAnimation skeletonAnimation, SkeletonGraphic skeletonGraphic)
	{
		if (Application.isPlaying || animationStateComponent.IsNullOrDestroyed() || skeletonComponent == null)
		{
			return;
		}
		int inputCount = playable.GetInputCount();
		int num = -1;
		for (int i = 0; i < inputCount; i++)
		{
			if (playable.GetInputWeight(i) > 0f)
			{
				num = i;
			}
		}
		if (num == -1)
		{
			return;
		}
		ScriptPlayable<SpineAnimationStateBehaviour> playable2 = (ScriptPlayable<SpineAnimationStateBehaviour>)playable.GetInput(num);
		SpineAnimationStateBehaviour behaviour = playable2.GetBehaviour();
		Skeleton skeleton = skeletonComponent.Skeleton;
		if (behaviour.animationReference != null && (bool)behaviour.animationReference.SkeletonDataAsset && skeletonComponent.SkeletonDataAsset.GetSkeletonData(quiet: true) != behaviour.animationReference.SkeletonDataAsset.GetSkeletonData(quiet: true))
		{
			Debug.LogWarningFormat("SpineAnimationStateMixerBehaviour tried to apply an animation for the wrong skeleton. Expected {0}. Was {1}", skeletonComponent.SkeletonDataAsset, behaviour.animationReference.SkeletonDataAsset);
		}
		Animation animation = null;
		float trackTime = 0f;
		bool loop = false;
		if (num != 0 && inputCount > 1)
		{
			ScriptPlayable<SpineAnimationStateBehaviour> playable3 = (ScriptPlayable<SpineAnimationStateBehaviour>)playable.GetInput(num - 1);
			SpineAnimationStateBehaviour behaviour2 = playable3.GetBehaviour();
			animation = ((behaviour2.animationReference != null) ? behaviour2.animationReference.Animation : null);
			trackTime = (float)playable3.GetTime() * (float)playable3.GetSpeed();
			loop = behaviour2.loop;
		}
		Animation animation2 = ((behaviour.animationReference != null) ? behaviour.animationReference.Animation : null);
		float num2 = (float)playable2.GetTime() * (float)playable2.GetSpeed();
		float num3 = behaviour.mixDuration;
		if (!behaviour.customDuration && animation != null && animation2 != null)
		{
			num3 = animationStateComponent.AnimationState.Data.GetMix(animation, animation2);
		}
		if (trackIndex == 0)
		{
			skeleton.SetToSetupPose();
		}
		if (animation != null && num3 > 0f && num2 < num3)
		{
			dummyAnimationState = dummyAnimationState ?? new AnimationState(skeletonComponent.SkeletonDataAsset.GetAnimationStateData());
			TrackEntry trackEntry = dummyAnimationState.GetCurrent(0);
			TrackEntry trackEntry2 = trackEntry?.MixingFrom;
			if (trackEntry == null || trackEntry.Animation != animation2 || trackEntry2 == null || trackEntry2.Animation != animation)
			{
				dummyAnimationState.ClearTracks();
				trackEntry2 = dummyAnimationState.SetAnimation(0, animation, loop);
				trackEntry2.AllowImmediateQueue();
				if (animation2 != null)
				{
					trackEntry = dummyAnimationState.SetAnimation(0, animation2, behaviour.loop);
					trackEntry.HoldPrevious = behaviour.holdPrevious;
				}
			}
			trackEntry2.TrackTime = trackTime;
			if (trackEntry != null)
			{
				trackEntry.TrackTime = num2;
				trackEntry.MixTime = num2;
			}
			dummyAnimationState.Update(0f);
			dummyAnimationState.Apply(skeleton);
		}
		else
		{
			animation2?.Apply(skeleton, 0f, num2, behaviour.loop, null, 1f, MixBlend.Setup, MixDirection.In);
		}
		skeleton.UpdateWorldTransform();
		if ((bool)skeletonAnimation)
		{
			skeletonAnimation.LateUpdate();
		}
		else if ((bool)(UnityEngine.Object)(object)skeletonGraphic)
		{
			skeletonGraphic.LateUpdate();
		}
	}

	private float GetCustomMixDuration(SpineAnimationStateBehaviour clipData)
	{
		if (clipData.useBlendDuration)
		{
			TimelineClip timelineClip = clipData.timelineClip;
			return (float)Math.Max(timelineClip.blendInDuration, timelineClip.easeInDuration);
		}
		return clipData.mixDuration;
	}
}
