using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace System.Net.Http.Headers;

public abstract class HttpHeaders : IEnumerable<KeyValuePair<string, IEnumerable<string>>>, IEnumerable
{
	private enum StoreLocation
	{
		Raw,
		Invalid,
		Parsed
	}

	private class HeaderStoreItemInfo
	{
		private object _rawValue;

		private object _invalidValue;

		private object _parsedValue;

		internal object RawValue
		{
			get
			{
				return _rawValue;
			}
			set
			{
				_rawValue = value;
			}
		}

		internal object InvalidValue
		{
			get
			{
				return _invalidValue;
			}
			set
			{
				_invalidValue = value;
			}
		}

		internal object ParsedValue
		{
			get
			{
				return _parsedValue;
			}
			set
			{
				_parsedValue = value;
			}
		}

		internal bool IsEmpty
		{
			get
			{
				if (_rawValue == null && _invalidValue == null)
				{
					return _parsedValue == null;
				}
				return false;
			}
		}

		internal bool CanAddValue(HttpHeaderParser parser)
		{
			if (!parser.SupportsMultipleValues)
			{
				if (_invalidValue == null)
				{
					return _parsedValue == null;
				}
				return false;
			}
			return true;
		}

		internal HeaderStoreItemInfo()
		{
		}
	}

	private Dictionary<HeaderDescriptor, HeaderStoreItemInfo> _headerStore;

	private readonly HttpHeaderType _allowedHeaderTypes;

	private readonly HttpHeaderType _treatAsCustomHeaderTypes;

	protected HttpHeaders()
		: this(HttpHeaderType.All, HttpHeaderType.None)
	{
	}

	internal HttpHeaders(HttpHeaderType allowedHeaderTypes, HttpHeaderType treatAsCustomHeaderTypes)
	{
		_allowedHeaderTypes = allowedHeaderTypes;
		_treatAsCustomHeaderTypes = treatAsCustomHeaderTypes;
	}

	public void Add(string name, string value)
	{
		Add(GetHeaderDescriptor(name), value);
	}

	internal void Add(HeaderDescriptor descriptor, string value)
	{
		PrepareHeaderInfoForAdd(descriptor, out var info, out var addToStore);
		ParseAndAddValue(descriptor, info, value);
		if (addToStore && info.ParsedValue != null)
		{
			AddHeaderToStore(descriptor, info);
		}
	}

	public void Add(string name, IEnumerable<string> values)
	{
		Add(GetHeaderDescriptor(name), values);
	}

	internal void Add(HeaderDescriptor descriptor, IEnumerable<string> values)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		PrepareHeaderInfoForAdd(descriptor, out var info, out var addToStore);
		try
		{
			foreach (string value in values)
			{
				ParseAndAddValue(descriptor, info, value);
			}
		}
		finally
		{
			if (addToStore && info.ParsedValue != null)
			{
				AddHeaderToStore(descriptor, info);
			}
		}
	}

	public bool TryAddWithoutValidation(string name, string value)
	{
		if (TryGetHeaderDescriptor(name, out var descriptor))
		{
			return TryAddWithoutValidation(descriptor, value);
		}
		return false;
	}

	internal bool TryAddWithoutValidation(HeaderDescriptor descriptor, string value)
	{
		if (value == null)
		{
			value = string.Empty;
		}
		AddValue(GetOrCreateHeaderInfo(descriptor, parseRawValues: false), value, StoreLocation.Raw);
		return true;
	}

	public bool TryAddWithoutValidation(string name, IEnumerable<string> values)
	{
		if (TryGetHeaderDescriptor(name, out var descriptor))
		{
			return TryAddWithoutValidation(descriptor, values);
		}
		return false;
	}

	internal bool TryAddWithoutValidation(HeaderDescriptor descriptor, IEnumerable<string> values)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		HeaderStoreItemInfo orCreateHeaderInfo = GetOrCreateHeaderInfo(descriptor, parseRawValues: false);
		foreach (string value in values)
		{
			AddValue(orCreateHeaderInfo, value ?? string.Empty, StoreLocation.Raw);
		}
		return true;
	}

	public void Clear()
	{
		if (_headerStore != null)
		{
			_headerStore.Clear();
		}
	}

	public bool Remove(string name)
	{
		return Remove(GetHeaderDescriptor(name));
	}

	public IEnumerable<string> GetValues(string name)
	{
		return GetValues(GetHeaderDescriptor(name));
	}

	internal IEnumerable<string> GetValues(HeaderDescriptor descriptor)
	{
		if (!TryGetValues(descriptor, out var values))
		{
			throw new InvalidOperationException("The given header was not found.");
		}
		return values;
	}

	public bool TryGetValues(string name, out IEnumerable<string> values)
	{
		if (!TryGetHeaderDescriptor(name, out var descriptor))
		{
			values = null;
			return false;
		}
		return TryGetValues(descriptor, out values);
	}

	internal bool TryGetValues(HeaderDescriptor descriptor, out IEnumerable<string> values)
	{
		if (_headerStore == null)
		{
			values = null;
			return false;
		}
		HeaderStoreItemInfo info = null;
		if (TryGetAndParseHeaderInfo(descriptor, out info))
		{
			values = GetValuesAsStrings(descriptor, info);
			return true;
		}
		values = null;
		return false;
	}

	public bool Contains(string name)
	{
		return Contains(GetHeaderDescriptor(name));
	}

	internal bool Contains(HeaderDescriptor descriptor)
	{
		if (_headerStore == null)
		{
			return false;
		}
		HeaderStoreItemInfo info = null;
		return TryGetAndParseHeaderInfo(descriptor, out info);
	}

	public override string ToString()
	{
		if (_headerStore == null || _headerStore.Count == 0)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, string> headerString in GetHeaderStrings())
		{
			stringBuilder.Append(headerString.Key);
			stringBuilder.Append(": ");
			stringBuilder.Append(headerString.Value);
			stringBuilder.Append("\r\n");
		}
		return stringBuilder.ToString();
	}

	internal IEnumerable<KeyValuePair<string, string>> GetHeaderStrings()
	{
		if (_headerStore == null)
		{
			yield break;
		}
		foreach (KeyValuePair<HeaderDescriptor, HeaderStoreItemInfo> item in _headerStore)
		{
			string headerString = GetHeaderString(item.Key, item.Value);
			yield return new KeyValuePair<string, string>(item.Key.Name, headerString);
		}
	}

	internal string GetHeaderString(string name)
	{
		if (!TryGetHeaderDescriptor(name, out var descriptor))
		{
			return string.Empty;
		}
		return GetHeaderString(descriptor);
	}

	internal string GetHeaderString(HeaderDescriptor descriptor, object exclude = null)
	{
		if (!TryGetHeaderInfo(descriptor, out var info))
		{
			return string.Empty;
		}
		return GetHeaderString(descriptor, info, exclude);
	}

	private string GetHeaderString(HeaderDescriptor descriptor, HeaderStoreItemInfo info, object exclude = null)
	{
		string[] valuesAsStrings = GetValuesAsStrings(descriptor, info, exclude);
		if (valuesAsStrings.Length == 1)
		{
			return valuesAsStrings[0];
		}
		string separator = ", ";
		if (descriptor.Parser != null && descriptor.Parser.SupportsMultipleValues)
		{
			separator = descriptor.Parser.Separator;
		}
		return string.Join(separator, valuesAsStrings);
	}

	public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator()
	{
		if (_headerStore == null || _headerStore.Count <= 0)
		{
			return ((IEnumerable<KeyValuePair<string, IEnumerable<string>>>)Array.Empty<KeyValuePair<string, IEnumerable<string>>>()).GetEnumerator();
		}
		return GetEnumeratorCore();
	}

	private IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumeratorCore()
	{
		List<HeaderDescriptor> invalidHeaders = null;
		foreach (KeyValuePair<HeaderDescriptor, HeaderStoreItemInfo> item in _headerStore)
		{
			HeaderDescriptor key = item.Key;
			HeaderStoreItemInfo value = item.Value;
			if (!ParseRawHeaderValues(key, value, removeEmptyHeader: false))
			{
				if (invalidHeaders == null)
				{
					invalidHeaders = new List<HeaderDescriptor>();
				}
				invalidHeaders.Add(key);
			}
			else
			{
				string[] valuesAsStrings = GetValuesAsStrings(key, value);
				yield return new KeyValuePair<string, IEnumerable<string>>(key.Name, valuesAsStrings);
			}
		}
		if (invalidHeaders == null)
		{
			yield break;
		}
		foreach (HeaderDescriptor item2 in invalidHeaders)
		{
			_headerStore.Remove(item2);
		}
	}

	internal IEnumerable<KeyValuePair<HeaderDescriptor, string[]>> GetHeaderDescriptorsAndValues()
	{
		if (_headerStore == null || _headerStore.Count <= 0)
		{
			return Array.Empty<KeyValuePair<HeaderDescriptor, string[]>>();
		}
		return GetHeaderDescriptorsAndValuesCore();
	}

	private IEnumerable<KeyValuePair<HeaderDescriptor, string[]>> GetHeaderDescriptorsAndValuesCore()
	{
		List<HeaderDescriptor> invalidHeaders = null;
		foreach (KeyValuePair<HeaderDescriptor, HeaderStoreItemInfo> item in _headerStore)
		{
			HeaderDescriptor key = item.Key;
			HeaderStoreItemInfo value = item.Value;
			if (!ParseRawHeaderValues(key, value, removeEmptyHeader: false))
			{
				if (invalidHeaders == null)
				{
					invalidHeaders = new List<HeaderDescriptor>();
				}
				invalidHeaders.Add(key);
			}
			else
			{
				string[] valuesAsStrings = GetValuesAsStrings(key, value);
				yield return new KeyValuePair<HeaderDescriptor, string[]>(key, valuesAsStrings);
			}
		}
		if (invalidHeaders == null)
		{
			yield break;
		}
		foreach (HeaderDescriptor item2 in invalidHeaders)
		{
			_headerStore.Remove(item2);
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	internal void AddParsedValue(HeaderDescriptor descriptor, object value)
	{
		AddValue(GetOrCreateHeaderInfo(descriptor, parseRawValues: true), value, StoreLocation.Parsed);
	}

	internal void SetParsedValue(HeaderDescriptor descriptor, object value)
	{
		HeaderStoreItemInfo orCreateHeaderInfo = GetOrCreateHeaderInfo(descriptor, parseRawValues: true);
		orCreateHeaderInfo.InvalidValue = null;
		orCreateHeaderInfo.ParsedValue = null;
		orCreateHeaderInfo.RawValue = null;
		AddValue(orCreateHeaderInfo, value, StoreLocation.Parsed);
	}

	internal void SetOrRemoveParsedValue(HeaderDescriptor descriptor, object value)
	{
		if (value == null)
		{
			Remove(descriptor);
		}
		else
		{
			SetParsedValue(descriptor, value);
		}
	}

	internal bool Remove(HeaderDescriptor descriptor)
	{
		if (_headerStore == null)
		{
			return false;
		}
		return _headerStore.Remove(descriptor);
	}

	internal bool RemoveParsedValue(HeaderDescriptor descriptor, object value)
	{
		if (_headerStore == null)
		{
			return false;
		}
		HeaderStoreItemInfo info = null;
		if (TryGetAndParseHeaderInfo(descriptor, out info))
		{
			bool result = false;
			if (info.ParsedValue == null)
			{
				return false;
			}
			IEqualityComparer comparer = descriptor.Parser.Comparer;
			if (!(info.ParsedValue is List<object> list))
			{
				if (AreEqual(value, info.ParsedValue, comparer))
				{
					info.ParsedValue = null;
					result = true;
				}
			}
			else
			{
				foreach (object item in list)
				{
					if (AreEqual(value, item, comparer))
					{
						result = list.Remove(item);
						break;
					}
				}
				if (list.Count == 0)
				{
					info.ParsedValue = null;
				}
			}
			if (info.IsEmpty)
			{
				Remove(descriptor);
			}
			return result;
		}
		return false;
	}

	internal bool ContainsParsedValue(HeaderDescriptor descriptor, object value)
	{
		if (_headerStore == null)
		{
			return false;
		}
		HeaderStoreItemInfo info = null;
		if (TryGetAndParseHeaderInfo(descriptor, out info))
		{
			if (info.ParsedValue == null)
			{
				return false;
			}
			List<object> list = info.ParsedValue as List<object>;
			IEqualityComparer comparer = descriptor.Parser.Comparer;
			if (list == null)
			{
				return AreEqual(value, info.ParsedValue, comparer);
			}
			foreach (object item in list)
			{
				if (AreEqual(value, item, comparer))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	internal virtual void AddHeaders(HttpHeaders sourceHeaders)
	{
		if (sourceHeaders._headerStore == null)
		{
			return;
		}
		List<HeaderDescriptor> list = null;
		foreach (KeyValuePair<HeaderDescriptor, HeaderStoreItemInfo> item in sourceHeaders._headerStore)
		{
			if (_headerStore != null && _headerStore.ContainsKey(item.Key))
			{
				continue;
			}
			HeaderStoreItemInfo value = item.Value;
			if (!sourceHeaders.ParseRawHeaderValues(item.Key, value, removeEmptyHeader: false))
			{
				if (list == null)
				{
					list = new List<HeaderDescriptor>();
				}
				list.Add(item.Key);
			}
			else
			{
				AddHeaderInfo(item.Key, value);
			}
		}
		if (list == null)
		{
			return;
		}
		foreach (HeaderDescriptor item2 in list)
		{
			sourceHeaders._headerStore.Remove(item2);
		}
	}

	private void AddHeaderInfo(HeaderDescriptor descriptor, HeaderStoreItemInfo sourceInfo)
	{
		HeaderStoreItemInfo headerStoreItemInfo = CreateAndAddHeaderToStore(descriptor);
		if (descriptor.Parser == null)
		{
			headerStoreItemInfo.ParsedValue = CloneStringHeaderInfoValues(sourceInfo.ParsedValue);
			return;
		}
		headerStoreItemInfo.InvalidValue = CloneStringHeaderInfoValues(sourceInfo.InvalidValue);
		if (sourceInfo.ParsedValue == null)
		{
			return;
		}
		if (!(sourceInfo.ParsedValue is List<object> list))
		{
			CloneAndAddValue(headerStoreItemInfo, sourceInfo.ParsedValue);
			return;
		}
		foreach (object item in list)
		{
			CloneAndAddValue(headerStoreItemInfo, item);
		}
	}

	private static void CloneAndAddValue(HeaderStoreItemInfo destinationInfo, object source)
	{
		if (source is ICloneable cloneable)
		{
			AddValue(destinationInfo, cloneable.Clone(), StoreLocation.Parsed);
		}
		else
		{
			AddValue(destinationInfo, source, StoreLocation.Parsed);
		}
	}

	private static object CloneStringHeaderInfoValues(object source)
	{
		if (source == null)
		{
			return null;
		}
		if (!(source is List<object> collection))
		{
			return source;
		}
		return new List<object>(collection);
	}

	private HeaderStoreItemInfo GetOrCreateHeaderInfo(HeaderDescriptor descriptor, bool parseRawValues)
	{
		HeaderStoreItemInfo info = null;
		bool flag = false;
		if (!((!parseRawValues) ? TryGetHeaderInfo(descriptor, out info) : TryGetAndParseHeaderInfo(descriptor, out info)))
		{
			info = CreateAndAddHeaderToStore(descriptor);
		}
		return info;
	}

	private HeaderStoreItemInfo CreateAndAddHeaderToStore(HeaderDescriptor descriptor)
	{
		HeaderStoreItemInfo headerStoreItemInfo = new HeaderStoreItemInfo();
		AddHeaderToStore(descriptor, headerStoreItemInfo);
		return headerStoreItemInfo;
	}

	private void AddHeaderToStore(HeaderDescriptor descriptor, HeaderStoreItemInfo info)
	{
		if (_headerStore == null)
		{
			_headerStore = new Dictionary<HeaderDescriptor, HeaderStoreItemInfo>();
		}
		_headerStore.Add(descriptor, info);
	}

	private bool TryGetHeaderInfo(HeaderDescriptor descriptor, out HeaderStoreItemInfo info)
	{
		if (_headerStore == null)
		{
			info = null;
			return false;
		}
		return _headerStore.TryGetValue(descriptor, out info);
	}

	private bool TryGetAndParseHeaderInfo(HeaderDescriptor key, out HeaderStoreItemInfo info)
	{
		if (TryGetHeaderInfo(key, out info))
		{
			return ParseRawHeaderValues(key, info, removeEmptyHeader: true);
		}
		return false;
	}

	private bool ParseRawHeaderValues(HeaderDescriptor descriptor, HeaderStoreItemInfo info, bool removeEmptyHeader)
	{
		lock (info)
		{
			if (info.RawValue != null)
			{
				if (!(info.RawValue is List<string> rawValues))
				{
					ParseSingleRawHeaderValue(descriptor, info);
				}
				else
				{
					ParseMultipleRawHeaderValues(descriptor, info, rawValues);
				}
				info.RawValue = null;
				if (info.InvalidValue == null && info.ParsedValue == null)
				{
					if (removeEmptyHeader)
					{
						_headerStore.Remove(descriptor);
					}
					return false;
				}
			}
		}
		return true;
	}

	private static void ParseMultipleRawHeaderValues(HeaderDescriptor descriptor, HeaderStoreItemInfo info, List<string> rawValues)
	{
		if (descriptor.Parser == null)
		{
			foreach (string rawValue in rawValues)
			{
				if (!ContainsInvalidNewLine(rawValue, descriptor.Name))
				{
					AddValue(info, rawValue, StoreLocation.Parsed);
				}
			}
			return;
		}
		foreach (string rawValue2 in rawValues)
		{
			if (!TryParseAndAddRawHeaderValue(descriptor, info, rawValue2, addWhenInvalid: true) && NetEventSource.IsEnabled)
			{
				NetEventSource.Log.HeadersInvalidValue(descriptor.Name, rawValue2);
			}
		}
	}

	private static void ParseSingleRawHeaderValue(HeaderDescriptor descriptor, HeaderStoreItemInfo info)
	{
		string text = info.RawValue as string;
		if (descriptor.Parser == null)
		{
			if (!ContainsInvalidNewLine(text, descriptor.Name))
			{
				AddValue(info, text, StoreLocation.Parsed);
			}
		}
		else if (!TryParseAndAddRawHeaderValue(descriptor, info, text, addWhenInvalid: true) && NetEventSource.IsEnabled)
		{
			NetEventSource.Log.HeadersInvalidValue(descriptor.Name, text);
		}
	}

	internal bool TryParseAndAddValue(HeaderDescriptor descriptor, string value)
	{
		PrepareHeaderInfoForAdd(descriptor, out var info, out var addToStore);
		bool num = TryParseAndAddRawHeaderValue(descriptor, info, value, addWhenInvalid: false);
		if (num && addToStore && info.ParsedValue != null)
		{
			AddHeaderToStore(descriptor, info);
		}
		return num;
	}

	private static bool TryParseAndAddRawHeaderValue(HeaderDescriptor descriptor, HeaderStoreItemInfo info, string value, bool addWhenInvalid)
	{
		if (!info.CanAddValue(descriptor.Parser))
		{
			if (addWhenInvalid)
			{
				AddValue(info, value ?? string.Empty, StoreLocation.Invalid);
			}
			return false;
		}
		int index = 0;
		object parsedValue = null;
		if (descriptor.Parser.TryParseValue(value, info.ParsedValue, ref index, out parsedValue))
		{
			if (value == null || index == value.Length)
			{
				if (parsedValue != null)
				{
					AddValue(info, parsedValue, StoreLocation.Parsed);
				}
				return true;
			}
			List<object> list = new List<object>();
			if (parsedValue != null)
			{
				list.Add(parsedValue);
			}
			while (index < value.Length)
			{
				if (descriptor.Parser.TryParseValue(value, info.ParsedValue, ref index, out parsedValue))
				{
					if (parsedValue != null)
					{
						list.Add(parsedValue);
					}
					continue;
				}
				if (!ContainsInvalidNewLine(value, descriptor.Name) && addWhenInvalid)
				{
					AddValue(info, value, StoreLocation.Invalid);
				}
				return false;
			}
			foreach (object item in list)
			{
				AddValue(info, item, StoreLocation.Parsed);
			}
			return true;
		}
		if (!ContainsInvalidNewLine(value, descriptor.Name) && addWhenInvalid)
		{
			AddValue(info, value ?? string.Empty, StoreLocation.Invalid);
		}
		return false;
	}

	private static void AddValue(HeaderStoreItemInfo info, object value, StoreLocation location)
	{
		object obj = null;
		switch (location)
		{
		case StoreLocation.Raw:
			obj = info.RawValue;
			AddValueToStoreValue<string>(value, ref obj);
			info.RawValue = obj;
			break;
		case StoreLocation.Invalid:
			obj = info.InvalidValue;
			AddValueToStoreValue<string>(value, ref obj);
			info.InvalidValue = obj;
			break;
		case StoreLocation.Parsed:
			obj = info.ParsedValue;
			AddValueToStoreValue<object>(value, ref obj);
			info.ParsedValue = obj;
			break;
		}
	}

	private static void AddValueToStoreValue<T>(object value, ref object currentStoreValue) where T : class
	{
		if (currentStoreValue == null)
		{
			currentStoreValue = value;
			return;
		}
		List<T> list = currentStoreValue as List<T>;
		if (list == null)
		{
			list = new List<T>(2);
			list.Add(currentStoreValue as T);
			currentStoreValue = list;
		}
		list.Add(value as T);
	}

	internal object GetParsedValues(HeaderDescriptor descriptor)
	{
		HeaderStoreItemInfo info = null;
		if (!TryGetAndParseHeaderInfo(descriptor, out info))
		{
			return null;
		}
		return info.ParsedValue;
	}

	private void PrepareHeaderInfoForAdd(HeaderDescriptor descriptor, out HeaderStoreItemInfo info, out bool addToStore)
	{
		info = null;
		addToStore = false;
		if (!TryGetAndParseHeaderInfo(descriptor, out info))
		{
			info = new HeaderStoreItemInfo();
			addToStore = true;
		}
	}

	private void ParseAndAddValue(HeaderDescriptor descriptor, HeaderStoreItemInfo info, string value)
	{
		if (descriptor.Parser == null)
		{
			CheckInvalidNewLine(value);
			AddValue(info, value ?? string.Empty, StoreLocation.Parsed);
			return;
		}
		if (!info.CanAddValue(descriptor.Parser))
		{
			throw new FormatException(string.Format(CultureInfo.InvariantCulture, "Cannot add value because header '{0}' does not support multiple values.", descriptor.Name));
		}
		int index = 0;
		object obj = descriptor.Parser.ParseValue(value, info.ParsedValue, ref index);
		if (value == null || index == value.Length)
		{
			if (obj != null)
			{
				AddValue(info, obj, StoreLocation.Parsed);
			}
			return;
		}
		List<object> list = new List<object>();
		if (obj != null)
		{
			list.Add(obj);
		}
		while (index < value.Length)
		{
			obj = descriptor.Parser.ParseValue(value, info.ParsedValue, ref index);
			if (obj != null)
			{
				list.Add(obj);
			}
		}
		foreach (object item in list)
		{
			AddValue(info, item, StoreLocation.Parsed);
		}
	}

	private HeaderDescriptor GetHeaderDescriptor(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("The value cannot be null or empty.", "name");
		}
		if (!HeaderDescriptor.TryGet(name, out var descriptor))
		{
			throw new FormatException("The header name format is invalid.");
		}
		if ((descriptor.HeaderType & _allowedHeaderTypes) != HttpHeaderType.None)
		{
			return descriptor;
		}
		if ((descriptor.HeaderType & _treatAsCustomHeaderTypes) != HttpHeaderType.None)
		{
			return descriptor.AsCustomHeader();
		}
		throw new InvalidOperationException("Misused header name. Make sure request headers are used with HttpRequestMessage, response headers with HttpResponseMessage, and content headers with HttpContent objects.");
	}

	private bool TryGetHeaderDescriptor(string name, out HeaderDescriptor descriptor)
	{
		if (string.IsNullOrEmpty(name))
		{
			descriptor = default(HeaderDescriptor);
			return false;
		}
		if (!HeaderDescriptor.TryGet(name, out descriptor))
		{
			return false;
		}
		if ((descriptor.HeaderType & _allowedHeaderTypes) != HttpHeaderType.None)
		{
			return true;
		}
		if ((descriptor.HeaderType & _treatAsCustomHeaderTypes) != HttpHeaderType.None)
		{
			descriptor = descriptor.AsCustomHeader();
			return true;
		}
		return false;
	}

	private static void CheckInvalidNewLine(string value)
	{
		if (value == null || !HttpRuleParser.ContainsInvalidNewLine(value))
		{
			return;
		}
		throw new FormatException("New-line characters in header values must be followed by a white-space character.");
	}

	private static bool ContainsInvalidNewLine(string value, string name)
	{
		if (HttpRuleParser.ContainsInvalidNewLine(value))
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(null, global::SR.Format("Value for header '{0}' contains invalid new-line characters. Value: '{1}'.", name, value));
			}
			return true;
		}
		return false;
	}

	private static string[] GetValuesAsStrings(HeaderDescriptor descriptor, HeaderStoreItemInfo info, object exclude = null)
	{
		int valueCount = GetValueCount(info);
		string[] array;
		if (valueCount > 0)
		{
			array = new string[valueCount];
			int currentIndex = 0;
			ReadStoreValues<string>(array, info.RawValue, null, null, ref currentIndex);
			ReadStoreValues(array, info.ParsedValue, descriptor.Parser, exclude, ref currentIndex);
			ReadStoreValues<string>(array, info.InvalidValue, null, null, ref currentIndex);
			if (currentIndex < valueCount)
			{
				string[] array2 = new string[currentIndex];
				Array.Copy(array, 0, array2, 0, currentIndex);
				array = array2;
			}
		}
		else
		{
			array = Array.Empty<string>();
		}
		return array;
	}

	private static int GetValueCount(HeaderStoreItemInfo info)
	{
		int valueCount = 0;
		UpdateValueCount<string>(info.RawValue, ref valueCount);
		UpdateValueCount<string>(info.InvalidValue, ref valueCount);
		UpdateValueCount<object>(info.ParsedValue, ref valueCount);
		return valueCount;
	}

	private static void UpdateValueCount<T>(object valueStore, ref int valueCount)
	{
		if (valueStore != null)
		{
			if (valueStore is List<T> list)
			{
				valueCount += list.Count;
			}
			else
			{
				valueCount++;
			}
		}
	}

	private static void ReadStoreValues<T>(string[] values, object storeValue, HttpHeaderParser parser, T exclude, ref int currentIndex)
	{
		if (storeValue == null)
		{
			return;
		}
		if (!(storeValue is List<T> list))
		{
			if (ShouldAdd(storeValue, parser, exclude))
			{
				values[currentIndex] = ((parser == null) ? storeValue.ToString() : parser.ToString(storeValue));
				currentIndex++;
			}
			return;
		}
		foreach (T item in list)
		{
			object obj = item;
			if (ShouldAdd(obj, parser, exclude))
			{
				values[currentIndex] = ((parser == null) ? obj.ToString() : parser.ToString(obj));
				currentIndex++;
			}
		}
	}

	private static bool ShouldAdd<T>(object storeValue, HttpHeaderParser parser, T exclude)
	{
		bool result = true;
		if (parser != null && exclude != null)
		{
			result = ((parser.Comparer == null) ? (!exclude.Equals(storeValue)) : (!parser.Comparer.Equals(exclude, storeValue)));
		}
		return result;
	}

	private bool AreEqual(object value, object storeValue, IEqualityComparer comparer)
	{
		return comparer?.Equals(value, storeValue) ?? value.Equals(storeValue);
	}
}
