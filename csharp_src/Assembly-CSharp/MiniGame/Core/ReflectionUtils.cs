using System.Text;

namespace MiniGame.Core;

internal static class ReflectionUtils
{
	public static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		bool flag2 = false;
		foreach (char c in fullyQualifiedTypeName)
		{
			switch (c)
			{
			case ',':
				if (!flag)
				{
					flag = true;
					stringBuilder.Append(c);
				}
				else
				{
					flag2 = true;
				}
				break;
			case '[':
			case ']':
				flag = false;
				flag2 = false;
				stringBuilder.Append(c);
				break;
			default:
				if (!flag2)
				{
					stringBuilder.Append(c);
				}
				break;
			}
		}
		return stringBuilder.ToString();
	}

	public static StructMultiKey<string, string> SplitFullyQualifiedTypeName(string fullyQualifiedTypeName)
	{
		int? assemblyDelimiterIndex = GetAssemblyDelimiterIndex(fullyQualifiedTypeName);
		string v;
		string v2;
		if (assemblyDelimiterIndex.HasValue)
		{
			v = fullyQualifiedTypeName.Trim(0, assemblyDelimiterIndex.GetValueOrDefault());
			v2 = fullyQualifiedTypeName.Trim(assemblyDelimiterIndex.GetValueOrDefault() + 1, fullyQualifiedTypeName.Length - assemblyDelimiterIndex.GetValueOrDefault() - 1);
		}
		else
		{
			v = fullyQualifiedTypeName;
			v2 = null;
		}
		return new StructMultiKey<string, string>(v2, v);
	}

	private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
	{
		int num = 0;
		for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
		{
			switch (fullyQualifiedTypeName[i])
			{
			case '[':
				num++;
				break;
			case ']':
				num--;
				break;
			case ',':
				if (num == 0)
				{
					return i;
				}
				break;
			}
		}
		return null;
	}
}
