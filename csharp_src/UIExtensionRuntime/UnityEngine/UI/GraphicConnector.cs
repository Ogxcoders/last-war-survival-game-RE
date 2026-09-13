using System;
using System.Collections.Generic;

namespace UnityEngine.UI;

public class GraphicConnector
{
	private static readonly List<GraphicConnector> s_Connectors = new List<GraphicConnector>();

	private static readonly Dictionary<Type, GraphicConnector> s_ConnectorMap = new Dictionary<Type, GraphicConnector>();

	private static readonly GraphicConnector s_EmptyConnector = new GraphicConnector();

	protected virtual int priority => -1;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init()
	{
		AddConnector(new GraphicConnector());
	}

	protected static void AddConnector(GraphicConnector connector)
	{
		s_Connectors.Add(connector);
		s_Connectors.Sort((GraphicConnector x, GraphicConnector y) => y.priority - x.priority);
	}

	public static GraphicConnector FindConnector(Graphic graphic)
	{
		if (!graphic)
		{
			return s_EmptyConnector;
		}
		Type type = graphic.GetType();
		GraphicConnector value = null;
		if (s_ConnectorMap.TryGetValue(type, out value))
		{
			return value;
		}
		foreach (GraphicConnector s_Connector in s_Connectors)
		{
			if (s_Connector.IsValid(graphic))
			{
				s_ConnectorMap.Add(type, s_Connector);
				return s_Connector;
			}
		}
		return s_EmptyConnector;
	}

	protected virtual bool IsValid(Graphic graphic)
	{
		return true;
	}

	public virtual void SetVerticesDirty(Graphic graphic)
	{
		if ((bool)graphic)
		{
			graphic.SetVerticesDirty();
		}
	}

	public virtual void SetMaterialDirty(Graphic graphic)
	{
		if ((bool)graphic)
		{
			graphic.SetMaterialDirty();
		}
	}
}
