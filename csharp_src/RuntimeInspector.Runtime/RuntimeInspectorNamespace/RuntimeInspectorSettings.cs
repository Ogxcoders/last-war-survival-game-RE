using UnityEngine;

namespace RuntimeInspectorNamespace;

[CreateAssetMenu(fileName = "Inspector Settings", menuName = "yasirkula/RuntimeInspector/Settings", order = 111)]
public class RuntimeInspectorSettings : ScriptableObject
{
	[SerializeField]
	private InspectorField[] m_standardDrawers;

	[SerializeField]
	private InspectorField[] m_referenceDrawers;

	[SerializeField]
	private VariableSet[] m_hiddenVariables;

	[SerializeField]
	private VariableSet[] m_exposedVariables;

	public InspectorField[] StandardDrawers => m_standardDrawers;

	public InspectorField[] ReferenceDrawers => m_referenceDrawers;

	public VariableSet[] HiddenVariables => m_hiddenVariables;

	public VariableSet[] ExposedVariables => m_exposedVariables;
}
