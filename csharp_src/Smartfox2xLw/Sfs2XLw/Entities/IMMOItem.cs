using System.Collections.Generic;
using Sfs2XLw.Entities.Data;
using Sfs2XLw.Entities.Variables;

namespace Sfs2XLw.Entities;

public interface IMMOItem
{
	int Id { get; }

	Vec3D AOIEntryPoint { get; set; }

	List<IMMOItemVariable> GetVariables();

	IMMOItemVariable GetVariable(string name);

	void SetVariable(IMMOItemVariable variable);

	void SetVariables(List<IMMOItemVariable> variables);

	bool ContainsVariable(string name);
}
