using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace System.ComponentModel.Design;

public class DesigntimeLicenseContextSerializer
{
	private DesigntimeLicenseContextSerializer()
	{
	}

	public static void Serialize(Stream o, string cryptoKey, DesigntimeLicenseContext context)
	{
		((IFormatter)new BinaryFormatter()).Serialize(o, (object)new object[2] { cryptoKey, context.savedLicenseKeys });
	}

	internal static void Deserialize(Stream o, string cryptoKey, RuntimeLicenseContext context)
	{
		object obj = ((IFormatter)new BinaryFormatter()).Deserialize(o);
		if (obj is object[])
		{
			object[] array = (object[])obj;
			if (array[0] is string && (string)array[0] == cryptoKey)
			{
				context.savedLicenseKeys = (Hashtable)array[1];
			}
		}
	}
}
