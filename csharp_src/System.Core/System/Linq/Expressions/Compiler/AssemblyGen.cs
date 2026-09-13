using System.Dynamic.Utils;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace System.Linq.Expressions.Compiler;

internal sealed class AssemblyGen
{
	private static AssemblyGen s_assembly;

	private readonly ModuleBuilder _myModule;

	private int _index;

	private static AssemblyGen Assembly
	{
		get
		{
			if (s_assembly == null)
			{
				Interlocked.CompareExchange(ref s_assembly, new AssemblyGen(), null);
			}
			return s_assembly;
		}
	}

	private AssemblyGen()
	{
		AssemblyName assemblyName = new AssemblyName("Snippets");
		AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
		_myModule = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
	}

	private TypeBuilder DefineType(string name, Type parent, TypeAttributes attr)
	{
		ContractUtils.RequiresNotNull(name, "name");
		ContractUtils.RequiresNotNull(parent, "parent");
		StringBuilder stringBuilder = new StringBuilder(name);
		int value = Interlocked.Increment(ref _index);
		stringBuilder.Append("$");
		stringBuilder.Append(value);
		stringBuilder.Replace('+', '_').Replace('[', '_').Replace(']', '_')
			.Replace('*', '_')
			.Replace('&', '_')
			.Replace(',', '_')
			.Replace('\\', '_');
		name = stringBuilder.ToString();
		return _myModule.DefineType(name, attr, parent);
	}

	internal static TypeBuilder DefineDelegateType(string name)
	{
		return Assembly.DefineType(name, typeof(MulticastDelegate), TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoClass);
	}
}
