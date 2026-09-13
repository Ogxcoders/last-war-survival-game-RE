using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineNetworkingUnityWebRequestWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UnityWebRequest);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 22, 12);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -3, "SendWebRequest", _m_SendWebRequest);
		Utils.RegisterFunc(L, -3, "Abort", _m_Abort);
		Utils.RegisterFunc(L, -3, "GetRequestHeader", _m_GetRequestHeader);
		Utils.RegisterFunc(L, -3, "SetRequestHeader", _m_SetRequestHeader);
		Utils.RegisterFunc(L, -3, "GetResponseHeader", _m_GetResponseHeader);
		Utils.RegisterFunc(L, -3, "GetResponseHeaders", _m_GetResponseHeaders);
		Utils.RegisterFunc(L, -2, "disposeCertificateHandlerOnDispose", _g_get_disposeCertificateHandlerOnDispose);
		Utils.RegisterFunc(L, -2, "disposeDownloadHandlerOnDispose", _g_get_disposeDownloadHandlerOnDispose);
		Utils.RegisterFunc(L, -2, "disposeUploadHandlerOnDispose", _g_get_disposeUploadHandlerOnDispose);
		Utils.RegisterFunc(L, -2, "method", _g_get_method);
		Utils.RegisterFunc(L, -2, "error", _g_get_error);
		Utils.RegisterFunc(L, -2, "useHttpContinue", _g_get_useHttpContinue);
		Utils.RegisterFunc(L, -2, "url", _g_get_url);
		Utils.RegisterFunc(L, -2, "uri", _g_get_uri);
		Utils.RegisterFunc(L, -2, "responseCode", _g_get_responseCode);
		Utils.RegisterFunc(L, -2, "uploadProgress", _g_get_uploadProgress);
		Utils.RegisterFunc(L, -2, "isModifiable", _g_get_isModifiable);
		Utils.RegisterFunc(L, -2, "isDone", _g_get_isDone);
		Utils.RegisterFunc(L, -2, "isNetworkError", _g_get_isNetworkError);
		Utils.RegisterFunc(L, -2, "isHttpError", _g_get_isHttpError);
		Utils.RegisterFunc(L, -2, "downloadProgress", _g_get_downloadProgress);
		Utils.RegisterFunc(L, -2, "uploadedBytes", _g_get_uploadedBytes);
		Utils.RegisterFunc(L, -2, "downloadedBytes", _g_get_downloadedBytes);
		Utils.RegisterFunc(L, -2, "redirectLimit", _g_get_redirectLimit);
		Utils.RegisterFunc(L, -2, "uploadHandler", _g_get_uploadHandler);
		Utils.RegisterFunc(L, -2, "downloadHandler", _g_get_downloadHandler);
		Utils.RegisterFunc(L, -2, "certificateHandler", _g_get_certificateHandler);
		Utils.RegisterFunc(L, -2, "timeout", _g_get_timeout);
		Utils.RegisterFunc(L, -1, "disposeCertificateHandlerOnDispose", _s_set_disposeCertificateHandlerOnDispose);
		Utils.RegisterFunc(L, -1, "disposeDownloadHandlerOnDispose", _s_set_disposeDownloadHandlerOnDispose);
		Utils.RegisterFunc(L, -1, "disposeUploadHandlerOnDispose", _s_set_disposeUploadHandlerOnDispose);
		Utils.RegisterFunc(L, -1, "method", _s_set_method);
		Utils.RegisterFunc(L, -1, "useHttpContinue", _s_set_useHttpContinue);
		Utils.RegisterFunc(L, -1, "url", _s_set_url);
		Utils.RegisterFunc(L, -1, "uri", _s_set_uri);
		Utils.RegisterFunc(L, -1, "redirectLimit", _s_set_redirectLimit);
		Utils.RegisterFunc(L, -1, "uploadHandler", _s_set_uploadHandler);
		Utils.RegisterFunc(L, -1, "downloadHandler", _s_set_downloadHandler);
		Utils.RegisterFunc(L, -1, "certificateHandler", _s_set_certificateHandler);
		Utils.RegisterFunc(L, -1, "timeout", _s_set_timeout);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 18, 0, 0);
		Utils.RegisterFunc(L, -4, "ClearCookieCache", _m_ClearCookieCache_xlua_st_);
		Utils.RegisterFunc(L, -4, "Get", _m_Get_xlua_st_);
		Utils.RegisterFunc(L, -4, "Delete", _m_Delete_xlua_st_);
		Utils.RegisterFunc(L, -4, "Head", _m_Head_xlua_st_);
		Utils.RegisterFunc(L, -4, "Put", _m_Put_xlua_st_);
		Utils.RegisterFunc(L, -4, "Post", _m_Post_xlua_st_);
		Utils.RegisterFunc(L, -4, "EscapeURL", _m_EscapeURL_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnEscapeURL", _m_UnEscapeURL_xlua_st_);
		Utils.RegisterFunc(L, -4, "SerializeFormSections", _m_SerializeFormSections_xlua_st_);
		Utils.RegisterFunc(L, -4, "GenerateBoundary", _m_GenerateBoundary_xlua_st_);
		Utils.RegisterFunc(L, -4, "SerializeSimpleForm", _m_SerializeSimpleForm_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "kHttpVerbGET", "GET");
		Utils.RegisterObject(L, translator, -4, "kHttpVerbHEAD", "HEAD");
		Utils.RegisterObject(L, translator, -4, "kHttpVerbPOST", "POST");
		Utils.RegisterObject(L, translator, -4, "kHttpVerbPUT", "PUT");
		Utils.RegisterObject(L, translator, -4, "kHttpVerbCREATE", "CREATE");
		Utils.RegisterObject(L, translator, -4, "kHttpVerbDELETE", "DELETE");
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 1)
			{
				UnityWebRequest o = new UnityWebRequest();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				UnityWebRequest o2 = new UnityWebRequest(Lua.lua_tostring(L, 2));
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<Uri>(L, 2))
			{
				UnityWebRequest o3 = new UnityWebRequest((Uri)objectTranslator.GetObject(L, 2, typeof(Uri)));
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string url = Lua.lua_tostring(L, 2);
				string method = Lua.lua_tostring(L, 3);
				UnityWebRequest o4 = new UnityWebRequest(url, method);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && objectTranslator.Assignable<Uri>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				Uri uri = (Uri)objectTranslator.GetObject(L, 2, typeof(Uri));
				string method2 = Lua.lua_tostring(L, 3);
				UnityWebRequest o5 = new UnityWebRequest(uri, method2);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (Lua.lua_gettop(L) == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<DownloadHandler>(L, 4) && objectTranslator.Assignable<UploadHandler>(L, 5))
			{
				string url2 = Lua.lua_tostring(L, 2);
				string method3 = Lua.lua_tostring(L, 3);
				DownloadHandler downloadHandler = (DownloadHandler)objectTranslator.GetObject(L, 4, typeof(DownloadHandler));
				UploadHandler uploadHandler = (UploadHandler)objectTranslator.GetObject(L, 5, typeof(UploadHandler));
				UnityWebRequest o6 = new UnityWebRequest(url2, method3, downloadHandler, uploadHandler);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (Lua.lua_gettop(L) == 5 && objectTranslator.Assignable<Uri>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<DownloadHandler>(L, 4) && objectTranslator.Assignable<UploadHandler>(L, 5))
			{
				Uri uri2 = (Uri)objectTranslator.GetObject(L, 2, typeof(Uri));
				string method4 = Lua.lua_tostring(L, 3);
				DownloadHandler downloadHandler2 = (DownloadHandler)objectTranslator.GetObject(L, 4, typeof(DownloadHandler));
				UploadHandler uploadHandler2 = (UploadHandler)objectTranslator.GetObject(L, 5, typeof(UploadHandler));
				UnityWebRequest o7 = new UnityWebRequest(uri2, method4, downloadHandler2, uploadHandler2);
				objectTranslator.Push(L, o7);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Networking.UnityWebRequest constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearCookieCache_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			switch (Lua.lua_gettop(L))
			{
			case 0:
				UnityWebRequest.ClearCookieCache();
				return 0;
			case 1:
				if (objectTranslator.Assignable<Uri>(L, 1))
				{
					UnityWebRequest.ClearCookieCache((Uri)objectTranslator.GetObject(L, 1, typeof(Uri)));
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Networking.UnityWebRequest.ClearCookieCache!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dispose(IntPtr L)
	{
		try
		{
			((UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendWebRequest(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityWebRequestAsyncOperation o = ((UnityWebRequest)objectTranslator.FastGetCSObj(L, 1)).SendWebRequest();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Abort(IntPtr L)
	{
		try
		{
			((UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Abort();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRequestHeader(IntPtr L)
	{
		try
		{
			UnityWebRequest obj = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			string requestHeader = obj.GetRequestHeader(name);
			Lua.lua_pushstring(L, requestHeader);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRequestHeader(IntPtr L)
	{
		try
		{
			UnityWebRequest obj = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			string value = Lua.lua_tostring(L, 3);
			obj.SetRequestHeader(name, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetResponseHeader(IntPtr L)
	{
		try
		{
			UnityWebRequest obj = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			string responseHeader = obj.GetResponseHeader(name);
			Lua.lua_pushstring(L, responseHeader);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetResponseHeaders(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, string> responseHeaders = ((UnityWebRequest)objectTranslator.FastGetCSObj(L, 1)).GetResponseHeaders();
			objectTranslator.Push(L, responseHeaders);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				UnityWebRequest o = UnityWebRequest.Get(Lua.lua_tostring(L, 1));
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<Uri>(L, 1))
			{
				UnityWebRequest o2 = UnityWebRequest.Get((Uri)objectTranslator.GetObject(L, 1, typeof(Uri)));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Networking.UnityWebRequest.Get!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Delete_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				UnityWebRequest o = UnityWebRequest.Delete(Lua.lua_tostring(L, 1));
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<Uri>(L, 1))
			{
				UnityWebRequest o2 = UnityWebRequest.Delete((Uri)objectTranslator.GetObject(L, 1, typeof(Uri)));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Networking.UnityWebRequest.Delete!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Head_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				UnityWebRequest o = UnityWebRequest.Head(Lua.lua_tostring(L, 1));
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<Uri>(L, 1))
			{
				UnityWebRequest o2 = UnityWebRequest.Head((Uri)objectTranslator.GetObject(L, 1, typeof(Uri)));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Networking.UnityWebRequest.Head!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Put_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string uri = Lua.lua_tostring(L, 1);
				byte[] bodyData = Lua.lua_tobytes(L, 2);
				UnityWebRequest o = UnityWebRequest.Put(uri, bodyData);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Uri>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				Uri uri2 = (Uri)objectTranslator.GetObject(L, 1, typeof(Uri));
				byte[] bodyData2 = Lua.lua_tobytes(L, 2);
				UnityWebRequest o2 = UnityWebRequest.Put(uri2, bodyData2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string uri3 = Lua.lua_tostring(L, 1);
				string bodyData3 = Lua.lua_tostring(L, 2);
				UnityWebRequest o3 = UnityWebRequest.Put(uri3, bodyData3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Uri>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				Uri uri4 = (Uri)objectTranslator.GetObject(L, 1, typeof(Uri));
				string bodyData4 = Lua.lua_tostring(L, 2);
				UnityWebRequest o4 = UnityWebRequest.Put(uri4, bodyData4);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Networking.UnityWebRequest.Put!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Post_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string uri = Lua.lua_tostring(L, 1);
				string postData = Lua.lua_tostring(L, 2);
				UnityWebRequest o = UnityWebRequest.Post(uri, postData);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Uri>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				Uri uri2 = (Uri)objectTranslator.GetObject(L, 1, typeof(Uri));
				string postData2 = Lua.lua_tostring(L, 2);
				UnityWebRequest o2 = UnityWebRequest.Post(uri2, postData2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<WWWForm>(L, 2))
			{
				string uri3 = Lua.lua_tostring(L, 1);
				WWWForm formData = (WWWForm)objectTranslator.GetObject(L, 2, typeof(WWWForm));
				UnityWebRequest o3 = UnityWebRequest.Post(uri3, formData);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Uri>(L, 1) && objectTranslator.Assignable<WWWForm>(L, 2))
			{
				Uri uri4 = (Uri)objectTranslator.GetObject(L, 1, typeof(Uri));
				WWWForm formData2 = (WWWForm)objectTranslator.GetObject(L, 2, typeof(WWWForm));
				UnityWebRequest o4 = UnityWebRequest.Post(uri4, formData2);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<IMultipartFormSection>>(L, 2))
			{
				string uri5 = Lua.lua_tostring(L, 1);
				List<IMultipartFormSection> multipartFormSections = (List<IMultipartFormSection>)objectTranslator.GetObject(L, 2, typeof(List<IMultipartFormSection>));
				UnityWebRequest o5 = UnityWebRequest.Post(uri5, multipartFormSections);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Uri>(L, 1) && objectTranslator.Assignable<List<IMultipartFormSection>>(L, 2))
			{
				Uri uri6 = (Uri)objectTranslator.GetObject(L, 1, typeof(Uri));
				List<IMultipartFormSection> multipartFormSections2 = (List<IMultipartFormSection>)objectTranslator.GetObject(L, 2, typeof(List<IMultipartFormSection>));
				UnityWebRequest o6 = UnityWebRequest.Post(uri6, multipartFormSections2);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Dictionary<string, string>>(L, 2))
			{
				string uri7 = Lua.lua_tostring(L, 1);
				Dictionary<string, string> formFields = (Dictionary<string, string>)objectTranslator.GetObject(L, 2, typeof(Dictionary<string, string>));
				UnityWebRequest o7 = UnityWebRequest.Post(uri7, formFields);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Uri>(L, 1) && objectTranslator.Assignable<Dictionary<string, string>>(L, 2))
			{
				Uri uri8 = (Uri)objectTranslator.GetObject(L, 1, typeof(Uri));
				Dictionary<string, string> formFields2 = (Dictionary<string, string>)objectTranslator.GetObject(L, 2, typeof(Dictionary<string, string>));
				UnityWebRequest o8 = UnityWebRequest.Post(uri8, formFields2);
				objectTranslator.Push(L, o8);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<List<IMultipartFormSection>>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string uri9 = Lua.lua_tostring(L, 1);
				List<IMultipartFormSection> multipartFormSections3 = (List<IMultipartFormSection>)objectTranslator.GetObject(L, 2, typeof(List<IMultipartFormSection>));
				byte[] boundary = Lua.lua_tobytes(L, 3);
				UnityWebRequest o9 = UnityWebRequest.Post(uri9, multipartFormSections3, boundary);
				objectTranslator.Push(L, o9);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Uri>(L, 1) && objectTranslator.Assignable<List<IMultipartFormSection>>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				Uri uri10 = (Uri)objectTranslator.GetObject(L, 1, typeof(Uri));
				List<IMultipartFormSection> multipartFormSections4 = (List<IMultipartFormSection>)objectTranslator.GetObject(L, 2, typeof(List<IMultipartFormSection>));
				byte[] boundary2 = Lua.lua_tobytes(L, 3);
				UnityWebRequest o10 = UnityWebRequest.Post(uri10, multipartFormSections4, boundary2);
				objectTranslator.Push(L, o10);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Networking.UnityWebRequest.Post!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EscapeURL_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				string str = UnityWebRequest.EscapeURL(Lua.lua_tostring(L, 1));
				Lua.lua_pushstring(L, str);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Encoding>(L, 2))
			{
				string s = Lua.lua_tostring(L, 1);
				Encoding e = (Encoding)objectTranslator.GetObject(L, 2, typeof(Encoding));
				string str2 = UnityWebRequest.EscapeURL(s, e);
				Lua.lua_pushstring(L, str2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Networking.UnityWebRequest.EscapeURL!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnEscapeURL_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				string str = UnityWebRequest.UnEscapeURL(Lua.lua_tostring(L, 1));
				Lua.lua_pushstring(L, str);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Encoding>(L, 2))
			{
				string s = Lua.lua_tostring(L, 1);
				Encoding e = (Encoding)objectTranslator.GetObject(L, 2, typeof(Encoding));
				string str2 = UnityWebRequest.UnEscapeURL(s, e);
				Lua.lua_pushstring(L, str2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Networking.UnityWebRequest.UnEscapeURL!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SerializeFormSections_xlua_st_(IntPtr L)
	{
		try
		{
			List<IMultipartFormSection> multipartFormSections = (List<IMultipartFormSection>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(List<IMultipartFormSection>));
			byte[] boundary = Lua.lua_tobytes(L, 2);
			byte[] str = UnityWebRequest.SerializeFormSections(multipartFormSections, boundary);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GenerateBoundary_xlua_st_(IntPtr L)
	{
		try
		{
			byte[] str = UnityWebRequest.GenerateBoundary();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SerializeSimpleForm_xlua_st_(IntPtr L)
	{
		try
		{
			byte[] str = UnityWebRequest.SerializeSimpleForm((Dictionary<string, string>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Dictionary<string, string>)));
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_disposeCertificateHandlerOnDispose(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, unityWebRequest.disposeCertificateHandlerOnDispose);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_disposeDownloadHandlerOnDispose(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, unityWebRequest.disposeDownloadHandlerOnDispose);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_disposeUploadHandlerOnDispose(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, unityWebRequest.disposeUploadHandlerOnDispose);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_method(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, unityWebRequest.method);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_error(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, unityWebRequest.error);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useHttpContinue(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, unityWebRequest.useHttpContinue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_url(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, unityWebRequest.url);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uri(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityWebRequest unityWebRequest = (UnityWebRequest)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, unityWebRequest.uri);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_responseCode(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, unityWebRequest.responseCode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uploadProgress(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, unityWebRequest.uploadProgress);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isModifiable(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, unityWebRequest.isModifiable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isDone(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, unityWebRequest.isDone);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isNetworkError(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, unityWebRequest.isNetworkError);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isHttpError(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, unityWebRequest.isHttpError);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_downloadProgress(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, unityWebRequest.downloadProgress);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uploadedBytes(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushuint64(L, unityWebRequest.uploadedBytes);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_downloadedBytes(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushuint64(L, unityWebRequest.downloadedBytes);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_redirectLimit(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, unityWebRequest.redirectLimit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uploadHandler(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityWebRequest unityWebRequest = (UnityWebRequest)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, unityWebRequest.uploadHandler);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_downloadHandler(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityWebRequest unityWebRequest = (UnityWebRequest)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, unityWebRequest.downloadHandler);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_certificateHandler(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityWebRequest unityWebRequest = (UnityWebRequest)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, unityWebRequest.certificateHandler);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_timeout(IntPtr L)
	{
		try
		{
			UnityWebRequest unityWebRequest = (UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, unityWebRequest.timeout);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_disposeCertificateHandlerOnDispose(IntPtr L)
	{
		try
		{
			((UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).disposeCertificateHandlerOnDispose = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_disposeDownloadHandlerOnDispose(IntPtr L)
	{
		try
		{
			((UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).disposeDownloadHandlerOnDispose = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_disposeUploadHandlerOnDispose(IntPtr L)
	{
		try
		{
			((UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).disposeUploadHandlerOnDispose = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_method(IntPtr L)
	{
		try
		{
			((UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).method = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useHttpContinue(IntPtr L)
	{
		try
		{
			((UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useHttpContinue = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_url(IntPtr L)
	{
		try
		{
			((UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).url = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uri(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UnityWebRequest)objectTranslator.FastGetCSObj(L, 1)).uri = (Uri)objectTranslator.GetObject(L, 2, typeof(Uri));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_redirectLimit(IntPtr L)
	{
		try
		{
			((UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).redirectLimit = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uploadHandler(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UnityWebRequest)objectTranslator.FastGetCSObj(L, 1)).uploadHandler = (UploadHandler)objectTranslator.GetObject(L, 2, typeof(UploadHandler));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_downloadHandler(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UnityWebRequest)objectTranslator.FastGetCSObj(L, 1)).downloadHandler = (DownloadHandler)objectTranslator.GetObject(L, 2, typeof(DownloadHandler));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_certificateHandler(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UnityWebRequest)objectTranslator.FastGetCSObj(L, 1)).certificateHandler = (CertificateHandler)objectTranslator.GetObject(L, 2, typeof(CertificateHandler));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_timeout(IntPtr L)
	{
		try
		{
			((UnityWebRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).timeout = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
