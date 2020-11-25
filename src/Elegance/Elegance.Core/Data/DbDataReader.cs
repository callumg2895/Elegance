using Elegance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Data
{
    internal class DbDataReader : IDbDataReader
    {
        private readonly IDataReader _reader;
        private readonly HashSet<string> _allColumns;
        private readonly HashSet<string> _readColumns;

        private DbDataReader()
        {
            _allColumns = new HashSet<string>();
            _readColumns = new HashSet<string>();
        }

        internal DbDataReader(IDataReader reader)
            : this()
        {
            _reader = reader;

            UpdateColumns();
        }

        #region Basic IDataReader Implementation

        public int Depth => _reader.Depth;
        public bool IsClosed => _reader.IsClosed;
        public int RecordsAffected => _reader.RecordsAffected;
        public int FieldCount => _reader.FieldCount;
        public object this[string name] => _reader[name];
        public object this[int i] => _reader[i];
        public void Dispose() => _reader.Dispose();
        public void Close() => _reader.Close();
        public DataTable GetSchemaTable() => _reader.GetSchemaTable();

        public bool NextResult() 
        {
            var success = _reader.NextResult();

            if (success)
            {
                UpdateColumns();
            }

            return success;

        }

        public bool Read() 
        {
            _readColumns.Clear();

            return _reader.Read();
        }

        public bool GetBoolean(int i) => _reader.GetBoolean(i);
        public byte GetByte(int i) => _reader.GetByte(i);
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => _reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        public char GetChar(int i) => _reader.GetChar(i);
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => _reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        public IDataReader GetData(int i) => _reader.GetData(i);
        public string GetDataTypeName(int i) => _reader.GetDataTypeName(i);
        public DateTime GetDateTime(int i) => _reader.GetDateTime(i);
        public decimal GetDecimal(int i) => _reader.GetDecimal(i);
        public double GetDouble(int i) => _reader.GetDouble(i);
        public Type GetFieldType(int i) => _reader.GetFieldType(i);
        public float GetFloat(int i) => _reader.GetFloat(i);
        public Guid GetGuid(int i) => _reader.GetGuid(i);
        public short GetInt16(int i) => _reader.GetInt16(i);
        public int GetInt32(int i) => _reader.GetInt32(i);
        public long GetInt64(int i) => _reader.GetInt64(i);
        public string GetName(int i) => _reader.GetName(i);
        public int GetOrdinal(string name) => _reader.GetOrdinal(name);
        public string GetString(int i) => _reader.GetString(i);
        public object GetValue(int i) => _reader.GetValue(i);
        public int GetValues(object[] values) => _reader.GetValues(values);
        public bool IsDBNull(int i) => _reader.IsDBNull(i);

        #endregion

        public object GetValue(string alias)
        {
            return HasValue(alias) && _readColumns.Add(alias)
                ? _reader[alias]
                : null;
        }

        public bool HasValue(string alias)
        {
            return  !string.IsNullOrWhiteSpace(alias) 
                &&  _allColumns.Contains(alias) 
                &&  !_readColumns.Contains(alias);
        }

        private void UpdateColumns()
        {
            _allColumns.Clear();
            _readColumns.Clear();

            foreach (DataRow row in GetSchemaTable().Rows)
            {
                var columnName = row["ColumnName"].ToString();

                _allColumns.Add(columnName);
            }
        }

    }
}
