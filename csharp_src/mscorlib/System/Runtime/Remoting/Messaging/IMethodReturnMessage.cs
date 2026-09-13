using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging;

[ComVisible(true)]
public interface IMethodReturnMessage : IMethodMessage, IMessage
{
	Exception Exception { get; }

	int OutArgCount { get; }

	object[] OutArgs { get; }

	object ReturnValue { get; }

	object GetOutArg(int argNum);

	string GetOutArgName(int index);
}
