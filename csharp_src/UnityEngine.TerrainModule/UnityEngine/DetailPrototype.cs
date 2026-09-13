using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
[UsedByNativeCode]
public sealed class DetailPrototype
{
	internal GameObject m_Prototype = null;

	internal Texture2D m_PrototypeTexture = null;

	internal Color m_HealthyColor = new Color(0.2627451f, 83f / 85f, 14f / 85f, 1f);

	internal Color m_DryColor = new Color(41f / 51f, 0.7372549f, 0.101960786f, 1f);

	internal float m_MinWidth = 1f;

	internal float m_MaxWidth = 2f;

	internal float m_MinHeight = 1f;

	internal float m_MaxHeight = 2f;

	internal float m_NoiseSpread = 0.1f;

	internal float m_BendFactor = 0.1f;

	internal int m_RenderMode = 2;

	internal int m_UsePrototypeMesh = 0;

	public GameObject prototype
	{
		get
		{
			return m_Prototype;
		}
		set
		{
			m_Prototype = value;
		}
	}

	public Texture2D prototypeTexture
	{
		get
		{
			return m_PrototypeTexture;
		}
		set
		{
			m_PrototypeTexture = value;
		}
	}

	public float minWidth
	{
		get
		{
			return m_MinWidth;
		}
		set
		{
			m_MinWidth = value;
		}
	}

	public float maxWidth
	{
		get
		{
			return m_MaxWidth;
		}
		set
		{
			m_MaxWidth = value;
		}
	}

	public float minHeight
	{
		get
		{
			return m_MinHeight;
		}
		set
		{
			m_MinHeight = value;
		}
	}

	public float maxHeight
	{
		get
		{
			return m_MaxHeight;
		}
		set
		{
			m_MaxHeight = value;
		}
	}

	public float noiseSpread
	{
		get
		{
			return m_NoiseSpread;
		}
		set
		{
			m_NoiseSpread = value;
		}
	}

	public float bendFactor
	{
		get
		{
			return m_BendFactor;
		}
		set
		{
			m_BendFactor = value;
		}
	}

	public Color healthyColor
	{
		get
		{
			return m_HealthyColor;
		}
		set
		{
			m_HealthyColor = value;
		}
	}

	public Color dryColor
	{
		get
		{
			return m_DryColor;
		}
		set
		{
			m_DryColor = value;
		}
	}

	public DetailRenderMode renderMode
	{
		get
		{
			return (DetailRenderMode)m_RenderMode;
		}
		set
		{
			m_RenderMode = (int)value;
		}
	}

	public bool usePrototypeMesh
	{
		get
		{
			return m_UsePrototypeMesh != 0;
		}
		set
		{
			m_UsePrototypeMesh = (value ? 1 : 0);
		}
	}

	public DetailPrototype()
	{
	}

	public DetailPrototype(DetailPrototype other)
	{
		m_Prototype = other.m_Prototype;
		m_PrototypeTexture = other.m_PrototypeTexture;
		m_HealthyColor = other.m_HealthyColor;
		m_DryColor = other.m_DryColor;
		m_MinWidth = other.m_MinWidth;
		m_MaxWidth = other.m_MaxWidth;
		m_MinHeight = other.m_MinHeight;
		m_MaxHeight = other.m_MaxHeight;
		m_NoiseSpread = other.m_NoiseSpread;
		m_BendFactor = other.m_BendFactor;
		m_RenderMode = other.m_RenderMode;
		m_UsePrototypeMesh = other.m_UsePrototypeMesh;
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as DetailPrototype);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	private bool Equals(DetailPrototype other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if ((object)GetType() != other.GetType())
		{
			return false;
		}
		return m_Prototype == other.m_Prototype && m_PrototypeTexture == other.m_PrototypeTexture && m_HealthyColor == other.m_HealthyColor && m_DryColor == other.m_DryColor && m_MinWidth == other.m_MinWidth && m_MaxWidth == other.m_MaxWidth && m_MinHeight == other.m_MinHeight && m_MaxHeight == other.m_MaxHeight && m_NoiseSpread == other.m_NoiseSpread && m_BendFactor == other.m_BendFactor && m_RenderMode == other.m_RenderMode && m_UsePrototypeMesh == other.m_UsePrototypeMesh;
	}
}
