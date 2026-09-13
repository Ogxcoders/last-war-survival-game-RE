using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
[UsedByNativeCode]
public sealed class TreePrototype
{
	internal GameObject m_Prefab;

	internal float m_BendFactor;

	public GameObject prefab
	{
		get
		{
			return m_Prefab;
		}
		set
		{
			m_Prefab = value;
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

	public TreePrototype()
	{
	}

	public TreePrototype(TreePrototype other)
	{
		prefab = other.prefab;
		bendFactor = other.bendFactor;
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as TreePrototype);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	private bool Equals(TreePrototype other)
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
		return prefab == other.prefab && bendFactor == other.bendFactor;
	}
}
