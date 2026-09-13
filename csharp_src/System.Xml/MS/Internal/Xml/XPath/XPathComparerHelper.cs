using System;
using System.Collections;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath;

internal sealed class XPathComparerHelper : IComparer
{
	private XmlSortOrder _order;

	private XmlCaseOrder _caseOrder;

	private CultureInfo _cinfo;

	private XmlDataType _dataType;

	public XPathComparerHelper(XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType)
	{
		if (lang == null)
		{
			_cinfo = CultureInfo.CurrentCulture;
		}
		else
		{
			try
			{
				_cinfo = new CultureInfo(lang);
			}
			catch (ArgumentException)
			{
				throw;
			}
		}
		if (order == XmlSortOrder.Descending)
		{
			switch (caseOrder)
			{
			case XmlCaseOrder.LowerFirst:
				caseOrder = XmlCaseOrder.UpperFirst;
				break;
			case XmlCaseOrder.UpperFirst:
				caseOrder = XmlCaseOrder.LowerFirst;
				break;
			}
		}
		_order = order;
		_caseOrder = caseOrder;
		_dataType = dataType;
	}

	public int Compare(object x, object y)
	{
		switch (_dataType)
		{
		case XmlDataType.Text:
		{
			string @string = Convert.ToString(x, _cinfo);
			string string2 = Convert.ToString(y, _cinfo);
			int num2 = _cinfo.CompareInfo.Compare(@string, string2, (_caseOrder != XmlCaseOrder.None) ? CompareOptions.IgnoreCase : CompareOptions.None);
			if (num2 != 0 || _caseOrder == XmlCaseOrder.None)
			{
				if (_order != XmlSortOrder.Ascending)
				{
					return -num2;
				}
				return num2;
			}
			num2 = _cinfo.CompareInfo.Compare(@string, string2);
			if (_caseOrder != XmlCaseOrder.LowerFirst)
			{
				return -num2;
			}
			return num2;
		}
		case XmlDataType.Number:
		{
			double num = XmlConvert.ToXPathDouble(x);
			double value = XmlConvert.ToXPathDouble(y);
			int num2 = num.CompareTo(value);
			if (_order != XmlSortOrder.Ascending)
			{
				return -num2;
			}
			return num2;
		}
		default:
			throw new InvalidOperationException("Operation is not valid due to the current state of the object.");
		}
	}
}
