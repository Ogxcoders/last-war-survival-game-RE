using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery;

public abstract class DiscoveryReference
{
	private string defaultFilename;

	private DiscoveryClientProtocol clientProtocol;

	[XmlIgnore]
	public DiscoveryClientProtocol ClientProtocol
	{
		get
		{
			return clientProtocol;
		}
		set
		{
			clientProtocol = value;
		}
	}

	[XmlIgnore]
	public virtual string DefaultFilename => FilenameFromUrl(Url);

	[XmlIgnore]
	public abstract string Url { get; set; }

	internal Uri BaseUri
	{
		get
		{
			int num = Url.IndexOf("://");
			if (num == -1 || !Uri.CheckSchemeName(Url.Substring(0, num)))
			{
				return new Uri("file://" + Url);
			}
			return new Uri(Url);
		}
	}

	public static string FilenameFromUrl(string url)
	{
		if (url.ToLower().EndsWith("/wsdl"))
		{
			url = url.Substring(0, url.Length - 5);
		}
		else if (url.ToLower().EndsWith("/soap"))
		{
			url = url.Substring(0, url.Length - 5);
		}
		else if (url.ToLower().EndsWith("/wsdl.jsp"))
		{
			url = url.Substring(0, url.Length - 9);
		}
		else if (url.ToLower().EndsWith("/soap.wsdl"))
		{
			url = url.Substring(0, url.Length - 10);
		}
		int num = url.LastIndexOf("/");
		if (num != -1)
		{
			url = url.Substring(num + 1);
		}
		num = url.IndexOfAny(new char[3] { '.', '?', '\\' });
		if (num != -1)
		{
			url = url.Substring(0, num);
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < url.Length; i++)
		{
			if (char.IsLetterOrDigit(url[i]) || url[i] == '_')
			{
				stringBuilder.Append(url[i]);
			}
		}
		return stringBuilder.ToString();
	}

	public abstract object ReadDocument(Stream stream);

	public void Resolve()
	{
		if (clientProtocol == null)
		{
			throw new InvalidOperationException("The ClientProtocol property is a null reference.");
		}
		if (clientProtocol.Documents.Contains(Url))
		{
			return;
		}
		try
		{
			string contentType = null;
			string url = Url;
			Stream stream = clientProtocol.Download(ref url, ref contentType);
			Resolve(contentType, stream);
		}
		catch (Exception ex)
		{
			ReportError(Url, ex);
		}
	}

	protected internal abstract void Resolve(string contentType, Stream stream);

	public abstract void WriteDocument(object document, Stream stream);

	internal void ReportError(string url, Exception ex)
	{
		if (ex is DiscoveryException)
		{
			throw ex;
		}
		throw new DiscoveryException(url, ex);
	}
}
