using System.Reflection;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public class PropertyTabAttribute : Attribute
{
	private Type[] _tabClasses;

	private string[] _tabClassNames;

	public Type[] TabClasses
	{
		get
		{
			if (_tabClasses == null && _tabClassNames != null)
			{
				_tabClasses = new Type[_tabClassNames.Length];
				for (int i = 0; i < _tabClassNames.Length; i++)
				{
					int num = _tabClassNames[i].IndexOf(',');
					string text = null;
					string text2 = null;
					if (num != -1)
					{
						text = _tabClassNames[i].Substring(0, num).Trim();
						text2 = _tabClassNames[i].Substring(num + 1).Trim();
					}
					else
					{
						text = _tabClassNames[i];
					}
					_tabClasses[i] = Type.GetType(text, throwOnError: false);
					if (_tabClasses[i] == null)
					{
						if (text2 == null)
						{
							throw new TypeLoadException(global::SR.Format("Couldn't find type {0}", text));
						}
						Assembly assembly = Assembly.Load(text2);
						if (assembly != null)
						{
							_tabClasses[i] = assembly.GetType(text, throwOnError: true);
						}
					}
				}
			}
			return _tabClasses;
		}
	}

	protected string[] TabClassNames => (string[])_tabClassNames?.Clone();

	public PropertyTabScope[] TabScopes { get; private set; }

	public PropertyTabAttribute()
	{
		TabScopes = Array.Empty<PropertyTabScope>();
		_tabClassNames = Array.Empty<string>();
	}

	public PropertyTabAttribute(Type tabClass)
		: this(tabClass, PropertyTabScope.Component)
	{
	}

	public PropertyTabAttribute(string tabClassName)
		: this(tabClassName, PropertyTabScope.Component)
	{
	}

	public PropertyTabAttribute(Type tabClass, PropertyTabScope tabScope)
	{
		_tabClasses = new Type[1] { tabClass };
		if (tabScope < PropertyTabScope.Document)
		{
			throw new ArgumentException(global::SR.Format("Scope must be PropertyTabScope.Document or PropertyTabScope.Component"), "tabScope");
		}
		TabScopes = new PropertyTabScope[1] { tabScope };
	}

	public PropertyTabAttribute(string tabClassName, PropertyTabScope tabScope)
	{
		_tabClassNames = new string[1] { tabClassName };
		if (tabScope < PropertyTabScope.Document)
		{
			throw new ArgumentException(global::SR.Format("Scope must be PropertyTabScope.Document or PropertyTabScope.Component"), "tabScope");
		}
		TabScopes = new PropertyTabScope[1] { tabScope };
	}

	public override bool Equals(object other)
	{
		if (other is PropertyTabAttribute)
		{
			return Equals((PropertyTabAttribute)other);
		}
		return false;
	}

	public bool Equals(PropertyTabAttribute other)
	{
		if (other == this)
		{
			return true;
		}
		if (other.TabClasses.Length != TabClasses.Length || other.TabScopes.Length != TabScopes.Length)
		{
			return false;
		}
		for (int i = 0; i < TabClasses.Length; i++)
		{
			if (TabClasses[i] != other.TabClasses[i] || TabScopes[i] != other.TabScopes[i])
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	protected void InitializeArrays(string[] tabClassNames, PropertyTabScope[] tabScopes)
	{
		InitializeArrays(tabClassNames, null, tabScopes);
	}

	protected void InitializeArrays(Type[] tabClasses, PropertyTabScope[] tabScopes)
	{
		InitializeArrays(null, tabClasses, tabScopes);
	}

	private void InitializeArrays(string[] tabClassNames, Type[] tabClasses, PropertyTabScope[] tabScopes)
	{
		if (tabClasses != null)
		{
			if (tabScopes != null && tabClasses.Length != tabScopes.Length)
			{
				throw new ArgumentException("tabClasses must have the same number of items as tabScopes");
			}
			_tabClasses = (Type[])tabClasses.Clone();
		}
		else if (tabClassNames != null)
		{
			if (tabScopes != null && tabClassNames.Length != tabScopes.Length)
			{
				throw new ArgumentException("tabClasses must have the same number of items as tabScopes");
			}
			_tabClassNames = (string[])tabClassNames.Clone();
			_tabClasses = null;
		}
		else if (_tabClasses == null && _tabClassNames == null)
		{
			throw new ArgumentException("An array of tab type names or tab types must be specified");
		}
		if (tabScopes != null)
		{
			for (int i = 0; i < tabScopes.Length; i++)
			{
				if (tabScopes[i] < PropertyTabScope.Document)
				{
					throw new ArgumentException("Scope must be PropertyTabScope.Document or PropertyTabScope.Component");
				}
			}
			TabScopes = (PropertyTabScope[])tabScopes.Clone();
		}
		else
		{
			TabScopes = new PropertyTabScope[tabClasses.Length];
			for (int j = 0; j < TabScopes.Length; j++)
			{
				TabScopes[j] = PropertyTabScope.Component;
			}
		}
	}
}
