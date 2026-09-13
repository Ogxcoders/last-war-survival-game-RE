using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BattleCardProto : IMessage<BattleCardProto>, IMessage, IEquatable<BattleCardProto>, IDeepCloneable<BattleCardProto>
{
	private static readonly MessageParser<BattleCardProto> _parser = new MessageParser<BattleCardProto>(() => new BattleCardProto());

	private UnknownFieldSet _unknownFields;

	public const int CardIdFieldNumber = 1;

	private int cardId_;

	public const int LevelFieldNumber = 2;

	private int level_;

	public const int StarFieldNumber = 3;

	private int star_;

	public const int SlotFieldNumber = 4;

	private int slot_;

	public const int RandomEffectsFieldNumber = 5;

	private static readonly FieldCodec<Effect> _repeated_randomEffects_codec = FieldCodec.ForMessage(42u, Effect.Parser);

	private readonly RepeatedField<Effect> randomEffects_ = new RepeatedField<Effect>();

	[DebuggerNonUserCode]
	public static MessageParser<BattleCardProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[10];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int CardId
	{
		get
		{
			return cardId_;
		}
		set
		{
			cardId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Star
	{
		get
		{
			return star_;
		}
		set
		{
			star_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Slot
	{
		get
		{
			return slot_;
		}
		set
		{
			slot_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Effect> RandomEffects => randomEffects_;

	[DebuggerNonUserCode]
	public BattleCardProto()
	{
	}

	[DebuggerNonUserCode]
	public BattleCardProto(BattleCardProto other)
		: this()
	{
		cardId_ = other.cardId_;
		level_ = other.level_;
		star_ = other.star_;
		slot_ = other.slot_;
		randomEffects_ = other.randomEffects_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleCardProto Clone()
	{
		return new BattleCardProto(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleCardProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleCardProto other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CardId != other.CardId)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (Star != other.Star)
		{
			return false;
		}
		if (Slot != other.Slot)
		{
			return false;
		}
		if (!randomEffects_.Equals(other.randomEffects_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (CardId != 0)
		{
			num ^= CardId.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (Star != 0)
		{
			num ^= Star.GetHashCode();
		}
		if (Slot != 0)
		{
			num ^= Slot.GetHashCode();
		}
		num ^= randomEffects_.GetHashCode();
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	public void WriteTo(CodedOutputStream output)
	{
		if (CardId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(CardId);
		}
		if (Level != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Level);
		}
		if (Star != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Star);
		}
		if (Slot != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Slot);
		}
		randomEffects_.WriteTo(output, _repeated_randomEffects_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (CardId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CardId);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (Star != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Star);
		}
		if (Slot != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Slot);
		}
		num += randomEffects_.CalculateSize(_repeated_randomEffects_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleCardProto other)
	{
		if (other != null)
		{
			if (other.CardId != 0)
			{
				CardId = other.CardId;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			if (other.Star != 0)
			{
				Star = other.Star;
			}
			if (other.Slot != 0)
			{
				Slot = other.Slot;
			}
			randomEffects_.Add(other.randomEffects_);
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 8u:
				CardId = input.ReadInt32();
				break;
			case 16u:
				Level = input.ReadInt32();
				break;
			case 24u:
				Star = input.ReadInt32();
				break;
			case 32u:
				Slot = input.ReadInt32();
				break;
			case 42u:
				randomEffects_.AddEntriesFrom(input, _repeated_randomEffects_codec);
				break;
			}
		}
	}
}
