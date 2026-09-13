using System.Collections.Specialized;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail;

public class MailMessage : IDisposable
{
	private AlternateViewCollection alternateViews;

	private AttachmentCollection attachments;

	private MailAddressCollection bcc;

	private MailAddressCollection replyTo;

	private string body;

	private MailPriority priority;

	private MailAddress sender;

	private DeliveryNotificationOptions deliveryNotificationOptions;

	private MailAddressCollection cc;

	private MailAddress from;

	private NameValueCollection headers;

	private MailAddressCollection to;

	private string subject;

	private Encoding subjectEncoding;

	private Encoding bodyEncoding;

	private Encoding headersEncoding = Encoding.UTF8;

	private bool isHtml;

	private static char[] hex = new char[16]
	{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'A', 'B', 'C', 'D', 'E', 'F'
	};

	private static Encoding utf8unmarked;

	public AlternateViewCollection AlternateViews => alternateViews;

	public AttachmentCollection Attachments => attachments;

	public MailAddressCollection Bcc => bcc;

	public string Body
	{
		get
		{
			return body;
		}
		set
		{
			if (value != null && bodyEncoding == null)
			{
				bodyEncoding = GuessEncoding(value) ?? Encoding.ASCII;
			}
			body = value;
		}
	}

	internal ContentType BodyContentType => new ContentType(isHtml ? "text/html" : "text/plain")
	{
		CharSet = (BodyEncoding ?? Encoding.ASCII).HeaderName
	};

	internal TransferEncoding ContentTransferEncoding => GuessTransferEncoding(BodyEncoding);

	public Encoding BodyEncoding
	{
		get
		{
			return bodyEncoding;
		}
		set
		{
			bodyEncoding = value;
		}
	}

	public TransferEncoding BodyTransferEncoding
	{
		get
		{
			return GuessTransferEncoding(BodyEncoding);
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public MailAddressCollection CC => cc;

	public DeliveryNotificationOptions DeliveryNotificationOptions
	{
		get
		{
			return deliveryNotificationOptions;
		}
		set
		{
			deliveryNotificationOptions = value;
		}
	}

	public MailAddress From
	{
		get
		{
			return from;
		}
		set
		{
			from = value;
		}
	}

	public NameValueCollection Headers => headers;

	public bool IsBodyHtml
	{
		get
		{
			return isHtml;
		}
		set
		{
			isHtml = value;
		}
	}

	public MailPriority Priority
	{
		get
		{
			return priority;
		}
		set
		{
			priority = value;
		}
	}

	public Encoding HeadersEncoding
	{
		get
		{
			return headersEncoding;
		}
		set
		{
			headersEncoding = value;
		}
	}

	public MailAddressCollection ReplyToList => replyTo;

	[Obsolete("Use ReplyToList instead")]
	public MailAddress ReplyTo
	{
		get
		{
			if (replyTo.Count == 0)
			{
				return null;
			}
			return replyTo[0];
		}
		set
		{
			replyTo.Clear();
			replyTo.Add(value);
		}
	}

	public MailAddress Sender
	{
		get
		{
			return sender;
		}
		set
		{
			sender = value;
		}
	}

	public string Subject
	{
		get
		{
			return subject;
		}
		set
		{
			if (value != null && subjectEncoding == null)
			{
				subjectEncoding = GuessEncoding(value);
			}
			subject = value;
		}
	}

	public Encoding SubjectEncoding
	{
		get
		{
			return subjectEncoding;
		}
		set
		{
			subjectEncoding = value;
		}
	}

	public MailAddressCollection To => to;

	private static Encoding UTF8Unmarked
	{
		get
		{
			if (utf8unmarked == null)
			{
				utf8unmarked = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
			}
			return utf8unmarked;
		}
	}

	public MailMessage()
	{
		to = new MailAddressCollection();
		alternateViews = new AlternateViewCollection();
		attachments = new AttachmentCollection();
		bcc = new MailAddressCollection();
		cc = new MailAddressCollection();
		replyTo = new MailAddressCollection();
		headers = new NameValueCollection();
		headers.Add("MIME-Version", "1.0");
	}

	public MailMessage(MailAddress from, MailAddress to)
		: this()
	{
		if (from == null || to == null)
		{
			throw new ArgumentNullException();
		}
		From = from;
		this.to.Add(to);
	}

	public MailMessage(string from, string to)
		: this()
	{
		if (from == null || from == string.Empty)
		{
			throw new ArgumentNullException("from");
		}
		if (to == null || to == string.Empty)
		{
			throw new ArgumentNullException("to");
		}
		this.from = new MailAddress(from);
		string[] array = to.Split(new char[1] { ',' });
		foreach (string text in array)
		{
			this.to.Add(new MailAddress(text.Trim()));
		}
	}

	public MailMessage(string from, string to, string subject, string body)
		: this()
	{
		if (from == null || from == string.Empty)
		{
			throw new ArgumentNullException("from");
		}
		if (to == null || to == string.Empty)
		{
			throw new ArgumentNullException("to");
		}
		this.from = new MailAddress(from);
		string[] array = to.Split(new char[1] { ',' });
		foreach (string text in array)
		{
			this.to.Add(new MailAddress(text.Trim()));
		}
		Body = body;
		Subject = subject;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	private Encoding GuessEncoding(string s)
	{
		for (int i = 0; i < s.Length; i++)
		{
			if (s[i] >= '\u0080')
			{
				return UTF8Unmarked;
			}
		}
		return null;
	}

	internal static TransferEncoding GuessTransferEncoding(Encoding enc)
	{
		if (Encoding.ASCII.Equals(enc))
		{
			return TransferEncoding.SevenBit;
		}
		if (Encoding.UTF8.CodePage == enc.CodePage || Encoding.Unicode.CodePage == enc.CodePage)
		{
			return TransferEncoding.Base64;
		}
		return TransferEncoding.QuotedPrintable;
	}

	internal static string To2047(byte[] bytes)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte b in bytes)
		{
			if (b < 33 || b > 126 || b == 63 || b == 61 || b == 95)
			{
				stringBuilder.Append('=');
				stringBuilder.Append(hex[(b >> 4) & 0xF]);
				stringBuilder.Append(hex[b & 0xF]);
			}
			else
			{
				stringBuilder.Append((char)b);
			}
		}
		return stringBuilder.ToString();
	}

	internal static string EncodeSubjectRFC2047(string s, Encoding enc)
	{
		if (s == null || Encoding.ASCII.Equals(enc))
		{
			return s;
		}
		for (int i = 0; i < s.Length; i++)
		{
			if (s[i] >= '\u0080')
			{
				string text = To2047(enc.GetBytes(s));
				return "=?" + enc.HeaderName + "?Q?" + text + "?=";
			}
		}
		return s;
	}
}
