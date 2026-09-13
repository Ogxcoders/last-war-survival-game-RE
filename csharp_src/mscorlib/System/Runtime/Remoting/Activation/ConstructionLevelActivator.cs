using System.Threading;

namespace System.Runtime.Remoting.Activation;

[Serializable]
internal class ConstructionLevelActivator : IActivator
{
	public ActivatorLevel Level => ActivatorLevel.Construction;

	public IActivator NextActivator
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public IConstructionReturnMessage Activate(IConstructionCallMessage msg)
	{
		return (IConstructionReturnMessage)Thread.CurrentContext.GetServerContextSinkChain().SyncProcessMessage(msg);
	}
}
