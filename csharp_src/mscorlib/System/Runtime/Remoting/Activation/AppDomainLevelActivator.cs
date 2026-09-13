using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Activation;

internal class AppDomainLevelActivator : IActivator
{
	private string _activationUrl;

	private IActivator _next;

	public ActivatorLevel Level => ActivatorLevel.AppDomain;

	public IActivator NextActivator
	{
		get
		{
			return _next;
		}
		set
		{
			_next = value;
		}
	}

	public AppDomainLevelActivator(string activationUrl, IActivator next)
	{
		_activationUrl = activationUrl;
		_next = next;
	}

	public IConstructionReturnMessage Activate(IConstructionCallMessage ctorCall)
	{
		IActivator activator = (IActivator)RemotingServices.Connect(typeof(IActivator), _activationUrl);
		ctorCall.Activator = ctorCall.Activator.NextActivator;
		IConstructionReturnMessage constructionReturnMessage;
		try
		{
			constructionReturnMessage = activator.Activate(ctorCall);
		}
		catch (Exception e)
		{
			return new ConstructionResponse(e, ctorCall);
		}
		ObjRef obj = (ObjRef)constructionReturnMessage.ReturnValue;
		if (RemotingServices.GetIdentityForUri(obj.URI) != null)
		{
			throw new RemotingException("Inconsistent state during activation; there may be two proxies for the same object");
		}
		object clientProxy;
		Identity orCreateClientIdentity = RemotingServices.GetOrCreateClientIdentity(obj, null, out clientProxy);
		RemotingServices.SetMessageTargetIdentity(ctorCall, orCreateClientIdentity);
		return constructionReturnMessage;
	}
}
