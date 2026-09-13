using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
[Conditional("UNITY_EDITOR")]
public class ButtonAttribute : ShowInInspectorAttribute
{
	public int ButtonHeight;

	public string Name;

	public ButtonStyle Style;

	public bool Expanded;

	public bool DisplayParameters = true;

	public bool DirtyOnClick = true;

	private bool drawResult;

	private bool drawResultIsSet;

	public bool DrawResult
	{
		get
		{
			return drawResult;
		}
		set
		{
			drawResult = value;
			drawResultIsSet = true;
		}
	}

	public bool DrawResultIsSet => drawResultIsSet;

	public ButtonAttribute()
	{
		Name = null;
		ButtonHeight = 0;
	}

	public ButtonAttribute(ButtonSizes size)
	{
		Name = null;
		ButtonHeight = (int)size;
	}

	public ButtonAttribute(int buttonSize)
	{
		ButtonHeight = buttonSize;
		Name = null;
	}

	public ButtonAttribute(string name)
	{
		Name = name;
		ButtonHeight = 0;
	}

	public ButtonAttribute(string name, ButtonSizes buttonSize)
	{
		Name = name;
		ButtonHeight = (int)buttonSize;
	}

	public ButtonAttribute(string name, int buttonSize)
	{
		Name = name;
		ButtonHeight = buttonSize;
	}

	public ButtonAttribute(ButtonStyle parameterBtnStyle)
	{
		Name = null;
		ButtonHeight = 0;
		Style = parameterBtnStyle;
	}

	public ButtonAttribute(int buttonSize, ButtonStyle parameterBtnStyle)
	{
		ButtonHeight = buttonSize;
		Name = null;
		Style = parameterBtnStyle;
	}

	public ButtonAttribute(ButtonSizes size, ButtonStyle parameterBtnStyle)
	{
		ButtonHeight = (int)size;
		Name = null;
		Style = parameterBtnStyle;
	}

	public ButtonAttribute(string name, ButtonStyle parameterBtnStyle)
	{
		Name = name;
		ButtonHeight = 0;
		Style = parameterBtnStyle;
	}

	public ButtonAttribute(string name, ButtonSizes buttonSize, ButtonStyle parameterBtnStyle)
	{
		Name = name;
		ButtonHeight = (int)buttonSize;
		Style = parameterBtnStyle;
	}

	public ButtonAttribute(string name, int buttonSize, ButtonStyle parameterBtnStyle)
	{
		Name = name;
		ButtonHeight = buttonSize;
		Style = parameterBtnStyle;
	}
}
