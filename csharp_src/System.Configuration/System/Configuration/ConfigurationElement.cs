using System.Collections;
using System.Runtime.CompilerServices;
using System.Xml;
using Unity;

namespace System.Configuration;

public abstract class ConfigurationElement
{
	public Configuration CurrentConfiguration
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ElementInformation ElementInformation
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	protected internal virtual ConfigurationElementProperty ElementProperty
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	protected ContextInformation EvaluationContext
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	protected bool HasContext
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	protected internal object this[string propertyName]
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

	public ConfigurationLockCollection LockAllAttributesExcept
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ConfigurationLockCollection LockAllElementsExcept
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ConfigurationLockCollection LockAttributes
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ConfigurationLockCollection LockElements
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public bool LockItem
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

	protected internal virtual ConfigurationPropertyCollection Properties
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	protected ConfigurationElement()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	[SpecialName]
	protected internal object get_Item(ConfigurationProperty prop)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	[SpecialName]
	protected internal void set_Item(ConfigurationProperty prop, object value)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal virtual void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected virtual string GetTransformedAssemblyString(string assemblyName)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	protected virtual string GetTransformedTypeString(string typeName)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	protected internal virtual void Init()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal virtual void InitializeDefault()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal virtual bool IsModified()
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public virtual bool IsReadOnly()
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected virtual void ListErrors(IList errorList)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected virtual bool OnDeserializeUnrecognizedAttribute(string name, string value)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected virtual bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected virtual object OnRequiredPropertyNotFound(string name)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	protected virtual void PostDeserialize()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected virtual void PreSerialize(XmlWriter writer)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal virtual void Reset(ConfigurationElement parentElement)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal virtual void ResetModified()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal virtual bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected internal virtual bool SerializeToXmlElement(XmlWriter writer, string elementName)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	protected void SetPropertyValue(ConfigurationProperty prop, object value, bool ignoreLocks)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal virtual void SetReadOnly()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected internal virtual void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
