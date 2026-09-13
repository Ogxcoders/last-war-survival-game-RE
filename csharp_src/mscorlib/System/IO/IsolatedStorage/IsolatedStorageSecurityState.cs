using System.Security;

namespace System.IO.IsolatedStorage;

public class IsolatedStorageSecurityState : SecurityState
{
	public IsolatedStorageSecurityOptions Options => IsolatedStorageSecurityOptions.IncreaseQuotaForApplication;

	public long Quota
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
		}
	}

	public long UsedSize
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal IsolatedStorageSecurityState()
	{
	}

	public override void EnsureState()
	{
		throw new NotImplementedException();
	}
}
