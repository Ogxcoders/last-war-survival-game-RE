using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameKit.Base;
using UnityEngine;

public class ProfilerPrimitiveStats : SingletonBehaviour<ProfilerPrimitiveStats>
{
	public struct PrimitiveStats
	{
		public int StaticMeshCount;

		public int StaticMeshTriangles;

		public int SkinnedMeshCount;

		public int SkinnedMeshTriangles;

		public int ParticleTriangles;

		public int ParticleActiveParCount;

		public int ParticleSystemCount;

		public int ParticleMeshTriangles;

		public int ParticleTrailTriangles;

		public int ParticleNormalTriangles;

		public int ParticleNoiseCount;

		public int ParticleMeshCount;
	}

	public struct PrimitiveStatsStatics
	{
		public PrimitiveStats Min;

		public PrimitiveStats Max;

		public PrimitiveStats Avg;

		public PrimitiveStats Current;
	}

	private PrimitiveStats _stats;

	private StringBuilder _dumpBuilder = new StringBuilder();

	private List<PrimitiveStats> _frameStatsBuffer = new List<PrimitiveStats>();

	private float _statsAccumulateTimer;

	private const float StatsUpdateInterval = 1f;

	private PrimitiveStatsStatics _statics;

	private const int MaxBufferFrames = 120;

	public GameObject Target { get; set; }

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
		_frameStatsBuffer.Clear();
	}

	private void Update()
	{
		_stats = CollectPrimitiveInfo(Target);
		if (_frameStatsBuffer.Count >= 120)
		{
			_frameStatsBuffer.RemoveAt(0);
		}
		_frameStatsBuffer.Add(_stats);
		_statsAccumulateTimer += Time.unscaledDeltaTime;
		if (_statsAccumulateTimer >= 1f)
		{
			_statsAccumulateTimer = 0f;
			UpdateStatics();
		}
	}

	private void UpdateStatics()
	{
		int count = _frameStatsBuffer.Count;
		PrimitiveStats min = _frameStatsBuffer[0];
		PrimitiveStats max = _frameStatsBuffer[0];
		PrimitiveStats primitiveStats = default(PrimitiveStats);
		foreach (PrimitiveStats item in _frameStatsBuffer)
		{
			min.StaticMeshCount = Mathf.Min(min.StaticMeshCount, item.StaticMeshCount);
			max.StaticMeshCount = Mathf.Max(max.StaticMeshCount, item.StaticMeshCount);
			primitiveStats.StaticMeshCount += item.StaticMeshCount;
			min.StaticMeshTriangles = Mathf.Min(min.StaticMeshTriangles, item.StaticMeshTriangles);
			max.StaticMeshTriangles = Mathf.Max(max.StaticMeshTriangles, item.StaticMeshTriangles);
			primitiveStats.StaticMeshTriangles += item.StaticMeshTriangles;
			min.SkinnedMeshCount = Mathf.Min(min.SkinnedMeshCount, item.SkinnedMeshCount);
			max.SkinnedMeshCount = Mathf.Max(max.SkinnedMeshCount, item.SkinnedMeshCount);
			primitiveStats.SkinnedMeshCount += item.SkinnedMeshCount;
			min.SkinnedMeshTriangles = Mathf.Min(min.SkinnedMeshTriangles, item.SkinnedMeshTriangles);
			max.SkinnedMeshTriangles = Mathf.Max(max.SkinnedMeshTriangles, item.SkinnedMeshTriangles);
			primitiveStats.SkinnedMeshTriangles += item.SkinnedMeshTriangles;
			min.ParticleTriangles = Mathf.Min(min.ParticleTriangles, item.ParticleTriangles);
			max.ParticleTriangles = Mathf.Max(max.ParticleTriangles, item.ParticleTriangles);
			primitiveStats.ParticleTriangles += item.ParticleTriangles;
			min.ParticleActiveParCount = Mathf.Min(min.ParticleActiveParCount, item.ParticleActiveParCount);
			max.ParticleActiveParCount = Mathf.Max(max.ParticleActiveParCount, item.ParticleActiveParCount);
			primitiveStats.ParticleActiveParCount += item.ParticleActiveParCount;
			min.ParticleSystemCount = Mathf.Min(min.ParticleSystemCount, item.ParticleSystemCount);
			max.ParticleSystemCount = Mathf.Max(max.ParticleSystemCount, item.ParticleSystemCount);
			primitiveStats.ParticleSystemCount += item.ParticleSystemCount;
			min.ParticleMeshTriangles = Mathf.Min(min.ParticleMeshTriangles, item.ParticleMeshTriangles);
			max.ParticleMeshTriangles = Mathf.Max(max.ParticleMeshTriangles, item.ParticleMeshTriangles);
			primitiveStats.ParticleMeshTriangles += item.ParticleMeshTriangles;
			min.ParticleTrailTriangles = Mathf.Min(min.ParticleTrailTriangles, item.ParticleTrailTriangles);
			max.ParticleTrailTriangles = Mathf.Max(max.ParticleTrailTriangles, item.ParticleTrailTriangles);
			primitiveStats.ParticleTrailTriangles += item.ParticleTrailTriangles;
			min.ParticleNormalTriangles = Mathf.Min(min.ParticleNormalTriangles, item.ParticleNormalTriangles);
			max.ParticleNormalTriangles = Mathf.Max(max.ParticleNormalTriangles, item.ParticleNormalTriangles);
			primitiveStats.ParticleNormalTriangles += item.ParticleNormalTriangles;
			min.ParticleNoiseCount = Mathf.Min(min.ParticleNoiseCount, item.ParticleNoiseCount);
			max.ParticleNoiseCount = Mathf.Max(max.ParticleNoiseCount, item.ParticleNoiseCount);
			primitiveStats.ParticleNoiseCount += item.ParticleNoiseCount;
			min.ParticleMeshCount = Mathf.Min(min.ParticleMeshCount, item.ParticleMeshCount);
			max.ParticleMeshCount = Mathf.Max(max.ParticleMeshCount, item.ParticleMeshCount);
			primitiveStats.ParticleMeshCount += item.ParticleMeshCount;
		}
		PrimitiveStats avg = new PrimitiveStats
		{
			StaticMeshCount = primitiveStats.StaticMeshCount / count,
			StaticMeshTriangles = primitiveStats.StaticMeshTriangles / count,
			SkinnedMeshCount = primitiveStats.SkinnedMeshCount / count,
			SkinnedMeshTriangles = primitiveStats.SkinnedMeshTriangles / count,
			ParticleTriangles = primitiveStats.ParticleTriangles / count,
			ParticleActiveParCount = primitiveStats.ParticleActiveParCount / count,
			ParticleSystemCount = primitiveStats.ParticleSystemCount / count,
			ParticleMeshTriangles = primitiveStats.ParticleMeshTriangles / count,
			ParticleTrailTriangles = primitiveStats.ParticleTrailTriangles / count,
			ParticleNormalTriangles = primitiveStats.ParticleNormalTriangles / count,
			ParticleNoiseCount = primitiveStats.ParticleNoiseCount / count,
			ParticleMeshCount = primitiveStats.ParticleMeshCount / count
		};
		_statics = new PrimitiveStatsStatics
		{
			Min = min,
			Max = max,
			Avg = avg,
			Current = _frameStatsBuffer[_frameStatsBuffer.Count - 1]
		};
	}

	public static int GetMeshTriangleCount(Mesh mesh, Material[] materials, int layer)
	{
		int num = 0;
		if (mesh != null)
		{
			int num2 = 1;
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				num += (int)mesh.GetIndexCount(i) * num2 / 3;
			}
		}
		return num;
	}

	public static PrimitiveStats CollectPrimitiveInfo(GameObject target)
	{
		PrimitiveStats result = default(PrimitiveStats);
		Renderer[] array = ((target == null) ? Object.FindObjectsOfType<Renderer>() : target.GetComponentsInChildren<Renderer>());
		foreach (Renderer renderer in array)
		{
			if (IsVisibleInMainCamera(renderer))
			{
				SkinnedMeshRenderer component2;
				if (renderer.TryGetComponent<MeshFilter>(out var component) && component.sharedMesh != null)
				{
					Mesh sharedMesh = component.sharedMesh;
					Material[] sharedMaterials = renderer.sharedMaterials;
					result.StaticMeshTriangles += GetMeshTriangleCount(sharedMesh, sharedMaterials, renderer.gameObject.layer);
					result.StaticMeshCount += sharedMesh.subMeshCount;
				}
				else if (renderer.TryGetComponent<SkinnedMeshRenderer>(out component2) && component2.sharedMesh != null)
				{
					Mesh sharedMesh2 = component2.sharedMesh;
					Material[] sharedMaterials2 = component2.sharedMaterials;
					result.SkinnedMeshTriangles += GetMeshTriangleCount(sharedMesh2, sharedMaterials2, renderer.gameObject.layer);
					result.SkinnedMeshCount += sharedMesh2.subMeshCount;
				}
			}
		}
		ParticleSystem[] array2 = ((target == null) ? Object.FindObjectsOfType<ParticleSystem>() : target.GetComponentsInChildren<ParticleSystem>());
		foreach (ParticleSystem particleSystem in array2)
		{
			if (particleSystem.gameObject.activeInHierarchy && particleSystem.TryGetComponent<ParticleSystemRenderer>(out var component3) && IsVisibleInMainCamera(component3))
			{
				int particleCount = particleSystem.particleCount;
				result.ParticleSystemCount++;
				result.ParticleActiveParCount += particleCount;
				if (particleSystem.noise.enabled)
				{
					result.ParticleNoiseCount++;
				}
				ParticleSystem.TrailModule trails = particleSystem.trails;
				if (trails.enabled)
				{
					float constant = trails.lifetime.constant;
					int num = Mathf.FloorToInt(particleSystem.main.startSpeed.constant * constant * 0.5f);
					int num2 = particleCount * num * 2;
					result.ParticleTriangles += num2;
					result.ParticleTrailTriangles += num2;
				}
				else if (component3.renderMode == ParticleSystemRenderMode.Mesh && component3.mesh != null)
				{
					Material[] sharedMaterials3 = component3.sharedMaterials;
					int num3 = GetMeshTriangleCount(component3.mesh, sharedMaterials3, component3.gameObject.layer) * particleCount;
					result.ParticleTriangles += num3;
					result.ParticleMeshTriangles += num3;
					result.ParticleMeshCount++;
				}
				else
				{
					result.ParticleTriangles += 2 * particleCount;
					result.ParticleNormalTriangles += 2 * particleCount;
				}
			}
		}
		return result;
	}

	private static bool IsVisibleInMainCamera(Renderer renderer)
	{
		if (renderer == null || !renderer.isVisible || !renderer.gameObject.activeInHierarchy || renderer.sharedMaterials == null)
		{
			return false;
		}
		Camera main = Camera.main;
		if (main != null && !GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(main), renderer.bounds))
		{
			return false;
		}
		return true;
	}

	public PrimitiveStats GetStats()
	{
		return _stats;
	}

	public PrimitiveStatsStatics GetStatsStatics()
	{
		return _statics;
	}

	public string DumpToString()
	{
		PrimitiveStatsStatics statsStatics = GetStatsStatics();
		_dumpBuilder.Clear();
		int current = statsStatics.Current.StaticMeshTriangles + statsStatics.Current.SkinnedMeshTriangles + statsStatics.Current.ParticleTriangles;
		int min = statsStatics.Min.StaticMeshTriangles + statsStatics.Min.SkinnedMeshTriangles + statsStatics.Min.ParticleTriangles;
		int max = statsStatics.Max.StaticMeshTriangles + statsStatics.Max.SkinnedMeshTriangles + statsStatics.Max.ParticleTriangles;
		int avg = statsStatics.Avg.StaticMeshTriangles + statsStatics.Avg.SkinnedMeshTriangles + statsStatics.Avg.ParticleTriangles;
		_dumpBuilder.AppendLine("Primitive Statistics (Current / Min / Max / Avg):");
		AppendStatLine("   Total Triangles", current, min, max, avg);
		_dumpBuilder.AppendLine();
		AppendStatLine("Static Meshes", statsStatics.Current.StaticMeshCount, statsStatics.Min.StaticMeshCount, statsStatics.Max.StaticMeshCount, statsStatics.Avg.StaticMeshCount);
		AppendStatLine("   Static Triangles", statsStatics.Current.StaticMeshTriangles, statsStatics.Min.StaticMeshTriangles, statsStatics.Max.StaticMeshTriangles, statsStatics.Avg.StaticMeshTriangles);
		AppendStatLine("Skinned Meshes", statsStatics.Current.SkinnedMeshCount, statsStatics.Min.SkinnedMeshCount, statsStatics.Max.SkinnedMeshCount, statsStatics.Avg.SkinnedMeshCount);
		AppendStatLine("   Skinned Triangles", statsStatics.Current.SkinnedMeshTriangles, statsStatics.Min.SkinnedMeshTriangles, statsStatics.Max.SkinnedMeshTriangles, statsStatics.Avg.SkinnedMeshTriangles);
		_dumpBuilder.AppendLine("Particles:");
		AppendStatLine("   Systems Count", statsStatics.Current.ParticleSystemCount, statsStatics.Min.ParticleSystemCount, statsStatics.Max.ParticleSystemCount, statsStatics.Avg.ParticleSystemCount);
		AppendStatLine("   Active Par Count", statsStatics.Current.ParticleActiveParCount, statsStatics.Min.ParticleActiveParCount, statsStatics.Max.ParticleActiveParCount, statsStatics.Avg.ParticleActiveParCount);
		AppendStatLine("   Noise Count", statsStatics.Current.ParticleNoiseCount, statsStatics.Min.ParticleNoiseCount, statsStatics.Max.ParticleNoiseCount, statsStatics.Avg.ParticleNoiseCount);
		AppendStatLine("   Mesh Count", statsStatics.Current.ParticleMeshCount, statsStatics.Min.ParticleMeshCount, statsStatics.Max.ParticleMeshCount, statsStatics.Avg.ParticleMeshCount);
		AppendStatLine("   Triangles Count", statsStatics.Current.ParticleTriangles, statsStatics.Min.ParticleTriangles, statsStatics.Max.ParticleTriangles, statsStatics.Avg.ParticleTriangles);
		AppendStatLine("       Normal", statsStatics.Current.ParticleNormalTriangles, statsStatics.Min.ParticleNormalTriangles, statsStatics.Max.ParticleNormalTriangles, statsStatics.Avg.ParticleNormalTriangles);
		AppendStatLine("       Mesh", statsStatics.Current.ParticleMeshTriangles, statsStatics.Min.ParticleMeshTriangles, statsStatics.Max.ParticleMeshTriangles, statsStatics.Avg.ParticleMeshTriangles);
		AppendStatLine("       Trail", statsStatics.Current.ParticleTrailTriangles, statsStatics.Min.ParticleTrailTriangles, statsStatics.Max.ParticleTrailTriangles, statsStatics.Avg.ParticleTrailTriangles);
		return _dumpBuilder.ToString();
		void AppendStatLine(string label, int num, int num2, int num3, int num4)
		{
			_dumpBuilder.AppendFormat("{0}: {1} (min: {2}, max: {3}, avg: {4})\n", label, num, num2, num3, num4);
		}
	}

	public void DumpToConsole()
	{
		Log.Info(DumpToString());
	}
}
