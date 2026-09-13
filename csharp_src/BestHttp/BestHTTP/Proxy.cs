using System;
using System.IO;
using BestHTTP.Authentication;

namespace BestHTTP;

public abstract class Proxy
{
	public Uri Address { get; set; }

	public Credentials Credentials { get; set; }

	internal Proxy(Uri address, Credentials credentials)
	{
		Address = address;
		Credentials = credentials;
	}

	internal abstract void Connect(Stream stream, HTTPRequest request);

	internal abstract string GetRequestPath(Uri uri);
}
