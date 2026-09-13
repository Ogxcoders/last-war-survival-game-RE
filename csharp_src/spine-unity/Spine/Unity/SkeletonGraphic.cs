using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Spine.Unity;

[ExecuteAlways]
[RequireComponent(typeof(CanvasRenderer), typeof(RectTransform))]
[DisallowMultipleComponent]
[AddComponentMenu("Spine/SkeletonGraphic (Unity UI Canvas)")]
[HelpURL("http://esotericsoftware.com/spine-unity#SkeletonGraphic-Component")]
public class SkeletonGraphic : MaskableGraphic, ISkeletonComponent, ISpineComponent, IAnimationStateComponent, ISkeletonAnimation, IHasSkeletonDataAsset
{
	public delegate void SkeletonRendererDelegate(SkeletonGraphic skeletonGraphic);

	public SkeletonDataAsset skeletonDataAsset;

	public Material additiveMaterial;

	public Material multiplyMaterial;

	public Material screenMaterial;

	[SpineSkin("", "skeletonDataAsset", true, false, true)]
	public string initialSkinName;

	public bool initialFlipX;

	public bool initialFlipY;

	[SpineAnimation("", "skeletonDataAsset", true, false)]
	public string startingAnimation;

	public bool startingLoop;

	public float timeScale = 1f;

	public bool freeze;

	protected UpdateMode updateMode = UpdateMode.FullUpdate;

	public UpdateMode updateWhenInvisible = UpdateMode.FullUpdate;

	public bool unscaledTime;

	public bool allowMultipleCanvasRenderers;

	public List<CanvasRenderer> canvasRenderers = new List<CanvasRenderer>();

	protected List<SkeletonSubmeshGraphic> submeshGraphics = new List<SkeletonSubmeshGraphic>();

	protected int usedRenderersCount;

	public const string SeparatorPartGameObjectName = "Part";

	[SerializeField]
	[SpineSlot("", "", false, true, false)]
	protected string[] separatorSlotNames = new string[0];

	[NonSerialized]
	public readonly List<Slot> separatorSlots = new List<Slot>();

	public bool enableSeparatorSlots;

	[SerializeField]
	protected List<Transform> separatorParts = new List<Transform>();

	public bool updateSeparatorPartLocation = true;

	private bool wasUpdatedAfterInit = true;

	private Texture baseTexture;

	[NonSerialized]
	private readonly Dictionary<Texture, Texture> customTextureOverride = new Dictionary<Texture, Texture>();

	[NonSerialized]
	private readonly Dictionary<Texture, Material> customMaterialOverride = new Dictionary<Texture, Material>();

	private Texture overrideTexture;

	protected Skeleton skeleton;

	protected AnimationState state;

	[SerializeField]
	protected MeshGenerator meshGenerator = new MeshGenerator();

	private DoubleBuffered<MeshRendererBuffers.SmartMesh> meshBuffers;

	private SkeletonRendererInstruction currentInstructions = new SkeletonRendererInstruction();

	private readonly ExposedList<Mesh> meshes = new ExposedList<Mesh>();

	public SkeletonDataAsset SkeletonDataAsset => skeletonDataAsset;

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

	public List<Transform> SeparatorParts => separatorParts;

	public Dictionary<Texture, Texture> CustomTextureOverride => customTextureOverride;

	public Dictionary<Texture, Material> CustomMaterialOverride => customMaterialOverride;

	public Texture OverrideTexture
	{
		get
		{
			return overrideTexture;
		}
		set
		{
			overrideTexture = value;
			base.canvasRenderer.SetTexture(mainTexture);
		}
	}

	public override Texture mainTexture
	{
		get
		{
			if (overrideTexture != null)
			{
				return overrideTexture;
			}
			return baseTexture;
		}
	}

	public Skeleton Skeleton
	{
		get
		{
			Initialize(overwrite: false);
			return skeleton;
		}
		set
		{
			skeleton = value;
		}
	}

	public SkeletonData SkeletonData
	{
		get
		{
			if (skeleton != null)
			{
				return skeleton.Data;
			}
			return null;
		}
	}

	public bool IsValid => skeleton != null;

	public AnimationState AnimationState
	{
		get
		{
			Initialize(overwrite: false);
			return state;
		}
	}

	public MeshGenerator MeshGenerator => meshGenerator;

	public event SkeletonRendererDelegate OnRebuild;

	public event SkeletonRendererDelegate OnMeshAndMaterialsUpdated;

	public event UpdateBonesDelegate BeforeApply;

	public event UpdateBonesDelegate UpdateLocal;

	public event UpdateBonesDelegate UpdateWorld;

	public event UpdateBonesDelegate UpdateComplete;

	public event MeshGeneratorDelegate OnPostProcessVertices;

	public static SkeletonGraphic NewSkeletonGraphicGameObject(SkeletonDataAsset skeletonDataAsset, Transform parent, Material material)
	{
		SkeletonGraphic skeletonGraphic = AddSkeletonGraphicComponent(new GameObject("New Spine GameObject"), skeletonDataAsset, material);
		if (parent != null)
		{
			skeletonGraphic.transform.SetParent(parent, worldPositionStays: false);
		}
		return skeletonGraphic;
	}

	public static SkeletonGraphic AddSkeletonGraphicComponent(GameObject gameObject, SkeletonDataAsset skeletonDataAsset, Material material)
	{
		SkeletonGraphic skeletonGraphic = gameObject.AddComponent<SkeletonGraphic>();
		if (skeletonDataAsset != null)
		{
			skeletonGraphic.material = material;
			skeletonGraphic.skeletonDataAsset = skeletonDataAsset;
			skeletonGraphic.Initialize(overwrite: false);
		}
		return skeletonGraphic;
	}

	protected override void Awake()
	{
		base.Awake();
		base.onCullStateChanged.AddListener(OnCullStateChanged);
		SyncSubmeshGraphicsWithCanvasRenderers();
		if (!IsValid)
		{
			Initialize(overwrite: false);
			Rebuild(CanvasUpdate.PreRender);
		}
	}

	protected override void OnDestroy()
	{
		Clear();
		base.OnDestroy();
	}

	public override void Rebuild(CanvasUpdate update)
	{
		base.Rebuild(update);
		if (!base.canvasRenderer.cull)
		{
			if (update == CanvasUpdate.PreRender)
			{
				UpdateMesh(keepRendererCount: true);
			}
			if (allowMultipleCanvasRenderers)
			{
				base.canvasRenderer.Clear();
			}
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		foreach (CanvasRenderer canvasRenderer in canvasRenderers)
		{
			canvasRenderer.Clear();
		}
	}

	public virtual void Update()
	{
		if (!freeze)
		{
			Update(unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
		}
	}

	public virtual void Update(float deltaTime)
	{
		if (!IsValid)
		{
			return;
		}
		wasUpdatedAfterInit = true;
		if (updateMode >= UpdateMode.OnlyAnimationStatus)
		{
			UpdateAnimationStatus(deltaTime);
			if (updateMode == UpdateMode.OnlyAnimationStatus)
			{
				state.ApplyEventTimelinesOnly(skeleton, issueEvents: false);
			}
			else
			{
				ApplyAnimation();
			}
		}
	}

	protected void SyncSubmeshGraphicsWithCanvasRenderers()
	{
		submeshGraphics.Clear();
		foreach (CanvasRenderer canvasRenderer in canvasRenderers)
		{
			SkeletonSubmeshGraphic skeletonSubmeshGraphic = canvasRenderer.GetComponent<SkeletonSubmeshGraphic>();
			if (skeletonSubmeshGraphic == null)
			{
				skeletonSubmeshGraphic = canvasRenderer.gameObject.AddComponent<SkeletonSubmeshGraphic>();
				skeletonSubmeshGraphic.maskable = base.maskable;
				skeletonSubmeshGraphic.raycastTarget = false;
			}
			submeshGraphics.Add(skeletonSubmeshGraphic);
		}
	}

	protected void UpdateAnimationStatus(float deltaTime)
	{
		deltaTime *= timeScale;
		skeleton.Update(deltaTime);
		state.Update(deltaTime);
	}

	protected void ApplyAnimation()
	{
		if (this.BeforeApply != null)
		{
			this.BeforeApply(this);
		}
		if (updateMode != UpdateMode.OnlyEventTimelines)
		{
			state.Apply(skeleton);
		}
		else
		{
			state.ApplyEventTimelinesOnly(skeleton);
		}
		if (this.UpdateLocal != null)
		{
			this.UpdateLocal(this);
		}
		skeleton.UpdateWorldTransform();
		if (this.UpdateWorld != null)
		{
			this.UpdateWorld(this);
			skeleton.UpdateWorldTransform();
		}
		if (this.UpdateComplete != null)
		{
			this.UpdateComplete(this);
		}
	}

	public void LateUpdate()
	{
		if (!wasUpdatedAfterInit)
		{
			Update(0f);
		}
		if (!freeze && updateMode == UpdateMode.FullUpdate)
		{
			UpdateMesh();
		}
	}

	protected void OnCullStateChanged(bool culled)
	{
		if (culled)
		{
			OnBecameInvisible();
		}
		else
		{
			OnBecameVisible();
		}
	}

	public void OnBecameVisible()
	{
		updateMode = UpdateMode.FullUpdate;
	}

	public void OnBecameInvisible()
	{
		updateMode = updateWhenInvisible;
	}

	public void ReapplySeparatorSlotNames()
	{
		if (!IsValid)
		{
			return;
		}
		separatorSlots.Clear();
		int i = 0;
		for (int num = separatorSlotNames.Length; i < num; i++)
		{
			string text = separatorSlotNames[i];
			if (!(text == ""))
			{
				Slot slot = skeleton.FindSlot(text);
				if (slot != null)
				{
					separatorSlots.Add(slot);
				}
			}
		}
		UpdateSeparatorPartParents();
	}

	public Mesh GetLastMesh()
	{
		return meshBuffers.GetCurrent().mesh;
	}

	public bool MatchRectTransformWithBounds()
	{
		if (!wasUpdatedAfterInit)
		{
			Update(0f);
		}
		UpdateMesh();
		if (!allowMultipleCanvasRenderers)
		{
			return MatchRectTransformSingleRenderer();
		}
		return MatchRectTransformMultipleRenderers();
	}

	protected bool MatchRectTransformSingleRenderer()
	{
		Mesh lastMesh = GetLastMesh();
		if (lastMesh == null)
		{
			return false;
		}
		if (lastMesh.vertexCount == 0)
		{
			base.rectTransform.sizeDelta = new Vector2(50f, 50f);
			base.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			return false;
		}
		lastMesh.RecalculateBounds();
		SetRectTransformBounds(lastMesh.bounds);
		return true;
	}

	protected bool MatchRectTransformMultipleRenderers()
	{
		bool flag = false;
		Bounds rectTransformBounds = default(Bounds);
		for (int i = 0; i < canvasRenderers.Count; i++)
		{
			if (!canvasRenderers[i].gameObject.activeSelf)
			{
				continue;
			}
			Mesh mesh = meshes.Items[i];
			if (!(mesh == null) && mesh.vertexCount != 0)
			{
				mesh.RecalculateBounds();
				Bounds bounds = mesh.bounds;
				if (flag)
				{
					rectTransformBounds.Encapsulate(bounds);
					continue;
				}
				flag = true;
				rectTransformBounds = bounds;
			}
		}
		if (!flag)
		{
			base.rectTransform.sizeDelta = new Vector2(50f, 50f);
			base.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			return false;
		}
		SetRectTransformBounds(rectTransformBounds);
		return true;
	}

	private void SetRectTransformBounds(Bounds combinedBounds)
	{
		Vector3 size = combinedBounds.size;
		Vector3 center = combinedBounds.center;
		Vector2 pivot = new Vector2(0.5f - center.x / size.x, 0.5f - center.y / size.y);
		base.rectTransform.sizeDelta = size;
		base.rectTransform.pivot = pivot;
		foreach (SkeletonSubmeshGraphic submeshGraphic in submeshGraphics)
		{
			submeshGraphic.rectTransform.sizeDelta = size;
			submeshGraphic.rectTransform.pivot = pivot;
		}
	}

	public void Clear()
	{
		skeleton = null;
		base.canvasRenderer.Clear();
		for (int i = 0; i < canvasRenderers.Count; i++)
		{
			canvasRenderers[i].Clear();
		}
		DestroyMeshes();
		DisposeMeshBuffers();
	}

	public void TrimRenderers()
	{
		List<CanvasRenderer> list = new List<CanvasRenderer>();
		foreach (CanvasRenderer canvasRenderer in canvasRenderers)
		{
			if (canvasRenderer.gameObject.activeSelf)
			{
				list.Add(canvasRenderer);
			}
			else if (Application.isEditor && !Application.isPlaying)
			{
				UnityEngine.Object.DestroyImmediate(canvasRenderer.gameObject);
			}
			else
			{
				UnityEngine.Object.Destroy(canvasRenderer.gameObject);
			}
		}
		canvasRenderers = list;
		SyncSubmeshGraphicsWithCanvasRenderers();
	}

	public void Initialize(bool overwrite)
	{
		if ((IsValid && !overwrite) || skeletonDataAsset == null)
		{
			return;
		}
		SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(quiet: false);
		if (skeletonData == null || skeletonDataAsset.atlasAssets.Length == 0 || skeletonDataAsset.atlasAssets[0].MaterialCount <= 0)
		{
			return;
		}
		state = new AnimationState(skeletonDataAsset.GetAnimationStateData());
		if (state == null)
		{
			Clear();
			return;
		}
		skeleton = new Skeleton(skeletonData)
		{
			ScaleX = ((!initialFlipX) ? 1 : (-1)),
			ScaleY = ((!initialFlipY) ? 1 : (-1))
		};
		InitMeshBuffers();
		baseTexture = skeletonDataAsset.atlasAssets[0].PrimaryMaterial.mainTexture;
		base.canvasRenderer.SetTexture(mainTexture);
		if (!string.IsNullOrEmpty(initialSkinName))
		{
			skeleton.SetSkin(initialSkinName);
		}
		separatorSlots.Clear();
		for (int i = 0; i < separatorSlotNames.Length; i++)
		{
			separatorSlots.Add(skeleton.FindSlot(separatorSlotNames[i]));
		}
		wasUpdatedAfterInit = false;
		if (!string.IsNullOrEmpty(startingAnimation))
		{
			Animation animation = skeletonDataAsset.GetSkeletonData(quiet: false).FindAnimation(startingAnimation);
			if (animation != null)
			{
				state.SetAnimation(0, animation, startingLoop);
			}
		}
		if (this.OnRebuild != null)
		{
			this.OnRebuild(this);
		}
	}

	public void UpdateMesh(bool keepRendererCount = false)
	{
		if (IsValid)
		{
			skeleton.SetColor(color);
			SkeletonRendererInstruction skeletonRendererInstruction = currentInstructions;
			if (!allowMultipleCanvasRenderers)
			{
				UpdateMeshSingleCanvasRenderer();
			}
			else
			{
				UpdateMeshMultipleCanvasRenderers(skeletonRendererInstruction, keepRendererCount);
			}
			if (this.OnMeshAndMaterialsUpdated != null)
			{
				this.OnMeshAndMaterialsUpdated(this);
			}
		}
	}

	public bool HasMultipleSubmeshInstructions()
	{
		if (!IsValid)
		{
			return false;
		}
		return MeshGenerator.RequiresMultipleSubmeshesByDrawOrder(skeleton);
	}

	protected void InitMeshBuffers()
	{
		if (meshBuffers != null)
		{
			meshBuffers.GetNext().Clear();
			meshBuffers.GetNext().Clear();
		}
		else
		{
			meshBuffers = new DoubleBuffered<MeshRendererBuffers.SmartMesh>();
		}
	}

	protected void DisposeMeshBuffers()
	{
		if (meshBuffers != null)
		{
			meshBuffers.GetNext().Dispose();
			meshBuffers.GetNext().Dispose();
			meshBuffers = null;
		}
	}

	protected void UpdateMeshSingleCanvasRenderer()
	{
		if (canvasRenderers.Count > 0)
		{
			DisableUnusedCanvasRenderers(0);
		}
		MeshRendererBuffers.SmartMesh next = meshBuffers.GetNext();
		MeshGenerator.GenerateSingleSubmeshInstruction(currentInstructions, skeleton, null);
		bool flag = SkeletonRendererInstruction.GeometryNotEqual(currentInstructions, next.instructionUsed);
		meshGenerator.Begin();
		if (currentInstructions.hasActiveClipping && currentInstructions.submeshInstructions.Count > 0)
		{
			meshGenerator.AddSubmesh(currentInstructions.submeshInstructions.Items[0], flag);
		}
		else
		{
			meshGenerator.BuildMeshWithArrays(currentInstructions, flag);
		}
		if (base.canvas != null)
		{
			meshGenerator.ScaleVertexData(base.canvas.referencePixelsPerUnit);
		}
		if (this.OnPostProcessVertices != null)
		{
			this.OnPostProcessVertices(meshGenerator.Buffers);
		}
		Mesh mesh = next.mesh;
		meshGenerator.FillVertexData(mesh);
		if (flag)
		{
			meshGenerator.FillTriangles(mesh);
		}
		meshGenerator.FillLateVertexData(mesh);
		base.canvasRenderer.SetMesh(mesh);
		next.instructionUsed.Set(currentInstructions);
		if (currentInstructions.submeshInstructions.Count > 0)
		{
			Material material = currentInstructions.submeshInstructions.Items[0].material;
			if (material != null && baseTexture != material.mainTexture)
			{
				baseTexture = material.mainTexture;
				if (overrideTexture == null)
				{
					base.canvasRenderer.SetTexture(mainTexture);
				}
			}
		}
		usedRenderersCount = 0;
	}

	protected void UpdateMeshMultipleCanvasRenderers(SkeletonRendererInstruction currentInstructions, bool keepRendererCount)
	{
		MeshGenerator.GenerateSkeletonRendererInstruction(currentInstructions, skeleton, null, enableSeparatorSlots ? separatorSlots : null, enableSeparatorSlots && separatorSlots.Count > 0);
		int count = currentInstructions.submeshInstructions.Count;
		if (keepRendererCount && count != usedRenderersCount)
		{
			return;
		}
		EnsureCanvasRendererCount(count);
		EnsureMeshesCount(count);
		EnsureSeparatorPartCount();
		Canvas canvas = base.canvas;
		float scale = ((canvas == null) ? 100f : canvas.referencePixelsPerUnit);
		Mesh[] items = meshes.Items;
		bool flag = customMaterialOverride.Count == 0 && customTextureOverride.Count == 0;
		int num = 0;
		Transform transform = ((separatorSlots.Count == 0) ? base.transform : separatorParts[0]);
		if (updateSeparatorPartLocation)
		{
			for (int i = 0; i < separatorParts.Count; i++)
			{
				separatorParts[i].position = base.transform.position;
				separatorParts[i].rotation = base.transform.rotation;
			}
		}
		BlendModeMaterials blendModeMaterials = skeletonDataAsset.blendModeMaterials;
		bool requiresBlendModeMaterials = blendModeMaterials.RequiresBlendModeMaterials;
		bool cullTransparentMesh = base.canvasRenderer.cullTransparentMesh;
		bool pmaVertexColors = meshGenerator.settings.pmaVertexColors;
		int num2 = 0;
		for (int j = 0; j < count; j++)
		{
			SubmeshInstruction instruction = currentInstructions.submeshInstructions.Items[j];
			meshGenerator.Begin();
			meshGenerator.AddSubmesh(instruction);
			Mesh mesh = items[j];
			meshGenerator.ScaleVertexData(scale);
			if (this.OnPostProcessVertices != null)
			{
				this.OnPostProcessVertices(meshGenerator.Buffers);
			}
			meshGenerator.FillVertexData(mesh);
			meshGenerator.FillTriangles(mesh);
			meshGenerator.FillLateVertexData(mesh);
			Material material = instruction.material;
			CanvasRenderer canvasRenderer = canvasRenderers[j];
			if (j >= usedRenderersCount)
			{
				canvasRenderer.gameObject.SetActive(value: true);
			}
			canvasRenderer.SetMesh(mesh);
			canvasRenderer.materialCount = 1;
			if (canvasRenderer.transform.parent != transform.transform)
			{
				canvasRenderer.transform.SetParent(transform.transform, worldPositionStays: false);
				canvasRenderer.transform.localPosition = Vector3.zero;
			}
			canvasRenderer.transform.SetSiblingIndex(num2++);
			if (instruction.forceSeparate)
			{
				num2 = 0;
				transform = separatorParts[++num];
			}
			SkeletonSubmeshGraphic skeletonSubmeshGraphic = submeshGraphics[j];
			if (flag)
			{
				Texture texture = material.mainTexture;
				if (!requiresBlendModeMaterials)
				{
					canvasRenderer.SetMaterial(materialForRendering, texture);
					continue;
				}
				bool flag2 = true;
				BlendMode blendMode = blendModeMaterials.BlendModeForMaterial(material);
				Material baseMaterial = materialForRendering;
				if (blendMode == BlendMode.Normal)
				{
					if (instruction.hasPMAAdditiveSlot)
					{
						flag2 = false;
					}
				}
				else if (blendMode == BlendMode.Additive)
				{
					if (pmaVertexColors)
					{
						flag2 = false;
					}
					else if ((bool)additiveMaterial)
					{
						baseMaterial = additiveMaterial;
					}
				}
				else if (blendMode == BlendMode.Multiply && (bool)multiplyMaterial)
				{
					baseMaterial = multiplyMaterial;
				}
				else if (blendMode == BlendMode.Screen && (bool)screenMaterial)
				{
					baseMaterial = screenMaterial;
				}
				baseMaterial = skeletonSubmeshGraphic.GetModifiedMaterial(baseMaterial);
				canvasRenderer.SetMaterial(baseMaterial, texture);
				canvasRenderer.cullTransparentMesh = flag2 && cullTransparentMesh;
			}
			else
			{
				Texture texture2 = material.mainTexture;
				if (!customMaterialOverride.TryGetValue(texture2, out var value))
				{
					value = this.material;
				}
				if (!customTextureOverride.TryGetValue(texture2, out var value2))
				{
					value2 = texture2;
				}
				value = skeletonSubmeshGraphic.GetModifiedMaterial(value);
				canvasRenderer.SetMaterial(value, value2);
			}
		}
		DisableUnusedCanvasRenderers(count);
		usedRenderersCount = count;
	}

	protected void EnsureCanvasRendererCount(int targetCount)
	{
		for (int i = canvasRenderers.Count; i < targetCount; i++)
		{
			GameObject obj = new GameObject($"Renderer{i}", typeof(RectTransform));
			obj.transform.SetParent(base.transform, worldPositionStays: false);
			obj.transform.localPosition = Vector3.zero;
			CanvasRenderer item = obj.AddComponent<CanvasRenderer>();
			canvasRenderers.Add(item);
			SkeletonSubmeshGraphic skeletonSubmeshGraphic = obj.AddComponent<SkeletonSubmeshGraphic>();
			skeletonSubmeshGraphic.maskable = base.maskable;
			skeletonSubmeshGraphic.raycastTarget = false;
			submeshGraphics.Add(skeletonSubmeshGraphic);
		}
	}

	protected void DisableUnusedCanvasRenderers(int usedCount)
	{
		for (int i = usedCount; i < canvasRenderers.Count; i++)
		{
			canvasRenderers[i].Clear();
			canvasRenderers[i].gameObject.SetActive(value: false);
		}
	}

	protected void EnsureMeshesCount(int targetCount)
	{
		int count = meshes.Count;
		meshes.EnsureCapacity(targetCount);
		for (int i = count; i < targetCount; i++)
		{
			meshes.Add(SpineMesh.NewSkeletonMesh());
		}
	}

	protected void DestroyMeshes()
	{
		foreach (Mesh mesh in meshes)
		{
			UnityEngine.Object.Destroy(mesh);
		}
		meshes.Clear();
	}

	protected void EnsureSeparatorPartCount()
	{
		int num = separatorSlots.Count + 1;
		if (num != 1)
		{
			for (int i = separatorParts.Count; i < num; i++)
			{
				GameObject gameObject = new GameObject(string.Format("{0}[{1}]", "Part", i), typeof(RectTransform));
				gameObject.transform.SetParent(base.transform, worldPositionStays: false);
				gameObject.transform.localPosition = Vector3.zero;
				separatorParts.Add(gameObject.transform);
			}
		}
	}

	protected void UpdateSeparatorPartParents()
	{
		int num = separatorSlots.Count + 1;
		if (num == 1)
		{
			num = 0;
			for (int i = 0; i < canvasRenderers.Count; i++)
			{
				CanvasRenderer canvasRenderer = canvasRenderers[i];
				if (canvasRenderer.transform.parent.name.Contains("Part"))
				{
					canvasRenderer.transform.SetParent(base.transform, worldPositionStays: false);
					canvasRenderer.transform.localPosition = Vector3.zero;
				}
			}
		}
		for (int j = 0; j < separatorParts.Count; j++)
		{
			bool active = j < num;
			separatorParts[j].gameObject.SetActive(active);
		}
	}
}
