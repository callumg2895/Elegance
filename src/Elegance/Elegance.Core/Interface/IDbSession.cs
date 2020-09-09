using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    public interface IDbSession : IDisposable
    {
        public void OpenTransaction(IsolationLevel? isolationLevel = null);

        public void RollbackTransaction();

        public void CommitTransaction();

        public IDbCommand CreateCommand(string sql);

        public IDbQuery<T> CreateObjectQuery<T>(string commandText, CommandType commandType) where T : new();

        public IDbQuery<T> CreateScalarQuery<T>(string commandText, CommandType commandType) where T : IConvertible;
    }
}
