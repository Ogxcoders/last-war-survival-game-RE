using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.LWRP")]
public static class RenderingUtils
{
	private static List<ShaderTagId> m_LegacyShaderPassNames = new List<ShaderTagId>
	{
		new ShaderTagId("Always"),
		new ShaderTagId("ForwardBase"),
		new ShaderTagId("PrepassBase"),
		new ShaderTagId("Vertex"),
		new ShaderTagId("VertexLMRGBM"),
		new ShaderTagId("VertexLM")
	};

	private static Mesh s_FullscreenMesh = null;

	private static Mesh s_CustomScreenMesh = null;

	private static Vector2 s_CustomScreenMeshScale;

	private static Vector2 s_CustomScreenMeshOffset;

	private static Material s_ErrorMaterial;

	private static Dictionary<RenderTextureFormat, bool> m_RenderTextureFormatSupport = new Dictionary<RenderTextureFormat, bool>();

	private static Dictionary<GraphicsFormat, Dictionary<FormatUsage, bool>> m_GraphicsFormatSupport = new Dictionary<GraphicsFormat, Dictionary<FormatUsage, bool>>();

	public static Mesh fullscreenMesh
	{
		get
		{
			if (s_FullscreenMesh != null)
			{
				return s_FullscreenMesh;
			}
			float y = 1f;
			float y2 = 0f;
			s_FullscreenMesh = new Mesh
			{
				name = "Fullscreen Quad"
			};
			s_FullscreenMesh.SetVertices(new List<Vector3>
			{
				new Vector3(-1f, -1f, 0f),
				new Vector3(-1f, 1f, 0f),
				new Vector3(1f, -1f, 0f),
				new Vector3(1f, 1f, 0f)
			});
			s_FullscreenMesh.SetUVs(0, new List<Vector2>
			{
				new Vector2(0f, y2),
				new Vector2(0f, y),
				new Vector2(1f, y2),
				new Vector2(1f, y)
			});
			s_FullscreenMesh.SetIndices(new int[6] { 0, 1, 2, 2, 1, 3 }, MeshTopology.Triangles, 0, calculateBounds: false);
			s_FullscreenMesh.UploadMeshData(markNoLongerReadable: true);
			return s_FullscreenMesh;
		}
	}

	internal static bool useStructuredBuffer => false;

	private static Material errorMaterial
	{
		get
		{
			if (s_ErrorMaterial == null)
			{
				try
				{
					s_ErrorMaterial = new Material(Shader.Find("Hidden/Universal Render Pipeline/FallbackError"));
				}
				catch
				{
				}
			}
			return s_ErrorMaterial;
		}
	}

	public static Mesh GetCustomScreenMesh(Vector2 scale, Vector2 offset)
	{
		if ((double)(s_CustomScreenMeshScale - scale).sqrMagnitude > 1E-05 || (double)(s_CustomScreenMeshOffset - offset).sqrMagnitude > 1E-05)
		{
			s_CustomScreenMesh = null;
		}
		if (s_CustomScreenMesh != null)
		{
			return s_CustomScreenMesh;
		}
		s_CustomScreenMeshScale = scale;
		s_CustomScreenMeshOffset = offset;
		Matrix4x4 matrix4x = Matrix4x4.TRS(offset, Quaternion.identity, scale);
		float y = 1f;
		float y2 = 0f;
		s_CustomScreenMesh = new Mesh
		{
			name = "Custom Screen Mesh"
		};
		List<Vector3> list = new List<Vector3>
		{
			new Vector3(-1f, -1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(1f, -1f, 0f),
			new Vector3(1f, 1f, 0f)
		};
		List<Vector2> uvs = new List<Vector2>
		{
			new Vector2(0f, y2),
			new Vector2(0f, y),
			new Vector2(1f, y2),
			new Vector2(1f, y)
		};
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = matrix4x.MultiplyPoint(list[i]);
		}
		s_CustomScreenMesh.SetVertices(list);
		s_CustomScreenMesh.SetUVs(0, uvs);
		s_CustomScreenMesh.SetIndices(new int[6] { 0, 1, 2, 2, 1, 3 }, MeshTopology.Triangles, 0, calculateBounds: false);
		s_CustomScreenMesh.UploadMeshData(markNoLongerReadable: true);
		return s_CustomScreenMesh;
	}

	public static void SetViewAndProjectionMatrices(CommandBuffer cmd, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix, bool setInverseMatrices)
	{
		Matrix4x4 matrix4x = projectionMatrix * viewMatrix;
		cmd.SetGlobalMatrix(ShaderPropertyId.viewMatrix, viewMatrix);
		cmd.SetGlobalMatrix(ShaderPropertyId.projectionMatrix, projectionMatrix);
		cmd.SetGlobalMatrix(ShaderPropertyId.viewAndProjectionMatrix, matrix4x);
		if (setInverseMatrices)
		{
			Matrix4x4 value = Matrix4x4.Inverse(viewMatrix);
			Matrix4x4 value2 = Matrix4x4.Inverse(matrix4x);
			cmd.SetGlobalMatrix(ShaderPropertyId.inverseViewMatrix, value);
			cmd.SetGlobalMatrix(ShaderPropertyId.inverseViewAndProjectionMatrix, value2);
		}
	}

	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	internal static void RenderObjectsWithError(ScriptableRenderContext context, ref CullingResults cullResults, Camera camera, FilteringSettings filterSettings, SortingCriteria sortFlags)
	{
		if (!(errorMaterial == null))
		{
			SortingSettings sortingSettings = new SortingSettings(camera);
			sortingSettings.criteria = sortFlags;
			SortingSettings sortingSettings2 = sortingSettings;
			DrawingSettings drawingSettings = new DrawingSettings(m_LegacyShaderPassNames[0], sortingSettings2);
			drawingSettings.perObjectData = PerObjectData.None;
			drawingSettings.overrideMaterial = errorMaterial;
			drawingSettings.overrideMaterialPassIndex = 0;
			DrawingSettings drawingSettings2 = drawingSettings;
			for (int i = 1; i < m_LegacyShaderPassNames.Count; i++)
			{
				drawingSettings2.SetShaderPassName(i, m_LegacyShaderPassNames[i]);
			}
			context.DrawRenderers(cullResults, ref drawingSettings2, ref filterSettings);
		}
	}

	internal static void ClearSystemInfoCache()
	{
		m_RenderTextureFormatSupport.Clear();
		m_GraphicsFormatSupport.Clear();
	}

	public static bool SupportsRenderTextureFormat(RenderTextureFormat format)
	{
		if (!m_RenderTextureFormatSupport.TryGetValue(format, out var value))
		{
			value = SystemInfo.SupportsRenderTextureFormat(format);
			m_RenderTextureFormatSupport.Add(format, value);
		}
		return value;
	}

	public static bool SupportsGraphicsFormat(GraphicsFormat format, FormatUsage usage)
	{
		bool value = false;
		if (!m_GraphicsFormatSupport.TryGetValue(format, out var value2))
		{
			value2 = new Dictionary<FormatUsage, bool>();
			value = SystemInfo.IsFormatSupported(format, usage);
			value2.Add(usage, value);
			m_GraphicsFormatSupport.Add(format, value2);
		}
		else if (!value2.TryGetValue(usage, out value))
		{
			value = SystemInfo.IsFormatSupported(format, usage);
			value2.Add(usage, value);
		}
		return value;
	}

	internal static int GetLastValidColorBufferIndex(RenderTargetIdentifier[] colorBuffers)
	{
		int num = colorBuffers.Length - 1;
		while (num >= 0 && !(colorBuffers[num] != 0))
		{
			num--;
		}
		return num;
	}

	internal static uint GetValidColorBufferCount(RenderTargetIdentifier[] colorBuffers)
	{
		uint num = 0u;
		if (colorBuffers != null)
		{
			for (int i = 0; i < colorBuffers.Length; i++)
			{
				if (colorBuffers[i] != 0)
				{
					num++;
				}
			}
		}
		return num;
	}

	internal static bool IsMRT(RenderTargetIdentifier[] colorBuffers)
	{
		return GetValidColorBufferCount(colorBuffers) > 1;
	}

	internal static bool Contains(RenderTargetIdentifier[] source, RenderTargetIdentifier value)
	{
		for (int i = 0; i < source.Length; i++)
		{
			if (source[i] == value)
			{
				return true;
			}
		}
		return false;
	}

	internal static int IndexOf(RenderTargetIdentifier[] source, RenderTargetIdentifier value)
	{
		for (int i = 0; i < source.Length; i++)
		{
			if (source[i] == value)
			{
				return i;
			}
		}
		return -1;
	}

	internal static uint CountDistinct(RenderTargetIdentifier[] source, RenderTargetIdentifier value)
	{
		uint num = 0u;
		for (int i = 0; i < source.Length; i++)
		{
			if (source[i] != value && source[i] != 0)
			{
				num++;
			}
		}
		return num;
	}

	internal static int LastValid(RenderTargetIdentifier[] source)
	{
		for (int num = source.Length - 1; num >= 0; num--)
		{
			if (source[num] != 0)
			{
				return num;
			}
		}
		return -1;
	}

	internal static bool Contains(ClearFlag a, ClearFlag b)
	{
		return (a & b) == b;
	}

	internal static bool SequenceEqual(RenderTargetIdentifier[] left, RenderTargetIdentifier[] right)
	{
		if (left.Length != right.Length)
		{
			return false;
		}
		for (int i = 0; i < left.Length; i++)
		{
			if (left[i] != right[i])
			{
				return false;
			}
		}
		return true;
	}
}
