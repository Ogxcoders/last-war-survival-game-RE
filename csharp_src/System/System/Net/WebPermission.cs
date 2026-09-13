using System.Collections;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace System.Net;

[Serializable]
public sealed class WebPermission : CodeAccessPermission, IUnrestrictedPermission
{
	private bool m_noRestriction;

	[OptionalField]
	private bool m_UnrestrictedConnect;

	[OptionalField]
	private bool m_UnrestrictedAccept;

	private ArrayList m_connectList = new ArrayList();

	private ArrayList m_acceptList = new ArrayList();

	internal const string MatchAll = ".*";

	private static volatile Regex s_MatchAllRegex;

	internal static Regex MatchAllRegex
	{
		get
		{
			if (s_MatchAllRegex == null)
			{
				s_MatchAllRegex = new Regex(".*");
			}
			return s_MatchAllRegex;
		}
	}

	public IEnumerator ConnectList
	{
		get
		{
			if (m_UnrestrictedConnect)
			{
				return new Regex[1] { MatchAllRegex }.GetEnumerator();
			}
			ArrayList arrayList = new ArrayList(m_connectList.Count);
			for (int i = 0; i < m_connectList.Count; i++)
			{
				arrayList.Add((m_connectList[i] is DelayedRegex) ? ((DelayedRegex)m_connectList[i]).AsRegex : ((m_connectList[i] is Uri) ? ((Uri)m_connectList[i]).GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped) : m_connectList[i]));
			}
			return arrayList.GetEnumerator();
		}
	}

	public IEnumerator AcceptList
	{
		get
		{
			if (m_UnrestrictedAccept)
			{
				return new Regex[1] { MatchAllRegex }.GetEnumerator();
			}
			ArrayList arrayList = new ArrayList(m_acceptList.Count);
			for (int i = 0; i < m_acceptList.Count; i++)
			{
				arrayList.Add((m_acceptList[i] is DelayedRegex) ? ((DelayedRegex)m_acceptList[i]).AsRegex : ((m_acceptList[i] is Uri) ? ((Uri)m_acceptList[i]).GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped) : m_acceptList[i]));
			}
			return arrayList.GetEnumerator();
		}
	}

	public WebPermission(PermissionState state)
	{
		m_noRestriction = state == PermissionState.Unrestricted;
	}

	internal WebPermission(bool unrestricted)
	{
		m_noRestriction = unrestricted;
	}

	public WebPermission()
	{
	}

	internal WebPermission(NetworkAccess access)
	{
		m_UnrestrictedConnect = (access & NetworkAccess.Connect) != 0;
		m_UnrestrictedAccept = (access & NetworkAccess.Accept) != 0;
	}

	public WebPermission(NetworkAccess access, Regex uriRegex)
	{
		AddPermission(access, uriRegex);
	}

	public WebPermission(NetworkAccess access, string uriString)
	{
		AddPermission(access, uriString);
	}

	internal WebPermission(NetworkAccess access, Uri uri)
	{
		AddPermission(access, uri);
	}

	public void AddPermission(NetworkAccess access, string uriString)
	{
		if (uriString == null)
		{
			throw new ArgumentNullException("uriString");
		}
		if (m_noRestriction)
		{
			return;
		}
		if (Uri.TryCreate(uriString, UriKind.Absolute, out var result))
		{
			AddPermission(access, result);
			return;
		}
		ArrayList arrayList = new ArrayList();
		if ((access & NetworkAccess.Connect) != 0 && !m_UnrestrictedConnect)
		{
			arrayList.Add(m_connectList);
		}
		if ((access & NetworkAccess.Accept) != 0 && !m_UnrestrictedAccept)
		{
			arrayList.Add(m_acceptList);
		}
		foreach (ArrayList item in arrayList)
		{
			bool flag = false;
			foreach (object item2 in item)
			{
				if (item2 is string strA && string.Compare(strA, uriString, StringComparison.OrdinalIgnoreCase) == 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				item.Add(uriString);
			}
		}
	}

	internal void AddPermission(NetworkAccess access, Uri uri)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		if (m_noRestriction)
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		if ((access & NetworkAccess.Connect) != 0 && !m_UnrestrictedConnect)
		{
			arrayList.Add(m_connectList);
		}
		if ((access & NetworkAccess.Accept) != 0 && !m_UnrestrictedAccept)
		{
			arrayList.Add(m_acceptList);
		}
		foreach (ArrayList item in arrayList)
		{
			bool flag = false;
			foreach (object item2 in item)
			{
				if (item2 is Uri && uri.Equals(item2))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				item.Add(uri);
			}
		}
	}

	public void AddPermission(NetworkAccess access, Regex uriRegex)
	{
		if (uriRegex == null)
		{
			throw new ArgumentNullException("uriRegex");
		}
		if (m_noRestriction)
		{
			return;
		}
		if (uriRegex.ToString() == ".*")
		{
			if (!m_UnrestrictedConnect && (access & NetworkAccess.Connect) != 0)
			{
				m_UnrestrictedConnect = true;
				m_connectList.Clear();
			}
			if (!m_UnrestrictedAccept && (access & NetworkAccess.Accept) != 0)
			{
				m_UnrestrictedAccept = true;
				m_acceptList.Clear();
			}
		}
		else
		{
			AddAsPattern(access, new DelayedRegex(uriRegex));
		}
	}

	internal void AddAsPattern(NetworkAccess access, DelayedRegex uriRegexPattern)
	{
		ArrayList arrayList = new ArrayList();
		if ((access & NetworkAccess.Connect) != 0 && !m_UnrestrictedConnect)
		{
			arrayList.Add(m_connectList);
		}
		if ((access & NetworkAccess.Accept) != 0 && !m_UnrestrictedAccept)
		{
			arrayList.Add(m_acceptList);
		}
		foreach (ArrayList item in arrayList)
		{
			bool flag = false;
			foreach (object item2 in item)
			{
				if (item2 is DelayedRegex && string.Compare(uriRegexPattern.ToString(), item2.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				item.Add(uriRegexPattern);
			}
		}
	}

	public bool IsUnrestricted()
	{
		return m_noRestriction;
	}

	public override IPermission Copy()
	{
		if (m_noRestriction)
		{
			return new WebPermission(unrestricted: true);
		}
		return new WebPermission((NetworkAccess)((m_UnrestrictedConnect ? 64 : 0) | (m_UnrestrictedAccept ? 128 : 0)))
		{
			m_acceptList = (ArrayList)m_acceptList.Clone(),
			m_connectList = (ArrayList)m_connectList.Clone()
		};
	}

	public override bool IsSubsetOf(IPermission target)
	{
		if (target == null)
		{
			if (!m_noRestriction && !m_UnrestrictedConnect && !m_UnrestrictedAccept && m_connectList.Count == 0)
			{
				return m_acceptList.Count == 0;
			}
			return false;
		}
		if (!(target is WebPermission webPermission))
		{
			throw new ArgumentException(global::SR.GetString("Cannot cast target permission type."), "target");
		}
		if (webPermission.m_noRestriction)
		{
			return true;
		}
		if (m_noRestriction)
		{
			return false;
		}
		if (!webPermission.m_UnrestrictedAccept)
		{
			if (m_UnrestrictedAccept)
			{
				return false;
			}
			if (m_acceptList.Count != 0)
			{
				if (webPermission.m_acceptList.Count == 0)
				{
					return false;
				}
				foreach (object accept in m_acceptList)
				{
					if (accept is DelayedRegex)
					{
						if (!isSpecialSubsetCase(accept.ToString(), webPermission.m_acceptList))
						{
							throw new NotSupportedException(global::SR.GetString("Cannot subset Regex. Only support if both patterns are identical."));
						}
					}
					else if (!isMatchedURI(accept, webPermission.m_acceptList))
					{
						return false;
					}
				}
			}
		}
		if (!webPermission.m_UnrestrictedConnect)
		{
			if (m_UnrestrictedConnect)
			{
				return false;
			}
			if (m_connectList.Count != 0)
			{
				if (webPermission.m_connectList.Count == 0)
				{
					return false;
				}
				foreach (object connect in m_connectList)
				{
					if (connect is DelayedRegex)
					{
						if (!isSpecialSubsetCase(connect.ToString(), webPermission.m_connectList))
						{
							throw new NotSupportedException(global::SR.GetString("Cannot subset Regex. Only support if both patterns are identical."));
						}
					}
					else if (!isMatchedURI(connect, webPermission.m_connectList))
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	private static bool isSpecialSubsetCase(string regexToCheck, ArrayList permList)
	{
		foreach (object perm in permList)
		{
			Uri uri;
			if (perm is DelayedRegex delayedRegex)
			{
				if (string.Compare(regexToCheck, delayedRegex.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			else if ((uri = perm as Uri) != null)
			{
				if (string.Compare(regexToCheck, Regex.Escape(uri.GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped)), StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			else if (string.Compare(regexToCheck, Regex.Escape(perm.ToString()), StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
		}
		return false;
	}

	public override IPermission Union(IPermission target)
	{
		if (target == null)
		{
			return Copy();
		}
		if (!(target is WebPermission webPermission))
		{
			throw new ArgumentException(global::SR.GetString("Cannot cast target permission type."), "target");
		}
		if (m_noRestriction || webPermission.m_noRestriction)
		{
			return new WebPermission(unrestricted: true);
		}
		WebPermission webPermission2 = new WebPermission();
		if (m_UnrestrictedConnect || webPermission.m_UnrestrictedConnect)
		{
			webPermission2.m_UnrestrictedConnect = true;
		}
		else
		{
			webPermission2.m_connectList = (ArrayList)webPermission.m_connectList.Clone();
			for (int i = 0; i < m_connectList.Count; i++)
			{
				if (!(m_connectList[i] is DelayedRegex uriRegexPattern))
				{
					if (m_connectList[i] is string)
					{
						webPermission2.AddPermission(NetworkAccess.Connect, (string)m_connectList[i]);
					}
					else
					{
						webPermission2.AddPermission(NetworkAccess.Connect, (Uri)m_connectList[i]);
					}
				}
				else
				{
					webPermission2.AddAsPattern(NetworkAccess.Connect, uriRegexPattern);
				}
			}
		}
		if (m_UnrestrictedAccept || webPermission.m_UnrestrictedAccept)
		{
			webPermission2.m_UnrestrictedAccept = true;
		}
		else
		{
			webPermission2.m_acceptList = (ArrayList)webPermission.m_acceptList.Clone();
			for (int j = 0; j < m_acceptList.Count; j++)
			{
				if (!(m_acceptList[j] is DelayedRegex uriRegexPattern2))
				{
					if (m_acceptList[j] is string)
					{
						webPermission2.AddPermission(NetworkAccess.Accept, (string)m_acceptList[j]);
					}
					else
					{
						webPermission2.AddPermission(NetworkAccess.Accept, (Uri)m_acceptList[j]);
					}
				}
				else
				{
					webPermission2.AddAsPattern(NetworkAccess.Accept, uriRegexPattern2);
				}
			}
		}
		return webPermission2;
	}

	public override IPermission Intersect(IPermission target)
	{
		if (target == null)
		{
			return null;
		}
		if (!(target is WebPermission webPermission))
		{
			throw new ArgumentException(global::SR.GetString("Cannot cast target permission type."), "target");
		}
		if (m_noRestriction)
		{
			return webPermission.Copy();
		}
		if (webPermission.m_noRestriction)
		{
			return Copy();
		}
		WebPermission webPermission2 = new WebPermission();
		if (m_UnrestrictedConnect && webPermission.m_UnrestrictedConnect)
		{
			webPermission2.m_UnrestrictedConnect = true;
		}
		else if (m_UnrestrictedConnect || webPermission.m_UnrestrictedConnect)
		{
			webPermission2.m_connectList = (ArrayList)(m_UnrestrictedConnect ? webPermission : this).m_connectList.Clone();
		}
		else
		{
			intersectList(m_connectList, webPermission.m_connectList, webPermission2.m_connectList);
		}
		if (m_UnrestrictedAccept && webPermission.m_UnrestrictedAccept)
		{
			webPermission2.m_UnrestrictedAccept = true;
		}
		else if (m_UnrestrictedAccept || webPermission.m_UnrestrictedAccept)
		{
			webPermission2.m_acceptList = (ArrayList)(m_UnrestrictedAccept ? webPermission : this).m_acceptList.Clone();
		}
		else
		{
			intersectList(m_acceptList, webPermission.m_acceptList, webPermission2.m_acceptList);
		}
		if (!webPermission2.m_UnrestrictedConnect && !webPermission2.m_UnrestrictedAccept && webPermission2.m_connectList.Count == 0 && webPermission2.m_acceptList.Count == 0)
		{
			return null;
		}
		return webPermission2;
	}

	public override void FromXml(SecurityElement securityElement)
	{
		if (securityElement == null)
		{
			throw new ArgumentNullException("securityElement");
		}
		if (!securityElement.Tag.Equals("IPermission"))
		{
			throw new ArgumentException(global::SR.GetString("Specified value does not contain 'IPermission' as its tag."), "securityElement");
		}
		if ((securityElement.Attribute("class") ?? throw new ArgumentException(global::SR.GetString("Specified value does not contain a 'class' attribute."), "securityElement")).IndexOf(GetType().FullName) < 0)
		{
			throw new ArgumentException(global::SR.GetString("The value class attribute is not valid."), "securityElement");
		}
		string text = securityElement.Attribute("Unrestricted");
		m_connectList = new ArrayList();
		m_acceptList = new ArrayList();
		m_UnrestrictedAccept = (m_UnrestrictedConnect = false);
		if (text != null && string.Compare(text, "true", StringComparison.OrdinalIgnoreCase) == 0)
		{
			m_noRestriction = true;
			return;
		}
		m_noRestriction = false;
		SecurityElement securityElement2 = securityElement.SearchForChildByTag("ConnectAccess");
		if (securityElement2 != null)
		{
			foreach (SecurityElement child in securityElement2.Children)
			{
				if (child.Tag.Equals("URI"))
				{
					string text2;
					try
					{
						text2 = child.Attribute("uri");
					}
					catch
					{
						text2 = null;
					}
					if (text2 == null)
					{
						throw new ArgumentException(global::SR.GetString("The '{0}' element contains one or more invalid values."), "ConnectAccess");
					}
					if (text2 == ".*")
					{
						m_UnrestrictedConnect = true;
						m_connectList = new ArrayList();
						break;
					}
					AddAsPattern(NetworkAccess.Connect, new DelayedRegex(text2));
				}
			}
		}
		securityElement2 = securityElement.SearchForChildByTag("AcceptAccess");
		if (securityElement2 == null)
		{
			return;
		}
		foreach (SecurityElement child2 in securityElement2.Children)
		{
			if (child2.Tag.Equals("URI"))
			{
				string text2;
				try
				{
					text2 = child2.Attribute("uri");
				}
				catch
				{
					text2 = null;
				}
				if (text2 == null)
				{
					throw new ArgumentException(global::SR.GetString("The '{0}' element contains one or more invalid values."), "AcceptAccess");
				}
				if (text2 == ".*")
				{
					m_UnrestrictedAccept = true;
					m_acceptList = new ArrayList();
					break;
				}
				AddAsPattern(NetworkAccess.Accept, new DelayedRegex(text2));
			}
		}
	}

	public override SecurityElement ToXml()
	{
		SecurityElement securityElement = new SecurityElement("IPermission");
		securityElement.AddAttribute("class", GetType().FullName + ", " + GetType().Module.Assembly.FullName.Replace('"', '\''));
		securityElement.AddAttribute("version", "1");
		if (!IsUnrestricted())
		{
			string text = null;
			if (m_UnrestrictedConnect || m_connectList.Count > 0)
			{
				SecurityElement securityElement2 = new SecurityElement("ConnectAccess");
				if (m_UnrestrictedConnect)
				{
					SecurityElement securityElement3 = new SecurityElement("URI");
					securityElement3.AddAttribute("uri", SecurityElement.Escape(".*"));
					securityElement2.AddChild(securityElement3);
				}
				else
				{
					foreach (object connect in m_connectList)
					{
						Uri uri = connect as Uri;
						text = ((!(uri != null)) ? connect.ToString() : Regex.Escape(uri.GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped)));
						if (connect is string)
						{
							text = Regex.Escape(text);
						}
						SecurityElement securityElement4 = new SecurityElement("URI");
						securityElement4.AddAttribute("uri", SecurityElement.Escape(text));
						securityElement2.AddChild(securityElement4);
					}
				}
				securityElement.AddChild(securityElement2);
			}
			if (m_UnrestrictedAccept || m_acceptList.Count > 0)
			{
				SecurityElement securityElement5 = new SecurityElement("AcceptAccess");
				if (m_UnrestrictedAccept)
				{
					SecurityElement securityElement6 = new SecurityElement("URI");
					securityElement6.AddAttribute("uri", SecurityElement.Escape(".*"));
					securityElement5.AddChild(securityElement6);
				}
				else
				{
					foreach (object accept in m_acceptList)
					{
						Uri uri2 = accept as Uri;
						text = ((!(uri2 != null)) ? accept.ToString() : Regex.Escape(uri2.GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped)));
						if (accept is string)
						{
							text = Regex.Escape(text);
						}
						SecurityElement securityElement7 = new SecurityElement("URI");
						securityElement7.AddAttribute("uri", SecurityElement.Escape(text));
						securityElement5.AddChild(securityElement7);
					}
				}
				securityElement.AddChild(securityElement5);
			}
		}
		else
		{
			securityElement.AddAttribute("Unrestricted", "true");
		}
		return securityElement;
	}

	private static bool isMatchedURI(object uriToCheck, ArrayList uriPatternList)
	{
		string text = uriToCheck as string;
		foreach (object uriPattern in uriPatternList)
		{
			if (!(uriPattern is DelayedRegex delayedRegex))
			{
				if (uriToCheck.GetType() == uriPattern.GetType())
				{
					if (text != null && string.Compare(text, (string)uriPattern, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return true;
					}
					if (text == null && uriToCheck.Equals(uriPattern))
					{
						return true;
					}
				}
				continue;
			}
			string text2 = ((text != null) ? text : ((Uri)uriToCheck).GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped));
			Match match = delayedRegex.AsRegex.Match(text2);
			if (match != null && match.Index == 0 && match.Length == text2.Length)
			{
				return true;
			}
			if (text == null)
			{
				text2 = ((Uri)uriToCheck).GetComponents(UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped);
				match = delayedRegex.AsRegex.Match(text2);
				if (match != null && match.Index == 0 && match.Length == text2.Length)
				{
					return true;
				}
			}
		}
		return false;
	}

	private static void intersectList(ArrayList A, ArrayList B, ArrayList result)
	{
		bool[] array = new bool[A.Count];
		bool[] array2 = new bool[B.Count];
		int num = 0;
		foreach (object item in A)
		{
			int num2 = 0;
			foreach (object item2 in B)
			{
				if (!array2[num2] && item.GetType() == item2.GetType())
				{
					if (item is Uri)
					{
						if (item.Equals(item2))
						{
							result.Add(item);
							array[num] = (array2[num2] = true);
							break;
						}
					}
					else if (string.Compare(item.ToString(), item2.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
					{
						result.Add(item);
						array[num] = (array2[num2] = true);
						break;
					}
				}
				num2++;
			}
			num++;
		}
		num = 0;
		foreach (object item3 in A)
		{
			if (!array[num])
			{
				int num2 = 0;
				foreach (object item4 in B)
				{
					if (!array2[num2])
					{
						bool isUri;
						object obj = intersectPair(item3, item4, out isUri);
						if (obj != null)
						{
							bool flag = false;
							foreach (object item5 in result)
							{
								if (isUri == item5 is Uri && (isUri ? obj.Equals(item5) : (string.Compare(item5.ToString(), obj.ToString(), StringComparison.OrdinalIgnoreCase) == 0)))
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								result.Add(obj);
							}
						}
					}
					num2++;
				}
			}
			num++;
		}
	}

	private static object intersectPair(object L, object R, out bool isUri)
	{
		isUri = false;
		DelayedRegex delayedRegex = L as DelayedRegex;
		DelayedRegex delayedRegex2 = R as DelayedRegex;
		if (delayedRegex != null && delayedRegex2 != null)
		{
			return new DelayedRegex("(?=(" + delayedRegex.ToString() + "))(" + delayedRegex2.ToString() + ")");
		}
		if (delayedRegex != null && delayedRegex2 == null)
		{
			isUri = R is Uri;
			string text = (isUri ? ((Uri)R).GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped) : R.ToString());
			Match match = delayedRegex.AsRegex.Match(text);
			if (match != null && match.Index == 0 && match.Length == text.Length)
			{
				return R;
			}
			return null;
		}
		if (delayedRegex == null && delayedRegex2 != null)
		{
			isUri = L is Uri;
			string text2 = (isUri ? ((Uri)L).GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped) : L.ToString());
			Match match2 = delayedRegex2.AsRegex.Match(text2);
			if (match2 != null && match2.Index == 0 && match2.Length == text2.Length)
			{
				return L;
			}
			return null;
		}
		isUri = L is Uri;
		if (isUri)
		{
			if (!L.Equals(R))
			{
				return null;
			}
			return L;
		}
		if (string.Compare(L.ToString(), R.ToString(), StringComparison.OrdinalIgnoreCase) != 0)
		{
			return null;
		}
		return L;
	}
}
