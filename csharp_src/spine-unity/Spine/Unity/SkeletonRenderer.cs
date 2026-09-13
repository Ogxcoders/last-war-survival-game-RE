using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Spine.Unity;

[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))]
[DisallowMultipleComponent]
[HelpURL("http://esotericsoftware.com/spine-unity#SkeletonRenderer-Component")]
public class SkeletonRenderer : MonoBehaviour, ISkeletonComponent, ISpineComponent, IHasSkeletonDataAsset
{
	[Serializable]
	public class SpriteMaskInteractionMaterials
	{
		public Material[] materialsMaskDisabled = new Material[0];

		public Material[] materialsInsideMask = new Material[0];

		public Material[] materialsOutsideMask = new Material[0];

		public bool AnyMaterialCreated
		{
			get
			{
				if (materialsMaskDisabled.Length == 0 && materialsInsideMask.Length == 0)
				{
					return materialsOutsideMask.Length != 0;
				}
				return true;
			}
		}
	}

	public delegate void InstructionDelegate(SkeletonRendererInstruction instruction);

	public delegate void SkeletonRendererDelegate(SkeletonRenderer skeletonRenderer);

	public SkeletonDataAsset skeletonDataAsset;

	[SpineSkin("", "", true, false, true)]
	public string initialSkinName;

	public bool initialFlipX;

	public bool initialFlipY;

	protected UpdateMode updateMode = UpdateMode.FullUpdate;

	public UpdateMode updateWhenInvisible = UpdateMode.FullUpdate;

	[FormerlySerializedAs("submeshSeparators")]
	[SerializeField]
	[SpineSlot("", "", false, true, false)]
	protected string[] separatorSlotNames = new string[0];

	[NonSerialized]
	public readonly List<Slot> separatorSlots = new List<Slot>();

	[Range(-0.1f, 0f)]
	public float zSpacing;

	public bool useClipping = true;

	public bool immutableTriangles;

	public bool pmaVertexColors = true;

	public bool clearStateOnDisable;

	public bool tintBlack;

	public bool singleSubmesh;

	public bool fixDrawOrder;

	[FormerlySerializedAs("calculateNormals")]
	public bool addNormals;

	public bool calculateTangents;

	public SpriteMaskInteraction maskInteraction;

	public SpriteMaskInteractionMaterials maskMaterials = new SpriteMaskInteractionMaterials();

	public static readonly int STENCIL_COMP_PARAM_ID = Shader.PropertyToID("_StencilComp");

	public const CompareFunction STENCIL_COMP_MASKINTERACTION_NONE = CompareFunction.Always;

	public const CompareFunction STENCIL_COMP_MASKINTERACTION_VISIBLE_INSIDE = CompareFunction.LessEqual;

	public const CompareFunction STENCIL_COMP_MASKINTERACTION_VISIBLE_OUTSIDE = CompareFunction.Greater;

	public bool disableRenderingOnOverride = true;

	[NonSerialized]
	private readonly Dictionary<Material, Material> customMaterialOverride = new Dictionary<Material, Material>();

	[NonSerialized]
	private readonly Dictionary<Slot, Material> customSlotMaterials = new Dictionary<Slot, Material>();

	[NonSerialized]
	private readonly SkeletonRendererInstruction currentInstructions = new SkeletonRendererInstruction();

	private readonly MeshGenerator meshGenerator = new MeshGenerator();

	[NonSerialized]
	private readonly MeshRendererBuffers rendererBuffers = new MeshRendererBuffers();

	private MeshRenderer meshRenderer;

	private MeshFilter meshFilter;

	[NonSerialized]
	public bool valid;

	[NonSerialized]
	public Skeleton skeleton;

	private MaterialPropertyBlock reusedPropertyBlock;

	public static readonly int SUBMESH_DUMMY_PARAM_ID = Shader.PropertyToID("_Submesh");

	public UpdateMode UpdateMode
	{
		get
		{
			return updateMode;
		}
		set
		{
			updateMode = value;
		}
	}

	public Dictionary<Material, Material> CustomMaterialOverride => customMaterialOverride;

	public Dictionary<Slot, Material> CustomSlotMaterials => customSlotMaterials;

	public Skeleton Skeleton
	{
		get
		{
			Initialize(overwrite: false);
			return skeleton;
		}
	}

	public SkeletonDataAsset SkeletonDataAsset => skeletonDataAsset;

	private event InstructionDelegate generateMeshOverride;

	public event InstructionDelegate GenerateMeshOverride
	{
		add
		{
			generateMeshOverride += value;
			if (disableRenderingOnOverride && this.generateMeshOverride != null)
			{
				Initialize(overwrite: false);
				if ((bool)meshRenderer)
				{
					meshRenderer.enabled = false;
				}
			}
		}
		remove
		{
			generateMeshOverride -= value;
			if (disableRenderingOnOverride && this.generateMeshOverride == null)
			{
				Initialize(overwrite: false);
				if ((bool)meshRenderer)
				{
					meshRenderer.enabled = true;
				}
			}
		}
	}

	public event MeshGeneratorDelegate OnPostProcessVertices;

	public event SkeletonRendererDelegate OnRebuild;

	public event SkeletonRendererDelegate OnMeshAndMaterialsUpdated;

	public static T NewSpineGameObject<T>(SkeletonDataAsset skeletonDataAsset, bool quiet = false) where T : SkeletonRenderer
	{
		return AddSpineComponent<T>(new GameObject("New Spine GameObject"), skeletonDataAsset, quiet);
	}

	public static T AddSpineComponent<T>(GameObject gameObject, SkeletonDataAsset skeletonDataAsset, bool quiet = false) where T : SkeletonRenderer
	{
		T val = gameObject.AddComponent<T>();
		if (skeletonDataAsset != null)
		{
			val.skeletonDataAsset = skeletonDataAsset;
			val.Initialize(overwrite: false, quiet);
		}
		return val;
	}

	public void SetMeshSettings(MeshGenerator.Settings settings)
	{
		calculateTangents = settings.calculateTangents;
		immutableTriangles = settings.immutableTriangles;
		pmaVertexColors = settings.pmaVertexColors;
		tintBlack = settings.tintBlack;
		useClipping = settings.useClipping;
		zSpacing = settings.zSpacing;
		meshGenerator.settings = settings;
	}

	public virtual void Awake()
	{
		Initialize(overwrite: false);
		updateMode = updateWhenInvisible;
	}

	private void OnDisable()
	{
		if (clearStateOnDisable && valid)
		{
			ClearState();
		}
	}

	private void OnDestroy()
	{
		rendererBuffers.Dispose();
		valid = false;
	}

	public virtual void ClearState()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		if (component != null)
		{
			component.sharedMesh = null;
		}
		currentInstructions.Clear();
		if (skeleton != null)
		{
			skeleton.SetToSetupPose();
		}
	}

	public void EnsureMeshGeneratorCapacity(int minimumVertexCount)
	{
		meshGenerator.EnsureVertexCapacity(minimumVertexCount);
	}

	public virtual void Initialize(bool overwrite, bool quiet = false)
	{
		if (valid && !overwrite)
		{
			return;
		}
		currentInstructions.Clear();
		rendererBuffers.Clear();
		meshGenerator.Begin();
		skeleton = null;
		valid = false;
		if (skeletonDataAsset == null)
		{
			return;
		}
		SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(quiet: false);
		if (skeletonData != null)
		{
			valid = true;
			meshFilter = GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				meshFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			meshRenderer = GetComponent<MeshRenderer>();
			rendererBuffers.Initialize();
			skeleton = new Skeleton(skeletonData)
			{
				ScaleX = ((!initialFlipX) ? 1 : (-1)),
				ScaleY = ((!initialFlipY) ? 1 : (-1))
			};
			if (!string.IsNullOrEmpty(initialSkinName) && !string.Equals(initialSkinName, "default", StringComparison.Ordinal))
			{
				skeleton.SetSkin(initialSkinName);
			}
			separatorSlots.Clear();
			for (int i = 0; i < separatorSlotNames.Length; i++)
			{
				separatorSlots.Add(skeleton.FindSlot(separatorSlotNames[i]));
			}
			UpdateMode updateMode = this.updateMode;
			this.updateMode = UpdateMode.FullUpdate;
			skeleton.UpdateWorldTransform();
			LateUpdate();
			this.updateMode = updateMode;
			if (this.OnRebuild != null)
			{
				this.OnRebuild(this);
			}
		}
	}

	public virtual void LateUpdate()
	{
		if (valid && updateMode == UpdateMode.FullUpdate)
		{
			LateUpdateMesh();
		}
	}

	public virtual void LateUpdateMesh()
	{
		bool flag = this.generateMeshOverride != null;
		if ((!meshRenderer || !meshRenderer.enabled) && !flag)
		{
			return;
		}
		SkeletonRendererInstruction skeletonRendererInstruction = currentInstructions;
		ExposedList<SubmeshInstruction> submeshInstructions = skeletonRendererInstruction.submeshInstructions;
		MeshRendererBuffers.SmartMesh nextMesh = rendererBuffers.GetNextMesh();
		bool flag2;
		if (singleSubmesh)
		{
			MeshGenerator.GenerateSingleSubmeshInstruction(skeletonRendererInstruction, skeleton, skeletonDataAsset.atlasAssets[0].PrimaryMaterial);
			if (customMaterialOverride.Count > 0)
			{
				MeshGenerator.TryReplaceMaterials(submeshInstructions, customMaterialOverride);
			}
			meshGenerator.settings = new MeshGenerator.Settings
			{
				pmaVertexColors = pmaVertexColors,
				zSpacing = zSpacing,
				useClipping = useClipping,
				tintBlack = tintBlack,
				calculateTangents = calculateTangents,
				addNormals = addNormals
			};
			meshGenerator.Begin();
			flag2 = SkeletonRendererInstruction.GeometryNotEqual(skeletonRendererInstruction, nextMesh.instructionUsed);
			if (skeletonRendererInstruction.hasActiveClipping)
			{
				meshGenerator.AddSubmesh(submeshInstructions.Items[0], flag2);
			}
			else
			{
				meshGenerator.BuildMeshWithArrays(skeletonRendererInstruction, flag2);
			}
		}
		else
		{
			MeshGenerator.GenerateSkeletonRendererInstruction(skeletonRendererInstruction, skeleton, customSlotMaterials, separatorSlots, flag, immutableTriangles);
			if (customMaterialOverride.Count > 0)
			{
				MeshGenerator.TryReplaceMaterials(submeshInstructions, customMaterialOverride);
			}
			if (flag)
			{
				this.generateMeshOverride(skeletonRendererInstruction);
				if (disableRenderingOnOverride)
				{
					return;
				}
			}
			flag2 = SkeletonRendererInstruction.GeometryNotEqual(skeletonRendererInstruction, nextMesh.instructionUsed);
			meshGenerator.settings = new MeshGenerator.Settings
			{
				pmaVertexColors = pmaVertexColors,
				zSpacing = zSpacing,
				useClipping = useClipping,
				tintBlack = tintBlack,
				calculateTangents = calculateTangents,
				addNormals = addNormals
			};
			meshGenerator.Begin();
			if (skeletonRendererInstruction.hasActiveClipping)
			{
				meshGenerator.BuildMesh(skeletonRendererInstruction, flag2);
			}
			else
			{
				meshGenerator.BuildMeshWithArrays(skeletonRendererInstruction, flag2);
			}
		}
		if (this.OnPostProcessVertices != null)
		{
			this.OnPostProcessVertices(meshGenerator.Buffers);
		}
		Mesh mesh = nextMesh.mesh;
		meshGenerator.FillVertexData(mesh);
		rendererBuffers.UpdateSharedMaterials(submeshInstructions);
		bool flag3 = rendererBuffers.MaterialsChangedInLastUpdate();
		if (flag2)
		{
			meshGenerator.FillTriangles(mesh);
			meshRenderer.sharedMaterials = rendererBuffers.GetUpdatedSharedMaterialsArray();
		}
		else if (flag3)
		{
			meshRenderer.sharedMaterials = rendererBuffers.GetUpdatedSharedMaterialsArray();
		}
		if (flag3 && maskMaterials.AnyMaterialCreated)
		{
			maskMaterials = new SpriteMaskInteractionMaterials();
		}
		meshGenerator.FillLateVertexData(mesh);
		if ((bool)meshFilter)
		{
			meshFilter.sharedMesh = mesh;
		}
		nextMesh.instructionUsed.Set(skeletonRendererInstruction);
		if (meshRenderer != null)
		{
			AssignSpriteMaskMaterials();
		}
		if (fixDrawOrder && meshRenderer.sharedMaterials.Length > 2)
		{
			SetMaterialSettingsToFixDrawOrder();
		}
		if (this.OnMeshAndMaterialsUpdated != null)
		{
			this.OnMeshAndMaterialsUpdated(this);
		}
	}

	public virtual void OnBecameVisible()
	{
		UpdateMode num = updateMode;
		updateMode = UpdateMode.FullUpdate;
		if (num != UpdateMode.FullUpdate)
		{
			LateUpdate();
		}
	}

	public void OnBecameInvisible()
	{
		updateMode = updateWhenInvisible;
	}

	public void FindAndApplySeparatorSlots(string startsWith, bool clearExistingSeparators = true, bool updateStringArray = false)
	{
		if (!string.IsNullOrEmpty(startsWith))
		{
			FindAndApplySeparatorSlots((string slotName) => slotName.StartsWith(startsWith), clearExistingSeparators, updateStringArray);
		}
	}

	public void FindAndApplySeparatorSlots(Func<string, bool> slotNamePredicate, bool clearExistingSeparators = true, bool updateStringArray = false)
	{
		if (slotNamePredicate == null || !valid)
		{
			return;
		}
		if (clearExistingSeparators)
		{
			separatorSlots.Clear();
		}
		foreach (Slot slot in skeleton.Slots)
		{
			if (slotNamePredicate(slot.Data.Name))
			{
				separatorSlots.Add(slot);
			}
		}
		if (!updateStringArray)
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (Slot slot2 in skeleton.Slots)
		{
			string text = slot2.Data.Name;
			if (slotNamePredicate(text))
			{
				list.Add(text);
			}
		}
		if (!clearExistingSeparators)
		{
			string[] array = separatorSlotNames;
			foreach (string item in array)
			{
				list.Add(item);
			}
		}
		separatorSlotNames = list.ToArray();
	}

	public void ReapplySeparatorSlotNames()
	{
		if (!valid)
		{
			return;
		}
		separatorSlots.Clear();
		int i = 0;
		for (int num = separatorSlotNames.Length; i < num; i++)
		{
			Slot slot = skeleton.FindSlot(separatorSlotNames[i]);
			if (slot != null)
			{
				separatorSlots.Add(slot);
			}
		}
	}

	private void AssignSpriteMaskMaterials()
	{
		if (Application.isPlaying && maskInteraction != SpriteMaskInteraction.None && maskMaterials.materialsMaskDisabled.Length == 0)
		{
			maskMaterials.materialsMaskDisabled = meshRenderer.sharedMaterials;
		}
		if (maskMaterials.materialsMaskDisabled.Length != 0 && maskMaterials.materialsMaskDisabled[0] != null && maskInteraction == SpriteMaskInteraction.None)
		{
			meshRenderer.materials = maskMaterials.materialsMaskDisabled;
		}
		else if (maskInteraction == SpriteMaskInteraction.VisibleInsideMask)
		{
			if ((maskMaterials.materialsInsideMask.Length != 0 && !(maskMaterials.materialsInsideMask[0] == null)) || InitSpriteMaskMaterialsInsideMask())
			{
				meshRenderer.materials = maskMaterials.materialsInsideMask;
			}
		}
		else if (maskInteraction == SpriteMaskInteraction.VisibleOutsideMask && ((maskMaterials.materialsOutsideMask.Length != 0 && !(maskMaterials.materialsOutsideMask[0] == null)) || InitSpriteMaskMaterialsOutsideMask()))
		{
			meshRenderer.materials = maskMaterials.materialsOutsideMask;
		}
	}

	private bool InitSpriteMaskMaterialsInsideMask()
	{
		return InitSpriteMaskMaterialsForMaskType(CompareFunction.LessEqual, ref maskMaterials.materialsInsideMask);
	}

	private bool InitSpriteMaskMaterialsOutsideMask()
	{
		return InitSpriteMaskMaterialsForMaskType(CompareFunction.Greater, ref maskMaterials.materialsOutsideMask);
	}

	private bool InitSpriteMaskMaterialsForMaskType(CompareFunction maskFunction, ref Material[] materialsToFill)
	{
		Material[] materialsMaskDisabled = maskMaterials.materialsMaskDisabled;
		materialsToFill = new Material[materialsMaskDisabled.Length];
		for (int i = 0; i < materialsMaskDisabled.Length; i++)
		{
			Material material = new Material(materialsMaskDisabled[i]);
			material.SetFloat(STENCIL_COMP_PARAM_ID, (float)maskFunction);
			materialsToFill[i] = material;
		}
		return true;
	}

	private void SetMaterialSettingsToFixDrawOrder()
	{
		if (reusedPropertyBlock == null)
		{
			reusedPropertyBlock = new MaterialPropertyBlock();
		}
		bool flag = meshRenderer.HasPropertyBlock();
		if (flag)
		{
			meshRenderer.GetPropertyBlock(reusedPropertyBlock);
		}
		for (int i = 0; i < meshRenderer.sharedMaterials.Length; i++)
		{
			if ((bool)meshRenderer.sharedMaterials[i])
			{
				if (!flag)
				{
					meshRenderer.GetPropertyBlock(reusedPropertyBlock, i);
				}
				reusedPropertyBlock.SetFloat(SUBMESH_DUMMY_PARAM_ID, i);
				meshRenderer.SetPropertyBlock(reusedPropertyBlock, i);
				meshRenderer.sharedMaterials[i].enableInstancing = false;
			}
		}
	}
}
