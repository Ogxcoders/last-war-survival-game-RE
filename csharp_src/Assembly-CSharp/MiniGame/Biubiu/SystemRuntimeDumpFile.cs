using System;
using System.Collections.Generic;
using System.IO;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu;

public class SystemRuntimeDumpFile : SystemRuntimeDump, IEcsDestroySystem, IEcsSystem
{
	protected string _filePath;

	public SystemRuntimeDumpFile(string file, List<Type> incs, List<Type> excs, List<(IEcsInclude, IEcsExclude)> filters, int step = 20, int maxLen = int.MaxValue)
		: base(incs, excs, filters, step, maxLen)
	{
		_filePath = file;
	}

	public SystemRuntimeDumpFile(string file, List<Type> incs, List<Type> excs, (IEcsInclude, IEcsExclude) filter, int step = 20, int maxLen = int.MaxValue)
		: base(incs, excs, filter, step, maxLen)
	{
		_filePath = file;
	}

	public SystemRuntimeDumpFile(string file, List<Type> incs, List<Type> excs, (IEcsInclude, IEcsExclude) filter, List<Type> incsFrame0, List<Type> excsFrame0, (IEcsInclude, IEcsExclude) filterFrame0, int step = 20, int maxLen = int.MaxValue)
		: base(incs, excs, filter, incsFrame0, excsFrame0, filterFrame0, step, maxLen)
	{
		_filePath = file;
	}

	public SystemRuntimeDumpFile(string file, List<Type> incs, List<Type> excs, List<(IEcsInclude, IEcsExclude)> filters, List<Type> incsFrame0, List<Type> excsFrame0, List<(IEcsInclude, IEcsExclude)> filtersFrame0, int step = 20, int maxLen = int.MaxValue)
		: base(incs, excs, filters, incsFrame0, excsFrame0, filtersFrame0, step, maxLen)
	{
		_filePath = file;
	}

	public void Destroy(IEcsSystems systems)
	{
		File.WriteAllText(_filePath, GetDump().ToString());
	}
}
