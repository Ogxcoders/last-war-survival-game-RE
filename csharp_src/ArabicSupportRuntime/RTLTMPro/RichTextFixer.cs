namespace RTLTMPro;

public static class RichTextFixer
{
	public enum TagType
	{
		None,
		Opening,
		Closing,
		SelfContained
	}

	public struct Tag
	{
		public int Start;

		public int End;

		public int HashCode;

		public TagType Type;

		public Tag(int start, int end, TagType type, int hashCode)
		{
			Type = type;
			Start = start;
			End = end;
			HashCode = hashCode;
		}
	}

	public static void Fix(FastStringBuilder text)
	{
		int num = 0;
		while (num < text.Length)
		{
			FindTag(text, num, out var tag);
			if (tag.Type != TagType.None)
			{
				text.Reverse(tag.Start, tag.End - tag.Start + 1);
				num = tag.End;
				num++;
				continue;
			}
			break;
		}
	}

	public static void FindTag(FastStringBuilder str, int start, out Tag tag)
	{
		int num = start;
		while (num < str.Length)
		{
			if (str.Get(num) != 60)
			{
				num++;
				continue;
			}
			bool flag = true;
			tag.HashCode = 0;
			for (int i = num + 1; i < str.Length; i++)
			{
				int num2 = str.Get(i);
				if (flag)
				{
					if (Char32Utils.IsLetter(num2))
					{
						if (tag.HashCode == 0)
						{
							tag.HashCode = num2.GetHashCode();
						}
						else
						{
							tag.HashCode = (tag.HashCode * 397) ^ num2.GetHashCode();
						}
					}
					else if (tag.HashCode != 0)
					{
						flag = false;
					}
				}
				if (i == num + 1 && num2 == 32)
				{
					break;
				}
				switch (num2)
				{
				case 62:
					tag.Start = num;
					tag.End = i;
					if (str.Get(i - 1) == 47)
					{
						tag.Type = TagType.SelfContained;
					}
					else if (str.Get(num + 1) == 47)
					{
						tag.Type = TagType.Closing;
					}
					else
					{
						tag.Type = TagType.Opening;
					}
					return;
				default:
					continue;
				case 60:
					break;
				}
				break;
			}
			num++;
		}
		tag.Start = 0;
		tag.End = 0;
		tag.Type = TagType.None;
		tag.HashCode = 0;
	}
}
