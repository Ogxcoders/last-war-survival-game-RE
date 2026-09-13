using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RiverGame.Rendering.MaterialPropertyBlockUtilities;
using UnityEngine;

[DefaultExecutionOrder(99999)]
[DisallowMultipleComponent]
public class GPUSkinnedMeshRenderer : MonoBehaviour
{
	public class MaterialPropertyGroupGPUSkin : MaterialPropertyGroup
	{
		private static int ID = MaterialPropertyGroup.IDAllocator++;

		public override int id => ID;

		public MaterialPropertyGroupGPUSkin()
		{
			properties = new IMaterialProperty[1]
			{
				new VectorMaterialProperty(s_AnimGPUTimeId, s_AnimGPUTimeInstancingId, new Vector4(0f, 0f, 0f, 0f))
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateMaterialProperty(Vector4 value)
		{
			(properties[0] as VectorMaterialProperty).value = value;
		}
	}

	public GPUSkinnedMeshInfo info;

	public Animator animator;

	public MeshRenderer meshRenderer;

	public MeshFilter meshFilter;

	private GPUSkinAnimTexData.ClipData _lastClipData;

	private GPUSkinAnimTexData.ClipData _nextClipData;

	private static bool _crossFadeInterruptEnabled = false;

	private int _sortingOrder;

	private static Dictionary<int, Material[]> s_SharedMaterials = new Dictionary<int, Material[]>();

	private Material[] _sharedMaterialsInsts;

	private int _sharedMeshId;

	public SimpleAnimation simpleAnimation;

	private MaterialPropertyGroupGPUSkin materialPropertyGroupGPUSkin = new MaterialPropertyGroupGPUSkin();

	private static MaterialPropertyBlock s_mpb;

	private bool _enableInstancing;

	private int _instancingCountFilter;

	private const bool k_UseMpb = false;

	private bool _batchModeDirty = true;

	private static int s_AnimParamsId = Shader.PropertyToID("_AnimParams");

	private static int s_AnimParamsInstancingId = Shader.PropertyToID("_AnimParamsInstancing");

	private static int s_AnimInterruptParamsId = Shader.PropertyToID("_AnimInterruptParams");

	private static int s_AnimInterruptParamsInstancingId = Shader.PropertyToID("_AnimInterruptParamsInstancing");

	private static int s_AnimGPUTimeId = Shader.PropertyToID("_AnimGPUTime");

	private static int s_AnimGPUTimeInstancingId = Shader.PropertyToID("_AnimGPUTimeInstancing");

	private const bool k_GPUDriveAnimTime = true;

	private const bool k_OnlyUpdateDirtyOnes = true;

	public static Dictionary<int, HashSet<GPUSkinnedMeshRenderer>> s_ActiveRenderers = new Dictionary<int, HashSet<GPUSkinnedMeshRenderer>>();

	public static Dictionary<SimpleAnimation, HashSet<GPUSkinnedMeshRenderer>> s_SimpleAnimRendererMap = new Dictionary<SimpleAnimation, HashSet<GPUSkinnedMeshRenderer>>();

	public static HashSet<GPUSkinnedMeshRenderer> s_AllNonSimpleAnimGpuSkinRenderers = new HashSet<GPUSkinnedMeshRenderer>();

	private static int s_CurrentRenderQueue = 1701;

	private static Dictionary<string, int> s_ShaderRenderQueue = new Dictionary<string, int>();

	private static int s_CurrentSortingOrderAPSCharactorV3 = 3000;

	private static int s_CurrentSortingOrderLastWarBaked = 5000;

	private static int s_CurrentSortingOrderLastWarBuildUnlit = 6000;

	private static int s_CurrentSortingOrderLastWarCharactorV1 = 7000;

	private static int s_CurrentSortingOrderLastWarBakedRamp = 8000;

	private static int s_CurrentSortingOrderAPSBuildPBR = 9000;

	private static int s_CurrentSortingOrderAPSPlaneShadowOnly = 9500;

	private static int s_CurrentSortingOrder = 10000;

	private const int k_InstancingSortingOrderOffset = 12000;

	private static Dictionary<int, int> s_MaterailToSortingOrder = new Dictionary<int, int>();

	public static bool crossFadeEnabled { get; set; } = true;

	public static bool crossFadeInterruptEnabled
	{
		get
		{
			return _crossFadeInterruptEnabled;
		}
		set
		{
			_crossFadeInterruptEnabled = value;
			if (_crossFadeInterruptEnabled)
			{
				Shader.EnableKeyword("_CROSSFADE_INTERRUPT_ON");
			}
			else
			{
				Shader.DisableKeyword("_CROSSFADE_INTERRUPT_ON");
			}
		}
	}

	public int SharedMeshId => _sharedMeshId;

	public MaterialPropertyBlockController materialPropertyBlockController { get; private set; }

	public bool enableInstancing => _enableInstancing;

	public static int activeRendererCount { get; private set; }

	public static int totalRendererCount { get; private set; }

	private void Awake()
	{
		totalRendererCount++;
		if (simpleAnimation == null)
		{
			simpleAnimation = ((animator != null) ? animator.GetComponent<SimpleAnimation>() : null);
		}
		if (meshFilter == null)
		{
			meshFilter = GetComponent<MeshFilter>();
		}
		_sharedMeshId = meshFilter.sharedMesh.GetInstanceID();
		if (meshRenderer == null)
		{
			meshRenderer = GetComponent<MeshRenderer>();
		}
		if (info.animTexData.texHeight != info.animTexData.animTex.height)
		{
			Debug.LogError($"AnimTex height info mismatch: {info.animTexData.texHeight}, {info.animTexData.animTex.height}, for: {base.name}", info.animTexData);
		}
		_sortingOrder = meshRenderer.sortingOrder;
		if (!s_SharedMaterials.TryGetValue(meshFilter.sharedMesh.GetInstanceID(), out var value))
		{
			int num = meshRenderer.sharedMaterials.Length;
			value = new Material[num];
			for (int i = 0; i < num; i++)
			{
				value[i] = UnityEngine.Object.Instantiate(meshRenderer.sharedMaterials[i]);
				value[i].enableInstancing = true;
			}
			s_SharedMaterials.Add(meshFilter.sharedMesh.GetInstanceID(), value);
		}
		meshRenderer.sharedMaterials = value;
		SetMaterialProperty(this, s_AnimGPUTimeId, new Vector4(0f, 0f, 0f, 0f));
	}

	private void OnDestroy()
	{
		totalRendererCount--;
		if (_sharedMaterialsInsts != null)
		{
			Material[] sharedMaterialsInsts = _sharedMaterialsInsts;
			for (int i = 0; i < sharedMaterialsInsts.Length; i++)
			{
				UnityEngine.Object.Destroy(sharedMaterialsInsts[i]);
			}
		}
	}

	private void OnEnable()
	{
		activeRendererCount++;
		if (!s_ActiveRenderers.TryGetValue(_sharedMeshId, out var value))
		{
			value = new HashSet<GPUSkinnedMeshRenderer>();
			s_ActiveRenderers.Add(_sharedMeshId, value);
		}
		value.Add(this);
		if ((bool)simpleAnimation)
		{
			if (!s_SimpleAnimRendererMap.TryGetValue(simpleAnimation, out var value2))
			{
				value2 = new HashSet<GPUSkinnedMeshRenderer>();
				s_SimpleAnimRendererMap.Add(simpleAnimation, value2);
			}
			value2.Add(this);
		}
		else if ((bool)animator && animator.runtimeAnimatorController != null)
		{
			s_AllNonSimpleAnimGpuSkinRenderers.Add(this);
		}
		int count = value.Count;
		_batchModeDirty = true;
		if (count == _instancingCountFilter)
		{
			foreach (GPUSkinnedMeshRenderer item in value)
			{
				if (!item._enableInstancing)
				{
					item._batchModeDirty = true;
					item._enableInstancing = true;
					UpdateBatchMode(item);
					if (item != this)
					{
						item.UpdateGpuRendererInternal();
					}
				}
			}
			return;
		}
		_enableInstancing = count > _instancingCountFilter;
		UpdateBatchMode(this);
	}

	private static void UpdateBatchMode(GPUSkinnedMeshRenderer gsmr)
	{
		if (gsmr._enableInstancing)
		{
			if (s_mpb == null)
			{
				s_mpb = new MaterialPropertyBlock();
			}
			if (gsmr.materialPropertyBlockController == null)
			{
				gsmr.materialPropertyBlockController = new MaterialPropertyBlockController();
				gsmr.materialPropertyBlockController.Add(gsmr.materialPropertyGroupGPUSkin);
			}
			if (s_SharedMaterials.TryGetValue(gsmr.meshFilter.sharedMesh.GetInstanceID(), out var value))
			{
				gsmr.meshRenderer.sharedMaterials = value;
			}
			if (!s_MaterailToSortingOrder.TryGetValue(gsmr.meshRenderer.sharedMaterial.GetInstanceID(), out var value2))
			{
				string text = gsmr.meshRenderer.sharedMaterial.shader.name;
				value2 = (text.Equals("APS/CharactorV3", StringComparison.Ordinal) ? s_CurrentSortingOrderAPSCharactorV3++ : (text.Equals("LastWar/Baked", StringComparison.Ordinal) ? s_CurrentSortingOrderLastWarBaked++ : (text.Equals("LastWar/Build_unlit", StringComparison.Ordinal) ? s_CurrentSortingOrderLastWarBuildUnlit++ : (text.Equals("LastWar/CharactorV1", StringComparison.Ordinal) ? s_CurrentSortingOrderLastWarCharactorV1++ : (text.Equals("LastWar/Baked_Ramp", StringComparison.Ordinal) ? s_CurrentSortingOrderLastWarBakedRamp++ : (text.Equals("APS/Build_PBR", StringComparison.Ordinal) ? s_CurrentSortingOrderAPSBuildPBR++ : ((!text.Equals("APS/PlaneShadowOnly", StringComparison.Ordinal)) ? s_CurrentSortingOrder++ : s_CurrentSortingOrderAPSPlaneShadowOnly++)))))));
				s_MaterailToSortingOrder.Add(gsmr.meshRenderer.sharedMaterial.GetInstanceID(), value2);
			}
			gsmr.meshRenderer.sortingOrder = value2;
			return;
		}
		gsmr.meshRenderer.SetPropertyBlock(null);
		gsmr.meshRenderer.sortingOrder = gsmr._sortingOrder;
		if (gsmr._sharedMaterialsInsts == null)
		{
			gsmr._sharedMaterialsInsts = new Material[gsmr.meshRenderer.sharedMaterials.Length];
			for (int i = 0; i < gsmr.meshRenderer.sharedMaterials.Length; i++)
			{
				gsmr._sharedMaterialsInsts[i] = new Material(gsmr.meshRenderer.sharedMaterials[i]);
			}
			for (int j = 0; j < gsmr._sharedMaterialsInsts.Length; j++)
			{
				Material material = gsmr._sharedMaterialsInsts[j];
				if (!s_ShaderRenderQueue.TryGetValue(material.shader.name, out var value3))
				{
					value3 = s_CurrentRenderQueue++;
					s_ShaderRenderQueue.Add(material.shader.name, value3);
				}
				material.enableInstancing = false;
				material.renderQueue = value3;
			}
		}
		gsmr.meshRenderer.sharedMaterials = gsmr._sharedMaterialsInsts;
	}

	public void ResetMaterial()
	{
		UpdateBatchMode(this);
	}

	private void OnDisable()
	{
		activeRendererCount--;
		if (s_ActiveRenderers.TryGetValue(_sharedMeshId, out var value))
		{
			value.Remove(this);
			if (value.Count == 0)
			{
				s_ActiveRenderers.Remove(_sharedMeshId);
			}
		}
		else
		{
			Debug.LogError("remove gpuSkinRenderer failed:" + base.name + ", use search all to remove");
			foreach (KeyValuePair<int, HashSet<GPUSkinnedMeshRenderer>> s_ActiveRenderer in s_ActiveRenderers)
			{
				s_ActiveRenderer.Value.Remove(this);
			}
		}
		if (simpleAnimation != null && s_SimpleAnimRendererMap.TryGetValue(simpleAnimation, out var value2))
		{
			value2.Remove(this);
			if (value2.Count == 0)
			{
				s_SimpleAnimRendererMap.Remove(simpleAnimation);
			}
		}
		else
		{
			s_AllNonSimpleAnimGpuSkinRenderers.Remove(this);
		}
	}

	public static void UpdateAllActiveGpuSkinRenderer()
	{
		foreach (SimpleAnimation s_DirtySimpleAnim in SimpleAnimation.s_DirtySimpleAnims)
		{
			if (!s_SimpleAnimRendererMap.TryGetValue(s_DirtySimpleAnim, out var value))
			{
				continue;
			}
			foreach (GPUSkinnedMeshRenderer item in value)
			{
				item.UpdateGpuRendererInternal();
			}
		}
		foreach (GPUSkinnedMeshRenderer s_AllNonSimpleAnimGpuSkinRenderer in s_AllNonSimpleAnimGpuSkinRenderers)
		{
			s_AllNonSimpleAnimGpuSkinRenderer.UpdateGpuRendererInternal();
		}
		SimpleAnimation.s_DirtySimpleAnims.Clear();
	}

	private static void SetMaterialProperty(GPUSkinnedMeshRenderer gsmr, int prop1, Vector4 prop1Vec)
	{
		if (gsmr._enableInstancing)
		{
			gsmr.materialPropertyGroupGPUSkin.UpdateMaterialProperty(prop1Vec);
			gsmr.meshRenderer.GetPropertyBlock(s_mpb);
			s_mpb.SetVector(prop1, prop1Vec);
			gsmr.meshRenderer.SetPropertyBlock(s_mpb);
			return;
		}
		Material[] array = gsmr._sharedMaterialsInsts ?? gsmr.meshRenderer.sharedMaterials;
		if (gsmr.meshRenderer.sharedMaterial != array[0])
		{
			gsmr.meshRenderer.sharedMaterial.SetVector(prop1, prop1Vec);
		}
		Material[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].SetVector(prop1, prop1Vec);
		}
	}

	private void UpdateGpuRendererInternal()
	{
		if ((bool)simpleAnimation)
		{
			(bool, string, float, float) tuple;
			if (!simpleAnimation.IsCrossFading() || !crossFadeEnabled)
			{
				(bool, string, float, float) currentState = simpleAnimation.GetCurrentState();
				tuple = currentState;
				if (tuple.Item1 || tuple.Item2 != null || tuple.Item3 != 0f || tuple.Item4 != 0f)
				{
					bool flag = _batchModeDirty;
					if (_nextClipData == null || currentState.Item1)
					{
						_nextClipData = info.GetClipByStateIndex(currentState.Item2);
						flag = true;
					}
					if (flag && _nextClipData != null)
					{
						SetMaterialProperty(this, s_AnimInterruptParamsInstancingId, Vector4.zero);
						SetGpuParam(currentState.Item3);
					}
				}
				return;
			}
			if (!crossFadeInterruptEnabled || !simpleAnimation.InterruptingCrossFading())
			{
				(bool, string, float, float) lastState = simpleAnimation.GetLastState();
				(bool, string, float, float) currentState2 = simpleAnimation.GetCurrentState();
				tuple = lastState;
				if (!tuple.Item1 && !(tuple.Item2 != null) && tuple.Item3 == 0f && tuple.Item4 == 0f)
				{
					return;
				}
				tuple = currentState2;
				if (tuple.Item1 || tuple.Item2 != null || tuple.Item3 != 0f || tuple.Item4 != 0f)
				{
					bool flag2 = _batchModeDirty;
					if (_lastClipData == null || lastState.Item1)
					{
						_lastClipData = info.GetClipByStateIndex(lastState.Item2);
						flag2 = true;
					}
					if (_nextClipData == null || currentState2.Item1)
					{
						_nextClipData = info.GetClipByStateIndex(currentState2.Item2);
						flag2 = true;
					}
					if (flag2 && _lastClipData != null && _nextClipData != null)
					{
						SetMaterialProperty(this, s_AnimInterruptParamsInstancingId, Vector4.zero);
						SetGpuParam(currentState2.Item3, blendClip: true, lastState.Item3, 0.2f, currentState2.Item4, lastState.Item4);
					}
				}
				return;
			}
			(bool, string, float, float, string, float) interruptingState = simpleAnimation.GetInterruptingState();
			(bool, string, float, float) currentState3 = simpleAnimation.GetCurrentState();
			(bool, string, float, float, string, float) tuple2 = interruptingState;
			if (!tuple2.Item1 && !(tuple2.Item2 != null) && tuple2.Item3 == 0f && tuple2.Item4 == 0f && !(tuple2.Item5 != null) && tuple2.Item6 == 0f)
			{
				return;
			}
			tuple = currentState3;
			if (!tuple.Item1 && !(tuple.Item2 != null) && tuple.Item3 == 0f && tuple.Item4 == 0f)
			{
				return;
			}
			bool flag3 = _batchModeDirty;
			if (_lastClipData == null || interruptingState.Item1)
			{
				_lastClipData = info.GetClipByStateIndex(interruptingState.Item5);
				flag3 = true;
			}
			if (_nextClipData == null || currentState3.Item1)
			{
				_nextClipData = info.GetClipByStateIndex(currentState3.Item2);
				flag3 = true;
			}
			if (flag3 && _lastClipData != null && _nextClipData != null)
			{
				int texHeight = info.animTexData.texHeight;
				GPUSkinAnimTexData.ClipData clipByStateIndex = info.GetClipByStateIndex(interruptingState.Item2);
				float item = interruptingState.Item3;
				float num = ((float)clipByStateIndex.startFrameRow + 0.5f) / (float)texHeight;
				if (!clipByStateIndex.isLooping)
				{
					num = 0f - num;
				}
				float z = ((float)clipByStateIndex.totalFrame - 1f) / (float)texHeight;
				float item2 = interruptingState.Item4;
				SetMaterialProperty(this, s_AnimInterruptParamsInstancingId, new Vector4(item, num, z, item2));
				float item3 = interruptingState.Item6;
				float y = ((float)_lastClipData.startFrameRow + 0.5f) / (float)texHeight;
				if (!_lastClipData.isLooping)
				{
					num = 0f - num;
				}
				float z2 = ((float)_lastClipData.totalFrame - 1f) / (float)texHeight;
				float w = _lastClipData.frameRate / (float)texHeight;
				SetMaterialProperty(this, s_AnimParamsInstancingId, new Vector4(item3, y, z2, w));
				Vector4 prop1Vec = ComputeAnimTimeInGpuDrivenMode(currentState3.Item3, currentState3.Item4, _nextClipData, texHeight, blendClip: true, isLastData: false);
				SetMaterialProperty(this, s_AnimGPUTimeInstancingId, prop1Vec);
			}
		}
		else
		{
			if (!(animator.runtimeAnimatorController != null))
			{
				return;
			}
			if (!animator.IsInTransition(0))
			{
				bool flag4 = _batchModeDirty;
				AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
				if (_nextClipData == null || currentAnimatorStateInfo.shortNameHash != _nextClipData.nameHash)
				{
					info.clipMap.TryGetValue(currentAnimatorStateInfo.shortNameHash, out _nextClipData);
					flag4 = true;
				}
				if (flag4 && _nextClipData != null)
				{
					SetGpuParam(currentAnimatorStateInfo.normalizedTime);
				}
				return;
			}
			AnimatorTransitionInfo animatorTransitionInfo = animator.GetAnimatorTransitionInfo(0);
			bool flag5 = _batchModeDirty;
			AnimatorStateInfo currentAnimatorStateInfo2 = animator.GetCurrentAnimatorStateInfo(0);
			if (_lastClipData == null || currentAnimatorStateInfo2.shortNameHash != _lastClipData.nameHash)
			{
				info.clipMap.TryGetValue(currentAnimatorStateInfo2.shortNameHash, out _lastClipData);
				flag5 = true;
			}
			AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(0);
			if (_nextClipData == null || nextAnimatorStateInfo.shortNameHash != _nextClipData.nameHash)
			{
				info.clipMap.TryGetValue(nextAnimatorStateInfo.shortNameHash, out _nextClipData);
				flag5 = true;
			}
			if (flag5 && _lastClipData != null && _nextClipData != null)
			{
				SetGpuParam(currentAnimatorStateInfo2.normalizedTime, blendClip: true, nextAnimatorStateInfo.normalizedTime, animatorTransitionInfo.normalizedTime);
			}
		}
	}

	private void SetGpuParam(float nextNormalizedTime, bool blendClip = false, float lastNormalizedTime = 0f, float transitionNormalizedTime = 0f, float nextNonNormalizedTime = 0f, float lastNonNormalizedTime = 0f)
	{
		Vector4 prop1Vec = ComputeAnimTimeInGpuDrivenMode(nextNormalizedTime, nextNonNormalizedTime, _nextClipData, info.animTexData.texHeight, blendClip, isLastData: false);
		SetMaterialProperty(this, s_AnimGPUTimeInstancingId, prop1Vec);
		if (blendClip)
		{
			Vector4 prop1Vec2 = ComputeAnimTimeInGpuDrivenMode(lastNormalizedTime, lastNonNormalizedTime, _lastClipData, info.animTexData.texHeight, blendClip, isLastData: true);
			SetMaterialProperty(this, s_AnimParamsInstancingId, prop1Vec2);
		}
	}

	private Vector4 ComputeAnimTimeInGpuDrivenMode(float normalizedTime, float nonNormalizedTime, GPUSkinAnimTexData.ClipData clipData, int texHeight, bool blendClip, bool isLastData)
	{
		float x = ((normalizedTime > 0f) ? (Time.timeSinceLevelLoad - (normalizedTime - Mathf.Floor(normalizedTime)) * clipData.duration) : (Time.timeSinceLevelLoad - nonNormalizedTime));
		float num = ((float)clipData.startFrameRow + 0.5f) / (float)texHeight;
		if (!clipData.isLooping)
		{
			num = 0f - num;
		}
		float z = ((float)clipData.totalFrame - 1f) / (float)texHeight;
		float num2 = clipData.frameRate / (float)texHeight;
		if (blendClip && !isLastData)
		{
			num2 = 0f - num2;
		}
		return new Vector4(x, num, z, num2);
	}

	private float ComputeGpuAinmProgressParam(float normalizedTime, GPUSkinAnimTexData.ClipData clipData)
	{
		if (normalizedTime > 1f && clipData.isLooping)
		{
			normalizedTime -= Mathf.Floor(normalizedTime);
		}
		int a = clipData.totalFrame - 1;
		a = Mathf.Max(a, 0);
		return (Mathf.Clamp(normalizedTime * (float)a, 0f, a) + (float)clipData.startFrameRow + 0.5f) * 1f / (float)info.animTexData.texHeight;
	}
}
