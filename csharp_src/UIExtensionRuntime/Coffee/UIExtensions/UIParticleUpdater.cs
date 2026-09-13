using System;
using System.Collections.Generic;
using Coffee.UIParticleExtensions;
using UnityEngine;

namespace Coffee.UIExtensions;

internal static class UIParticleUpdater
{
	private static readonly List<UIParticle> s_ActiveParticles = new List<UIParticle>();

	private static MaterialPropertyBlock s_Mpb;

	private static ParticleSystem.Particle[] s_Particles = new ParticleSystem.Particle[2048];

	private static readonly List<UITrail> s_ActiveTrails = new List<UITrail>();

	private const float FRAMERATE_20_ELAPSED = 0.05f;

	private const int FRAMERATE_MODE_MAX = 4;

	private static int s_Mod = 1;

	private static int s_Remainder = 0;

	public static void Register(UIParticle particle)
	{
		if ((bool)particle)
		{
			s_ActiveParticles.Add(particle);
		}
	}

	public static void Unregister(UIParticle particle)
	{
		if ((bool)particle)
		{
			s_ActiveParticles.Remove(particle);
		}
	}

	public static void Register(UITrail trail)
	{
		if ((bool)trail)
		{
			s_ActiveTrails.Add(trail);
		}
	}

	public static void Unregister(UITrail trail)
	{
		if ((bool)trail)
		{
			s_ActiveTrails.Remove(trail);
		}
	}

	[RuntimeInitializeOnLoadMethod]
	private static void InitializeOnLoad()
	{
		MeshHelper.Init();
		MeshPool.Init();
		CombineInstanceArrayPool.Init();
		Canvas.willRenderCanvases -= Refresh;
		Canvas.willRenderCanvases += Refresh;
	}

	private static void Refresh()
	{
		ShareMeshPool.OnFrameBegin();
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		if (UIParticle.ENABLE_STEP_BAKE_MESH && unscaledDeltaTime > 0.05f)
		{
			if (s_Remainder < 0)
			{
				s_Mod = Math.Min(Mathf.CeilToInt(unscaledDeltaTime / 0.05f) + 1, 4);
				s_Remainder = s_Mod - 1;
			}
		}
		else
		{
			s_Mod = 1;
			s_Remainder = 0;
		}
		for (int i = 0; i < s_ActiveParticles.Count; i++)
		{
			try
			{
				if (i % s_Mod == s_Remainder)
				{
					Refresh(s_ActiveParticles[i]);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
		for (int j = 0; j < s_ActiveTrails.Count; j++)
		{
			try
			{
				if (j % s_Mod == s_Remainder)
				{
					Refresh(s_ActiveTrails[j]);
				}
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2);
			}
		}
		s_Remainder--;
	}

	private static void Refresh(UITrail trail)
	{
		trail.Refresh();
	}

	private static void _BakeMesh(UIParticle particle, bool share)
	{
		Mesh self = BakeMesh(particle, share);
		if (QualitySettings.activeColorSpace == ColorSpace.Linear)
		{
			self.ModifyColorSpaceToLinear();
		}
	}

	private static void Refresh(UIParticle particle)
	{
		if (!particle || !particle.bakedMesh || !particle.canvas || !particle.canvasRenderer)
		{
			return;
		}
		ModifyScale(particle);
		ShareMeshItem shareMeshItem = null;
		if (!string.IsNullOrEmpty(particle.shareMeshTag))
		{
			shareMeshItem = ShareMeshPool.GetMesh(particle.shareMeshTag);
			if (shareMeshItem == null)
			{
				_BakeMesh(particle, share: true);
				shareMeshItem = ShareMeshPool.GetMesh(particle.shareMeshTag);
			}
			particle.activeMeshIndices = shareMeshItem.meshIndices;
		}
		else
		{
			_BakeMesh(particle, share: false);
		}
		particle.canvasRenderer.SetMesh((shareMeshItem == null) ? particle.bakedMesh : shareMeshItem.mesh);
		particle.UpdateMaterialProperties();
	}

	private static void ModifyScale(UIParticle particle)
	{
		if (particle.ignoreCanvasScaler && (bool)particle.canvas)
		{
			Vector3 localScale = particle.canvas.rootCanvas.transform.localScale;
			Vector3 vector = new Vector3(Mathf.Approximately(localScale.x, 0f) ? 1f : (1f / localScale.x), Mathf.Approximately(localScale.y, 0f) ? 1f : (1f / localScale.y), Mathf.Approximately(localScale.z, 0f) ? 1f : (1f / localScale.z));
			Transform transform = particle.transform;
			if (!Mathf.Approximately((transform.localScale - vector).sqrMagnitude, 0f))
			{
				transform.localScale = vector;
			}
		}
	}

	private static Matrix4x4 GetScaledMatrix(ParticleSystem particle)
	{
		Transform transform = particle.transform;
		ParticleSystem.MainModule main = particle.main;
		ParticleSystemSimulationSpace particleSystemSimulationSpace = main.simulationSpace;
		if (particleSystemSimulationSpace == ParticleSystemSimulationSpace.Custom && !main.customSimulationSpace)
		{
			particleSystemSimulationSpace = ParticleSystemSimulationSpace.Local;
		}
		return particleSystemSimulationSpace switch
		{
			ParticleSystemSimulationSpace.Local => Matrix4x4.Rotate(transform.rotation).inverse * Matrix4x4.Scale(transform.lossyScale).inverse, 
			ParticleSystemSimulationSpace.World => transform.worldToLocalMatrix, 
			ParticleSystemSimulationSpace.Custom => transform.worldToLocalMatrix * Matrix4x4.Translate(main.customSimulationSpace.position), 
			_ => Matrix4x4.identity, 
		};
	}

	private static Mesh BakeMesh(UIParticle particle, bool share)
	{
		MeshHelper.Clear();
		particle.bakedMesh.Clear(keepVertexLayout: false);
		Camera camera = BakingCamera.GetCamera(particle.canvas);
		Transform transform = particle.transform;
		Matrix4x4 matrix4x = Matrix4x4.Rotate(transform.rotation).inverse * Matrix4x4.Scale(transform.lossyScale).inverse;
		Vector3 vector = (particle.ignoreCanvasScaler ? Vector3.Scale(particle.canvas.rootCanvas.transform.localScale, particle.scale3D) : particle.scale3D);
		Matrix4x4 matrix4x2 = Matrix4x4.Scale(vector);
		Vector3 position = particle.transform.position;
		Vector3 vector2 = position - particle.cachedPosition;
		vector2.x *= 1f - 1f / Mathf.Max(0.001f, vector.x);
		vector2.y *= 1f - 1f / Mathf.Max(0.001f, vector.y);
		vector2.z *= 1f - 1f / Mathf.Max(0.001f, vector.z);
		particle.cachedPosition = position;
		if (particle.activeMeshIndices.CountFast() == 0)
		{
			vector2 = Vector3.zero;
		}
		for (int i = 0; i < particle.particles.Count; i++)
		{
			MeshHelper.activeMeshIndices.Add(item: false);
			MeshHelper.activeMeshIndices.Add(item: false);
			ParticleSystem particleSystem = particle.particles[i];
			if (!particleSystem || !particleSystem.IsAlive() || particleSystem.particleCount == 0)
			{
				continue;
			}
			ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
			if (!component.sharedMaterial && !component.trailMaterial)
			{
				continue;
			}
			Matrix4x4 matrix4x3 = matrix4x;
			if (particleSystem.transform != transform)
			{
				if (particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.Local)
				{
					matrix4x3 = Matrix4x4.Translate(transform.InverseTransformPoint(particleSystem.transform.position)) * matrix4x3;
				}
				else
				{
					matrix4x3 *= Matrix4x4.Translate(-transform.position);
				}
			}
			else
			{
				matrix4x3 = GetScaledMatrix(particleSystem);
			}
			matrix4x3 = matrix4x2 * matrix4x3;
			if (particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World && 0f < vector2.sqrMagnitude)
			{
				int particleCount = particleSystem.particleCount;
				if (s_Particles.Length < particleCount)
				{
					s_Particles = new ParticleSystem.Particle[Mathf.NextPowerOfTwo(particleCount)];
				}
				particleSystem.GetParticles(s_Particles);
				for (int j = 0; j < particleCount; j++)
				{
					ParticleSystem.Particle particle2 = s_Particles[j];
					particle2.position += vector2;
					s_Particles[j] = particle2;
				}
				particleSystem.SetParticles(s_Particles, particleCount);
			}
			if (Mathf.Approximately(particle.canvasRenderer.GetInheritedAlpha(), 0f))
			{
				continue;
			}
			if (CanBakeMesh(component))
			{
				long materialHash = particleSystem.GetMaterialHash(trail: false);
				if (materialHash != 0L)
				{
					Mesh temporaryMesh = MeshHelper.GetTemporaryMesh();
					component.BakeMesh(temporaryMesh, camera, useTransform: true);
					MeshHelper.Push(i * 2, materialHash, temporaryMesh, matrix4x3);
				}
			}
			if (!particleSystem.trails.enabled)
			{
				continue;
			}
			long materialHash2 = particleSystem.GetMaterialHash(trail: true);
			if (materialHash2 != 0L)
			{
				matrix4x3 = ((particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.Local && particleSystem.trails.worldSpace) ? (matrix4x3 * Matrix4x4.Translate(-particleSystem.transform.position)) : matrix4x3);
				Mesh temporaryMesh2 = MeshHelper.GetTemporaryMesh();
				try
				{
					component.BakeTrailsMesh(temporaryMesh2, camera, useTransform: true);
					MeshHelper.Push(i * 2 + 1, materialHash2, temporaryMesh2, matrix4x3);
				}
				catch
				{
					MeshHelper.DiscardTemporaryMesh(temporaryMesh2);
				}
			}
		}
		if (share)
		{
			Mesh mesh = MeshPool.Rent();
			MeshHelper.CombineMesh(mesh);
			ShareMeshPool.PushMesh(particle.shareMeshTag, mesh, MeshHelper.activeMeshIndices);
			MeshHelper.Clear();
			return mesh;
		}
		particle.activeMeshIndices = MeshHelper.activeMeshIndices;
		MeshHelper.CombineMesh(particle.bakedMesh);
		MeshHelper.Clear();
		return particle.bakedMesh;
	}

	private static bool CanBakeMesh(ParticleSystemRenderer renderer)
	{
		if (renderer.renderMode == ParticleSystemRenderMode.Mesh && renderer.mesh == null)
		{
			return false;
		}
		if (renderer.renderMode == ParticleSystemRenderMode.None)
		{
			return false;
		}
		return true;
	}
}
