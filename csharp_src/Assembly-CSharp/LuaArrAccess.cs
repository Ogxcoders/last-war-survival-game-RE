using System;
using XLua;

[LuaCallCSharp(GenFlag.No)]
public class LuaArrAccess
{
	public override string ToString()
	{
		return "LuaArrAccess Unknown Type";
	}

	public virtual void OnPin(IntPtr TablePtr)
	{
	}

	public virtual void OnGC()
	{
	}

	public virtual void AutoDetectArch()
	{
	}

	public virtual bool IsValid()
	{
		return false;
	}

	public virtual uint GetArrayCapacity()
	{
		return 0u;
	}

	public virtual int GetInt(int index)
	{
		return 0;
	}

	public virtual void SetInt(int index, int Value)
	{
	}

	public virtual double GetDouble(int index)
	{
		return 0.0;
	}

	public virtual void SetDouble(int index, double Value)
	{
	}

	public virtual long GetLong(int index)
	{
		return 0L;
	}

	public virtual void SetLong(int index, long Value)
	{
	}
}
