namespace System.Net.Http.Headers;

internal class MediaTypeHeaderParser : BaseHeaderParser
{
	private bool _supportsMultipleValues;

	private Func<MediaTypeHeaderValue> _mediaTypeCreator;

	internal static readonly MediaTypeHeaderParser SingleValueParser = new MediaTypeHeaderParser(supportsMultipleValues: false, CreateMediaType);

	internal static readonly MediaTypeHeaderParser SingleValueWithQualityParser = new MediaTypeHeaderParser(supportsMultipleValues: false, CreateMediaTypeWithQuality);

	internal static readonly MediaTypeHeaderParser MultipleValuesParser = new MediaTypeHeaderParser(supportsMultipleValues: true, CreateMediaTypeWithQuality);

	private MediaTypeHeaderParser(bool supportsMultipleValues, Func<MediaTypeHeaderValue> mediaTypeCreator)
		: base(supportsMultipleValues)
	{
		_supportsMultipleValues = supportsMultipleValues;
		_mediaTypeCreator = mediaTypeCreator;
	}

	protected override int GetParsedValueLength(string value, int startIndex, object storeValue, out object parsedValue)
	{
		MediaTypeHeaderValue parsedValue2 = null;
		int mediaTypeLength = MediaTypeHeaderValue.GetMediaTypeLength(value, startIndex, _mediaTypeCreator, out parsedValue2);
		parsedValue = parsedValue2;
		return mediaTypeLength;
	}

	private static MediaTypeHeaderValue CreateMediaType()
	{
		return new MediaTypeHeaderValue();
	}

	private static MediaTypeHeaderValue CreateMediaTypeWithQuality()
	{
		return new MediaTypeWithQualityHeaderValue();
	}
}
