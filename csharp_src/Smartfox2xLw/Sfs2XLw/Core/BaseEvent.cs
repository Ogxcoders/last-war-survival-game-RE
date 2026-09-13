using System.Collections;

namespace Sfs2XLw.Core;

public class BaseEvent
{
	protected Hashtable arguments;

	protected string type;

	protected object target;

	public string Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	public IDictionary Params
	{
		get
		{
			return arguments;
		}
		set
		{
			arguments = value as Hashtable;
		}
	}

	public object Target
	{
		get
		{
			return target;
		}
		set
		{
			target = value;
		}
	}

	public override string ToString()
	{
		return type + " [ " + ((target != null) ? target.ToString() : "null") + "]";
	}

	public BaseEvent Clone()
	{
		return new BaseEvent(type, arguments);
	}

	public BaseEvent(string type)
	{
		Type = type;
		if (arguments == null)
		{
			arguments = new Hashtable();
		}
	}

	public BaseEvent(string type, Hashtable args)
	{
		Type = type;
		arguments = args;
		if (arguments == null)
		{
			arguments = new Hashtable();
		}
	}
}
