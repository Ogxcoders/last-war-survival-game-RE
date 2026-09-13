using System.Runtime.InteropServices;

namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
[ComVisible(true)]
public sealed class DebuggerVisualizerAttribute : Attribute
{
	private string visualizerObjectSourceName;

	private string visualizerName;

	private string description;

	private string targetName;

	private Type target;

	public string VisualizerObjectSourceTypeName => visualizerObjectSourceName;

	public string VisualizerTypeName => visualizerName;

	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}

	public Type Target
	{
		get
		{
			return target;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			targetName = value.AssemblyQualifiedName;
			target = value;
		}
	}

	public string TargetTypeName
	{
		get
		{
			return targetName;
		}
		set
		{
			targetName = value;
		}
	}

	public DebuggerVisualizerAttribute(string visualizerTypeName)
	{
		visualizerName = visualizerTypeName;
	}

	public DebuggerVisualizerAttribute(string visualizerTypeName, string visualizerObjectSourceTypeName)
	{
		visualizerName = visualizerTypeName;
		visualizerObjectSourceName = visualizerObjectSourceTypeName;
	}

	public DebuggerVisualizerAttribute(string visualizerTypeName, Type visualizerObjectSource)
	{
		if (visualizerObjectSource == null)
		{
			throw new ArgumentNullException("visualizerObjectSource");
		}
		visualizerName = visualizerTypeName;
		visualizerObjectSourceName = visualizerObjectSource.AssemblyQualifiedName;
	}

	public DebuggerVisualizerAttribute(Type visualizer)
	{
		if (visualizer == null)
		{
			throw new ArgumentNullException("visualizer");
		}
		visualizerName = visualizer.AssemblyQualifiedName;
	}

	public DebuggerVisualizerAttribute(Type visualizer, Type visualizerObjectSource)
	{
		if (visualizer == null)
		{
			throw new ArgumentNullException("visualizer");
		}
		if (visualizerObjectSource == null)
		{
			throw new ArgumentNullException("visualizerObjectSource");
		}
		visualizerName = visualizer.AssemblyQualifiedName;
		visualizerObjectSourceName = visualizerObjectSource.AssemblyQualifiedName;
	}

	public DebuggerVisualizerAttribute(Type visualizer, string visualizerObjectSourceTypeName)
	{
		if (visualizer == null)
		{
			throw new ArgumentNullException("visualizer");
		}
		visualizerName = visualizer.AssemblyQualifiedName;
		visualizerObjectSourceName = visualizerObjectSourceTypeName;
	}
}
