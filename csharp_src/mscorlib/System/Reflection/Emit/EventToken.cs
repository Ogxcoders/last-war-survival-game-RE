using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[Serializable]
[ComVisible(true)]
public readonly struct EventToken : IEquatable<EventToken>
{
	internal readonly int tokValue;

	public static readonly EventToken Empty;

	public int Token => tokValue;

	internal EventToken(int val)
	{
		tokValue = val;
	}

	public override bool Equals(object obj)
	{
		bool flag = obj is EventToken;
		if (flag)
		{
			EventToken eventToken = (EventToken)obj;
			flag = tokValue == eventToken.tokValue;
		}
		return flag;
	}

	public bool Equals(EventToken obj)
	{
		return tokValue == obj.tokValue;
	}

	public static bool operator ==(EventToken a, EventToken b)
	{
		return object.Equals(a, b);
	}

	public static bool operator !=(EventToken a, EventToken b)
	{
		return !object.Equals(a, b);
	}

	public override int GetHashCode()
	{
		return tokValue;
	}
}
