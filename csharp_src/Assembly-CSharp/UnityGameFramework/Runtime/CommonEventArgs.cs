namespace UnityGameFramework.Runtime;

public class CommonEventArgs : GameEventArgs
{
	private int m_EventId;

	private object m_UserData;

	public override int Id => m_EventId;

	public CommonEventArgs(EventId eventId)
	{
		m_EventId = (int)eventId;
	}

	public override void Clear()
	{
		m_EventId = 0;
		m_UserData = null;
	}
}
