using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

internal static class VisualElementFactoryRegistry
{
	private static Dictionary<string, List<IUxmlFactory>> s_Factories;

	internal static Dictionary<string, List<IUxmlFactory>> factories
	{
		get
		{
			if (s_Factories == null)
			{
				s_Factories = new Dictionary<string, List<IUxmlFactory>>();
				RegisterEngineFactories();
			}
			return s_Factories;
		}
	}

	internal static void RegisterFactory(IUxmlFactory factory)
	{
		if (factories.TryGetValue(factory.uxmlQualifiedName, out var value))
		{
			foreach (IUxmlFactory item in value)
			{
				if ((object)item.GetType() == factory.GetType())
				{
					throw new ArgumentException("A factory of this type was already registered");
				}
			}
			value.Add(factory);
		}
		else
		{
			value = new List<IUxmlFactory>();
			value.Add(factory);
			factories.Add(factory.uxmlQualifiedName, value);
		}
	}

	internal static bool TryGetValue(string fullTypeName, out List<IUxmlFactory> factoryList)
	{
		factoryList = null;
		return factories.TryGetValue(fullTypeName, out factoryList);
	}

	private static void RegisterEngineFactories()
	{
		IUxmlFactory[] array = new IUxmlFactory[21]
		{
			new UxmlRootElementFactory(),
			new Button.UxmlFactory(),
			new VisualElement.UxmlFactory(),
			new IMGUIContainer.UxmlFactory(),
			new Image.UxmlFactory(),
			new Label.UxmlFactory(),
			new RepeatButton.UxmlFactory(),
			new ScrollView.UxmlFactory(),
			new Scroller.UxmlFactory(),
			new Slider.UxmlFactory(),
			new SliderInt.UxmlFactory(),
			new MinMaxSlider.UxmlFactory(),
			new Toggle.UxmlFactory(),
			new TextField.UxmlFactory(),
			new TemplateContainer.UxmlFactory(),
			new Box.UxmlFactory(),
			new PopupWindow.UxmlFactory(),
			new ListView.UxmlFactory(),
			new TreeView.UxmlFactory(),
			new Foldout.UxmlFactory(),
			new BindableElement.UxmlFactory()
		};
		IUxmlFactory[] array2 = array;
		foreach (IUxmlFactory factory in array2)
		{
			RegisterFactory(factory);
		}
	}
}
