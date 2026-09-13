using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;

namespace System.Security;

[Serializable]
[ComVisible(true)]
[MonoTODO("CAS support is experimental (and unsupported).")]
[StrongNameIdentityPermission(SecurityAction.InheritanceDemand, PublicKey = "002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293")]
public class PermissionSet : ISecurityEncodable, ICollection, IEnumerable, IStackWalk, IDeserializationCallback
{
	private const string tagName = "PermissionSet";

	private const int version = 1;

	private static object[] psUnrestricted = new object[1] { PermissionState.Unrestricted };

	private PermissionState state;

	private ArrayList list;

	private PolicyLevel _policyLevel;

	private bool _declsec;

	private bool _readOnly;

	private bool[] _ignored;

	private static object[] action = new object[1] { (SecurityAction)0 };

	public virtual int Count => list.Count;

	public virtual bool IsSynchronized => list.IsSynchronized;

	public virtual bool IsReadOnly => false;

	public virtual object SyncRoot => this;

	internal bool DeclarativeSecurity
	{
		get
		{
			return _declsec;
		}
		set
		{
			_declsec = value;
		}
	}

	internal PolicyLevel Resolver
	{
		get
		{
			return _policyLevel;
		}
		set
		{
			_policyLevel = value;
		}
	}

	internal PermissionSet()
	{
		list = new ArrayList();
	}

	public PermissionSet(PermissionState state)
		: this()
	{
		this.state = CodeAccessPermission.CheckPermissionState(state, allowUnrestricted: true);
	}

	public PermissionSet(PermissionSet permSet)
		: this()
	{
		if (permSet == null)
		{
			return;
		}
		state = permSet.state;
		foreach (IPermission item in permSet.list)
		{
			list.Add(item);
		}
	}

	internal PermissionSet(string xml)
		: this()
	{
		state = PermissionState.None;
		if (xml != null)
		{
			SecurityElement et = SecurityElement.FromString(xml);
			FromXml(et);
		}
	}

	internal PermissionSet(IPermission perm)
		: this()
	{
		if (perm != null)
		{
			list.Add(perm);
		}
	}

	public IPermission AddPermission(IPermission perm)
	{
		if (perm == null || _readOnly)
		{
			return perm;
		}
		if (state == PermissionState.Unrestricted)
		{
			return (IPermission)Activator.CreateInstance(perm.GetType(), psUnrestricted);
		}
		IPermission permission = RemovePermission(perm.GetType());
		if (permission != null)
		{
			perm = perm.Union(permission);
		}
		list.Add(perm);
		return perm;
	}

	[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
	[SecurityPermission(SecurityAction.Demand, Assertion = true)]
	public void Assert()
	{
		int num = Count;
		foreach (IPermission item in list)
		{
			if (item is IStackWalk)
			{
				if (!SecurityManager.IsGranted(item))
				{
					return;
				}
			}
			else
			{
				num--;
			}
		}
		if (SecurityManager.SecurityEnabled && num > 0)
		{
			throw new NotSupportedException("Currently only declarative Assert are supported.");
		}
	}

	internal void Clear()
	{
		list.Clear();
	}

	public virtual PermissionSet Copy()
	{
		return new PermissionSet(this);
	}

	public virtual void CopyTo(Array array, int index)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (list.Count > 0)
		{
			if (array.Rank > 1)
			{
				throw new ArgumentException(Locale.GetText("Array has more than one dimension"));
			}
			if (index < 0 || index >= array.Length)
			{
				throw new IndexOutOfRangeException("index");
			}
			list.CopyTo(array, index);
		}
	}

	public void Demand()
	{
		if (IsEmpty())
		{
			return;
		}
		int count = list.Count;
		if (_ignored == null || _ignored.Length != count)
		{
			_ignored = new bool[count];
		}
		bool flag = IsUnrestricted();
		for (int i = 0; i < count; i++)
		{
			IPermission permission = (IPermission)list[i];
			if (permission.GetType().IsSubclassOf(typeof(CodeAccessPermission)))
			{
				_ignored[i] = false;
				flag = true;
			}
			else
			{
				_ignored[i] = true;
				permission.Demand();
			}
		}
		if (flag && SecurityManager.SecurityEnabled)
		{
			CasOnlyDemand(_declsec ? 5 : 3);
		}
	}

	internal void CasOnlyDemand(int skip)
	{
		if (_ignored == null)
		{
			_ignored = new bool[list.Count];
		}
	}

	[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
	public void Deny()
	{
		if (!SecurityManager.SecurityEnabled)
		{
			return;
		}
		foreach (IPermission item in list)
		{
			if (item is IStackWalk)
			{
				throw new NotSupportedException("Currently only declarative Deny are supported.");
			}
		}
	}

	public virtual void FromXml(SecurityElement et)
	{
		if (et == null)
		{
			throw new ArgumentNullException("et");
		}
		if (et.Tag != "PermissionSet")
		{
			throw new ArgumentException(string.Format("Invalid tag {0} expected {1}", et.Tag, "PermissionSet"), "et");
		}
		list.Clear();
		if (CodeAccessPermission.IsUnrestricted(et))
		{
			state = PermissionState.Unrestricted;
			return;
		}
		state = PermissionState.None;
		if (et.Children == null)
		{
			return;
		}
		foreach (SecurityElement child in et.Children)
		{
			string text = child.Attribute("class");
			if (text == null)
			{
				throw new ArgumentException(Locale.GetText("No permission class is specified."));
			}
			if (Resolver != null)
			{
				text = Resolver.ResolveClassName(text);
			}
			list.Add(PermissionBuilder.Create(text, child));
		}
	}

	public IEnumerator GetEnumerator()
	{
		return list.GetEnumerator();
	}

	public bool IsSubsetOf(PermissionSet target)
	{
		if (target == null || target.IsEmpty())
		{
			return IsEmpty();
		}
		if (target.IsUnrestricted())
		{
			return true;
		}
		if (IsUnrestricted())
		{
			return false;
		}
		if (IsUnrestricted() && (target == null || !target.IsUnrestricted()))
		{
			return false;
		}
		foreach (IPermission item in list)
		{
			Type type = item.GetType();
			IPermission permission2 = null;
			permission2 = ((!target.IsUnrestricted() || !(item is CodeAccessPermission) || !(item is IUnrestrictedPermission)) ? target.GetPermission(type) : ((IPermission)Activator.CreateInstance(type, psUnrestricted)));
			if (!item.IsSubsetOf(permission2))
			{
				return false;
			}
		}
		return true;
	}

	[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
	public void PermitOnly()
	{
		if (!SecurityManager.SecurityEnabled)
		{
			return;
		}
		foreach (IPermission item in list)
		{
			if (item is IStackWalk)
			{
				throw new NotSupportedException("Currently only declarative Deny are supported.");
			}
		}
	}

	public bool ContainsNonCodeAccessPermissions()
	{
		if (list.Count > 0)
		{
			foreach (IPermission item in list)
			{
				if (!item.GetType().IsSubclassOf(typeof(CodeAccessPermission)))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static byte[] ConvertPermissionSet(string inFormat, byte[] inData, string outFormat)
	{
		if (inFormat == null)
		{
			throw new ArgumentNullException("inFormat");
		}
		if (outFormat == null)
		{
			throw new ArgumentNullException("outFormat");
		}
		if (inData == null)
		{
			return null;
		}
		if (inFormat == outFormat)
		{
			return inData;
		}
		PermissionSet permissionSet = null;
		if (inFormat == "BINARY")
		{
			if (outFormat.StartsWith("XML"))
			{
				using (MemoryStream memoryStream = new MemoryStream(inData))
				{
					permissionSet = (PermissionSet)new BinaryFormatter().Deserialize(memoryStream);
					memoryStream.Close();
				}
				string s = permissionSet.ToString();
				switch (outFormat)
				{
				case "XML":
				case "XMLASCII":
					return Encoding.ASCII.GetBytes(s);
				case "XMLUNICODE":
					return Encoding.Unicode.GetBytes(s);
				}
			}
		}
		else
		{
			if (!inFormat.StartsWith("XML"))
			{
				return null;
			}
			if (outFormat == "BINARY")
			{
				string text = null;
				switch (inFormat)
				{
				case "XML":
				case "XMLASCII":
					text = Encoding.ASCII.GetString(inData);
					break;
				case "XMLUNICODE":
					text = Encoding.Unicode.GetString(inData);
					break;
				}
				if (text != null)
				{
					permissionSet = new PermissionSet(PermissionState.None);
					permissionSet.FromXml(SecurityElement.FromString(text));
					MemoryStream memoryStream2 = new MemoryStream();
					new BinaryFormatter().Serialize(memoryStream2, permissionSet);
					memoryStream2.Close();
					return memoryStream2.ToArray();
				}
			}
			else if (outFormat.StartsWith("XML"))
			{
				throw new XmlSyntaxException(string.Format(Locale.GetText("Can't convert from {0} to {1}"), inFormat, outFormat));
			}
		}
		throw new SerializationException(string.Format(Locale.GetText("Unknown output format {0}."), outFormat));
	}

	public IPermission GetPermission(Type permClass)
	{
		if (permClass == null || list.Count == 0)
		{
			return null;
		}
		foreach (object item in list)
		{
			if (item != null && item.GetType().Equals(permClass))
			{
				return (IPermission)item;
			}
		}
		return null;
	}

	public PermissionSet Intersect(PermissionSet other)
	{
		if (other == null || other.IsEmpty() || IsEmpty())
		{
			return null;
		}
		PermissionState permissionState = PermissionState.None;
		if (IsUnrestricted() && other.IsUnrestricted())
		{
			permissionState = PermissionState.Unrestricted;
		}
		PermissionSet permissionSet = null;
		if (permissionState == PermissionState.Unrestricted)
		{
			permissionSet = new PermissionSet(permissionState);
		}
		else if (IsUnrestricted())
		{
			permissionSet = other.Copy();
		}
		else if (other.IsUnrestricted())
		{
			permissionSet = Copy();
		}
		else
		{
			permissionSet = new PermissionSet(permissionState);
			InternalIntersect(permissionSet, this, other, unrestricted: false);
		}
		return permissionSet;
	}

	internal void InternalIntersect(PermissionSet intersect, PermissionSet a, PermissionSet b, bool unrestricted)
	{
		foreach (IPermission item in b.list)
		{
			IPermission permission2 = a.GetPermission(item.GetType());
			if (permission2 != null)
			{
				intersect.AddPermission(item.Intersect(permission2));
			}
			else if (unrestricted)
			{
				intersect.AddPermission(item);
			}
		}
	}

	public bool IsEmpty()
	{
		if (state == PermissionState.Unrestricted)
		{
			return false;
		}
		if (list == null || list.Count == 0)
		{
			return true;
		}
		foreach (IPermission item in list)
		{
			if (!item.IsSubsetOf(null))
			{
				return false;
			}
		}
		return true;
	}

	public bool IsUnrestricted()
	{
		return state == PermissionState.Unrestricted;
	}

	public IPermission RemovePermission(Type permClass)
	{
		if (permClass == null || _readOnly)
		{
			return null;
		}
		foreach (object item in list)
		{
			if (item.GetType().Equals(permClass))
			{
				list.Remove(item);
				return (IPermission)item;
			}
		}
		return null;
	}

	public IPermission SetPermission(IPermission perm)
	{
		if (perm == null || _readOnly)
		{
			return perm;
		}
		if (!(perm is IUnrestrictedPermission unrestrictedPermission))
		{
			state = PermissionState.None;
		}
		else
		{
			state = (unrestrictedPermission.IsUnrestricted() ? state : PermissionState.None);
		}
		RemovePermission(perm.GetType());
		list.Add(perm);
		return perm;
	}

	public override string ToString()
	{
		return ToXml().ToString();
	}

	public virtual SecurityElement ToXml()
	{
		SecurityElement securityElement = new SecurityElement("PermissionSet");
		securityElement.AddAttribute("class", GetType().FullName);
		securityElement.AddAttribute("version", 1.ToString());
		if (state == PermissionState.Unrestricted)
		{
			securityElement.AddAttribute("Unrestricted", "true");
		}
		foreach (IPermission item in list)
		{
			securityElement.AddChild(item.ToXml());
		}
		return securityElement;
	}

	public PermissionSet Union(PermissionSet other)
	{
		if (other == null)
		{
			return Copy();
		}
		PermissionSet permissionSet = null;
		if (IsUnrestricted() || other.IsUnrestricted())
		{
			return new PermissionSet(PermissionState.Unrestricted);
		}
		permissionSet = Copy();
		foreach (IPermission item in other.list)
		{
			permissionSet.AddPermission(item);
		}
		return permissionSet;
	}

	[MonoTODO("may not be required")]
	void IDeserializationCallback.OnDeserialization(object sender)
	{
	}

	[ComVisible(false)]
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (!(obj is PermissionSet permissionSet))
		{
			return false;
		}
		if (state != permissionSet.state)
		{
			return false;
		}
		if (list.Count != permissionSet.Count)
		{
			return false;
		}
		for (int i = 0; i < list.Count; i++)
		{
			bool flag = false;
			int num = 0;
			while (i < permissionSet.list.Count)
			{
				if (list[i].Equals(permissionSet.list[num]))
				{
					flag = true;
					break;
				}
				num++;
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	[ComVisible(false)]
	public override int GetHashCode()
	{
		if (list.Count != 0)
		{
			return base.GetHashCode();
		}
		return (int)state;
	}

	public static void RevertAssert()
	{
	}

	internal void SetReadOnly(bool value)
	{
		_readOnly = value;
	}

	private bool AllIgnored()
	{
		if (_ignored == null)
		{
			throw new NotSupportedException("bad bad bad");
		}
		for (int i = 0; i < _ignored.Length; i++)
		{
			if (!_ignored[i])
			{
				return false;
			}
		}
		return true;
	}

	internal static PermissionSet CreateFromBinaryFormat(byte[] data)
	{
		if (data == null || data[0] != 46 || data.Length < 2)
		{
			throw new SecurityException(Locale.GetText("Invalid data in 2.0 metadata format."));
		}
		int position = 1;
		int num = ReadEncodedInt(data, ref position);
		PermissionSet permissionSet = new PermissionSet(PermissionState.None);
		for (int i = 0; i < num; i++)
		{
			IPermission permission = ProcessAttribute(data, ref position);
			if (permission == null)
			{
				throw new SecurityException(Locale.GetText("Unsupported data found in 2.0 metadata format."));
			}
			permissionSet.AddPermission(permission);
		}
		return permissionSet;
	}

	internal static int ReadEncodedInt(byte[] data, ref int position)
	{
		int num = 0;
		if ((data[position] & 0x80) == 0)
		{
			num = data[position];
			position++;
		}
		else if ((data[position] & 0x40) == 0)
		{
			num = ((data[position] & 0x3F) << 8) | data[position + 1];
			position += 2;
		}
		else
		{
			num = ((data[position] & 0x1F) << 24) | (data[position + 1] << 16) | (data[position + 2] << 8) | data[position + 3];
			position += 4;
		}
		return num;
	}

	internal static IPermission ProcessAttribute(byte[] data, ref int position)
	{
		int num = ReadEncodedInt(data, ref position);
		string typeName = Encoding.UTF8.GetString(data, position, num);
		position += num;
		Type type = Type.GetType(typeName);
		if (!(Activator.CreateInstance(type, action) is SecurityAttribute securityAttribute))
		{
			return null;
		}
		ReadEncodedInt(data, ref position);
		int num2 = ReadEncodedInt(data, ref position);
		for (int i = 0; i < num2; i++)
		{
			bool flag = false;
			switch (data[position++])
			{
			case 83:
				flag = false;
				break;
			case 84:
				flag = true;
				break;
			default:
				return null;
			}
			bool flag2 = false;
			byte b = data[position++];
			if (b == 29)
			{
				flag2 = true;
				b = data[position++];
			}
			int num3 = ReadEncodedInt(data, ref position);
			string name = Encoding.UTF8.GetString(data, position, num3);
			position += num3;
			int num4 = 1;
			if (flag2)
			{
				num4 = BitConverter.ToInt32(data, position);
				position += 4;
			}
			object obj = null;
			object[] index = null;
			for (int j = 0; j < num4; j++)
			{
				switch (b)
				{
				case 2:
					obj = Convert.ToBoolean(data[position++]);
					break;
				case 3:
					obj = Convert.ToChar(data[position]);
					position += 2;
					break;
				case 4:
					obj = Convert.ToSByte(data[position++]);
					break;
				case 5:
					obj = Convert.ToByte(data[position++]);
					break;
				case 6:
					obj = Convert.ToInt16(data[position]);
					position += 2;
					break;
				case 7:
					obj = Convert.ToUInt16(data[position]);
					position += 2;
					break;
				case 8:
					obj = Convert.ToInt32(data[position]);
					position += 4;
					break;
				case 9:
					obj = Convert.ToUInt32(data[position]);
					position += 4;
					break;
				case 10:
					obj = Convert.ToInt64(data[position]);
					position += 8;
					break;
				case 11:
					obj = Convert.ToUInt64(data[position]);
					position += 8;
					break;
				case 12:
					obj = Convert.ToSingle(data[position]);
					position += 4;
					break;
				case 13:
					obj = Convert.ToDouble(data[position]);
					position += 8;
					break;
				case 14:
				{
					string text = null;
					if (data[position] != byte.MaxValue)
					{
						int num6 = ReadEncodedInt(data, ref position);
						text = Encoding.UTF8.GetString(data, position, num6);
						position += num6;
					}
					else
					{
						position++;
					}
					obj = text;
					break;
				}
				case 80:
				{
					int num5 = ReadEncodedInt(data, ref position);
					obj = Type.GetType(Encoding.UTF8.GetString(data, position, num5));
					position += num5;
					break;
				}
				default:
					return null;
				}
				if (flag)
				{
					type.GetProperty(name).SetValue(securityAttribute, obj, index);
				}
				else
				{
					type.GetField(name).SetValue(securityAttribute, obj);
				}
			}
		}
		return securityAttribute.CreatePermission();
	}
}
