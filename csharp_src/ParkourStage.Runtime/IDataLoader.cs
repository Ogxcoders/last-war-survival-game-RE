using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParkourStage.Runtime.Data;

public interface IDataLoader : IDisposable
{
	bool Loaded { get; }

	string TableName { get; set; }

	string ParentPath { get; set; }

	int RowCount { get; }

	int ContainsId(string targetId);

	void LoadData();

	Task LoadDataAsync();

	string GetValue(int rowIndex, string columnName);

	bool InsertNewRowData(int rowIndex, in List<RowData> rowDataList);

	bool GetEmptyRowData(in List<RowData> rowDataList);

	bool ReadRowAllDataById(string targetId, in List<RowData> rowDataList, out int rowIndex);

	bool ReadRowAllDataByRowIndex(int rowIndex, in List<RowData> rowDataList);

	string GetRowDataDiffTip(int rowIndex, in List<RowData> rowDataList);

	bool WriteRowData(int rowIndex, in List<RowData> rowDataList);

	void RemoveRowAllDataByIndex(int rowIndex);

	bool SaveData();

	Task<bool> SaveDataAsync();

	bool IsFocus();

	void SelectRow(int rowIndex);

	bool IsSelectedRow(int rowIndex);
}
