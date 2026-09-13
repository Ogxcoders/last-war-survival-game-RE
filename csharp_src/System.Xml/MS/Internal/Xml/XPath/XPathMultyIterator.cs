using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath;

internal class XPathMultyIterator : ResetableIterator
{
	protected ResetableIterator[] arr;

	protected int firstNotEmpty;

	protected int position;

	public override XPathNavigator Current => arr[firstNotEmpty].Current;

	public override int CurrentPosition => position;

	public XPathMultyIterator(ArrayList inputArray)
	{
		arr = new ResetableIterator[inputArray.Count];
		for (int i = 0; i < arr.Length; i++)
		{
			arr[i] = new XPathArrayIterator((ArrayList)inputArray[i]);
		}
		Init();
	}

	private void Init()
	{
		for (int i = 0; i < arr.Length; i++)
		{
			Advance(i);
		}
		int num = arr.Length - 2;
		while (firstNotEmpty <= num)
		{
			if (SiftItem(num))
			{
				num--;
			}
		}
	}

	private bool Advance(int pos)
	{
		if (!arr[pos].MoveNext())
		{
			if (firstNotEmpty != pos)
			{
				ResetableIterator resetableIterator = arr[pos];
				Array.Copy(arr, firstNotEmpty, arr, firstNotEmpty + 1, pos - firstNotEmpty);
				arr[firstNotEmpty] = resetableIterator;
			}
			firstNotEmpty++;
			return false;
		}
		return true;
	}

	private bool SiftItem(int item)
	{
		ResetableIterator resetableIterator = arr[item];
		while (item + 1 < arr.Length)
		{
			switch (Query.CompareNodes(resetableIterator.Current, arr[item + 1].Current))
			{
			case XmlNodeOrder.After:
				arr[item] = arr[item + 1];
				item++;
				continue;
			default:
				arr[item] = resetableIterator;
				if (!Advance(item))
				{
					return false;
				}
				resetableIterator = arr[item];
				continue;
			case XmlNodeOrder.Before:
				break;
			}
			break;
		}
		arr[item] = resetableIterator;
		return true;
	}

	public override void Reset()
	{
		firstNotEmpty = 0;
		position = 0;
		for (int i = 0; i < arr.Length; i++)
		{
			arr[i].Reset();
		}
		Init();
	}

	public XPathMultyIterator(XPathMultyIterator it)
	{
		arr = (ResetableIterator[])it.arr.Clone();
		firstNotEmpty = it.firstNotEmpty;
		position = it.position;
	}

	public override XPathNodeIterator Clone()
	{
		return new XPathMultyIterator(this);
	}

	public override bool MoveNext()
	{
		if (firstNotEmpty >= arr.Length)
		{
			return false;
		}
		if (position != 0)
		{
			if (Advance(firstNotEmpty))
			{
				SiftItem(firstNotEmpty);
			}
			if (firstNotEmpty >= arr.Length)
			{
				return false;
			}
		}
		position++;
		return true;
	}
}
