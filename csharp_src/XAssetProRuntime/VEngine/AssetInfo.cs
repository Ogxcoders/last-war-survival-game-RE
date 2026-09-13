using System;

namespace VEngine;

public class AssetInfo : ISerializable
{
	public int bundle;

	public int[] bundles = Utility.IntArrayEmpty;

	public int id;

	public void Deserialize(string line)
	{
		string[] array = line.Split(new char[1] { ',' });
		id = array[0].IntValue();
		bundle = array[1].IntValue();
		bundles = array[2].IntArrayValue("|");
	}

	public void Deserialize(ReadOnlySpan<char> line)
	{
		line.Split_to_spans(',', out var span, out var span2, out var span3);
		id = span.ToInt();
		bundle = span2.ToInt();
		if (span3.IsEmpty)
		{
			bundles = Utility.IntArrayEmpty;
		}
		else
		{
			bundles = span3.Split_to_IntArray('|');
		}
	}

	public string Serialize()
	{
		return $"{id},{bundle},";
	}
}
