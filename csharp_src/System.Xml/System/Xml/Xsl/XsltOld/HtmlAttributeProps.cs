using System.Collections;

namespace System.Xml.Xsl.XsltOld;

internal class HtmlAttributeProps
{
	private bool abr;

	private bool uri;

	private bool name;

	private static Hashtable s_table = CreatePropsTable();

	public bool Abr => abr;

	public bool Uri => uri;

	public bool Name => name;

	public static HtmlAttributeProps Create(bool abr, bool uri, bool name)
	{
		return new HtmlAttributeProps
		{
			abr = abr,
			uri = uri,
			name = name
		};
	}

	public static HtmlAttributeProps GetProps(string name)
	{
		return (HtmlAttributeProps)s_table[name];
	}

	private static Hashtable CreatePropsTable()
	{
		bool flag = false;
		bool flag2 = true;
		return new Hashtable(26, StringComparer.OrdinalIgnoreCase)
		{
			{
				"action",
				Create(flag, flag2, flag)
			},
			{
				"checked",
				Create(flag2, flag, flag)
			},
			{
				"cite",
				Create(flag, flag2, flag)
			},
			{
				"classid",
				Create(flag, flag2, flag)
			},
			{
				"codebase",
				Create(flag, flag2, flag)
			},
			{
				"compact",
				Create(flag2, flag, flag)
			},
			{
				"data",
				Create(flag, flag2, flag)
			},
			{
				"datasrc",
				Create(flag, flag2, flag)
			},
			{
				"declare",
				Create(flag2, flag, flag)
			},
			{
				"defer",
				Create(flag2, flag, flag)
			},
			{
				"disabled",
				Create(flag2, flag, flag)
			},
			{
				"for",
				Create(flag, flag2, flag)
			},
			{
				"href",
				Create(flag, flag2, flag)
			},
			{
				"ismap",
				Create(flag2, flag, flag)
			},
			{
				"longdesc",
				Create(flag, flag2, flag)
			},
			{
				"multiple",
				Create(flag2, flag, flag)
			},
			{
				"name",
				Create(flag, flag, flag2)
			},
			{
				"nohref",
				Create(flag2, flag, flag)
			},
			{
				"noresize",
				Create(flag2, flag, flag)
			},
			{
				"noshade",
				Create(flag2, flag, flag)
			},
			{
				"nowrap",
				Create(flag2, flag, flag)
			},
			{
				"profile",
				Create(flag, flag2, flag)
			},
			{
				"readonly",
				Create(flag2, flag, flag)
			},
			{
				"selected",
				Create(flag2, flag, flag)
			},
			{
				"src",
				Create(flag, flag2, flag)
			},
			{
				"usemap",
				Create(flag, flag2, flag)
			}
		};
	}
}
