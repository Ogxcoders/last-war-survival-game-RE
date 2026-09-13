using UnityEngine;

namespace RuntimeInspectorNamespace;

public abstract class SkinnedWindow : MonoBehaviour
{
	[SerializeField]
	private UISkin m_skin;

	private int m_skinVersion;

	public UISkin Skin
	{
		get
		{
			return m_skin;
		}
		set
		{
			if (value != null && m_skin != value)
			{
				m_skin = value;
				m_skinVersion = m_skin.Version - 1;
			}
		}
	}

	protected virtual void Awake()
	{
		if ((bool)m_skin)
		{
			m_skinVersion = m_skin.Version - 1;
		}
		base.gameObject.SetActive(value: false);
		base.gameObject.SetActive(value: true);
	}

	protected virtual void Update()
	{
		if ((bool)m_skin && m_skinVersion != m_skin.Version)
		{
			m_skinVersion = m_skin.Version;
			RefreshSkin();
		}
	}

	protected abstract void RefreshSkin();
}
