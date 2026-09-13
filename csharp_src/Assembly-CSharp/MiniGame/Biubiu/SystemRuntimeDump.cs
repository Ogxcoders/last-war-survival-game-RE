using System;
using System.Collections.Generic;
using System.Text;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public class SystemRuntimeDump : IEcsPreInitSystem, IEcsSystem, IEcsRunSystem
{
	protected EcsWorldInject _world;

	protected EcsSharedInject<IGameSharedEnv> _env;

	protected StringBuilder _builder = new StringBuilder();

	protected int _step;

	protected int _maxLength;

	protected List<Type> _incs;

	protected List<Type> _excs;

	protected List<(IEcsInclude, IEcsExclude)> _filters;

	protected List<EcsFilter> _worldFilters;

	protected List<Type> _incsFrame0;

	protected List<Type> _excsFrame0;

	protected List<(IEcsInclude, IEcsExclude)> _filtersFrame0;

	protected List<EcsFilter> _worldFiltersFrame0;

	public SystemRuntimeDump(List<Type> incs, List<Type> excs, List<(IEcsInclude, IEcsExclude)> filters, int step = 20, int maxLen = int.MaxValue)
		: this(incs, excs, filters, incs, excs, filters, step, maxLen)
	{
	}

	public SystemRuntimeDump(List<Type> incs, List<Type> excs, (IEcsInclude, IEcsExclude) filter, int step = 20, int maxLen = int.MaxValue)
		: this(incs, excs, new List<(IEcsInclude, IEcsExclude)> { filter }, step, maxLen)
	{
	}

	public SystemRuntimeDump(List<Type> incs, List<Type> excs, (IEcsInclude, IEcsExclude) filter, List<Type> incsFrame0, List<Type> excsFrame0, (IEcsInclude, IEcsExclude) filterFrame0, int step = 20, int maxLen = int.MaxValue)
		: this(incs, excs, new List<(IEcsInclude, IEcsExclude)> { filter }, incsFrame0, excsFrame0, new List<(IEcsInclude, IEcsExclude)> { filterFrame0 }, step, maxLen)
	{
	}

	public SystemRuntimeDump(List<Type> incs, List<Type> excs, List<(IEcsInclude, IEcsExclude)> filters, List<Type> incsFrame0, List<Type> excsFrame0, List<(IEcsInclude, IEcsExclude)> filtersFrame0, int step = 20, int maxLen = int.MaxValue)
	{
		_step = Math.Max(1, step);
		_maxLength = maxLen;
		_incs = incs;
		_excs = excs;
		_filters = filters;
		_incsFrame0 = incsFrame0;
		_excsFrame0 = excsFrame0;
		_filtersFrame0 = filtersFrame0;
	}

	public void PreInit(IEcsSystems systems)
	{
		EcsWorld world = systems.GetWorld();
		_worldFilters = InitFilters(world, _filters);
		_worldFiltersFrame0 = InitFilters(world, _filtersFrame0);
	}

	public void Run(IEcsSystems systems)
	{
		if (_builder.Length > _maxLength)
		{
			return;
		}
		int logicTickCount = _env.Value.LogicTickCount;
		EcsAliveEntitiesSnapshot ecsAliveEntitiesSnapshot = null;
		if (logicTickCount == 0)
		{
			ecsAliveEntitiesSnapshot = _world.Value.TakeEntitiesStates(_incsFrame0, _excsFrame0, _worldFiltersFrame0);
		}
		else
		{
			if (logicTickCount % _step != 0)
			{
				return;
			}
			ecsAliveEntitiesSnapshot = _world.Value.TakeEntitiesStates(_incs, _excs, _worldFilters);
		}
		_builder.Append($"logic:{_env.Value.LogicTickCount} physics:{_env.Value.PhysicsTickCount}\r\n");
		string value = _env.Value.ResourceLoader.GetSerializer().ToJson(ecsAliveEntitiesSnapshot);
		_builder.Append(value);
	}

	public StringBuilder GetDump()
	{
		return _builder;
	}

	protected static List<EcsFilter> InitFilters(EcsWorld world, List<(IEcsInclude, IEcsExclude)> filters)
	{
		List<EcsFilter> list = null;
		if (filters != null && filters.Count > 0)
		{
			list = new List<EcsFilter>();
			for (int i = 0; i < filters.Count; i++)
			{
				(IEcsInclude, IEcsExclude) tuple = filters[i];
				if (tuple.Item2 == null)
				{
					list.Add(tuple.Item1.Fill(world).End());
				}
				else
				{
					list.Add(tuple.Item2.Fill(tuple.Item1.Fill(world)).End());
				}
			}
		}
		return list;
	}
}
