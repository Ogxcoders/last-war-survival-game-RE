namespace UnityEngine.UIElements;

public abstract class CommandEventBase<T> : EventBase<T>, ICommandEvent where T : CommandEventBase<T>, new()
{
	private string m_CommandName;

	public string commandName
	{
		get
		{
			if (m_CommandName == null && base.imguiEvent != null)
			{
				return base.imguiEvent.commandName;
			}
			return m_CommandName;
		}
		protected set
		{
			m_CommandName = value;
		}
	}

	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.Bubbles | EventPropagation.TricklesDown | EventPropagation.Cancellable;
		commandName = null;
	}

	public static T GetPooled(Event systemEvent)
	{
		T val = EventBase<T>.GetPooled();
		val.imguiEvent = systemEvent;
		return val;
	}

	public static T GetPooled(string commandName)
	{
		T val = EventBase<T>.GetPooled();
		val.commandName = commandName;
		return val;
	}

	protected CommandEventBase()
	{
		LocalInit();
	}
}
