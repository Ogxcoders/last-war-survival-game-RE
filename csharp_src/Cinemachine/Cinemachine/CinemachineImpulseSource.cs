using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[SaveDuringPlay]
public class CinemachineImpulseSource : MonoBehaviour
{
	[CinemachineImpulseDefinitionProperty]
	public CinemachineImpulseDefinition m_ImpulseDefinition = new CinemachineImpulseDefinition();

	private void OnValidate()
	{
		m_ImpulseDefinition.OnValidate();
	}

	public void GenerateImpulseAt(Vector3 position, Vector3 velocity)
	{
		if (m_ImpulseDefinition != null)
		{
			m_ImpulseDefinition.CreateEvent(position, velocity);
		}
	}

	public void GenerateImpulse(Vector3 velocity)
	{
		GenerateImpulseAt(base.transform.position, velocity);
	}

	public void GenerateImpulse()
	{
		GenerateImpulse(Vector3.down);
	}
}
