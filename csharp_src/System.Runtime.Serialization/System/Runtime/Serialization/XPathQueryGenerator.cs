using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization;

public static class XPathQueryGenerator
{
	private class ExportContext
	{
		private XmlNamespaceManager namespaces;

		private int nextPrefix;

		private StringBuilder xPathBuilder;

		public XmlNamespaceManager Namespaces => namespaces;

		public string XPath => xPathBuilder.ToString();

		public ExportContext(DataContract rootContract)
		{
			namespaces = new XmlNamespaceManager(new NameTable());
			string text = SetNamespace(rootContract.TopLevelElementNamespace.Value);
			xPathBuilder = new StringBuilder("/" + text + ":" + rootContract.TopLevelElementName.Value);
		}

		public ExportContext(StringBuilder rootContractXPath)
		{
			namespaces = new XmlNamespaceManager(new NameTable());
			xPathBuilder = rootContractXPath;
		}

		public void WriteChildToContext(DataMember contextMember, string prefix)
		{
			xPathBuilder.Append("/" + prefix + ":" + contextMember.Name);
		}

		public string SetNamespace(string ns)
		{
			string text = namespaces.LookupPrefix(ns);
			if (text == null || text.Length == 0)
			{
				text = "xg" + nextPrefix++.ToString(NumberFormatInfo.InvariantInfo);
				Namespaces.AddNamespace(text, ns);
			}
			return text;
		}
	}

	private const string XPathSeparator = "/";

	private const string NsSeparator = ":";

	public static string CreateFromDataContractSerializer(Type type, MemberInfo[] pathToMember, out XmlNamespaceManager namespaces)
	{
		return CreateFromDataContractSerializer(type, pathToMember, null, out namespaces);
	}

	public static string CreateFromDataContractSerializer(Type type, MemberInfo[] pathToMember, StringBuilder rootElementXpath, out XmlNamespaceManager namespaces)
	{
		if (type == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("type"));
		}
		if (pathToMember == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("pathToMember"));
		}
		DataContract dataContract = DataContract.GetDataContract(type);
		ExportContext exportContext = ((rootElementXpath != null) ? new ExportContext(rootElementXpath) : new ExportContext(dataContract));
		for (int i = 0; i < pathToMember.Length; i++)
		{
			dataContract = ProcessDataContract(dataContract, exportContext, pathToMember[i]);
		}
		namespaces = exportContext.Namespaces;
		return exportContext.XPath;
	}

	private static DataContract ProcessDataContract(DataContract contract, ExportContext context, MemberInfo memberNode)
	{
		if (contract is ClassDataContract)
		{
			return ProcessClassDataContract((ClassDataContract)contract, context, memberNode);
		}
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("The path to member was not found for XPath query generator.")));
	}

	private static DataContract ProcessClassDataContract(ClassDataContract contract, ExportContext context, MemberInfo memberNode)
	{
		string prefix = context.SetNamespace(contract.Namespace.Value);
		foreach (DataMember dataMember in GetDataMembers(contract))
		{
			if (dataMember.MemberInfo.Name == memberNode.Name && dataMember.MemberInfo.DeclaringType.IsAssignableFrom(memberNode.DeclaringType))
			{
				context.WriteChildToContext(dataMember, prefix);
				return dataMember.MemberTypeContract;
			}
		}
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("The path to member was not found for XPath query generator.")));
	}

	private static IEnumerable<DataMember> GetDataMembers(ClassDataContract contract)
	{
		if (contract.BaseContract != null)
		{
			foreach (DataMember dataMember in GetDataMembers(contract.BaseContract))
			{
				yield return dataMember;
			}
		}
		if (contract.Members == null)
		{
			yield break;
		}
		foreach (DataMember member in contract.Members)
		{
			yield return member;
		}
	}
}
