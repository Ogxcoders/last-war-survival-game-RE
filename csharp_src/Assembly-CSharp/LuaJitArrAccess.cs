using System;
using UnityEngine;
using XLua;

[LuaCallCSharp(GenFlag.No)]
public class LuaJitArrAccess : LuaArrAccess
{
	private unsafe LuaJitGCtab32* TableRawPtr;

	public unsafe override string ToString()
	{
		return "LuaJitTablePin " + (IntPtr)TableRawPtr;
	}

	public unsafe override void OnPin(IntPtr TablePtr)
	{
		TableRawPtr = (LuaJitGCtab32*)(void*)TablePtr;
	}

	public unsafe override void OnGC()
	{
		TableRawPtr = null;
	}

	private bool IsArchCorrect()
	{
		if (GetInt(1) == 32167 && !double.IsNaN(GetDouble(2)) && GetInt(2) == 9527 && Math.Abs(GetDouble(2) - 9527.5) < 1E-05 && GetInt(3) == -2000000)
		{
			return true;
		}
		return false;
	}

	private void LogDebug(string msg)
	{
		Debug.Log(msg);
	}

	public override void AutoDetectArch()
	{
		if (IsArchCorrect())
		{
			LogDebug("luajit with default arch");
			return;
		}
		if (!LuaJitType.GC64)
		{
			LuaJitType.LJ_TISNUM = 4294901759u;
			if (IsArchCorrect())
			{
				LogDebug("luajit with LJ64 + !GC64");
				return;
			}
		}
		LuaJitType.GC64 = true;
		LuaJitType.LJ_TISNUM = 4294967282u;
		if (IsArchCorrect())
		{
			LogDebug("luajit with LJ64 + GC64");
			return;
		}
		LuaJitType.GC64 = false;
		LuaJitType.LJ_TISNUM = 4294967282u;
		if (IsArchCorrect())
		{
			LogDebug("luajit with LJ32 + !GC64");
			return;
		}
		LuaJitType.LJ_DUALNUM = false;
		if (IsArchCorrect())
		{
			LogDebug("luajit with DUALNUM == false");
			return;
		}
		throw new LuaAdapterException("unknown arch for lua jit, try to modify this function to support it");
	}

	public unsafe override bool IsValid()
	{
		return TableRawPtr != null;
	}

	public unsafe override uint GetArrayCapacity()
	{
		if (TableRawPtr != null)
		{
			if (TableRawPtr->asize == 0)
			{
				return 0u;
			}
			return TableRawPtr->asize - 1;
		}
		return 0u;
	}

	public unsafe double GetDoubleFast(int index)
	{
		return TableRawPtr->array[index].n;
	}

	public unsafe int GetIntFast(int index)
	{
		return TableRawPtr->array[index].i;
	}

	public unsafe override double GetDouble(int index)
	{
		if (TableRawPtr != null && index >= 0 && index <= TableRawPtr->asize)
		{
			return LuaJitType.GetDouble(ref TableRawPtr->array[index]);
		}
		LuaAdapterException.ThrowIfNeeded(TableRawPtr == null, index, GetArrayCapacity());
		return 0.0;
	}

	public unsafe override void SetDouble(int index, double val)
	{
		if (TableRawPtr != null && index >= 0 && index <= TableRawPtr->asize)
		{
			TableRawPtr->array[index].n = val;
		}
		else
		{
			LuaAdapterException.ThrowIfNeeded(TableRawPtr == null, index, GetArrayCapacity());
		}
	}

	public unsafe override int GetInt(int index)
	{
		if (TableRawPtr != null && index >= 0 && index <= TableRawPtr->asize)
		{
			return LuaJitType.GetInt(ref TableRawPtr->array[index]);
		}
		LuaAdapterException.ThrowIfNeeded(TableRawPtr == null, index, GetArrayCapacity());
		return 0;
	}

	public unsafe override void SetInt(int index, int val)
	{
		if (TableRawPtr != null && index >= 0 && index <= TableRawPtr->asize)
		{
			TableRawPtr->array[index].n = val;
		}
		else
		{
			LuaAdapterException.ThrowIfNeeded(TableRawPtr == null, index, GetArrayCapacity());
		}
	}
}
