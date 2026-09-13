namespace System.Data.SqlClient.SNI;

internal class SNIMarsQueuedPacket
{
	private SNIPacket _packet;

	private SNIAsyncCallback _callback;

	public SNIPacket Packet
	{
		get
		{
			return _packet;
		}
		set
		{
			_packet = value;
		}
	}

	public SNIAsyncCallback Callback
	{
		get
		{
			return _callback;
		}
		set
		{
			_callback = value;
		}
	}

	public SNIMarsQueuedPacket(SNIPacket packet, SNIAsyncCallback callback)
	{
		_packet = packet;
		_callback = callback;
	}
}
