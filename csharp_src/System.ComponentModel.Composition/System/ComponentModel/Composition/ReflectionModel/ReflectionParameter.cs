using System.Globalization;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class ReflectionParameter : ReflectionItem
{
	private readonly ParameterInfo _parameter;

	public ParameterInfo UnderlyingParameter => _parameter;

	public override string Name => UnderlyingParameter.Name;

	public override Type ReturnType => UnderlyingParameter.ParameterType;

	public override ReflectionItemType ItemType => ReflectionItemType.Parameter;

	public ReflectionParameter(ParameterInfo parameter)
	{
		Assumes.NotNull(parameter);
		_parameter = parameter;
	}

	public override string GetDisplayName()
	{
		return string.Format(CultureInfo.CurrentCulture, "{0} (Parameter=\"{1}\")", UnderlyingParameter.Member.GetDisplayName(), UnderlyingParameter.Name);
	}
}
