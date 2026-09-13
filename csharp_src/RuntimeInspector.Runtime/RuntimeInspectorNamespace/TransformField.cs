using System;
using System.Reflection;
using UnityEngine;

namespace RuntimeInspectorNamespace;

public class TransformField : ExpandableInspectorField
{
	private PropertyInfo positionProp;

	private PropertyInfo rotationProp;

	private PropertyInfo scaleProp;

	protected override int Length => 3;

	public override void Initialize()
	{
		base.Initialize();
		positionProp = typeof(Transform).GetProperty("localPosition");
		rotationProp = typeof(Transform).GetProperty("localEulerAngles");
		scaleProp = typeof(Transform).GetProperty("localScale");
	}

	public override bool SupportsType(Type type)
	{
		return type == typeof(Transform);
	}

	protected override void GenerateElements()
	{
		CreateDrawerForVariable(positionProp, "Position");
		CreateDrawerForVariable(rotationProp, "Rotation");
		CreateDrawerForVariable(scaleProp, "Scale");
	}
}
