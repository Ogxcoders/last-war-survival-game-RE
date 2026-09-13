using System.Text;

namespace System.IO;

internal static class PathInternal
{
	internal const char DirectorySeparatorChar = '/';

	internal const char AltDirectorySeparatorChar = '/';

	internal const char VolumeSeparatorChar = '/';

	internal const char PathSeparator = ':';

	internal const string DirectorySeparatorCharAsString = "/";

	private const char InvalidPathChar = '\0';

	internal const string ParentDirectoryPrefix = "../";

	private static readonly bool s_isCaseSensitive = GetIsCaseSensitive();

	internal static StringComparison StringComparison
	{
		get
		{
			if (!s_isCaseSensitive)
			{
				return StringComparison.OrdinalIgnoreCase;
			}
			return StringComparison.Ordinal;
		}
	}

	internal static bool IsCaseSensitive => s_isCaseSensitive;

	internal static int GetRootLength(ReadOnlySpan<char> path)
	{
		if (path.Length <= 0 || !IsDirectorySeparator(path[0]))
		{
			return 0;
		}
		return 1;
	}

	internal static bool IsDirectorySeparator(char c)
	{
		return c == '/';
	}

	internal static string NormalizeDirectorySeparators(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return path;
		}
		bool flag = true;
		for (int i = 0; i < path.Length; i++)
		{
			if (IsDirectorySeparator(path[i]) && i + 1 < path.Length && IsDirectorySeparator(path[i + 1]))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			return path;
		}
		StringBuilder stringBuilder = new StringBuilder(path.Length);
		for (int j = 0; j < path.Length; j++)
		{
			char c = path[j];
			if (!IsDirectorySeparator(c) || j + 1 >= path.Length || !IsDirectorySeparator(path[j + 1]))
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	internal static bool IsPartiallyQualified(ReadOnlySpan<char> path)
	{
		return !Path.IsPathRooted(path);
	}

	internal static bool IsEffectivelyEmpty(string path)
	{
		return string.IsNullOrEmpty(path);
	}

	internal static bool IsEffectivelyEmpty(ReadOnlySpan<char> path)
	{
		return path.IsEmpty;
	}

	internal static bool EndsInDirectorySeparator(ReadOnlySpan<char> path)
	{
		if (path.Length > 0)
		{
			return IsDirectorySeparator(path[path.Length - 1]);
		}
		return false;
	}

	internal static bool StartsWithDirectorySeparator(ReadOnlySpan<char> path)
	{
		if (path.Length > 0)
		{
			return IsDirectorySeparator(path[0]);
		}
		return false;
	}

	internal static string EnsureTrailingSeparator(string path)
	{
		if (!EndsInDirectorySeparator(path))
		{
			return path + "/";
		}
		return path;
	}

	internal static string TrimEndingDirectorySeparator(string path)
	{
		if (!EndsInDirectorySeparator(path) || IsRoot(path))
		{
			return path;
		}
		return path.Substring(0, path.Length - 1);
	}

	internal static ReadOnlySpan<char> TrimEndingDirectorySeparator(ReadOnlySpan<char> path)
	{
		if (!EndsInDirectorySeparator(path) || IsRoot(path))
		{
			return path;
		}
		return path.Slice(0, path.Length - 1);
	}

	internal static bool IsRoot(ReadOnlySpan<char> path)
	{
		return path.Length == GetRootLength(path);
	}

	internal static int GetCommonPathLength(string first, string second, bool ignoreCase)
	{
		int num = EqualStartingCharacterCount(first, second, ignoreCase);
		if (num == 0)
		{
			return num;
		}
		if (num == first.Length && (num == second.Length || IsDirectorySeparator(second[num])))
		{
			return num;
		}
		if (num == second.Length && IsDirectorySeparator(first[num]))
		{
			return num;
		}
		while (num > 0 && !IsDirectorySeparator(first[num - 1]))
		{
			num--;
		}
		return num;
	}

	internal unsafe static int EqualStartingCharacterCount(string first, string second, bool ignoreCase)
	{
		if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
		{
			return 0;
		}
		int num = 0;
		fixed (char* ptr = first)
		{
			fixed (char* ptr2 = second)
			{
				char* ptr3 = ptr;
				char* ptr4 = ptr2;
				char* ptr5 = ptr3 + first.Length;
				char* ptr6 = ptr4 + second.Length;
				while (ptr3 != ptr5 && ptr4 != ptr6 && (*ptr3 == *ptr4 || (ignoreCase && char.ToUpperInvariant(*ptr3) == char.ToUpperInvariant(*ptr4))))
				{
					num++;
					ptr3++;
					ptr4++;
				}
			}
		}
		return num;
	}

	internal static bool AreRootsEqual(string first, string second, StringComparison comparisonType)
	{
		int rootLength = GetRootLength(first);
		int rootLength2 = GetRootLength(second);
		if (rootLength == rootLength2)
		{
			return string.Compare(first, 0, second, 0, rootLength, comparisonType) == 0;
		}
		return false;
	}

	internal static string RemoveRelativeSegments(string path, int rootLength)
	{
		bool flag = false;
		int num = rootLength;
		if (IsDirectorySeparator(path[num - 1]))
		{
			num--;
		}
		Span<char> initialBuffer = stackalloc char[260];
		ValueStringBuilder valueStringBuilder = new ValueStringBuilder(initialBuffer);
		if (num > 0)
		{
			valueStringBuilder.Append(path.AsSpan(0, num));
		}
		for (int i = num; i < path.Length; i++)
		{
			char c = path[i];
			if (IsDirectorySeparator(c) && i + 1 < path.Length)
			{
				if (IsDirectorySeparator(path[i + 1]))
				{
					continue;
				}
				if ((i + 2 == path.Length || IsDirectorySeparator(path[i + 2])) && path[i + 1] == '.')
				{
					i++;
					continue;
				}
				if (i + 2 < path.Length && (i + 3 == path.Length || IsDirectorySeparator(path[i + 3])) && path[i + 1] == '.' && path[i + 2] == '.')
				{
					int num2;
					for (num2 = valueStringBuilder.Length - 1; num2 >= num; num2--)
					{
						if (IsDirectorySeparator(valueStringBuilder[num2]))
						{
							valueStringBuilder.Length = ((i + 3 >= path.Length && num2 == num) ? (num2 + 1) : num2);
							break;
						}
					}
					if (num2 < num)
					{
						valueStringBuilder.Length = num;
					}
					i += 2;
					continue;
				}
			}
			if (c != '/' && c == '/')
			{
				c = '/';
				flag = true;
			}
			valueStringBuilder.Append(c);
		}
		if (!flag && valueStringBuilder.Length == path.Length)
		{
			valueStringBuilder.Dispose();
			return path;
		}
		if (valueStringBuilder.Length >= rootLength)
		{
			return valueStringBuilder.ToString();
		}
		return path.Substring(0, rootLength);
	}

	private static bool GetIsCaseSensitive()
	{
		try
		{
			string text = Path.Combine(Path.GetTempPath(), "CASESENSITIVETEST" + Guid.NewGuid().ToString("N"));
			using (new FileStream(text, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose))
			{
				return !File.Exists(text.ToLowerInvariant());
			}
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static bool IsPartiallyQualified(string path)
	{
		return false;
	}

	public static bool HasIllegalCharacters(string path, bool checkAdditional)
	{
		return path.IndexOfAny(Path.InvalidPathChars) != -1;
	}
}
