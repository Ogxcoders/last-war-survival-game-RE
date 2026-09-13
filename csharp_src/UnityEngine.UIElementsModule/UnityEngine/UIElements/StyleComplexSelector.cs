using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements;

[Serializable]
internal class StyleComplexSelector
{
	private struct PseudoStateData
	{
		public readonly PseudoStates state;

		public readonly bool negate;

		public PseudoStateData(PseudoStates state, bool negate)
		{
			this.state = state;
			this.negate = negate;
		}
	}

	[SerializeField]
	private int m_Specificity;

	[SerializeField]
	private StyleSelector[] m_Selectors;

	[SerializeField]
	internal int ruleIndex;

	[NonSerialized]
	internal StyleComplexSelector nextInTable;

	[NonSerialized]
	internal int orderInStyleSheet;

	private static Dictionary<string, PseudoStateData> s_PseudoStates;

	public int specificity
	{
		get
		{
			return m_Specificity;
		}
		internal set
		{
			m_Specificity = value;
		}
	}

	public StyleRule rule { get; internal set; }

	public bool isSimple => selectors.Length == 1;

	public StyleSelector[] selectors
	{
		get
		{
			return m_Selectors;
		}
		internal set
		{
			m_Selectors = value;
		}
	}

	internal void CachePseudoStateMasks()
	{
		if (s_PseudoStates == null)
		{
			s_PseudoStates = new Dictionary<string, PseudoStateData>();
			s_PseudoStates["active"] = new PseudoStateData(PseudoStates.Active, negate: false);
			s_PseudoStates["hover"] = new PseudoStateData(PseudoStates.Hover, negate: false);
			s_PseudoStates["checked"] = new PseudoStateData(PseudoStates.Checked, negate: false);
			s_PseudoStates["selected"] = new PseudoStateData(PseudoStates.Checked, negate: false);
			s_PseudoStates["disabled"] = new PseudoStateData(PseudoStates.Disabled, negate: false);
			s_PseudoStates["focus"] = new PseudoStateData(PseudoStates.Focus, negate: false);
			s_PseudoStates["root"] = new PseudoStateData(PseudoStates.Root, negate: false);
			s_PseudoStates["inactive"] = new PseudoStateData(PseudoStates.Active, negate: true);
			s_PseudoStates["enabled"] = new PseudoStateData(PseudoStates.Disabled, negate: true);
		}
		int i = 0;
		for (int num = selectors.Length; i < num; i++)
		{
			StyleSelector styleSelector = selectors[i];
			StyleSelectorPart[] parts = styleSelector.parts;
			PseudoStates pseudoStates = (PseudoStates)0;
			PseudoStates pseudoStates2 = (PseudoStates)0;
			for (int j = 0; j < styleSelector.parts.Length; j++)
			{
				if (styleSelector.parts[j].type != StyleSelectorType.PseudoClass)
				{
					continue;
				}
				if (s_PseudoStates.TryGetValue(parts[j].value, out var value))
				{
					if (!value.negate)
					{
						pseudoStates |= value.state;
					}
					else
					{
						pseudoStates2 |= value.state;
					}
				}
				else
				{
					Debug.LogWarningFormat("Unknown pseudo class \"{0}\"", parts[j].value);
				}
			}
			styleSelector.pseudoStateMask = (int)pseudoStates;
			styleSelector.negatedPseudoStateMask = (int)pseudoStates2;
		}
	}

	public override string ToString()
	{
		return string.Format("[{0}]", string.Join(", ", m_Selectors.Select((StyleSelector x) => x.ToString()).ToArray()));
	}
}
