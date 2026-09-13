using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Mono.Net.Security;

namespace Mono.Security.Interface;

public static class MonoTlsProviderFactory
{
	internal const int InternalVersion = 4;

	public static bool IsInitialized => NoReflectionHelper.IsInitialized;

	public static MonoTlsProvider GetProvider()
	{
		return (MonoTlsProvider)NoReflectionHelper.GetProvider();
	}

	public static void Initialize()
	{
		NoReflectionHelper.Initialize();
	}

	public static void Initialize(string provider)
	{
		NoReflectionHelper.Initialize(provider);
	}

	public static bool IsProviderSupported(string provider)
	{
		return NoReflectionHelper.IsProviderSupported(provider);
	}

	public static MonoTlsProvider GetProvider(string provider)
	{
		return (MonoTlsProvider)NoReflectionHelper.GetProvider(provider);
	}

	public static HttpWebRequest CreateHttpsRequest(Uri requestUri, MonoTlsProvider provider, MonoTlsSettings settings = null)
	{
		return NoReflectionHelper.CreateHttpsRequest(requestUri, provider, settings);
	}

	public static HttpListener CreateHttpListener(X509Certificate certificate, MonoTlsProvider provider = null, MonoTlsSettings settings = null)
	{
		return (HttpListener)NoReflectionHelper.CreateHttpListener(certificate, provider, settings);
	}

	public static IMonoSslStream GetMonoSslStream(SslStream stream)
	{
		return (IMonoSslStream)NoReflectionHelper.GetMonoSslStream(stream);
	}

	public static IMonoSslStream GetMonoSslStream(HttpListenerContext context)
	{
		return (IMonoSslStream)NoReflectionHelper.GetMonoSslStream(context);
	}
}
