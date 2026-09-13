using System;
using System.IO;
using System.Net;
using System.Text;
using BestHTTP.Authentication;
using BestHTTP.Extensions;
using BestHTTP.Logger;

namespace BestHTTP;

public sealed class SOCKSProxy : Proxy
{
	public SOCKSProxy(Uri address, Credentials credentials)
		: base(address, credentials)
	{
	}

	internal override string GetRequestPath(Uri uri)
	{
		return uri.GetRequestPathAndQueryURL();
	}

	internal override void Connect(Stream stream, HTTPRequest request)
	{
		byte[] array = new byte[1024];
		int count = 0;
		array[count++] = 5;
		if (base.Credentials != null)
		{
			array[count++] = 2;
			array[count++] = 2;
			array[count++] = 0;
		}
		else
		{
			array[count++] = 1;
			array[count++] = 0;
		}
		if (HTTPManager.Logger.Level == Loglevels.All)
		{
			HTTPManager.Logger.Information("SOCKSProxy", $"Sending method negotiation - count: {count.ToString()} buffer: {BufferToHexStr(array, count)} ");
		}
		stream.Write(array, 0, count);
		count = stream.Read(array, 0, array.Length);
		if (HTTPManager.Logger.Level == Loglevels.All)
		{
			HTTPManager.Logger.Information("SOCKSProxy", $"Negotiation response - count: {count.ToString()} buffer: {BufferToHexStr(array, count)} ");
		}
		SOCKSVersions sOCKSVersions = (SOCKSVersions)array[0];
		SOCKSMethods sOCKSMethods = (SOCKSMethods)array[1];
		if (count != 2)
		{
			throw new Exception(string.Format("SOCKS Proxy - Expected read count: 2! count: {0} buffer: {1}" + count, BufferToHexStr(array, count)));
		}
		if (sOCKSVersions != SOCKSVersions.V5)
		{
			throw new Exception("SOCKS Proxy - Expected version: 5, received version: " + array[0].ToString("X2"));
		}
		if (sOCKSMethods == SOCKSMethods.NoAcceptableMethods)
		{
			throw new Exception("SOCKS Proxy - Received 'NO ACCEPTABLE METHODS' (0xFF)");
		}
		HTTPManager.Logger.Information("SOCKSProxy", "Method negotiation over. Method: " + sOCKSMethods);
		switch (sOCKSMethods)
		{
		case SOCKSMethods.UsernameAndPassword:
		{
			if (base.Credentials.UserName.Length > 255)
			{
				throw new Exception($"SOCKS Proxy - Credentials.UserName too long! {base.Credentials.UserName.Length.ToString()} > 255");
			}
			if (base.Credentials.Password.Length > 255)
			{
				throw new Exception($"SOCKS Proxy - Credentials.Password too long! {base.Credentials.Password.Length.ToString()} > 255");
			}
			HTTPManager.Logger.Information("SOCKSProxy", "starting sub-negotiation");
			count = 0;
			array[count++] = 1;
			WriteString(array, ref count, base.Credentials.UserName);
			WriteString(array, ref count, base.Credentials.Password);
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Information("SOCKSProxy", $"Sending username and password sub-negotiation - count: {count.ToString()} buffer: {BufferToHexStr(array, count)} ");
			}
			stream.Write(array, 0, count);
			count = stream.Read(array, 0, array.Length);
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Information("SOCKSProxy", $"Username and password sub-negotiation response - count: {count.ToString()} buffer: {BufferToHexStr(array, count)} ");
			}
			bool flag = array[1] == 0;
			if (count != 2)
			{
				throw new Exception(string.Format("SOCKS Proxy - Expected read count: 2! count: {0} buffer: {1}" + count, BufferToHexStr(array, count)));
			}
			if (!flag)
			{
				throw new Exception("SOCKS proxy: username+password authentication failed!");
			}
			HTTPManager.Logger.Information("SOCKSProxy", "Authenticated!");
			break;
		}
		case SOCKSMethods.GSSAPI:
			throw new Exception("SOCKS proxy: GSSAPI not supported!");
		case SOCKSMethods.NoAcceptableMethods:
			throw new Exception("SOCKS proxy: No acceptable method");
		}
		count = 0;
		array[count++] = 5;
		array[count++] = 1;
		array[count++] = 0;
		if (request.CurrentUri.IsHostIsAnIPAddress())
		{
			bool flag2 = BestHTTP.Extensions.Extensions.IsIpV4AddressValid(request.CurrentUri.Host);
			array[count++] = (byte)((!flag2) ? 4 : 0);
			byte[] addressBytes = IPAddress.Parse(request.CurrentUri.Host).GetAddressBytes();
			WriteBytes(array, ref count, addressBytes);
		}
		else
		{
			array[count++] = 3;
			WriteString(array, ref count, request.CurrentUri.Host);
		}
		array[count++] = (byte)((request.CurrentUri.Port >> 8) & 0xFF);
		array[count++] = (byte)(request.CurrentUri.Port & 0xFF);
		if (HTTPManager.Logger.Level == Loglevels.All)
		{
			HTTPManager.Logger.Information("SOCKSProxy", $"Sending connect request - count: {count.ToString()} buffer: {BufferToHexStr(array, count)} ");
		}
		stream.Write(array, 0, count);
		count = stream.Read(array, 0, array.Length);
		if (HTTPManager.Logger.Level == Loglevels.All)
		{
			HTTPManager.Logger.Information("SOCKSProxy", $"Connect response - count: {count.ToString()} buffer: {BufferToHexStr(array, count)} ");
		}
		sOCKSVersions = (SOCKSVersions)array[0];
		SOCKSReplies sOCKSReplies = (SOCKSReplies)array[1];
		if (count < 10)
		{
			throw new Exception($"SOCKS proxy: not enough data returned by the server. Expected count is at least 10 bytes, server returned {count.ToString()} bytes! content: {BufferToHexStr(array, count)}");
		}
		if (sOCKSReplies != SOCKSReplies.Succeeded)
		{
			throw new Exception("SOCKS proxy error: " + sOCKSReplies);
		}
		HTTPManager.Logger.Information("SOCKSProxy", "Connected!");
	}

	private void WriteString(byte[] buffer, ref int count, string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		if (bytes.Length > 255)
		{
			throw new Exception($"SOCKS Proxy - String is too large ({bytes.Length.ToString()}) to fit in 255 bytes!");
		}
		buffer[count++] = (byte)bytes.Length;
		Array.Copy(bytes, 0, buffer, count, bytes.Length);
		count += bytes.Length;
	}

	private void WriteBytes(byte[] buffer, ref int count, byte[] bytes)
	{
		Array.Copy(bytes, 0, buffer, count, bytes.Length);
		count += bytes.Length;
	}

	private string BufferToHexStr(byte[] buffer, int count)
	{
		StringBuilder stringBuilder = new StringBuilder(count * 2);
		for (int i = 0; i < count; i++)
		{
			stringBuilder.AppendFormat("0x{0} ", buffer[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}
}
