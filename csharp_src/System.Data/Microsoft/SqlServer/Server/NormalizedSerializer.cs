using System;
using System.IO;

namespace Microsoft.SqlServer.Server;

internal sealed class NormalizedSerializer : Serializer
{
	private BinaryOrderedUdtNormalizer _normalizer;

	private bool _isFixedSize;

	private int _maxSize;

	internal NormalizedSerializer(Type t)
		: base(t)
	{
		SqlUserDefinedTypeAttribute udtAttribute = SerializationHelperSql9.GetUdtAttribute(t);
		_normalizer = new BinaryOrderedUdtNormalizer(t, isTopLevelUdt: true);
		_isFixedSize = udtAttribute.IsFixedLength;
		_maxSize = _normalizer.Size;
	}

	public override void Serialize(Stream s, object o)
	{
		_normalizer.NormalizeTopObject(o, s);
	}

	public override object Deserialize(Stream s)
	{
		return _normalizer.DeNormalizeTopObject(_type, s);
	}
}
