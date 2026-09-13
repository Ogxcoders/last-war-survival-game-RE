using System.Text;

namespace System.Runtime.Versioning;

[Serializable]
public sealed class FrameworkName : IEquatable<FrameworkName>
{
	private readonly string m_identifier;

	private readonly Version m_version;

	private readonly string m_profile;

	private string m_fullName;

	private const char c_componentSeparator = ',';

	private const char c_keyValueSeparator = '=';

	private const char c_versionValuePrefix = 'v';

	private const string c_versionKey = "Version";

	private const string c_profileKey = "Profile";

	public string Identifier => m_identifier;

	public Version Version => m_version;

	public string Profile => m_profile;

	public string FullName
	{
		get
		{
			if (m_fullName == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Identifier);
				stringBuilder.Append(',');
				stringBuilder.Append("Version").Append('=');
				stringBuilder.Append('v');
				stringBuilder.Append(Version);
				if (!string.IsNullOrEmpty(Profile))
				{
					stringBuilder.Append(',');
					stringBuilder.Append("Profile").Append('=');
					stringBuilder.Append(Profile);
				}
				m_fullName = stringBuilder.ToString();
			}
			return m_fullName;
		}
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as FrameworkName);
	}

	public bool Equals(FrameworkName other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if (Identifier == other.Identifier && Version == other.Version)
		{
			return Profile == other.Profile;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Identifier.GetHashCode() ^ Version.GetHashCode() ^ Profile.GetHashCode();
	}

	public override string ToString()
	{
		return FullName;
	}

	public FrameworkName(string identifier, Version version)
		: this(identifier, version, null)
	{
	}

	public FrameworkName(string identifier, Version version, string profile)
	{
		if (identifier == null)
		{
			throw new ArgumentNullException("identifier");
		}
		if (identifier.Trim().Length == 0)
		{
			throw new ArgumentException(global::SR.GetString("The parameter '{0}' cannot be an empty string.", "identifier"), "identifier");
		}
		if (version == null)
		{
			throw new ArgumentNullException("version");
		}
		m_identifier = identifier.Trim();
		m_version = (Version)version.Clone();
		m_profile = ((profile == null) ? string.Empty : profile.Trim());
	}

	public FrameworkName(string frameworkName)
	{
		if (frameworkName == null)
		{
			throw new ArgumentNullException("frameworkName");
		}
		if (frameworkName.Length == 0)
		{
			throw new ArgumentException(global::SR.GetString("The parameter '{0}' cannot be an empty string.", "frameworkName"), "frameworkName");
		}
		string[] array = frameworkName.Split(',');
		if (array.Length < 2 || array.Length > 3)
		{
			throw new ArgumentException(global::SR.GetString("FrameworkName cannot have less than two components or more than three components."), "frameworkName");
		}
		m_identifier = array[0].Trim();
		if (m_identifier.Length == 0)
		{
			throw new ArgumentException(global::SR.GetString("FrameworkName is invalid."), "frameworkName");
		}
		bool flag = false;
		m_profile = string.Empty;
		for (int i = 1; i < array.Length; i++)
		{
			string[] array2 = array[i].Split('=');
			if (array2.Length != 2)
			{
				throw new ArgumentException(global::SR.GetString("FrameworkName is invalid."), "frameworkName");
			}
			string text = array2[0].Trim();
			string text2 = array2[1].Trim();
			if (text.Equals("Version", StringComparison.OrdinalIgnoreCase))
			{
				flag = true;
				if (text2.Length > 0 && (text2[0] == 'v' || text2[0] == 'V'))
				{
					text2 = text2.Substring(1);
				}
				try
				{
					m_version = new Version(text2);
				}
				catch (Exception innerException)
				{
					throw new ArgumentException(global::SR.GetString("FrameworkName version component is invalid."), "frameworkName", innerException);
				}
			}
			else
			{
				if (!text.Equals("Profile", StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException(global::SR.GetString("FrameworkName is invalid."), "frameworkName");
				}
				if (!string.IsNullOrEmpty(text2))
				{
					m_profile = text2;
				}
			}
		}
		if (!flag)
		{
			throw new ArgumentException(global::SR.GetString("FrameworkName version component is missing."), "frameworkName");
		}
	}

	public static bool operator ==(FrameworkName left, FrameworkName right)
	{
		return left?.Equals(right) ?? ((object)right == null);
	}

	public static bool operator !=(FrameworkName left, FrameworkName right)
	{
		return !(left == right);
	}
}
