using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    public interface IDbSession : IDisposable
    {
        public void OpenTransaction();

        public void RollbackTransaction();

        public void CommitTransaction();

        public IDbCommand CreateCommand(string sql);

        public IDbQuery<T> CreateQuery<T>(string sql) where T : new();
    }
}
