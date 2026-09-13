using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
[VisibleToOtherModules]
internal class NativeTypeAttribute : Attribute, IBindingsHeaderProviderAttribute, IBindingsAttribute, IBindingsGenerateMarshallingTypeAttribute
{
	public string Header { get; set; }

	public string IntermediateScriptingStructName { get; set; }

	public CodegenOptions CodegenOptions { get; set; }

	public NativeTypeAttribute()
	{
		CodegenOptions = CodegenOptions.Auto;
	}

	public NativeTypeAttribute(CodegenOptions codegenOptions)
	{
		CodegenOptions = codegenOptions;
	}

	public NativeTypeAttribute(string header)
	{
		if (header == null)
		{
			throw new ArgumentNullException("header");
		}
		if (header == "")
		{
			throw new ArgumentException("header cannot be empty", "header");
		}
		CodegenOptions = CodegenOptions.Auto;
		Header = header;
	}

	public NativeTypeAttribute(string header, CodegenOptions codegenOptions)
		: this(header)
	{
		CodegenOptions = codegenOptions;
	}

	public NativeTypeAttribute(CodegenOptions codegenOptions, string intermediateStructName)
		: this(codegenOptions)
	{
		IntermediateScriptingStructName = intermediateStructName;
	}
}
