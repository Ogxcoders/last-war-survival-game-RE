namespace System.Net.Http.Headers;

public sealed class HttpRequestHeaders : HttpHeaders
{
	private const int AcceptSlot = 0;

	private const int AcceptCharsetSlot = 1;

	private const int AcceptEncodingSlot = 2;

	private const int AcceptLanguageSlot = 3;

	private const int ExpectSlot = 4;

	private const int IfMatchSlot = 5;

	private const int IfNoneMatchSlot = 6;

	private const int TransferEncodingSlot = 7;

	private const int UserAgentSlot = 8;

	private const int NumCollectionsSlots = 9;

	private object[] _specialCollectionsSlots;

	private HttpGeneralHeaders _generalHeaders;

	private bool _expectContinueSet;

	public HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> Accept => GetSpecializedCollection(0, (HttpRequestHeaders thisRef) => new HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue>(KnownHeaders.Accept.Descriptor, thisRef));

	public HttpHeaderValueCollection<StringWithQualityHeaderValue> AcceptCharset => GetSpecializedCollection(1, (HttpRequestHeaders thisRef) => new HttpHeaderValueCollection<StringWithQualityHeaderValue>(KnownHeaders.AcceptCharset.Descriptor, thisRef));

	public HttpHeaderValueCollection<StringWithQualityHeaderValue> AcceptEncoding => GetSpecializedCollection(2, (HttpRequestHeaders thisRef) => new HttpHeaderValueCollection<StringWithQualityHeaderValue>(KnownHeaders.AcceptEncoding.Descriptor, thisRef));

	public HttpHeaderValueCollection<StringWithQualityHeaderValue> AcceptLanguage => GetSpecializedCollection(3, (HttpRequestHeaders thisRef) => new HttpHeaderValueCollection<StringWithQualityHeaderValue>(KnownHeaders.AcceptLanguage.Descriptor, thisRef));

	public AuthenticationHeaderValue Authorization
	{
		get
		{
			return (AuthenticationHeaderValue)GetParsedValues(KnownHeaders.Authorization.Descriptor);
		}
		set
		{
			SetOrRemoveParsedValue(KnownHeaders.Authorization.Descriptor, value);
		}
	}

	public HttpHeaderValueCollection<NameValueWithParametersHeaderValue> Expect => ExpectCore;

	public bool? ExpectContinue
	{
		get
		{
			if (ExpectCore.IsSpecialValueSet)
			{
				return true;
			}
			if (_expectContinueSet)
			{
				return false;
			}
			return null;
		}
		set
		{
			if (value == true)
			{
				_expectContinueSet = true;
				ExpectCore.SetSpecialValue();
			}
			else
			{
				_expectContinueSet = value.HasValue;
				ExpectCore.RemoveSpecialValue();
			}
		}
	}

	public string From
	{
		get
		{
			return (string)GetParsedValues(KnownHeaders.From.Descriptor);
		}
		set
		{
			if (value == string.Empty)
			{
				value = null;
			}
			if (value != null && !HeaderUtilities.IsValidEmailAddress(value))
			{
				throw new FormatException("The specified value is not a valid 'From' header string.");
			}
			SetOrRemoveParsedValue(KnownHeaders.From.Descriptor, value);
		}
	}

	public string Host
	{
		get
		{
			return (string)GetParsedValues(KnownHeaders.Host.Descriptor);
		}
		set
		{
			if (value == string.Empty)
			{
				value = null;
			}
			string host = null;
			if (value != null && HttpRuleParser.GetHostLength(value, 0, allowToken: false, out host) != value.Length)
			{
				throw new FormatException("The specified value is not a valid 'Host' header string.");
			}
			SetOrRemoveParsedValue(KnownHeaders.Host.Descriptor, value);
		}
	}

	public HttpHeaderValueCollection<EntityTagHeaderValue> IfMatch => GetSpecializedCollection(5, (HttpRequestHeaders thisRef) => new HttpHeaderValueCollection<EntityTagHeaderValue>(KnownHeaders.IfMatch.Descriptor, thisRef));

	public DateTimeOffset? IfModifiedSince
	{
		get
		{
			return HeaderUtilities.GetDateTimeOffsetValue(KnownHeaders.IfModifiedSince.Descriptor, this);
		}
		set
		{
			SetOrRemoveParsedValue(KnownHeaders.IfModifiedSince.Descriptor, value);
		}
	}

	public HttpHeaderValueCollection<EntityTagHeaderValue> IfNoneMatch => GetSpecializedCollection(6, (HttpRequestHeaders thisRef) => new HttpHeaderValueCollection<EntityTagHeaderValue>(KnownHeaders.IfNoneMatch.Descriptor, thisRef));

	public RangeConditionHeaderValue IfRange
	{
		get
		{
			return (RangeConditionHeaderValue)GetParsedValues(KnownHeaders.IfRange.Descriptor);
		}
		set
		{
			SetOrRemoveParsedValue(KnownHeaders.IfRange.Descriptor, value);
		}
	}

	public DateTimeOffset? IfUnmodifiedSince
	{
		get
		{
			return HeaderUtilities.GetDateTimeOffsetValue(KnownHeaders.IfUnmodifiedSince.Descriptor, this);
		}
		set
		{
			SetOrRemoveParsedValue(KnownHeaders.IfUnmodifiedSince.Descriptor, value);
		}
	}

	public int? MaxForwards
	{
		get
		{
			object parsedValues = GetParsedValues(KnownHeaders.MaxForwards.Descriptor);
			if (parsedValues != null)
			{
				return (int)parsedValues;
			}
			return null;
		}
		set
		{
			SetOrRemoveParsedValue(KnownHeaders.MaxForwards.Descriptor, value);
		}
	}

	public AuthenticationHeaderValue ProxyAuthorization
	{
		get
		{
			return (AuthenticationHeaderValue)GetParsedValues(KnownHeaders.ProxyAuthorization.Descriptor);
		}
		set
		{
			SetOrRemoveParsedValue(KnownHeaders.ProxyAuthorization.Descriptor, value);
		}
	}

	public RangeHeaderValue Range
	{
		get
		{
			return (RangeHeaderValue)GetParsedValues(KnownHeaders.Range.Descriptor);
		}
		set
		{
			SetOrRemoveParsedValue(KnownHeaders.Range.Descriptor, value);
		}
	}

	public Uri Referrer
	{
		get
		{
			return (Uri)GetParsedValues(KnownHeaders.Referer.Descriptor);
		}
		set
		{
			SetOrRemoveParsedValue(KnownHeaders.Referer.Descriptor, value);
		}
	}

	public HttpHeaderValueCollection<TransferCodingWithQualityHeaderValue> TE => GetSpecializedCollection(7, (HttpRequestHeaders thisRef) => new HttpHeaderValueCollection<TransferCodingWithQualityHeaderValue>(KnownHeaders.TE.Descriptor, thisRef));

	public HttpHeaderValueCollection<ProductInfoHeaderValue> UserAgent => GetSpecializedCollection(8, (HttpRequestHeaders thisRef) => new HttpHeaderValueCollection<ProductInfoHeaderValue>(KnownHeaders.UserAgent.Descriptor, thisRef));

	private HttpHeaderValueCollection<NameValueWithParametersHeaderValue> ExpectCore => GetSpecializedCollection(4, (HttpRequestHeaders thisRef) => new HttpHeaderValueCollection<NameValueWithParametersHeaderValue>(KnownHeaders.Expect.Descriptor, thisRef, HeaderUtilities.ExpectContinue));

	public CacheControlHeaderValue CacheControl
	{
		get
		{
			return GeneralHeaders.CacheControl;
		}
		set
		{
			GeneralHeaders.CacheControl = value;
		}
	}

	public HttpHeaderValueCollection<string> Connection => GeneralHeaders.Connection;

	public bool? ConnectionClose
	{
		get
		{
			return HttpGeneralHeaders.GetConnectionClose(this, _generalHeaders);
		}
		set
		{
			GeneralHeaders.ConnectionClose = value;
		}
	}

	public DateTimeOffset? Date
	{
		get
		{
			return GeneralHeaders.Date;
		}
		set
		{
			GeneralHeaders.Date = value;
		}
	}

	public HttpHeaderValueCollection<NameValueHeaderValue> Pragma => GeneralHeaders.Pragma;

	public HttpHeaderValueCollection<string> Trailer => GeneralHeaders.Trailer;

	public HttpHeaderValueCollection<TransferCodingHeaderValue> TransferEncoding => GeneralHeaders.TransferEncoding;

	public bool? TransferEncodingChunked
	{
		get
		{
			return HttpGeneralHeaders.GetTransferEncodingChunked(this, _generalHeaders);
		}
		set
		{
			GeneralHeaders.TransferEncodingChunked = value;
		}
	}

	public HttpHeaderValueCollection<ProductHeaderValue> Upgrade => GeneralHeaders.Upgrade;

	public HttpHeaderValueCollection<ViaHeaderValue> Via => GeneralHeaders.Via;

	public HttpHeaderValueCollection<WarningHeaderValue> Warning => GeneralHeaders.Warning;

	private HttpGeneralHeaders GeneralHeaders => _generalHeaders ?? (_generalHeaders = new HttpGeneralHeaders(this));

	private T GetSpecializedCollection<T>(int slot, Func<HttpRequestHeaders, T> creationFunc)
	{
		object[] array = _specialCollectionsSlots ?? (_specialCollectionsSlots = new object[9]);
		object obj = array[slot];
		if (obj == null)
		{
			obj = (array[slot] = creationFunc(this));
		}
		return (T)obj;
	}

	internal HttpRequestHeaders()
		: base(HttpHeaderType.General | HttpHeaderType.Request | HttpHeaderType.Custom, HttpHeaderType.Response)
	{
	}

	internal override void AddHeaders(HttpHeaders sourceHeaders)
	{
		base.AddHeaders(sourceHeaders);
		HttpRequestHeaders httpRequestHeaders = sourceHeaders as HttpRequestHeaders;
		if (httpRequestHeaders._generalHeaders != null)
		{
			GeneralHeaders.AddSpecialsFrom(httpRequestHeaders._generalHeaders);
		}
		if (!ExpectContinue.HasValue)
		{
			ExpectContinue = httpRequestHeaders.ExpectContinue;
		}
	}
}
