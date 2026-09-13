using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Resources;

[Serializable]
[ComVisible(true)]
public class ResourceSet : IDisposable, IEnumerable
{
	[NonSerialized]
	protected IResourceReader Reader;

	protected Hashtable Table;

	private Hashtable _caseInsensitiveTable;

	protected ResourceSet()
	{
		CommonInit();
	}

	internal ResourceSet(bool junk)
	{
	}

	public ResourceSet(string fileName)
	{
		Reader = new ResourceReader(fileName);
		CommonInit();
		ReadResources();
	}

	[SecurityCritical]
	public ResourceSet(Stream stream)
	{
		Reader = new ResourceReader(stream);
		CommonInit();
		ReadResources();
	}

	public ResourceSet(IResourceReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		Reader = reader;
		CommonInit();
		ReadResources();
	}

	private void CommonInit()
	{
		Table = new Hashtable();
	}

	public virtual void Close()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			IResourceReader reader = Reader;
			Reader = null;
			reader?.Close();
		}
		Reader = null;
		_caseInsensitiveTable = null;
		Table = null;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public virtual Type GetDefaultReader()
	{
		return typeof(ResourceReader);
	}

	public virtual Type GetDefaultWriter()
	{
		return typeof(ResourceWriter);
	}

	[ComVisible(false)]
	public virtual IDictionaryEnumerator GetEnumerator()
	{
		return GetEnumeratorHelper();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumeratorHelper();
	}

	private IDictionaryEnumerator GetEnumeratorHelper()
	{
		return (Table ?? throw new ObjectDisposedException(null, Environment.GetResourceString("Cannot access a closed resource set."))).GetEnumerator();
	}

	public virtual string GetString(string name)
	{
		object objectInternal = GetObjectInternal(name);
		try
		{
			return (string)objectInternal;
		}
		catch (InvalidCastException)
		{
			throw new InvalidOperationException(Environment.GetResourceString("Resource '{0}' was not a String - call GetObject instead.", name));
		}
	}

	public virtual string GetString(string name, bool ignoreCase)
	{
		object objectInternal = GetObjectInternal(name);
		string text;
		try
		{
			text = (string)objectInternal;
		}
		catch (InvalidCastException)
		{
			throw new InvalidOperationException(Environment.GetResourceString("Resource '{0}' was not a String - call GetObject instead.", name));
		}
		if (text != null || !ignoreCase)
		{
			return text;
		}
		objectInternal = GetCaseInsensitiveObjectInternal(name);
		try
		{
			return (string)objectInternal;
		}
		catch (InvalidCastException)
		{
			throw new InvalidOperationException(Environment.GetResourceString("Resource '{0}' was not a String - call GetObject instead.", name));
		}
	}

	public virtual object GetObject(string name)
	{
		return GetObjectInternal(name);
	}

	public virtual object GetObject(string name, bool ignoreCase)
	{
		object objectInternal = GetObjectInternal(name);
		if (objectInternal != null || !ignoreCase)
		{
			return objectInternal;
		}
		return GetCaseInsensitiveObjectInternal(name);
	}

	protected virtual void ReadResources()
	{
		IDictionaryEnumerator enumerator = Reader.GetEnumerator();
		while (enumerator.MoveNext())
		{
			object value = enumerator.Value;
			Table.Add(enumerator.Key, value);
		}
	}

	private object GetObjectInternal(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return (Table ?? throw new ObjectDisposedException(null, Environment.GetResourceString("Cannot access a closed resource set.")))[name];
	}

	private object GetCaseInsensitiveObjectInternal(string name)
	{
		Hashtable table = Table;
		if (table == null)
		{
			throw new ObjectDisposedException(null, Environment.GetResourceString("Cannot access a closed resource set."));
		}
		Hashtable hashtable = _caseInsensitiveTable;
		if (hashtable == null)
		{
			hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			IDictionaryEnumerator enumerator = table.GetEnumerator();
			while (enumerator.MoveNext())
			{
				hashtable.Add(enumerator.Key, enumerator.Value);
			}
			_caseInsensitiveTable = hashtable;
		}
		return hashtable[name];
	}
}
