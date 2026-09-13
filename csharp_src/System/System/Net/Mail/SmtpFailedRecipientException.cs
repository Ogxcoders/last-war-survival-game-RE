using System.Runtime.Serialization;

namespace System.Net.Mail;

[Serializable]
public class SmtpFailedRecipientException : SmtpException, ISerializable
{
	private string failedRecipient;

	public string FailedRecipient => failedRecipient;

	public SmtpFailedRecipientException()
	{
	}

	public SmtpFailedRecipientException(string message)
		: base(message)
	{
	}

	protected SmtpFailedRecipientException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		failedRecipient = info.GetString("failedRecipient");
	}

	public SmtpFailedRecipientException(SmtpStatusCode statusCode, string failedRecipient)
		: base(statusCode)
	{
		this.failedRecipient = failedRecipient;
	}

	public SmtpFailedRecipientException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public SmtpFailedRecipientException(string message, string failedRecipient, Exception innerException)
		: base(message, innerException)
	{
		this.failedRecipient = failedRecipient;
	}

	public SmtpFailedRecipientException(SmtpStatusCode statusCode, string failedRecipient, string serverResponse)
		: base(statusCode, serverResponse)
	{
		this.failedRecipient = failedRecipient;
	}

	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
		if (serializationInfo == null)
		{
			throw new ArgumentNullException("serializationInfo");
		}
		base.GetObjectData(serializationInfo, streamingContext);
		serializationInfo.AddValue("failedRecipient", failedRecipient);
	}

	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
		GetObjectData(serializationInfo, streamingContext);
	}
}
