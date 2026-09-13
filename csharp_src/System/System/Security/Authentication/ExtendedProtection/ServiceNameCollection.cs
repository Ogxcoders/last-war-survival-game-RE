using System.Collections;
using System.Globalization;

namespace System.Security.Authentication.ExtendedProtection;

[Serializable]
public class ServiceNameCollection : ReadOnlyCollectionBase
{
	public ServiceNameCollection(ICollection items)
	{
		if (items == null)
		{
			throw new ArgumentNullException("items");
		}
		foreach (string item in items)
		{
			AddIfNew(base.InnerList, item);
		}
	}

	public ServiceNameCollection Merge(string serviceName)
	{
		ArrayList arrayList = new ArrayList();
		arrayList.AddRange(base.InnerList);
		AddIfNew(arrayList, serviceName);
		return new ServiceNameCollection(arrayList);
	}

	public ServiceNameCollection Merge(IEnumerable serviceNames)
	{
		ArrayList arrayList = new ArrayList();
		arrayList.AddRange(base.InnerList);
		foreach (object serviceName in serviceNames)
		{
			AddIfNew(arrayList, serviceName as string);
		}
		return new ServiceNameCollection(arrayList);
	}

	private static void AddIfNew(ArrayList newServiceNames, string serviceName)
	{
		if (string.IsNullOrEmpty(serviceName))
		{
			throw new ArgumentException(global::SR.GetString("A service name must not be null or empty."));
		}
		serviceName = NormalizeServiceName(serviceName);
		if (!Contains(serviceName, newServiceNames))
		{
			newServiceNames.Add(serviceName);
		}
	}

	internal static bool Contains(string searchServiceName, ICollection serviceNames)
	{
		foreach (string serviceName in serviceNames)
		{
			if (Match(serviceName, searchServiceName))
			{
				return true;
			}
		}
		return false;
	}

	public bool Contains(string searchServiceName)
	{
		return Contains(NormalizeServiceName(searchServiceName), base.InnerList);
	}

	internal static string NormalizeServiceName(string inputServiceName)
	{
		if (string.IsNullOrWhiteSpace(inputServiceName))
		{
			return inputServiceName;
		}
		int num = inputServiceName.IndexOf('/');
		if (num < 0)
		{
			return inputServiceName;
		}
		string text = inputServiceName.Substring(0, num + 1);
		string text2 = inputServiceName.Substring(num + 1);
		if (string.IsNullOrWhiteSpace(text2))
		{
			return inputServiceName;
		}
		string text3 = text2;
		string text4 = string.Empty;
		string text5 = string.Empty;
		UriHostNameType uriHostNameType = Uri.CheckHostName(text2);
		if (uriHostNameType == UriHostNameType.Unknown)
		{
			string text6 = text2;
			int num2 = text2.IndexOf('/');
			if (num2 >= 0)
			{
				text6 = text2.Substring(0, num2);
				text5 = text2.Substring(num2);
				text3 = text6;
			}
			int num3 = text6.LastIndexOf(':');
			if (num3 >= 0)
			{
				text3 = text6.Substring(0, num3);
				text4 = text6.Substring(num3 + 1);
				if (!ushort.TryParse(text4, NumberStyles.Integer, CultureInfo.InvariantCulture, out var _))
				{
					return inputServiceName;
				}
				text4 = text6.Substring(num3);
			}
			uriHostNameType = Uri.CheckHostName(text3);
		}
		if (uriHostNameType != UriHostNameType.Dns)
		{
			return inputServiceName;
		}
		if (!Uri.TryCreate(Uri.UriSchemeHttp + Uri.SchemeDelimiter + text3, UriKind.Absolute, out var result2))
		{
			return inputServiceName;
		}
		string components = result2.GetComponents(UriComponents.NormalizedHost, UriFormat.SafeUnescaped);
		string text7 = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", text, components, text4, text5);
		if (Match(inputServiceName, text7))
		{
			return inputServiceName;
		}
		return text7;
	}

	internal static bool Match(string serviceName1, string serviceName2)
	{
		return string.Compare(serviceName1, serviceName2, StringComparison.OrdinalIgnoreCase) == 0;
	}
}
