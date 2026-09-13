using System.Collections.Generic;
using GameKit.Base;

public static class LuaClientProfiler
{
	private static int _sampleDepth;

	private static readonly Dictionary<int, string> _showNames = new Dictionary<int, string>();

	private static readonly Dictionary<int, string> _showNamesManual = new Dictionary<int, string>();

	private static bool _isAttached;

	public static bool LuaRuntimeSampleEnabled => ProfilerRuntime.Enabled;

	public static bool IsAttached => _isAttached;

	public static void BeginSampleById(int id, int luaAllocBytes)
	{
		_showNames.TryGetValue(id, out var value);
		value = value ?? string.Empty;
		if (luaAllocBytes > 0)
		{
			_ = new byte[luaAllocBytes];
		}
		_sampleDepth++;
	}

	public static void BeginSampleByIdName(int id, string name, int luaAllocBytes)
	{
		name = name ?? string.Empty;
		_showNames[id] = name;
		if (luaAllocBytes > 0)
		{
			_ = new byte[luaAllocBytes];
		}
		_sampleDepth++;
	}

	public static void EndSample(int luaAllocBytes)
	{
		if (_sampleDepth > 0)
		{
			if (luaAllocBytes > 0)
			{
				_ = new byte[luaAllocBytes];
			}
			_sampleDepth--;
		}
	}

	public static void LuaBeginSampleById(int id)
	{
		_showNamesManual.TryGetValue(id, out var value);
		value = value ?? string.Empty;
		_ = _isAttached;
	}

	public static void LuaBeginSampleByIdName(int id, string name)
	{
		name = name ?? string.Empty;
		_showNamesManual[id] = name;
		_ = _isAttached;
	}

	public static void LuaEndSample()
	{
		_ = _isAttached;
	}

	public static void LuaBeginRuntimeSampleById(int id)
	{
		_showNamesManual.TryGetValue(id, out var value);
		value = value ?? string.Empty;
		ProfilerRuntime.BeginSample(value);
	}

	public static void LuaBeginRuntimeSampleByIdName(int id, string name)
	{
		name = name ?? string.Empty;
		_showNamesManual[id] = name;
		ProfilerRuntime.BeginSample(name);
	}

	public static void LuaEndRuntimeSample()
	{
		ProfilerRuntime.EndSample();
	}

	public static void Attach()
	{
		if (!_isAttached && GameEntry.Lua != null)
		{
			_isAttached = true;
			string chunk = "\n        local profiler = CS.LuaClientProfiler\n        local debug = debug\n\n        local _cache = {}\n        local _id_generator = 0\n        local _ignore_count = 0\n        local _lastMem = collectgarbage('count')\n        local _mimicLuaAlloc = true -- 以在cs端分配内存的方式，显示lua的内存分配值\n\n        local function lua_profiler_hook (event, line)\n            local allocBytes = 0\n            if _mimicLuaAlloc then\n                allocBytes = math.max(0, collectgarbage('count') - _lastMem) * 1024\n                allocBytes = math.floor(allocBytes)\n            end\n            if event == 'call' then\n                local func = debug.getinfo (2, 'f').func\n                local id = _cache[func]\n\n                if id then\n                    profiler.BeginSampleById (id, allocBytes)\n                else\n                    local ar = debug.getinfo (2, 'Sn')\n                    local method_name = ar.name\n                    local linedefined = ar.linedefined\n\n                    if linedefined ~= -1 or (method_name and method_name ~= '__index')  then\n                        local short_src = ar.short_src\n                        method_name = method_name or '[unknown]'\n\n                        local index = short_src:match ('^.*()[/\\\\]')\n                        local filename  = index and short_src:sub (index + 1) or short_src\n                        local show_name = filename .. ':' .. method_name .. ' '.. linedefined\n\n                        local id = _id_generator + 1\n                        _id_generator = id\n                        _cache[func] = id\n\n                        profiler.BeginSampleByIdName (id, show_name, allocBytes)\n                    else\n                        _ignore_count = _ignore_count + 1\n                    end\n                end\n            elseif event == 'return' then\n                if _ignore_count == 0 then\n                    profiler.EndSample (allocBytes)\n                else\n                    _ignore_count = _ignore_count - 1\n                end\n            end\n            if _mimicLuaAlloc then\n                _lastMem = collectgarbage('count')\n            end\n        end\n\n        debug.sethook (lua_profiler_hook, 'cr', 0)\n        ";
			GameEntry.Lua.Env.DoString(chunk);
		}
	}

	public static void Detach()
	{
		if (_isAttached && GameEntry.Lua != null)
		{
			_isAttached = false;
			string chunk = "debug.sethook (nil)";
			GameEntry.Lua.Env.DoString(chunk);
		}
	}
}
