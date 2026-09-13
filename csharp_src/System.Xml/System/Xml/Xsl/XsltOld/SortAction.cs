using System.Globalization;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class SortAction : CompiledAction
{
	private int selectKey = -1;

	private Avt langAvt;

	private Avt dataTypeAvt;

	private Avt orderAvt;

	private Avt caseOrderAvt;

	private string lang;

	private XmlDataType dataType = XmlDataType.Text;

	private XmlSortOrder order = XmlSortOrder.Ascending;

	private XmlCaseOrder caseOrder;

	private Sort sort;

	private bool forwardCompatibility;

	private InputScopeManager manager;

	private string ParseLang(string value)
	{
		if (value == null)
		{
			return null;
		}
		if (!XmlComplianceUtil.IsValidLanguageID(value.ToCharArray(), 0, value.Length) && (value.Length == 0 || CultureInfo.GetCultureInfo(value) == null))
		{
			if (forwardCompatibility)
			{
				return null;
			}
			throw XsltException.Create("'{1}' is an invalid value for the '{0}' attribute.", "lang", value);
		}
		return value;
	}

	private XmlDataType ParseDataType(string value, InputScopeManager manager)
	{
		if (value == null)
		{
			return XmlDataType.Text;
		}
		if (value == "text")
		{
			return XmlDataType.Text;
		}
		if (value == "number")
		{
			return XmlDataType.Number;
		}
		PrefixQName.ParseQualifiedName(value, out var prefix, out var _);
		manager.ResolveXmlNamespace(prefix);
		if (prefix.Length == 0 && !forwardCompatibility)
		{
			throw XsltException.Create("'{1}' is an invalid value for the '{0}' attribute.", "data-type", value);
		}
		return XmlDataType.Text;
	}

	private XmlSortOrder ParseOrder(string value)
	{
		if (value == null)
		{
			return XmlSortOrder.Ascending;
		}
		if (value == "ascending")
		{
			return XmlSortOrder.Ascending;
		}
		if (value == "descending")
		{
			return XmlSortOrder.Descending;
		}
		if (forwardCompatibility)
		{
			return XmlSortOrder.Ascending;
		}
		throw XsltException.Create("'{1}' is an invalid value for the '{0}' attribute.", "order", value);
	}

	private XmlCaseOrder ParseCaseOrder(string value)
	{
		if (value == null)
		{
			return XmlCaseOrder.None;
		}
		if (value == "upper-first")
		{
			return XmlCaseOrder.UpperFirst;
		}
		if (value == "lower-first")
		{
			return XmlCaseOrder.LowerFirst;
		}
		if (forwardCompatibility)
		{
			return XmlCaseOrder.None;
		}
		throw XsltException.Create("'{1}' is an invalid value for the '{0}' attribute.", "case-order", value);
	}

	internal override void Compile(Compiler compiler)
	{
		CompileAttributes(compiler);
		CheckEmpty(compiler);
		if (selectKey == -1)
		{
			selectKey = compiler.AddQuery(".");
		}
		forwardCompatibility = compiler.ForwardCompatibility;
		manager = compiler.CloneScopeManager();
		lang = ParseLang(CompiledAction.PrecalculateAvt(ref langAvt));
		dataType = ParseDataType(CompiledAction.PrecalculateAvt(ref dataTypeAvt), manager);
		order = ParseOrder(CompiledAction.PrecalculateAvt(ref orderAvt));
		caseOrder = ParseCaseOrder(CompiledAction.PrecalculateAvt(ref caseOrderAvt));
		if (langAvt == null && dataTypeAvt == null && orderAvt == null && caseOrderAvt == null)
		{
			sort = new Sort(selectKey, lang, dataType, order, caseOrder);
		}
	}

	internal override bool CompileAttribute(Compiler compiler)
	{
		string localName = compiler.Input.LocalName;
		string value = compiler.Input.Value;
		if (Ref.Equal(localName, compiler.Atoms.Select))
		{
			selectKey = compiler.AddQuery(value);
		}
		else if (Ref.Equal(localName, compiler.Atoms.Lang))
		{
			langAvt = Avt.CompileAvt(compiler, value);
		}
		else if (Ref.Equal(localName, compiler.Atoms.DataType))
		{
			dataTypeAvt = Avt.CompileAvt(compiler, value);
		}
		else if (Ref.Equal(localName, compiler.Atoms.Order))
		{
			orderAvt = Avt.CompileAvt(compiler, value);
		}
		else
		{
			if (!Ref.Equal(localName, compiler.Atoms.CaseOrder))
			{
				return false;
			}
			caseOrderAvt = Avt.CompileAvt(compiler, value);
		}
		return true;
	}

	internal override void Execute(Processor processor, ActionFrame frame)
	{
		processor.AddSort((sort != null) ? sort : new Sort(selectKey, (langAvt == null) ? lang : ParseLang(langAvt.Evaluate(processor, frame)), (dataTypeAvt == null) ? dataType : ParseDataType(dataTypeAvt.Evaluate(processor, frame), manager), (orderAvt == null) ? order : ParseOrder(orderAvt.Evaluate(processor, frame)), (caseOrderAvt == null) ? caseOrder : ParseCaseOrder(caseOrderAvt.Evaluate(processor, frame))));
		frame.Finished();
	}
}
