using System;

namespace UnityEngine.Experimental.Rendering.Universal;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("Rendering/2D/Shadow Caster 2D (Experimental)")]
public class ShadowCaster2D : ShadowCasterGroup2D
{
	[SerializeField]
	private bool m_HasRenderer;

	[SerializeField]
	private bool m_UseRendererSilhouette = true;

	[SerializeField]
	private bool m_CastsShadows = true;

	[SerializeField]
	private bool m_SelfShadows;

	[SerializeField]
	private int[] m_ApplyToSortingLayers;

	[SerializeField]
	private Vector3[] m_ShapePath;

	[SerializeField]
	private int m_ShapePathHash;

	[SerializeField]
	private Mesh m_Mesh;

	[SerializeField]
	private int m_InstanceId;

	internal ShadowCasterGroup2D m_ShadowCasterGroup;

	internal ShadowCasterGroup2D m_PreviousShadowCasterGroup;

	private int m_PreviousShadowGroup;

	private bool m_PreviousCastsShadows = true;

	private int m_PreviousPathHash;

	internal Mesh mesh => m_Mesh;

	internal Vector3[] shapePath => m_ShapePath;

	internal int shapePathHash
	{
		get
		{
			return m_ShapePathHash;
		}
		set
		{
			m_ShapePathHash = value;
		}
	}

	public bool useRendererSilhouette
	{
		get
		{
			if (m_UseRendererSilhouette)
			{
				return m_HasRenderer;
			}
			return false;
		}
		set
		{
			m_UseRendererSilhouette = value;
		}
	}

	public bool selfShadows
	{
		get
		{
			return m_SelfShadows;
		}
		set
		{
			m_SelfShadows = value;
		}
	}

	public bool castsShadows
	{
		get
		{
			return m_CastsShadows;
		}
		set
		{
			m_CastsShadows = value;
		}
	}

	private static int[] SetDefaultSortingLayers()
	{
		int num = SortingLayer.layers.Length;
		int[] array = new int[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = SortingLayer.layers[i].id;
		}
		return array;
	}

	internal bool IsShadowedLayer(int layer)
	{
		if (m_ApplyToSortingLayers == null)
		{
			return false;
		}
		return Array.IndexOf(m_ApplyToSortingLayers, layer) >= 0;
	}

	private void Awake()
	{
		if (m_ApplyToSortingLayers == null)
		{
			m_ApplyToSortingLayers = SetDefaultSortingLayers();
		}
		Bounds bounds = new Bounds(base.transform.position, Vector3.one);
		Renderer component = GetComponent<Renderer>();
		if (component != null)
		{
			bounds = component.bounds;
		}
		else
		{
			Collider2D component2 = GetComponent<Collider2D>();
			if (component2 != null)
			{
				bounds = component2.bounds;
			}
		}
		Vector3 vector = bounds.center - base.transform.position;
		if (m_ShapePath == null || m_ShapePath.Length == 0)
		{
			m_ShapePath = new Vector3[4]
			{
				vector + new Vector3(0f - bounds.extents.x, 0f - bounds.extents.y),
				vector + new Vector3(bounds.extents.x, 0f - bounds.extents.y),
				vector + new Vector3(bounds.extents.x, bounds.extents.y),
				vector + new Vector3(0f - bounds.extents.x, bounds.extents.y)
			};
		}
	}

	protected void OnEnable()
	{
		if (m_Mesh == null || m_InstanceId != GetInstanceID())
		{
			m_Mesh = new Mesh();
			ShadowUtility.GenerateShadowMesh(m_Mesh, m_ShapePath);
			m_InstanceId = GetInstanceID();
		}
		m_ShadowCasterGroup = null;
	}

	protected void OnDisable()
	{
		LightUtility.RemoveFromShadowCasterGroup(this, m_ShadowCasterGroup);
	}

	public void Update()
	{
		Renderer component = GetComponent<Renderer>();
		m_HasRenderer = component != null;
		if (LightUtility.CheckForChange(m_ShapePathHash, ref m_PreviousPathHash))
		{
			ShadowUtility.GenerateShadowMesh(m_Mesh, m_ShapePath);
		}
		m_PreviousShadowCasterGroup = m_ShadowCasterGroup;
		if (LightUtility.AddToShadowCasterGroup(this, ref m_ShadowCasterGroup) && m_ShadowCasterGroup != null)
		{
			if (m_PreviousShadowCasterGroup == this)
			{
				ShadowCasterGroup2DManager.RemoveGroup(this);
			}
			LightUtility.RemoveFromShadowCasterGroup(this, m_PreviousShadowCasterGroup);
			if (m_ShadowCasterGroup == this)
			{
				ShadowCasterGroup2DManager.AddGroup(this);
			}
		}
		if (LightUtility.CheckForChange(m_ShadowGroup, ref m_PreviousShadowGroup))
		{
			ShadowCasterGroup2DManager.RemoveGroup(this);
			ShadowCasterGroup2DManager.AddGroup(this);
		}
		if (LightUtility.CheckForChange(m_CastsShadows, ref m_PreviousCastsShadows))
		{
			if (m_CastsShadows)
			{
				ShadowCasterGroup2DManager.AddGroup(this);
			}
			else
			{
				ShadowCasterGroup2DManager.RemoveGroup(this);
			}
		}
	}
}
