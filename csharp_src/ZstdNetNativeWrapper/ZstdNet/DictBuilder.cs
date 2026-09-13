using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZstdNet;

public static class DictBuilder
{
	public const int DefaultDictCapacity = 112640;

	public static byte[] TrainFromBuffer(IEnumerable<byte[]> samples, int dictCapacity = 112640)
	{
		MemoryStream ms = new MemoryStream();
		UIntPtr[] array = samples.Select(delegate(byte[] sample)
		{
			ms.Write(sample, 0, sample.Length);
			return (UIntPtr)(ulong)sample.Length;
		}).ToArray();
		byte[] array2 = new byte[dictCapacity];
		int num = (int)(uint)ExternMethods.ZDICT_trainFromBuffer(array2, (UIntPtr)(ulong)dictCapacity, ms.GetBuffer(), array, (uint)array.Length).EnsureZdictSuccess();
		if (dictCapacity != num)
		{
			Array.Resize(ref array2, num);
		}
		return array2;
	}
}
