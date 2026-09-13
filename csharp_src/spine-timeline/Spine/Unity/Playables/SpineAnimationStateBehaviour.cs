using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace Spine.Unity.Playables;

[Serializable]
public class SpineAnimationStateBehaviour : PlayableBehaviour
{
	[NonSerialized]
	public TimelineClip timelineClip;

	public AnimationReferenceAsset animationReference;

	public bool loop;

	public bool customDuration;

	public bool useBlendDuration = true;

	[SerializeField]
	private bool isInitialized;

	public float mixDuration = 0.1f;

	public bool holdPrevious;

	public bool dontPauseWithDirector;

	[FormerlySerializedAs("dontPauseOnStop")]
	public bool dontEndWithClip;

	public float endMixOutDuration = 0.1f;

	[Range(0f, 1f)]
	public float attachmentThreshold = 0.5f;

	[Range(0f, 1f)]
	public float eventThreshold = 0.5f;

	[Range(0f, 1f)]
	public float drawOrderThreshold = 0.5f;
}
