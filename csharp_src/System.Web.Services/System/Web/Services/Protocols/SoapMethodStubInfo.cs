using System.Collections;
using System.Reflection;
using System.Web.Services.Description;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

internal class SoapMethodStubInfo : MethodStubInfo
{
	internal readonly string Action;

	internal readonly string Binding;

	internal readonly string RequestName;

	internal readonly string RequestNamespace;

	internal readonly string ResponseName;

	internal readonly string ResponseNamespace;

	internal readonly bool OneWay;

	internal readonly SoapParameterStyle ParameterStyle;

	internal readonly SoapBindingStyle SoapBindingStyle;

	internal readonly SoapBindingUse Use;

	internal readonly SoapHeaderMapping[] Headers;

	internal readonly SoapHeaderMapping[] InHeaders;

	internal readonly SoapHeaderMapping[] OutHeaders;

	internal readonly SoapHeaderMapping[] FaultHeaders;

	internal readonly SoapExtensionRuntimeConfig[] SoapExtensions;

	internal readonly XmlMembersMapping InputMembersMapping;

	internal readonly XmlMembersMapping OutputMembersMapping;

	internal readonly XmlMembersMapping InputHeaderMembersMapping;

	internal readonly XmlMembersMapping OutputHeaderMembersMapping;

	internal readonly XmlMembersMapping FaultHeaderMembersMapping;

	private readonly int requestSerializerId;

	private readonly int responseSerializerId;

	private readonly int requestHeadersSerializerId = -1;

	private readonly int responseHeadersSerializerId = -1;

	private readonly int faultHeadersSerializerId = -1;

	internal XmlSerializer RequestSerializer => TypeStub.GetSerializer(requestSerializerId);

	internal XmlSerializer ResponseSerializer => TypeStub.GetSerializer(responseSerializerId);

	internal XmlSerializer RequestHeadersSerializer
	{
		get
		{
			if (requestHeadersSerializerId == -1)
			{
				return null;
			}
			return TypeStub.GetSerializer(requestHeadersSerializerId);
		}
	}

	internal XmlSerializer ResponseHeadersSerializer
	{
		get
		{
			if (responseHeadersSerializerId == -1)
			{
				return null;
			}
			return TypeStub.GetSerializer(responseHeadersSerializerId);
		}
	}

	internal XmlSerializer FaultHeadersSerializer
	{
		get
		{
			if (faultHeadersSerializerId == -1)
			{
				return null;
			}
			return TypeStub.GetSerializer(faultHeadersSerializerId);
		}
	}

	public SoapMethodStubInfo(TypeStubInfo typeStub, LogicalMethodInfo source, object kind, XmlReflectionImporter xmlImporter, SoapReflectionImporter soapImporter)
		: base(typeStub, source)
	{
		SoapTypeStubInfo soapTypeStubInfo = (SoapTypeStubInfo)typeStub;
		XmlElementAttribute optional_ns = null;
		if (kind == null)
		{
			Use = soapTypeStubInfo.LogicalType.BindingUse;
			RequestName = "";
			RequestNamespace = "";
			ResponseName = "";
			ResponseNamespace = "";
			ParameterStyle = soapTypeStubInfo.ParameterStyle;
			SoapBindingStyle = soapTypeStubInfo.SoapBindingStyle;
			OneWay = false;
		}
		else if (kind is SoapDocumentMethodAttribute)
		{
			SoapDocumentMethodAttribute soapDocumentMethodAttribute = (SoapDocumentMethodAttribute)kind;
			Use = soapDocumentMethodAttribute.Use;
			if (Use == SoapBindingUse.Default)
			{
				if (soapTypeStubInfo.SoapBindingStyle == SoapBindingStyle.Document)
				{
					Use = soapTypeStubInfo.LogicalType.BindingUse;
				}
				else
				{
					Use = SoapBindingUse.Literal;
				}
			}
			Action = soapDocumentMethodAttribute.Action;
			Binding = soapDocumentMethodAttribute.Binding;
			RequestName = soapDocumentMethodAttribute.RequestElementName;
			RequestNamespace = soapDocumentMethodAttribute.RequestNamespace;
			ResponseName = soapDocumentMethodAttribute.ResponseElementName;
			ResponseNamespace = soapDocumentMethodAttribute.ResponseNamespace;
			ParameterStyle = soapDocumentMethodAttribute.ParameterStyle;
			if (ParameterStyle == SoapParameterStyle.Default)
			{
				ParameterStyle = soapTypeStubInfo.ParameterStyle;
			}
			OneWay = soapDocumentMethodAttribute.OneWay;
			SoapBindingStyle = SoapBindingStyle.Document;
		}
		else
		{
			SoapRpcMethodAttribute soapRpcMethodAttribute = (SoapRpcMethodAttribute)kind;
			Use = SoapBindingUse.Encoded;
			Action = soapRpcMethodAttribute.Action;
			if (Action != null && Action.Length == 0)
			{
				Action = null;
			}
			Binding = soapRpcMethodAttribute.Binding;
			RequestName = source.Name;
			ResponseName = source.Name + "Response";
			RequestNamespace = soapRpcMethodAttribute.RequestNamespace;
			ResponseNamespace = soapRpcMethodAttribute.ResponseNamespace;
			ParameterStyle = SoapParameterStyle.Wrapped;
			OneWay = soapRpcMethodAttribute.OneWay;
			SoapBindingStyle = SoapBindingStyle.Rpc;
			optional_ns = new XmlElementAttribute
			{
				Namespace = ""
			};
		}
		if (OneWay)
		{
			if (source.ReturnType != typeof(void))
			{
				throw new Exception("OneWay methods should not have a return value.");
			}
			if (source.OutParameters.Length != 0)
			{
				throw new Exception("OneWay methods should not have out/ref parameters.");
			}
		}
		BindingInfo binding = soapTypeStubInfo.GetBinding(Binding);
		if (binding == null)
		{
			throw new InvalidOperationException("Type '" + soapTypeStubInfo.Type?.ToString() + "' is missing WebServiceBinding attribute that defines a binding named '" + Binding + "'.");
		}
		string text = binding.Namespace;
		if (RequestNamespace == "")
		{
			RequestNamespace = soapTypeStubInfo.LogicalType.GetWebServiceNamespace(text, Use);
		}
		if (ResponseNamespace == "")
		{
			ResponseNamespace = soapTypeStubInfo.LogicalType.GetWebServiceNamespace(text, Use);
		}
		if (RequestName == "")
		{
			RequestName = Name;
		}
		if (ResponseName == "")
		{
			ResponseName = Name + "Response";
		}
		if (Action == null)
		{
			Action = (text.EndsWith("/") ? (text + Name) : (text + "/" + Name));
		}
		bool hasWrapperElement = ParameterStyle == SoapParameterStyle.Wrapped;
		bool writeAccessors = SoapBindingStyle == SoapBindingStyle.Rpc;
		XmlReflectionMember[] members = BuildRequestReflectionMembers(optional_ns);
		XmlReflectionMember[] members2 = BuildResponseReflectionMembers(optional_ns);
		if (Use == SoapBindingUse.Literal)
		{
			xmlImporter.IncludeTypes(source.CustomAttributeProvider);
			InputMembersMapping = xmlImporter.ImportMembersMapping(RequestName, RequestNamespace, members, hasWrapperElement);
			OutputMembersMapping = xmlImporter.ImportMembersMapping(ResponseName, ResponseNamespace, members2, hasWrapperElement);
		}
		else
		{
			soapImporter.IncludeTypes(source.CustomAttributeProvider);
			InputMembersMapping = soapImporter.ImportMembersMapping(RequestName, RequestNamespace, members, hasWrapperElement, writeAccessors);
			OutputMembersMapping = soapImporter.ImportMembersMapping(ResponseName, ResponseNamespace, members2, hasWrapperElement, writeAccessors);
		}
		InputMembersMapping.SetKey(RequestName);
		OutputMembersMapping.SetKey(ResponseName);
		requestSerializerId = soapTypeStubInfo.RegisterSerializer(InputMembersMapping);
		responseSerializerId = soapTypeStubInfo.RegisterSerializer(OutputMembersMapping);
		object[] customAttributes = source.GetCustomAttributes(typeof(SoapHeaderAttribute));
		ArrayList arrayList = new ArrayList(customAttributes.Length);
		ArrayList arrayList2 = new ArrayList(customAttributes.Length);
		ArrayList arrayList3 = new ArrayList(customAttributes.Length);
		ArrayList arrayList4 = new ArrayList();
		SoapHeaderDirection soapHeaderDirection = (SoapHeaderDirection)0;
		for (int i = 0; i < customAttributes.Length; i++)
		{
			SoapHeaderAttribute soapHeaderAttribute = (SoapHeaderAttribute)customAttributes[i];
			MemberInfo[] member = source.DeclaringType.GetMember(soapHeaderAttribute.MemberName);
			if (member.Length == 0)
			{
				throw new InvalidOperationException("Member " + soapHeaderAttribute.MemberName + " not found in class " + source.DeclaringType.FullName + ".");
			}
			SoapHeaderMapping soapHeaderMapping = new SoapHeaderMapping(member[0], soapHeaderAttribute);
			arrayList.Add(soapHeaderMapping);
			if (!soapHeaderMapping.Custom)
			{
				if ((soapHeaderMapping.Direction & SoapHeaderDirection.In) != 0)
				{
					arrayList2.Add(soapHeaderMapping);
				}
				if ((soapHeaderMapping.Direction & SoapHeaderDirection.Out) != 0)
				{
					arrayList3.Add(soapHeaderMapping);
				}
				if ((soapHeaderMapping.Direction & SoapHeaderDirection.Fault) != 0)
				{
					arrayList4.Add(soapHeaderMapping);
				}
			}
			else
			{
				soapHeaderDirection |= soapHeaderMapping.Direction;
			}
		}
		Headers = (SoapHeaderMapping[])arrayList.ToArray(typeof(SoapHeaderMapping));
		if (arrayList2.Count > 0 || (soapHeaderDirection & SoapHeaderDirection.In) != 0)
		{
			InHeaders = (SoapHeaderMapping[])arrayList2.ToArray(typeof(SoapHeaderMapping));
			XmlReflectionMember[] members3 = BuildHeadersReflectionMembers(InHeaders);
			if (Use == SoapBindingUse.Literal)
			{
				InputHeaderMembersMapping = xmlImporter.ImportMembersMapping("", RequestNamespace, members3, hasWrapperElement: false);
			}
			else
			{
				InputHeaderMembersMapping = soapImporter.ImportMembersMapping("", RequestNamespace, members3, hasWrapperElement: false, writeAccessors: false);
			}
			InputHeaderMembersMapping.SetKey(RequestName + ":InHeaders");
			requestHeadersSerializerId = soapTypeStubInfo.RegisterSerializer(InputHeaderMembersMapping);
		}
		if (arrayList3.Count > 0 || (soapHeaderDirection & SoapHeaderDirection.Out) != 0)
		{
			OutHeaders = (SoapHeaderMapping[])arrayList3.ToArray(typeof(SoapHeaderMapping));
			XmlReflectionMember[] members4 = BuildHeadersReflectionMembers(OutHeaders);
			if (Use == SoapBindingUse.Literal)
			{
				OutputHeaderMembersMapping = xmlImporter.ImportMembersMapping("", RequestNamespace, members4, hasWrapperElement: false);
			}
			else
			{
				OutputHeaderMembersMapping = soapImporter.ImportMembersMapping("", RequestNamespace, members4, hasWrapperElement: false, writeAccessors: false);
			}
			OutputHeaderMembersMapping.SetKey(ResponseName + ":OutHeaders");
			responseHeadersSerializerId = soapTypeStubInfo.RegisterSerializer(OutputHeaderMembersMapping);
		}
		if (arrayList4.Count > 0 || (soapHeaderDirection & SoapHeaderDirection.Fault) != 0)
		{
			FaultHeaders = (SoapHeaderMapping[])arrayList4.ToArray(typeof(SoapHeaderMapping));
			XmlReflectionMember[] members5 = BuildHeadersReflectionMembers(FaultHeaders);
			if (Use == SoapBindingUse.Literal)
			{
				FaultHeaderMembersMapping = xmlImporter.ImportMembersMapping("", RequestNamespace, members5, hasWrapperElement: false);
			}
			else
			{
				FaultHeaderMembersMapping = soapImporter.ImportMembersMapping("", RequestNamespace, members5, hasWrapperElement: false, writeAccessors: false);
			}
			faultHeadersSerializerId = soapTypeStubInfo.RegisterSerializer(FaultHeaderMembersMapping);
		}
		SoapExtensions = SoapExtension.GetMethodExtensions(source);
	}

	private XmlReflectionMember[] BuildRequestReflectionMembers(XmlElementAttribute optional_ns)
	{
		ParameterInfo[] inParameters = MethodInfo.InParameters;
		XmlReflectionMember[] array = new XmlReflectionMember[inParameters.Length];
		for (int i = 0; i < inParameters.Length; i++)
		{
			XmlReflectionMember xmlReflectionMember = new XmlReflectionMember();
			xmlReflectionMember.IsReturnValue = false;
			xmlReflectionMember.MemberName = inParameters[i].Name;
			xmlReflectionMember.MemberType = inParameters[i].ParameterType;
			xmlReflectionMember.XmlAttributes = new XmlAttributes(inParameters[i]);
			xmlReflectionMember.SoapAttributes = new SoapAttributes(inParameters[i]);
			if (xmlReflectionMember.MemberType.IsByRef)
			{
				xmlReflectionMember.MemberType = xmlReflectionMember.MemberType.GetElementType();
			}
			if (optional_ns != null)
			{
				xmlReflectionMember.XmlAttributes.XmlElements.Add(optional_ns);
			}
			array[i] = xmlReflectionMember;
		}
		return array;
	}

	private XmlReflectionMember[] BuildResponseReflectionMembers(XmlElementAttribute optional_ns)
	{
		ParameterInfo[] outParameters = MethodInfo.OutParameters;
		int num;
		int num2;
		if (!OneWay)
		{
			num = ((!(MethodInfo.ReturnType == typeof(void))) ? 1 : 0);
			if (num != 0)
			{
				num2 = 1;
				goto IL_003b;
			}
		}
		else
		{
			num = 0;
		}
		num2 = 0;
		goto IL_003b;
		IL_003b:
		XmlReflectionMember[] array = new XmlReflectionMember[num2 + outParameters.Length];
		int num3 = 0;
		if (num != 0)
		{
			XmlReflectionMember xmlReflectionMember = new XmlReflectionMember();
			xmlReflectionMember.IsReturnValue = true;
			xmlReflectionMember.MemberName = Name + "Result";
			xmlReflectionMember.MemberType = MethodInfo.ReturnType;
			xmlReflectionMember.XmlAttributes = new XmlAttributes(MethodInfo.ReturnTypeCustomAttributeProvider);
			xmlReflectionMember.SoapAttributes = new SoapAttributes(MethodInfo.ReturnTypeCustomAttributeProvider);
			if (optional_ns != null)
			{
				xmlReflectionMember.XmlAttributes.XmlElements.Add(optional_ns);
			}
			num3++;
			array[0] = xmlReflectionMember;
		}
		for (int i = 0; i < outParameters.Length; i++)
		{
			XmlReflectionMember xmlReflectionMember = new XmlReflectionMember();
			xmlReflectionMember.IsReturnValue = false;
			xmlReflectionMember.MemberName = outParameters[i].Name;
			xmlReflectionMember.MemberType = outParameters[i].ParameterType;
			xmlReflectionMember.XmlAttributes = new XmlAttributes(outParameters[i]);
			xmlReflectionMember.SoapAttributes = new SoapAttributes(outParameters[i]);
			if (xmlReflectionMember.MemberType.IsByRef)
			{
				xmlReflectionMember.MemberType = xmlReflectionMember.MemberType.GetElementType();
			}
			if (optional_ns != null)
			{
				xmlReflectionMember.XmlAttributes.XmlElements.Add(optional_ns);
			}
			array[i + num3] = xmlReflectionMember;
		}
		return array;
	}

	private XmlReflectionMember[] BuildHeadersReflectionMembers(SoapHeaderMapping[] headers)
	{
		XmlReflectionMember[] array = new XmlReflectionMember[headers.Length];
		for (int i = 0; i < headers.Length; i++)
		{
			SoapHeaderMapping soapHeaderMapping = headers[i];
			XmlReflectionMember xmlReflectionMember = new XmlReflectionMember();
			xmlReflectionMember.IsReturnValue = false;
			xmlReflectionMember.MemberName = soapHeaderMapping.HeaderType.Name;
			xmlReflectionMember.MemberType = soapHeaderMapping.HeaderType;
			XmlAttributes xmlAttributes = new XmlAttributes(soapHeaderMapping.HeaderType);
			if (xmlAttributes.XmlRoot != null)
			{
				XmlElementAttribute xmlElementAttribute = new XmlElementAttribute();
				xmlElementAttribute.ElementName = xmlAttributes.XmlRoot.ElementName;
				xmlElementAttribute.Namespace = xmlAttributes.XmlRoot.Namespace;
				xmlReflectionMember.XmlAttributes = new XmlAttributes();
				xmlReflectionMember.XmlAttributes.XmlElements.Add(xmlElementAttribute);
			}
			array[i] = xmlReflectionMember;
		}
		return array;
	}

	public SoapHeaderMapping GetHeaderInfo(Type headerType)
	{
		SoapHeaderMapping[] headers = Headers;
		foreach (SoapHeaderMapping soapHeaderMapping in headers)
		{
			if (soapHeaderMapping.HeaderType == headerType)
			{
				return soapHeaderMapping;
			}
		}
		return null;
	}

	public XmlSerializer GetBodySerializer(SoapHeaderDirection dir, bool soap12)
	{
		switch (dir)
		{
		case SoapHeaderDirection.In:
			return RequestSerializer;
		case SoapHeaderDirection.Out:
			return ResponseSerializer;
		case SoapHeaderDirection.Fault:
			if (!soap12)
			{
				return Fault.Serializer;
			}
			return Soap12Fault.Serializer;
		default:
			return null;
		}
	}

	public XmlSerializer GetHeaderSerializer(SoapHeaderDirection dir)
	{
		return dir switch
		{
			SoapHeaderDirection.In => RequestHeadersSerializer, 
			SoapHeaderDirection.Out => ResponseHeadersSerializer, 
			SoapHeaderDirection.Fault => FaultHeadersSerializer, 
			_ => null, 
		};
	}

	private SoapHeaderMapping[] GetHeaders(SoapHeaderDirection dir)
	{
		return dir switch
		{
			SoapHeaderDirection.In => InHeaders, 
			SoapHeaderDirection.Out => OutHeaders, 
			SoapHeaderDirection.Fault => FaultHeaders, 
			_ => null, 
		};
	}

	public object[] GetHeaderValueArray(SoapHeaderDirection dir, SoapHeaderCollection headers)
	{
		SoapHeaderMapping[] headers2 = GetHeaders(dir);
		if (headers2 == null)
		{
			return null;
		}
		object[] array = new object[headers2.Length];
		for (int i = 0; i < headers.Count; i++)
		{
			SoapHeader soapHeader = headers[i];
			Type type = soapHeader.GetType();
			for (int j = 0; j < headers2.Length; j++)
			{
				if (headers2[j].HeaderType == type)
				{
					array[j] = soapHeader;
				}
			}
		}
		return array;
	}
}
