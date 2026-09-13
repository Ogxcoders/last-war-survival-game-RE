using System.Text;

namespace System.Net.Http.Headers;

internal sealed class KnownHeader
{
	private readonly string _name;

	private readonly HttpHeaderType _headerType;

	private readonly HttpHeaderParser _parser;

	private readonly string[] _knownValues;

	private readonly byte[] _asciiBytesWithColonSpace;

	public string Name => _name;

	public HttpHeaderParser Parser => _parser;

	public HttpHeaderType HeaderType => _headerType;

	public string[] KnownValues => _knownValues;

	public byte[] AsciiBytesWithColonSpace => _asciiBytesWithColonSpace;

	public HeaderDescriptor Descriptor => new HeaderDescriptor(this);

	public KnownHeader(string name)
		: this(name, HttpHeaderType.Custom, null)
	{
	}

	public KnownHeader(string name, HttpHeaderType headerType, HttpHeaderParser parser, string[] knownValues = null)
	{
		_name = name;
		_headerType = headerType;
		_parser = parser;
		_knownValues = knownValues;
		_asciiBytesWithColonSpace = new byte[name.Length + 2];
		Encoding.ASCII.GetBytes(name, _asciiBytesWithColonSpace);
		_asciiBytesWithColonSpace[_asciiBytesWithColonSpace.Length - 2] = 58;
		_asciiBytesWithColonSpace[_asciiBytesWithColonSpace.Length - 1] = 32;
	}
}
