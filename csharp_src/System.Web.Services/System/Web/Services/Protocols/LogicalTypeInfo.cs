using System.Collections;
using System.Reflection;
using System.Web.Services.Description;

namespace System.Web.Services.Protocols;

internal class LogicalTypeInfo
{
	private LogicalMethodInfo[] logicalMethods;

	internal string WebServiceName;

	internal string WebServiceNamespace;

	internal string WebServiceAbstractNamespace;

	internal string Description;

	internal Type Type;

	private SoapBindingUse bindingUse;

	private SoapServiceRoutingStyle routingStyle;

	private TypeStubInfo soapProtocol;

	private TypeStubInfo soap12Protocol;

	private TypeStubInfo httpGetProtocol;

	private TypeStubInfo httpPostProtocol;

	internal SoapBindingUse BindingUse => bindingUse;

	internal SoapServiceRoutingStyle RoutingStyle => routingStyle;

	internal LogicalMethodInfo[] LogicalMethods => logicalMethods;

	public LogicalTypeInfo(Type t)
	{
		Type = t;
		object[] customAttributes = Type.GetCustomAttributes(typeof(WebServiceAttribute), inherit: false);
		if (customAttributes.Length == 1)
		{
			WebServiceAttribute webServiceAttribute = (WebServiceAttribute)customAttributes[0];
			WebServiceName = ((webServiceAttribute.Name != string.Empty) ? webServiceAttribute.Name : Type.Name);
			WebServiceNamespace = ((webServiceAttribute.Namespace != string.Empty) ? webServiceAttribute.Namespace : "http://tempuri.org/");
			Description = webServiceAttribute.Description;
		}
		else
		{
			WebServiceName = Type.Name;
			WebServiceNamespace = "http://tempuri.org/";
		}
		bindingUse = SoapBindingUse.Literal;
		customAttributes = t.GetCustomAttributes(typeof(SoapDocumentServiceAttribute), inherit: true);
		if (customAttributes.Length != 0)
		{
			SoapDocumentServiceAttribute soapDocumentServiceAttribute = (SoapDocumentServiceAttribute)customAttributes[0];
			bindingUse = soapDocumentServiceAttribute.Use;
			if (bindingUse == SoapBindingUse.Default)
			{
				bindingUse = SoapBindingUse.Literal;
			}
			routingStyle = soapDocumentServiceAttribute.RoutingStyle;
		}
		else if (t.GetCustomAttributes(typeof(SoapRpcServiceAttribute), inherit: true).Length != 0)
		{
			customAttributes = t.GetCustomAttributes(typeof(SoapRpcServiceAttribute), inherit: true);
			SoapRpcServiceAttribute soapRpcServiceAttribute = (SoapRpcServiceAttribute)customAttributes[0];
			bindingUse = soapRpcServiceAttribute.Use;
			routingStyle = soapRpcServiceAttribute.RoutingStyle;
			if (bindingUse == SoapBindingUse.Default)
			{
				bindingUse = SoapBindingUse.Encoded;
			}
		}
		else
		{
			routingStyle = SoapServiceRoutingStyle.SoapAction;
		}
		string text = (WebServiceNamespace.EndsWith("/") ? "" : "/");
		WebServiceAbstractNamespace = WebServiceNamespace + text + "AbstractTypes";
		MethodInfo[] method_infos;
		if (typeof(WebClientProtocol).IsAssignableFrom(Type))
		{
			method_infos = Type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
		}
		else
		{
			MethodInfo[] methods = Type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			ArrayList arrayList = new ArrayList(methods.Length);
			MethodInfo[] array = methods;
			foreach (MethodInfo methodInfo in array)
			{
				if (methodInfo.IsPublic && methodInfo.GetCustomAttributes(typeof(WebMethodAttribute), inherit: false).Length != 0)
				{
					arrayList.Add(methodInfo);
					continue;
				}
				Type[] interfaces = Type.GetInterfaces();
				foreach (Type type in interfaces)
				{
					if (type.GetCustomAttributes(typeof(WebServiceBindingAttribute), inherit: false).Length == 0)
					{
						continue;
					}
					MethodInfo methodInfo2 = FindInInterface(type, methodInfo);
					if (methodInfo2 != null)
					{
						if (methodInfo2.GetCustomAttributes(typeof(WebMethodAttribute), inherit: false).Length != 0)
						{
							arrayList.Add(methodInfo2);
						}
						break;
					}
				}
			}
			method_infos = (MethodInfo[])arrayList.ToArray(typeof(MethodInfo));
		}
		logicalMethods = LogicalMethodInfo.Create(method_infos, LogicalMethodTypes.Sync);
	}

	private static MethodInfo FindInInterface(Type ifaceType, MethodInfo method)
	{
		int indexA = 0;
		if (method.IsPrivate)
		{
			indexA = method.Name.LastIndexOf('.');
			if (indexA < 0)
			{
				indexA = 0;
			}
			else
			{
				if (string.CompareOrdinal(ifaceType.FullName.Replace('+', '.'), 0, method.Name, 0, indexA) != 0)
				{
					return null;
				}
				indexA++;
			}
		}
		MemberInfo[] members = ifaceType.GetMembers();
		for (int i = 0; i < members.Length; i++)
		{
			MethodInfo methodInfo = (MethodInfo)members[i];
			if (!(method.ReturnType == methodInfo.ReturnType) || string.CompareOrdinal(method.Name, indexA, methodInfo.Name, 0, methodInfo.Name.Length) != 0)
			{
				continue;
			}
			ParameterInfo[] parameters = method.GetParameters();
			ParameterInfo[] parameters2 = methodInfo.GetParameters();
			if (parameters.Length != parameters2.Length)
			{
				continue;
			}
			bool flag = true;
			for (int j = 0; j < parameters.Length; j++)
			{
				if (parameters[j].ParameterType != parameters2[j].ParameterType)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return methodInfo;
			}
		}
		return null;
	}

	internal TypeStubInfo GetTypeStub(string protocolName)
	{
		lock (this)
		{
			if (!(protocolName == "Soap"))
			{
				if (protocolName == "Soap12")
				{
					if (soap12Protocol == null)
					{
						soap12Protocol = new Soap12TypeStubInfo(this);
						soap12Protocol.Initialize();
					}
					return soap12Protocol;
				}
				throw new InvalidOperationException("Protocol " + protocolName + " not supported");
			}
			if (soapProtocol == null)
			{
				soapProtocol = new SoapTypeStubInfo(this);
				soapProtocol.Initialize();
			}
			return soapProtocol;
		}
	}

	internal string GetWebServiceLiteralNamespace(string baseNamespace)
	{
		if (BindingUse == SoapBindingUse.Encoded)
		{
			string text = (baseNamespace.EndsWith("/") ? "" : "/");
			return baseNamespace + text + "literalTypes";
		}
		return baseNamespace;
	}

	internal string GetWebServiceEncodedNamespace(string baseNamespace)
	{
		if (BindingUse == SoapBindingUse.Encoded)
		{
			return baseNamespace;
		}
		string text = (baseNamespace.EndsWith("/") ? "" : "/");
		return baseNamespace + text + "encodedTypes";
	}

	internal string GetWebServiceNamespace(string baseNamespace, SoapBindingUse use)
	{
		if (use == SoapBindingUse.Literal)
		{
			return GetWebServiceLiteralNamespace(baseNamespace);
		}
		return GetWebServiceEncodedNamespace(baseNamespace);
	}
}
