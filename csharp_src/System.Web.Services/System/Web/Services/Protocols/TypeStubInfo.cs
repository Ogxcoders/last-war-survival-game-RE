using System.Collections;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

internal abstract class TypeStubInfo
{
	private Hashtable name_to_method = new Hashtable();

	private MethodStubInfo[] methods;

	private ArrayList bindings = new ArrayList();

	private LogicalTypeInfo logicalType;

	private string defaultBinding;

	private ArrayList mappings;

	private XmlSerializer[] serializers;

	public WsiProfiles WsiClaims
	{
		get
		{
			if (((BindingInfo)Bindings[0]).WebServiceBindingAttribute == null)
			{
				return WsiProfiles.None;
			}
			return ((BindingInfo)Bindings[0]).WebServiceBindingAttribute.ConformsTo;
		}
	}

	public LogicalTypeInfo LogicalType => logicalType;

	public Type Type => logicalType.Type;

	public string DefaultBinding => defaultBinding;

	public virtual XmlReflectionImporter XmlImporter => null;

	public virtual SoapReflectionImporter SoapImporter => null;

	public virtual string ProtocolName => null;

	public MethodStubInfo[] Methods => methods;

	internal ArrayList Bindings => bindings;

	public TypeStubInfo(LogicalTypeInfo logicalTypeInfo)
	{
		logicalType = logicalTypeInfo;
		object[] customAttributes = Type.GetCustomAttributes(typeof(WebServiceBindingAttribute), inherit: false);
		bool flag = typeof(SoapHttpClientProtocol).IsAssignableFrom(Type);
		bool flag2 = false;
		string text = logicalType.WebServiceName + ProtocolName;
		if (customAttributes.Length != 0)
		{
			object[] array = customAttributes;
			for (int i = 0; i < array.Length; i++)
			{
				WebServiceBindingAttribute webServiceBindingAttribute = (WebServiceBindingAttribute)array[i];
				AddBinding(new BindingInfo(webServiceBindingAttribute, text, LogicalType.WebServiceNamespace));
				if (webServiceBindingAttribute.Name == null || webServiceBindingAttribute.Name.Length == 0 || webServiceBindingAttribute.Name == text)
				{
					flag2 = true;
				}
			}
		}
		if (!flag2 && !flag)
		{
			AddBindingAt(0, new BindingInfo(null, text, logicalType.WebServiceNamespace));
		}
		Type[] interfaces = Type.GetInterfaces();
		foreach (Type type in interfaces)
		{
			customAttributes = type.GetCustomAttributes(typeof(WebServiceBindingAttribute), inherit: false);
			if (customAttributes.Length != 0)
			{
				text = type.Name + ProtocolName;
				object[] array = customAttributes;
				for (int j = 0; j < array.Length; j++)
				{
					WebServiceBindingAttribute at = (WebServiceBindingAttribute)array[j];
					AddBinding(new BindingInfo(at, text, LogicalType.WebServiceNamespace));
				}
			}
		}
	}

	public XmlSerializer GetSerializer(int n)
	{
		return serializers[n];
	}

	public int RegisterSerializer(XmlMapping map)
	{
		if (mappings == null)
		{
			mappings = new ArrayList();
		}
		return mappings.Add(map);
	}

	public void Initialize()
	{
		BuildTypeMethods();
		if (mappings != null)
		{
			XmlMapping[] array = (XmlMapping[])mappings.ToArray(typeof(XmlMapping));
			serializers = XmlSerializer.FromMappings(array);
		}
	}

	protected virtual void BuildTypeMethods()
	{
		bool flag = typeof(WebClientProtocol).IsAssignableFrom(Type);
		ArrayList arrayList = new ArrayList();
		LogicalMethodInfo[] logicalMethods = logicalType.LogicalMethods;
		foreach (LogicalMethodInfo logicalMethodInfo in logicalMethods)
		{
			if (!flag && logicalMethodInfo.CustomAttributeProvider.GetCustomAttributes(typeof(WebMethodAttribute), inherit: true).Length == 0)
			{
				continue;
			}
			MethodStubInfo methodStubInfo = CreateMethodStubInfo(this, logicalMethodInfo, flag);
			if (methodStubInfo != null)
			{
				if (name_to_method.ContainsKey(methodStubInfo.Name))
				{
					throw new InvalidOperationException(string.Concat("Both " + methodStubInfo.MethodInfo.ToString() + " and " + GetMethod(methodStubInfo.Name).MethodInfo?.ToString() + " use the message name '" + methodStubInfo.Name + "'. ", "Use the MessageName property of WebMethod custom attribute to specify unique message names for the methods"));
				}
				name_to_method[methodStubInfo.Name] = methodStubInfo;
				arrayList.Add(methodStubInfo);
			}
		}
		methods = (MethodStubInfo[])arrayList.ToArray(typeof(MethodStubInfo));
	}

	protected abstract MethodStubInfo CreateMethodStubInfo(TypeStubInfo typeInfo, LogicalMethodInfo methodInfo, bool isClientProxy);

	public MethodStubInfo GetMethod(string name)
	{
		return (MethodStubInfo)name_to_method[name];
	}

	internal void AddBinding(BindingInfo info)
	{
		bindings.Add(info);
	}

	internal void AddBindingAt(int pos, BindingInfo info)
	{
		bindings.Insert(pos, info);
	}

	internal BindingInfo GetBinding(string name)
	{
		if (name == null || name.Length == 0)
		{
			return (BindingInfo)bindings[0];
		}
		for (int i = 0; i < bindings.Count; i++)
		{
			if (((BindingInfo)bindings[i]).Name == name)
			{
				return (BindingInfo)bindings[i];
			}
		}
		return null;
	}
}
