using System.Collections;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery;

public class DiscoveryClientProtocol : HttpWebClientProtocol
{
	public sealed class DiscoveryClientResultsFile
	{
		private DiscoveryClientResultCollection results;

		public DiscoveryClientResultCollection Results => results;

		public DiscoveryClientResultsFile()
		{
			results = new DiscoveryClientResultCollection();
		}
	}

	private IList additionalInformation = new ArrayList();

	private DiscoveryClientDocumentCollection documents = new DiscoveryClientDocumentCollection();

	private DiscoveryExceptionDictionary errors = new DiscoveryExceptionDictionary();

	private DiscoveryClientReferenceCollection references = new DiscoveryClientReferenceCollection();

	public IList AdditionalInformation => additionalInformation;

	public DiscoveryClientDocumentCollection Documents => documents;

	public DiscoveryExceptionDictionary Errors => errors;

	public DiscoveryClientReferenceCollection References => references;

	public DiscoveryDocument Discover(string url)
	{
		Stream input = Download(ref url);
		XmlTextReader obj = new XmlTextReader(url, input)
		{
			XmlResolver = null
		};
		if (!DiscoveryDocument.CanRead(obj))
		{
			throw new InvalidOperationException("The url '" + url + "' does not point to a valid discovery document");
		}
		DiscoveryDocument discoveryDocument = DiscoveryDocument.Read(obj);
		obj.Close();
		documents.Add(url, discoveryDocument);
		AddDiscoReferences(discoveryDocument);
		return discoveryDocument;
	}

	public DiscoveryDocument DiscoverAny(string url)
	{
		try
		{
			string contentType = null;
			Stream stream = Download(ref url, ref contentType);
			if (contentType.IndexOf("text/html") != -1)
			{
				string input = new StreamReader(stream).ReadToEnd();
				Match match = new Regex("link\\s*rel\\s*=\\s*[\"']?alternate[\"']?\\s*" + "type\\s*=\\s*[\"']?text/xml[\"']?\\s*href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|'(?<1>[^']*)'|(?<1>\\S+))", RegexOptions.IgnoreCase).Match(input);
				if (!match.Success)
				{
					throw new InvalidOperationException("The HTML document does not contain Web service discovery information");
				}
				if (url.StartsWith("/"))
				{
					url = new Uri(url).GetLeftPart(UriPartial.Authority) + match.Groups[1];
				}
				else
				{
					if (url.LastIndexOf('/') == -1)
					{
						throw new InvalidOperationException("The HTML document does not contain Web service discovery information");
					}
					url = new Uri(new Uri(url), match.Groups[1].ToString()).ToString();
				}
				stream = Download(ref url);
			}
			XmlTextReader xmlTextReader = new XmlTextReader(url, stream);
			xmlTextReader.XmlResolver = null;
			xmlTextReader.MoveToContent();
			DiscoveryReference discoveryReference = null;
			DiscoveryDocument discoveryDocument;
			if (DiscoveryDocument.CanRead(xmlTextReader))
			{
				discoveryDocument = DiscoveryDocument.Read(xmlTextReader);
				documents.Add(url, discoveryDocument);
				discoveryReference = new DiscoveryDocumentReference();
				AddDiscoReferences(discoveryDocument);
			}
			else
			{
				XmlSchema xmlSchema = XmlSchema.Read(xmlTextReader, null);
				documents.Add(url, xmlSchema);
				discoveryDocument = new DiscoveryDocument();
				discoveryReference = new SchemaReference();
				discoveryReference.Url = url;
				((SchemaReference)discoveryReference).ResolveInternal(this, xmlSchema);
				discoveryDocument.References.Add(discoveryReference);
			}
			discoveryReference.ClientProtocol = this;
			discoveryReference.Url = url;
			references.Add(url, discoveryReference);
			xmlTextReader.Close();
			return discoveryDocument;
		}
		catch (DiscoveryException ex)
		{
			throw ex.Exception;
		}
	}

	private void AddDiscoReferences(DiscoveryDocument doc)
	{
		foreach (DiscoveryReference reference in doc.References)
		{
			reference.ClientProtocol = this;
			references.Add(reference.Url, reference);
		}
		if (doc.AdditionalInfo == null)
		{
			return;
		}
		foreach (object item in doc.AdditionalInfo)
		{
			additionalInformation.Add(item);
		}
	}

	public Stream Download(ref string url)
	{
		string contentType = null;
		return Download(ref url, ref contentType);
	}

	public Stream Download(ref string url, ref string contentType)
	{
		if (url.StartsWith("http://") || url.StartsWith("https://"))
		{
			WebResponse response = GetWebRequest(new Uri(url)).GetResponse();
			contentType = response.ContentType;
			return response.GetResponseStream();
		}
		if (url.StartsWith("file://"))
		{
			WebResponse response2 = WebRequest.Create(new Uri(url)).GetResponse();
			contentType = response2.ContentType;
			return response2.GetResponseStream();
		}
		string text = Path.GetExtension(url).ToLower();
		if (text == ".wsdl" || text == ".xsd")
		{
			contentType = "text/xml";
			return new FileStream(url, FileMode.Open, FileAccess.Read);
		}
		throw new InvalidOperationException("Unrecognized file type '" + url + "'. Extension must be one of .wsdl or .xsd");
	}

	[ComVisible(false)]
	[Obsolete("This method will be removed from a future version. The method call is no longer required for resource discovery", false)]
	public void LoadExternals()
	{
	}

	public DiscoveryClientResultCollection ReadAll(string topLevelFilename)
	{
		StreamReader streamReader = new StreamReader(topLevelFilename);
		DiscoveryClientResultsFile discoveryClientResultsFile = (DiscoveryClientResultsFile)new XmlSerializer(typeof(DiscoveryClientResultsFile)).Deserialize(streamReader);
		streamReader.Close();
		string directoryName = Path.GetDirectoryName(topLevelFilename);
		foreach (DiscoveryClientResult result in discoveryClientResultsFile.Results)
		{
			DiscoveryReference discoveryReference = (DiscoveryReference)Activator.CreateInstance(Type.GetType(result.ReferenceTypeName));
			discoveryReference.Url = result.Url;
			FileStream fileStream = new FileStream(Path.Combine(directoryName, result.Filename), FileMode.Open, FileAccess.Read);
			Documents.Add(discoveryReference.Url, discoveryReference.ReadDocument(fileStream));
			fileStream.Close();
			References.Add(discoveryReference.Url, discoveryReference);
		}
		return discoveryClientResultsFile.Results;
	}

	public void ResolveAll()
	{
		foreach (DiscoveryReference item in new ArrayList(References.Values))
		{
			try
			{
				if (item is DiscoveryDocumentReference)
				{
					((DiscoveryDocumentReference)item).ResolveAll();
				}
				else
				{
					item.Resolve();
				}
			}
			catch (DiscoveryException ex)
			{
				Errors[ex.Url] = ex.Exception;
			}
			catch (Exception value)
			{
				Errors[item.Url] = value;
			}
		}
	}

	public void ResolveOneLevel()
	{
		foreach (DiscoveryReference item in new ArrayList(References.Values))
		{
			item.Resolve();
		}
	}

	public DiscoveryClientResultCollection WriteAll(string directory, string topLevelFilename)
	{
		DiscoveryClientResultsFile discoveryClientResultsFile = new DiscoveryClientResultsFile();
		foreach (DiscoveryReference value in References.Values)
		{
			object obj = Documents[value.Url];
			if (obj != null)
			{
				string text = FindValidName(discoveryClientResultsFile, value.DefaultFilename);
				discoveryClientResultsFile.Results.Add(new DiscoveryClientResult(value.GetType(), value.Url, text));
				FileStream fileStream = new FileStream(Path.Combine(directory, text), FileMode.Create, FileAccess.Write);
				value.WriteDocument(obj, fileStream);
				fileStream.Close();
			}
		}
		StreamWriter streamWriter = new StreamWriter(Path.Combine(directory, topLevelFilename));
		new XmlSerializer(typeof(DiscoveryClientResultsFile)).Serialize(streamWriter, discoveryClientResultsFile);
		streamWriter.Close();
		return discoveryClientResultsFile.Results;
	}

	private string FindValidName(DiscoveryClientResultsFile resfile, string baseName)
	{
		string text = baseName;
		int num = 0;
		bool flag;
		do
		{
			flag = false;
			foreach (DiscoveryClientResult result in resfile.Results)
			{
				if (text == result.Filename)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(baseName);
				int num2 = ++num;
				text = fileNameWithoutExtension + num2 + Path.GetExtension(baseName);
			}
		}
		while (flag);
		return text;
	}
}
