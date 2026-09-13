namespace UnityEngine;

public abstract class Subsystem : ISubsystem
{
	internal ISubsystemDescriptor m_subsystemDescriptor;

	public abstract bool running { get; }

	public abstract void Start();

	public abstract void Stop();

	public void Destroy()
	{
		if (Internal_SubsystemInstances.s_StandaloneSubsystemInstances.Remove(this))
		{
			OnDestroy();
		}
	}

	protected abstract void OnDestroy();
}
public abstract class Subsystem<TSubsystemDescriptor> : Subsystem where TSubsystemDescriptor : ISubsystemDescriptor
{
	public TSubsystemDescriptor SubsystemDescriptor => (TSubsystemDescriptor)m_subsystemDescriptor;
}
