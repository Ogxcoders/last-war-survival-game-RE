using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging;

[ComVisible(true)]
public interface IMethodCallMessage : IMethodMessage, IMessage
{
	int InArgCount { get; }

	object[] InArgs { get; }

	object GetInArg(int argNum);

	string GetInArgName(int index);
}
