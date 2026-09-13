using System;

namespace UnityEngine.UIElements;

[Serializable]
internal struct StyleValueHandle
{
	[SerializeField]
	private StyleValueType m_ValueType;

	[SerializeField]
	internal int valueIndex;

	public StyleValueType valueType
	{
		get
		{
			return m_ValueType;
		}
		internal set
		{
			m_ValueType = value;
		}
	}

	internal StyleValueHandle(int valueIndex, StyleValueType valueType)
	{
		this.valueIndex = valueIndex;
		m_ValueType = valueType;
	}
}
