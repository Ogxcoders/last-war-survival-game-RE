namespace BestHTTP.SignalRCore.Messages;

public struct Message
{
	public MessageTypes type;

	public string invocationId;

	public bool nonblocking;

	public string target;

	public object[] arguments;

	public object item;

	public object result;

	public string error;

	public override string ToString()
	{
		switch (type)
		{
		case MessageTypes.Invocation:
			return $"[Invocation Id: {invocationId}, Target: '{target}', Argument count: {((arguments != null) ? arguments.Length : 0)}]";
		case MessageTypes.StreamItem:
			return $"[StreamItem Id: {invocationId}, Item: {item.ToString()}]";
		case MessageTypes.Completion:
			return $"[Completion Id: {invocationId}, Result: {result}, Error: '{error}']";
		case MessageTypes.StreamInvocation:
			return $"[StreamInvocation Id: {invocationId}, Target: '{target}', Argument count: {((arguments != null) ? arguments.Length : 0)}]";
		case MessageTypes.CancelInvocation:
			return $"[CancelInvocation Id: {invocationId}]";
		case MessageTypes.Ping:
			return "[Ping]";
		case MessageTypes.Close:
			if (!string.IsNullOrEmpty(error))
			{
				return $"[Close {error}]";
			}
			return "[Close]";
		default:
			return "Unknown message! Type: " + type;
		}
	}
}
