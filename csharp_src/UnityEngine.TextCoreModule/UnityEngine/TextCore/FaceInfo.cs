using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.TextCore.LowLevel;

namespace UnityEngine.TextCore;

[Serializable]
[UsedByNativeCode]
public struct FaceInfo
{
	[SerializeField]
	[NativeName("faceIndex")]
	private int m_FaceIndex;

	[SerializeField]
	[NativeName("familyName")]
	private string m_FamilyName;

	[NativeName("styleName")]
	[SerializeField]
	private string m_StyleName;

	[NativeName("pointSize")]
	[SerializeField]
	private int m_PointSize;

	[NativeName("scale")]
	[SerializeField]
	private float m_Scale;

	[NativeName("lineHeight")]
	[SerializeField]
	private float m_LineHeight;

	[NativeName("ascentLine")]
	[SerializeField]
	private float m_AscentLine;

	[NativeName("capLine")]
	[SerializeField]
	private float m_CapLine;

	[NativeName("meanLine")]
	[SerializeField]
	private float m_MeanLine;

	[SerializeField]
	[NativeName("baseline")]
	private float m_Baseline;

	[SerializeField]
	[NativeName("descentLine")]
	private float m_DescentLine;

	[SerializeField]
	[NativeName("superscriptOffset")]
	private float m_SuperscriptOffset;

	[SerializeField]
	[NativeName("superscriptSize")]
	private float m_SuperscriptSize;

	[SerializeField]
	[NativeName("subscriptOffset")]
	private float m_SubscriptOffset;

	[NativeName("subscriptSize")]
	[SerializeField]
	private float m_SubscriptSize;

	[SerializeField]
	[NativeName("underlineOffset")]
	private float m_UnderlineOffset;

	[NativeName("underlineThickness")]
	[SerializeField]
	private float m_UnderlineThickness;

	[NativeName("strikethroughOffset")]
	[SerializeField]
	private float m_StrikethroughOffset;

	[NativeName("strikethroughThickness")]
	[SerializeField]
	private float m_StrikethroughThickness;

	[SerializeField]
	[NativeName("tabWidth")]
	private float m_TabWidth;

	internal int faceIndex
	{
		get
		{
			return m_FaceIndex;
		}
		set
		{
			m_FaceIndex = value;
		}
	}

	public string familyName
	{
		get
		{
			return m_FamilyName;
		}
		set
		{
			m_FamilyName = value;
		}
	}

	public string styleName
	{
		get
		{
			return m_StyleName;
		}
		set
		{
			m_StyleName = value;
		}
	}

	public int pointSize
	{
		get
		{
			return m_PointSize;
		}
		set
		{
			m_PointSize = value;
		}
	}

	public float scale
	{
		get
		{
			return m_Scale;
		}
		set
		{
			m_Scale = value;
		}
	}

	public float lineHeight
	{
		get
		{
			return m_LineHeight;
		}
		set
		{
			m_LineHeight = value;
		}
	}

	public float ascentLine
	{
		get
		{
			return m_AscentLine;
		}
		set
		{
			m_AscentLine = value;
		}
	}

	public float capLine
	{
		get
		{
			return m_CapLine;
		}
		set
		{
			m_CapLine = value;
		}
	}

	public float meanLine
	{
		get
		{
			return m_MeanLine;
		}
		set
		{
			m_MeanLine = value;
		}
	}

	public float baseline
	{
		get
		{
			return m_Baseline;
		}
		set
		{
			m_Baseline = value;
		}
	}

	public float descentLine
	{
		get
		{
			return m_DescentLine;
		}
		set
		{
			m_DescentLine = value;
		}
	}

	public float superscriptOffset
	{
		get
		{
			return m_SuperscriptOffset;
		}
		set
		{
			m_SuperscriptOffset = value;
		}
	}

	public float superscriptSize
	{
		get
		{
			return m_SuperscriptSize;
		}
		set
		{
			m_SuperscriptSize = value;
		}
	}

	public float subscriptOffset
	{
		get
		{
			return m_SubscriptOffset;
		}
		set
		{
			m_SubscriptOffset = value;
		}
	}

	public float subscriptSize
	{
		get
		{
			return m_SubscriptSize;
		}
		set
		{
			m_SubscriptSize = value;
		}
	}

	public float underlineOffset
	{
		get
		{
			return m_UnderlineOffset;
		}
		set
		{
			m_UnderlineOffset = value;
		}
	}

	public float underlineThickness
	{
		get
		{
			return m_UnderlineThickness;
		}
		set
		{
			m_UnderlineThickness = value;
		}
	}

	public float strikethroughOffset
	{
		get
		{
			return m_StrikethroughOffset;
		}
		set
		{
			m_StrikethroughOffset = value;
		}
	}

	public float strikethroughThickness
	{
		get
		{
			return m_StrikethroughThickness;
		}
		set
		{
			m_StrikethroughThickness = value;
		}
	}

	public float tabWidth
	{
		get
		{
			return m_TabWidth;
		}
		set
		{
			m_TabWidth = value;
		}
	}

	internal FaceInfo(string familyName, string styleName, int pointSize, float scale, float lineHeight, float ascentLine, float capLine, float meanLine, float baseline, float descentLine, float superscriptOffset, float superscriptSize, float subscriptOffset, float subscriptSize, float underlineOffset, float underlineThickness, float strikethroughOffset, float strikethroughThickness, float tabWidth)
	{
		m_FaceIndex = 0;
		m_FamilyName = familyName;
		m_StyleName = styleName;
		m_PointSize = pointSize;
		m_Scale = scale;
		m_LineHeight = lineHeight;
		m_AscentLine = ascentLine;
		m_CapLine = capLine;
		m_MeanLine = meanLine;
		m_Baseline = baseline;
		m_DescentLine = descentLine;
		m_SuperscriptOffset = superscriptOffset;
		m_SuperscriptSize = superscriptSize;
		m_SubscriptOffset = subscriptOffset;
		m_SubscriptSize = subscriptSize;
		m_UnderlineOffset = underlineOffset;
		m_UnderlineThickness = underlineThickness;
		m_StrikethroughOffset = strikethroughOffset;
		m_StrikethroughThickness = strikethroughThickness;
		m_TabWidth = tabWidth;
	}

	public bool Compare(FaceInfo other)
	{
		return familyName == other.familyName && styleName == other.styleName && faceIndex == other.faceIndex && pointSize == other.pointSize && FontEngineUtilities.Approximately(scale, other.scale) && FontEngineUtilities.Approximately(lineHeight, other.lineHeight) && FontEngineUtilities.Approximately(ascentLine, other.ascentLine) && FontEngineUtilities.Approximately(capLine, other.capLine) && FontEngineUtilities.Approximately(meanLine, other.meanLine) && FontEngineUtilities.Approximately(baseline, other.baseline) && FontEngineUtilities.Approximately(descentLine, other.descentLine) && FontEngineUtilities.Approximately(superscriptOffset, other.superscriptOffset) && FontEngineUtilities.Approximately(superscriptSize, other.superscriptSize) && FontEngineUtilities.Approximately(subscriptOffset, other.subscriptOffset) && FontEngineUtilities.Approximately(subscriptSize, other.subscriptSize) && FontEngineUtilities.Approximately(underlineOffset, other.underlineOffset) && FontEngineUtilities.Approximately(underlineThickness, other.underlineThickness) && FontEngineUtilities.Approximately(strikethroughOffset, other.strikethroughOffset) && FontEngineUtilities.Approximately(strikethroughThickness, other.strikethroughThickness) && FontEngineUtilities.Approximately(tabWidth, other.tabWidth);
	}
}
