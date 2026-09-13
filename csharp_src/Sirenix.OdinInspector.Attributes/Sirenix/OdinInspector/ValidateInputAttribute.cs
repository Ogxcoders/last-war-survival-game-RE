using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[DontApplyToListElements]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public sealed class ValidateInputAttribute : Attribute
{
	public string DefaultMessage;

	public string Condition;

	public InfoMessageType MessageType;

	public bool IncludeChildren;

	public bool ContinuousValidationCheck;

	[Obsolete("Use the Condition member instead.", false)]
	public string MemberName
	{
		get
		{
			return Condition;
		}
		set
		{
			Condition = value;
		}
	}

	[Obsolete("Use the ContinuousValidationCheck member instead.")]
	public bool ContiniousValidationCheck
	{
		get
		{
			return ContinuousValidationCheck;
		}
		set
		{
			ContinuousValidationCheck = value;
		}
	}

	public ValidateInputAttribute(string condition, string defaultMessage = null, InfoMessageType messageType = InfoMessageType.Error)
	{
		Condition = condition;
		DefaultMessage = defaultMessage;
		MessageType = messageType;
		IncludeChildren = true;
	}

	[Obsolete("Rejecting invalid input is no longer supported. Use the other constructor instead.", true)]
	public ValidateInputAttribute(string condition, string message, InfoMessageType messageType, bool rejectedInvalidInput)
	{
		Condition = condition;
		DefaultMessage = message;
		MessageType = messageType;
		IncludeChildren = true;
	}
}
