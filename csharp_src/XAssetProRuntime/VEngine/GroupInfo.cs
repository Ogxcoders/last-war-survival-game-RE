using System;

namespace VEngine;

public class GroupInfo : ISerializable
{
	public int[] bundles = Utility.IntArrayEmpty;

	public string name;

	public void Deserialize(string line)
	{
		string[] array = line.Split(new char[1] { ',' });
		name = array[0];
		bundles = array[1].IntArrayValue("|");
	}

	public void Deserialize(ReadOnlySpan<char> line)
	{
		global::StringExtensions.SegmentSplitEnumerator segmentSplitEnumerator = line.SplitSegments(',');
		int num = 0;
		while (segmentSplitEnumerator.MoveNext())
		{
			ReadOnlySpan<char> line2 = segmentSplitEnumerator.Current.Line;
			switch (num)
			{
			case 0:
				name = line2.ToString();
				break;
			case 1:
				if (line2.IsEmpty)
				{
					bundles = Utility.IntArrayEmpty;
				}
				else
				{
					bundles = line2.Split_to_IntArray('|');
				}
				break;
			}
			num++;
		}
	}

	public string Serialize()
	{
		return name + "," + StringExtensions.Join("|", bundles);
	}
}
