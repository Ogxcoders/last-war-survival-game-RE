using System;

namespace VEngine;

public class BundleInfo : ISerializable
{
	public int[] assets = Utility.IntArrayEmpty;

	public uint crc;

	public int[] deps = Utility.IntArrayEmpty;

	public int id;

	public string name;

	public ulong size;

	public int[] downloadModes = Utility.IntArrayEmpty;

	private string _alias;

	public bool isSplitBundle;

	public bool isUnusedAssetsPackage;

	public static bool UseBundleAlias;

	public bool inPlayerAssets;

	public bool isDownloaded;

	public string alias
	{
		get
		{
			return _alias;
		}
		set
		{
			_alias = value;
		}
	}

	public void Deserialize(string line)
	{
		string[] array = line.Split(new char[1] { ',' });
		id = array[0].IntValue();
		name = array[1];
		crc = array[2].UIntValue();
		size = array[3].ULongValue();
		assets = array[4].IntArrayValue("|");
		deps = array[5].IntArrayValue("|");
	}

	public void Deserialize(ReadOnlySpan<char> line)
	{
		global::StringExtensions.SegmentSplitEnumerator segmentSplitEnumerator = line.SplitSegments(',');
		int num = 0;
		bool flag = false;
		while (segmentSplitEnumerator.MoveNext())
		{
			ReadOnlySpan<char> line2 = segmentSplitEnumerator.Current.Line;
			switch (num)
			{
			case 0:
				id = line2.ToInt();
				break;
			case 1:
				name = line2.ToString();
				break;
			case 2:
				crc = (uint)line2.ToULong();
				break;
			case 3:
				size = line2.ToULong();
				break;
			case 4:
				if (line2.IsEmpty)
				{
					assets = Utility.IntArrayEmpty;
				}
				else
				{
					assets = line2.Split_to_IntArray('|');
				}
				break;
			case 5:
				if (line2.IsEmpty)
				{
					deps = Utility.IntArrayEmpty;
				}
				else
				{
					deps = line2.Split_to_IntArray('|');
				}
				break;
			case 6:
			{
				if (line2.IsEmpty)
				{
					downloadModes = new int[1];
				}
				else
				{
					downloadModes = line2.Split_to_IntArray('|');
				}
				isSplitBundle = true;
				int i = 0;
				for (int num2 = downloadModes.Length; i < num2; i++)
				{
					if (downloadModes[i] == 0)
					{
						isSplitBundle = false;
					}
					if (downloadModes[i] == 899999 && downloadModes.Length == 1)
					{
						isUnusedAssetsPackage = true;
					}
				}
				break;
			}
			case 7:
				_alias = line2.ToString();
				flag = true;
				break;
			}
			num++;
		}
		if (!flag)
		{
			_alias = name;
		}
		Versions.SyncInPlayerAssets(this);
	}

	public string Serialize()
	{
		return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", id, name, crc, size, StringExtensions.Join("|", assets), StringExtensions.Join("|", deps), StringExtensions.Join("|", downloadModes), _alias);
	}
}
