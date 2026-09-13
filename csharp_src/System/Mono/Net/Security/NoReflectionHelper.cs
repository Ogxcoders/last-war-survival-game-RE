using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Mono.Security.Interface;

namespace Mono.Net.Security;

internal static class NoReflectionHelper
{
	internal static bool IsInitialized => MonoTlsProviderFactory.IsInitialized;

	internal static object GetDefaultValidator(object settings)
	{
		return ChainValidationHelper.GetDefaultValidator((MonoTlsSettings)settings);
	}

	internal static object GetProvider()
	{
		return MonoTlsProviderFactory.GetProvider();
	}

	internal static void Initialize()
	{
		MonoTlsProviderFactory.Initialize();
	}

	internal static void Initialize(string provider)
	{
		MonoTlsProviderFactory.Initialize(provider);
	}

	internal static HttpWebRequest CreateHttpsRequest(Uri requestUri, object provider, object settings)
	{
		return new HttpWebRequest(requestUri, (MobileTlsProvider)provider, (MonoTlsSettings)settings);
	}

	internal static object CreateHttpListener(object certificate, object provider, object settings)
	{
		return new HttpListener((X509Certificate)certificate, (MonoTlsProvider)provider, (MonoTlsSettings)settings);
	}

	internal static object GetMonoSslStream(SslStream stream)
	{
		return stream.Impl;
	}

	internal static object GetMonoSslStream(HttpListenerContext context)
	{
		return context.Connection.SslStream?.Impl;
	}

	internal static bool IsProviderSupported(string name)
	{
		return MonoTlsProviderFactory.IsProviderSupported(name);
	}

	internal static object GetProvider(string name)
	{
		return MonoTlsProviderFactory.GetProvider(name);
	}
}
