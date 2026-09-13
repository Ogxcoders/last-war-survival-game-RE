using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using XLua.LuaDLL;
using XLua.TemplateEngine;

namespace XLua;

public class LuaEnv : IDisposable
{
	internal struct GCAction
	{
		public int Reference;

		public bool IsDelegate;
	}

	public delegate byte[] CustomLoader(ref string filepath);

	public const string CSHARP_NAMESPACE = "xlua_csharp_namespace";

	public const string MAIN_SHREAD = "xlua_main_thread";

	internal IntPtr rawL;

	private LuaTable _G;

	internal ObjectTranslator translator;

	internal int errorFuncRef = -1;

	private const int LIB_VERSION_EXPECT = 105;

	private static List<Action<LuaEnv, ObjectTranslator>> initers;

	private int last_check_point;

	private int max_check_per_tick = 20;

	private Func<object, bool> object_valid_checker = ObjectValidCheck;

	private bool disposed;

	public Action beforeDispose;

	private Queue<GCAction> refQueue = new Queue<GCAction>();

	private string init_xlua = " \n            local metatable = {}\n            local rawget = rawget\n            local setmetatable = setmetatable\n            local import_type = xlua.import_type\n            local import_generic_type = xlua.import_generic_type\n            local load_assembly = xlua.load_assembly\n\n            function metatable:__index(key) \n                local fqn = rawget(self,'.fqn')\n                fqn = ((fqn and fqn .. '.') or '') .. key\n\n                local obj = import_type(fqn)\n\n                if obj == nil then\n                    -- It might be an assembly, so we load it too.\n                    obj = { ['.fqn'] = fqn }\n                    setmetatable(obj, metatable)\n                elseif obj == true then\n                    return rawget(self, key)\n                end\n\n                -- Cache this lookup\n                rawset(self, key, obj)\n                return obj\n            end\n\n            function metatable:__newindex()\n                error('No such type: ' .. rawget(self,'.fqn'), 2)\n            end\n\n            -- A non-type has been called; e.g. foo = System.Foo()\n            function metatable:__call(...)\n                local n = select('#', ...)\n                local fqn = rawget(self,'.fqn')\n                if n > 0 then\n                    local gt = import_generic_type(fqn, ...)\n                    if gt then\n                        return rawget(CS, gt)\n                    end\n                end\n                error('No such type: ' .. fqn, 2)\n            end\n\n            CS = CS or {}\n            setmetatable(CS, metatable)\n\n            typeof = function(t) return t.UnderlyingSystemType end\n            cast = xlua.cast\n            if not setfenv or not getfenv then\n                local function getfunction(level)\n                    local info = debug.getinfo(level + 1, 'f')\n                    return info and info.func\n                end\n\n                function setfenv(fn, env)\n                  if type(fn) == 'number' then fn = getfunction(fn + 1) end\n                  local i = 1\n                  while true do\n                    local name = debug.getupvalue(fn, i)\n                    if name == '_ENV' then\n                      debug.upvaluejoin(fn, i, (function()\n                        return env\n                      end), 1)\n                      break\n                    elseif not name then\n                      break\n                    end\n\n                    i = i + 1\n                  end\n\n                  return fn\n                end\n\n                function getfenv(fn)\n                  if type(fn) == 'number' then fn = getfunction(fn + 1) end\n                  local i = 1\n                  while true do\n                    local name, val = debug.getupvalue(fn, i)\n                    if name == '_ENV' then\n                      return val\n                    elseif not name then\n                      break\n                    end\n                    i = i + 1\n                  end\n                end\n            end\n\n            xlua.hotfix = function(cs, field, func)\n                if func == nil then func = false end\n                local tbl = (type(field) == 'table') and field or {[field] = func}\n                for k, v in pairs(tbl) do\n                    local cflag = ''\n                    if k == '.ctor' then\n                        cflag = '_c'\n                        k = 'ctor'\n                    end\n                    local f = type(v) == 'function' and v or nil\n                    xlua.access(cs, cflag .. '__Hotfix0_'..k, f) -- at least one\n                    pcall(function()\n                        for i = 1, 99 do\n                            xlua.access(cs, cflag .. '__Hotfix'..i..'_'..k, f)\n                        end\n                    end)\n                end\n                xlua.private_accessible(cs)\n            end\n            xlua.getmetatable = function(cs)\n                return xlua.metatable_operation(cs)\n            end\n            xlua.setmetatable = function(cs, mt)\n                return xlua.metatable_operation(cs, mt)\n            end\n            xlua.setclass = function(parent, name, impl)\n                impl.UnderlyingSystemType = parent[name].UnderlyingSystemType\n                rawset(parent, name, impl)\n            end\n            \n            local base_mt = {\n                __index = function(t, k)\n                    local csobj = t['__csobj']\n                    local func = csobj['<>xLuaBaseProxy_'..k]\n                    return function(_, ...)\n                         return func(csobj, ...)\n                    end\n                end\n            }\n            base = function(csobj)\n                return setmetatable({__csobj = csobj}, base_mt)\n            end\n            ";

	internal List<CustomLoader> customLoaders = new List<CustomLoader>();

	internal Dictionary<string, lua_CSFunction> buildin_initer = new Dictionary<string, lua_CSFunction>();

	internal IntPtr L
	{
		get
		{
			if (rawL == IntPtr.Zero)
			{
				throw new InvalidOperationException("this lua env had disposed!");
			}
			return rawL;
		}
	}

	public LuaTable Global => _G;

	public int GcPause
	{
		get
		{
			int num = Lua.lua_gc(L, LuaGCOptions.LUA_GCSETPAUSE, 200);
			Lua.lua_gc(L, LuaGCOptions.LUA_GCSETPAUSE, num);
			return num;
		}
		set
		{
			Lua.lua_gc(L, LuaGCOptions.LUA_GCSETPAUSE, value);
		}
	}

	public int GcStepmul
	{
		get
		{
			int num = Lua.lua_gc(L, LuaGCOptions.LUA_GCSETSTEPMUL, 200);
			Lua.lua_gc(L, LuaGCOptions.LUA_GCSETSTEPMUL, num);
			return num;
		}
		set
		{
			Lua.lua_gc(L, LuaGCOptions.LUA_GCSETSTEPMUL, value);
		}
	}

	public int Memroy => Lua.lua_gc(L, LuaGCOptions.LUA_GCCOUNT, 0);

	public LuaEnv()
	{
		Lua.InternalGlobals.GetStrBuff = () => InternalGlobals.strBuff;
		Lua.InternalGlobals.SetStrBuff = delegate(byte[] v)
		{
			InternalGlobals.strBuff = v;
		};
		Lua.LuaIndexes.GetLUA_REGISTRYINDEX = () => LuaIndexes.LUA_REGISTRYINDEX;
		if (Lua.xlua_get_lib_version() != 105)
		{
			throw new InvalidProgramException("wrong lib version expect:" + 105 + " but got:" + Lua.xlua_get_lib_version());
		}
		LuaIndexes.LUA_REGISTRYINDEX = Lua.xlua_get_registry_index();
		rawL = Lua.luaL_newstate();
		Lua.luaopen_xlua(rawL);
		Lua.luaopen_i64lib(rawL);
		Lua.LoadStringExt(rawL);
		try
		{
			Lua.LoadLuaCNativeCode(rawL);
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
		}
		translator = new ObjectTranslator(this, rawL);
		translator.createFunctionMetatable(rawL);
		translator.OpenLib(rawL);
		ObjectTranslatorPool.Instance.Add(rawL, translator);
		Lua.lua_atpanic(rawL, StaticLuaCallbacks.Panic);
		Lua.lua_pushstdcallcfunction(rawL, StaticLuaCallbacks.Print);
		if (Lua.xlua_setglobal(rawL, "print") != 0)
		{
			throw new Exception("call xlua_setglobal fail!");
		}
		LuaTemplate.OpenLib(rawL);
		AddSearcher(StaticLuaCallbacks.LoadBuiltinLib, 2);
		AddSearcher(StaticLuaCallbacks.LoadFromCustomLoaders, 3);
		AddSearcher(StaticLuaCallbacks.LoadFromResource, 4);
		AddSearcher(StaticLuaCallbacks.LoadFromStreamingAssetsPath, -1);
		DoString(init_xlua, "Init");
		init_xlua = null;
		AddBuildin("socket.core", StaticLuaCallbacks.LoadSocketCore);
		AddBuildin("socket", StaticLuaCallbacks.LoadSocketCore);
		AddBuildin("CS", StaticLuaCallbacks.LoadCS);
		Lua.lua_newtable(rawL);
		Lua.xlua_pushasciistring(rawL, "__index");
		Lua.lua_pushstdcallcfunction(rawL, StaticLuaCallbacks.MetaFuncIndex);
		Lua.lua_rawset(rawL, -3);
		Lua.xlua_pushasciistring(rawL, "LuaIndexs");
		Lua.lua_newtable(rawL);
		Lua.lua_pushvalue(rawL, -3);
		Lua.lua_setmetatable(rawL, -2);
		Lua.lua_rawset(rawL, LuaIndexes.LUA_REGISTRYINDEX);
		Lua.xlua_pushasciistring(rawL, "LuaNewIndexs");
		Lua.lua_newtable(rawL);
		Lua.lua_pushvalue(rawL, -3);
		Lua.lua_setmetatable(rawL, -2);
		Lua.lua_rawset(rawL, LuaIndexes.LUA_REGISTRYINDEX);
		Lua.xlua_pushasciistring(rawL, "LuaClassIndexs");
		Lua.lua_newtable(rawL);
		Lua.lua_pushvalue(rawL, -3);
		Lua.lua_setmetatable(rawL, -2);
		Lua.lua_rawset(rawL, LuaIndexes.LUA_REGISTRYINDEX);
		Lua.xlua_pushasciistring(rawL, "LuaClassNewIndexs");
		Lua.lua_newtable(rawL);
		Lua.lua_pushvalue(rawL, -3);
		Lua.lua_setmetatable(rawL, -2);
		Lua.lua_rawset(rawL, LuaIndexes.LUA_REGISTRYINDEX);
		Lua.lua_pop(rawL, 1);
		Lua.xlua_pushasciistring(rawL, "xlua_main_thread");
		Lua.lua_pushthread(rawL);
		Lua.lua_rawset(rawL, LuaIndexes.LUA_REGISTRYINDEX);
		Lua.xlua_pushasciistring(rawL, "xlua_csharp_namespace");
		if (Lua.xlua_getglobal(rawL, "CS") != 0)
		{
			throw new Exception("get CS fail!");
		}
		Lua.lua_rawset(rawL, LuaIndexes.LUA_REGISTRYINDEX);
		translator.Alias(typeof(Type), "System.MonoType");
		if (Lua.xlua_getglobal(rawL, "_G") != 0)
		{
			throw new Exception("get _G fail!");
		}
		translator.Get(rawL, -1, out _G);
		Lua.lua_pop(rawL, 1);
		errorFuncRef = Lua.get_error_func_ref(rawL);
		if (initers != null)
		{
			for (int num = 0; num < initers.Count; num++)
			{
				initers[num](this, translator);
			}
		}
		translator.CreateArrayMetatable(rawL);
		translator.CreateDelegateMetatable(rawL);
		translator.CreateEnumerablePairs(rawL);
	}

	public static void AddIniter(Action<LuaEnv, ObjectTranslator> initer)
	{
		if (initers == null)
		{
			initers = new List<Action<LuaEnv, ObjectTranslator>>();
		}
		initers.Add(initer);
	}

	public T LoadString<T>(byte[] chunk, string chunkName = "chunk", LuaTable env = null)
	{
		if (typeof(T) != typeof(LuaFunction) && !typeof(T).IsSubclassOf(typeof(Delegate)))
		{
			throw new InvalidOperationException(typeof(T).Name + " is not a delegate type nor LuaFunction");
		}
		IntPtr l = L;
		int num = Lua.lua_gettop(l);
		if (Lua.xluaL_loadbuffer(l, chunk, chunk.Length, chunkName) != 0)
		{
			ThrowExceptionFromError(num);
		}
		if (env != null)
		{
			env.push(l);
			Lua.lua_setfenv(l, -2);
		}
		T result = (T)translator.GetObject(l, -1, typeof(T));
		Lua.lua_settop(l, num);
		return result;
	}

	public T LoadString<T>(string chunk, string chunkName = "chunk", LuaTable env = null)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(chunk);
		return LoadString<T>(bytes, chunkName, env);
	}

	public LuaFunction LoadString(string chunk, string chunkName = "chunk", LuaTable env = null)
	{
		return LoadString<LuaFunction>(chunk, chunkName, env);
	}

	public object[] DoString(byte[] chunk, string chunkName = "chunk", LuaTable env = null)
	{
		IntPtr l = L;
		int oldTop = Lua.lua_gettop(l);
		int num = Lua.load_error_func(l, errorFuncRef);
		if (Lua.xluaL_loadbuffer(l, chunk, chunk.Length, chunkName) == 0)
		{
			if (env != null)
			{
				env.push(l);
				Lua.lua_setfenv(l, -2);
			}
			if (Lua.lua_pcall(l, 0, -1, num) == 0)
			{
				Lua.lua_remove(l, num);
				return translator.popValues(l, oldTop);
			}
			ThrowExceptionFromError(oldTop);
		}
		else
		{
			ThrowExceptionFromError(oldTop);
		}
		return null;
	}

	public object[] DoString(string chunk, string chunkName = "chunk", LuaTable env = null)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(chunk);
		return DoString(bytes, chunkName, env);
	}

	private void AddSearcher(lua_CSFunction searcher, int index)
	{
		IntPtr l = L;
		Lua.xlua_getloaders(l);
		if (!Lua.lua_istable(l, -1))
		{
			throw new Exception("Can not set searcher!");
		}
		uint num = Lua.xlua_objlen(l, -1);
		index = (int)((index < 0) ? (num + index + 2) : index);
		for (int num2 = (int)(num + 1); num2 > index; num2--)
		{
			Lua.xlua_rawgeti(l, -1, num2 - 1);
			Lua.xlua_rawseti(l, -2, num2);
		}
		Lua.lua_pushstdcallcfunction(l, searcher);
		Lua.xlua_rawseti(l, -2, index);
		Lua.lua_pop(l, 1);
	}

	public void Alias(Type type, string alias)
	{
		translator.Alias(type, alias);
	}

	private static bool ObjectValidCheck(object obj)
	{
		if (obj is UnityEngine.Object)
		{
			return obj as UnityEngine.Object != null;
		}
		return true;
	}

	public void Tick()
	{
		IntPtr l = L;
		lock (refQueue)
		{
			while (refQueue.Count > 0)
			{
				GCAction gCAction = refQueue.Dequeue();
				translator.ReleaseLuaBase(l, gCAction.Reference, gCAction.IsDelegate);
			}
		}
		last_check_point = translator.objects.Check(last_check_point, max_check_per_tick, object_valid_checker, translator.reverseMap);
	}

	public void GC()
	{
		Tick();
	}

	public LuaTable NewTable()
	{
		IntPtr l = L;
		int newTop = Lua.lua_gettop(l);
		Lua.lua_newtable(l);
		LuaTable result = (LuaTable)translator.GetObject(l, -1, typeof(LuaTable));
		Lua.lua_settop(l, newTop);
		return result;
	}

	public void Dispose()
	{
		if (beforeDispose != null)
		{
			Delegate[] invocationList = beforeDispose.GetInvocationList();
			foreach (Delegate obj in invocationList)
			{
				try
				{
					((Action)obj)();
				}
				catch (Exception arg)
				{
					Log.Error($"Exception in luaenv beforeDispose {arg}");
				}
			}
			beforeDispose = null;
		}
		FullGc();
		System.GC.Collect();
		System.GC.WaitForPendingFinalizers();
		Dispose(dispose: true);
		System.GC.Collect();
		System.GC.WaitForPendingFinalizers();
	}

	public virtual void Dispose(bool dispose)
	{
		if (!disposed)
		{
			Tick();
			if (!translator.AllDelegateBridgeReleased())
			{
				throw new InvalidOperationException("try to dispose a LuaEnv with C# callback!");
			}
			ObjectTranslatorPool.Instance.Remove(L);
			Lua.lua_close(L);
			translator = null;
			rawL = IntPtr.Zero;
			disposed = true;
		}
	}

	public void ThrowExceptionFromError(int oldTop)
	{
		object obj = translator.GetObject(L, -1);
		Lua.lua_settop(L, oldTop);
		if (obj is Exception ex)
		{
			throw ex;
		}
		if (obj == null)
		{
			obj = "Unknown Lua Error";
		}
		throw new LuaException(obj.ToString());
	}

	internal void equeueGCAction(GCAction action)
	{
		lock (refQueue)
		{
			refQueue.Enqueue(action);
		}
	}

	public void AddLoader(CustomLoader loader)
	{
		customLoaders.Add(loader);
	}

	public void AddBuildin(string name, lua_CSFunction initer)
	{
		if (!Utils.IsStaticPInvokeCSFunction(initer))
		{
			throw new Exception("initer must be static and has MonoPInvokeCallback Attribute!");
		}
		buildin_initer.Add(name, initer);
	}

	public void FullGc()
	{
		Lua.lua_gc(L, LuaGCOptions.LUA_GCCOLLECT, 0);
	}

	public void StopGc()
	{
		Lua.lua_gc(L, LuaGCOptions.LUA_GCSTOP, 0);
	}

	public void RestartGc()
	{
		Lua.lua_gc(L, LuaGCOptions.LUA_GCRESTART, 0);
	}

	public bool GcStep(int data)
	{
		return Lua.lua_gc(L, LuaGCOptions.LUA_GCSTEP, data) != 0;
	}
}
