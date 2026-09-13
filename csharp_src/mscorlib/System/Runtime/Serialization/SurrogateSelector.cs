using System.Runtime.InteropServices;
using System.Security;

namespace System.Runtime.Serialization;

[ComVisible(true)]
public class SurrogateSelector : ISurrogateSelector
{
	internal SurrogateHashtable m_surrogates;

	internal ISurrogateSelector m_nextSelector;

	public SurrogateSelector()
	{
		m_surrogates = new SurrogateHashtable(32);
	}

	public virtual void AddSurrogate(Type type, StreamingContext context, ISerializationSurrogate surrogate)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		if (surrogate == null)
		{
			throw new ArgumentNullException("surrogate");
		}
		SurrogateKey key = new SurrogateKey(type, context);
		m_surrogates.Add(key, surrogate);
	}

	[SecurityCritical]
	private static bool HasCycle(ISurrogateSelector selector)
	{
		ISurrogateSelector surrogateSelector = selector;
		ISurrogateSelector surrogateSelector2 = selector;
		while (surrogateSelector != null)
		{
			surrogateSelector = surrogateSelector.GetNextSelector();
			if (surrogateSelector == null)
			{
				return true;
			}
			if (surrogateSelector == surrogateSelector2)
			{
				return false;
			}
			surrogateSelector = surrogateSelector.GetNextSelector();
			surrogateSelector2 = surrogateSelector2.GetNextSelector();
			if (surrogateSelector == surrogateSelector2)
			{
				return false;
			}
		}
		return true;
	}

	[SecurityCritical]
	public virtual void ChainSelector(ISurrogateSelector selector)
	{
		if (selector == null)
		{
			throw new ArgumentNullException("selector");
		}
		if (selector == this)
		{
			throw new SerializationException(Environment.GetResourceString("Selector is already on the list of checked selectors."));
		}
		if (!HasCycle(selector))
		{
			throw new ArgumentException(Environment.GetResourceString("Selector contained a cycle."), "selector");
		}
		ISurrogateSelector nextSelector = selector.GetNextSelector();
		ISurrogateSelector surrogateSelector = selector;
		while (nextSelector != null && nextSelector != this)
		{
			surrogateSelector = nextSelector;
			nextSelector = nextSelector.GetNextSelector();
		}
		if (nextSelector == this)
		{
			throw new ArgumentException(Environment.GetResourceString("Adding selector will introduce a cycle."), "selector");
		}
		nextSelector = selector;
		ISurrogateSelector surrogateSelector2 = selector;
		while (nextSelector != null)
		{
			nextSelector = ((nextSelector != surrogateSelector) ? nextSelector.GetNextSelector() : GetNextSelector());
			if (nextSelector == null)
			{
				break;
			}
			if (nextSelector == surrogateSelector2)
			{
				throw new ArgumentException(Environment.GetResourceString("Adding selector will introduce a cycle."), "selector");
			}
			nextSelector = ((nextSelector != surrogateSelector) ? nextSelector.GetNextSelector() : GetNextSelector());
			surrogateSelector2 = ((surrogateSelector2 != surrogateSelector) ? surrogateSelector2.GetNextSelector() : GetNextSelector());
			if (nextSelector == surrogateSelector2)
			{
				throw new ArgumentException(Environment.GetResourceString("Adding selector will introduce a cycle."), "selector");
			}
		}
		ISurrogateSelector nextSelector2 = m_nextSelector;
		m_nextSelector = selector;
		if (nextSelector2 != null)
		{
			surrogateSelector.ChainSelector(nextSelector2);
		}
	}

	[SecurityCritical]
	public virtual ISurrogateSelector GetNextSelector()
	{
		return m_nextSelector;
	}

	[SecurityCritical]
	public virtual ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		selector = this;
		SurrogateKey key = new SurrogateKey(type, context);
		ISerializationSurrogate serializationSurrogate = (ISerializationSurrogate)m_surrogates[key];
		if (serializationSurrogate != null)
		{
			return serializationSurrogate;
		}
		if (m_nextSelector != null)
		{
			return m_nextSelector.GetSurrogate(type, context, out selector);
		}
		return null;
	}

	public virtual void RemoveSurrogate(Type type, StreamingContext context)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		SurrogateKey key = new SurrogateKey(type, context);
		m_surrogates.Remove(key);
	}
}
