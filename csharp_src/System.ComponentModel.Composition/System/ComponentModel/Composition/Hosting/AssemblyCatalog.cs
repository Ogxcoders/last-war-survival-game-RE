using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

[DebuggerTypeProxy(typeof(AssemblyCatalogDebuggerProxy))]
public class AssemblyCatalog : ComposablePartCatalog, ICompositionElement
{
	private readonly object _thisLock = new object();

	private readonly ICompositionElement _definitionOrigin;

	private volatile Assembly _assembly;

	private volatile ComposablePartCatalog _innerCatalog;

	private int _isDisposed;

	private ReflectionContext _reflectionContext;

	private ComposablePartCatalog InnerCatalog
	{
		get
		{
			ThrowIfDisposed();
			if (_innerCatalog == null)
			{
				CatalogReflectionContextAttribute firstAttribute = _assembly.GetFirstAttribute<CatalogReflectionContextAttribute>();
				Assembly assembly = ((firstAttribute != null) ? firstAttribute.CreateReflectionContext().MapAssembly(_assembly) : _assembly);
				lock (_thisLock)
				{
					if (_innerCatalog == null)
					{
						TypeCatalog innerCatalog = ((_reflectionContext != null) ? new TypeCatalog(assembly.GetTypes(), _reflectionContext, _definitionOrigin) : new TypeCatalog(assembly.GetTypes(), _definitionOrigin));
						Thread.MemoryBarrier();
						_innerCatalog = innerCatalog;
					}
				}
			}
			return _innerCatalog;
		}
	}

	public Assembly Assembly => _assembly;

	string ICompositionElement.DisplayName => GetDisplayName();

	ICompositionElement ICompositionElement.Origin => null;

	public AssemblyCatalog(string codeBase)
	{
		Requires.NotNullOrEmpty(codeBase, "codeBase");
		InitializeAssemblyCatalog(LoadAssembly(codeBase));
		_definitionOrigin = this;
	}

	public AssemblyCatalog(string codeBase, ReflectionContext reflectionContext)
	{
		Requires.NotNullOrEmpty(codeBase, "codeBase");
		Requires.NotNull(reflectionContext, "reflectionContext");
		InitializeAssemblyCatalog(LoadAssembly(codeBase));
		_reflectionContext = reflectionContext;
		_definitionOrigin = this;
	}

	public AssemblyCatalog(string codeBase, ICompositionElement definitionOrigin)
	{
		Requires.NotNullOrEmpty(codeBase, "codeBase");
		Requires.NotNull(definitionOrigin, "definitionOrigin");
		InitializeAssemblyCatalog(LoadAssembly(codeBase));
		_definitionOrigin = definitionOrigin;
	}

	public AssemblyCatalog(string codeBase, ReflectionContext reflectionContext, ICompositionElement definitionOrigin)
	{
		Requires.NotNullOrEmpty(codeBase, "codeBase");
		Requires.NotNull(reflectionContext, "reflectionContext");
		Requires.NotNull(definitionOrigin, "definitionOrigin");
		InitializeAssemblyCatalog(LoadAssembly(codeBase));
		_reflectionContext = reflectionContext;
		_definitionOrigin = definitionOrigin;
	}

	public AssemblyCatalog(Assembly assembly, ReflectionContext reflectionContext)
	{
		Requires.NotNull(assembly, "assembly");
		Requires.NotNull(reflectionContext, "reflectionContext");
		InitializeAssemblyCatalog(assembly);
		_reflectionContext = reflectionContext;
		_definitionOrigin = this;
	}

	public AssemblyCatalog(Assembly assembly, ReflectionContext reflectionContext, ICompositionElement definitionOrigin)
	{
		Requires.NotNull(assembly, "assembly");
		Requires.NotNull(reflectionContext, "reflectionContext");
		Requires.NotNull(definitionOrigin, "definitionOrigin");
		InitializeAssemblyCatalog(assembly);
		_reflectionContext = reflectionContext;
		_definitionOrigin = definitionOrigin;
	}

	public AssemblyCatalog(Assembly assembly)
	{
		Requires.NotNull(assembly, "assembly");
		InitializeAssemblyCatalog(assembly);
		_definitionOrigin = this;
	}

	public AssemblyCatalog(Assembly assembly, ICompositionElement definitionOrigin)
	{
		Requires.NotNull(assembly, "assembly");
		Requires.NotNull(definitionOrigin, "definitionOrigin");
		InitializeAssemblyCatalog(assembly);
		_definitionOrigin = definitionOrigin;
	}

	private void InitializeAssemblyCatalog(Assembly assembly)
	{
		_assembly = assembly;
	}

	public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
	{
		return InnerCatalog.GetExports(definition);
	}

	public override string ToString()
	{
		return GetDisplayName();
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) == 0 && disposing && _innerCatalog != null)
			{
				_innerCatalog.Dispose();
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public override IEnumerator<ComposablePartDefinition> GetEnumerator()
	{
		return InnerCatalog.GetEnumerator();
	}

	private void ThrowIfDisposed()
	{
		if (_isDisposed == 1)
		{
			throw ExceptionBuilder.CreateObjectDisposed(this);
		}
	}

	private string GetDisplayName()
	{
		return string.Format(CultureInfo.CurrentCulture, "{0} (Assembly=\"{1}\")", GetType().Name, Assembly.FullName);
	}

	private static Assembly LoadAssembly(string codeBase)
	{
		Requires.NotNullOrEmpty(codeBase, "codeBase");
		AssemblyName assemblyName;
		try
		{
			assemblyName = AssemblyName.GetAssemblyName(codeBase);
		}
		catch (ArgumentException)
		{
			assemblyName = new AssemblyName();
			assemblyName.CodeBase = codeBase;
		}
		return Assembly.Load(assemblyName);
	}
}
