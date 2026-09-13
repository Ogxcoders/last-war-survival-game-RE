using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web.Services.Discovery;
using System.Xml;

namespace System.Web.Services.Protocols;

[ComVisible(true)]
public class SoapHttpClientProtocol : HttpWebClientProtocol
{
	internal class SoapWebClientAsyncResult : WebClientAsyncResult
	{
		public SoapClientMessage Message;

		public SoapExtension[] Extensions;

		public SoapWebClientAsyncResult(WebRequest request, AsyncCallback callback, object asyncState)
			: base(request, callback, asyncState)
		{
		}
	}

	private SoapTypeStubInfo type_info;

	private SoapProtocolVersion soapVersion;

	[System.MonoTODO("Do something with this")]
	[ComVisible(false)]
	[DefaultValue(SoapProtocolVersion.Default)]
	public SoapProtocolVersion SoapVersion
	{
		get
		{
			return soapVersion;
		}
		set
		{
			soapVersion = value;
		}
	}

	public SoapHttpClientProtocol()
	{
		type_info = (SoapTypeStubInfo)TypeStubManager.GetTypeStub(GetType(), "Soap");
	}

	protected IAsyncResult BeginInvoke(string methodName, object[] parameters, AsyncCallback callback, object asyncState)
	{
		SoapMethodStubInfo soapMethodStubInfo = (SoapMethodStubInfo)type_info.GetMethod(methodName);
		SoapWebClientAsyncResult soapWebClientAsyncResult = null;
		try
		{
			SoapClientMessage soapClientMessage = new SoapClientMessage(this, soapMethodStubInfo, base.Url, parameters);
			soapClientMessage.CollectHeaders(this, soapClientMessage.MethodStubInfo.Headers, SoapHeaderDirection.In);
			soapWebClientAsyncResult = new SoapWebClientAsyncResult(GetRequestForMessage(uri, soapClientMessage), callback, asyncState);
			soapWebClientAsyncResult.Message = soapClientMessage;
			soapWebClientAsyncResult.Extensions = SoapExtension.CreateExtensionChain(type_info.SoapExtensions[0], soapMethodStubInfo.SoapExtensions, type_info.SoapExtensions[1]);
			soapWebClientAsyncResult.Request.BeginGetRequestStream(AsyncGetRequestStreamDone, soapWebClientAsyncResult);
			RegisterMapping(asyncState, soapWebClientAsyncResult);
		}
		catch (Exception exception)
		{
			soapWebClientAsyncResult?.SetCompleted(null, exception, async: false);
		}
		return soapWebClientAsyncResult;
	}

	private void AsyncGetRequestStreamDone(IAsyncResult ar)
	{
		SoapWebClientAsyncResult soapWebClientAsyncResult = (SoapWebClientAsyncResult)ar.AsyncState;
		try
		{
			SendRequest(soapWebClientAsyncResult.Request.EndGetRequestStream(ar), soapWebClientAsyncResult.Message, soapWebClientAsyncResult.Extensions);
			soapWebClientAsyncResult.Request.BeginGetResponse(AsyncGetResponseDone, soapWebClientAsyncResult);
		}
		catch (Exception exception)
		{
			soapWebClientAsyncResult.SetCompleted(null, exception, async: true);
		}
	}

	private void AsyncGetResponseDone(IAsyncResult ar)
	{
		SoapWebClientAsyncResult soapWebClientAsyncResult = (SoapWebClientAsyncResult)ar.AsyncState;
		WebResponse webResponse = null;
		try
		{
			webResponse = GetWebResponse(soapWebClientAsyncResult.Request, ar);
		}
		catch (WebException ex)
		{
			webResponse = ex.Response;
			if (!(webResponse is HttpWebResponse { StatusCode: HttpStatusCode.InternalServerError }))
			{
				soapWebClientAsyncResult.SetCompleted(null, ex, async: true);
				return;
			}
		}
		catch (Exception exception)
		{
			soapWebClientAsyncResult.SetCompleted(null, exception, async: true);
			return;
		}
		try
		{
			object[] result = ReceiveResponse(webResponse, soapWebClientAsyncResult.Message, soapWebClientAsyncResult.Extensions);
			soapWebClientAsyncResult.SetCompleted(result, null, async: true);
		}
		catch (Exception exception2)
		{
			soapWebClientAsyncResult.SetCompleted(null, exception2, async: true);
		}
		finally
		{
			webResponse.Close();
		}
	}

	protected object[] EndInvoke(IAsyncResult asyncResult)
	{
		if (!(asyncResult is SoapWebClientAsyncResult))
		{
			throw new ArgumentException("asyncResult is not the return value from BeginInvoke");
		}
		SoapWebClientAsyncResult soapWebClientAsyncResult = (SoapWebClientAsyncResult)asyncResult;
		lock (soapWebClientAsyncResult)
		{
			if (!soapWebClientAsyncResult.IsCompleted)
			{
				soapWebClientAsyncResult.WaitForComplete();
			}
			UnregisterMapping(soapWebClientAsyncResult.AsyncState);
			if (soapWebClientAsyncResult.Exception != null)
			{
				throw soapWebClientAsyncResult.Exception;
			}
			return (object[])soapWebClientAsyncResult.Result;
		}
	}

	public void Discover()
	{
		BindingInfo bindingInfo = (BindingInfo)type_info.Bindings[0];
		DiscoveryClientProtocol discoveryClientProtocol = new DiscoveryClientProtocol();
		discoveryClientProtocol.Discover(base.Url);
		foreach (object item in discoveryClientProtocol.AdditionalInformation)
		{
			if (item is SoapBinding soapBinding && soapBinding.Binding.Name == bindingInfo.Name && soapBinding.Binding.Namespace == bindingInfo.Namespace)
			{
				base.Url = soapBinding.Address;
				return;
			}
		}
		throw new Exception($"The binding named '{bindingInfo.Name}' from namespace '{bindingInfo.Namespace}' was not found in the discovery document at '{base.Url}'");
	}

	protected override WebRequest GetWebRequest(Uri uri)
	{
		return base.GetWebRequest(uri);
	}

	private WebRequest GetRequestForMessage(Uri uri, SoapClientMessage message)
	{
		WebRequest webRequest = GetWebRequest(uri);
		webRequest.Method = "POST";
		WebHeaderCollection headers = webRequest.Headers;
		webRequest.ContentType = message.ContentType + "; charset=utf-8";
		if (!message.IsSoap12)
		{
			headers.Add("SOAPAction", "\"" + message.Action + "\"");
		}
		else
		{
			webRequest.ContentType = webRequest.ContentType + "; action=\"" + message.Action + "\"";
		}
		return webRequest;
	}

	[System.MonoTODO]
	protected virtual XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	protected virtual XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
	{
		throw new NotImplementedException();
	}

	private void SendRequest(Stream s, SoapClientMessage message, SoapExtension[] extensions)
	{
		using (s)
		{
			if (extensions != null)
			{
				s = SoapExtension.ExecuteChainStream(extensions, s);
				message.SetStage(SoapMessageStage.BeforeSerialize);
				SoapExtension.ExecuteProcessMessage(extensions, message, s, inverseOrder: true);
			}
			XmlTextWriter xmlTextWriter = WebServiceHelper.CreateXmlWriter(s);
			WebServiceHelper.WriteSoapMessage(xmlTextWriter, message.MethodStubInfo, SoapHeaderDirection.In, message.Parameters, message.Headers, message.IsSoap12);
			if (extensions != null)
			{
				message.SetStage(SoapMessageStage.AfterSerialize);
				SoapExtension.ExecuteProcessMessage(extensions, message, s, inverseOrder: true);
			}
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
		}
	}

	private object[] ReceiveResponse(WebResponse response, SoapClientMessage message, SoapExtension[] extensions)
	{
		SoapMethodStubInfo methodStubInfo = message.MethodStubInfo;
		if (response is HttpWebResponse { StatusCode: var statusCode } httpWebResponse)
		{
			if (statusCode != HttpStatusCode.Accepted && statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.InternalServerError)
			{
				throw new WebException($"The request failed with HTTP status {(int)statusCode}: {statusCode}", null, WebExceptionStatus.ProtocolError, httpWebResponse);
			}
			if (message.OneWay && response.ContentLength <= 0 && (statusCode == HttpStatusCode.Accepted || statusCode == HttpStatusCode.OK))
			{
				return new object[0];
			}
		}
		string content_type;
		Encoding contentEncoding = WebServiceHelper.GetContentEncoding(response.ContentType, out content_type);
		content_type = content_type.ToLower(CultureInfo.InvariantCulture);
		if ((!message.IsSoap12 || content_type != "application/soap+xml") && content_type != "text/xml")
		{
			WebServiceHelper.InvalidOperation($"Not supported Content-Type in the response: '{response.ContentType}'", response, contentEncoding);
		}
		message.ContentType = content_type;
		message.ContentEncoding = contentEncoding.WebName;
		Stream stream = response.GetResponseStream();
		if (extensions != null)
		{
			stream = SoapExtension.ExecuteChainStream(extensions, stream);
			message.SetStage(SoapMessageStage.BeforeDeserialize);
			SoapExtension.ExecuteProcessMessage(extensions, message, stream, inverseOrder: false);
		}
		object body;
		SoapHeaderCollection headers;
		using (StreamReader input = new StreamReader(stream, contentEncoding, detectEncodingFromByteOrderMarks: false))
		{
			WebServiceHelper.ReadSoapMessage(new XmlTextReader(input), methodStubInfo, SoapHeaderDirection.Out, message.IsSoap12, out body, out headers);
		}
		if (body is Soap12Fault)
		{
			SoapException exception = WebServiceHelper.Soap12FaultToSoapException((Soap12Fault)body);
			message.SetException(exception);
		}
		else if (body is Fault)
		{
			Fault fault = (Fault)body;
			SoapException exception2 = new SoapException(fault.faultstring, fault.faultcode, fault.faultactor, fault.detail);
			message.SetException(exception2);
		}
		else
		{
			message.OutParameters = (object[])body;
		}
		message.SetHeaders(headers);
		message.UpdateHeaderValues(this, message.MethodStubInfo.Headers);
		if (extensions != null)
		{
			message.SetStage(SoapMessageStage.AfterDeserialize);
			SoapExtension.ExecuteProcessMessage(extensions, message, stream, inverseOrder: false);
		}
		if (message.Exception == null)
		{
			return message.OutParameters;
		}
		throw message.Exception;
	}

	protected object[] Invoke(string method_name, object[] parameters)
	{
		SoapMethodStubInfo soapMethodStubInfo = (SoapMethodStubInfo)type_info.GetMethod(method_name);
		SoapClientMessage soapClientMessage = new SoapClientMessage(this, soapMethodStubInfo, base.Url, parameters);
		soapClientMessage.CollectHeaders(this, soapClientMessage.MethodStubInfo.Headers, SoapHeaderDirection.In);
		SoapExtension[] extensions = SoapExtension.CreateExtensionChain(type_info.SoapExtensions[0], soapMethodStubInfo.SoapExtensions, type_info.SoapExtensions[1]);
		WebResponse webResponse;
		try
		{
			WebRequest requestForMessage = GetRequestForMessage(uri, soapClientMessage);
			SendRequest(requestForMessage.GetRequestStream(), soapClientMessage, extensions);
			webResponse = GetWebResponse(requestForMessage);
		}
		catch (WebException ex)
		{
			webResponse = ex.Response;
			if (!(webResponse is HttpWebResponse { StatusCode: HttpStatusCode.InternalServerError }))
			{
				throw ex;
			}
		}
		try
		{
			return ReceiveResponse(webResponse, soapClientMessage, extensions);
		}
		finally
		{
			webResponse.Close();
		}
	}

	protected void InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback)
	{
		InvokeAsync(methodName, parameters, callback, null);
	}

	protected void InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback, object userState)
	{
		InvokeAsyncInfo asyncState = new InvokeAsyncInfo(callback, userState);
		BeginInvoke(methodName, parameters, InvokeAsyncCallback, asyncState);
	}

	private void InvokeAsyncCallback(IAsyncResult ar)
	{
		InvokeAsyncInfo invokeAsyncInfo = (InvokeAsyncInfo)ar.AsyncState;
		SoapWebClientAsyncResult soapWebClientAsyncResult = (SoapWebClientAsyncResult)ar;
		InvokeCompletedEventArgs state = new InvokeCompletedEventArgs(soapWebClientAsyncResult.Exception, cancelled: false, invokeAsyncInfo.UserState, (object[])soapWebClientAsyncResult.Result);
		UnregisterMapping(ar.AsyncState);
		if (invokeAsyncInfo.Context != null)
		{
			invokeAsyncInfo.Context.Send(invokeAsyncInfo.Callback, state);
		}
		else
		{
			invokeAsyncInfo.Callback(state);
		}
	}
}
