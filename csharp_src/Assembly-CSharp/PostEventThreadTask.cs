using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using GameFramework;

internal class PostEventThreadTask
{
	private object requestLock = new object();

	private WebRequest webrequest;

	private ManualResetEvent allDone;

	private byte[] content;

	private int contentLen;

	private string uri;

	public string param;

	public PostEventThreadTask(string para)
	{
		param = para;
		allDone = new ManualResetEvent(initialState: false);
		webrequest = null;
	}

	public void Abort()
	{
		try
		{
			lock (requestLock)
			{
				if (webrequest != null)
				{
					webrequest.Abort();
					webrequest = null;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Info("Abort excep " + ex.Message);
		}
		finally
		{
			allDone.Set();
		}
	}

	public void BeginProcess(byte[] buffer)
	{
		try
		{
			if (string.IsNullOrEmpty(param))
			{
				allDone.Set();
				return;
			}
			content = buffer;
			contentLen = Encoding.UTF8.GetBytes(param, 0, param.Length, content, 0);
			uri = "";
			WebRequest webRequest = WebRequest.Create(uri);
			webRequest.Timeout = 10000;
			webRequest.Method = "POST";
			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.ContentLength = contentLen;
			webRequest.BeginGetRequestStream(GetRequestStreamCallback, this);
			lock (requestLock)
			{
				webrequest = webRequest;
			}
		}
		catch (Exception ex)
		{
			allDone.Set();
			Log.Info("Process excep " + ex.Message);
		}
	}

	public void WaitProcessDone()
	{
		try
		{
			allDone.WaitOne();
		}
		catch (Exception)
		{
		}
	}

	private static void GetRequestStreamCallback(IAsyncResult asynchronousResult)
	{
		Stream stream = null;
		PostEventThreadTask postEventThreadTask = null;
		try
		{
			postEventThreadTask = (PostEventThreadTask)asynchronousResult.AsyncState;
			WebRequest webRequest;
			lock (postEventThreadTask.requestLock)
			{
				webRequest = postEventThreadTask.webrequest;
			}
			stream = webRequest.EndGetRequestStream(asynchronousResult);
			stream.Write(postEventThreadTask.content, 0, postEventThreadTask.contentLen);
			stream.Flush();
			webRequest.BeginGetResponse(GetResponseCallback, postEventThreadTask);
		}
		catch (Exception)
		{
			postEventThreadTask?.allDone.Set();
		}
		finally
		{
			stream?.Close();
		}
	}

	private static void GetResponseCallback(IAsyncResult asynchronousResult)
	{
		WebResponse webResponse = null;
		PostEventThreadTask postEventThreadTask = null;
		try
		{
			postEventThreadTask = (PostEventThreadTask)asynchronousResult.AsyncState;
			WebRequest webRequest;
			lock (postEventThreadTask.requestLock)
			{
				webRequest = postEventThreadTask.webrequest;
			}
			webResponse = webRequest.EndGetResponse(asynchronousResult);
		}
		catch (Exception)
		{
		}
		finally
		{
			webResponse?.Close();
			postEventThreadTask.allDone.Set();
		}
	}
}
