using System.IO;
using System.Threading.Tasks;

namespace System.Xml;

internal class XmlSystemPathResolver : XmlResolver
{
	public override object GetEntity(Uri uri, string role, Type typeOfObjectToReturn)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		if (typeOfObjectToReturn != null && typeOfObjectToReturn != typeof(Stream) && typeOfObjectToReturn != typeof(object))
		{
			throw new XmlException("Object type is not supported.", string.Empty);
		}
		string path = uri.OriginalString;
		if (uri.IsAbsoluteUri)
		{
			if (!uri.IsFile)
			{
				throw new XmlException("Cannot open '{0}'. The Uri parameter must be a file system relative or absolute path.", uri.ToString());
			}
			path = uri.LocalPath;
		}
		try
		{
			return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		catch (ArgumentException innerException)
		{
			throw new XmlException("Cannot open '{0}'. The Uri parameter must be a file system relative or absolute path.", uri.ToString(), innerException, null);
		}
	}

	public override Task<object> GetEntityAsync(Uri absoluteUri, string role, Type ofObjectToReturn)
	{
		return Task.FromResult(GetEntity(absoluteUri, role, ofObjectToReturn));
	}
}
