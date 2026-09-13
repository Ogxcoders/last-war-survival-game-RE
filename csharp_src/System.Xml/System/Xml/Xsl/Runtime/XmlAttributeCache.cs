using System.Xml.Schema;

namespace System.Xml.Xsl.Runtime;

internal sealed class XmlAttributeCache : XmlRawWriter, IRemovableWriter
{
	private struct AttrNameVal
	{
		private string localName;

		private string prefix;

		private string namespaceName;

		private string text;

		private XmlAtomicValue value;

		private int hashCode;

		private int nextNameIndex;

		public string LocalName => localName;

		public string Prefix => prefix;

		public string Namespace => namespaceName;

		public string Text => text;

		public XmlAtomicValue Value => value;

		public int NextNameIndex
		{
			get
			{
				return nextNameIndex;
			}
			set
			{
				nextNameIndex = value;
			}
		}

		public void Init(string prefix, string localName, string ns, int hashCode)
		{
			this.localName = localName;
			this.prefix = prefix;
			namespaceName = ns;
			this.hashCode = hashCode;
			nextNameIndex = 0;
		}

		public void Init(string text)
		{
			this.text = text;
			value = null;
		}

		public void Init(XmlAtomicValue value)
		{
			text = null;
			this.value = value;
		}

		public bool IsDuplicate(string localName, string ns, int hashCode)
		{
			if (this.localName != null && this.hashCode == hashCode && this.localName.Equals(localName) && namespaceName.Equals(ns))
			{
				this.localName = null;
				return true;
			}
			return false;
		}
	}

	private XmlRawWriter wrapped;

	private OnRemoveWriter onRemove;

	private AttrNameVal[] arrAttrs;

	private int numEntries;

	private int idxLastName;

	private int hashCodeUnion;

	private const int DefaultCacheSize = 32;

	public int Count => numEntries;

	public OnRemoveWriter OnRemoveWriterEvent
	{
		get
		{
			return onRemove;
		}
		set
		{
			onRemove = value;
		}
	}

	public void Init(XmlRawWriter wrapped)
	{
		SetWrappedWriter(wrapped);
		numEntries = 0;
		idxLastName = 0;
		hashCodeUnion = 0;
	}

	private void SetWrappedWriter(XmlRawWriter writer)
	{
		if (writer is IRemovableWriter removableWriter)
		{
			removableWriter.OnRemoveWriterEvent = SetWrappedWriter;
		}
		wrapped = writer;
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		int num = 0;
		int num2 = 1 << (localName[0] & 0x1F);
		if ((hashCodeUnion & num2) != 0)
		{
			while (!arrAttrs[num].IsDuplicate(localName, ns, num2))
			{
				num = arrAttrs[num].NextNameIndex;
				if (num == 0)
				{
					break;
				}
			}
		}
		else
		{
			hashCodeUnion |= num2;
		}
		EnsureAttributeCache();
		if (numEntries != 0)
		{
			arrAttrs[idxLastName].NextNameIndex = numEntries;
		}
		idxLastName = numEntries++;
		arrAttrs[idxLastName].Init(prefix, localName, ns, num2);
	}

	public override void WriteEndAttribute()
	{
	}

	internal override void WriteNamespaceDeclaration(string prefix, string ns)
	{
		FlushAttributes();
		wrapped.WriteNamespaceDeclaration(prefix, ns);
	}

	public override void WriteString(string text)
	{
		EnsureAttributeCache();
		arrAttrs[numEntries++].Init(text);
	}

	public override void WriteValue(object value)
	{
		EnsureAttributeCache();
		arrAttrs[numEntries++].Init((XmlAtomicValue)value);
	}

	public override void WriteValue(string value)
	{
		WriteValue(value);
	}

	internal override void StartElementContent()
	{
		FlushAttributes();
		wrapped.StartElementContent();
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
	}

	internal override void WriteEndElement(string prefix, string localName, string ns)
	{
	}

	public override void WriteComment(string text)
	{
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
	}

	public override void WriteEntityRef(string name)
	{
	}

	public override void Close()
	{
		wrapped.Close();
	}

	public override void Flush()
	{
		wrapped.Flush();
	}

	private void FlushAttributes()
	{
		int num = 0;
		while (num != numEntries)
		{
			int nextNameIndex = arrAttrs[num].NextNameIndex;
			if (nextNameIndex == 0)
			{
				nextNameIndex = numEntries;
			}
			string localName = arrAttrs[num].LocalName;
			if (localName != null)
			{
				string prefix = arrAttrs[num].Prefix;
				string ns = arrAttrs[num].Namespace;
				wrapped.WriteStartAttribute(prefix, localName, ns);
				while (++num != nextNameIndex)
				{
					string text = arrAttrs[num].Text;
					if (text != null)
					{
						wrapped.WriteString(text);
					}
					else
					{
						wrapped.WriteValue(arrAttrs[num].Value);
					}
				}
				wrapped.WriteEndAttribute();
			}
			else
			{
				num = nextNameIndex;
			}
		}
		if (onRemove != null)
		{
			onRemove(wrapped);
		}
	}

	private void EnsureAttributeCache()
	{
		if (arrAttrs == null)
		{
			arrAttrs = new AttrNameVal[32];
		}
		else if (numEntries >= arrAttrs.Length)
		{
			AttrNameVal[] destinationArray = new AttrNameVal[numEntries * 2];
			Array.Copy(arrAttrs, destinationArray, numEntries);
			arrAttrs = destinationArray;
		}
	}
}
