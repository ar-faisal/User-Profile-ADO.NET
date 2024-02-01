using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET.Application.Common.Interfaces
{
    public interface IDbContext
    {
        IDbConnection Connection { get; }
        void OpenConnection();
        void CloseConnection();
        IDbTransaction BeginTransaction();
    }
}
