using System.Security.Permissions;

namespace System.Security.Cryptography;

[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public struct CngProperty : IEquatable<CngProperty>
{
	private string m_name;

	private CngPropertyOptions m_propertyOptions;

	private byte[] m_value;

	private int? m_hashCode;

	public string Name => m_name;

	public CngPropertyOptions Options => m_propertyOptions;

	internal byte[] Value => m_value;

	public CngProperty(string name, byte[] value, CngPropertyOptions options)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		m_name = name;
		m_propertyOptions = options;
		m_hashCode = null;
		if (value != null)
		{
			m_value = value.Clone() as byte[];
		}
		else
		{
			m_value = null;
		}
	}

	public byte[] GetValue()
	{
		byte[] result = null;
		if (m_value != null)
		{
			result = m_value.Clone() as byte[];
		}
		return result;
	}

	public static bool operator ==(CngProperty left, CngProperty right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(CngProperty left, CngProperty right)
	{
		return !left.Equals(right);
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !(obj is CngProperty))
		{
			return false;
		}
		return Equals((CngProperty)obj);
	}

	public bool Equals(CngProperty other)
	{
		if (!string.Equals(Name, other.Name, StringComparison.Ordinal))
		{
			return false;
		}
		if (Options != other.Options)
		{
			return false;
		}
		if (m_value == null)
		{
			return other.m_value == null;
		}
		if (other.m_value == null)
		{
			return false;
		}
		if (m_value.Length != other.m_value.Length)
		{
			return false;
		}
		for (int i = 0; i < m_value.Length; i++)
		{
			if (m_value[i] != other.m_value[i])
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		if (!m_hashCode.HasValue)
		{
			int num = Name.GetHashCode() ^ Options.GetHashCode();
			if (m_value != null)
			{
				for (int i = 0; i < m_value.Length; i++)
				{
					int num2 = m_value[i] << i % 4 * 8;
					num ^= num2;
				}
			}
			m_hashCode = num;
		}
		return m_hashCode.Value;
	}
}
