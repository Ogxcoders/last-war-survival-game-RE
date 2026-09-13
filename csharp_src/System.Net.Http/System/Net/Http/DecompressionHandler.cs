using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal sealed class DecompressionHandler : HttpMessageHandler
{
	private abstract class DecompressedContent : HttpContent
	{
		private HttpContent _originalContent;

		private bool _contentConsumed;

		public DecompressedContent(HttpContent originalContent)
		{
			_originalContent = originalContent;
			_contentConsumed = false;
			base.Headers.AddHeaders(originalContent.Headers);
			base.Headers.ContentLength = null;
			base.Headers.ContentEncoding.Clear();
			string text = null;
			foreach (string item in originalContent.Headers.ContentEncoding)
			{
				if (text != null)
				{
					base.Headers.ContentEncoding.Add(text);
				}
				text = item;
			}
		}

		protected abstract Stream GetDecompressedStream(Stream originalStream);

		protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
		{
			return SerializeToStreamAsync(stream, context, CancellationToken.None);
		}

		internal override async Task SerializeToStreamAsync(Stream stream, TransportContext context, CancellationToken cancellationToken)
		{
			using Stream decompressedStream = await CreateContentReadStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
			await decompressedStream.CopyToAsync(stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		protected override async Task<Stream> CreateContentReadStreamAsync()
		{
			if (_contentConsumed)
			{
				throw new InvalidOperationException("The stream was already consumed. It cannot be read again.");
			}
			_contentConsumed = true;
			Stream stream = _originalContent.TryReadAsStream();
			if (stream == null)
			{
				stream = await _originalContent.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			Stream originalStream = stream;
			return GetDecompressedStream(originalStream);
		}

		protected internal override bool TryComputeLength(out long length)
		{
			length = 0L;
			return false;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_originalContent.Dispose();
			}
			base.Dispose(disposing);
		}
	}

	private sealed class GZipDecompressedContent : DecompressedContent
	{
		public GZipDecompressedContent(HttpContent originalContent)
			: base(originalContent)
		{
		}

		protected override Stream GetDecompressedStream(Stream originalStream)
		{
			return new GZipStream(originalStream, CompressionMode.Decompress);
		}
	}

	private sealed class DeflateDecompressedContent : DecompressedContent
	{
		public DeflateDecompressedContent(HttpContent originalContent)
			: base(originalContent)
		{
		}

		protected override Stream GetDecompressedStream(Stream originalStream)
		{
			return new DeflateStream(originalStream, CompressionMode.Decompress);
		}
	}

	private readonly HttpMessageHandler _innerHandler;

	private readonly DecompressionMethods _decompressionMethods;

	private const string s_gzip = "gzip";

	private const string s_deflate = "deflate";

	private static readonly StringWithQualityHeaderValue s_gzipHeaderValue = new StringWithQualityHeaderValue("gzip");

	private static readonly StringWithQualityHeaderValue s_deflateHeaderValue = new StringWithQualityHeaderValue("deflate");

	internal bool GZipEnabled => (_decompressionMethods & DecompressionMethods.GZip) != 0;

	internal bool DeflateEnabled => (_decompressionMethods & DecompressionMethods.Deflate) != 0;

	public DecompressionHandler(DecompressionMethods decompressionMethods, HttpMessageHandler innerHandler)
	{
		_decompressionMethods = decompressionMethods;
		_innerHandler = innerHandler;
	}

	protected internal override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (GZipEnabled)
		{
			request.Headers.AcceptEncoding.Add(s_gzipHeaderValue);
		}
		if (DeflateEnabled)
		{
			request.Headers.AcceptEncoding.Add(s_deflateHeaderValue);
		}
		HttpResponseMessage httpResponseMessage = await _innerHandler.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		ICollection<string> contentEncoding = httpResponseMessage.Content.Headers.ContentEncoding;
		if (contentEncoding.Count > 0)
		{
			string text = null;
			foreach (string item in contentEncoding)
			{
				text = item;
			}
			if (GZipEnabled && text == "gzip")
			{
				httpResponseMessage.Content = new GZipDecompressedContent(httpResponseMessage.Content);
			}
			else if (DeflateEnabled && text == "deflate")
			{
				httpResponseMessage.Content = new DeflateDecompressedContent(httpResponseMessage.Content);
			}
		}
		return httpResponseMessage;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_innerHandler.Dispose();
		}
		base.Dispose(disposing);
	}
}
