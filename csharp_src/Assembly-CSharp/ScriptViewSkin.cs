using System;
using System.Collections.Generic;
using UnityEngine;

public class ScriptViewSkin : MonoBehaviour
{
	public enum ExportType
	{
		Uibasecontainer,
		Uibaseview
	}

	[Serializable]
	public class SkinSingleProperty
	{
		public string propName;

		public UnityEngine.Object propVal;

		[NonSerialized]
		public bool generateScript = true;
	}

	[Serializable]
	public class SkinMultipleProperty
	{
		public string propName;

		public UnityEngine.Object[] propVal;

		[NonSerialized]
		public bool generateScript = true;
	}

	[NonSerialized]
	public bool messageDisabled;

	[NonSerialized]
	public string exportId;

	[NonSerialized]
	public string exportPath;

	[NonSerialized]
	public ExportType exportType;

	public List<SkinSingleProperty> singlePropList;

	public List<SkinMultipleProperty> multiplePropList;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}
}
