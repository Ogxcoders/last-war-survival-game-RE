using System;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UnityEngine.Experimental.Rendering.Universal;

[ExecuteAlways]
[DisallowMultipleComponent]
[AddComponentMenu("Rendering/2D/Light 2D (Experimental)")]
public sealed class Light2D : MonoBehaviour
{
	public enum LightType
	{
		Parametric,
		Freeform,
		Sprite,
		Point,
		Global
	}

	internal struct LightStats
	{
		public int totalLights;

		public int totalNormalMapUsage;

		public int totalVolumetricUsage;

		public uint blendStylesUsed;
	}

	public enum PointLightQuality
	{
		Fast,
		Accurate
	}

	[NotKeyable]
	[SerializeField]
	private LightType m_LightType;

	private LightType m_PreviousLightType;

	[SerializeField]
	[FormerlySerializedAs("m_LightOperationIndex")]
	private int m_BlendStyleIndex;

	[SerializeField]
	private float m_FalloffIntensity = 0.5f;

	[ColorUsage(false)]
	[SerializeField]
	private Color m_Color = Color.white;

	[SerializeField]
	private float m_Intensity = 1f;

	[SerializeField]
	private float m_LightVolumeOpacity;

	[SerializeField]
	private int[] m_ApplyToSortingLayers = new int[1];

	[SerializeField]
	private Sprite m_LightCookieSprite;

	[SerializeField]
	private bool m_UseNormalMap;

	[SerializeField]
	private int m_LightOrder;

	[SerializeField]
	private bool m_AlphaBlendOnOverlap;

	private int m_PreviousLightOrder = -1;

	private int m_PreviousBlendStyleIndex;

	private float m_PreviousLightVolumeOpacity;

	private bool m_PreviousLightCookieSpriteExists;

	private Sprite m_PreviousLightCookieSprite;

	private Mesh m_Mesh;

	private int m_LightCullingIndex = -1;

	private Bounds m_LocalBounds;

	[Range(0f, 1f)]
	[SerializeField]
	private float m_ShadowIntensity;

	[Range(0f, 1f)]
	[SerializeField]
	private float m_ShadowVolumeIntensity;

	private static SortingLayer[] s_SortingLayers;

	[SerializeField]
	private float m_PointLightInnerAngle = 360f;

	[SerializeField]
	private float m_PointLightOuterAngle = 360f;

	[SerializeField]
	private float m_PointLightInnerRadius;

	[SerializeField]
	private float m_PointLightOuterRadius = 1f;

	[SerializeField]
	private float m_PointLightDistance = 3f;

	[NotKeyable]
	[SerializeField]
	private PointLightQuality m_PointLightQuality = PointLightQuality.Accurate;

	[SerializeField]
	private int m_ShapeLightParametricSides = 5;

	[SerializeField]
	private float m_ShapeLightParametricAngleOffset;

	[SerializeField]
	private float m_ShapeLightParametricRadius = 1f;

	[SerializeField]
	private float m_ShapeLightFalloffSize = 0.5f;

	[SerializeField]
	private Vector2 m_ShapeLightFalloffOffset = Vector2.zero;

	[SerializeField]
	private Vector3[] m_ShapePath;

	private float m_PreviousShapeLightFalloffSize = -1f;

	private int m_PreviousShapeLightParametricSides = -1;

	private float m_PreviousShapeLightParametricAngleOffset = -1f;

	private float m_PreviousShapeLightParametricRadius = -1f;

	private Vector2 m_PreviousShapeLightFalloffOffset = Vector2.negativeInfinity;

	public LightType lightType
	{
		get
		{
			return m_LightType;
		}
		set
		{
			m_LightType = value;
		}
	}

	public int blendStyleIndex
	{
		get
		{
			return m_BlendStyleIndex;
		}
		set
		{
			m_BlendStyleIndex = value;
		}
	}

	public float shadowIntensity
	{
		get
		{
			return m_ShadowIntensity;
		}
		set
		{
			m_ShadowIntensity = Mathf.Clamp01(value);
		}
	}

	public float shadowVolumeIntensity
	{
		get
		{
			return m_ShadowVolumeIntensity;
		}
		set
		{
			m_ShadowVolumeIntensity = Mathf.Clamp01(value);
		}
	}

	public Color color
	{
		get
		{
			return m_Color;
		}
		set
		{
			m_Color = value;
		}
	}

	public float intensity
	{
		get
		{
			return m_Intensity;
		}
		set
		{
			m_Intensity = value;
		}
	}

	public float volumeOpacity => m_LightVolumeOpacity;

	public Sprite lightCookieSprite => m_LightCookieSprite;

	public float falloffIntensity => m_FalloffIntensity;

	public bool useNormalMap => m_UseNormalMap;

	public bool alphaBlendOnOverlap => m_AlphaBlendOnOverlap;

	public int lightOrder
	{
		get
		{
			return m_LightOrder;
		}
		set
		{
			m_LightOrder = value;
		}
	}

	internal int lightCullingIndex => m_LightCullingIndex;

	public float pointLightInnerAngle
	{
		get
		{
			return m_PointLightInnerAngle;
		}
		set
		{
			m_PointLightInnerAngle = value;
		}
	}

	public float pointLightOuterAngle
	{
		get
		{
			return m_PointLightOuterAngle;
		}
		set
		{
			m_PointLightOuterAngle = value;
		}
	}

	public float pointLightInnerRadius
	{
		get
		{
			return m_PointLightInnerRadius;
		}
		set
		{
			m_PointLightInnerRadius = value;
		}
	}

	public float pointLightOuterRadius
	{
		get
		{
			return m_PointLightOuterRadius;
		}
		set
		{
			m_PointLightOuterRadius = value;
		}
	}

	public float pointLightDistance => m_PointLightDistance;

	public PointLightQuality pointLightQuality => m_PointLightQuality;

	public int shapeLightParametricSides => m_ShapeLightParametricSides;

	public float shapeLightParametricAngleOffset => m_ShapeLightParametricAngleOffset;

	public float shapeLightParametricRadius => m_ShapeLightParametricRadius;

	public float shapeLightFalloffSize => m_ShapeLightFalloffSize;

	public Vector2 shapeLightFalloffOffset => m_ShapeLightFalloffOffset;

	public Vector3[] shapePath => m_ShapePath;

	internal static void SetupCulling(ScriptableRenderContext context, Camera camera)
	{
		if (Light2DManager.cullingGroup == null)
		{
			return;
		}
		Light2DManager.cullingGroup.targetCamera = camera;
		int num = 0;
		for (int i = 0; i < Light2DManager.lights.Length; i++)
		{
			num += Light2DManager.lights[i].Count;
		}
		if (Light2DManager.boundingSpheres == null)
		{
			Light2DManager.boundingSpheres = new BoundingSphere[Mathf.Max(1024, 2 * num)];
		}
		else if (num > Light2DManager.boundingSpheres.Length)
		{
			Light2DManager.boundingSpheres = new BoundingSphere[2 * num];
		}
		int num2 = 0;
		for (int j = 0; j < Light2DManager.lights.Length; j++)
		{
			List<Light2D> list = Light2DManager.lights[j];
			for (int k = 0; k < list.Count; k++)
			{
				Light2D light2D = list[k];
				if (!(light2D == null))
				{
					Light2DManager.boundingSpheres[num2] = light2D.GetBoundingSphere();
					light2D.m_LightCullingIndex = num2++;
				}
			}
		}
		Light2DManager.cullingGroup.SetBoundingSpheres(Light2DManager.boundingSpheres);
		Light2DManager.cullingGroup.SetBoundingSphereCount(num2);
	}

	internal static bool IsSceneLit(Camera camera)
	{
		for (int i = 0; i < Light2DManager.lights.Length; i++)
		{
			List<Light2D> list = Light2DManager.lights[i];
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].lightType == LightType.Global || list[j].IsLightVisible(camera))
				{
					return true;
				}
			}
		}
		return false;
	}

	internal static List<Light2D> GetLightsByBlendStyle(int blendStyleIndex)
	{
		return Light2DManager.lights[blendStyleIndex];
	}

	internal int GetTopMostLitLayer()
	{
		int num = -1;
		int num2 = 0;
		SortingLayer[] layers;
		if (Application.isPlaying)
		{
			if (s_SortingLayers == null)
			{
				s_SortingLayers = SortingLayer.layers;
			}
			layers = s_SortingLayers;
		}
		else
		{
			layers = SortingLayer.layers;
		}
		for (int i = 0; i < m_ApplyToSortingLayers.Length; i++)
		{
			for (int num3 = layers.Length - 1; num3 >= num2; num3--)
			{
				if (layers[num3].id == m_ApplyToSortingLayers[i])
				{
					num = i;
					num2 = num3;
				}
			}
		}
		if (num >= 0)
		{
			return m_ApplyToSortingLayers[num];
		}
		return -1;
	}

	private void UpdateMesh()
	{
		GetMesh(forceUpdate: true);
	}

	internal bool IsLitLayer(int layer)
	{
		if (m_ApplyToSortingLayers == null)
		{
			return false;
		}
		return Array.IndexOf(m_ApplyToSortingLayers, layer) >= 0;
	}

	private void InsertLight()
	{
		List<Light2D> list = Light2DManager.lights[m_BlendStyleIndex];
		int i;
		for (i = 0; i < list.Count && m_LightOrder > list[i].m_LightOrder; i++)
		{
		}
		list.Insert(i, this);
	}

	private void UpdateBlendStyle()
	{
		if (m_BlendStyleIndex != m_PreviousBlendStyleIndex)
		{
			Light2DManager.lights[m_PreviousBlendStyleIndex].Remove(this);
			m_PreviousBlendStyleIndex = m_BlendStyleIndex;
			InsertLight();
			if (m_LightType == LightType.Global)
			{
				ErrorIfDuplicateGlobalLight();
			}
		}
	}

	internal BoundingSphere GetBoundingSphere()
	{
		if (!IsShapeLight())
		{
			return GetPointLightBoundingSphere();
		}
		return GetShapeLightBoundingSphere();
	}

	internal Mesh GetMesh(bool forceUpdate = false)
	{
		if (m_Mesh != null && !forceUpdate)
		{
			return m_Mesh;
		}
		if (m_Mesh == null)
		{
			m_Mesh = new Mesh();
		}
		_ = m_Intensity * m_Color;
		switch (m_LightType)
		{
		case LightType.Freeform:
			m_LocalBounds = LightUtility.GenerateShapeMesh(ref m_Mesh, m_ShapePath, m_ShapeLightFalloffSize);
			break;
		case LightType.Parametric:
			m_LocalBounds = LightUtility.GenerateParametricMesh(ref m_Mesh, m_ShapeLightParametricRadius, m_ShapeLightFalloffSize, m_ShapeLightParametricAngleOffset, m_ShapeLightParametricSides);
			break;
		case LightType.Sprite:
			m_Mesh.Clear();
			m_LocalBounds = LightUtility.GenerateSpriteMesh(ref m_Mesh, m_LightCookieSprite, 1f);
			break;
		case LightType.Point:
			m_LocalBounds = LightUtility.GenerateParametricMesh(ref m_Mesh, 1.412135f, 0f, 0f, 4);
			break;
		}
		return m_Mesh;
	}

	internal bool IsLightVisible(Camera camera)
	{
		if (Light2DManager.cullingGroup == null || Light2DManager.cullingGroup.IsVisible(m_LightCullingIndex))
		{
			return base.isActiveAndEnabled;
		}
		return false;
	}

	internal void ErrorIfDuplicateGlobalLight()
	{
		for (int i = 0; i < m_ApplyToSortingLayers.Length; i++)
		{
			int num = m_ApplyToSortingLayers[i];
			if (Light2DManager.ContainsDuplicateGlobalLight(num, blendStyleIndex))
			{
				Debug.LogError("More than one global light on layer " + SortingLayer.IDToName(num) + " for light blend style index " + m_BlendStyleIndex);
			}
		}
	}

	private void Awake()
	{
		GetMesh();
	}

	private void OnEnable()
	{
		if (Light2DManager.cullingGroup == null)
		{
			Light2DManager.cullingGroup = new CullingGroup();
			RenderPipelineManager.beginCameraRendering += SetupCulling;
		}
		if (!Light2DManager.lights[m_BlendStyleIndex].Contains(this))
		{
			InsertLight();
		}
		m_PreviousBlendStyleIndex = m_BlendStyleIndex;
		if (m_LightType == LightType.Global)
		{
			ErrorIfDuplicateGlobalLight();
		}
		m_PreviousLightType = m_LightType;
	}

	private void OnDisable()
	{
		bool flag = false;
		for (int i = 0; i < Light2DManager.lights.Length; i++)
		{
			Light2DManager.lights[i].Remove(this);
			if (Light2DManager.lights[i].Count > 0)
			{
				flag = true;
			}
		}
		if (!flag && Light2DManager.cullingGroup != null)
		{
			Light2DManager.cullingGroup.Dispose();
			Light2DManager.cullingGroup = null;
			RenderPipelineManager.beginCameraRendering -= SetupCulling;
		}
	}

	internal List<Vector2> GetFalloffShape()
	{
		List<Vector2> list = new List<Vector2>();
		List<Vector2> extrusionDir = new List<Vector2>();
		LightUtility.GetFalloffShape(m_ShapePath, ref extrusionDir);
		for (int i = 0; i < m_ShapePath.Length; i++)
		{
			list.Add(new Vector2
			{
				x = m_ShapePath[i].x + shapeLightFalloffSize * extrusionDir[i].x,
				y = m_ShapePath[i].y + shapeLightFalloffSize * extrusionDir[i].y
			});
		}
		return list;
	}

	internal static LightStats GetLightStatsByLayer(int layer, Camera camera = null)
	{
		LightStats result = default(LightStats);
		for (int i = 0; i < Light2DManager.lights.Length; i++)
		{
			List<Light2D> list = Light2DManager.lights[i];
			for (int j = 0; j < list.Count; j++)
			{
				Light2D light2D = list[j];
				if (light2D.IsLitLayer(layer) && (!(camera != null) || light2D.lightType == LightType.Global || light2D.IsLightVisible(camera)))
				{
					result.totalLights++;
					if (light2D.useNormalMap)
					{
						result.totalNormalMapUsage++;
					}
					if (light2D.volumeOpacity > 0f)
					{
						result.totalVolumetricUsage++;
					}
					uint num = (uint)(1 << light2D.blendStyleIndex);
					result.blendStylesUsed |= num;
				}
			}
		}
		return result;
	}

	private void LateUpdate()
	{
		UpdateBlendStyle();
		bool flag = false;
		if (LightUtility.CheckForChange(m_LightOrder, ref m_PreviousLightOrder))
		{
			Light2DManager.lights[m_BlendStyleIndex].Remove(this);
			InsertLight();
		}
		if (m_LightType != m_PreviousLightType)
		{
			if (m_LightType == LightType.Global)
			{
				ErrorIfDuplicateGlobalLight();
			}
			else
			{
				flag = true;
			}
			m_PreviousLightType = m_LightType;
		}
		flag |= LightUtility.CheckForChange(m_ShapeLightFalloffSize, ref m_PreviousShapeLightFalloffSize);
		flag |= LightUtility.CheckForChange(m_ShapeLightParametricRadius, ref m_PreviousShapeLightParametricRadius);
		flag |= LightUtility.CheckForChange(m_ShapeLightParametricSides, ref m_PreviousShapeLightParametricSides);
		flag |= LightUtility.CheckForChange(m_LightVolumeOpacity, ref m_PreviousLightVolumeOpacity);
		flag |= LightUtility.CheckForChange(m_ShapeLightParametricAngleOffset, ref m_PreviousShapeLightParametricAngleOffset);
		flag |= LightUtility.CheckForChange(m_LightCookieSprite != null, ref m_PreviousLightCookieSpriteExists);
		flag |= LightUtility.CheckForChange(m_LightCookieSprite, ref m_PreviousLightCookieSprite);
		if ((flag | LightUtility.CheckForChange(m_ShapeLightFalloffOffset, ref m_PreviousShapeLightFalloffOffset)) && m_LightType != LightType.Global)
		{
			UpdateMesh();
		}
	}

	private BoundingSphere GetPointLightBoundingSphere()
	{
		BoundingSphere result = default(BoundingSphere);
		result.radius = m_PointLightOuterRadius;
		result.position = base.transform.position;
		return result;
	}

	internal bool IsShapeLight()
	{
		return m_LightType != LightType.Point;
	}

	private BoundingSphere GetShapeLightBoundingSphere()
	{
		Vector3 position = Vector3.Max(m_LocalBounds.max, m_LocalBounds.max + (Vector3)m_ShapeLightFalloffOffset);
		Vector3 position2 = Vector3.Min(m_LocalBounds.min, m_LocalBounds.min + (Vector3)m_ShapeLightFalloffOffset);
		Vector3 vector = base.transform.TransformPoint(position);
		Vector3 vector2 = base.transform.TransformPoint(position2);
		Vector3 vector3 = 0.5f * (vector + vector2);
		float radius = Vector3.Magnitude(vector - vector3);
		BoundingSphere result = default(BoundingSphere);
		result.radius = radius;
		result.position = vector3;
		return result;
	}
}
