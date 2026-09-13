using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Security.Claims;

[Serializable]
[ComVisible(true)]
public class ClaimsPrincipal : IPrincipal
{
	private enum SerializationMask
	{
		None,
		HasIdentities,
		UserData
	}

	[NonSerialized]
	private byte[] m_userSerializationData;

	[NonSerialized]
	private const string PreFix = "System.Security.ClaimsPrincipal.";

	[NonSerialized]
	private const string IdentitiesKey = "System.Security.ClaimsPrincipal.Identities";

	[NonSerialized]
	private const string VersionKey = "System.Security.ClaimsPrincipal.Version";

	[OptionalField(VersionAdded = 2)]
	private string m_version = "1.0";

	[OptionalField(VersionAdded = 2)]
	private string m_serializedClaimsIdentities;

	[NonSerialized]
	private List<ClaimsIdentity> m_identities = new List<ClaimsIdentity>();

	[NonSerialized]
	private static Func<IEnumerable<ClaimsIdentity>, ClaimsIdentity> s_identitySelector = SelectPrimaryIdentity;

	[NonSerialized]
	private static Func<ClaimsPrincipal> s_principalSelector = ClaimsPrincipalSelector;

	public static Func<IEnumerable<ClaimsIdentity>, ClaimsIdentity> PrimaryIdentitySelector
	{
		get
		{
			return s_identitySelector;
		}
		[SecurityCritical]
		set
		{
			s_identitySelector = value;
		}
	}

	public static Func<ClaimsPrincipal> ClaimsPrincipalSelector
	{
		get
		{
			return s_principalSelector;
		}
		[SecurityCritical]
		set
		{
			s_principalSelector = value;
		}
	}

	protected virtual byte[] CustomSerializationData => m_userSerializationData;

	public virtual IEnumerable<Claim> Claims
	{
		get
		{
			foreach (ClaimsIdentity identity in Identities)
			{
				foreach (Claim claim in identity.Claims)
				{
					yield return claim;
				}
			}
		}
	}

	public static ClaimsPrincipal Current
	{
		get
		{
			if (s_principalSelector != null)
			{
				return s_principalSelector();
			}
			return SelectClaimsPrincipal();
		}
	}

	public virtual IEnumerable<ClaimsIdentity> Identities => m_identities.AsReadOnly();

	public virtual IIdentity Identity
	{
		get
		{
			if (s_identitySelector != null)
			{
				return s_identitySelector(m_identities);
			}
			return SelectPrimaryIdentity(m_identities);
		}
	}

	private static ClaimsIdentity SelectPrimaryIdentity(IEnumerable<ClaimsIdentity> identities)
	{
		if (identities == null)
		{
			throw new ArgumentNullException("identities");
		}
		ClaimsIdentity claimsIdentity = null;
		foreach (ClaimsIdentity identity in identities)
		{
			if (identity is WindowsIdentity)
			{
				claimsIdentity = identity;
				break;
			}
			if (claimsIdentity == null)
			{
				claimsIdentity = identity;
			}
		}
		return claimsIdentity;
	}

	private static ClaimsPrincipal SelectClaimsPrincipal()
	{
		if (Thread.CurrentPrincipal is ClaimsPrincipal result)
		{
			return result;
		}
		return new ClaimsPrincipal(Thread.CurrentPrincipal);
	}

	public ClaimsPrincipal()
	{
	}

	public ClaimsPrincipal(IEnumerable<ClaimsIdentity> identities)
	{
		if (identities == null)
		{
			throw new ArgumentNullException("identities");
		}
		m_identities.AddRange(identities);
	}

	public ClaimsPrincipal(IIdentity identity)
	{
		if (identity == null)
		{
			throw new ArgumentNullException("identity");
		}
		if (identity is ClaimsIdentity item)
		{
			m_identities.Add(item);
		}
		else
		{
			m_identities.Add(new ClaimsIdentity(identity));
		}
	}

	public ClaimsPrincipal(IPrincipal principal)
	{
		if (principal == null)
		{
			throw new ArgumentNullException("principal");
		}
		if (!(principal is ClaimsPrincipal claimsPrincipal))
		{
			m_identities.Add(new ClaimsIdentity(principal.Identity));
		}
		else if (claimsPrincipal.Identities != null)
		{
			m_identities.AddRange(claimsPrincipal.Identities);
		}
	}

	public ClaimsPrincipal(BinaryReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		Initialize(reader);
	}

	[SecurityCritical]
	protected ClaimsPrincipal(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		Deserialize(info, context);
	}

	public virtual ClaimsPrincipal Clone()
	{
		return new ClaimsPrincipal(this);
	}

	protected virtual ClaimsIdentity CreateClaimsIdentity(BinaryReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		return new ClaimsIdentity(reader);
	}

	[OnSerializing]
	[SecurityCritical]
	private void OnSerializingMethod(StreamingContext context)
	{
		if (!(this is ISerializable))
		{
			m_serializedClaimsIdentities = SerializeIdentities();
		}
	}

	[OnDeserialized]
	[SecurityCritical]
	private void OnDeserializedMethod(StreamingContext context)
	{
		if (!(this is ISerializable))
		{
			DeserializeIdentities(m_serializedClaimsIdentities);
			m_serializedClaimsIdentities = null;
		}
	}

	[SecurityCritical]
	[SecurityPermission(SecurityAction.Assert, SerializationFormatter = true)]
	protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("System.Security.ClaimsPrincipal.Identities", SerializeIdentities());
		info.AddValue("System.Security.ClaimsPrincipal.Version", m_version);
	}

	[SecurityCritical]
	[SecurityPermission(SecurityAction.Assert, SerializationFormatter = true)]
	private void Deserialize(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		SerializationInfoEnumerator enumerator = info.GetEnumerator();
		while (enumerator.MoveNext())
		{
			string name = enumerator.Name;
			if (!(name == "System.Security.ClaimsPrincipal.Identities"))
			{
				if (name == "System.Security.ClaimsPrincipal.Version")
				{
					m_version = info.GetString("System.Security.ClaimsPrincipal.Version");
				}
			}
			else
			{
				DeserializeIdentities(info.GetString("System.Security.ClaimsPrincipal.Identities"));
			}
		}
	}

	[SecurityCritical]
	private void DeserializeIdentities(string identities)
	{
		m_identities = new List<ClaimsIdentity>();
		if (string.IsNullOrEmpty(identities))
		{
			return;
		}
		List<string> list = null;
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		using MemoryStream serializationStream = new MemoryStream(Convert.FromBase64String(identities));
		list = (List<string>)binaryFormatter.Deserialize(serializationStream, null, fCheck: false);
		for (int i = 0; i < list.Count; i += 2)
		{
			ClaimsIdentity claimsIdentity = null;
			using (MemoryStream serializationStream2 = new MemoryStream(Convert.FromBase64String(list[i + 1])))
			{
				claimsIdentity = (ClaimsIdentity)binaryFormatter.Deserialize(serializationStream2, null, fCheck: false);
			}
			if (!string.IsNullOrEmpty(list[i]))
			{
				if (!long.TryParse(list[i], NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out var result))
				{
					throw new SerializationException(Environment.GetResourceString("Invalid BinaryFormatter stream."));
				}
				claimsIdentity = new WindowsIdentity(claimsIdentity, new IntPtr(result));
			}
			m_identities.Add(claimsIdentity);
		}
	}

	[SecurityCritical]
	private string SerializeIdentities()
	{
		List<string> list = new List<string>();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		foreach (ClaimsIdentity identity in m_identities)
		{
			if (identity.GetType() == typeof(WindowsIdentity))
			{
				WindowsIdentity windowsIdentity = identity as WindowsIdentity;
				list.Add(windowsIdentity.GetTokenInternal().ToInt64().ToString(NumberFormatInfo.InvariantInfo));
				using MemoryStream memoryStream = new MemoryStream();
				binaryFormatter.Serialize(memoryStream, windowsIdentity.CloneAsBase(), null, fCheck: false);
				list.Add(Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length));
			}
			else
			{
				using MemoryStream memoryStream2 = new MemoryStream();
				list.Add("");
				binaryFormatter.Serialize(memoryStream2, identity, null, fCheck: false);
				list.Add(Convert.ToBase64String(memoryStream2.GetBuffer(), 0, (int)memoryStream2.Length));
			}
		}
		using MemoryStream memoryStream3 = new MemoryStream();
		binaryFormatter.Serialize(memoryStream3, list, null, fCheck: false);
		return Convert.ToBase64String(memoryStream3.GetBuffer(), 0, (int)memoryStream3.Length);
	}

	[SecurityCritical]
	public virtual void AddIdentity(ClaimsIdentity identity)
	{
		if (identity == null)
		{
			throw new ArgumentNullException("identity");
		}
		m_identities.Add(identity);
	}

	[SecurityCritical]
	public virtual void AddIdentities(IEnumerable<ClaimsIdentity> identities)
	{
		if (identities == null)
		{
			throw new ArgumentNullException("identities");
		}
		m_identities.AddRange(identities);
	}

	public virtual IEnumerable<Claim> FindAll(Predicate<Claim> match)
	{
		if (match == null)
		{
			throw new ArgumentNullException("match");
		}
		List<Claim> list = new List<Claim>();
		foreach (ClaimsIdentity identity in Identities)
		{
			if (identity == null)
			{
				continue;
			}
			foreach (Claim item in identity.FindAll(match))
			{
				list.Add(item);
			}
		}
		return list.AsReadOnly();
	}

	public virtual IEnumerable<Claim> FindAll(string type)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		List<Claim> list = new List<Claim>();
		foreach (ClaimsIdentity identity in Identities)
		{
			if (identity == null)
			{
				continue;
			}
			foreach (Claim item in identity.FindAll(type))
			{
				list.Add(item);
			}
		}
		return list.AsReadOnly();
	}

	public virtual Claim FindFirst(Predicate<Claim> match)
	{
		if (match == null)
		{
			throw new ArgumentNullException("match");
		}
		Claim claim = null;
		foreach (ClaimsIdentity identity in Identities)
		{
			if (identity != null)
			{
				claim = identity.FindFirst(match);
				if (claim != null)
				{
					return claim;
				}
			}
		}
		return claim;
	}

	public virtual Claim FindFirst(string type)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		Claim claim = null;
		for (int i = 0; i < m_identities.Count; i++)
		{
			if (m_identities[i] != null)
			{
				claim = m_identities[i].FindFirst(type);
				if (claim != null)
				{
					return claim;
				}
			}
		}
		return claim;
	}

	public virtual bool HasClaim(Predicate<Claim> match)
	{
		if (match == null)
		{
			throw new ArgumentNullException("match");
		}
		for (int i = 0; i < m_identities.Count; i++)
		{
			if (m_identities[i] != null && m_identities[i].HasClaim(match))
			{
				return true;
			}
		}
		return false;
	}

	public virtual bool HasClaim(string type, string value)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		for (int i = 0; i < m_identities.Count; i++)
		{
			if (m_identities[i] != null && m_identities[i].HasClaim(type, value))
			{
				return true;
			}
		}
		return false;
	}

	public virtual bool IsInRole(string role)
	{
		for (int i = 0; i < m_identities.Count; i++)
		{
			if (m_identities[i] != null && m_identities[i].HasClaim(m_identities[i].RoleClaimType, role))
			{
				return true;
			}
		}
		return false;
	}

	private void Initialize(BinaryReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		SerializationMask serializationMask = (SerializationMask)reader.ReadInt32();
		int num = reader.ReadInt32();
		int num2 = 0;
		if ((serializationMask & SerializationMask.HasIdentities) == SerializationMask.HasIdentities)
		{
			num2++;
			int num3 = reader.ReadInt32();
			for (int i = 0; i < num3; i++)
			{
				m_identities.Add(CreateClaimsIdentity(reader));
			}
		}
		if ((serializationMask & SerializationMask.UserData) == SerializationMask.UserData)
		{
			int count = reader.ReadInt32();
			m_userSerializationData = reader.ReadBytes(count);
			num2++;
		}
		for (int j = num2; j < num; j++)
		{
			reader.ReadString();
		}
	}

	public virtual void WriteTo(BinaryWriter writer)
	{
		WriteTo(writer, null);
	}

	protected virtual void WriteTo(BinaryWriter writer, byte[] userData)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		int num = 0;
		SerializationMask serializationMask = SerializationMask.None;
		if (m_identities.Count > 0)
		{
			serializationMask |= SerializationMask.HasIdentities;
			num++;
		}
		if (userData != null && userData.Length != 0)
		{
			num++;
			serializationMask |= SerializationMask.UserData;
		}
		writer.Write((int)serializationMask);
		writer.Write(num);
		if ((serializationMask & SerializationMask.HasIdentities) == SerializationMask.HasIdentities)
		{
			writer.Write(m_identities.Count);
			foreach (ClaimsIdentity identity in m_identities)
			{
				identity.WriteTo(writer);
			}
		}
		if ((serializationMask & SerializationMask.UserData) == SerializationMask.UserData)
		{
			writer.Write(userData.Length);
			writer.Write(userData);
		}
		writer.Flush();
	}
}
