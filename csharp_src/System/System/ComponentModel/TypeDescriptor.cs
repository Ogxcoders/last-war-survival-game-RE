using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;

namespace System.ComponentModel;

[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
public sealed class TypeDescriptor
{
	private sealed class AttributeProvider : TypeDescriptionProvider
	{
		private class AttributeTypeDescriptor : CustomTypeDescriptor
		{
			private Attribute[] _attributeArray;

			internal AttributeTypeDescriptor(Attribute[] attrs, ICustomTypeDescriptor parent)
				: base(parent)
			{
				_attributeArray = attrs;
			}

			public override AttributeCollection GetAttributes()
			{
				Attribute[] array = null;
				AttributeCollection attributes = base.GetAttributes();
				Attribute[] attributeArray = _attributeArray;
				Attribute[] array2 = new Attribute[attributes.Count + attributeArray.Length];
				int count = attributes.Count;
				attributes.CopyTo(array2, 0);
				for (int i = 0; i < attributeArray.Length; i++)
				{
					bool flag = false;
					for (int j = 0; j < attributes.Count; j++)
					{
						if (array2[j].TypeId.Equals(attributeArray[i].TypeId))
						{
							flag = true;
							array2[j] = attributeArray[i];
							break;
						}
					}
					if (!flag)
					{
						array2[count++] = attributeArray[i];
					}
				}
				if (count < array2.Length)
				{
					array = new Attribute[count];
					Array.Copy(array2, 0, array, 0, count);
				}
				else
				{
					array = array2;
				}
				return new AttributeCollection(array);
			}
		}

		private Attribute[] _attrs;

		internal AttributeProvider(TypeDescriptionProvider existingProvider, params Attribute[] attrs)
			: base(existingProvider)
		{
			_attrs = attrs;
		}

		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			return new AttributeTypeDescriptor(_attrs, base.GetTypeDescriptor(objectType, instance));
		}
	}

	private sealed class ComNativeDescriptionProvider : TypeDescriptionProvider
	{
		private sealed class ComNativeTypeDescriptor : ICustomTypeDescriptor
		{
			private IComNativeDescriptorHandler _handler;

			private object _instance;

			internal ComNativeTypeDescriptor(IComNativeDescriptorHandler handler, object instance)
			{
				_handler = handler;
				_instance = instance;
			}

			AttributeCollection ICustomTypeDescriptor.GetAttributes()
			{
				return _handler.GetAttributes(_instance);
			}

			string ICustomTypeDescriptor.GetClassName()
			{
				return _handler.GetClassName(_instance);
			}

			string ICustomTypeDescriptor.GetComponentName()
			{
				return null;
			}

			TypeConverter ICustomTypeDescriptor.GetConverter()
			{
				return _handler.GetConverter(_instance);
			}

			EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
			{
				return _handler.GetDefaultEvent(_instance);
			}

			PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
			{
				return _handler.GetDefaultProperty(_instance);
			}

			object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
			{
				return _handler.GetEditor(_instance, editorBaseType);
			}

			EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
			{
				return _handler.GetEvents(_instance);
			}

			EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
			{
				return _handler.GetEvents(_instance, attributes);
			}

			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
			{
				return _handler.GetProperties(_instance, null);
			}

			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
			{
				return _handler.GetProperties(_instance, attributes);
			}

			object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
			{
				return _instance;
			}
		}

		private IComNativeDescriptorHandler _handler;

		internal IComNativeDescriptorHandler Handler
		{
			get
			{
				return _handler;
			}
			set
			{
				_handler = value;
			}
		}

		internal ComNativeDescriptionProvider(IComNativeDescriptorHandler handler)
		{
			_handler = handler;
		}

		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			if (objectType == null)
			{
				throw new ArgumentNullException("objectType");
			}
			if (instance == null)
			{
				return null;
			}
			if (!objectType.IsInstanceOfType(instance))
			{
				throw new ArgumentException("instance");
			}
			return new ComNativeTypeDescriptor(_handler, instance);
		}
	}

	private sealed class AttributeFilterCacheItem
	{
		private Attribute[] _filter;

		internal ICollection FilteredMembers;

		internal AttributeFilterCacheItem(Attribute[] filter, ICollection filteredMembers)
		{
			_filter = filter;
			FilteredMembers = filteredMembers;
		}

		internal bool IsValid(Attribute[] filter)
		{
			if (_filter.Length != filter.Length)
			{
				return false;
			}
			for (int i = 0; i < filter.Length; i++)
			{
				if (_filter[i] != filter[i])
				{
					return false;
				}
			}
			return true;
		}
	}

	private sealed class FilterCacheItem
	{
		private ITypeDescriptorFilterService _filterService;

		internal ICollection FilteredMembers;

		internal FilterCacheItem(ITypeDescriptorFilterService filterService, ICollection filteredMembers)
		{
			_filterService = filterService;
			FilteredMembers = filteredMembers;
		}

		internal bool IsValid(ITypeDescriptorFilterService filterService)
		{
			if (_filterService != filterService)
			{
				return false;
			}
			return true;
		}
	}

	private interface IUnimplemented
	{
	}

	private sealed class MemberDescriptorComparer : IComparer
	{
		public static readonly MemberDescriptorComparer Instance = new MemberDescriptorComparer();

		public int Compare(object left, object right)
		{
			return string.Compare(((MemberDescriptor)left).Name, ((MemberDescriptor)right).Name, ignoreCase: false, CultureInfo.InvariantCulture);
		}
	}

	private sealed class MergedTypeDescriptor : ICustomTypeDescriptor
	{
		private ICustomTypeDescriptor _primary;

		private ICustomTypeDescriptor _secondary;

		internal MergedTypeDescriptor(ICustomTypeDescriptor primary, ICustomTypeDescriptor secondary)
		{
			_primary = primary;
			_secondary = secondary;
		}

		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			AttributeCollection attributes = _primary.GetAttributes();
			if (attributes == null)
			{
				attributes = _secondary.GetAttributes();
			}
			return attributes;
		}

		string ICustomTypeDescriptor.GetClassName()
		{
			string className = _primary.GetClassName();
			if (className == null)
			{
				className = _secondary.GetClassName();
			}
			return className;
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
			string componentName = _primary.GetComponentName();
			if (componentName == null)
			{
				componentName = _secondary.GetComponentName();
			}
			return componentName;
		}

		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			TypeConverter converter = _primary.GetConverter();
			if (converter == null)
			{
				converter = _secondary.GetConverter();
			}
			return converter;
		}

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			EventDescriptor defaultEvent = _primary.GetDefaultEvent();
			if (defaultEvent == null)
			{
				defaultEvent = _secondary.GetDefaultEvent();
			}
			return defaultEvent;
		}

		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			PropertyDescriptor defaultProperty = _primary.GetDefaultProperty();
			if (defaultProperty == null)
			{
				defaultProperty = _secondary.GetDefaultProperty();
			}
			return defaultProperty;
		}

		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			if (editorBaseType == null)
			{
				throw new ArgumentNullException("editorBaseType");
			}
			object editor = _primary.GetEditor(editorBaseType);
			if (editor == null)
			{
				editor = _secondary.GetEditor(editorBaseType);
			}
			return editor;
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			EventDescriptorCollection events = _primary.GetEvents();
			if (events == null)
			{
				events = _secondary.GetEvents();
			}
			return events;
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			EventDescriptorCollection events = _primary.GetEvents(attributes);
			if (events == null)
			{
				events = _secondary.GetEvents(attributes);
			}
			return events;
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			PropertyDescriptorCollection properties = _primary.GetProperties();
			if (properties == null)
			{
				properties = _secondary.GetProperties();
			}
			return properties;
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = _primary.GetProperties(attributes);
			if (properties == null)
			{
				properties = _secondary.GetProperties(attributes);
			}
			return properties;
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			object propertyOwner = _primary.GetPropertyOwner(pd);
			if (propertyOwner == null)
			{
				propertyOwner = _secondary.GetPropertyOwner(pd);
			}
			return propertyOwner;
		}
	}

	private sealed class TypeDescriptionNode : TypeDescriptionProvider
	{
		private struct DefaultExtendedTypeDescriptor : ICustomTypeDescriptor
		{
			private TypeDescriptionNode _node;

			private object _instance;

			internal DefaultExtendedTypeDescriptor(TypeDescriptionNode node, object instance)
			{
				_node = node;
				_instance = instance;
			}

			AttributeCollection ICustomTypeDescriptor.GetAttributes()
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetExtendedAttributes(_instance);
				}
				ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(_instance);
				if (extendedTypeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetExtendedTypeDescriptor"));
				}
				AttributeCollection attributes = extendedTypeDescriptor.GetAttributes();
				if (attributes == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetAttributes"));
				}
				return attributes;
			}

			string ICustomTypeDescriptor.GetClassName()
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetExtendedClassName(_instance);
				}
				ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(_instance);
				if (extendedTypeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetExtendedTypeDescriptor"));
				}
				string text = extendedTypeDescriptor.GetClassName();
				if (text == null)
				{
					text = _instance.GetType().FullName;
				}
				return text;
			}

			string ICustomTypeDescriptor.GetComponentName()
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetExtendedComponentName(_instance);
				}
				ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(_instance);
				if (extendedTypeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetExtendedTypeDescriptor"));
				}
				return extendedTypeDescriptor.GetComponentName();
			}

			TypeConverter ICustomTypeDescriptor.GetConverter()
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetExtendedConverter(_instance);
				}
				ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(_instance);
				if (extendedTypeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetExtendedTypeDescriptor"));
				}
				TypeConverter converter = extendedTypeDescriptor.GetConverter();
				if (converter == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetConverter"));
				}
				return converter;
			}

			EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetExtendedDefaultEvent(_instance);
				}
				ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(_instance);
				if (extendedTypeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetExtendedTypeDescriptor"));
				}
				return extendedTypeDescriptor.GetDefaultEvent();
			}

			PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetExtendedDefaultProperty(_instance);
				}
				ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(_instance);
				if (extendedTypeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetExtendedTypeDescriptor"));
				}
				return extendedTypeDescriptor.GetDefaultProperty();
			}

			object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
			{
				if (editorBaseType == null)
				{
					throw new ArgumentNullException("editorBaseType");
				}
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetExtendedEditor(_instance, editorBaseType);
				}
				ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(_instance);
				if (extendedTypeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetExtendedTypeDescriptor"));
				}
				return extendedTypeDescriptor.GetEditor(editorBaseType);
			}

			EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetExtendedEvents(_instance);
				}
				ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(_instance);
				if (extendedTypeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetExtendedTypeDescriptor"));
				}
				EventDescriptorCollection events = extendedTypeDescriptor.GetEvents();
				if (events == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetEvents"));
				}
				return events;
			}

			EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetExtendedEvents(_instance);
				}
				ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(_instance);
				if (extendedTypeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetExtendedTypeDescriptor"));
				}
				EventDescriptorCollection events = extendedTypeDescriptor.GetEvents(attributes);
				if (events == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetEvents"));
				}
				return events;
			}

			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetExtendedProperties(_instance);
				}
				ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(_instance);
				if (extendedTypeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetExtendedTypeDescriptor"));
				}
				PropertyDescriptorCollection properties = extendedTypeDescriptor.GetProperties();
				if (properties == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetProperties"));
				}
				return properties;
			}

			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetExtendedProperties(_instance);
				}
				ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(_instance);
				if (extendedTypeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetExtendedTypeDescriptor"));
				}
				PropertyDescriptorCollection properties = extendedTypeDescriptor.GetProperties(attributes);
				if (properties == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetProperties"));
				}
				return properties;
			}

			object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetExtendedPropertyOwner(_instance, pd);
				}
				ICustomTypeDescriptor extendedTypeDescriptor = provider.GetExtendedTypeDescriptor(_instance);
				if (extendedTypeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetExtendedTypeDescriptor"));
				}
				object obj = extendedTypeDescriptor.GetPropertyOwner(pd);
				if (obj == null)
				{
					obj = _instance;
				}
				return obj;
			}
		}

		private struct DefaultTypeDescriptor : ICustomTypeDescriptor
		{
			private TypeDescriptionNode _node;

			private Type _objectType;

			private object _instance;

			internal DefaultTypeDescriptor(TypeDescriptionNode node, Type objectType, object instance)
			{
				_node = node;
				_objectType = objectType;
				_instance = instance;
			}

			AttributeCollection ICustomTypeDescriptor.GetAttributes()
			{
				TypeDescriptionProvider provider = _node.Provider;
				AttributeCollection attributes;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					attributes = reflectTypeDescriptionProvider.GetAttributes(_objectType);
				}
				else
				{
					ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(_objectType, _instance);
					if (typeDescriptor == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetTypeDescriptor"));
					}
					attributes = typeDescriptor.GetAttributes();
					if (attributes == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetAttributes"));
					}
				}
				return attributes;
			}

			string ICustomTypeDescriptor.GetClassName()
			{
				TypeDescriptionProvider provider = _node.Provider;
				string text;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					text = reflectTypeDescriptionProvider.GetClassName(_objectType);
				}
				else
				{
					ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(_objectType, _instance);
					if (typeDescriptor == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetTypeDescriptor"));
					}
					text = typeDescriptor.GetClassName();
					if (text == null)
					{
						text = _objectType.FullName;
					}
				}
				return text;
			}

			string ICustomTypeDescriptor.GetComponentName()
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetComponentName(_objectType, _instance);
				}
				ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(_objectType, _instance);
				if (typeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetTypeDescriptor"));
				}
				return typeDescriptor.GetComponentName();
			}

			TypeConverter ICustomTypeDescriptor.GetConverter()
			{
				TypeDescriptionProvider provider = _node.Provider;
				TypeConverter converter;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					converter = reflectTypeDescriptionProvider.GetConverter(_objectType, _instance);
				}
				else
				{
					ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(_objectType, _instance);
					if (typeDescriptor == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetTypeDescriptor"));
					}
					converter = typeDescriptor.GetConverter();
					if (converter == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetConverter"));
					}
				}
				return converter;
			}

			EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetDefaultEvent(_objectType, _instance);
				}
				ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(_objectType, _instance);
				if (typeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetTypeDescriptor"));
				}
				return typeDescriptor.GetDefaultEvent();
			}

			PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
			{
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetDefaultProperty(_objectType, _instance);
				}
				ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(_objectType, _instance);
				if (typeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetTypeDescriptor"));
				}
				return typeDescriptor.GetDefaultProperty();
			}

			object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
			{
				if (editorBaseType == null)
				{
					throw new ArgumentNullException("editorBaseType");
				}
				TypeDescriptionProvider provider = _node.Provider;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					return reflectTypeDescriptionProvider.GetEditor(_objectType, _instance, editorBaseType);
				}
				ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(_objectType, _instance);
				if (typeDescriptor == null)
				{
					throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetTypeDescriptor"));
				}
				return typeDescriptor.GetEditor(editorBaseType);
			}

			EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
			{
				TypeDescriptionProvider provider = _node.Provider;
				EventDescriptorCollection events;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					events = reflectTypeDescriptionProvider.GetEvents(_objectType);
				}
				else
				{
					ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(_objectType, _instance);
					if (typeDescriptor == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetTypeDescriptor"));
					}
					events = typeDescriptor.GetEvents();
					if (events == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetEvents"));
					}
				}
				return events;
			}

			EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
			{
				TypeDescriptionProvider provider = _node.Provider;
				EventDescriptorCollection events;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					events = reflectTypeDescriptionProvider.GetEvents(_objectType);
				}
				else
				{
					ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(_objectType, _instance);
					if (typeDescriptor == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetTypeDescriptor"));
					}
					events = typeDescriptor.GetEvents(attributes);
					if (events == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetEvents"));
					}
				}
				return events;
			}

			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
			{
				TypeDescriptionProvider provider = _node.Provider;
				PropertyDescriptorCollection properties;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					properties = reflectTypeDescriptionProvider.GetProperties(_objectType);
				}
				else
				{
					ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(_objectType, _instance);
					if (typeDescriptor == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetTypeDescriptor"));
					}
					properties = typeDescriptor.GetProperties();
					if (properties == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetProperties"));
					}
				}
				return properties;
			}

			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
			{
				TypeDescriptionProvider provider = _node.Provider;
				PropertyDescriptorCollection properties;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					properties = reflectTypeDescriptionProvider.GetProperties(_objectType);
				}
				else
				{
					ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(_objectType, _instance);
					if (typeDescriptor == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetTypeDescriptor"));
					}
					properties = typeDescriptor.GetProperties(attributes);
					if (properties == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetProperties"));
					}
				}
				return properties;
			}

			object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
			{
				TypeDescriptionProvider provider = _node.Provider;
				object obj;
				if (provider is ReflectTypeDescriptionProvider reflectTypeDescriptionProvider)
				{
					obj = reflectTypeDescriptionProvider.GetPropertyOwner(_objectType, _instance, pd);
				}
				else
				{
					ICustomTypeDescriptor typeDescriptor = provider.GetTypeDescriptor(_objectType, _instance);
					if (typeDescriptor == null)
					{
						throw new InvalidOperationException(global::SR.GetString("The type description provider {0} has returned null from {1} which is illegal.", _node.Provider.GetType().FullName, "GetTypeDescriptor"));
					}
					obj = typeDescriptor.GetPropertyOwner(pd);
					if (obj == null)
					{
						obj = _instance;
					}
				}
				return obj;
			}
		}

		internal TypeDescriptionNode Next;

		internal TypeDescriptionProvider Provider;

		internal TypeDescriptionNode(TypeDescriptionProvider provider)
		{
			Provider = provider;
		}

		public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
		{
			if (objectType == null)
			{
				throw new ArgumentNullException("objectType");
			}
			if (argTypes != null)
			{
				if (args == null)
				{
					throw new ArgumentNullException("args");
				}
				if (argTypes.Length != args.Length)
				{
					throw new ArgumentException(global::SR.GetString("The number of elements in the Type and Object arrays must match."));
				}
			}
			return Provider.CreateInstance(provider, objectType, argTypes, args);
		}

		public override IDictionary GetCache(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return Provider.GetCache(instance);
		}

		public override ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return new DefaultExtendedTypeDescriptor(this, instance);
		}

		protected internal override IExtenderProvider[] GetExtenderProviders(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return Provider.GetExtenderProviders(instance);
		}

		public override string GetFullComponentName(object component)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			return Provider.GetFullComponentName(component);
		}

		public override Type GetReflectionType(Type objectType, object instance)
		{
			if (objectType == null)
			{
				throw new ArgumentNullException("objectType");
			}
			return Provider.GetReflectionType(objectType, instance);
		}

		public override Type GetRuntimeType(Type objectType)
		{
			if (objectType == null)
			{
				throw new ArgumentNullException("objectType");
			}
			return Provider.GetRuntimeType(objectType);
		}

		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			if (objectType == null)
			{
				throw new ArgumentNullException("objectType");
			}
			if (instance != null && !objectType.IsInstanceOfType(instance))
			{
				throw new ArgumentException("instance");
			}
			return new DefaultTypeDescriptor(this, objectType, instance);
		}

		public override bool IsSupportedType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return Provider.IsSupportedType(type);
		}
	}

	[TypeDescriptionProvider("System.Windows.Forms.ComponentModel.Com2Interop.ComNativeDescriptor, System.Windows.Forms, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
	private sealed class TypeDescriptorComObject
	{
	}

	private sealed class TypeDescriptorInterface
	{
	}

	private static WeakHashtable _providerTable = new WeakHashtable();

	private static Hashtable _providerTypeTable = new Hashtable();

	private static volatile Hashtable _defaultProviders = new Hashtable();

	private static volatile WeakHashtable _associationTable;

	private static int _metadataVersion;

	private static int _collisionIndex;

	private static BooleanSwitch TraceDescriptor = new BooleanSwitch("TypeDescriptor", "Debug TypeDescriptor.");

	private const int PIPELINE_ATTRIBUTES = 0;

	private const int PIPELINE_PROPERTIES = 1;

	private const int PIPELINE_EVENTS = 2;

	private static readonly Guid[] _pipelineInitializeKeys = new Guid[3]
	{
		Guid.NewGuid(),
		Guid.NewGuid(),
		Guid.NewGuid()
	};

	private static readonly Guid[] _pipelineMergeKeys = new Guid[3]
	{
		Guid.NewGuid(),
		Guid.NewGuid(),
		Guid.NewGuid()
	};

	private static readonly Guid[] _pipelineFilterKeys = new Guid[3]
	{
		Guid.NewGuid(),
		Guid.NewGuid(),
		Guid.NewGuid()
	};

	private static readonly Guid[] _pipelineAttributeFilterKeys = new Guid[3]
	{
		Guid.NewGuid(),
		Guid.NewGuid(),
		Guid.NewGuid()
	};

	private static object _internalSyncObject = new object();

	[Obsolete("This property has been deprecated.  Use a type description provider to supply type information for COM types instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
	public static IComNativeDescriptorHandler ComNativeDescriptorHandler
	{
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		get
		{
			TypeDescriptionNode typeDescriptionNode = NodeFor(ComObjectType);
			ComNativeDescriptionProvider comNativeDescriptionProvider = null;
			do
			{
				comNativeDescriptionProvider = typeDescriptionNode.Provider as ComNativeDescriptionProvider;
				typeDescriptionNode = typeDescriptionNode.Next;
			}
			while (typeDescriptionNode != null && comNativeDescriptionProvider == null);
			return comNativeDescriptionProvider?.Handler;
		}
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		set
		{
			TypeDescriptionNode typeDescriptionNode = NodeFor(ComObjectType);
			while (typeDescriptionNode != null && !(typeDescriptionNode.Provider is ComNativeDescriptionProvider))
			{
				typeDescriptionNode = typeDescriptionNode.Next;
			}
			if (typeDescriptionNode == null)
			{
				AddProvider(new ComNativeDescriptionProvider(value), ComObjectType);
			}
			else
			{
				((ComNativeDescriptionProvider)typeDescriptionNode.Provider).Handler = value;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static Type ComObjectType => typeof(TypeDescriptorComObject);

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static Type InterfaceType => typeof(TypeDescriptorInterface);

	internal static int MetadataVersion => _metadataVersion;

	public static event RefreshEventHandler Refreshed;

	private TypeDescriptor()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static TypeDescriptionProvider AddAttributes(Type type, params Attribute[] attributes)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		if (attributes == null)
		{
			throw new ArgumentNullException("attributes");
		}
		AttributeProvider attributeProvider = new AttributeProvider(GetProvider(type), attributes);
		AddProvider(attributeProvider, type);
		return attributeProvider;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static TypeDescriptionProvider AddAttributes(object instance, params Attribute[] attributes)
	{
		if (instance == null)
		{
			throw new ArgumentNullException("instance");
		}
		if (attributes == null)
		{
			throw new ArgumentNullException("attributes");
		}
		AttributeProvider attributeProvider = new AttributeProvider(GetProvider(instance), attributes);
		AddProvider(attributeProvider, instance);
		return attributeProvider;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void AddEditorTable(Type editorBaseType, Hashtable table)
	{
		ReflectTypeDescriptionProvider.AddEditorTable(editorBaseType, table);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static void AddProvider(TypeDescriptionProvider provider, Type type)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		lock (_providerTable)
		{
			TypeDescriptionNode next = NodeFor(type, createDelegator: true);
			TypeDescriptionNode typeDescriptionNode = new TypeDescriptionNode(provider);
			typeDescriptionNode.Next = next;
			_providerTable[type] = typeDescriptionNode;
			_providerTypeTable.Clear();
		}
		Refresh(type);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static void AddProvider(TypeDescriptionProvider provider, object instance)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		if (instance == null)
		{
			throw new ArgumentNullException("instance");
		}
		bool flag;
		lock (_providerTable)
		{
			flag = _providerTable.ContainsKey(instance);
			TypeDescriptionNode next = NodeFor(instance, createDelegator: true);
			TypeDescriptionNode typeDescriptionNode = new TypeDescriptionNode(provider);
			typeDescriptionNode.Next = next;
			_providerTable.SetWeak(instance, typeDescriptionNode);
			_providerTypeTable.Clear();
		}
		if (flag)
		{
			Refresh(instance, refreshReflectionProvider: false);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void AddProviderTransparent(TypeDescriptionProvider provider, Type type)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		AddProvider(provider, type);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void AddProviderTransparent(TypeDescriptionProvider provider, object instance)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		if (instance == null)
		{
			throw new ArgumentNullException("instance");
		}
		AddProvider(provider, instance);
	}

	private static void CheckDefaultProvider(Type type)
	{
		if (_defaultProviders == null)
		{
			lock (_internalSyncObject)
			{
				if (_defaultProviders == null)
				{
					_defaultProviders = new Hashtable();
				}
			}
		}
		if (_defaultProviders.ContainsKey(type))
		{
			return;
		}
		lock (_internalSyncObject)
		{
			if (_defaultProviders.ContainsKey(type))
			{
				return;
			}
			_defaultProviders[type] = null;
		}
		object[] customAttributes = type.GetCustomAttributes(typeof(TypeDescriptionProviderAttribute), inherit: false);
		bool flag = false;
		for (int num = customAttributes.Length - 1; num >= 0; num--)
		{
			Type type2 = Type.GetType(((TypeDescriptionProviderAttribute)customAttributes[num]).TypeName);
			if (type2 != null && typeof(TypeDescriptionProvider).IsAssignableFrom(type2))
			{
				AddProvider((TypeDescriptionProvider)Activator.CreateInstance(type2), type);
				flag = true;
			}
		}
		if (!flag)
		{
			Type baseType = type.BaseType;
			if (baseType != null && baseType != type)
			{
				CheckDefaultProvider(baseType);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static void CreateAssociation(object primary, object secondary)
	{
		if (primary == null)
		{
			throw new ArgumentNullException("primary");
		}
		if (secondary == null)
		{
			throw new ArgumentNullException("secondary");
		}
		if (primary == secondary)
		{
			throw new ArgumentException(global::SR.GetString("Cannot create an association when the primary and secondary objects are the same."));
		}
		if (_associationTable == null)
		{
			lock (_internalSyncObject)
			{
				if (_associationTable == null)
				{
					_associationTable = new WeakHashtable();
				}
			}
		}
		IList list = (IList)_associationTable[primary];
		if (list == null)
		{
			lock (_associationTable)
			{
				list = (IList)_associationTable[primary];
				if (list == null)
				{
					list = new ArrayList(4);
					_associationTable.SetWeak(primary, list);
				}
			}
		}
		else
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				WeakReference weakReference = (WeakReference)list[num];
				if (weakReference.IsAlive && weakReference.Target == secondary)
				{
					throw new ArgumentException(global::SR.GetString("The primary and secondary objects are already associated with each other."));
				}
			}
		}
		lock (list)
		{
			list.Add(new WeakReference(secondary));
		}
	}

	public static IDesigner CreateDesigner(IComponent component, Type designerBaseType)
	{
		Type type = null;
		IDesigner result = null;
		AttributeCollection attributes = GetAttributes(component);
		for (int i = 0; i < attributes.Count; i++)
		{
			if (!(attributes[i] is DesignerAttribute designerAttribute))
			{
				continue;
			}
			Type type2 = Type.GetType(designerAttribute.DesignerBaseTypeName);
			if (!(type2 != null) || !(type2 == designerBaseType))
			{
				continue;
			}
			ISite site = component.Site;
			bool flag = false;
			if (site != null)
			{
				ITypeResolutionService typeResolutionService = (ITypeResolutionService)site.GetService(typeof(ITypeResolutionService));
				if (typeResolutionService != null)
				{
					flag = true;
					type = typeResolutionService.GetType(designerAttribute.DesignerTypeName);
				}
			}
			if (!flag)
			{
				type = Type.GetType(designerAttribute.DesignerTypeName);
			}
			if (type != null)
			{
				break;
			}
		}
		if (type != null)
		{
			result = (IDesigner)SecurityUtils.SecureCreateInstance(type, null, allowNonPublic: true);
		}
		return result;
	}

	[ReflectionPermission(SecurityAction.LinkDemand, Flags = ReflectionPermissionFlag.MemberAccess)]
	public static EventDescriptor CreateEvent(Type componentType, string name, Type type, params Attribute[] attributes)
	{
		return new ReflectEventDescriptor(componentType, name, type, attributes);
	}

	[ReflectionPermission(SecurityAction.LinkDemand, Flags = ReflectionPermissionFlag.MemberAccess)]
	public static EventDescriptor CreateEvent(Type componentType, EventDescriptor oldEventDescriptor, params Attribute[] attributes)
	{
		return new ReflectEventDescriptor(componentType, oldEventDescriptor, attributes);
	}

	public static object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
	{
		if (objectType == null)
		{
			throw new ArgumentNullException("objectType");
		}
		if (argTypes != null)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			if (argTypes.Length != args.Length)
			{
				throw new ArgumentException(global::SR.GetString("The number of elements in the Type and Object arrays must match."));
			}
		}
		object obj = null;
		if (provider != null && provider.GetService(typeof(TypeDescriptionProvider)) is TypeDescriptionProvider typeDescriptionProvider)
		{
			obj = typeDescriptionProvider.CreateInstance(provider, objectType, argTypes, args);
		}
		if (obj == null)
		{
			obj = NodeFor(objectType).CreateInstance(provider, objectType, argTypes, args);
		}
		return obj;
	}

	[ReflectionPermission(SecurityAction.LinkDemand, Flags = ReflectionPermissionFlag.MemberAccess)]
	public static PropertyDescriptor CreateProperty(Type componentType, string name, Type type, params Attribute[] attributes)
	{
		return new ReflectPropertyDescriptor(componentType, name, type, attributes);
	}

	[ReflectionPermission(SecurityAction.LinkDemand, Flags = ReflectionPermissionFlag.MemberAccess)]
	public static PropertyDescriptor CreateProperty(Type componentType, PropertyDescriptor oldPropertyDescriptor, params Attribute[] attributes)
	{
		if (componentType == oldPropertyDescriptor.ComponentType && ((ExtenderProvidedPropertyAttribute)oldPropertyDescriptor.Attributes[typeof(ExtenderProvidedPropertyAttribute)]).ExtenderProperty is ReflectPropertyDescriptor)
		{
			return new ExtendedPropertyDescriptor(oldPropertyDescriptor, attributes);
		}
		return new ReflectPropertyDescriptor(componentType, oldPropertyDescriptor, attributes);
	}

	[Conditional("DEBUG")]
	private static void DebugValidate(Type type, AttributeCollection attributes, AttributeCollection debugAttributes)
	{
	}

	[Conditional("DEBUG")]
	private static void DebugValidate(AttributeCollection attributes, AttributeCollection debugAttributes)
	{
	}

	[Conditional("DEBUG")]
	private static void DebugValidate(AttributeCollection attributes, Type type)
	{
	}

	[Conditional("DEBUG")]
	private static void DebugValidate(AttributeCollection attributes, object instance, bool noCustomTypeDesc)
	{
	}

	[Conditional("DEBUG")]
	private static void DebugValidate(TypeConverter converter, Type type)
	{
	}

	[Conditional("DEBUG")]
	private static void DebugValidate(TypeConverter converter, object instance, bool noCustomTypeDesc)
	{
	}

	[Conditional("DEBUG")]
	private static void DebugValidate(EventDescriptorCollection events, Type type, Attribute[] attributes)
	{
	}

	[Conditional("DEBUG")]
	private static void DebugValidate(EventDescriptorCollection events, object instance, Attribute[] attributes, bool noCustomTypeDesc)
	{
	}

	[Conditional("DEBUG")]
	private static void DebugValidate(PropertyDescriptorCollection properties, Type type, Attribute[] attributes)
	{
	}

	[Conditional("DEBUG")]
	private static void DebugValidate(PropertyDescriptorCollection properties, object instance, Attribute[] attributes, bool noCustomTypeDesc)
	{
	}

	private static ArrayList FilterMembers(IList members, Attribute[] attributes)
	{
		ArrayList arrayList = null;
		int count = members.Count;
		for (int i = 0; i < count; i++)
		{
			bool flag = false;
			for (int j = 0; j < attributes.Length; j++)
			{
				if (ShouldHideMember((MemberDescriptor)members[i], attributes[j]))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				if (arrayList == null)
				{
					arrayList = new ArrayList(count);
					for (int k = 0; k < i; k++)
					{
						arrayList.Add(members[k]);
					}
				}
			}
			else
			{
				arrayList?.Add(members[i]);
			}
		}
		return arrayList;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static object GetAssociation(Type type, object primary)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		if (primary == null)
		{
			throw new ArgumentNullException("primary");
		}
		object obj = primary;
		if (!type.IsInstanceOfType(primary))
		{
			Hashtable associationTable = _associationTable;
			if (associationTable != null)
			{
				IList list = (IList)associationTable[primary];
				if (list != null)
				{
					lock (list)
					{
						for (int num = list.Count - 1; num >= 0; num--)
						{
							object target = ((WeakReference)list[num]).Target;
							if (target == null)
							{
								list.RemoveAt(num);
							}
							else if (type.IsInstanceOfType(target))
							{
								obj = target;
							}
						}
					}
				}
			}
			if (obj == primary && primary is IComponent { Site: { DesignMode: not false } site } component && site.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
			{
				object designer = designerHost.GetDesigner(component);
				if (designer != null && type.IsInstanceOfType(designer))
				{
					obj = designer;
				}
			}
		}
		return obj;
	}

	public static AttributeCollection GetAttributes(Type componentType)
	{
		if (componentType == null)
		{
			return new AttributeCollection((Attribute[])null);
		}
		return GetDescriptor(componentType, "componentType").GetAttributes();
	}

	public static AttributeCollection GetAttributes(object component)
	{
		return GetAttributes(component, noCustomTypeDesc: false);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static AttributeCollection GetAttributes(object component, bool noCustomTypeDesc)
	{
		if (component == null)
		{
			return new AttributeCollection((Attribute[])null);
		}
		ICollection collection = GetDescriptor(component, noCustomTypeDesc).GetAttributes();
		if (component is ICustomTypeDescriptor)
		{
			if (noCustomTypeDesc)
			{
				ICustomTypeDescriptor extendedDescriptor = GetExtendedDescriptor(component);
				if (extendedDescriptor != null)
				{
					ICollection attributes = extendedDescriptor.GetAttributes();
					collection = PipelineMerge(0, collection, attributes, component, null);
				}
			}
			else
			{
				collection = PipelineFilter(0, collection, component, null);
			}
		}
		else
		{
			IDictionary cache = GetCache(component);
			collection = PipelineInitialize(0, collection, cache);
			ICustomTypeDescriptor extendedDescriptor2 = GetExtendedDescriptor(component);
			if (extendedDescriptor2 != null)
			{
				ICollection attributes2 = extendedDescriptor2.GetAttributes();
				collection = PipelineMerge(0, collection, attributes2, component, cache);
			}
			collection = PipelineFilter(0, collection, component, cache);
		}
		AttributeCollection attributeCollection = collection as AttributeCollection;
		if (attributeCollection == null)
		{
			Attribute[] array = new Attribute[collection.Count];
			collection.CopyTo(array, 0);
			attributeCollection = new AttributeCollection(array);
		}
		return attributeCollection;
	}

	internal static IDictionary GetCache(object instance)
	{
		return NodeFor(instance).GetCache(instance);
	}

	public static string GetClassName(object component)
	{
		return GetClassName(component, noCustomTypeDesc: false);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static string GetClassName(object component, bool noCustomTypeDesc)
	{
		return GetDescriptor(component, noCustomTypeDesc).GetClassName();
	}

	public static string GetClassName(Type componentType)
	{
		return GetDescriptor(componentType, "componentType").GetClassName();
	}

	public static string GetComponentName(object component)
	{
		return GetComponentName(component, noCustomTypeDesc: false);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static string GetComponentName(object component, bool noCustomTypeDesc)
	{
		return GetDescriptor(component, noCustomTypeDesc).GetComponentName();
	}

	public static TypeConverter GetConverter(object component)
	{
		return GetConverter(component, noCustomTypeDesc: false);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static TypeConverter GetConverter(object component, bool noCustomTypeDesc)
	{
		return GetDescriptor(component, noCustomTypeDesc).GetConverter();
	}

	public static TypeConverter GetConverter(Type type)
	{
		return GetDescriptor(type, "type").GetConverter();
	}

	private static object ConvertFromInvariantString(Type type, string stringValue)
	{
		return GetConverter(type).ConvertFromInvariantString(stringValue);
	}

	public static EventDescriptor GetDefaultEvent(Type componentType)
	{
		if (componentType == null)
		{
			return null;
		}
		return GetDescriptor(componentType, "componentType").GetDefaultEvent();
	}

	public static EventDescriptor GetDefaultEvent(object component)
	{
		return GetDefaultEvent(component, noCustomTypeDesc: false);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static EventDescriptor GetDefaultEvent(object component, bool noCustomTypeDesc)
	{
		if (component == null)
		{
			return null;
		}
		return GetDescriptor(component, noCustomTypeDesc).GetDefaultEvent();
	}

	public static PropertyDescriptor GetDefaultProperty(Type componentType)
	{
		if (componentType == null)
		{
			return null;
		}
		return GetDescriptor(componentType, "componentType").GetDefaultProperty();
	}

	public static PropertyDescriptor GetDefaultProperty(object component)
	{
		return GetDefaultProperty(component, noCustomTypeDesc: false);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static PropertyDescriptor GetDefaultProperty(object component, bool noCustomTypeDesc)
	{
		if (component == null)
		{
			return null;
		}
		return GetDescriptor(component, noCustomTypeDesc).GetDefaultProperty();
	}

	internal static ICustomTypeDescriptor GetDescriptor(Type type, string typeName)
	{
		if (type == null)
		{
			throw new ArgumentNullException(typeName);
		}
		return NodeFor(type).GetTypeDescriptor(type);
	}

	internal static ICustomTypeDescriptor GetDescriptor(object component, bool noCustomTypeDesc)
	{
		if (component == null)
		{
			throw new ArgumentException("component");
		}
		if (component is IUnimplemented)
		{
			throw new NotSupportedException(global::SR.GetString("The object {0} is being remoted by a proxy that does not support interface discovery.  This type of remoted object is not supported.", component.GetType().FullName));
		}
		ICustomTypeDescriptor customTypeDescriptor = NodeFor(component).GetTypeDescriptor(component);
		ICustomTypeDescriptor customTypeDescriptor2 = component as ICustomTypeDescriptor;
		if (!noCustomTypeDesc && customTypeDescriptor2 != null)
		{
			customTypeDescriptor = new MergedTypeDescriptor(customTypeDescriptor2, customTypeDescriptor);
		}
		return customTypeDescriptor;
	}

	internal static ICustomTypeDescriptor GetExtendedDescriptor(object component)
	{
		if (component == null)
		{
			throw new ArgumentException("component");
		}
		return NodeFor(component).GetExtendedTypeDescriptor(component);
	}

	public static object GetEditor(object component, Type editorBaseType)
	{
		return GetEditor(component, editorBaseType, noCustomTypeDesc: false);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static object GetEditor(object component, Type editorBaseType, bool noCustomTypeDesc)
	{
		if (editorBaseType == null)
		{
			throw new ArgumentNullException("editorBaseType");
		}
		return GetDescriptor(component, noCustomTypeDesc).GetEditor(editorBaseType);
	}

	public static object GetEditor(Type type, Type editorBaseType)
	{
		if (editorBaseType == null)
		{
			throw new ArgumentNullException("editorBaseType");
		}
		return GetDescriptor(type, "type").GetEditor(editorBaseType);
	}

	public static EventDescriptorCollection GetEvents(Type componentType)
	{
		if (componentType == null)
		{
			return new EventDescriptorCollection(null, readOnly: true);
		}
		return GetDescriptor(componentType, "componentType").GetEvents();
	}

	public static EventDescriptorCollection GetEvents(Type componentType, Attribute[] attributes)
	{
		if (componentType == null)
		{
			return new EventDescriptorCollection(null, readOnly: true);
		}
		EventDescriptorCollection eventDescriptorCollection = GetDescriptor(componentType, "componentType").GetEvents(attributes);
		if (attributes != null && attributes.Length != 0)
		{
			ArrayList arrayList = FilterMembers(eventDescriptorCollection, attributes);
			if (arrayList != null)
			{
				eventDescriptorCollection = new EventDescriptorCollection((EventDescriptor[])arrayList.ToArray(typeof(EventDescriptor)), readOnly: true);
			}
		}
		return eventDescriptorCollection;
	}

	public static EventDescriptorCollection GetEvents(object component)
	{
		return GetEvents(component, null, noCustomTypeDesc: false);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static EventDescriptorCollection GetEvents(object component, bool noCustomTypeDesc)
	{
		return GetEvents(component, null, noCustomTypeDesc);
	}

	public static EventDescriptorCollection GetEvents(object component, Attribute[] attributes)
	{
		return GetEvents(component, attributes, noCustomTypeDesc: false);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static EventDescriptorCollection GetEvents(object component, Attribute[] attributes, bool noCustomTypeDesc)
	{
		if (component == null)
		{
			return new EventDescriptorCollection(null, readOnly: true);
		}
		ICustomTypeDescriptor descriptor = GetDescriptor(component, noCustomTypeDesc);
		ICollection collection;
		if (component is ICustomTypeDescriptor)
		{
			collection = descriptor.GetEvents(attributes);
			if (noCustomTypeDesc)
			{
				ICustomTypeDescriptor extendedDescriptor = GetExtendedDescriptor(component);
				if (extendedDescriptor != null)
				{
					ICollection events = extendedDescriptor.GetEvents(attributes);
					collection = PipelineMerge(2, collection, events, component, null);
				}
			}
			else
			{
				collection = PipelineFilter(2, collection, component, null);
				collection = PipelineAttributeFilter(2, collection, attributes, component, null);
			}
		}
		else
		{
			IDictionary cache = GetCache(component);
			collection = descriptor.GetEvents(attributes);
			collection = PipelineInitialize(2, collection, cache);
			ICustomTypeDescriptor extendedDescriptor2 = GetExtendedDescriptor(component);
			if (extendedDescriptor2 != null)
			{
				ICollection events2 = extendedDescriptor2.GetEvents(attributes);
				collection = PipelineMerge(2, collection, events2, component, cache);
			}
			collection = PipelineFilter(2, collection, component, cache);
			collection = PipelineAttributeFilter(2, collection, attributes, component, cache);
		}
		EventDescriptorCollection eventDescriptorCollection = collection as EventDescriptorCollection;
		if (eventDescriptorCollection == null)
		{
			EventDescriptor[] array = new EventDescriptor[collection.Count];
			collection.CopyTo(array, 0);
			eventDescriptorCollection = new EventDescriptorCollection(array, readOnly: true);
		}
		return eventDescriptorCollection;
	}

	private static string GetExtenderCollisionSuffix(MemberDescriptor member)
	{
		string result = null;
		if (member.Attributes[typeof(ExtenderProvidedPropertyAttribute)] is ExtenderProvidedPropertyAttribute { Provider: { } provider })
		{
			string text = null;
			if (provider is IComponent { Site: not null } component)
			{
				text = component.Site.Name;
			}
			if (text == null || text.Length == 0)
			{
				text = (Interlocked.Increment(ref _collisionIndex) - 1).ToString(CultureInfo.InvariantCulture);
			}
			result = string.Format(CultureInfo.InvariantCulture, "_{0}", text);
		}
		return result;
	}

	public static string GetFullComponentName(object component)
	{
		if (component == null)
		{
			throw new ArgumentNullException("component");
		}
		return GetProvider(component).GetFullComponentName(component);
	}

	private static Type GetNodeForBaseType(Type searchType)
	{
		if (searchType.IsInterface)
		{
			return InterfaceType;
		}
		if (searchType == InterfaceType)
		{
			return null;
		}
		return searchType.BaseType;
	}

	public static PropertyDescriptorCollection GetProperties(Type componentType)
	{
		if (componentType == null)
		{
			return new PropertyDescriptorCollection(null, readOnly: true);
		}
		return GetDescriptor(componentType, "componentType").GetProperties();
	}

	public static PropertyDescriptorCollection GetProperties(Type componentType, Attribute[] attributes)
	{
		if (componentType == null)
		{
			return new PropertyDescriptorCollection(null, readOnly: true);
		}
		PropertyDescriptorCollection propertyDescriptorCollection = GetDescriptor(componentType, "componentType").GetProperties(attributes);
		if (attributes != null && attributes.Length != 0)
		{
			ArrayList arrayList = FilterMembers(propertyDescriptorCollection, attributes);
			if (arrayList != null)
			{
				propertyDescriptorCollection = new PropertyDescriptorCollection((PropertyDescriptor[])arrayList.ToArray(typeof(PropertyDescriptor)), readOnly: true);
			}
		}
		return propertyDescriptorCollection;
	}

	public static PropertyDescriptorCollection GetProperties(object component)
	{
		return GetProperties(component, noCustomTypeDesc: false);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static PropertyDescriptorCollection GetProperties(object component, bool noCustomTypeDesc)
	{
		return GetPropertiesImpl(component, null, noCustomTypeDesc, noAttributes: true);
	}

	public static PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
	{
		return GetProperties(component, attributes, noCustomTypeDesc: false);
	}

	public static PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes, bool noCustomTypeDesc)
	{
		return GetPropertiesImpl(component, attributes, noCustomTypeDesc, noAttributes: false);
	}

	private static PropertyDescriptorCollection GetPropertiesImpl(object component, Attribute[] attributes, bool noCustomTypeDesc, bool noAttributes)
	{
		if (component == null)
		{
			return new PropertyDescriptorCollection(null, readOnly: true);
		}
		ICustomTypeDescriptor descriptor = GetDescriptor(component, noCustomTypeDesc);
		ICollection collection;
		if (component is ICustomTypeDescriptor)
		{
			collection = (noAttributes ? descriptor.GetProperties() : descriptor.GetProperties(attributes));
			if (noCustomTypeDesc)
			{
				ICustomTypeDescriptor extendedDescriptor = GetExtendedDescriptor(component);
				if (extendedDescriptor != null)
				{
					ICollection secondary = (noAttributes ? extendedDescriptor.GetProperties() : extendedDescriptor.GetProperties(attributes));
					collection = PipelineMerge(1, collection, secondary, component, null);
				}
			}
			else
			{
				collection = PipelineFilter(1, collection, component, null);
				collection = PipelineAttributeFilter(1, collection, attributes, component, null);
			}
		}
		else
		{
			IDictionary cache = GetCache(component);
			collection = (noAttributes ? descriptor.GetProperties() : descriptor.GetProperties(attributes));
			collection = PipelineInitialize(1, collection, cache);
			ICustomTypeDescriptor extendedDescriptor2 = GetExtendedDescriptor(component);
			if (extendedDescriptor2 != null)
			{
				ICollection secondary2 = (noAttributes ? extendedDescriptor2.GetProperties() : extendedDescriptor2.GetProperties(attributes));
				collection = PipelineMerge(1, collection, secondary2, component, cache);
			}
			collection = PipelineFilter(1, collection, component, cache);
			collection = PipelineAttributeFilter(1, collection, attributes, component, cache);
		}
		PropertyDescriptorCollection propertyDescriptorCollection = collection as PropertyDescriptorCollection;
		if (propertyDescriptorCollection == null)
		{
			PropertyDescriptor[] array = new PropertyDescriptor[collection.Count];
			collection.CopyTo(array, 0);
			propertyDescriptorCollection = new PropertyDescriptorCollection(array, readOnly: true);
		}
		return propertyDescriptorCollection;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static TypeDescriptionProvider GetProvider(Type type)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		return NodeFor(type, createDelegator: true);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static TypeDescriptionProvider GetProvider(object instance)
	{
		if (instance == null)
		{
			throw new ArgumentNullException("instance");
		}
		return NodeFor(instance, createDelegator: true);
	}

	internal static TypeDescriptionProvider GetProviderRecursive(Type type)
	{
		return NodeFor(type, createDelegator: false);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static Type GetReflectionType(Type type)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		return NodeFor(type).GetReflectionType(type);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static Type GetReflectionType(object instance)
	{
		if (instance == null)
		{
			throw new ArgumentNullException("instance");
		}
		return NodeFor(instance).GetReflectionType(instance);
	}

	private static TypeDescriptionNode NodeFor(Type type)
	{
		return NodeFor(type, createDelegator: false);
	}

	private static TypeDescriptionNode NodeFor(Type type, bool createDelegator)
	{
		CheckDefaultProvider(type);
		TypeDescriptionNode typeDescriptionNode = null;
		Type type2 = type;
		while (typeDescriptionNode == null)
		{
			typeDescriptionNode = (TypeDescriptionNode)_providerTypeTable[type2];
			if (typeDescriptionNode == null)
			{
				typeDescriptionNode = (TypeDescriptionNode)_providerTable[type2];
			}
			if (typeDescriptionNode != null)
			{
				continue;
			}
			Type nodeForBaseType = GetNodeForBaseType(type2);
			if (type2 == typeof(object) || nodeForBaseType == null)
			{
				lock (_providerTable)
				{
					typeDescriptionNode = (TypeDescriptionNode)_providerTable[type2];
					if (typeDescriptionNode == null)
					{
						typeDescriptionNode = new TypeDescriptionNode(new ReflectTypeDescriptionProvider());
						_providerTable[type2] = typeDescriptionNode;
					}
				}
			}
			else if (createDelegator)
			{
				typeDescriptionNode = new TypeDescriptionNode(new DelegatingTypeDescriptionProvider(nodeForBaseType));
				lock (_providerTable)
				{
					_providerTypeTable[type2] = typeDescriptionNode;
				}
			}
			else
			{
				type2 = nodeForBaseType;
			}
		}
		return typeDescriptionNode;
	}

	private static TypeDescriptionNode NodeFor(object instance)
	{
		return NodeFor(instance, createDelegator: false);
	}

	private static TypeDescriptionNode NodeFor(object instance, bool createDelegator)
	{
		TypeDescriptionNode typeDescriptionNode = (TypeDescriptionNode)_providerTable[instance];
		if (typeDescriptionNode == null)
		{
			Type type = instance.GetType();
			if (type.IsCOMObject)
			{
				type = ComObjectType;
			}
			typeDescriptionNode = ((!createDelegator) ? NodeFor(type) : new TypeDescriptionNode(new DelegatingTypeDescriptionProvider(type)));
		}
		return typeDescriptionNode;
	}

	private static void NodeRemove(object key, TypeDescriptionProvider provider)
	{
		lock (_providerTable)
		{
			TypeDescriptionNode typeDescriptionNode = (TypeDescriptionNode)_providerTable[key];
			TypeDescriptionNode typeDescriptionNode2 = typeDescriptionNode;
			while (typeDescriptionNode2 != null && typeDescriptionNode2.Provider != provider)
			{
				typeDescriptionNode2 = typeDescriptionNode2.Next;
			}
			if (typeDescriptionNode2 == null)
			{
				return;
			}
			if (typeDescriptionNode2.Next != null)
			{
				typeDescriptionNode2.Provider = typeDescriptionNode2.Next.Provider;
				typeDescriptionNode2.Next = typeDescriptionNode2.Next.Next;
				if (typeDescriptionNode2 == typeDescriptionNode && typeDescriptionNode2.Provider is DelegatingTypeDescriptionProvider)
				{
					_providerTable.Remove(key);
				}
			}
			else if (typeDescriptionNode2 != typeDescriptionNode)
			{
				Type type = key as Type;
				if (type == null)
				{
					type = key.GetType();
				}
				typeDescriptionNode2.Provider = new DelegatingTypeDescriptionProvider(type.BaseType);
			}
			else
			{
				_providerTable.Remove(key);
			}
			_providerTypeTable.Clear();
		}
	}

	private static ICollection PipelineAttributeFilter(int pipelineType, ICollection members, Attribute[] filter, object instance, IDictionary cache)
	{
		IList list = members as ArrayList;
		if (filter == null || filter.Length == 0)
		{
			return members;
		}
		if (cache != null && (list == null || list.IsReadOnly) && cache[_pipelineAttributeFilterKeys[pipelineType]] is AttributeFilterCacheItem attributeFilterCacheItem && attributeFilterCacheItem.IsValid(filter))
		{
			return attributeFilterCacheItem.FilteredMembers;
		}
		if (list == null || list.IsReadOnly)
		{
			list = new ArrayList(members);
		}
		ArrayList arrayList = FilterMembers(list, filter);
		if (arrayList != null)
		{
			list = arrayList;
		}
		if (cache != null)
		{
			ICollection filteredMembers;
			switch (pipelineType)
			{
			case 1:
			{
				PropertyDescriptor[] array2 = new PropertyDescriptor[list.Count];
				list.CopyTo(array2, 0);
				filteredMembers = new PropertyDescriptorCollection(array2, readOnly: true);
				break;
			}
			case 2:
			{
				EventDescriptor[] array = new EventDescriptor[list.Count];
				list.CopyTo(array, 0);
				filteredMembers = new EventDescriptorCollection(array, readOnly: true);
				break;
			}
			default:
				filteredMembers = null;
				break;
			}
			AttributeFilterCacheItem value = new AttributeFilterCacheItem(filter, filteredMembers);
			cache[_pipelineAttributeFilterKeys[pipelineType]] = value;
		}
		return list;
	}

	private static ICollection PipelineFilter(int pipelineType, ICollection members, object instance, IDictionary cache)
	{
		IComponent component = instance as IComponent;
		ITypeDescriptorFilterService typeDescriptorFilterService = null;
		if (component != null)
		{
			ISite site = component.Site;
			if (site != null)
			{
				typeDescriptorFilterService = site.GetService(typeof(ITypeDescriptorFilterService)) as ITypeDescriptorFilterService;
			}
		}
		IList list = members as ArrayList;
		if (typeDescriptorFilterService == null)
		{
			return members;
		}
		if (cache != null && (list == null || list.IsReadOnly) && cache[_pipelineFilterKeys[pipelineType]] is FilterCacheItem filterCacheItem && filterCacheItem.IsValid(typeDescriptorFilterService))
		{
			return filterCacheItem.FilteredMembers;
		}
		OrderedDictionary orderedDictionary = new OrderedDictionary(members.Count);
		bool flag;
		switch (pipelineType)
		{
		case 0:
			foreach (Attribute member in members)
			{
				orderedDictionary[member.TypeId] = member;
			}
			flag = typeDescriptorFilterService.FilterAttributes(component, orderedDictionary);
			break;
		case 1:
		case 2:
			foreach (MemberDescriptor member2 in members)
			{
				string name = member2.Name;
				if (orderedDictionary.Contains(name))
				{
					string extenderCollisionSuffix = GetExtenderCollisionSuffix(member2);
					if (extenderCollisionSuffix != null)
					{
						orderedDictionary[name + extenderCollisionSuffix] = member2;
					}
					MemberDescriptor memberDescriptor2 = (MemberDescriptor)orderedDictionary[name];
					extenderCollisionSuffix = GetExtenderCollisionSuffix(memberDescriptor2);
					if (extenderCollisionSuffix != null)
					{
						orderedDictionary.Remove(name);
						orderedDictionary[memberDescriptor2.Name + extenderCollisionSuffix] = memberDescriptor2;
					}
				}
				else
				{
					orderedDictionary[name] = member2;
				}
			}
			flag = ((pipelineType != 1) ? typeDescriptorFilterService.FilterEvents(component, orderedDictionary) : typeDescriptorFilterService.FilterProperties(component, orderedDictionary));
			break;
		default:
			flag = false;
			break;
		}
		if (list == null || list.IsReadOnly)
		{
			list = new ArrayList(orderedDictionary.Values);
		}
		else
		{
			list.Clear();
			foreach (object value2 in orderedDictionary.Values)
			{
				list.Add(value2);
			}
		}
		if (flag && cache != null)
		{
			ICollection filteredMembers;
			switch (pipelineType)
			{
			case 0:
			{
				Attribute[] array2 = new Attribute[list.Count];
				try
				{
					list.CopyTo(array2, 0);
				}
				catch (InvalidCastException)
				{
					throw new ArgumentException(global::SR.GetString("Expected types in the collection to be of type {0}.", typeof(Attribute).FullName));
				}
				filteredMembers = new AttributeCollection(array2);
				break;
			}
			case 1:
			{
				PropertyDescriptor[] array3 = new PropertyDescriptor[list.Count];
				try
				{
					list.CopyTo(array3, 0);
				}
				catch (InvalidCastException)
				{
					throw new ArgumentException(global::SR.GetString("Expected types in the collection to be of type {0}.", typeof(PropertyDescriptor).FullName));
				}
				filteredMembers = new PropertyDescriptorCollection(array3, readOnly: true);
				break;
			}
			case 2:
			{
				EventDescriptor[] array = new EventDescriptor[list.Count];
				try
				{
					list.CopyTo(array, 0);
				}
				catch (InvalidCastException)
				{
					throw new ArgumentException(global::SR.GetString("Expected types in the collection to be of type {0}.", typeof(EventDescriptor).FullName));
				}
				filteredMembers = new EventDescriptorCollection(array, readOnly: true);
				break;
			}
			default:
				filteredMembers = null;
				break;
			}
			FilterCacheItem value = new FilterCacheItem(typeDescriptorFilterService, filteredMembers);
			cache[_pipelineFilterKeys[pipelineType]] = value;
			cache.Remove(_pipelineAttributeFilterKeys[pipelineType]);
		}
		return list;
	}

	private static ICollection PipelineInitialize(int pipelineType, ICollection members, IDictionary cache)
	{
		if (cache != null)
		{
			bool flag = true;
			if (cache[_pipelineInitializeKeys[pipelineType]] is ICollection collection && collection.Count == members.Count)
			{
				IEnumerator enumerator = collection.GetEnumerator();
				IEnumerator enumerator2 = members.GetEnumerator();
				while (enumerator.MoveNext() && enumerator2.MoveNext())
				{
					if (enumerator.Current != enumerator2.Current)
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag)
			{
				cache.Remove(_pipelineMergeKeys[pipelineType]);
				cache.Remove(_pipelineFilterKeys[pipelineType]);
				cache.Remove(_pipelineAttributeFilterKeys[pipelineType]);
				cache[_pipelineInitializeKeys[pipelineType]] = members;
			}
		}
		return members;
	}

	private static ICollection PipelineMerge(int pipelineType, ICollection primary, ICollection secondary, object instance, IDictionary cache)
	{
		if (secondary == null || secondary.Count == 0)
		{
			return primary;
		}
		if (cache != null && cache[_pipelineMergeKeys[pipelineType]] is ICollection collection && collection.Count == primary.Count + secondary.Count)
		{
			IEnumerator enumerator = collection.GetEnumerator();
			IEnumerator enumerator2 = primary.GetEnumerator();
			bool flag = true;
			while (enumerator2.MoveNext() && enumerator.MoveNext())
			{
				if (enumerator2.Current != enumerator.Current)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				IEnumerator enumerator3 = secondary.GetEnumerator();
				while (enumerator3.MoveNext() && enumerator.MoveNext())
				{
					if (enumerator3.Current != enumerator.Current)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				return collection;
			}
		}
		ArrayList arrayList = new ArrayList(primary.Count + secondary.Count);
		foreach (object item in primary)
		{
			arrayList.Add(item);
		}
		foreach (object item2 in secondary)
		{
			arrayList.Add(item2);
		}
		if (cache != null)
		{
			ICollection value;
			switch (pipelineType)
			{
			case 0:
			{
				Attribute[] array3 = new Attribute[arrayList.Count];
				arrayList.CopyTo(array3, 0);
				value = new AttributeCollection(array3);
				break;
			}
			case 1:
			{
				PropertyDescriptor[] array2 = new PropertyDescriptor[arrayList.Count];
				arrayList.CopyTo(array2, 0);
				value = new PropertyDescriptorCollection(array2, readOnly: true);
				break;
			}
			case 2:
			{
				EventDescriptor[] array = new EventDescriptor[arrayList.Count];
				arrayList.CopyTo(array, 0);
				value = new EventDescriptorCollection(array, readOnly: true);
				break;
			}
			default:
				value = null;
				break;
			}
			cache[_pipelineMergeKeys[pipelineType]] = value;
			cache.Remove(_pipelineFilterKeys[pipelineType]);
			cache.Remove(_pipelineAttributeFilterKeys[pipelineType]);
		}
		return arrayList;
	}

	private static void RaiseRefresh(object component)
	{
		Volatile.Read(ref TypeDescriptor.Refreshed)?.Invoke(new RefreshEventArgs(component));
	}

	private static void RaiseRefresh(Type type)
	{
		Volatile.Read(ref TypeDescriptor.Refreshed)?.Invoke(new RefreshEventArgs(type));
	}

	public static void Refresh(object component)
	{
		Refresh(component, refreshReflectionProvider: true);
	}

	private static void Refresh(object component, bool refreshReflectionProvider)
	{
		if (component == null)
		{
			return;
		}
		bool flag = false;
		if (refreshReflectionProvider)
		{
			Type type = component.GetType();
			lock (_providerTable)
			{
				foreach (DictionaryEntry item in _providerTable)
				{
					Type type2 = item.Key as Type;
					if ((!(type2 != null) || !type.IsAssignableFrom(type2)) && !(type2 == typeof(object)))
					{
						continue;
					}
					TypeDescriptionNode typeDescriptionNode = (TypeDescriptionNode)item.Value;
					while (typeDescriptionNode != null && !(typeDescriptionNode.Provider is ReflectTypeDescriptionProvider))
					{
						flag = true;
						typeDescriptionNode = typeDescriptionNode.Next;
					}
					if (typeDescriptionNode != null)
					{
						ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = (ReflectTypeDescriptionProvider)typeDescriptionNode.Provider;
						if (reflectTypeDescriptionProvider.IsPopulated(type))
						{
							flag = true;
							reflectTypeDescriptionProvider.Refresh(type);
						}
					}
				}
			}
		}
		IDictionary cache = GetCache(component);
		if (!flag && cache == null)
		{
			return;
		}
		if (cache != null)
		{
			for (int i = 0; i < _pipelineFilterKeys.Length; i++)
			{
				cache.Remove(_pipelineFilterKeys[i]);
				cache.Remove(_pipelineMergeKeys[i]);
				cache.Remove(_pipelineAttributeFilterKeys[i]);
			}
		}
		Interlocked.Increment(ref _metadataVersion);
		RaiseRefresh(component);
	}

	public static void Refresh(Type type)
	{
		if (type == null)
		{
			return;
		}
		bool flag = false;
		lock (_providerTable)
		{
			foreach (DictionaryEntry item in _providerTable)
			{
				Type type2 = item.Key as Type;
				if ((!(type2 != null) || !type.IsAssignableFrom(type2)) && !(type2 == typeof(object)))
				{
					continue;
				}
				TypeDescriptionNode typeDescriptionNode = (TypeDescriptionNode)item.Value;
				while (typeDescriptionNode != null && !(typeDescriptionNode.Provider is ReflectTypeDescriptionProvider))
				{
					flag = true;
					typeDescriptionNode = typeDescriptionNode.Next;
				}
				if (typeDescriptionNode != null)
				{
					ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = (ReflectTypeDescriptionProvider)typeDescriptionNode.Provider;
					if (reflectTypeDescriptionProvider.IsPopulated(type))
					{
						flag = true;
						reflectTypeDescriptionProvider.Refresh(type);
					}
				}
			}
		}
		if (flag)
		{
			Interlocked.Increment(ref _metadataVersion);
			RaiseRefresh(type);
		}
	}

	public static void Refresh(Module module)
	{
		if (module == null)
		{
			return;
		}
		Hashtable hashtable = null;
		lock (_providerTable)
		{
			foreach (DictionaryEntry item in _providerTable)
			{
				Type type = item.Key as Type;
				if ((!(type != null) || !type.Module.Equals(module)) && !(type == typeof(object)))
				{
					continue;
				}
				TypeDescriptionNode typeDescriptionNode = (TypeDescriptionNode)item.Value;
				while (typeDescriptionNode != null && !(typeDescriptionNode.Provider is ReflectTypeDescriptionProvider))
				{
					if (hashtable == null)
					{
						hashtable = new Hashtable();
					}
					hashtable[type] = type;
					typeDescriptionNode = typeDescriptionNode.Next;
				}
				if (typeDescriptionNode == null)
				{
					continue;
				}
				ReflectTypeDescriptionProvider reflectTypeDescriptionProvider = (ReflectTypeDescriptionProvider)typeDescriptionNode.Provider;
				Type[] populatedTypes = reflectTypeDescriptionProvider.GetPopulatedTypes(module);
				foreach (Type type2 in populatedTypes)
				{
					reflectTypeDescriptionProvider.Refresh(type2);
					if (hashtable == null)
					{
						hashtable = new Hashtable();
					}
					hashtable[type2] = type2;
				}
			}
		}
		if (hashtable == null || TypeDescriptor.Refreshed == null)
		{
			return;
		}
		foreach (Type key in hashtable.Keys)
		{
			RaiseRefresh(key);
		}
	}

	public static void Refresh(Assembly assembly)
	{
		if (!(assembly == null))
		{
			Module[] modules = assembly.GetModules();
			for (int i = 0; i < modules.Length; i++)
			{
				Refresh(modules[i]);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static void RemoveAssociation(object primary, object secondary)
	{
		if (primary == null)
		{
			throw new ArgumentNullException("primary");
		}
		if (secondary == null)
		{
			throw new ArgumentNullException("secondary");
		}
		Hashtable associationTable = _associationTable;
		if (associationTable == null)
		{
			return;
		}
		IList list = (IList)associationTable[primary];
		if (list == null)
		{
			return;
		}
		lock (list)
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				object target = ((WeakReference)list[num]).Target;
				if (target == null || target == secondary)
				{
					list.RemoveAt(num);
				}
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static void RemoveAssociations(object primary)
	{
		if (primary == null)
		{
			throw new ArgumentNullException("primary");
		}
		_associationTable?.Remove(primary);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static void RemoveProvider(TypeDescriptionProvider provider, Type type)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		NodeRemove(type, provider);
		RaiseRefresh(type);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static void RemoveProvider(TypeDescriptionProvider provider, object instance)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		if (instance == null)
		{
			throw new ArgumentNullException("instance");
		}
		NodeRemove(instance, provider);
		RaiseRefresh(instance);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void RemoveProviderTransparent(TypeDescriptionProvider provider, Type type)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		RemoveProvider(provider, type);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void RemoveProviderTransparent(TypeDescriptionProvider provider, object instance)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		if (instance == null)
		{
			throw new ArgumentNullException("instance");
		}
		RemoveProvider(provider, instance);
	}

	private static bool ShouldHideMember(MemberDescriptor member, Attribute attribute)
	{
		if (member == null || attribute == null)
		{
			return true;
		}
		Attribute attribute2 = member.Attributes[attribute.GetType()];
		if (attribute2 == null)
		{
			return !attribute.IsDefaultAttribute();
		}
		return !attribute.Match(attribute2);
	}

	public static void SortDescriptorArray(IList infos)
	{
		if (infos == null)
		{
			throw new ArgumentNullException("infos");
		}
		ArrayList.Adapter(infos).Sort(MemberDescriptorComparer.Instance);
	}

	[Conditional("DEBUG")]
	internal static void Trace(string message, params object[] args)
	{
	}
}
