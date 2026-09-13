using System.Collections;
using System.Diagnostics;
using System.Xml;
using Unity;

namespace System.Configuration;

[DebuggerDisplay("Count = {Count}")]
public abstract class ConfigurationElementCollection : ConfigurationElement, ICollection, IEnumerable
{
	protected internal string AddElementName
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	protected internal string ClearElementName
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public virtual ConfigurationElementCollectionType CollectionType
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(ConfigurationElementCollectionType);
		}
	}

	public int Count
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(int);
		}
	}

	protected virtual string ElementName
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public bool EmitClear
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public bool IsSynchronized
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	protected internal string RemoveElementName
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public object SyncRoot
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	protected virtual bool ThrowOnDuplicate
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	protected ConfigurationElementCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected ConfigurationElementCollection(IComparer comparer)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected virtual void BaseAdd(ConfigurationElement element)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal void BaseAdd(ConfigurationElement element, bool throwIfExists)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected virtual void BaseAdd(int index, ConfigurationElement element)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal void BaseClear()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal ConfigurationElement BaseGet(int index)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	protected internal ConfigurationElement BaseGet(object key)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	protected internal object[] BaseGetAllKeys()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	protected internal object BaseGetKey(int index)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	protected int BaseIndexOf(ConfigurationElement element)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(int);
	}

	protected internal bool BaseIsRemoved(object key)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected internal void BaseRemove(object key)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal void BaseRemoveAt(int index)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void CopyTo(ConfigurationElement[] array, int index)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected abstract ConfigurationElement CreateNewElement();

	protected virtual ConfigurationElement CreateNewElement(string elementName)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	protected abstract object GetElementKey(ConfigurationElement element);

	public IEnumerator GetEnumerator()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	protected virtual bool IsElementName(string elementName)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected virtual bool IsElementRemovable(ConfigurationElement element)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected internal override bool IsModified()
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public override bool IsReadOnly()
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected internal override void Reset(ConfigurationElement parentElement)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal override void ResetModified()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected internal override void SetReadOnly()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	void ICollection.CopyTo(Array arr, int index)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
