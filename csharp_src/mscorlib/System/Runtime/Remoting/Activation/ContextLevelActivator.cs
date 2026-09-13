using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Activation;

[Serializable]
internal class ContextLevelActivator : IActivator
{
	private IActivator m_NextActivator;

	public ActivatorLevel Level => ActivatorLevel.Context;

	public IActivator NextActivator
	{
		get
		{
			return m_NextActivator;
		}
		set
		{
			m_NextActivator = value;
		}
	}

	public ContextLevelActivator(IActivator next)
	{
		m_NextActivator = next;
	}

	public IConstructionReturnMessage Activate(IConstructionCallMessage ctorCall)
	{
		ServerIdentity serverIdentity = RemotingServices.CreateContextBoundObjectIdentity(ctorCall.ActivationType);
		RemotingServices.SetMessageTargetIdentity(ctorCall, serverIdentity);
		if (!(ctorCall is ConstructionCall { IsContextOk: not false }))
		{
			serverIdentity.Context = Context.CreateNewContext(ctorCall);
			Context newContext = Context.SwitchToContext(serverIdentity.Context);
			try
			{
				return m_NextActivator.Activate(ctorCall);
			}
			finally
			{
				Context.SwitchToContext(newContext);
			}
		}
		return m_NextActivator.Activate(ctorCall);
	}
}
