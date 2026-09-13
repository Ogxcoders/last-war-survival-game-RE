using System;
using UnityEngine;
using XLua;

public class OasisViewScale
{
	public int id;

	public int order;

	public Color innerColor;

	public Color outlineColor;

	public Color baseColor;

	private Tuple<int, int, int> _range;

	public OasisViewScale(LuaTable table)
	{
		id = table.Get<int>("id");
		order = table.Get<int>("order");
		innerColor = table.Get<Color>("innerColor");
		outlineColor = table.Get<Color>("outlineColor");
		baseColor = table.Get<Color>("baseColor");
		string[] array = table.Get<string>("range").Split(new char[1] { ';' });
		if (array.Length <= 1)
		{
			return;
		}
		int item = 3;
		if (int.TryParse(array[0], out var result))
		{
			item = result;
		}
		string[] array2 = array[1].Split(new char[1] { ',' });
		if (array2.Length >= 2 && float.TryParse(array2[0], out var result2) && float.TryParse(array2[1], out var result3))
		{
			if (result3 < result2)
			{
				float num = result2;
				result2 = result3;
				result3 = num;
			}
			_range = new Tuple<int, int, int>((int)(result2 * 10f), (int)(result3 * 10f), item);
		}
	}

	public bool InRange(int rate)
	{
		if (_range == null)
		{
			return false;
		}
		switch (_range.Item3)
		{
		case 1:
			if (rate > _range.Item1)
			{
				return rate < _range.Item2;
			}
			return false;
		case 2:
			if (rate > _range.Item1)
			{
				return rate <= _range.Item2;
			}
			return false;
		case 3:
			if (rate >= _range.Item1)
			{
				return rate < _range.Item2;
			}
			return false;
		case 4:
			if (rate >= _range.Item1)
			{
				return rate <= _range.Item2;
			}
			return false;
		default:
			return false;
		}
	}
}
