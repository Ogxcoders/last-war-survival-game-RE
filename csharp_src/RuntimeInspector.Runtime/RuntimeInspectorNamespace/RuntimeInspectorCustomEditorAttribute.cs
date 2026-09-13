using System;

namespace RuntimeInspectorNamespace;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
public class RuntimeInspectorCustomEditorAttribute : Attribute, IComparable<RuntimeInspectorCustomEditorAttribute>
{
	private readonly Type m_inspectedType;

	private readonly bool m_editorForChildClasses;

	private readonly int m_inspectedTypeDepth;

	public Type InspectedType => m_inspectedType;

	public bool EditorForChildClasses => m_editorForChildClasses;

	public RuntimeInspectorCustomEditorAttribute(Type inspectedType, bool editorForChildClasses = false)
	{
		m_inspectedType = inspectedType;
		m_editorForChildClasses = editorForChildClasses;
		m_inspectedTypeDepth = 0;
		while (inspectedType != typeof(object))
		{
			inspectedType = inspectedType.BaseType;
			m_inspectedTypeDepth++;
		}
	}

	int IComparable<RuntimeInspectorCustomEditorAttribute>.CompareTo(RuntimeInspectorCustomEditorAttribute other)
	{
		int inspectedTypeDepth = other.m_inspectedTypeDepth;
		return inspectedTypeDepth.CompareTo(m_inspectedTypeDepth);
	}
}
