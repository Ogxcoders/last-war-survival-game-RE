using System.Collections;
using System.Threading;
using System.Xml.XmlConfiguration;

namespace System.Xml.Schema;

[Obsolete("Use System.Xml.Schema.XmlSchemaSet for schema compilation and validation. https://go.microsoft.com/fwlink/?linkid=14202")]
public sealed class XmlSchemaCollection : ICollection, IEnumerable
{
	private Hashtable collection;

	private XmlNameTable nameTable;

	private SchemaNames schemaNames;

	private ReaderWriterLock wLock;

	private int timeout = -1;

	private bool isThreadSafe = true;

	private ValidationEventHandler validationEventHandler;

	private XmlResolver xmlResolver;

	public int Count => collection.Count;

	public XmlNameTable NameTable => nameTable;

	internal XmlResolver XmlResolver
	{
		set
		{
			xmlResolver = value;
		}
	}

	public XmlSchema this[string ns] => ((XmlSchemaCollectionNode)collection[(ns != null) ? ns : string.Empty])?.Schema;

	bool ICollection.IsSynchronized => true;

	object ICollection.SyncRoot => this;

	int ICollection.Count => collection.Count;

	internal ValidationEventHandler EventHandler
	{
		get
		{
			return validationEventHandler;
		}
		set
		{
			validationEventHandler = value;
		}
	}

	public event ValidationEventHandler ValidationEventHandler
	{
		add
		{
			validationEventHandler = (ValidationEventHandler)Delegate.Combine(validationEventHandler, value);
		}
		remove
		{
			validationEventHandler = (ValidationEventHandler)Delegate.Remove(validationEventHandler, value);
		}
	}

	public XmlSchemaCollection()
		: this(new NameTable())
	{
	}

	public XmlSchemaCollection(XmlNameTable nametable)
	{
		if (nametable == null)
		{
			throw new ArgumentNullException("nametable");
		}
		nameTable = nametable;
		collection = Hashtable.Synchronized(new Hashtable());
		xmlResolver = XmlReaderSection.CreateDefaultResolver();
		isThreadSafe = true;
		if (isThreadSafe)
		{
			wLock = new ReaderWriterLock();
		}
	}

	public XmlSchema Add(string ns, string uri)
	{
		if (uri == null || uri.Length == 0)
		{
			throw new ArgumentNullException("uri");
		}
		XmlTextReader xmlTextReader = new XmlTextReader(uri, nameTable);
		xmlTextReader.XmlResolver = xmlResolver;
		XmlSchema xmlSchema = null;
		try
		{
			xmlSchema = Add(ns, xmlTextReader, xmlResolver);
			while (xmlTextReader.Read())
			{
			}
			return xmlSchema;
		}
		finally
		{
			xmlTextReader.Close();
		}
	}

	public XmlSchema Add(string ns, XmlReader reader)
	{
		return Add(ns, reader, xmlResolver);
	}

	public XmlSchema Add(string ns, XmlReader reader, XmlResolver resolver)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		XmlNameTable nt = reader.NameTable;
		SchemaInfo schemaInfo = new SchemaInfo();
		Parser parser = new Parser(SchemaType.None, nt, GetSchemaNames(nt), validationEventHandler);
		parser.XmlResolver = resolver;
		SchemaType schemaType;
		try
		{
			schemaType = parser.Parse(reader, ns);
		}
		catch (XmlSchemaException e)
		{
			SendValidationEvent(e);
			return null;
		}
		if (schemaType == SchemaType.XSD)
		{
			schemaInfo.SchemaType = SchemaType.XSD;
			return Add(ns, schemaInfo, parser.XmlSchema, compile: true, resolver);
		}
		_ = parser.XdrSchema;
		return Add(ns, parser.XdrSchema, null, compile: true, resolver);
	}

	public XmlSchema Add(XmlSchema schema)
	{
		return Add(schema, xmlResolver);
	}

	public XmlSchema Add(XmlSchema schema, XmlResolver resolver)
	{
		if (schema == null)
		{
			throw new ArgumentNullException("schema");
		}
		SchemaInfo schemaInfo = new SchemaInfo();
		schemaInfo.SchemaType = SchemaType.XSD;
		return Add(schema.TargetNamespace, schemaInfo, schema, compile: true, resolver);
	}

	public void Add(XmlSchemaCollection schema)
	{
		if (schema == null)
		{
			throw new ArgumentNullException("schema");
		}
		if (this != schema)
		{
			IDictionaryEnumerator enumerator = schema.collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				XmlSchemaCollectionNode xmlSchemaCollectionNode = (XmlSchemaCollectionNode)enumerator.Value;
				Add(xmlSchemaCollectionNode.NamespaceURI, xmlSchemaCollectionNode);
			}
		}
	}

	public bool Contains(XmlSchema schema)
	{
		if (schema == null)
		{
			throw new ArgumentNullException("schema");
		}
		return this[schema.TargetNamespace] != null;
	}

	public bool Contains(string ns)
	{
		return collection[(ns != null) ? ns : string.Empty] != null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new XmlSchemaCollectionEnumerator(collection);
	}

	public XmlSchemaCollectionEnumerator GetEnumerator()
	{
		return new XmlSchemaCollectionEnumerator(collection);
	}

	void ICollection.CopyTo(Array array, int index)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		XmlSchemaCollectionEnumerator enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (index == array.Length && array.IsFixedSize)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			array.SetValue(enumerator.Current, index++);
		}
	}

	public void CopyTo(XmlSchema[] array, int index)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		XmlSchemaCollectionEnumerator enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current != null)
			{
				if (index == array.Length)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				array[index++] = enumerator.Current;
			}
		}
	}

	internal SchemaInfo GetSchemaInfo(string ns)
	{
		return ((XmlSchemaCollectionNode)collection[(ns != null) ? ns : string.Empty])?.SchemaInfo;
	}

	internal SchemaNames GetSchemaNames(XmlNameTable nt)
	{
		if (nameTable != nt)
		{
			return new SchemaNames(nt);
		}
		if (schemaNames == null)
		{
			schemaNames = new SchemaNames(nameTable);
		}
		return schemaNames;
	}

	internal XmlSchema Add(string ns, SchemaInfo schemaInfo, XmlSchema schema, bool compile)
	{
		return Add(ns, schemaInfo, schema, compile, xmlResolver);
	}

	private XmlSchema Add(string ns, SchemaInfo schemaInfo, XmlSchema schema, bool compile, XmlResolver resolver)
	{
		int num = 0;
		if (schema != null)
		{
			if (schema.ErrorCount == 0 && compile)
			{
				if (!schema.CompileSchema(this, resolver, schemaInfo, ns, validationEventHandler, nameTable, CompileContentModel: true))
				{
					num = 1;
				}
				ns = ((schema.TargetNamespace == null) ? string.Empty : schema.TargetNamespace);
			}
			num += schema.ErrorCount;
		}
		else
		{
			num += schemaInfo.ErrorCount;
			ns = NameTable.Add(ns);
		}
		if (num == 0)
		{
			XmlSchemaCollectionNode xmlSchemaCollectionNode = new XmlSchemaCollectionNode();
			xmlSchemaCollectionNode.NamespaceURI = ns;
			xmlSchemaCollectionNode.SchemaInfo = schemaInfo;
			xmlSchemaCollectionNode.Schema = schema;
			Add(ns, xmlSchemaCollectionNode);
			return schema;
		}
		return null;
	}

	private void Add(string ns, XmlSchemaCollectionNode node)
	{
		if (isThreadSafe)
		{
			wLock.AcquireWriterLock(timeout);
		}
		try
		{
			if (collection[ns] != null)
			{
				collection.Remove(ns);
			}
			collection.Add(ns, node);
		}
		finally
		{
			if (isThreadSafe)
			{
				wLock.ReleaseWriterLock();
			}
		}
	}

	private void SendValidationEvent(XmlSchemaException e)
	{
		if (validationEventHandler != null)
		{
			validationEventHandler(this, new ValidationEventArgs(e));
			return;
		}
		throw e;
	}
}
