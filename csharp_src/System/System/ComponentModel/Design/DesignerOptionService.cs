using System.Collections;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel.Design;

[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
public abstract class DesignerOptionService : IDesignerOptionService
{
	[TypeConverter(typeof(DesignerOptionConverter))]
	[Editor("", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public sealed class DesignerOptionCollection : IList, ICollection, IEnumerable
	{
		private sealed class WrappedPropertyDescriptor : PropertyDescriptor
		{
			private object target;

			private PropertyDescriptor property;

			public override AttributeCollection Attributes => property.Attributes;

			public override Type ComponentType => property.ComponentType;

			public override bool IsReadOnly => property.IsReadOnly;

			public override Type PropertyType => property.PropertyType;

			internal WrappedPropertyDescriptor(PropertyDescriptor property, object target)
				: base(property.Name, null)
			{
				this.property = property;
				this.target = target;
			}

			public override bool CanResetValue(object component)
			{
				return property.CanResetValue(target);
			}

			public override object GetValue(object component)
			{
				return property.GetValue(target);
			}

			public override void ResetValue(object component)
			{
				property.ResetValue(target);
			}

			public override void SetValue(object component, object value)
			{
				property.SetValue(target, value);
			}

			public override bool ShouldSerializeValue(object component)
			{
				return property.ShouldSerializeValue(target);
			}
		}

		private DesignerOptionService _service;

		private DesignerOptionCollection _parent;

		private string _name;

		private object _value;

		private ArrayList _children;

		private PropertyDescriptorCollection _properties;

		public int Count
		{
			get
			{
				EnsurePopulated();
				return _children.Count;
			}
		}

		public string Name => _name;

		public DesignerOptionCollection Parent => _parent;

		public PropertyDescriptorCollection Properties
		{
			get
			{
				if (_properties == null)
				{
					ArrayList arrayList;
					if (_value != null)
					{
						PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(_value);
						arrayList = new ArrayList(properties.Count);
						foreach (PropertyDescriptor item in properties)
						{
							arrayList.Add(new WrappedPropertyDescriptor(item, _value));
						}
					}
					else
					{
						arrayList = new ArrayList(1);
					}
					EnsurePopulated();
					foreach (DesignerOptionCollection child in _children)
					{
						arrayList.AddRange(child.Properties);
					}
					PropertyDescriptor[] properties2 = (PropertyDescriptor[])arrayList.ToArray(typeof(PropertyDescriptor));
					_properties = new PropertyDescriptorCollection(properties2, readOnly: true);
				}
				return _properties;
			}
		}

		public DesignerOptionCollection this[int index]
		{
			get
			{
				EnsurePopulated();
				if (index < 0 || index >= _children.Count)
				{
					throw new IndexOutOfRangeException("index");
				}
				return (DesignerOptionCollection)_children[index];
			}
		}

		public DesignerOptionCollection this[string name]
		{
			get
			{
				EnsurePopulated();
				foreach (DesignerOptionCollection child in _children)
				{
					if (string.Compare(child.Name, name, ignoreCase: true, CultureInfo.InvariantCulture) == 0)
					{
						return child;
					}
				}
				return null;
			}
		}

		bool ICollection.IsSynchronized => false;

		object ICollection.SyncRoot => this;

		bool IList.IsFixedSize => true;

		bool IList.IsReadOnly => true;

		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		internal DesignerOptionCollection(DesignerOptionService service, DesignerOptionCollection parent, string name, object value)
		{
			_service = service;
			_parent = parent;
			_name = name;
			_value = value;
			if (_parent != null)
			{
				if (_parent._children == null)
				{
					_parent._children = new ArrayList(1);
				}
				_parent._children.Add(this);
			}
		}

		public void CopyTo(Array array, int index)
		{
			EnsurePopulated();
			_children.CopyTo(array, index);
		}

		private void EnsurePopulated()
		{
			if (_children == null)
			{
				_service.PopulateOptionCollection(this);
				if (_children == null)
				{
					_children = new ArrayList(1);
				}
			}
		}

		public IEnumerator GetEnumerator()
		{
			EnsurePopulated();
			return _children.GetEnumerator();
		}

		public int IndexOf(DesignerOptionCollection value)
		{
			EnsurePopulated();
			return _children.IndexOf(value);
		}

		private static object RecurseFindValue(DesignerOptionCollection options)
		{
			if (options._value != null)
			{
				return options._value;
			}
			foreach (DesignerOptionCollection option in options)
			{
				object obj = RecurseFindValue(option);
				if (obj != null)
				{
					return obj;
				}
			}
			return null;
		}

		public bool ShowDialog()
		{
			object obj = RecurseFindValue(this);
			if (obj == null)
			{
				return false;
			}
			return _service.ShowDialog(this, obj);
		}

		int IList.Add(object value)
		{
			throw new NotSupportedException();
		}

		void IList.Clear()
		{
			throw new NotSupportedException();
		}

		bool IList.Contains(object value)
		{
			EnsurePopulated();
			return _children.Contains(value);
		}

		int IList.IndexOf(object value)
		{
			EnsurePopulated();
			return _children.IndexOf(value);
		}

		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		void IList.Remove(object value)
		{
			throw new NotSupportedException();
		}

		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}
	}

	internal sealed class DesignerOptionConverter : TypeConverter
	{
		private class OptionPropertyDescriptor : PropertyDescriptor
		{
			private DesignerOptionCollection _option;

			public override Type ComponentType => _option.GetType();

			public override bool IsReadOnly => true;

			public override Type PropertyType => _option.GetType();

			internal OptionPropertyDescriptor(DesignerOptionCollection option)
				: base(option.Name, null)
			{
				_option = option;
			}

			public override bool CanResetValue(object component)
			{
				return false;
			}

			public override object GetValue(object component)
			{
				return _option;
			}

			public override void ResetValue(object component)
			{
			}

			public override void SetValue(object component, object value)
			{
			}

			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext cxt)
		{
			return true;
		}

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext cxt, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
			if (!(value is DesignerOptionCollection designerOptionCollection))
			{
				return propertyDescriptorCollection;
			}
			foreach (DesignerOptionCollection item in designerOptionCollection)
			{
				propertyDescriptorCollection.Add(new OptionPropertyDescriptor(item));
			}
			foreach (PropertyDescriptor property in designerOptionCollection.Properties)
			{
				propertyDescriptorCollection.Add(property);
			}
			return propertyDescriptorCollection;
		}

		public override object ConvertTo(ITypeDescriptorContext cxt, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return global::SR.GetString("(Collection)");
			}
			return base.ConvertTo(cxt, culture, value, destinationType);
		}
	}

	private DesignerOptionCollection _options;

	public DesignerOptionCollection Options
	{
		get
		{
			if (_options == null)
			{
				_options = new DesignerOptionCollection(this, null, string.Empty, null);
			}
			return _options;
		}
	}

	protected DesignerOptionCollection CreateOptionCollection(DesignerOptionCollection parent, string name, object value)
	{
		if (parent == null)
		{
			throw new ArgumentNullException("parent");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0)
		{
			throw new ArgumentException(global::SR.GetString("'{1}' is not a valid value for '{0}'.", name.Length.ToString(CultureInfo.CurrentCulture), 0.ToString(CultureInfo.CurrentCulture)), "name.Length");
		}
		return new DesignerOptionCollection(this, parent, name, value);
	}

	private PropertyDescriptor GetOptionProperty(string pageName, string valueName)
	{
		if (pageName == null)
		{
			throw new ArgumentNullException("pageName");
		}
		if (valueName == null)
		{
			throw new ArgumentNullException("valueName");
		}
		string[] array = pageName.Split(new char[1] { '\\' });
		DesignerOptionCollection designerOptionCollection = Options;
		string[] array2 = array;
		foreach (string name in array2)
		{
			designerOptionCollection = designerOptionCollection[name];
			if (designerOptionCollection == null)
			{
				return null;
			}
		}
		return designerOptionCollection.Properties[valueName];
	}

	protected virtual void PopulateOptionCollection(DesignerOptionCollection options)
	{
	}

	protected virtual bool ShowDialog(DesignerOptionCollection options, object optionObject)
	{
		return false;
	}

	object IDesignerOptionService.GetOptionValue(string pageName, string valueName)
	{
		return GetOptionProperty(pageName, valueName)?.GetValue(null);
	}

	void IDesignerOptionService.SetOptionValue(string pageName, string valueName, object value)
	{
		GetOptionProperty(pageName, valueName)?.SetValue(null, value);
	}
}
