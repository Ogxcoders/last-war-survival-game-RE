namespace System.Runtime.InteropServices.WindowsRuntime;

public struct EventRegistrationToken
{
	internal ulong m_value;

	internal ulong Value => m_value;

	internal EventRegistrationToken(ulong value)
	{
		m_value = value;
	}

	public static bool operator ==(EventRegistrationToken left, EventRegistrationToken right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(EventRegistrationToken left, EventRegistrationToken right)
	{
		return !left.Equals(right);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is EventRegistrationToken eventRegistrationToken))
		{
			return false;
		}
		return eventRegistrationToken.Value == Value;
	}

	public override int GetHashCode()
	{
		return m_value.GetHashCode();
	}
}
