using System;

namespace RuntimeInspectorNamespace;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class RuntimeInspectorButtonAttribute : Attribute
{
	private readonly string m_label;

	private readonly bool m_isInitializer;

	private readonly ButtonVisibility m_visibility;

	public string Label => m_label;

	public bool IsInitializer => m_isInitializer;

	public ButtonVisibility Visibility => m_visibility;

	public RuntimeInspectorButtonAttribute(string label, bool isInitializer, ButtonVisibility visibility)
	{
		m_label = label;
		m_isInitializer = isInitializer;
		m_visibility = visibility;
	}
}
