using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

public class ApplicationCatalog : ComposablePartCatalog, ICompositionElement
{
	private bool _isDisposed;

	private volatile AggregateCatalog _innerCatalog;

	private readonly object _thisLock = new object();

	private ICompositionElement _definitionOrigin;

	private ReflectionContext _reflectionContext;

	private AggregateCatalog InnerCatalog
	{
		get
		{
			if (_innerCatalog == null)
			{
				lock (_thisLock)
				{
					if (_innerCatalog == null)
					{
						string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
						Assumes.NotNull(baseDirectory);
						List<ComposablePartCatalog> list = new List<ComposablePartCatalog>();
						list.Add(CreateCatalog(baseDirectory, "*.exe"));
						list.Add(CreateCatalog(baseDirectory, "*.dll"));
						string relativeSearchPath = AppDomain.CurrentDomain.RelativeSearchPath;
						if (!string.IsNullOrEmpty(relativeSearchPath))
						{
							string[] array = relativeSearchPath.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
							foreach (string path in array)
							{
								string text = Path.Combine(baseDirectory, path);
								if (Directory.Exists(text))
								{
									list.Add(CreateCatalog(text, "*.dll"));
								}
							}
						}
						AggregateCatalog innerCatalog = new AggregateCatalog(list);
						_innerCatalog = innerCatalog;
					}
				}
			}
			return _innerCatalog;
		}
	}

	string ICompositionElement.DisplayName => GetDisplayName();

	ICompositionElement ICompositionElement.Origin => null;

	public ApplicationCatalog()
	{
	}

	public ApplicationCatalog(ICompositionElement definitionOrigin)
	{
		Requires.NotNull(definitionOrigin, "definitionOrigin");
		_definitionOrigin = definitionOrigin;
	}

	public ApplicationCatalog(ReflectionContext reflectionContext)
	{
		Requires.NotNull(reflectionContext, "reflectionContext");
		_reflectionContext = reflectionContext;
	}

	public ApplicationCatalog(ReflectionContext reflectionContext, ICompositionElement definitionOrigin)
	{
		Requires.NotNull(reflectionContext, "reflectionContext");
		Requires.NotNull(definitionOrigin, "definitionOrigin");
		_reflectionContext = reflectionContext;
		_definitionOrigin = definitionOrigin;
	}

	internal ComposablePartCatalog CreateCatalog(string location, string pattern)
	{
		if (_reflectionContext != null)
		{
			if (_definitionOrigin == null)
			{
				return new DirectoryCatalog(location, pattern, _reflectionContext);
			}
			return new DirectoryCatalog(location, pattern, _reflectionContext, _definitionOrigin);
		}
		if (_definitionOrigin == null)
		{
			return new DirectoryCatalog(location, pattern);
		}
		return new DirectoryCatalog(location, pattern, _definitionOrigin);
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				IDisposable disposable = null;
				lock (_thisLock)
				{
					disposable = _innerCatalog;
					_innerCatalog = null;
					_isDisposed = true;
				}
				disposable?.Dispose();
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public override IEnumerator<ComposablePartDefinition> GetEnumerator()
	{
		ThrowIfDisposed();
		return InnerCatalog.GetEnumerator();
	}

	public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
	{
		ThrowIfDisposed();
		Requires.NotNull(definition, "definition");
		return InnerCatalog.GetExports(definition);
	}

	[DebuggerStepThrough]
	private void ThrowIfDisposed()
	{
		if (_isDisposed)
		{
			throw ExceptionBuilder.CreateObjectDisposed(this);
		}
	}

	private string GetDisplayName()
	{
		return string.Format(CultureInfo.CurrentCulture, "{0} (Path=\"{1}\") (PrivateProbingPath=\"{2}\")", GetType().Name, AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath);
	}

	public override string ToString()
	{
		return GetDisplayName();
	}
}
