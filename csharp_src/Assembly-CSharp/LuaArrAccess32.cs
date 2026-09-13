using System;
using XLua;

[LuaCallCSharp(GenFlag.No)]
public class LuaArrAccess32 : LuaArrAccess
{
	private unsafe LuaTableRawDef* TableRawPtr;

	public unsafe override string ToString()
	{
		return "LuaTablePin32 " + (IntPtr)TableRawPtr;
	}

	public unsafe override void OnPin(IntPtr TablePtr)
	{
		TableRawPtr = (LuaTableRawDef*)(void*)TablePtr;
	}

	public unsafe override void OnGC()
	{
		TableRawPtr = null;
	}

	public unsafe override bool IsValid()
	{
		return TableRawPtr != null;
	}

	public unsafe override uint GetArrayCapacity()
	{
		if (TableRawPtr != null)
		{
			return TableRawPtr->sizearray;
		}
		return 0u;
	}

	public unsafe override int GetInt(int index)
	{
		if (TableRawPtr != null && index > 0 && index <= TableRawPtr->sizearray)
		{
			index--;
			LuaTValue32* ptr = (LuaTValue32*)(void*)TableRawPtr->alimit + index;
			if (ptr->tt_ != 19)
			{
				return (int)ptr->n;
			}
			return ptr->i;
		}
		LuaAdapterException.ThrowIfNeeded(TableRawPtr == null, index, GetArrayCapacity());
		return 0;
	}

	public unsafe override void SetInt(int index, int Value)
	{
		if (TableRawPtr != null && index > 0 && index <= TableRawPtr->sizearray)
		{
			index--;
			byte* num = (byte*)(void*)TableRawPtr->alimit + (nint)index * (nint)sizeof(LuaTValue32);
			((LuaTValue32*)num)->i = Value;
			((LuaTValue32*)num)->tt_ = 19;
		}
		else
		{
			LuaAdapterException.ThrowIfNeeded(TableRawPtr == null, index, GetArrayCapacity());
		}
	}

	public unsafe override double GetDouble(int index)
	{
		if (TableRawPtr != null && index > 0 && index <= TableRawPtr->sizearray)
		{
			index--;
			LuaTValue32* ptr = (LuaTValue32*)(void*)TableRawPtr->alimit + index;
			if (ptr->tt_ == 19)
			{
				return ptr->i;
			}
			return ptr->n;
		}
		LuaAdapterException.ThrowIfNeeded(TableRawPtr == null, index, GetArrayCapacity());
		return 0.0;
	}

	public unsafe override void SetDouble(int index, double Value)
	{
		if (TableRawPtr != null && index > 0 && index <= TableRawPtr->sizearray)
		{
			index--;
			byte* num = (byte*)(void*)TableRawPtr->alimit + (nint)index * (nint)sizeof(LuaTValue32);
			((LuaTValue32*)num)->n = (float)Value;
			((LuaTValue32*)num)->tt_ = 3;
		}
		else
		{
			LuaAdapterException.ThrowIfNeeded(TableRawPtr == null, index, GetArrayCapacity());
		}
	}

	public unsafe override long GetLong(int index)
	{
		if (TableRawPtr != null && index > 0 && index <= TableRawPtr->sizearray)
		{
			index--;
			LuaTValue32* ptr = (LuaTValue32*)(void*)TableRawPtr->alimit + index;
			if (ptr->tt_ == 19)
			{
				return ptr->i;
			}
			return (long)ptr->n;
		}
		LuaAdapterException.ThrowIfNeeded(TableRawPtr == null, index, GetArrayCapacity());
		return 0L;
	}

	public unsafe override void SetLong(int index, long Value)
	{
		if (TableRawPtr != null && index > 0 && index <= TableRawPtr->sizearray)
		{
			index--;
			byte* num = (byte*)(void*)TableRawPtr->alimit + (nint)index * (nint)sizeof(LuaTValue32);
			((LuaTValue32*)num)->i = (int)Value;
			((LuaTValue32*)num)->tt_ = 19;
		}
		else
		{
			LuaAdapterException.ThrowIfNeeded(TableRawPtr == null, index, GetArrayCapacity());
		}
	}
}
