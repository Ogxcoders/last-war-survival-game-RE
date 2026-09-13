using System;

namespace UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class CustomGridBrushAttribute : Attribute
{
	private bool m_HideAssetInstances;

	private bool m_HideDefaultInstance;

	private bool m_DefaultBrush;

	private string m_DefaultName;

	public bool hideAssetInstances => m_HideAssetInstances;

	public bool hideDefaultInstance => m_HideDefaultInstance;

	public bool defaultBrush => m_DefaultBrush;

	public string defaultName => m_DefaultName;

	public CustomGridBrushAttribute()
	{
		m_HideAssetInstances = false;
		m_HideDefaultInstance = false;
		m_DefaultBrush = false;
		m_DefaultName = "";
	}

	public CustomGridBrushAttribute(bool hideAssetInstances, bool hideDefaultInstance, bool defaultBrush, string defaultName)
	{
		m_HideAssetInstances = hideAssetInstances;
		m_HideDefaultInstance = hideDefaultInstance;
		m_DefaultBrush = defaultBrush;
		m_DefaultName = defaultName;
	}
}
