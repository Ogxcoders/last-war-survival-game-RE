using System;
using System.Collections.Generic;
using FibMatrix.Rendering;
using UnityEngine;
using XLua;

public class ViewSkinProPropertyRecorder : MonoBehaviour
{
	public enum CodeType
	{
		UIBaseContainer,
		UIBaseView
	}

	[Serializable]
	public class ViewSkinProProperty
	{
		public UnityEngine.Object Object;

		public string CustomTypeName;

		public string CustomCodeName;

		public GameObject GameObject
		{
			get
			{
				if (Object is GameObject result)
				{
					return result;
				}
				if (Object is Component component)
				{
					return component.gameObject;
				}
				return null;
			}
		}
	}

	[SerializeField]
	private List<ViewSkinProProperty> properties = new List<ViewSkinProProperty>();

	[SerializeField]
	private string codeGuid;

	[SerializeField]
	private CodeType codeType;

	public List<ViewSkinProProperty> GetAllProperties()
	{
		return properties;
	}

	public UnityEngine.Object GetProperty(int index)
	{
		if (index < 0 || index >= properties.Count)
		{
			return null;
		}
		ViewSkinProProperty viewSkinProProperty = properties[index];
		if (viewSkinProProperty == null)
		{
			return null;
		}
		if (viewSkinProProperty.Object == null)
		{
			return null;
		}
		return viewSkinProProperty.Object;
	}

	public void FillLua(LuaTable luaTable)
	{
		if (luaTable != null && properties != null && properties.Count != 0)
		{
			int num = 0;
			for (int i = 0; i < properties.Count; i++)
			{
				luaTable[++num] = properties[i].Object;
			}
		}
	}

	public void SetCodeGuid(string guid)
	{
		codeGuid = guid;
	}

	public string GetCodeGuid()
	{
		return codeGuid;
	}

	public CodeType GetCodeType()
	{
		return codeType;
	}

	public void SetCodeType(CodeType codeType)
	{
		this.codeType = codeType;
	}

	public bool AddProperty(ViewSkinProProperty property)
	{
		if (property == null || property.Object == null || property.GameObject == null)
		{
			return false;
		}
		if (!property.GameObject.IsChildOf(base.gameObject))
		{
			return false;
		}
		properties.Add(property);
		return true;
	}

	public void ClearProperties()
	{
		properties.Clear();
	}
}
