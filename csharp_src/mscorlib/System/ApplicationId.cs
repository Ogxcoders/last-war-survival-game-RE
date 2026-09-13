using System.Text;

namespace System;

[Serializable]
public sealed class ApplicationId
{
	private readonly byte[] _publicKeyToken;

	public string Culture { get; }

	public string Name { get; }

	public string ProcessorArchitecture { get; }

	public Version Version { get; }

	public byte[] PublicKeyToken => (byte[])_publicKeyToken.Clone();

	public ApplicationId(byte[] publicKeyToken, string name, Version version, string processorArchitecture, string culture)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0)
		{
			throw new ArgumentException("ApplicationId cannot have an empty string for the name.");
		}
		if (version == null)
		{
			throw new ArgumentNullException("version");
		}
		if (publicKeyToken == null)
		{
			throw new ArgumentNullException("publicKeyToken");
		}
		_publicKeyToken = (byte[])publicKeyToken.Clone();
		Name = name;
		Version = version;
		ProcessorArchitecture = processorArchitecture;
		Culture = culture;
	}

	public ApplicationId Copy()
	{
		return new ApplicationId(_publicKeyToken, Name, Version, ProcessorArchitecture, Culture);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		stringBuilder.Append(Name);
		if (Culture != null)
		{
			stringBuilder.Append(", culture=\"");
			stringBuilder.Append(Culture);
			stringBuilder.Append('"');
		}
		stringBuilder.Append(", version=\"");
		stringBuilder.Append(Version.ToString());
		stringBuilder.Append('"');
		if (_publicKeyToken != null)
		{
			stringBuilder.Append(", publicKeyToken=\"");
			stringBuilder.Append(EncodeHexString(_publicKeyToken));
			stringBuilder.Append('"');
		}
		if (ProcessorArchitecture != null)
		{
			stringBuilder.Append(", processorArchitecture =\"");
			stringBuilder.Append(ProcessorArchitecture);
			stringBuilder.Append('"');
		}
		return StringBuilderCache.GetStringAndRelease(stringBuilder);
	}

	private static char HexDigit(int num)
	{
		return (char)((num < 10) ? (num + 48) : (num + 55));
	}

	private static string EncodeHexString(byte[] sArray)
	{
		string result = null;
		if (sArray != null)
		{
			char[] array = new char[sArray.Length * 2];
			int i = 0;
			int num = 0;
			for (; i < sArray.Length; i++)
			{
				int num2 = (sArray[i] & 0xF0) >> 4;
				array[num++] = HexDigit(num2);
				num2 = sArray[i] & 0xF;
				array[num++] = HexDigit(num2);
			}
			result = new string(array);
		}
		return result;
	}

	public override bool Equals(object o)
	{
		if (!(o is ApplicationId applicationId))
		{
			return false;
		}
		if (!object.Equals(Name, applicationId.Name) || !object.Equals(Version, applicationId.Version) || !object.Equals(ProcessorArchitecture, applicationId.ProcessorArchitecture) || !object.Equals(Culture, applicationId.Culture))
		{
			return false;
		}
		if (_publicKeyToken.Length != applicationId._publicKeyToken.Length)
		{
			return false;
		}
		for (int i = 0; i < _publicKeyToken.Length; i++)
		{
			if (_publicKeyToken[i] != applicationId._publicKeyToken[i])
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		return Name.GetHashCode() ^ Version.GetHashCode();
	}
}
