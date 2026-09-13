namespace System.Data.SqlClient.SNI;

internal abstract class SNIHandle
{
	public abstract uint Status { get; }

	public abstract Guid ConnectionId { get; }

	public abstract void Dispose();

	public abstract void SetAsyncCallbacks(SNIAsyncCallback receiveCallback, SNIAsyncCallback sendCallback);

	public abstract void SetBufferSize(int bufferSize);

	public abstract uint Send(SNIPacket packet);

	public abstract uint SendAsync(SNIPacket packet, bool disposePacketAfterSendAsync, SNIAsyncCallback callback = null);

	public abstract uint Receive(out SNIPacket packet, int timeoutInMilliseconds);

	public abstract uint ReceiveAsync(ref SNIPacket packet);

	public abstract uint EnableSsl(uint options);

	public abstract void DisableSsl();

	public abstract uint CheckConnection();
}
