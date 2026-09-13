namespace UnityEngine.UIElements;

public abstract class Manipulator : IManipulator
{
	private VisualElement m_Target;

	public VisualElement target
	{
		get
		{
			return m_Target;
		}
		set
		{
			if (target != null)
			{
				UnregisterCallbacksFromTarget();
			}
			m_Target = value;
			if (target != null)
			{
				RegisterCallbacksOnTarget();
			}
		}
	}

	protected abstract void RegisterCallbacksOnTarget();

	protected abstract void UnregisterCallbacksFromTarget();
}
