using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADO.NET.Application.Common.Interfaces;

namespace ADO.NET.Infrastructure.Repository
{
    public class AdoDbContext : IDbContext
    {
        private readonly string connectionString;
        private IDbConnection connection;
        private IDbTransaction transaction;

        public AdoDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = new SqlConnection(connectionString);
                }
                return connection;
            }
        }

        public void OpenConnection()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
        }

        public IDbTransaction BeginTransaction()
        {
            transaction = Connection.BeginTransaction();
            return transaction;
        }
    }
}
