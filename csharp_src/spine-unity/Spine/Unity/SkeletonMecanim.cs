using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity;

[RequireComponent(typeof(Animator))]
[HelpURL("http://esotericsoftware.com/spine-unity#SkeletonMecanim-Component")]
public class SkeletonMecanim : SkeletonRenderer, ISkeletonAnimation, ISpineComponent
{
	[Serializable]
	public class MecanimTranslator
	{
		public delegate void OnClipAppliedDelegate(Animation clip, int layerIndex, float weight, float time, float lastTime, bool playsBackward);

		public enum MixMode
		{
			AlwaysMix,
			MixNext,
			Hard
		}

		protected class ClipInfos
		{
			public bool isInterruptionActive;

			public bool isLastFrameOfInterruption;

			public int clipInfoCount;

			public int nextClipInfoCount;

			public int interruptingClipInfoCount;

			public readonly List<AnimatorClipInfo> clipInfos = new List<AnimatorClipInfo>();

			public readonly List<AnimatorClipInfo> nextClipInfos = new List<AnimatorClipInfo>();

			public readonly List<AnimatorClipInfo> interruptingClipInfos = new List<AnimatorClipInfo>();

			public AnimatorStateInfo stateInfo;

			public AnimatorStateInfo nextStateInfo;

			public AnimatorStateInfo interruptingStateInfo;

			public float interruptingClipTimeAddition;
		}

		private class AnimationClipEqualityComparer : IEqualityComparer<AnimationClip>
		{
			internal static readonly IEqualityComparer<AnimationClip> Instance = new AnimationClipEqualityComparer();

			public bool Equals(AnimationClip x, AnimationClip y)
			{
				return x.GetInstanceID() == y.GetInstanceID();
			}

			public int GetHashCode(AnimationClip o)
			{
				return o.GetInstanceID();
			}
		}

		private class IntEqualityComparer : IEqualityComparer<int>
		{
			internal static readonly IEqualityComparer<int> Instance = new IntEqualityComparer();

			public bool Equals(int x, int y)
			{
				return x == y;
			}

			public int GetHashCode(int o)
			{
				return o;
			}
		}

		private const float WeightEpsilon = 0.0001f;

		public bool autoReset = true;

		public bool useCustomMixMode = true;

		public MixMode[] layerMixModes = new MixMode[0];

		public MixBlend[] layerBlendModes = new MixBlend[0];

		private readonly Dictionary<int, Animation> animationTable = new Dictionary<int, Animation>(IntEqualityComparer.Instance);

		private readonly Dictionary<AnimationClip, int> clipNameHashCodeTable = new Dictionary<AnimationClip, int>(AnimationClipEqualityComparer.Instance);

		private readonly List<Animation> previousAnimations = new List<Animation>();

		protected ClipInfos[] layerClipInfos = new ClipInfos[0];

		private Animator animator;

		public Animator Animator => animator;

		public int MecanimLayerCount
		{
			get
			{
				if (!animator)
				{
					return 0;
				}
				return animator.layerCount;
			}
		}

		public string[] MecanimLayerNames
		{
			get
			{
				if (!animator)
				{
					return new string[0];
				}
				string[] array = new string[animator.layerCount];
				for (int i = 0; i < animator.layerCount; i++)
				{
					array[i] = animator.GetLayerName(i);
				}
				return array;
			}
		}

		protected event OnClipAppliedDelegate _OnClipApplied;

		public event OnClipAppliedDelegate OnClipApplied
		{
			add
			{
				_OnClipApplied += value;
			}
			remove
			{
				_OnClipApplied -= value;
			}
		}

		public void Initialize(Animator animator, SkeletonDataAsset skeletonDataAsset)
		{
			this.animator = animator;
			previousAnimations.Clear();
			animationTable.Clear();
			foreach (Animation animation in skeletonDataAsset.GetSkeletonData(quiet: true).Animations)
			{
				animationTable.Add(animation.Name.GetHashCode(), animation);
			}
			clipNameHashCodeTable.Clear();
			ClearClipInfosForLayers();
		}

		private bool ApplyAnimation(Skeleton skeleton, AnimatorClipInfo info, AnimatorStateInfo stateInfo, int layerIndex, float layerWeight, MixBlend layerBlendMode, bool useClipWeight1 = false)
		{
			float num = info.weight * layerWeight;
			if (num < 0.0001f)
			{
				return false;
			}
			Animation animation = GetAnimation(info.clip);
			if (animation == null)
			{
				return false;
			}
			float time = AnimationTime(stateInfo.normalizedTime, info.clip.length, info.clip.isLooping, stateInfo.speed < 0f);
			num = (useClipWeight1 ? layerWeight : num);
			animation.Apply(skeleton, 0f, time, info.clip.isLooping, null, num, layerBlendMode, MixDirection.In);
			if (this._OnClipApplied != null)
			{
				OnClipAppliedCallback(animation, stateInfo, layerIndex, time, info.clip.isLooping, num);
			}
			return true;
		}

		private bool ApplyInterruptionAnimation(Skeleton skeleton, bool interpolateWeightTo1, AnimatorClipInfo info, AnimatorStateInfo stateInfo, int layerIndex, float layerWeight, MixBlend layerBlendMode, float interruptingClipTimeAddition, bool useClipWeight1 = false)
		{
			float num = (interpolateWeightTo1 ? ((info.weight + 1f) * 0.5f) : info.weight) * layerWeight;
			if (num < 0.0001f)
			{
				return false;
			}
			Animation animation = GetAnimation(info.clip);
			if (animation == null)
			{
				return false;
			}
			float time = AnimationTime(stateInfo.normalizedTime + interruptingClipTimeAddition, info.clip.length, stateInfo.speed < 0f);
			num = (useClipWeight1 ? layerWeight : num);
			animation.Apply(skeleton, 0f, time, info.clip.isLooping, null, num, layerBlendMode, MixDirection.In);
			if (this._OnClipApplied != null)
			{
				OnClipAppliedCallback(animation, stateInfo, layerIndex, time, info.clip.isLooping, num);
			}
			return true;
		}

		private void OnClipAppliedCallback(Animation clip, AnimatorStateInfo stateInfo, int layerIndex, float time, bool isLooping, float weight)
		{
			float num = stateInfo.speedMultiplier * stateInfo.speed;
			float num2 = time - Time.deltaTime * num;
			float duration = clip.Duration;
			if (isLooping && duration != 0f)
			{
				time %= duration;
				num2 %= duration;
			}
			this._OnClipApplied(clip, layerIndex, weight, time, num2, num < 0f);
		}

		public void Apply(Skeleton skeleton)
		{
			if (layerMixModes.Length < animator.layerCount)
			{
				int num = layerMixModes.Length;
				Array.Resize(ref layerMixModes, animator.layerCount);
				for (int i = num; i < animator.layerCount; i++)
				{
					bool flag = false;
					if (i < layerBlendModes.Length)
					{
						flag = layerBlendModes[i] == MixBlend.Add;
					}
					layerMixModes[i] = ((!flag) ? MixMode.MixNext : MixMode.AlwaysMix);
				}
			}
			InitClipInfosForLayers();
			int j = 0;
			for (int layerCount = animator.layerCount; j < layerCount; j++)
			{
				GetStateUpdatesFromAnimator(j);
			}
			if (autoReset)
			{
				List<Animation> list = previousAnimations;
				int k = 0;
				for (int count = list.Count; k < count; k++)
				{
					list[k].Apply(skeleton, 0f, 0f, loop: false, null, 0f, MixBlend.Setup, MixDirection.Out);
				}
				list.Clear();
				int l = 0;
				for (int layerCount2 = animator.layerCount; l < layerCount2; l++)
				{
					float num2 = ((l == 0) ? 1f : animator.GetLayerWeight(l));
					if (num2 <= 0f)
					{
						continue;
					}
					bool flag2 = animator.GetNextAnimatorStateInfo(l).fullPathHash != 0;
					GetAnimatorClipInfos(l, out var isInterruptionActive, out var clipInfoCount, out var nextClipInfoCount, out var interruptingClipInfoCount, out var clipInfo, out var nextClipInfo, out var interruptingClipInfo, out var shallInterpolateWeightTo);
					for (int m = 0; m < clipInfoCount; m++)
					{
						AnimatorClipInfo animatorClipInfo = clipInfo[m];
						if (!(animatorClipInfo.weight * num2 < 0.0001f))
						{
							Animation animation = GetAnimation(animatorClipInfo.clip);
							if (animation != null)
							{
								list.Add(animation);
							}
						}
					}
					if (flag2)
					{
						for (int n = 0; n < nextClipInfoCount; n++)
						{
							AnimatorClipInfo animatorClipInfo2 = nextClipInfo[n];
							if (!(animatorClipInfo2.weight * num2 < 0.0001f))
							{
								Animation animation2 = GetAnimation(animatorClipInfo2.clip);
								if (animation2 != null)
								{
									list.Add(animation2);
								}
							}
						}
					}
					if (!isInterruptionActive)
					{
						continue;
					}
					for (int num3 = 0; num3 < interruptingClipInfoCount; num3++)
					{
						AnimatorClipInfo animatorClipInfo3 = interruptingClipInfo[num3];
						if (!((shallInterpolateWeightTo ? ((animatorClipInfo3.weight + 1f) * 0.5f) : animatorClipInfo3.weight) * num2 < 0.0001f))
						{
							Animation animation3 = GetAnimation(animatorClipInfo3.clip);
							if (animation3 != null)
							{
								list.Add(animation3);
							}
						}
					}
				}
			}
			int num4 = 0;
			for (int layerCount3 = animator.layerCount; num4 < layerCount3; num4++)
			{
				float layerWeight = ((num4 == 0) ? 1f : animator.GetLayerWeight(num4));
				GetAnimatorStateInfos(num4, out var isInterruptionActive2, out var stateInfo, out var nextStateInfo, out var interruptingStateInfo, out var interruptingClipTimeAddition);
				bool flag3 = nextStateInfo.fullPathHash != 0;
				GetAnimatorClipInfos(num4, out isInterruptionActive2, out var clipInfoCount2, out var nextClipInfoCount2, out var interruptingClipInfoCount2, out var clipInfo2, out var nextClipInfo2, out var interruptingClipInfo2, out var shallInterpolateWeightTo2);
				MixBlend layerBlendMode = ((num4 < layerBlendModes.Length) ? layerBlendModes[num4] : MixBlend.Replace);
				MixMode mixMode = GetMixMode(num4, layerBlendMode);
				if (mixMode == MixMode.AlwaysMix)
				{
					for (int num5 = 0; num5 < clipInfoCount2; num5++)
					{
						ApplyAnimation(skeleton, clipInfo2[num5], stateInfo, num4, layerWeight, layerBlendMode);
					}
					if (flag3)
					{
						for (int num6 = 0; num6 < nextClipInfoCount2; num6++)
						{
							ApplyAnimation(skeleton, nextClipInfo2[num6], nextStateInfo, num4, layerWeight, layerBlendMode);
						}
					}
					if (isInterruptionActive2)
					{
						for (int num7 = 0; num7 < interruptingClipInfoCount2; num7++)
						{
							ApplyInterruptionAnimation(skeleton, shallInterpolateWeightTo2, interruptingClipInfo2[num7], interruptingStateInfo, num4, layerWeight, layerBlendMode, interruptingClipTimeAddition);
						}
					}
					continue;
				}
				int num8;
				for (num8 = 0; num8 < clipInfoCount2; num8++)
				{
					if (ApplyAnimation(skeleton, clipInfo2[num8], stateInfo, num4, layerWeight, layerBlendMode, useClipWeight1: true))
					{
						num8++;
						break;
					}
				}
				for (; num8 < clipInfoCount2; num8++)
				{
					ApplyAnimation(skeleton, clipInfo2[num8], stateInfo, num4, layerWeight, layerBlendMode);
				}
				num8 = 0;
				if (flag3)
				{
					if (mixMode == MixMode.Hard)
					{
						for (; num8 < nextClipInfoCount2; num8++)
						{
							if (ApplyAnimation(skeleton, nextClipInfo2[num8], nextStateInfo, num4, layerWeight, layerBlendMode, useClipWeight1: true))
							{
								num8++;
								break;
							}
						}
					}
					for (; num8 < nextClipInfoCount2; num8++)
					{
						ApplyAnimation(skeleton, nextClipInfo2[num8], nextStateInfo, num4, layerWeight, layerBlendMode);
					}
				}
				num8 = 0;
				if (!isInterruptionActive2)
				{
					continue;
				}
				if (mixMode == MixMode.Hard)
				{
					for (; num8 < interruptingClipInfoCount2; num8++)
					{
						if (ApplyInterruptionAnimation(skeleton, shallInterpolateWeightTo2, interruptingClipInfo2[num8], interruptingStateInfo, num4, layerWeight, layerBlendMode, interruptingClipTimeAddition, useClipWeight1: true))
						{
							num8++;
							break;
						}
					}
				}
				for (; num8 < interruptingClipInfoCount2; num8++)
				{
					ApplyInterruptionAnimation(skeleton, shallInterpolateWeightTo2, interruptingClipInfo2[num8], interruptingStateInfo, num4, layerWeight, layerBlendMode, interruptingClipTimeAddition);
				}
			}
		}

		public KeyValuePair<Animation, float> GetActiveAnimationAndTime(int layer)
		{
			if (layer >= layerClipInfos.Length)
			{
				return new KeyValuePair<Animation, float>(null, 0f);
			}
			ClipInfos clipInfos = layerClipInfos[layer];
			bool isInterruptionActive = clipInfos.isInterruptionActive;
			AnimationClip animationClip = null;
			AnimatorStateInfo animatorStateInfo;
			if (isInterruptionActive && clipInfos.interruptingClipInfoCount > 0)
			{
				animationClip = clipInfos.interruptingClipInfos[0].clip;
				animatorStateInfo = clipInfos.interruptingStateInfo;
			}
			else
			{
				animationClip = clipInfos.clipInfos[0].clip;
				animatorStateInfo = clipInfos.stateInfo;
			}
			Animation animation = GetAnimation(animationClip);
			float value = AnimationTime(animatorStateInfo.normalizedTime, animationClip.length, animationClip.isLooping, animatorStateInfo.speed < 0f);
			return new KeyValuePair<Animation, float>(animation, value);
		}

		private static float AnimationTime(float normalizedTime, float clipLength, bool loop, bool reversed)
		{
			float num = AnimationTime(normalizedTime, clipLength, reversed);
			if (loop)
			{
				return num;
			}
			if (!(clipLength - num < 1f / 30f))
			{
				return num;
			}
			return clipLength;
		}

		private static float AnimationTime(float normalizedTime, float clipLength, bool reversed)
		{
			if (reversed)
			{
				normalizedTime = 1f - normalizedTime;
			}
			if (normalizedTime < 0f)
			{
				normalizedTime = normalizedTime % 1f + 1f;
			}
			return normalizedTime * clipLength;
		}

		private void InitClipInfosForLayers()
		{
			if (layerClipInfos.Length >= animator.layerCount)
			{
				return;
			}
			Array.Resize(ref layerClipInfos, animator.layerCount);
			int i = 0;
			for (int layerCount = animator.layerCount; i < layerCount; i++)
			{
				if (layerClipInfos[i] == null)
				{
					layerClipInfos[i] = new ClipInfos();
				}
			}
		}

		private void ClearClipInfosForLayers()
		{
			int i = 0;
			for (int num = layerClipInfos.Length; i < num; i++)
			{
				if (layerClipInfos[i] == null)
				{
					layerClipInfos[i] = new ClipInfos();
					continue;
				}
				layerClipInfos[i].isInterruptionActive = false;
				layerClipInfos[i].isLastFrameOfInterruption = false;
				layerClipInfos[i].clipInfos.Clear();
				layerClipInfos[i].nextClipInfos.Clear();
				layerClipInfos[i].interruptingClipInfos.Clear();
			}
		}

		private MixMode GetMixMode(int layer, MixBlend layerBlendMode)
		{
			if (useCustomMixMode)
			{
				MixMode mixMode = layerMixModes[layer];
				if (layerBlendMode == MixBlend.Add && mixMode == MixMode.MixNext)
				{
					mixMode = MixMode.AlwaysMix;
					layerMixModes[layer] = mixMode;
				}
				return mixMode;
			}
			if (layerBlendMode != MixBlend.Add)
			{
				return MixMode.MixNext;
			}
			return MixMode.AlwaysMix;
		}

		private void GetStateUpdatesFromAnimator(int layer)
		{
			ClipInfos clipInfos = layerClipInfos[layer];
			int currentAnimatorClipInfoCount = animator.GetCurrentAnimatorClipInfoCount(layer);
			int nextAnimatorClipInfoCount = animator.GetNextAnimatorClipInfoCount(layer);
			List<AnimatorClipInfo> clipInfos2 = clipInfos.clipInfos;
			List<AnimatorClipInfo> nextClipInfos = clipInfos.nextClipInfos;
			List<AnimatorClipInfo> interruptingClipInfos = clipInfos.interruptingClipInfos;
			clipInfos.isInterruptionActive = currentAnimatorClipInfoCount == 0 && clipInfos2.Count != 0 && nextAnimatorClipInfoCount == 0 && nextClipInfos.Count != 0;
			if (clipInfos.isInterruptionActive)
			{
				AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(layer);
				clipInfos.isLastFrameOfInterruption = nextAnimatorStateInfo.fullPathHash == 0;
				if (!clipInfos.isLastFrameOfInterruption)
				{
					animator.GetNextAnimatorClipInfo(layer, interruptingClipInfos);
					clipInfos.interruptingClipInfoCount = interruptingClipInfos.Count;
					float normalizedTime = clipInfos.interruptingStateInfo.normalizedTime;
					float normalizedTime2 = nextAnimatorStateInfo.normalizedTime;
					clipInfos.interruptingClipTimeAddition = normalizedTime2 - normalizedTime;
					clipInfos.interruptingStateInfo = nextAnimatorStateInfo;
				}
				return;
			}
			clipInfos.clipInfoCount = currentAnimatorClipInfoCount;
			clipInfos.nextClipInfoCount = nextAnimatorClipInfoCount;
			clipInfos.interruptingClipInfoCount = 0;
			clipInfos.isLastFrameOfInterruption = false;
			if (clipInfos2.Capacity < currentAnimatorClipInfoCount)
			{
				clipInfos2.Capacity = currentAnimatorClipInfoCount;
			}
			if (nextClipInfos.Capacity < nextAnimatorClipInfoCount)
			{
				nextClipInfos.Capacity = nextAnimatorClipInfoCount;
			}
			animator.GetCurrentAnimatorClipInfo(layer, clipInfos2);
			animator.GetNextAnimatorClipInfo(layer, nextClipInfos);
			clipInfos.stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
			clipInfos.nextStateInfo = animator.GetNextAnimatorStateInfo(layer);
		}

		private void GetAnimatorClipInfos(int layer, out bool isInterruptionActive, out int clipInfoCount, out int nextClipInfoCount, out int interruptingClipInfoCount, out IList<AnimatorClipInfo> clipInfo, out IList<AnimatorClipInfo> nextClipInfo, out IList<AnimatorClipInfo> interruptingClipInfo, out bool shallInterpolateWeightTo1)
		{
			ClipInfos clipInfos = layerClipInfos[layer];
			isInterruptionActive = clipInfos.isInterruptionActive;
			clipInfoCount = clipInfos.clipInfoCount;
			nextClipInfoCount = clipInfos.nextClipInfoCount;
			interruptingClipInfoCount = clipInfos.interruptingClipInfoCount;
			clipInfo = clipInfos.clipInfos;
			nextClipInfo = clipInfos.nextClipInfos;
			interruptingClipInfo = (isInterruptionActive ? clipInfos.interruptingClipInfos : null);
			shallInterpolateWeightTo1 = clipInfos.isLastFrameOfInterruption;
		}

		private void GetAnimatorStateInfos(int layer, out bool isInterruptionActive, out AnimatorStateInfo stateInfo, out AnimatorStateInfo nextStateInfo, out AnimatorStateInfo interruptingStateInfo, out float interruptingClipTimeAddition)
		{
			ClipInfos clipInfos = layerClipInfos[layer];
			isInterruptionActive = clipInfos.isInterruptionActive;
			stateInfo = clipInfos.stateInfo;
			nextStateInfo = clipInfos.nextStateInfo;
			interruptingStateInfo = clipInfos.interruptingStateInfo;
			interruptingClipTimeAddition = (clipInfos.isLastFrameOfInterruption ? clipInfos.interruptingClipTimeAddition : 0f);
		}

		private Animation GetAnimation(AnimationClip clip)
		{
			if (!clipNameHashCodeTable.TryGetValue(clip, out var value))
			{
				value = clip.name.GetHashCode();
				clipNameHashCodeTable.Add(clip, value);
			}
			animationTable.TryGetValue(value, out var value2);
			return value2;
		}
	}

	[SerializeField]
	protected MecanimTranslator translator;

	private bool wasUpdatedAfterInit = true;

	public MecanimTranslator Translator => translator;

	protected event UpdateBonesDelegate _BeforeApply;

	protected event UpdateBonesDelegate _UpdateLocal;

	protected event UpdateBonesDelegate _UpdateWorld;

	protected event UpdateBonesDelegate _UpdateComplete;

	public event UpdateBonesDelegate BeforeApply
	{
		add
		{
			_BeforeApply += value;
		}
		remove
		{
			_BeforeApply -= value;
		}
	}

	public event UpdateBonesDelegate UpdateLocal
	{
		add
		{
			_UpdateLocal += value;
		}
		remove
		{
			_UpdateLocal -= value;
		}
	}

	public event UpdateBonesDelegate UpdateWorld
	{
		add
		{
			_UpdateWorld += value;
		}
		remove
		{
			_UpdateWorld -= value;
		}
	}

	public event UpdateBonesDelegate UpdateComplete
	{
		add
		{
			_UpdateComplete += value;
		}
		remove
		{
			_UpdateComplete -= value;
		}
	}

	public override void Initialize(bool overwrite, bool quiet = false)
	{
		if (valid && !overwrite)
		{
			return;
		}
		base.Initialize(overwrite, quiet);
		if (valid)
		{
			if (translator == null)
			{
				translator = new MecanimTranslator();
			}
			translator.Initialize(GetComponent<Animator>(), skeletonDataAsset);
			wasUpdatedAfterInit = false;
		}
	}

	public void Update()
	{
		if (valid)
		{
			wasUpdatedAfterInit = true;
			if (updateMode > UpdateMode.OnlyAnimationStatus)
			{
				ApplyAnimation();
			}
		}
	}

	protected void ApplyAnimation()
	{
		if (this._BeforeApply != null)
		{
			this._BeforeApply(this);
		}
		translator.Apply(skeleton);
		if (this._UpdateLocal != null)
		{
			this._UpdateLocal(this);
		}
		skeleton.UpdateWorldTransform();
		if (this._UpdateWorld != null)
		{
			this._UpdateWorld(this);
			skeleton.UpdateWorldTransform();
		}
		if (this._UpdateComplete != null)
		{
			this._UpdateComplete(this);
		}
	}

	public override void LateUpdate()
	{
		if (!wasUpdatedAfterInit)
		{
			Update();
		}
		base.LateUpdate();
	}

	public override void OnBecameVisible()
	{
		UpdateMode updateMode = base.updateMode;
		base.updateMode = UpdateMode.FullUpdate;
		if (updateMode != UpdateMode.FullUpdate && updateMode != UpdateMode.EverythingExceptMesh)
		{
			Update();
		}
		if (updateMode != UpdateMode.FullUpdate)
		{
			LateUpdate();
		}
	}
}
