using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
[Conditional("FALSE")]
public sealed class DesignerAttribute : Attribute
{
	private readonly string designerTypeName;

	private readonly string designerBaseTypeName;

	private string typeId;

	public string DesignerBaseTypeName => designerBaseTypeName;

	public string DesignerTypeName => designerTypeName;

	public override object TypeId
	{
		get
		{
			if (typeId == null)
			{
				string text = designerBaseTypeName;
				int num = text.IndexOf(',');
				if (num != -1)
				{
					text = text.Substring(0, num);
				}
				typeId = GetType().FullName + text;
			}
			return typeId;
		}
	}

	public DesignerAttribute(string designerTypeName)
	{
		designerTypeName.ToUpper(CultureInfo.InvariantCulture);
		this.designerTypeName = designerTypeName;
		designerBaseTypeName = typeof(IDesigner).FullName;
	}

	public DesignerAttribute(Type designerType)
	{
		designerTypeName = designerType.AssemblyQualifiedName;
		designerBaseTypeName = typeof(IDesigner).FullName;
	}

	public DesignerAttribute(string designerTypeName, string designerBaseTypeName)
	{
		designerTypeName.ToUpper(CultureInfo.InvariantCulture);
		this.designerTypeName = designerTypeName;
		this.designerBaseTypeName = designerBaseTypeName;
	}

	public DesignerAttribute(string designerTypeName, Type designerBaseType)
	{
		designerTypeName.ToUpper(CultureInfo.InvariantCulture);
		this.designerTypeName = designerTypeName;
		designerBaseTypeName = designerBaseType.AssemblyQualifiedName;
	}

	public DesignerAttribute(Type designerType, Type designerBaseType)
	{
		designerTypeName = designerType.AssemblyQualifiedName;
		designerBaseTypeName = designerBaseType.AssemblyQualifiedName;
	}

	public override bool Equals(object obj)
	{
		if (obj == this)
		{
			return true;
		}
		if (obj is DesignerAttribute designerAttribute && designerAttribute.designerBaseTypeName == designerBaseTypeName)
		{
			return designerAttribute.designerTypeName == designerTypeName;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return designerTypeName.GetHashCode() ^ designerBaseTypeName.GetHashCode();
	}
}
