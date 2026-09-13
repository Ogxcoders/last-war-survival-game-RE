using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public abstract class MessageProcessingHandler : DelegatingHandler
{
	private sealed class SendState : TaskCompletionSource<HttpResponseMessage>
	{
		internal readonly MessageProcessingHandler _handler;

		internal readonly CancellationToken _token;

		public SendState(MessageProcessingHandler handler, CancellationToken token)
		{
			_handler = handler;
			_token = token;
		}
	}

	protected MessageProcessingHandler()
	{
	}

	protected MessageProcessingHandler(HttpMessageHandler innerHandler)
		: base(innerHandler)
	{
	}

	protected abstract HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken);

	protected abstract HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken);

	protected internal sealed override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (request == null)
		{
			throw new ArgumentNullException("request", "A request message must be provided. It cannot be null.");
		}
		SendState sendState = new SendState(this, cancellationToken);
		try
		{
			HttpRequestMessage request2 = ProcessRequest(request, cancellationToken);
			base.SendAsync(request2, cancellationToken).ContinueWithStandard(sendState, delegate(Task<HttpResponseMessage> task, object state)
			{
				SendState sendState2 = (SendState)state;
				MessageProcessingHandler handler = sendState2._handler;
				CancellationToken token = sendState2._token;
				if (task.IsFaulted)
				{
					sendState2.TrySetException(task.Exception.GetBaseException());
				}
				else if (task.IsCanceled)
				{
					sendState2.TrySetCanceled();
				}
				else
				{
					if (task.Result != null)
					{
						try
						{
							HttpResponseMessage result = handler.ProcessResponse(task.Result, token);
							sendState2.TrySetResult(result);
							return;
						}
						catch (OperationCanceledException e2)
						{
							HandleCanceledOperations(token, sendState2, e2);
							return;
						}
						catch (Exception exception2)
						{
							sendState2.TrySetException(exception2);
							return;
						}
					}
					sendState2.TrySetException(new InvalidOperationException("Handler did not return a response message."));
				}
			});
		}
		catch (OperationCanceledException e)
		{
			HandleCanceledOperations(cancellationToken, sendState, e);
		}
		catch (Exception exception)
		{
			sendState.TrySetException(exception);
		}
		return sendState.Task;
	}

	private static void HandleCanceledOperations(CancellationToken cancellationToken, TaskCompletionSource<HttpResponseMessage> tcs, OperationCanceledException e)
	{
		if (cancellationToken.IsCancellationRequested && e.CancellationToken == cancellationToken)
		{
			tcs.TrySetCanceled();
		}
		else
		{
			tcs.TrySetException(e);
		}
	}
}
