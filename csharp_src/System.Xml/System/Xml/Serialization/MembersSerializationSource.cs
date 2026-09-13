using System.Globalization;
using System.Text;

namespace System.Xml.Serialization;

internal class MembersSerializationSource : SerializationSource
{
	private string elementName;

	private bool hasWrapperElement;

	private string membersHash;

	private bool literalFormat;

	public MembersSerializationSource(string elementName, bool hasWrapperElement, XmlReflectionMember[] members, bool writeAccessors, bool literalFormat, string namspace, Type[] includedTypes)
		: base(namspace, includedTypes)
	{
		this.elementName = elementName;
		this.hasWrapperElement = hasWrapperElement;
		this.literalFormat = literalFormat;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(members.Length.ToString(CultureInfo.InvariantCulture));
		for (int i = 0; i < members.Length; i++)
		{
			members[i].AddKeyHash(stringBuilder);
		}
		membersHash = stringBuilder.ToString();
	}

	public override bool Equals(object o)
	{
		if (!(o is MembersSerializationSource membersSerializationSource))
		{
			return false;
		}
		if (literalFormat = membersSerializationSource.literalFormat)
		{
			return false;
		}
		if (elementName != membersSerializationSource.elementName)
		{
			return false;
		}
		if (hasWrapperElement != membersSerializationSource.hasWrapperElement)
		{
			return false;
		}
		if (membersHash != membersSerializationSource.membersHash)
		{
			return false;
		}
		return BaseEquals(membersSerializationSource);
	}

	public override int GetHashCode()
	{
		return membersHash.GetHashCode();
	}
}
