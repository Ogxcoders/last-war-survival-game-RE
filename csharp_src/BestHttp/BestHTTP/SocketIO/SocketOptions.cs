using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.SocketIO.Transports;
using PlatformSupport.Collections.ObjectModel;
using PlatformSupport.Collections.Specialized;

namespace BestHTTP.SocketIO;

public sealed class SocketOptions
{
	private float randomizationFactor;

	private ObservableDictionary<string, string> additionalQueryParams;

	private string BuiltQueryParams;

	public TransportTypes ConnectWith { get; set; }

	public bool Reconnection { get; set; }

	public int ReconnectionAttempts { get; set; }

	public TimeSpan ReconnectionDelay { get; set; }

	public TimeSpan ReconnectionDelayMax { get; set; }

	public float RandomizationFactor
	{
		get
		{
			return randomizationFactor;
		}
		set
		{
			randomizationFactor = Math.Min(1f, Math.Max(0f, value));
		}
	}

	public TimeSpan Timeout { get; set; }

	public bool AutoConnect { get; set; }

	public ObservableDictionary<string, string> AdditionalQueryParams
	{
		get
		{
			return additionalQueryParams;
		}
		set
		{
			if (additionalQueryParams != null)
			{
				additionalQueryParams.CollectionChanged -= AdditionalQueryParams_CollectionChanged;
			}
			additionalQueryParams = value;
			BuiltQueryParams = null;
			if (value != null)
			{
				value.CollectionChanged += AdditionalQueryParams_CollectionChanged;
			}
		}
	}

	public bool QueryParamsOnlyForHandshake { get; set; }

	public SocketOptions()
	{
		ConnectWith = TransportTypes.Polling;
		Reconnection = true;
		ReconnectionAttempts = int.MaxValue;
		ReconnectionDelay = TimeSpan.FromMilliseconds(1000.0);
		ReconnectionDelayMax = TimeSpan.FromMilliseconds(5000.0);
		RandomizationFactor = 0.5f;
		Timeout = TimeSpan.FromMilliseconds(20000.0);
		AutoConnect = true;
		QueryParamsOnlyForHandshake = true;
	}

	internal string BuildQueryParams()
	{
		if (AdditionalQueryParams == null || AdditionalQueryParams.Count == 0)
		{
			return string.Empty;
		}
		if (!string.IsNullOrEmpty(BuiltQueryParams))
		{
			return BuiltQueryParams;
		}
		StringBuilder stringBuilder = new StringBuilder(AdditionalQueryParams.Count * 4);
		foreach (KeyValuePair<string, string> additionalQueryParam in AdditionalQueryParams)
		{
			stringBuilder.Append("&");
			stringBuilder.Append(additionalQueryParam.Key);
			if (!string.IsNullOrEmpty(additionalQueryParam.Value))
			{
				stringBuilder.Append("=");
				stringBuilder.Append(additionalQueryParam.Value);
			}
		}
		return BuiltQueryParams = stringBuilder.ToString();
	}

	private void AdditionalQueryParams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		BuiltQueryParams = null;
	}
}
