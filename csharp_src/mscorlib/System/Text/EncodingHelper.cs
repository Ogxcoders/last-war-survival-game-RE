using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.Text;

internal static class EncodingHelper
{
	private static volatile Encoding utf8EncodingWithoutMarkers;

	private static volatile Encoding utf8EncodingUnsafe;

	private static volatile Encoding bigEndianUTF32Encoding;

	private static readonly object lockobj = new object();

	private static Assembly i18nAssembly;

	private static bool i18nDisabled;

	internal static Encoding UTF8Unmarked
	{
		get
		{
			if (utf8EncodingWithoutMarkers == null)
			{
				lock (lockobj)
				{
					if (utf8EncodingWithoutMarkers == null)
					{
						utf8EncodingWithoutMarkers = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false);
						utf8EncodingWithoutMarkers.setReadOnly();
					}
				}
			}
			return utf8EncodingWithoutMarkers;
		}
	}

	internal static Encoding UTF8UnmarkedUnsafe
	{
		get
		{
			if (utf8EncodingUnsafe == null)
			{
				lock (lockobj)
				{
					if (utf8EncodingUnsafe == null)
					{
						utf8EncodingUnsafe = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false);
						utf8EncodingUnsafe.setReadOnly(value: false);
						utf8EncodingUnsafe.DecoderFallback = new DecoderReplacementFallback(string.Empty);
						utf8EncodingUnsafe.setReadOnly();
					}
				}
			}
			return utf8EncodingUnsafe;
		}
	}

	internal static Encoding BigEndianUTF32
	{
		get
		{
			if (bigEndianUTF32Encoding == null)
			{
				lock (lockobj)
				{
					if (bigEndianUTF32Encoding == null)
					{
						bigEndianUTF32Encoding = new UTF32Encoding(bigEndian: true, byteOrderMark: true);
						bigEndianUTF32Encoding.setReadOnly();
					}
				}
			}
			return bigEndianUTF32Encoding;
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern string InternalCodePage(ref int code_page);

	internal static Encoding GetDefaultEncoding()
	{
		Encoding encoding = null;
		int code_page = 1;
		string name = InternalCodePage(ref code_page);
		try
		{
			if (code_page == -1)
			{
				return Encoding.GetEncoding(name);
			}
			code_page &= 0xFFFFFFF;
			switch (code_page)
			{
			case 1:
				code_page = 20127;
				break;
			case 2:
				code_page = 65007;
				break;
			case 3:
				code_page = 65001;
				break;
			case 4:
				code_page = 1200;
				break;
			case 5:
				code_page = 1201;
				break;
			case 6:
				code_page = 1252;
				break;
			}
			return Encoding.GetEncoding(code_page);
		}
		catch (NotSupportedException)
		{
			return UTF8Unmarked;
		}
		catch (ArgumentException)
		{
			return UTF8Unmarked;
		}
	}

	internal static object InvokeI18N(string name, params object[] args)
	{
		lock (lockobj)
		{
			if (i18nDisabled)
			{
				return null;
			}
			if (i18nAssembly == null)
			{
				try
				{
					try
					{
						i18nAssembly = Assembly.Load("I18N, Version=2.0.5.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
					}
					catch (NotImplementedException)
					{
						i18nDisabled = true;
						return null;
					}
					if (i18nAssembly == null)
					{
						return null;
					}
				}
				catch (SystemException)
				{
					return null;
				}
			}
			Type type;
			try
			{
				type = i18nAssembly.GetType("I18N.Common.Manager");
			}
			catch (NotImplementedException)
			{
				i18nDisabled = true;
				return null;
			}
			if (type == null)
			{
				return null;
			}
			object obj;
			try
			{
				obj = type.InvokeMember("PrimaryManager", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty, null, null, null, null, null, null);
				if (obj == null)
				{
					return null;
				}
			}
			catch (MissingMethodException)
			{
				return null;
			}
			catch (SecurityException)
			{
				return null;
			}
			catch (NotImplementedException)
			{
				i18nDisabled = true;
				return null;
			}
			try
			{
				return type.InvokeMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, args, null, null, null);
			}
			catch (MissingMethodException)
			{
				return null;
			}
			catch (SecurityException)
			{
				return null;
			}
		}
	}
}
