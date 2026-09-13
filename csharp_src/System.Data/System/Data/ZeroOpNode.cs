using System.Collections.Generic;

namespace System.Data;

internal sealed class ZeroOpNode : ExpressionNode
{
	internal readonly int _op;

	internal const int zop_True = 1;

	internal const int zop_False = 0;

	internal const int zop_Null = -1;

	internal ZeroOpNode(int op)
		: base(null)
	{
		_op = op;
	}

	internal override void Bind(DataTable table, List<DataColumn> list)
	{
	}

	internal override object Eval()
	{
		return _op switch
		{
			33 => true, 
			34 => false, 
			32 => DBNull.Value, 
			_ => DBNull.Value, 
		};
	}

	internal override object Eval(DataRow row, DataRowVersion version)
	{
		return Eval();
	}

	internal override object Eval(int[] recordNos)
	{
		return Eval();
	}

	internal override bool IsConstant()
	{
		return true;
	}

	internal override bool IsTableConstant()
	{
		return true;
	}

	internal override bool HasLocalAggregate()
	{
		return false;
	}

	internal override bool HasRemoteAggregate()
	{
		return false;
	}

	internal override ExpressionNode Optimize()
	{
		return this;
	}
}
