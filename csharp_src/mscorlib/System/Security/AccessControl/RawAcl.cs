using System.Collections.Generic;
using System.Text;

namespace System.Security.AccessControl;

public sealed class RawAcl : GenericAcl
{
	private byte revision;

	private List<GenericAce> list;

	public override int BinaryLength
	{
		get
		{
			int num = 8;
			foreach (GenericAce item in list)
			{
				num += item.BinaryLength;
			}
			return num;
		}
	}

	public override int Count => list.Count;

	public override GenericAce this[int index]
	{
		get
		{
			return list[index];
		}
		set
		{
			list[index] = value;
		}
	}

	public override byte Revision => revision;

	public RawAcl(byte revision, int capacity)
	{
		this.revision = revision;
		list = new List<GenericAce>(capacity);
	}

	public RawAcl(byte[] binaryForm, int offset)
	{
		if (binaryForm == null)
		{
			throw new ArgumentNullException("binaryForm");
		}
		if (offset < 0 || offset > binaryForm.Length - 8)
		{
			throw new ArgumentOutOfRangeException("offset", offset, "Offset out of range");
		}
		revision = binaryForm[offset];
		if (revision != GenericAcl.AclRevision && revision != GenericAcl.AclRevisionDS)
		{
			throw new ArgumentException("Invalid ACL - unknown revision", "binaryForm");
		}
		int num = ReadUShort(binaryForm, offset + 2);
		if (offset > binaryForm.Length - num)
		{
			throw new ArgumentException("Invalid ACL - truncated", "binaryForm");
		}
		int num2 = offset + 8;
		int num3 = ReadUShort(binaryForm, offset + 4);
		list = new List<GenericAce>(num3);
		for (int i = 0; i < num3; i++)
		{
			GenericAce genericAce = GenericAce.CreateFromBinaryForm(binaryForm, num2);
			list.Add(genericAce);
			num2 += genericAce.BinaryLength;
		}
	}

	internal RawAcl(byte revision, List<GenericAce> aces)
	{
		this.revision = revision;
		list = aces;
	}

	public override void GetBinaryForm(byte[] binaryForm, int offset)
	{
		if (binaryForm == null)
		{
			throw new ArgumentNullException("binaryForm");
		}
		if (offset < 0 || offset > binaryForm.Length - BinaryLength)
		{
			throw new ArgumentException("Offset out of range", "offset");
		}
		binaryForm[offset] = Revision;
		binaryForm[offset + 1] = 0;
		WriteUShort((ushort)BinaryLength, binaryForm, offset + 2);
		WriteUShort((ushort)list.Count, binaryForm, offset + 4);
		WriteUShort(0, binaryForm, offset + 6);
		int num = offset + 8;
		foreach (GenericAce item in list)
		{
			item.GetBinaryForm(binaryForm, num);
			num += item.BinaryLength;
		}
	}

	public void InsertAce(int index, GenericAce ace)
	{
		if (ace == null)
		{
			throw new ArgumentNullException("ace");
		}
		list.Insert(index, ace);
	}

	public void RemoveAce(int index)
	{
		list.RemoveAt(index);
	}

	internal override string GetSddlForm(ControlFlags sdFlags, bool isDacl)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (isDacl)
		{
			if ((sdFlags & ControlFlags.DiscretionaryAclProtected) != ControlFlags.None)
			{
				stringBuilder.Append("P");
			}
			if ((sdFlags & ControlFlags.DiscretionaryAclAutoInheritRequired) != ControlFlags.None)
			{
				stringBuilder.Append("AR");
			}
			if ((sdFlags & ControlFlags.DiscretionaryAclAutoInherited) != ControlFlags.None)
			{
				stringBuilder.Append("AI");
			}
		}
		else
		{
			if ((sdFlags & ControlFlags.SystemAclProtected) != ControlFlags.None)
			{
				stringBuilder.Append("P");
			}
			if ((sdFlags & ControlFlags.SystemAclAutoInheritRequired) != ControlFlags.None)
			{
				stringBuilder.Append("AR");
			}
			if ((sdFlags & ControlFlags.SystemAclAutoInherited) != ControlFlags.None)
			{
				stringBuilder.Append("AI");
			}
		}
		foreach (GenericAce item in list)
		{
			stringBuilder.Append(item.GetSddlForm());
		}
		return stringBuilder.ToString();
	}

	internal static RawAcl ParseSddlForm(string sddlForm, bool isDacl, ref ControlFlags sdFlags, ref int pos)
	{
		ParseFlags(sddlForm, isDacl, ref sdFlags, ref pos);
		byte b = GenericAcl.AclRevision;
		List<GenericAce> list = new List<GenericAce>();
		while (pos < sddlForm.Length && sddlForm[pos] == '(')
		{
			GenericAce genericAce = GenericAce.CreateFromSddlForm(sddlForm, ref pos);
			if (genericAce as ObjectAce != null)
			{
				b = GenericAcl.AclRevisionDS;
			}
			list.Add(genericAce);
		}
		return new RawAcl(b, list);
	}

	private static void ParseFlags(string sddlForm, bool isDacl, ref ControlFlags sdFlags, ref int pos)
	{
		char c = char.ToUpperInvariant(sddlForm[pos]);
		while (c == 'P' || c == 'A')
		{
			if (c == 'P')
			{
				if (isDacl)
				{
					sdFlags |= ControlFlags.DiscretionaryAclProtected;
				}
				else
				{
					sdFlags |= ControlFlags.SystemAclProtected;
				}
				pos++;
			}
			else
			{
				if (sddlForm.Length <= pos + 1)
				{
					throw new ArgumentException("Invalid SDDL string.", "sddlForm");
				}
				switch (char.ToUpperInvariant(sddlForm[pos + 1]))
				{
				case 'R':
					if (isDacl)
					{
						sdFlags |= ControlFlags.DiscretionaryAclAutoInheritRequired;
					}
					else
					{
						sdFlags |= ControlFlags.SystemAclAutoInheritRequired;
					}
					pos += 2;
					break;
				case 'I':
					if (isDacl)
					{
						sdFlags |= ControlFlags.DiscretionaryAclAutoInherited;
					}
					else
					{
						sdFlags |= ControlFlags.SystemAclAutoInherited;
					}
					pos += 2;
					break;
				default:
					throw new ArgumentException("Invalid SDDL string.", "sddlForm");
				}
			}
			c = char.ToUpperInvariant(sddlForm[pos]);
		}
	}

	private void WriteUShort(ushort val, byte[] buffer, int offset)
	{
		buffer[offset] = (byte)val;
		buffer[offset + 1] = (byte)(val >> 8);
	}

	private ushort ReadUShort(byte[] buffer, int offset)
	{
		return (ushort)(buffer[offset] | (buffer[offset + 1] << 8));
	}
}
