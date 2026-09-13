using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Spine.Unity;

[ExecuteAlways]
[HelpURL("http://esotericsoftware.com/spine-unity#SkeletonRenderSeparator")]
public class SkeletonRenderSeparator : MonoBehaviour
{
	public const int DefaultSortingOrderIncrement = 5;

	[SerializeField]
	protected SkeletonRenderer skeletonRenderer;

	private MeshRenderer mainMeshRenderer;

	public bool copyPropertyBlock = true;

	[Tooltip("Copies MeshRenderer flags into each parts renderer")]
	public bool copyMeshRendererFlags = true;

	public List<SkeletonPartsRenderer> partsRenderers = new List<SkeletonPartsRenderer>();

	private MaterialPropertyBlock copiedBlock;

	public SkeletonRenderer SkeletonRenderer
	{
		get
		{
			return skeletonRenderer;
		}
		set
		{
			if (skeletonRenderer != null)
			{
				skeletonRenderer.GenerateMeshOverride -= HandleRender;
			}
			skeletonRenderer = value;
			if (value == null)
			{
				base.enabled = false;
			}
		}
	}

	public event SkeletonRenderer.SkeletonRendererDelegate OnMeshAndMaterialsUpdated;

	public static SkeletonRenderSeparator AddToSkeletonRenderer(SkeletonRenderer skeletonRenderer, int sortingLayerID = 0, int extraPartsRenderers = 0, int sortingOrderIncrement = 5, int baseSortingOrder = 0, bool addMinimumPartsRenderers = true)
	{
		if (skeletonRenderer == null)
		{
			Debug.Log("Tried to add SkeletonRenderSeparator to a null SkeletonRenderer reference.");
			return null;
		}
		SkeletonRenderSeparator skeletonRenderSeparator = skeletonRenderer.gameObject.AddComponent<SkeletonRenderSeparator>();
		skeletonRenderSeparator.skeletonRenderer = skeletonRenderer;
		skeletonRenderer.Initialize(overwrite: false);
		int num = extraPartsRenderers;
		if (addMinimumPartsRenderers)
		{
			num = extraPartsRenderers + skeletonRenderer.separatorSlots.Count + 1;
		}
		Transform parent = skeletonRenderer.transform;
		List<SkeletonPartsRenderer> list = skeletonRenderSeparator.partsRenderers;
		for (int i = 0; i < num; i++)
		{
			SkeletonPartsRenderer skeletonPartsRenderer = SkeletonPartsRenderer.NewPartsRendererGameObject(parent, i.ToString());
			MeshRenderer meshRenderer = skeletonPartsRenderer.MeshRenderer;
			meshRenderer.sortingLayerID = sortingLayerID;
			meshRenderer.sortingOrder = baseSortingOrder + i * sortingOrderIncrement;
			list.Add(skeletonPartsRenderer);
		}
		skeletonRenderSeparator.OnEnable();
		return skeletonRenderSeparator;
	}

	public SkeletonPartsRenderer AddPartsRenderer(int sortingOrderIncrement = 5, string name = null)
	{
		int sortingLayerID = 0;
		int sortingOrder = 0;
		if (partsRenderers.Count > 0)
		{
			MeshRenderer meshRenderer = partsRenderers[partsRenderers.Count - 1].MeshRenderer;
			sortingLayerID = meshRenderer.sortingLayerID;
			sortingOrder = meshRenderer.sortingOrder + sortingOrderIncrement;
		}
		if (string.IsNullOrEmpty(name))
		{
			name = partsRenderers.Count.ToString();
		}
		SkeletonPartsRenderer skeletonPartsRenderer = SkeletonPartsRenderer.NewPartsRendererGameObject(skeletonRenderer.transform, name);
		partsRenderers.Add(skeletonPartsRenderer);
		MeshRenderer meshRenderer2 = skeletonPartsRenderer.MeshRenderer;
		meshRenderer2.sortingLayerID = sortingLayerID;
		meshRenderer2.sortingOrder = sortingOrder;
		return skeletonPartsRenderer;
	}

	public void OnEnable()
	{
		if (skeletonRenderer == null)
		{
			return;
		}
		if (copiedBlock == null)
		{
			copiedBlock = new MaterialPropertyBlock();
		}
		mainMeshRenderer = skeletonRenderer.GetComponent<MeshRenderer>();
		skeletonRenderer.GenerateMeshOverride -= HandleRender;
		skeletonRenderer.GenerateMeshOverride += HandleRender;
		if (!copyMeshRendererFlags)
		{
			return;
		}
		LightProbeUsage lightProbeUsage = mainMeshRenderer.lightProbeUsage;
		bool receiveShadows = mainMeshRenderer.receiveShadows;
		ReflectionProbeUsage reflectionProbeUsage = mainMeshRenderer.reflectionProbeUsage;
		ShadowCastingMode shadowCastingMode = mainMeshRenderer.shadowCastingMode;
		MotionVectorGenerationMode motionVectorGenerationMode = mainMeshRenderer.motionVectorGenerationMode;
		Transform probeAnchor = mainMeshRenderer.probeAnchor;
		for (int i = 0; i < partsRenderers.Count; i++)
		{
			SkeletonPartsRenderer skeletonPartsRenderer = partsRenderers[i];
			if (!(skeletonPartsRenderer == null))
			{
				MeshRenderer meshRenderer = skeletonPartsRenderer.MeshRenderer;
				meshRenderer.lightProbeUsage = lightProbeUsage;
				meshRenderer.receiveShadows = receiveShadows;
				meshRenderer.reflectionProbeUsage = reflectionProbeUsage;
				meshRenderer.shadowCastingMode = shadowCastingMode;
				meshRenderer.motionVectorGenerationMode = motionVectorGenerationMode;
				meshRenderer.probeAnchor = probeAnchor;
			}
		}
	}

	public void OnDisable()
	{
		if (skeletonRenderer == null)
		{
			return;
		}
		skeletonRenderer.GenerateMeshOverride -= HandleRender;
		skeletonRenderer.LateUpdate();
		foreach (SkeletonPartsRenderer partsRenderer in partsRenderers)
		{
			if (partsRenderer != null)
			{
				partsRenderer.ClearMesh();
			}
		}
	}

	private void HandleRender(SkeletonRendererInstruction instruction)
	{
		int count = partsRenderers.Count;
		if (count <= 0)
		{
			return;
		}
		if (copyPropertyBlock)
		{
			mainMeshRenderer.GetPropertyBlock(copiedBlock);
		}
		MeshGenerator.Settings settings = new MeshGenerator.Settings
		{
			addNormals = skeletonRenderer.addNormals,
			calculateTangents = skeletonRenderer.calculateTangents,
			immutableTriangles = false,
			pmaVertexColors = skeletonRenderer.pmaVertexColors,
			tintBlack = skeletonRenderer.tintBlack,
			useClipping = true,
			zSpacing = skeletonRenderer.zSpacing
		};
		ExposedList<SubmeshInstruction> submeshInstructions = instruction.submeshInstructions;
		SubmeshInstruction[] items = submeshInstructions.Items;
		int num = submeshInstructions.Count - 1;
		int i = 0;
		SkeletonPartsRenderer skeletonPartsRenderer = partsRenderers[i];
		int j = 0;
		int startSubmesh = 0;
		for (; j <= num; j++)
		{
			if (!(skeletonPartsRenderer == null) && (items[j].forceSeparate || j == num))
			{
				skeletonPartsRenderer.MeshGenerator.settings = settings;
				if (copyPropertyBlock)
				{
					skeletonPartsRenderer.SetPropertyBlock(copiedBlock);
				}
				skeletonPartsRenderer.RenderParts(instruction.submeshInstructions, startSubmesh, j + 1);
				startSubmesh = j + 1;
				i++;
				if (i >= count)
				{
					break;
				}
				skeletonPartsRenderer = partsRenderers[i];
			}
		}
		if (this.OnMeshAndMaterialsUpdated != null)
		{
			this.OnMeshAndMaterialsUpdated(skeletonRenderer);
		}
		for (; i < count; i++)
		{
			skeletonPartsRenderer = partsRenderers[i];
			if (skeletonPartsRenderer != null)
			{
				partsRenderers[i].ClearMesh();
			}
		}
	}
}
