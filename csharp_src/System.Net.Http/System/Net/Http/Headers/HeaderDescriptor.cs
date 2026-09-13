namespace System.Net.Http.Headers;

internal readonly struct HeaderDescriptor : IEquatable<HeaderDescriptor>
{
	private readonly string _headerName;

	private readonly KnownHeader _knownHeader;

	public string Name => _headerName;

	public HttpHeaderParser Parser => _knownHeader?.Parser;

	public HttpHeaderType HeaderType
	{
		get
		{
			if (_knownHeader != null)
			{
				return _knownHeader.HeaderType;
			}
			return HttpHeaderType.Custom;
		}
	}

	public KnownHeader KnownHeader => _knownHeader;

	public HeaderDescriptor(KnownHeader knownHeader)
	{
		_knownHeader = knownHeader;
		_headerName = knownHeader.Name;
	}

	private HeaderDescriptor(string headerName)
	{
		_headerName = headerName;
		_knownHeader = null;
	}

	public bool Equals(HeaderDescriptor other)
	{
		if (_knownHeader != null)
		{
			return _knownHeader == other._knownHeader;
		}
		return string.Equals(_headerName, other._headerName, StringComparison.OrdinalIgnoreCase);
	}

	public override int GetHashCode()
	{
		return _knownHeader?.GetHashCode() ?? StringComparer.OrdinalIgnoreCase.GetHashCode(_headerName);
	}

	public override bool Equals(object obj)
	{
		throw new InvalidOperationException();
	}

	public static bool operator ==(HeaderDescriptor left, HeaderDescriptor right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(HeaderDescriptor left, HeaderDescriptor right)
	{
		return !left.Equals(right);
	}

	public static bool TryGet(string headerName, out HeaderDescriptor descriptor)
	{
		KnownHeader knownHeader = KnownHeaders.TryGetKnownHeader(headerName);
		if (knownHeader != null)
		{
			descriptor = new HeaderDescriptor(knownHeader);
			return true;
		}
		if (!HttpRuleParser.IsToken(headerName))
		{
			descriptor = default(HeaderDescriptor);
			return false;
		}
		descriptor = new HeaderDescriptor(headerName);
		return true;
	}

	public static bool TryGet(ReadOnlySpan<byte> headerName, out HeaderDescriptor descriptor)
	{
		KnownHeader knownHeader = KnownHeaders.TryGetKnownHeader(headerName);
		if (knownHeader != null)
		{
			descriptor = new HeaderDescriptor(knownHeader);
			return true;
		}
		if (!HttpRuleParser.IsToken(headerName))
		{
			descriptor = default(HeaderDescriptor);
			return false;
		}
		descriptor = new HeaderDescriptor(HttpRuleParser.GetTokenString(headerName));
		return true;
	}

	public HeaderDescriptor AsCustomHeader()
	{
		return new HeaderDescriptor(_knownHeader.Name);
	}

	public string GetHeaderValue(ReadOnlySpan<byte> headerValue)
	{
		if (headerValue.Length == 0)
		{
			return string.Empty;
		}
		if (_knownHeader != null && _knownHeader.KnownValues != null)
		{
			string[] knownValues = _knownHeader.KnownValues;
			for (int i = 0; i < knownValues.Length; i++)
			{
				if (ByteArrayHelpers.EqualsOrdinalAsciiIgnoreCase(knownValues[i], headerValue))
				{
					return knownValues[i];
				}
			}
		}
		return HttpRuleParser.DefaultHttpEncoding.GetString(headerValue);
	}
}
