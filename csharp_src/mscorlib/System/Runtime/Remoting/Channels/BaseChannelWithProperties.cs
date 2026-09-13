using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels;

[ComVisible(true)]
public abstract class BaseChannelWithProperties : BaseChannelObjectWithProperties
{
	protected IChannelSinkBase SinksWithProperties;

	public override IDictionary Properties
	{
		get
		{
			if (SinksWithProperties == null || SinksWithProperties.Properties == null)
			{
				return base.Properties;
			}
			return new AggregateDictionary(new IDictionary[2] { base.Properties, SinksWithProperties.Properties });
		}
	}
}
