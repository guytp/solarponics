using System;
using System.Data;

namespace Solarponics.Data
{
    public sealed class StoredProcedureParameter : IDbDataParameter
    {
        public StoredProcedureParameter(string parameterName, object value)
        {
            this.ParameterName = parameterName;
            this.Value = value;

            if (value is Guid)
            {
                DbType = DbType.Guid;
            }
        }

        public DbType DbType { get; set; }

        public ParameterDirection Direction { get; set; }

        public bool IsNullable { get; set; }

        public string ParameterName { get; set; }

        public byte Precision { get; set; }

        public byte Scale { get; set; }

        public int Size { get; set; }

        public string SourceColumn { get; set; }

        public DataRowVersion SourceVersion { get; set; }

        public object Value { get; set; }
    }
}