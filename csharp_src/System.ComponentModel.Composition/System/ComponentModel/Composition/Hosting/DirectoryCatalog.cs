using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Diagnostics;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Hosting;

[DebuggerTypeProxy(typeof(DirectoryCatalogDebuggerProxy))]
public class DirectoryCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged, ICompositionElement
{
	internal class DirectoryCatalogDebuggerProxy
	{
		private readonly DirectoryCatalog _catalog;

		public ReadOnlyCollection<Assembly> Assemblies => _catalog._assemblyCatalogs.Values.Select((AssemblyCatalog catalog) => catalog.Assembly).ToReadOnlyCollection();

		public ReflectionContext ReflectionContext => _catalog._reflectionContext;

		public string SearchPattern => _catalog.SearchPattern;

		public string Path => _catalog._path;

		public string FullPath => _catalog._fullPath;

		public ReadOnlyCollection<string> LoadedFiles => _catalog._loadedFiles;

		public ReadOnlyCollection<ComposablePartDefinition> Parts => _catalog.Parts.ToReadOnlyCollection();

		public DirectoryCatalogDebuggerProxy(DirectoryCatalog catalog)
		{
			Requires.NotNull(catalog, "catalog");
			_catalog = catalog;
		}
	}

	private readonly Lock _thisLock = new Lock();

	private readonly ICompositionElement _definitionOrigin;

	private ComposablePartCatalogCollection _catalogCollection;

	private Dictionary<string, AssemblyCatalog> _assemblyCatalogs;

	private volatile bool _isDisposed;

	private string _path;

	private string _fullPath;

	private string _searchPattern;

	private ReadOnlyCollection<string> _loadedFiles;

	private readonly ReflectionContext _reflectionContext;

	public string FullPath => _fullPath;

	public ReadOnlyCollection<string> LoadedFiles
	{
		get
		{
			using (new ReadLock(_thisLock))
			{
				return _loadedFiles;
			}
		}
	}

	public string Path => _path;

	public string SearchPattern => _searchPattern;

	string ICompositionElement.DisplayName => GetDisplayName();

	ICompositionElement ICompositionElement.Origin => null;

	public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed;

	public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing;

	public DirectoryCatalog(string path)
		: this(path, "*.dll")
	{
	}

	public DirectoryCatalog(string path, ReflectionContext reflectionContext)
		: this(path, "*.dll", reflectionContext)
	{
	}

	public DirectoryCatalog(string path, ICompositionElement definitionOrigin)
		: this(path, "*.dll", definitionOrigin)
	{
	}

	public DirectoryCatalog(string path, ReflectionContext reflectionContext, ICompositionElement definitionOrigin)
		: this(path, "*.dll", reflectionContext, definitionOrigin)
	{
	}

	public DirectoryCatalog(string path, string searchPattern)
	{
		Requires.NotNullOrEmpty(path, "path");
		Requires.NotNullOrEmpty(searchPattern, "searchPattern");
		_definitionOrigin = this;
		Initialize(path, searchPattern);
	}

	public DirectoryCatalog(string path, string searchPattern, ICompositionElement definitionOrigin)
	{
		Requires.NotNullOrEmpty(path, "path");
		Requires.NotNullOrEmpty(searchPattern, "searchPattern");
		Requires.NotNull(definitionOrigin, "definitionOrigin");
		_definitionOrigin = definitionOrigin;
		Initialize(path, searchPattern);
	}

	public DirectoryCatalog(string path, string searchPattern, ReflectionContext reflectionContext)
	{
		Requires.NotNullOrEmpty(path, "path");
		Requires.NotNullOrEmpty(searchPattern, "searchPattern");
		Requires.NotNull(reflectionContext, "reflectionContext");
		_reflectionContext = reflectionContext;
		_definitionOrigin = this;
		Initialize(path, searchPattern);
	}

	public DirectoryCatalog(string path, string searchPattern, ReflectionContext reflectionContext, ICompositionElement definitionOrigin)
	{
		Requires.NotNullOrEmpty(path, "path");
		Requires.NotNullOrEmpty(searchPattern, "searchPattern");
		Requires.NotNull(reflectionContext, "reflectionContext");
		Requires.NotNull(definitionOrigin, "definitionOrigin");
		_reflectionContext = reflectionContext;
		_definitionOrigin = definitionOrigin;
		Initialize(path, searchPattern);
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!disposing || _isDisposed)
			{
				return;
			}
			bool flag = false;
			ComposablePartCatalogCollection composablePartCatalogCollection = null;
			try
			{
				using (new WriteLock(_thisLock))
				{
					if (!_isDisposed)
					{
						flag = true;
						composablePartCatalogCollection = _catalogCollection;
						_catalogCollection = null;
						_assemblyCatalogs = null;
						_isDisposed = true;
					}
				}
			}
			finally
			{
				composablePartCatalogCollection?.Dispose();
				if (flag)
				{
					_thisLock.Dispose();
				}
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public override IEnumerator<ComposablePartDefinition> GetEnumerator()
	{
		return _catalogCollection.SelectMany((ComposablePartCatalog catalog) => catalog).GetEnumerator();
	}

	public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
	{
		ThrowIfDisposed();
		Requires.NotNull(definition, "definition");
		return _catalogCollection.SelectMany((ComposablePartCatalog catalog) => catalog.GetExports(definition));
	}

	protected virtual void OnChanged(ComposablePartCatalogChangeEventArgs e)
	{
		this.Changed?.Invoke(this, e);
	}

	protected virtual void OnChanging(ComposablePartCatalogChangeEventArgs e)
	{
		this.Changing?.Invoke(this, e);
	}

	public void Refresh()
	{
		ThrowIfDisposed();
		Assumes.NotNull(_loadedFiles);
		ComposablePartDefinition[] addedDefinitions;
		ComposablePartDefinition[] removedDefinitions;
		while (true)
		{
			string[] files = GetFiles();
			object loadedFiles;
			string[] beforeFiles;
			using (new ReadLock(_thisLock))
			{
				loadedFiles = _loadedFiles;
				beforeFiles = _loadedFiles.ToArray();
			}
			DiffChanges(beforeFiles, files, out var catalogsToAdd, out var catalogsToRemove);
			if (catalogsToAdd.Count == 0 && catalogsToRemove.Count == 0)
			{
				return;
			}
			addedDefinitions = catalogsToAdd.SelectMany((Tuple<string, AssemblyCatalog> cat) => cat.Item2).ToArray();
			removedDefinitions = catalogsToRemove.SelectMany((Tuple<string, AssemblyCatalog> cat) => cat.Item2).ToArray();
			using AtomicComposition atomicComposition = new AtomicComposition();
			ComposablePartCatalogChangeEventArgs e = new ComposablePartCatalogChangeEventArgs(addedDefinitions, removedDefinitions, atomicComposition);
			OnChanging(e);
			using (new WriteLock(_thisLock))
			{
				if (loadedFiles != _loadedFiles)
				{
					continue;
				}
				foreach (Tuple<string, AssemblyCatalog> item in catalogsToAdd)
				{
					_assemblyCatalogs.Add(item.Item1, item.Item2);
					_catalogCollection.Add(item.Item2);
				}
				foreach (Tuple<string, AssemblyCatalog> item2 in catalogsToRemove)
				{
					_assemblyCatalogs.Remove(item2.Item1);
					_catalogCollection.Remove(item2.Item2);
				}
				_loadedFiles = files.ToReadOnlyCollection();
				atomicComposition.Complete();
				break;
			}
		}
		ComposablePartCatalogChangeEventArgs e2 = new ComposablePartCatalogChangeEventArgs(addedDefinitions, removedDefinitions, null);
		OnChanged(e2);
	}

	public override string ToString()
	{
		return GetDisplayName();
	}

	private AssemblyCatalog CreateAssemblyCatalogGuarded(string assemblyFilePath)
	{
		Exception ex = null;
		try
		{
			return (_reflectionContext != null) ? new AssemblyCatalog(assemblyFilePath, _reflectionContext, this) : new AssemblyCatalog(assemblyFilePath, this);
		}
		catch (FileNotFoundException ex2)
		{
			ex = ex2;
		}
		catch (FileLoadException ex3)
		{
			ex = ex3;
		}
		catch (BadImageFormatException ex4)
		{
			ex = ex4;
		}
		catch (ReflectionTypeLoadException ex5)
		{
			ex = ex5;
		}
		CompositionTrace.AssemblyLoadFailed(this, assemblyFilePath, ex);
		return null;
	}

	private void DiffChanges(string[] beforeFiles, string[] afterFiles, out List<Tuple<string, AssemblyCatalog>> catalogsToAdd, out List<Tuple<string, AssemblyCatalog>> catalogsToRemove)
	{
		catalogsToAdd = new List<Tuple<string, AssemblyCatalog>>();
		catalogsToRemove = new List<Tuple<string, AssemblyCatalog>>();
		foreach (string item in afterFiles.Except(beforeFiles))
		{
			AssemblyCatalog assemblyCatalog = CreateAssemblyCatalogGuarded(item);
			if (assemblyCatalog != null)
			{
				catalogsToAdd.Add(new Tuple<string, AssemblyCatalog>(item, assemblyCatalog));
			}
		}
		IEnumerable<string> enumerable = beforeFiles.Except(afterFiles);
		using (new ReadLock(_thisLock))
		{
			foreach (string item2 in enumerable)
			{
				if (_assemblyCatalogs.TryGetValue(item2, out var value))
				{
					catalogsToRemove.Add(new Tuple<string, AssemblyCatalog>(item2, value));
				}
			}
		}
	}

	private string GetDisplayName()
	{
		return string.Format(CultureInfo.CurrentCulture, "{0} (Path=\"{1}\")", GetType().Name, _path);
	}

	private string[] GetFiles()
	{
		return Directory.GetFiles(_fullPath, _searchPattern);
	}

	private static string GetFullPath(string path)
	{
		if (!System.IO.Path.IsPathRooted(path) && AppDomain.CurrentDomain.BaseDirectory != null)
		{
			path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
		}
		return System.IO.Path.GetFullPath(path);
	}

	private void Initialize(string path, string searchPattern)
	{
		_path = path;
		_fullPath = GetFullPath(path);
		_searchPattern = searchPattern;
		_assemblyCatalogs = new Dictionary<string, AssemblyCatalog>();
		_catalogCollection = new ComposablePartCatalogCollection(null, null, null);
		_loadedFiles = GetFiles().ToReadOnlyCollection();
		foreach (string loadedFile in _loadedFiles)
		{
			AssemblyCatalog assemblyCatalog = null;
			assemblyCatalog = CreateAssemblyCatalogGuarded(loadedFile);
			if (assemblyCatalog != null)
			{
				_assemblyCatalogs.Add(loadedFile, assemblyCatalog);
				_catalogCollection.Add(assemblyCatalog);
			}
		}
	}

	[DebuggerStepThrough]
	private void ThrowIfDisposed()
	{
		if (_isDisposed)
		{
			throw ExceptionBuilder.CreateObjectDisposed(this);
		}
	}
}
