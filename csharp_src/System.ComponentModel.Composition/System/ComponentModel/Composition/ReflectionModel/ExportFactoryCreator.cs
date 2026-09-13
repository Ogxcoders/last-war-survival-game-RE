using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal sealed class ExportFactoryCreator
{
	private class LifetimeContext
	{
		private static Type[] types = new Type[1] { typeof(ComposablePartDefinition) };

		public Func<ComposablePartDefinition, bool> CatalogFilter { get; private set; }

		public void SetInstance(object instance)
		{
			Assumes.NotNull(instance);
			MethodInfo method = instance.GetType().GetMethod("IncludeInScopedCatalog", BindingFlags.Instance | BindingFlags.NonPublic, null, types, null);
			CatalogFilter = (Func<ComposablePartDefinition, bool>)Delegate.CreateDelegate(typeof(Func<ComposablePartDefinition, bool>), instance, method);
		}

		public Tuple<T, Action> GetExportLifetimeContextFromExport<T>(Export export)
		{
			IDisposable disposable = null;
			T item;
			if (export is CatalogExportProvider.ScopeFactoryExport scopeFactoryExport)
			{
				Export export2 = scopeFactoryExport.CreateExportProduct(CatalogFilter);
				item = ExportServices.GetCastedExportedValue<T>(export2);
				disposable = export2 as IDisposable;
			}
			else if (export is CatalogExportProvider.FactoryExport factoryExport)
			{
				Export export3 = factoryExport.CreateExportProduct();
				item = ExportServices.GetCastedExportedValue<T>(export3);
				disposable = export3 as IDisposable;
			}
			else
			{
				ComposablePartDefinition castedExportedValue = ExportServices.GetCastedExportedValue<ComposablePartDefinition>(export);
				ComposablePart composablePart = castedExportedValue.CreatePart();
				ExportDefinition definition = castedExportedValue.ExportDefinitions.Single();
				item = ExportServices.CastExportedValue<T>(composablePart.ToElement(), composablePart.GetExportedValue(definition));
				disposable = composablePart as IDisposable;
			}
			Action item2 = ((disposable == null) ? ((Action)delegate
			{
			}) : ((Action)delegate
			{
				disposable.Dispose();
			}));
			return new Tuple<T, Action>(item, item2);
		}
	}

	private static readonly MethodInfo _createStronglyTypedExportFactoryOfT = typeof(ExportFactoryCreator).GetMethod("CreateStronglyTypedExportFactoryOfT", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

	private static readonly MethodInfo _createStronglyTypedExportFactoryOfTM = typeof(ExportFactoryCreator).GetMethod("CreateStronglyTypedExportFactoryOfTM", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

	private Type _exportFactoryType;

	public ExportFactoryCreator(Type exportFactoryType)
	{
		Assumes.NotNull(exportFactoryType);
		_exportFactoryType = exportFactoryType;
	}

	public Func<Export, object> CreateStronglyTypedExportFactoryFactory(Type exportType, Type metadataViewType)
	{
		MethodInfo methodInfo = null;
		methodInfo = ((!(metadataViewType == null)) ? _createStronglyTypedExportFactoryOfTM.MakeGenericMethod(exportType, metadataViewType) : _createStronglyTypedExportFactoryOfT.MakeGenericMethod(exportType));
		Assumes.NotNull(methodInfo);
		Func<Export, object> exportFactoryFactory = (Func<Export, object>)Delegate.CreateDelegate(typeof(Func<Export, object>), this, methodInfo);
		return (Export e) => exportFactoryFactory(e);
	}

	private object CreateStronglyTypedExportFactoryOfT<T>(Export export)
	{
		Type[] typeArguments = new Type[1] { typeof(T) };
		Type type = _exportFactoryType.MakeGenericType(typeArguments);
		LifetimeContext lifetimeContext = new LifetimeContext();
		Func<Tuple<T, Action>> func = () => lifetimeContext.GetExportLifetimeContextFromExport<T>(export);
		object[] args = new object[1] { func };
		object obj = Activator.CreateInstance(type, args);
		lifetimeContext.SetInstance(obj);
		return obj;
	}

	private object CreateStronglyTypedExportFactoryOfTM<T, M>(Export export)
	{
		Type[] typeArguments = new Type[2]
		{
			typeof(T),
			typeof(M)
		};
		Type type = _exportFactoryType.MakeGenericType(typeArguments);
		LifetimeContext lifetimeContext = new LifetimeContext();
		Func<Tuple<T, Action>> func = () => lifetimeContext.GetExportLifetimeContextFromExport<T>(export);
		M metadataView = AttributedModelServices.GetMetadataView<M>(export.Metadata);
		object[] args = new object[2] { func, metadataView };
		object obj = Activator.CreateInstance(type, args);
		lifetimeContext.SetInstance(obj);
		return obj;
	}
}
