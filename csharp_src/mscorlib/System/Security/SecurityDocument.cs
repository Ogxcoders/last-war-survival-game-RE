using System.Collections;
using System.Security.Util;
using System.Text;

namespace System.Security;

[Serializable]
internal sealed class SecurityDocument
{
	internal byte[] m_data;

	internal const byte c_element = 1;

	internal const byte c_attribute = 2;

	internal const byte c_text = 3;

	internal const byte c_children = 4;

	internal const int c_growthSize = 32;

	public SecurityDocument(int numData)
	{
		m_data = new byte[numData];
	}

	public SecurityDocument(byte[] data)
	{
		m_data = data;
	}

	public SecurityDocument(SecurityElement elRoot)
	{
		m_data = new byte[32];
		int position = 0;
		ConvertElement(elRoot, ref position);
	}

	public void GuaranteeSize(int size)
	{
		if (m_data.Length < size)
		{
			byte[] array = new byte[(size / 32 + 1) * 32];
			Array.Copy(m_data, 0, array, 0, m_data.Length);
			m_data = array;
		}
	}

	public void AddString(string str, ref int position)
	{
		GuaranteeSize(position + str.Length * 2 + 2);
		for (int i = 0; i < str.Length; i++)
		{
			m_data[position + 2 * i] = (byte)((int)str[i] >> 8);
			m_data[position + 2 * i + 1] = (byte)(str[i] & 0xFF);
		}
		m_data[position + str.Length * 2] = 0;
		m_data[position + str.Length * 2 + 1] = 0;
		position += str.Length * 2 + 2;
	}

	public void AppendString(string str, ref int position)
	{
		if (position <= 1 || m_data[position - 1] != 0 || m_data[position - 2] != 0)
		{
			throw new XmlSyntaxException();
		}
		position -= 2;
		AddString(str, ref position);
	}

	public static int EncodedStringSize(string str)
	{
		return str.Length * 2 + 2;
	}

	public string GetString(ref int position)
	{
		return GetString(ref position, bCreate: true);
	}

	public string GetString(ref int position, bool bCreate)
	{
		int i;
		for (i = position; i < m_data.Length - 1 && (m_data[i] != 0 || m_data[i + 1] != 0); i += 2)
		{
		}
		Tokenizer.StringMaker maker = SharedStatics.GetSharedStringMaker();
		try
		{
			if (bCreate)
			{
				maker._outStringBuilder = null;
				maker._outIndex = 0;
				for (int j = position; j < i; j += 2)
				{
					char c = (char)((m_data[j] << 8) | m_data[j + 1]);
					if (maker._outIndex < 512)
					{
						maker._outChars[maker._outIndex++] = c;
						continue;
					}
					if (maker._outStringBuilder == null)
					{
						maker._outStringBuilder = new StringBuilder();
					}
					maker._outStringBuilder.Append(maker._outChars, 0, 512);
					maker._outChars[0] = c;
					maker._outIndex = 1;
				}
			}
			position = i + 2;
			if (bCreate)
			{
				return maker.MakeString();
			}
			return null;
		}
		finally
		{
			SharedStatics.ReleaseSharedStringMaker(ref maker);
		}
	}

	public void AddToken(byte b, ref int position)
	{
		GuaranteeSize(position + 1);
		m_data[position++] = b;
	}

	public void ConvertElement(SecurityElement elCurrent, ref int position)
	{
		AddToken(1, ref position);
		AddString(elCurrent.m_strTag, ref position);
		if (elCurrent.m_lAttributes != null)
		{
			for (int i = 0; i < elCurrent.m_lAttributes.Count; i += 2)
			{
				AddToken(2, ref position);
				AddString((string)elCurrent.m_lAttributes[i], ref position);
				AddString((string)elCurrent.m_lAttributes[i + 1], ref position);
			}
		}
		if (elCurrent.m_strText != null)
		{
			AddToken(3, ref position);
			AddString(elCurrent.m_strText, ref position);
		}
		if (elCurrent.InternalChildren != null)
		{
			for (int j = 0; j < elCurrent.InternalChildren.Count; j++)
			{
				ConvertElement((SecurityElement)elCurrent.Children[j], ref position);
			}
		}
		AddToken(4, ref position);
	}

	public SecurityElement GetRootElement()
	{
		return GetElement(0, bCreate: true);
	}

	public SecurityElement GetElement(int position, bool bCreate)
	{
		return InternalGetElement(ref position, bCreate);
	}

	internal SecurityElement InternalGetElement(ref int position, bool bCreate)
	{
		if (m_data.Length <= position)
		{
			throw new XmlSyntaxException();
		}
		if (m_data[position++] != 1)
		{
			throw new XmlSyntaxException();
		}
		SecurityElement securityElement = null;
		string tag = GetString(ref position, bCreate);
		if (bCreate)
		{
			securityElement = new SecurityElement(tag);
		}
		while (m_data[position] == 2)
		{
			position++;
			string name = GetString(ref position, bCreate);
			string value = GetString(ref position, bCreate);
			if (bCreate)
			{
				securityElement.AddAttribute(name, value);
			}
		}
		if (m_data[position] == 3)
		{
			position++;
			string strText = GetString(ref position, bCreate);
			if (bCreate)
			{
				securityElement.m_strText = strText;
			}
		}
		while (m_data[position] != 4)
		{
			SecurityElement child = InternalGetElement(ref position, bCreate);
			if (bCreate)
			{
				securityElement.AddChild(child);
			}
		}
		position++;
		return securityElement;
	}

	public string GetTagForElement(int position)
	{
		if (m_data.Length <= position)
		{
			throw new XmlSyntaxException();
		}
		if (m_data[position++] != 1)
		{
			throw new XmlSyntaxException();
		}
		return GetString(ref position);
	}

	public ArrayList GetChildrenPositionForElement(int position)
	{
		if (m_data.Length <= position)
		{
			throw new XmlSyntaxException();
		}
		if (m_data[position++] != 1)
		{
			throw new XmlSyntaxException();
		}
		ArrayList arrayList = new ArrayList();
		GetString(ref position);
		while (m_data[position] == 2)
		{
			position++;
			GetString(ref position, bCreate: false);
			GetString(ref position, bCreate: false);
		}
		if (m_data[position] == 3)
		{
			position++;
			GetString(ref position, bCreate: false);
		}
		while (m_data[position] != 4)
		{
			arrayList.Add(position);
			InternalGetElement(ref position, bCreate: false);
		}
		position++;
		return arrayList;
	}

	public string GetAttributeForElement(int position, string attributeName)
	{
		if (m_data.Length <= position)
		{
			throw new XmlSyntaxException();
		}
		if (m_data[position++] != 1)
		{
			throw new XmlSyntaxException();
		}
		string result = null;
		GetString(ref position, bCreate: false);
		while (m_data[position] == 2)
		{
			position++;
			string a = GetString(ref position);
			string text = GetString(ref position);
			if (string.Equals(a, attributeName))
			{
				result = text;
				break;
			}
		}
		return result;
	}
}
