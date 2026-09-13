using System.ComponentModel.Composition.Primitives;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

public class CompositionService : ICompositionService, IDisposable
{
	private CompositionContainer _compositionContainer;

	private INotifyComposablePartCatalogChanged _notifyCatalog;

	internal CompositionService(ComposablePartCatalog composablePartCatalog)
	{
		Assumes.NotNull(composablePartCatalog);
		_notifyCatalog = composablePartCatalog as INotifyComposablePartCatalogChanged;
		try
		{
			if (_notifyCatalog != null)
			{
				_notifyCatalog.Changing += OnCatalogChanging;
			}
			CompositionOptions compositionOptions = CompositionOptions.DisableSilentRejection | CompositionOptions.IsThreadSafe | CompositionOptions.ExportCompositionService;
			CompositionContainer compositionContainer = new CompositionContainer(composablePartCatalog, compositionOptions);
			_compositionContainer = compositionContainer;
		}
		catch
		{
			if (_notifyCatalog != null)
			{
				_notifyCatalog.Changing -= OnCatalogChanging;
			}
			throw;
		}
	}

	public void SatisfyImportsOnce(ComposablePart part)
	{
		Requires.NotNull(part, "part");
		Assumes.NotNull(_compositionContainer);
		_compositionContainer.SatisfyImportsOnce(part);
	}

	public void Dispose()
	{
		Assumes.NotNull(_compositionContainer);
		if (_notifyCatalog != null)
		{
			_notifyCatalog.Changing -= OnCatalogChanging;
		}
		_compositionContainer.Dispose();
	}

	private void OnCatalogChanging(object sender, ComposablePartCatalogChangeEventArgs e)
	{
		throw new ChangeRejectedException(Strings.NotSupportedCatalogChanges);
	}
}
