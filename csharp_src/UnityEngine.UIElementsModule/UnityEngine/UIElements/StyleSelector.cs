using System;
using System.Linq;

namespace UnityEngine.UIElements;

[Serializable]
internal class StyleSelector
{
	[SerializeField]
	private StyleSelectorPart[] m_Parts;

	[SerializeField]
	private StyleSelectorRelationship m_PreviousRelationship;

	internal int pseudoStateMask = -1;

	internal int negatedPseudoStateMask = -1;

	public StyleSelectorPart[] parts
	{
		get
		{
			return m_Parts;
		}
		internal set
		{
			m_Parts = value;
		}
	}

	public StyleSelectorRelationship previousRelationship
	{
		get
		{
			return m_PreviousRelationship;
		}
		internal set
		{
			m_PreviousRelationship = value;
		}
	}

	public override string ToString()
	{
		return string.Join(", ", parts.Select((StyleSelectorPart p) => p.ToString()).ToArray());
	}
}
