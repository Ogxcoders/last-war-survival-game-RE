using Coffee.UIParticleExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace Coffee.UIExtensions;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasRenderer))]
public class UITrail : MaskableGraphic
{
	private Mesh bakedMesh;

	private Mesh resultMesh;

	private TrailRenderer trailRenderer;

	public Material mat;

	protected override void OnEnable()
	{
		base.OnEnable();
		UIParticleUpdater.Register(this);
		bakedMesh = new Mesh();
		bakedMesh.MarkDynamic();
		resultMesh = new Mesh();
		resultMesh.MarkDynamic();
		trailRenderer = GetComponent<TrailRenderer>();
		color = new Color(1f, 1f, 1f, 0f);
		SetMaterialDirty();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		UIParticleUpdater.Unregister(this);
		trailRenderer.Clear();
		Object.Destroy(bakedMesh);
		Object.Destroy(resultMesh);
	}

	protected override void UpdateMaterial()
	{
	}

	public void Refresh()
	{
		Camera camera = BakingCamera.GetCamera(base.canvas);
		trailRenderer.BakeMesh(bakedMesh, camera, useTransform: true);
		CombineInstance[] combine = new CombineInstance[1]
		{
			new CombineInstance
			{
				mesh = bakedMesh,
				transform = base.transform.worldToLocalMatrix
			}
		};
		resultMesh.CombineMeshes(combine, mergeSubMeshes: false, useMatrices: true);
		if (resultMesh.vertexCount > 1)
		{
			Vector3[] vertices = resultMesh.vertices;
			Vector3 vector = (vertices[0] + vertices[1]) / 2f;
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] -= vector;
			}
			resultMesh.SetVertices(vertices);
		}
		base.canvasRenderer.SetMesh(resultMesh);
		base.canvasRenderer.materialCount = 1;
		base.canvasRenderer.SetMaterial(mat, 0);
	}
}
