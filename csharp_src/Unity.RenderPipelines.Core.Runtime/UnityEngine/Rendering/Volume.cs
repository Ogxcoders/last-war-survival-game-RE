namespace UnityEngine.Rendering;

[HelpURL("https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@7.5/manual/Volumes.html")]
[ExecuteAlways]
[AddComponentMenu("Miscellaneous/Volume")]
public class Volume : MonoBehaviour
{
	[Tooltip("When enabled, HDRP applies this Volume to the entire Scene.")]
	public bool isGlobal = true;

	[Tooltip("Sets the Volume priority in the stack. A higher value means higher priority. You can use negative values.")]
	public float priority;

	[Tooltip("Sets the outer distance to start blending from. A value of 0 means no blending and Unity applies the Volume overrides immediately upon entry.")]
	public float blendDistance;

	[Range(0f, 1f)]
	[Tooltip("Sets the total weight of this Volume in the Scene. 0 means no effect and 1 means full effect.")]
	public float weight = 1f;

	public VolumeProfile sharedProfile;

	private int m_PreviousLayer;

	private float m_PreviousPriority;

	private VolumeProfile m_InternalProfile;

	public VolumeProfile profile
	{
		get
		{
			if (m_InternalProfile == null)
			{
				m_InternalProfile = ScriptableObject.CreateInstance<VolumeProfile>();
				if (sharedProfile != null)
				{
					foreach (VolumeComponent component in sharedProfile.components)
					{
						VolumeComponent item = Object.Instantiate(component);
						m_InternalProfile.components.Add(item);
					}
				}
			}
			return m_InternalProfile;
		}
		set
		{
			m_InternalProfile = value;
		}
	}

	internal VolumeProfile profileRef
	{
		get
		{
			if (!(m_InternalProfile == null))
			{
				return m_InternalProfile;
			}
			return sharedProfile;
		}
	}

	public bool HasInstantiatedProfile()
	{
		return m_InternalProfile != null;
	}

	private void OnEnable()
	{
		m_PreviousLayer = base.gameObject.layer;
		VolumeManager.instance.Register(this, m_PreviousLayer);
	}

	private void OnDisable()
	{
		VolumeManager.instance.Unregister(this, base.gameObject.layer);
	}

	private void Update()
	{
		UpdateLayer();
		if (priority != m_PreviousPriority)
		{
			VolumeManager.instance.SetLayerDirty(base.gameObject.layer);
			m_PreviousPriority = priority;
		}
	}

	internal void UpdateLayer()
	{
		int layer = base.gameObject.layer;
		if (layer != m_PreviousLayer)
		{
			VolumeManager.instance.UpdateVolumeLayer(this, m_PreviousLayer, layer);
			m_PreviousLayer = layer;
		}
	}
}
