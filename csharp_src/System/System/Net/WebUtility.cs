using System.Globalization;
using System.IO;
using System.Net.Configuration;
using System.Runtime.Versioning;
using System.Text;

namespace System.Net;

public static class WebUtility
{
	private class UrlDecoder
	{
		private int _bufferSize;

		private int _numChars;

		private char[] _charBuffer;

		private int _numBytes;

		private byte[] _byteBuffer;

		private Encoding _encoding;

		private void FlushBytes()
		{
			if (_numBytes > 0)
			{
				_numChars += _encoding.GetChars(_byteBuffer, 0, _numBytes, _charBuffer, _numChars);
				_numBytes = 0;
			}
		}

		internal UrlDecoder(int bufferSize, Encoding encoding)
		{
			_bufferSize = bufferSize;
			_encoding = encoding;
			_charBuffer = new char[bufferSize];
		}

		internal void AddChar(char ch)
		{
			if (_numBytes > 0)
			{
				FlushBytes();
			}
			_charBuffer[_numChars++] = ch;
		}

		internal void AddByte(byte b)
		{
			if (_byteBuffer == null)
			{
				_byteBuffer = new byte[_bufferSize];
			}
			_byteBuffer[_numBytes++] = b;
		}

		internal string GetString()
		{
			if (_numBytes > 0)
			{
				FlushBytes();
			}
			if (_numChars > 0)
			{
				return new string(_charBuffer, 0, _numChars);
			}
			return string.Empty;
		}
	}

	private static class HtmlEntities
	{
		private static readonly long[] entities = new long[253]
		{
			4703284585813770240L, 4711156041321349120L, 4711725575167803392L, 4712861297990238208L, 4714266503556366336L, 4715947682705702912L, 4716510624025477120L, 4716796495364358144L, 4784358139111669760L, 4855836305175347200L,
			4857247646839996416L, 4927333161101295616L, 4928464614326272000L, 4995697051497922560L, 4999386417473060864L, 4999955951319515136L, 5001091674141949952L, 5003626082636623360L, 5004731738543357952L, 5005026871516069888L,
			5143512565980069888L, 5287616793624772608L, 5288186327471226880L, 5289322050293661696L, 5291576047144271872L, 5293257247667781632L, 5431746253551566848L, 5503800488981757952L, 5581367313195597824L, 5653259346518540288L,
			5653424907233525760L, 5712090902344761344L, 5719962357852340224L, 5720531891698794496L, 5721667614521229312L, 5723342196141195264L, 5723346577300352512L, 5725038717121855488L, 5725316940556468224L, 5725602811895349248L,
			5793996369333059584L, 5794162395588853760L, 5796811588946100224L, 5797092594076876800L, 5938118154478682112L, 6008753471966019584L, 6010448897179123712L, 6073191312423649280L, 6080269614787330048L, 6082222847281856512L,
			6152307922079907840L, 6152877455926362112L, 6154013178748796928L, 6156547587243470336L, 6157948376122916864L, 6370623147892277248L, 6440538298231619584L, 6446178752274628608L, 6513740396021940224L, 7016999050535043072L,
			7017568584381497344L, 7017581787144519680L, 7018134794282205184L, 7018704307203932160L, 7020097409862167808L, 7020109512770060288L, 7020390539442782208L, 7020658820279959552L, 7020662118814842880L, 7021234358782525440L,
			7021790691919396864L, 7022089754938179584L, 7022353633239171072L, 7022639504578052096L, 7089916462575386624L, 7090201148325363712L, 7093862527975686144L, 7094695999104352256L, 7161128027798110208L, 7161679314389041152L,
			7162241186348924928L, 7162252226897903616L, 7163090656053690368L, 7163382451836813312L, 7164230172936241152L, 7165066920830435328L, 7165069197163102208L, 7165897101266649088L, 7166757527332323328L, 7166760217683558400L,
			7224181111230824448L, 7233176170314989568L, 7233188310485565440L, 7234301626138230784L, 7234307623539965952L, 7235421399056121856L, 7235444471375396864L, 7305229426686754816L, 7305798960533209088L, 7306934683355643904L,
			7308621415840743424L, 7308624695165714432L, 7308906170142425088L, 7309469091850317312L, 7309752766010753024L, 7310574747757051904L, 7310582444338446336L, 7310869880729763840L, 7310875391172804608L, 7311709939624312832L,
			7380959323184168960L, 7381244077039943680L, 7382069817868681216L, 7382069817868812288L, 7382069817902366720L, 7382069887574736896L, 7449355575193763840L, 7450361158554353664L, 7454583283205013504L, 7512411487382536192L,
			7521418686637277184L, 7522525896800141312L, 7522537965473497088L, 7593459802838466560L, 7594029336684920832L, 7594608715039244288L, 7595165059507355648L, 7596835243147919360L, 7597122224423698432L, 7597137164769427456L,
			7597419056357965824L, 7597983124939866112L, 7598532917471477760L, 7599100256881475584L, 7737589262765260800L, 7800641863534247936L, 7809643498195451904L, 7809644617497837568L, 7809647978024534016L, 7809649062788988928L,
			7810197682248482816L, 7810492402954665984L, 7810649128743993344L, 7811049829587615744L, 7813595138943614976L, 7813598018929688576L, 7814428150208659456L, 7814696918347350016L, 7814714527605325824L, 7814871253394653184L,
			7881690164152500224L, 7882532396099174400L, 7883941965828456448L, 7883943005218144256L, 7883954073408372736L, 7887210322409291776L, 7953746634536386560L, 7954046816763248640L, 7954589990137102336L, 7954764316819849216L,
			7955890216726691840L, 7957706609935777792L, 7957707062752837632L, 7958834030261043200L, 7959102355732234240L, 7959267916447219712L, 8025805367066034176L, 8026374900912488448L, 8026941110813196288L, 8027510623734923264L,
			8028908158556569600L, 8029185205354889216L, 8029189586514046464L, 8030037387297947648L, 8030481085555015680L, 8030591474804457472L, 8030591504869228544L, 8030881726335549440L, 8031159949770162176L, 8031159954082824192L,
			8031445821109043200L, 8097879365926256640L, 8097879447530635264L, 8099005319141392384L, 8099005330257608704L, 8099839378546753536L, 8100005404802547712L, 8100135147174625280L, 8100978968350294016L, 8101823371647385600L,
			8102654598159794176L, 8102661154880356352L, 8102661206419963904L, 8102935603290570752L, 8175563242567892992L, 8232987427761815552L, 8241979196860006400L, 8241990181725405184L, 8241993542252101632L, 8241994627016556544L,
			8242543246476050432L, 8242837967182233600L, 8243101809455923200L, 8243107942669221888L, 8243395393815183360L, 8243961163692376064L, 8245084864575963136L, 8247042482574917632L, 8247060091832893440L, 8314332611266740224L,
			8314596481179713536L, 8314893356039667712L, 8315161636876845056L, 8316029752846581760L, 8316291906392817664L, 8316291906399502336L, 8316298033683759104L, 8318255595579965440L, 8319663638776381440L, 8319664072568078336L,
			8319675733404286976L, 8319679031939170304L, 8319679242392567808L, 8319679246687535104L, 8319679250982502400L, 8319679465730867200L, 8321082461475831808L, 8386112624001024000L, 8388065847976132608L, 8388065856495550464L,
			8388065856503118189L, 8388070229081587712L, 8388076843239997440L, 8388354959401287680L, 8388356063442763776L, 8390876139563778048L, 8449160209875599360L, 8458150931293601792L, 8458167409130340352L, 8458720465140056064L,
			8459856187962490880L, 8461538022154829824L, 8462390596382752768L, 8462390596457164288L, 8463791385336610816L, 8603398547593756672L, 8676466157105971200L, 8746381307445313536L, 8747518797516111872L, 8752021761488322560L,
			8819583405235634176L, 8824638543088320512L, 8824643396401364992L
		};

		private static readonly char[] entities_values = new char[253]
		{
			'Æ', 'Á', 'Â', 'À', 'Α', 'Å', 'Ã', 'Ä', 'Β', 'Ç',
			'Χ', '‡', 'Δ', 'Ð', 'É', 'Ê', 'È', 'Ε', 'Η', 'Ë',
			'Γ', 'Í', 'Î', 'Ì', 'Ι', 'Ï', 'Κ', 'Λ', 'Μ', 'Ñ',
			'Ν', 'Œ', 'Ó', 'Ô', 'Ò', 'Ω', 'Ο', 'Ø', 'Õ', 'Ö',
			'Φ', 'Π', '″', 'Ψ', 'Ρ', 'Š', 'Σ', 'Þ', 'Τ', 'Θ',
			'Ú', 'Û', 'Ù', 'Υ', 'Ü', 'Ξ', 'Ý', 'Ÿ', 'Ζ', 'á',
			'â', '\u00b4', 'æ', 'à', 'ℵ', 'α', '&', '∧', '∠', '\'',
			'å', '≈', 'ã', 'ä', '„', 'β', '¦', '•', '∩', 'ç',
			'\u00b8', '¢', 'χ', 'ˆ', '♣', '≅', '©', '↵', '∪', '¤',
			'⇓', '†', '↓', '°', 'δ', '♦', '÷', 'é', 'ê', 'è',
			'∅', '\u2003', '\u2002', 'ε', '≡', 'η', 'ð', 'ë', '€', '∃',
			'ƒ', '∀', '½', '¼', '¾', '⁄', 'γ', '≥', '>', '⇔',
			'↔', '♥', '…', 'í', 'î', '¡', 'ì', 'ℑ', '∞', '∫',
			'ι', '¿', '∈', 'ï', 'κ', '⇐', 'λ', '〈', '«', '←',
			'⌈', '“', '≤', '⌊', '∗', '◊', '\u200e', '‹', '‘', '<',
			'\u00af', '—', 'µ', '·', '−', 'μ', '∇', '\u00a0', '–', '≠',
			'∋', '¬', '∉', '⊄', 'ñ', 'ν', 'ó', 'ô', 'œ', 'ò',
			'‾', 'ω', 'ο', '⊕', '∨', 'ª', 'º', 'ø', 'õ', '⊗',
			'ö', '¶', '∂', '‰', '⊥', 'φ', 'π', 'ϖ', '±', '£',
			'′', '∏', '∝', 'ψ', '"', '⇒', '√', '〉', '»', '→',
			'⌉', '”', 'ℜ', '®', '⌋', 'ρ', '\u200f', '›', '’', '‚',
			'š', '⋅', '§', '\u00ad', 'σ', 'ς', '∼', '♠', '⊂', '⊆',
			'∑', '⊃', '¹', '²', '³', '⊇', 'ß', 'τ', '∴', 'θ',
			'ϑ', '\u2009', 'þ', '\u02dc', '×', '™', '⇑', 'ú', '↑', 'û',
			'ù', '\u00a8', 'ϒ', 'υ', 'ü', '℘', 'ξ', 'ý', '¥', 'ÿ',
			'ζ', '\u200d', '\u200c'
		};

		public static char Lookup(string entity)
		{
			long num = CalculateKeyValue(entity);
			if (num == 0L)
			{
				return '\0';
			}
			int num2 = Array.BinarySearch(entities, num);
			if (num2 < 0)
			{
				return '\0';
			}
			return entities_values[num2];
		}

		private static long CalculateKeyValue(string s)
		{
			if (s.Length > 8)
			{
				return 0L;
			}
			long num = 0L;
			for (int i = 0; i < s.Length; i++)
			{
				long num2 = s[i];
				if (num2 > 122 || num2 < 48)
				{
					return 0L;
				}
				num |= num2 << (7 - i) * 8;
			}
			return num;
		}
	}

	private const char HIGH_SURROGATE_START = '\ud800';

	private const char LOW_SURROGATE_START = '\udc00';

	private const char LOW_SURROGATE_END = '\udfff';

	private const int UNICODE_PLANE00_END = 65535;

	private const int UNICODE_PLANE01_START = 65536;

	private const int UNICODE_PLANE16_END = 1114111;

	private const int UnicodeReplacementChar = 65533;

	private static readonly char[] _htmlEntityEndingChars = new char[2] { ';', '&' };

	private static volatile UnicodeDecodingConformance _htmlDecodeConformance = UnicodeDecodingConformance.Auto;

	private static volatile UnicodeEncodingConformance _htmlEncodeConformance = UnicodeEncodingConformance.Auto;

	private static UnicodeDecodingConformance HtmlDecodeConformance
	{
		get
		{
			if (_htmlDecodeConformance != UnicodeDecodingConformance.Auto)
			{
				return _htmlDecodeConformance;
			}
			_htmlDecodeConformance = (BinaryCompatibility.TargetsAtLeast_Desktop_V4_5 ? UnicodeDecodingConformance.Strict : UnicodeDecodingConformance.Compat);
			return _htmlDecodeConformance;
		}
	}

	private static UnicodeEncodingConformance HtmlEncodeConformance
	{
		get
		{
			if (_htmlEncodeConformance != UnicodeEncodingConformance.Auto)
			{
				return _htmlEncodeConformance;
			}
			_htmlEncodeConformance = (BinaryCompatibility.TargetsAtLeast_Desktop_V4_5 ? UnicodeEncodingConformance.Strict : UnicodeEncodingConformance.Compat);
			return _htmlEncodeConformance;
		}
	}

	public static string HtmlEncode(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		if (IndexOfHtmlEncodingChars(value, 0) == -1)
		{
			return value;
		}
		StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		HtmlEncode(value, stringWriter);
		return stringWriter.ToString();
	}

	public unsafe static void HtmlEncode(string value, TextWriter output)
	{
		if (value == null)
		{
			return;
		}
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		int num = IndexOfHtmlEncodingChars(value, 0);
		if (num == -1)
		{
			output.Write(value);
			return;
		}
		UnicodeEncodingConformance htmlEncodeConformance = HtmlEncodeConformance;
		int charsRemaining = value.Length - num;
		fixed (char* ptr = value)
		{
			char* pch = ptr;
			while (num-- > 0)
			{
				output.Write(*(pch++));
			}
			while (charsRemaining > 0)
			{
				char c = *pch;
				if (c <= '>')
				{
					switch (c)
					{
					case '<':
						output.Write("&lt;");
						break;
					case '>':
						output.Write("&gt;");
						break;
					case '"':
						output.Write("&quot;");
						break;
					case '\'':
						output.Write("&#39;");
						break;
					case '&':
						output.Write("&amp;");
						break;
					default:
						output.Write(c);
						break;
					}
				}
				else
				{
					int num2 = -1;
					if (c >= '\u00a0' && !char.IsSurrogate(c))
					{
						num2 = c;
					}
					else if (htmlEncodeConformance == UnicodeEncodingConformance.Strict && char.IsSurrogate(c))
					{
						int nextUnicodeScalarValueFromUtf16Surrogate = GetNextUnicodeScalarValueFromUtf16Surrogate(ref pch, ref charsRemaining);
						if (nextUnicodeScalarValueFromUtf16Surrogate >= 65536)
						{
							num2 = nextUnicodeScalarValueFromUtf16Surrogate;
						}
						else
						{
							c = (char)nextUnicodeScalarValueFromUtf16Surrogate;
						}
					}
					if (num2 >= 0)
					{
						output.Write("&#");
						output.Write(num2.ToString(NumberFormatInfo.InvariantInfo));
						output.Write(';');
					}
					else
					{
						output.Write(c);
					}
				}
				charsRemaining--;
				pch++;
			}
		}
	}

	public static string HtmlDecode(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		if (!StringRequiresHtmlDecoding(value))
		{
			return value;
		}
		StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		HtmlDecode(value, stringWriter);
		return stringWriter.ToString();
	}

	public static void HtmlDecode(string value, TextWriter output)
	{
		if (value == null)
		{
			return;
		}
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		if (!StringRequiresHtmlDecoding(value))
		{
			output.Write(value);
			return;
		}
		UnicodeDecodingConformance htmlDecodeConformance = HtmlDecodeConformance;
		int length = value.Length;
		for (int i = 0; i < length; i++)
		{
			char c = value[i];
			if (c == '&')
			{
				int num = value.IndexOfAny(_htmlEntityEndingChars, i + 1);
				if (num > 0 && value[num] == ';')
				{
					string text = value.Substring(i + 1, num - i - 1);
					if (text.Length > 1 && text[0] == '#')
					{
						uint result;
						bool flag = ((text[1] != 'x' && text[1] != 'X') ? uint.TryParse(text.Substring(1), NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result) : uint.TryParse(text.Substring(2), NumberStyles.AllowHexSpecifier, NumberFormatInfo.InvariantInfo, out result));
						if (flag)
						{
							flag = htmlDecodeConformance switch
							{
								UnicodeDecodingConformance.Strict => result < 55296 || (57343 < result && result <= 1114111), 
								UnicodeDecodingConformance.Compat => 0 < result && result <= 65535, 
								UnicodeDecodingConformance.Loose => result <= 1114111, 
								_ => false, 
							};
						}
						if (flag)
						{
							if (result <= 65535)
							{
								output.Write((char)result);
							}
							else
							{
								ConvertSmpToUtf16(result, out var leadingSurrogate, out var trailingSurrogate);
								output.Write(leadingSurrogate);
								output.Write(trailingSurrogate);
							}
							i = num;
							continue;
						}
					}
					else
					{
						i = num;
						char c2 = HtmlEntities.Lookup(text);
						if (c2 == '\0')
						{
							output.Write('&');
							output.Write(text);
							output.Write(';');
							continue;
						}
						c = c2;
					}
				}
			}
			output.Write(c);
		}
	}

	private unsafe static int IndexOfHtmlEncodingChars(string s, int startPos)
	{
		UnicodeEncodingConformance htmlEncodeConformance = HtmlEncodeConformance;
		int num = s.Length - startPos;
		fixed (char* ptr = s)
		{
			char* ptr2 = ptr + startPos;
			while (num > 0)
			{
				char c = *ptr2;
				if (c <= '>')
				{
					switch (c)
					{
					case '"':
					case '&':
					case '\'':
					case '<':
					case '>':
						return s.Length - num;
					}
				}
				else
				{
					if (c >= '\u00a0')
					{
						return s.Length - num;
					}
					if (htmlEncodeConformance == UnicodeEncodingConformance.Strict && char.IsSurrogate(c))
					{
						return s.Length - num;
					}
				}
				ptr2++;
				num--;
			}
		}
		return -1;
	}

	private static byte[] UrlEncode(byte[] bytes, int offset, int count, bool alwaysCreateNewReturnValue)
	{
		byte[] array = UrlEncode(bytes, offset, count);
		if (!alwaysCreateNewReturnValue || array == null || array != bytes)
		{
			return array;
		}
		return (byte[])array.Clone();
	}

	private static byte[] UrlEncode(byte[] bytes, int offset, int count)
	{
		if (!ValidateUrlEncodingParameters(bytes, offset, count))
		{
			return null;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < count; i++)
		{
			char c = (char)bytes[offset + i];
			if (c == ' ')
			{
				num++;
			}
			else if (!IsUrlSafeChar(c))
			{
				num2++;
			}
		}
		if (num == 0 && num2 == 0)
		{
			if (offset == 0 && bytes.Length == count)
			{
				return bytes;
			}
			byte[] array = new byte[count];
			Buffer.BlockCopy(bytes, offset, array, 0, count);
			return array;
		}
		byte[] array2 = new byte[count + num2 * 2];
		int num3 = 0;
		for (int j = 0; j < count; j++)
		{
			byte b = bytes[offset + j];
			char c2 = (char)b;
			if (IsUrlSafeChar(c2))
			{
				array2[num3++] = b;
				continue;
			}
			if (c2 == ' ')
			{
				array2[num3++] = 43;
				continue;
			}
			array2[num3++] = 37;
			array2[num3++] = (byte)IntToHex((b >> 4) & 0xF);
			array2[num3++] = (byte)IntToHex(b & 0xF);
		}
		return array2;
	}

	public static string UrlEncode(string value)
	{
		if (value == null)
		{
			return null;
		}
		byte[] bytes = Encoding.UTF8.GetBytes(value);
		return Encoding.UTF8.GetString(UrlEncode(bytes, 0, bytes.Length, alwaysCreateNewReturnValue: false));
	}

	public static byte[] UrlEncodeToBytes(byte[] value, int offset, int count)
	{
		return UrlEncode(value, offset, count, alwaysCreateNewReturnValue: true);
	}

	private static string UrlDecodeInternal(string value, Encoding encoding)
	{
		if (value == null)
		{
			return null;
		}
		int length = value.Length;
		UrlDecoder urlDecoder = new UrlDecoder(length, encoding);
		for (int i = 0; i < length; i++)
		{
			char c = value[i];
			switch (c)
			{
			case '+':
				c = ' ';
				break;
			case '%':
				if (i < length - 2)
				{
					int num = HexToInt(value[i + 1]);
					int num2 = HexToInt(value[i + 2]);
					if (num >= 0 && num2 >= 0)
					{
						byte b = (byte)((num << 4) | num2);
						i += 2;
						urlDecoder.AddByte(b);
						continue;
					}
				}
				break;
			}
			if ((c & 0xFF80) == 0)
			{
				urlDecoder.AddByte((byte)c);
			}
			else
			{
				urlDecoder.AddChar(c);
			}
		}
		return urlDecoder.GetString();
	}

	private static byte[] UrlDecodeInternal(byte[] bytes, int offset, int count)
	{
		if (!ValidateUrlEncodingParameters(bytes, offset, count))
		{
			return null;
		}
		int num = 0;
		byte[] array = new byte[count];
		for (int i = 0; i < count; i++)
		{
			int num2 = offset + i;
			byte b = bytes[num2];
			switch (b)
			{
			case 43:
				b = 32;
				break;
			case 37:
				if (i < count - 2)
				{
					int num3 = HexToInt((char)bytes[num2 + 1]);
					int num4 = HexToInt((char)bytes[num2 + 2]);
					if (num3 >= 0 && num4 >= 0)
					{
						b = (byte)((num3 << 4) | num4);
						i += 2;
					}
				}
				break;
			}
			array[num++] = b;
		}
		if (num < array.Length)
		{
			byte[] array2 = new byte[num];
			Array.Copy(array, array2, num);
			array = array2;
		}
		return array;
	}

	public static string UrlDecode(string encodedValue)
	{
		if (encodedValue == null)
		{
			return null;
		}
		return UrlDecodeInternal(encodedValue, Encoding.UTF8);
	}

	public static byte[] UrlDecodeToBytes(byte[] encodedValue, int offset, int count)
	{
		return UrlDecodeInternal(encodedValue, offset, count);
	}

	private static void ConvertSmpToUtf16(uint smpChar, out char leadingSurrogate, out char trailingSurrogate)
	{
		int num = (int)(smpChar - 65536);
		leadingSurrogate = (char)(num / 1024 + 55296);
		trailingSurrogate = (char)(num % 1024 + 56320);
	}

	private unsafe static int GetNextUnicodeScalarValueFromUtf16Surrogate(ref char* pch, ref int charsRemaining)
	{
		if (charsRemaining <= 1)
		{
			return 65533;
		}
		char c = *pch;
		char c2 = pch[1];
		if (char.IsSurrogatePair(c, c2))
		{
			pch++;
			charsRemaining--;
			return (c - 55296) * 1024 + (c2 - 56320) + 65536;
		}
		return 65533;
	}

	private static int HexToInt(char h)
	{
		if (h < '0' || h > '9')
		{
			if (h < 'a' || h > 'f')
			{
				if (h < 'A' || h > 'F')
				{
					return -1;
				}
				return h - 65 + 10;
			}
			return h - 97 + 10;
		}
		return h - 48;
	}

	private static char IntToHex(int n)
	{
		if (n <= 9)
		{
			return (char)(n + 48);
		}
		return (char)(n - 10 + 65);
	}

	private static bool IsUrlSafeChar(char ch)
	{
		if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
		{
			return true;
		}
		switch (ch)
		{
		case '!':
		case '(':
		case ')':
		case '*':
		case '-':
		case '.':
		case '_':
			return true;
		default:
			return false;
		}
	}

	private static bool ValidateUrlEncodingParameters(byte[] bytes, int offset, int count)
	{
		if (bytes == null && count == 0)
		{
			return false;
		}
		if (bytes == null)
		{
			throw new ArgumentNullException("bytes");
		}
		if (offset < 0 || offset > bytes.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || offset + count > bytes.Length)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		return true;
	}

	private static bool StringRequiresHtmlDecoding(string s)
	{
		if (HtmlDecodeConformance == UnicodeDecodingConformance.Compat)
		{
			return s.IndexOf('&') >= 0;
		}
		foreach (char c in s)
		{
			if (c == '&' || char.IsSurrogate(c))
			{
				return true;
			}
		}
		return false;
	}
}
